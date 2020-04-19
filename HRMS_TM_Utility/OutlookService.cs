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
		public ConcurrentBag<Mail> GetMails(Profile profile, DateTime dateFrom, DateTime dateTo)
		{
			ConcurrentBag<Mail> result = new ConcurrentBag<Mail>();
			Application outlook = new Application();
			NameSpace names = outlook.GetNamespace("MAPI");

			Store store = null;
			foreach (Store s in names.Stores)
			{
				if (s.DisplayName == profile.StoreName)
				{
					store = s;
					break;
				}
			}
			if (store == null)
			{
				return result;
			}

			var template = JsonConvert.DeserializeObject<MainTemplate>(File.ReadAllText("templates.json"));
			MAPIFolder inbox = store.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
			dateTo = dateTo.Date.AddDays(1).AddSeconds(-1);
			inbox.Items.Cast<object>().AsParallel().ForAll(item =>
			{
				if (!(item is MailItem mailItem))
					return;
				try
				{
					if (mailItem.ReceivedTime > dateTo)
						return;
				}
				catch (System.InvalidCastException)
				{
					return;
				}
				if (mailItem.ReceivedTime.Date < dateFrom.Date)
					return;

				if (parseWFHMail(mailItem, template.wfh) is Mail mail)
				{
					result.Add(mail);
				}
			});

			return result;
		}

		private Mail parseWFHMail(MailItem mailItem, WFHTemplate wfhTemplate)
		{
			StringBuilder sbRemarks = new StringBuilder();
			Mail result = null;
			foreach (var dateTemplate in wfhTemplate.date)
			{
				var isWfh = Regex.Match(mailItem.Subject, dateTemplate.search, RegexOptions.IgnoreCase);
				if (isWfh.Success)
				{
					result = new Mail
					{
						Email = GetSenderSMTPAddress(mailItem),
						Name = mailItem.SenderName,
					};

					var wfhDate = Regex.Match(isWfh.Value, dateTemplate.extract, RegexOptions.IgnoreCase);
					if (wfhDate.Success && DateTime.TryParse(wfhDate.Value, out var date))
					{
						result.Date = date;
					}
					else
					{
						result.Status = false;
						sbRemarks.AppendLine("Invalid date");
					}

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


					var timeIn = parseTime(wfhTemplate.timein);
					if (timeIn.status
						&& DateTime.TryParse(timeIn.value, out var dateTimeIn))
					{
						result.TimeIn = dateTimeIn.TimeOfDay;
					}
					else
					{
						result.Status = false;
						sbRemarks.AppendLine($"Invald time in value {timeIn.value}.");
					}

					var timeOut = parseTime(wfhTemplate.timeout);
					if (timeOut.status
						&& DateTime.TryParse(timeOut.value, out var dateTimeOut))
					{
						result.TimeOut = dateTimeOut.TimeOfDay;
					}
					else
					{
						result.Status = false;
						sbRemarks.AppendLine($"Invald time out value {timeOut.value}.");
					}

					var hours = parseTime(wfhTemplate.hours);
					if (hours.status
						&& decimal.TryParse(hours.value, out var totalHours))
					{
						result.Hours = totalHours;
					}
					else
					{
						result.Status = false;
						sbRemarks.AppendLine($"Invald hours value {hours.value}.");
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
		ConcurrentBag<Mail> GetMails(Profile profile, DateTime dateFrom, DateTime dateTo);
	}
}
