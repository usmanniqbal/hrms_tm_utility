using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMS_TM_Utility
{
	public partial class frmImport : Form
	{
		private readonly IOutlookService _outlookService;
		private readonly IExcelService _excelService;
		private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public frmImport()
		{
			InitializeComponent();
			_outlookService = new OutlookService(_log);
			_excelService = new ExcelService();
		}

		private void btnFetch_Click(object sender, EventArgs e)
		{
			var dataSource = _outlookService.GetMails((Profile)cboProfiles.SelectedItem, dtpFrom.Value, dtpTo.Value, txtEmpCode.Text)
				.OrderBy(o => o.Date)
				.ThenBy(o => o.Email)
				.ToList();
			dgv.DataSource = dataSource;
			SetGrid();
			FetchActionControl(dataSource.Count > 0);
		}

		private void FetchActionControl(bool enabled)
		{
			btnExport.Enabled =
			btnArchive.Enabled =
			btnReset.Enabled = enabled;

			btnFetch.Enabled =
			btnNew.Enabled =
			dtpFrom.Enabled =
			dtpTo.Enabled =
			txtEmpCode.Enabled = !enabled;
		}

		private void SetGrid()
		{
			Mail mail;
			dgv.AutoSize = true;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			dgv.Columns[nameof(mail.Email)].ReadOnly = true;
			dgv.Columns[nameof(mail.Name)].ReadOnly = true;
			dgv.Columns[nameof(mail.Date)].ReadOnly = true;
			dgv.Columns[nameof(mail.Remarks)].ReadOnly = true;

			dgv.Columns[nameof(mail.MailId)].Visible = false;
			dgv.Columns[nameof(mail.Status)].Visible = false;

			dgv.Columns[nameof(mail.Icon)].DefaultCellStyle.NullValue = null;
			dgv.Columns[nameof(mail.TimeIn)].DefaultCellStyle.Format = "hh:mm tt";
			dgv.Columns[nameof(mail.TimeOut)].DefaultCellStyle.Format = "hh:mm tt";
		}

		private void frmImport_Load(object sender, EventArgs e)
		{
			ProfileActionControl(false);
			LoadProfiles();
			dtpFrom.MaxDate = dtpTo.MaxDate = DateTime.Now.Date;
			dtpFrom.Value = DateTime.Now.AddDays(-7);
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			ProfileActionControl(true);
		}

		private void ProfileActionControl(bool enabled)
		{
			txtProfileName.Enabled =
			txtStoreName.Enabled =
			txtFolderName.Enabled =
			txtArcFolder.Enabled =
			btnSave.Enabled =
			btnCancel.Enabled = enabled;

			btnFetch.Enabled =
			cboProfiles.Enabled = !enabled;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			var profile = _outlookService.SaveProfile(new Profile
			{
				Id = Guid.NewGuid(),
				ProfileName = txtProfileName.Text,
				StoreName = txtStoreName.Text,
				Folder = txtFolderName.Text,
				ArchiveFolder = txtArcFolder.Text
			});
			ProfileActionControl(false);
			LoadProfiles();
			cboProfiles.SelectedValue = profile.Id;
		}

		private void LoadProfiles()
		{
			var profiles = _outlookService.GetProfiles();
			cboProfiles.DataSource = profiles;
			cboProfiles.DisplayMember = "ProfileName";
			cboProfiles.ValueMember = "Id";
		}

		private void cboProfiles_SelectedValueChanged(object sender, EventArgs e)
		{
			var profile = (Profile)cboProfiles.SelectedItem;
			txtProfileName.Text = profile?.ProfileName;
			txtStoreName.Text = profile?.StoreName;
			txtFolderName.Text = profile?.Folder;
			txtArcFolder.Text = profile?.ArchiveFolder;
			btnFetch.Enabled = profile != null;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			ProfileActionControl(false);
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.FileName = $"{dtpFrom.Value.ToString("yyyyMMdd")}-{dtpTo.Value.ToString("yyyyMMdd")}.xlsx";
				saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx";
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					Export(saveFileDialog.FileName);
				}
			}
		}

		private void Export(string path)
		{
			List<ExcelEntity> excelEntities = new List<ExcelEntity>();
			ExcelEntity excelEntity;
			foreach (DataGridViewRow row in dgv.Rows)
			{
				excelEntity = new ExcelEntity
				{
					Name = Convert.ToString(row.Cells[nameof(excelEntity.Name)].Value),
					Email = Convert.ToString(row.Cells[nameof(excelEntity.Email)].Value),
					EmployeeCode = Convert.ToString(row.Cells[nameof(excelEntity.EmployeeCode)].Value),
					Date = (DateTime)row.Cells[nameof(excelEntity.Date)].Value,
					TimeIn = (DateTime)row.Cells[nameof(excelEntity.TimeIn)].Value,
					TimeOut = (DateTime)row.Cells[nameof(excelEntity.TimeOut)].Value,
					Hours = (decimal)row.Cells[nameof(excelEntity.Hours)].Value,
					Remarks = Convert.ToString(row.Cells[nameof(excelEntity.Remarks)].Value)
				};
				excelEntities.Add(excelEntity);
			}
			_excelService.Export(path, excelEntities);
			MessageBox.Show("Emails exported successfully");
			FetchActionControl(false);
			dgv.DataSource = null;
		}

		private void btnArchive_Click(object sender, EventArgs e)
		{
			var profile = (Profile)cboProfiles.SelectedItem;
			var mails = (List<Mail>)dgv.DataSource;
			var result = _outlookService.ArchiveMails(profile, mails);

			if (string.IsNullOrWhiteSpace(result.error))
			{
				dgv.DataSource = null;
				dgv.DataSource = result.mails;
				SetGrid();
				FetchActionControl(false);
				dgv.DataSource = null;
				MessageBox.Show("Emails archived successfully");
			}
			else
			{
				MessageBox.Show(result.error);
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			FetchActionControl(false);
			dgv.DataSource = null;
		}
	}
}
