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
		public frmImport()
		{
			InitializeComponent();
			_outlookService = new OutlookService();
			_excelService = new ExcelService();
		}

		private void btnFetch_Click(object sender, EventArgs e)
		{
			Mail mail;
			var dataSource = _outlookService.GetMails((Profile)cboProfiles.SelectedItem, dtpFrom.Value, dtpTo.Value)
				.OrderBy(o => o.Date)
				.ThenBy(o => o.Email)
				.ToList();
			dgv.DataSource = dataSource;
			dgv.Columns[nameof(mail.Email)].ReadOnly = true;
			dgv.Columns[nameof(mail.Name)].ReadOnly = true;
			dgv.Columns[nameof(mail.Status)].Visible = false;
			btnExport.Enabled = dataSource.Count > 0;
		}

		private void frmImport_Load(object sender, EventArgs e)
		{
			ActionControl(false);
			LoadProfiles();
			dtpFrom.MaxDate = dtpTo.MaxDate = DateTime.Now.Date;
			dtpFrom.Value = DateTime.Now.AddDays(-7);
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			ActionControl(true);
		}

		private void ActionControl(bool enabled)
		{
			txtProfileName.Enabled =
			txtStoreName.Enabled =
			txtFolderName.Enabled =
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
			});
			ActionControl(false);
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
			txtProfileName.Text = profile.ProfileName;
			txtStoreName.Text = profile.StoreName;
			txtFolderName.Text = profile.Folder;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			ActionControl(false);
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			string path;
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.FileName = $"{dtpFrom.Value.ToString("yyyyMMdd")}-{dtpTo.Value.ToString("yyyyMMdd")}.xlsx";
				saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx";
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					path = saveFileDialog.FileName;
				}
				else
				{
					return;
				}
			}
			List<ExcelEntity> excelEntities = new List<ExcelEntity>();
			ExcelEntity excelEntity;
			foreach (DataGridViewRow row in dgv.Rows)
			{
				excelEntity = new ExcelEntity
				{
					Name = row.Cells[nameof(excelEntity.Name)].Value.ToString(),
					Email = row.Cells[nameof(excelEntity.Email)].Value.ToString(),
					Date = (DateTime)row.Cells[nameof(excelEntity.Date)].Value,
					TimeIn = new DateTime(((TimeSpan)row.Cells[nameof(excelEntity.TimeIn)].Value).Ticks),
					TimeOut = new DateTime(((TimeSpan)row.Cells[nameof(excelEntity.TimeOut)].Value).Ticks),
					Hours = (decimal)row.Cells[nameof(excelEntity.Hours)].Value,
				};
				excelEntities.Add(excelEntity);
			}
			_excelService.Export(path, excelEntities);
		}
	}
}
