namespace HRMS_TM_Utility
{
    partial class frmImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnFetch = new System.Windows.Forms.Button();
			this.txtStoreName = new System.Windows.Forms.TextBox();
			this.txtFolderName = new System.Windows.Forms.TextBox();
			this.btnNew = new System.Windows.Forms.Button();
			this.cboProfiles = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.txtProfileName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.dtpFrom = new System.Windows.Forms.DateTimePicker();
			this.dtpTo = new System.Windows.Forms.DateTimePicker();
			this.label7 = new System.Windows.Forms.Label();
			this.btnExport = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
			this.SuspendLayout();
			// 
			// btnFetch
			// 
			this.btnFetch.Location = new System.Drawing.Point(355, 5);
			this.btnFetch.Margin = new System.Windows.Forms.Padding(2);
			this.btnFetch.Name = "btnFetch";
			this.btnFetch.Size = new System.Drawing.Size(48, 21);
			this.btnFetch.TabIndex = 0;
			this.btnFetch.Text = "Fetch";
			this.btnFetch.UseVisualStyleBackColor = true;
			this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
			// 
			// txtStoreName
			// 
			this.txtStoreName.Location = new System.Drawing.Point(82, 72);
			this.txtStoreName.Margin = new System.Windows.Forms.Padding(2);
			this.txtStoreName.Name = "txtStoreName";
			this.txtStoreName.Size = new System.Drawing.Size(322, 20);
			this.txtStoreName.TabIndex = 1;
			// 
			// txtFolderName
			// 
			this.txtFolderName.Location = new System.Drawing.Point(82, 94);
			this.txtFolderName.Margin = new System.Windows.Forms.Padding(2);
			this.txtFolderName.Name = "txtFolderName";
			this.txtFolderName.Size = new System.Drawing.Size(322, 20);
			this.txtFolderName.TabIndex = 2;
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(82, 28);
			this.btnNew.Margin = new System.Windows.Forms.Padding(2);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(48, 21);
			this.btnNew.TabIndex = 5;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// cboProfiles
			// 
			this.cboProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboProfiles.FormattingEnabled = true;
			this.cboProfiles.Location = new System.Drawing.Point(82, 5);
			this.cboProfiles.Margin = new System.Windows.Forms.Padding(2);
			this.cboProfiles.Name = "cboProfiles";
			this.cboProfiles.Size = new System.Drawing.Size(270, 21);
			this.cboProfiles.TabIndex = 6;
			this.cboProfiles.SelectedValueChanged += new System.EventHandler(this.cboProfiles_SelectedValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 7);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Select Profile";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(2, 76);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Account Name";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 97);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Folder Name";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(14, 115);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Date Range";
			// 
			// dgv
			// 
			this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Location = new System.Drawing.Point(9, 139);
			this.dgv.Margin = new System.Windows.Forms.Padding(2);
			this.dgv.Name = "dgv";
			this.dgv.RowHeadersWidth = 51;
			this.dgv.RowTemplate.Height = 24;
			this.dgv.Size = new System.Drawing.Size(583, 310);
			this.dgv.TabIndex = 12;
			// 
			// txtProfileName
			// 
			this.txtProfileName.Location = new System.Drawing.Point(82, 50);
			this.txtProfileName.Margin = new System.Windows.Forms.Padding(2);
			this.txtProfileName.Name = "txtProfileName";
			this.txtProfileName.Size = new System.Drawing.Size(322, 20);
			this.txtProfileName.TabIndex = 1;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(10, 54);
			this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Profile Name";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(134, 28);
			this.btnSave.Margin = new System.Windows.Forms.Padding(2);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(48, 21);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(187, 28);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(48, 21);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// dtpFrom
			// 
			this.dtpFrom.Checked = false;
			this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpFrom.Location = new System.Drawing.Point(82, 115);
			this.dtpFrom.Margin = new System.Windows.Forms.Padding(2);
			this.dtpFrom.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dtpFrom.Name = "dtpFrom";
			this.dtpFrom.Size = new System.Drawing.Size(90, 20);
			this.dtpFrom.TabIndex = 14;
			this.dtpFrom.Value = new System.DateTime(2029, 12, 25, 23, 59, 59, 0);
			// 
			// dtpTo
			// 
			this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpTo.Location = new System.Drawing.Point(198, 115);
			this.dtpTo.Margin = new System.Windows.Forms.Padding(2);
			this.dtpTo.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dtpTo.Name = "dtpTo";
			this.dtpTo.Size = new System.Drawing.Size(90, 20);
			this.dtpTo.TabIndex = 14;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(175, 119);
			this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(20, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "To";
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Enabled = false;
			this.btnExport.Location = new System.Drawing.Point(544, 116);
			this.btnExport.Margin = new System.Windows.Forms.Padding(2);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(48, 21);
			this.btnExport.TabIndex = 16;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// frmImport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(601, 459);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.dtpTo);
			this.Controls.Add(this.dtpFrom);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.dgv);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboProfiles);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.txtProfileName);
			this.Controls.Add(this.txtFolderName);
			this.Controls.Add(this.txtStoreName);
			this.Controls.Add(this.btnFetch);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "frmImport";
			this.Text = "Import";
			this.Load += new System.EventHandler(this.frmImport_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.TextBox txtStoreName;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ComboBox cboProfiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExport;
    }
}

