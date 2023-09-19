namespace  GroupM.Minder
{
    partial class MPA008_TVCalendar
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gvClient = new System.Windows.Forms.DataGridView();
            this.BuyingBriefID = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkCheckAll = new System.Windows.Forms.Button();
            this.chkUnCheckAll = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.rdThai = new System.Windows.Forms.RadioButton();
            this.rdEng = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.chkHideProduct = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(842, 667);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(135, 23);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate Excel";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gvClient
            // 
            this.gvClient.AllowUserToAddRows = false;
            this.gvClient.AllowUserToDeleteRows = false;
            this.gvClient.AllowUserToResizeRows = false;
            this.gvClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvClient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BuyingBriefID,
            this.Column2,
            this.Column9,
            this.Column8,
            this.Column6,
            this.Column7,
            this.Column3,
            this.Column4,
            this.Client,
            this.Column1,
            this.Column5});
            this.gvClient.Location = new System.Drawing.Point(12, 38);
            this.gvClient.MultiSelect = false;
            this.gvClient.Name = "gvClient";
            this.gvClient.RowHeadersVisible = false;
            this.gvClient.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvClient.Size = new System.Drawing.Size(966, 620);
            this.gvClient.TabIndex = 12;
            // 
            // BuyingBriefID
            // 
            this.BuyingBriefID.HeaderText = "chk";
            this.BuyingBriefID.Name = "BuyingBriefID";
            this.BuyingBriefID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BuyingBriefID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.BuyingBriefID.Width = 30;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "BuyingBriefID";
            this.Column2.HeaderText = "BuyingBriefID";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "Status";
            this.Column9.HeaderText = "Status";
            this.Column9.Name = "Column9";
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "Version";
            this.Column8.HeaderText = "V";
            this.Column8.Name = "Column8";
            this.Column8.Width = 30;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "MediaSubType";
            this.Column6.HeaderText = "MT";
            this.Column6.Name = "Column6";
            this.Column6.Width = 30;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "Target";
            this.Column7.HeaderText = "Target";
            this.Column7.Name = "Column7";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "ProductID";
            this.Column3.HeaderText = "ProductID";
            this.Column3.Name = "Column3";
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ProductName";
            this.Column4.HeaderText = "Product";
            this.Column4.Name = "Column4";
            this.Column4.Width = 150;
            // 
            // Client
            // 
            this.Client.DataPropertyName = "ClientName";
            this.Client.FillWeight = 300F;
            this.Client.HeaderText = "Client";
            this.Client.Name = "Client";
            this.Client.ReadOnly = true;
            this.Client.Width = 150;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "StartDate";
            this.Column1.HeaderText = "StartDate";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "EndDate";
            this.Column5.HeaderText = "EndDate";
            this.Column5.Name = "Column5";
            this.Column5.Width = 60;
            // 
            // cboMonth
            // 
            this.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            "JAN",
            "FEB",
            "MAR",
            "APR",
            "MAY",
            "JUN",
            "JUL",
            "AUG",
            "SEP",
            "OCT",
            "NOV",
            "DEC"});
            this.cboMonth.Location = new System.Drawing.Point(82, 11);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(60, 21);
            this.cboMonth.TabIndex = 13;
            this.cboMonth.TextChanged += new System.EventHandler(this.cboMonth_TextChanged);
            // 
            // cboYear
            // 
            this.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Items.AddRange(new object[] {
            "2004",
            "2005",
            "2006",
            "2007",
            "2008",
            "2009",
            "2010",
            "2011",
            "2012",
            "2013",
            "2014",
            "2015",
            "2016",
            "2017",
            "2018",
            "2019",
            "2020",
            "2021",
            "2022",
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030",
            "2031",
            "2032",
            "2033",
            "2034",
            "2035",
            "2036",
            "2037",
            "2038",
            "2039",
            "2040"});
            this.cboYear.Location = new System.Drawing.Point(148, 11);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(85, 21);
            this.cboYear.TabIndex = 13;
            this.cboYear.TextChanged += new System.EventHandler(this.cboYear_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Month/Year";
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCheckAll.Location = new System.Drawing.Point(12, 667);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Size = new System.Drawing.Size(73, 23);
            this.chkCheckAll.TabIndex = 15;
            this.chkCheckAll.Text = "Check All";
            this.chkCheckAll.UseVisualStyleBackColor = true;
            this.chkCheckAll.Click += new System.EventHandler(this.chkCheckAll_Click);
            // 
            // chkUnCheckAll
            // 
            this.chkUnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkUnCheckAll.Location = new System.Drawing.Point(93, 667);
            this.chkUnCheckAll.Name = "chkUnCheckAll";
            this.chkUnCheckAll.Size = new System.Drawing.Size(73, 23);
            this.chkUnCheckAll.TabIndex = 15;
            this.chkUnCheckAll.Text = "Uncheck All";
            this.chkUnCheckAll.UseVisualStyleBackColor = true;
            this.chkUnCheckAll.Click += new System.EventHandler(this.chkUnCheckAll_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "TV Calendar";
            this.saveFileDialog.Filter = "Excel Worksheets|*.xlsx";
            // 
            // rdThai
            // 
            this.rdThai.AutoSize = true;
            this.rdThai.Checked = true;
            this.rdThai.Location = new System.Drawing.Point(546, 13);
            this.rdThai.Name = "rdThai";
            this.rdThai.Size = new System.Drawing.Size(45, 17);
            this.rdThai.TabIndex = 16;
            this.rdThai.TabStop = true;
            this.rdThai.Text = "ไทย";
            this.rdThai.UseVisualStyleBackColor = true;
            // 
            // rdEng
            // 
            this.rdEng.AutoSize = true;
            this.rdEng.Location = new System.Drawing.Point(597, 13);
            this.rdEng.Name = "rdEng";
            this.rdEng.Size = new System.Drawing.Size(59, 17);
            this.rdEng.TabIndex = 16;
            this.rdEng.Text = "English";
            this.rdEng.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(467, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Program Name";
            // 
            // chkHideProduct
            // 
            this.chkHideProduct.AutoSize = true;
            this.chkHideProduct.Checked = true;
            this.chkHideProduct.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideProduct.Location = new System.Drawing.Point(671, 13);
            this.chkHideProduct.Name = "chkHideProduct";
            this.chkHideProduct.Size = new System.Drawing.Size(91, 17);
            this.chkHideProduct.TabIndex = 17;
            this.chkHideProduct.Text = "Hide Product ";
            this.chkHideProduct.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Status";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Lasted",
            "Draft"});
            this.cboStatus.Location = new System.Drawing.Point(300, 10);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(121, 21);
            this.cboStatus.TabIndex = 18;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            // 
            // MPA008_TVCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 699);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.chkHideProduct);
            this.Controls.Add(this.rdEng);
            this.Controls.Add(this.rdThai);
            this.Controls.Add(this.chkUnCheckAll);
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.cboMonth);
            this.Controls.Add(this.gvClient);
            this.Controls.Add(this.btnGenerate);
            this.Name = "MPA008_TVCalendar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MPA008 - TVCalendar";
            this.Load += new System.EventHandler(this.MPA008_TVCalendar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DataGridView gvClient;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button chkCheckAll;
        private System.Windows.Forms.Button chkUnCheckAll;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.RadioButton rdThai;
        private System.Windows.Forms.RadioButton rdEng;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkHideProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BuyingBriefID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}