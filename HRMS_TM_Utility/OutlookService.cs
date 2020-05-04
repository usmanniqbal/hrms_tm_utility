using log4net;
using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS_TM_Utility
{
	public class OutlookService : IOutlookService
	{
		private readonly ILog _log;
		public OutlookService(ILog log)
		{
			_log = log;
		}
		private Mail parseWFHMail(MailItem mailItem, WFHTemplate wfhTemplate, string empCode)
		{
			StringBuilder sbRemarks = new StringBuilder("WFH");
			Mail result = null;

			Func<SearchTemplate[], (string value, bool status)> parseTime = (searchTemplates) =>
			{
				foreach (var searchTemplate in searchTemplates)
				{
					var searchMatch = Regex.Match(mailItem.Body, searchTemplate.search, RegexOptions.IgnoreCase);
					if (searchMatch.Success)
					{
						var extractMatch = Regex.Match(searchMatch.Value, searchTemplate.extract, RegexOptions.IgnoreCase);
						if (extractMatch.Success)
						{
							return (extractMatch.Value, true);
						}
						else
						{
							return (string.Empty, false);
						}
					}
				}

				return (string.Empty, false);
			};

			foreach (var dateTemplate in wfhTemplate.date)
			{
				var isWfh = Regex.Match(mailItem.Subject, dateTemplate.search, RegexOptions.IgnoreCase);
				if (isWfh.Success)
				{
					var empCodeParsed = parseTime(wfhTemplate.empcode);

					if (string.IsNullOrWhiteSpace(empCode)
						|| (empCodeParsed.status && empCodeParsed.value == empCode))
					{
						result = new Mail
						{
							MailId = mailItem.EntryID,
							Email = GetSenderSMTPAddress(mailItem),
							Name = mailItem.SenderName,
							ReceivedTime = mailItem.ReceivedTime,
							Status = true
						};
					}
					else
					{
						continue;
					}

					if (empCodeParsed.status)
					{
						result.EmployeeCode = empCodeParsed.value;
					}
					else
					{
						result.Status = false;
						sbRemarks.Append($", Invald employee code value {empCodeParsed.value}");
					}

					var wfhDateParsed = Regex.Match(isWfh.Value, dateTemplate.extract, RegexOptions.IgnoreCase);
					if (wfhDateParsed.Success && DateTime.TryParse(wfhDateParsed.Value, out var date))
					{
						result.Date = date;
					}
					else
					{
						result.Status = false;
						sbRemarks.Append(", Invalid date");
					}

					var timeInParsed = parseTime(wfhTemplate.timein);
					if (timeInParsed.status
						&& DateTime.TryParse(timeInParsed.value, out var dateTimeIn))
					{
						result.TimeIn = dateTimeIn.TimeOfDay;
					}
					else
					{
						result.Status = false;
						sbRemarks.Append($", Invald time in value {timeInParsed.value}");
					}

					var timeOutParsed = parseTime(wfhTemplate.timeout);
					if (timeOutParsed.status
						&& DateTime.TryParse(timeOutParsed.value, out var dateTimeOut))
					{
						result.TimeOut = dateTimeOut.TimeOfDay;
					}
					else
					{
						result.Status = false;
						sbRemarks.Append($", Invald time out value {timeOutParsed.value}");
					}

					var hoursParsed = parseTime(wfhTemplate.hours);
					if (hoursParsed.status
						&& decimal.TryParse(hoursParsed.value, out var totalHours))
					{
						result.Hours = totalHours;
					}
					else
					{
						result.Status = false;
						sbRemarks.Append($", Invald hours value {hoursParsed.value}");
					}

					result.Remarks = sbRemarks.ToString();
					break;
				}
			}

			return result;
		}

		private string GetSenderSMTPAddress(MailItem mail)
		{
			string PR_SMTP_ADDRESS =
				@"http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
			if (mail == null)
			{
				throw new ArgumentNullException();
			}
			if (mail.SenderEmailType == "EX")
			{
				AddressEntry sender =
					mail.Sender;
				if (sender != null)
				{
					//Now we have an AddressEntry representing the Sender
					if (sender.AddressEntryUserType ==
						OlAddressEntryUserType.
						olExchangeUserAddressEntry
						|| sender.AddressEntryUserType ==
						OlAddressEntryUserType.
						olExchangeRemoteUserAddressEntry)
					{
						//Use the ExchangeUser object PrimarySMTPAddress
						ExchangeUser exchUser =
							sender.GetExchangeUser();
						if (exchUser != null)
						{
							return exchUser.PrimarySmtpAddress;
						}
						else
						{
							return null;
						}
					}
					else
					{
						return sender.PropertyAccessor.GetProperty(
							PR_SMTP_ADDRESS) as string;
					}
				}
				else
				{
					return null;
				}
			}
			else
			{
				return mail.SenderEmailAddress;
			}
		}

		public IEnumerable<Mail> GetMails(Profile profile, DateTime dateFrom, DateTime dateTo, string empCode)
		{
			if (profile == null)
			{
				throw new ApplicationException("Please create profile.");
			}
			_log.Info($"GetMails: between {dateFrom.Date} - {dateTo.Date}");
			ConcurrentDictionary<(string, DateTime), Mail> result = new ConcurrentDictionary<(string, DateTime), Mail>();
			Application outlook = new Application();
			NameSpace names = outlook.GetNamespace("MAPI");

			Store store = null;
			_log.Info("GetMails: Reading Accounts");
			foreach (Store s in names.Stores)
			{
				_log.Info($"GetMails: {s.DisplayName} account processing");
				if (s.DisplayName == profile.StoreName)
				{
					_log.Info($"GetMails: {s.DisplayName} account matched");
					store = s;
					break;
				}
			}
			if (store == null)
			{
				return result.Values;
			}

			_log.Info($"GetMails: reading templates.json");
			var template = JsonConvert.DeserializeObject<MainTemplate>(File.ReadAllText("templates.json"));
			_log.Info($"GetMails: fetching Inbox folder");
			MAPIFolder folder = store.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
			if (!string.IsNullOrEmpty(profile.Folder))
			{
				_log.Info($"GetMails: fetching {profile.Folder} folder");
				folder = folder.Folders.Cast<Folder>().First(o => o.Name == profile.Folder);
			}

			dateTo = dateTo.Date.AddDays(1).AddSeconds(-1);
			_log.Info($"GetMails: Starting loop on all emails in {folder.Name}");
			folder.Items.Cast<object>().AsParallel().ForAll(item =>
			{
				if (!(item is MailItem mailItem))
					return;
				try
				{
					_log.Info($"GetMails: Mail entry in: {folder.Name} received on: {mailItem.ReceivedTime.ToLongDateString()} from: {mailItem.SenderName} with subject: {mailItem.Subject}");
					if (mailItem.ReceivedTime > dateTo)
						return;
				}
				catch (System.InvalidCastException)
				{
					_log.Info($"GetMails: Invalid mail entry in {folder.Name}");
					return;
				}
				if (mailItem.ReceivedTime.Date < dateFrom.Date)
					return;

				if (parseWFHMail(mailItem, template.wfh, empCode) is Mail mail)
				{
					if (!result.TryGetValue((mail.Email, mail.Date), out var existingMail)
						|| existingMail.ReceivedTime < mail.ReceivedTime)
					{
						if (existingMail == null || result.TryRemove((existingMail.Email, existingMail.Date.Date), out existingMail))
						{
							result.TryAdd((mail.Email, mail.Date.Date), mail);
						}
					}
				}
			});

			return result.Values;
		}

		public (List<Mail> mails, string error) ArchiveMails(Profile profile, List<Mail> mails)
		{
			if (profile == null)
			{
				throw new ApplicationException("Please create profile.");
			}
			_log.Info($"ArchiveMails: started.");
			Application outlook = new Application();
			NameSpace names = outlook.GetNamespace("MAPI");

			Store store = null;
			_log.Info("ArchiveMails: Reading Accounts");
			foreach (Store s in names.Stores)
			{
				_log.Info($"ArchiveMails: {s.DisplayName} account processing");
				if (s.DisplayName == profile.StoreName)
				{
					_log.Info($"ArchiveMails: {s.DisplayName} account matched");
					store = s;
					break;
				}
			}
			if (store == null)
			{
				return (mails, "Account name does not exist.");
			}

			MAPIFolder destFolder = store.GetDefaultFolder(OlDefaultFolders.olFolderInbox)
				.Folders.Cast<Folder>().First(o => o.Name == profile.Folder)
				.Folders.Cast<Folder>().First(o => o.Name == profile.ArchiveFolder);

			mails.AsParallel().ForAll(mail =>
			{
				var item = names.GetItemFromID(mail.MailId, store.StoreID);
				if(!(item is MailItem mailItem))
				{
					mail.Status = false;
					mail.Remarks = $"Invalid mail item.";
					return;
				}

				mailItem.Move(destFolder);
				mail.Remarks = $"Archived";
				mail.Status = true;
			});

			return (mails, null);
		}

		public List<Profile> GetProfiles()
		{
			List<Profile> result;

			if (File.Exists("profiles.json"))
			{
				var json = File.ReadAllText("profiles.json");
				result = JsonConvert.DeserializeObject<List<Profile>>(json);
			}
			else
			{
				result = new List<Profile>();
			}
			return result;
		}

		public Profile SaveProfile(Profile profile)
		{
			var savedProfiles = GetProfiles();

			savedProfiles.Add(profile);
			var json = JsonConvert.SerializeObject(savedProfiles);
			File.WriteAllText("profiles.json", json);

			return profile;
		}
	}

	public interface IOutlookService
	{
		Profile SaveProfile(Profile profile);
		List<Profile> GetProfiles();
		IEnumerable<Mail> GetMails(Profile profile, DateTime dateFrom, DateTime dateTo, string empCode);
		(List<Mail> mails, string error) ArchiveMails(Profile profile, List<Mail> mails);
	}
}
