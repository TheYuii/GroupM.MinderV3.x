namespace  GroupM.Minder
{
    partial class MPA006_ExportReport
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("TV Schedul Supplier Report");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Monthly", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Tracking GRP Report");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Weekly", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPA006_ExportReport));
            this.tvReportName = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUnCheckAll = new System.Windows.Forms.Button();
            this.gvBuyingBrief = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdPeriodDate = new System.Windows.Forms.RadioButton();
            this.rdPeriodMonth = new System.Windows.Forms.RadioButton();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lbDateTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.cboMediaSubType = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvClient = new System.Windows.Forms.DataGridView();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.picThumbnail = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvBuyingBrief)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // tvReportName
            // 
            this.tvReportName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvReportName.FullRowSelect = true;
            this.tvReportName.Location = new System.Drawing.Point(7, 19);
            this.tvReportName.Name = "tvReportName";
            treeNode1.Name = "TVSchedulSupplierReport";
            treeNode1.Text = "TV Schedul Supplier Report";
            treeNode2.Name = "Monthly";
            treeNode2.Text = "Monthly";
            treeNode3.Checked = true;
            treeNode3.Name = "TrackingGRPReport";
            treeNode3.Text = "Tracking GRP Report";
            treeNode4.Name = "Weekly";
            treeNode4.Text = "Weekly";
            this.tvReportName.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode4});
            this.tvReportName.Size = new System.Drawing.Size(258, 662);
            this.tvReportName.TabIndex = 1;
            this.tvReportName.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvReportName_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.tvReportName);
            this.groupBox1.Location = new System.Drawing.Point(6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 687);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Report Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.btnExport);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.cboMediaSubType);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.picThumbnail);
            this.groupBox2.Location = new System.Drawing.Point(286, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(717, 687);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Condition";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox6.Controls.Add(this.btnExportCSV);
            this.groupBox6.Controls.Add(this.txtTargetName);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.btnImport);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Location = new System.Drawing.Point(6, 248);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(399, 124);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Import && Export AGB File";
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.BackColor = System.Drawing.SystemColors.Control;
            this.btnExportCSV.Enabled = false;
            this.btnExportCSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCSV.Location = new System.Drawing.Point(69, 83);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(322, 30);
            this.btnExportCSV.TabIndex = 15;
            this.btnExportCSV.Text = "Expot Excel";
            this.btnExportCSV.UseVisualStyleBackColor = false;
            this.btnExportCSV.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTargetName
            // 
            this.txtTargetName.Enabled = false;
            this.txtTargetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTargetName.Location = new System.Drawing.Point(191, 54);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(199, 23);
            this.txtTargetName.TabIndex = 19;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Step 3.";
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.SystemColors.Control;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(68, 18);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(322, 30);
            this.btnImport.TabIndex = 14;
            this.btnImport.Text = "Import AGB File";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 18;
            this.label4.Text = "Step 1.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "Step 2. Input Target Name";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.btnCheckAll);
            this.groupBox5.Controls.Add(this.btnUnCheckAll);
            this.groupBox5.Controls.Add(this.gvBuyingBrief);
            this.groupBox5.Location = new System.Drawing.Point(411, 207);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 165);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Buying Brief";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Image = global::GroupM.Minder.Properties.Resources.ok;
            this.btnCheckAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckAll.Location = new System.Drawing.Point(122, 13);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(80, 23);
            this.btnCheckAll.TabIndex = 11;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUnCheckAll
            // 
            this.btnUnCheckAll.Image = global::GroupM.Minder.Properties.Resources.delete1;
            this.btnUnCheckAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUnCheckAll.Location = new System.Drawing.Point(206, 13);
            this.btnUnCheckAll.Name = "btnUnCheckAll";
            this.btnUnCheckAll.Size = new System.Drawing.Size(88, 23);
            this.btnUnCheckAll.TabIndex = 11;
            this.btnUnCheckAll.Text = "Uncheck All";
            this.btnUnCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUnCheckAll.UseVisualStyleBackColor = true;
            this.btnUnCheckAll.Click += new System.EventHandler(this.btnUnCheckAll_Click);
            // 
            // gvBuyingBrief
            // 
            this.gvBuyingBrief.AllowUserToAddRows = false;
            this.gvBuyingBrief.AllowUserToDeleteRows = false;
            this.gvBuyingBrief.AllowUserToResizeRows = false;
            this.gvBuyingBrief.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gvBuyingBrief.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvBuyingBrief.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.dataGridViewTextBoxColumn1,
            this.Column2,
            this.Column3});
            this.gvBuyingBrief.Location = new System.Drawing.Point(7, 40);
            this.gvBuyingBrief.MultiSelect = false;
            this.gvBuyingBrief.Name = "gvBuyingBrief";
            this.gvBuyingBrief.RowHeadersVisible = false;
            this.gvBuyingBrief.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvBuyingBrief.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvBuyingBrief.Size = new System.Drawing.Size(287, 117);
            this.gvBuyingBrief.TabIndex = 10;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CHK";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 25;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Buying_Brief_ID";
            this.dataGridViewTextBoxColumn1.FillWeight = 300F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Buying Brief No";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "TargetName";
            this.Column2.HeaderText = "Target";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Media_Sub_Type";
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 50;
            // 
            // btnExport
            // 
            this.btnExport.Image = global::GroupM.Minder.Properties.Resources.misc_green;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(609, 167);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(102, 34);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export Report";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(507, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Media Sub Type : ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdPeriodDate);
            this.groupBox4.Controls.Add(this.rdPeriodMonth);
            this.groupBox4.Controls.Add(this.cboYear);
            this.groupBox4.Controls.Add(this.cboMonth);
            this.groupBox4.Controls.Add(this.dtpFrom);
            this.groupBox4.Controls.Add(this.lbDateTo);
            this.groupBox4.Controls.Add(this.dtpTo);
            this.groupBox4.Location = new System.Drawing.Point(440, 50);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(271, 111);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected Date";
            // 
            // rdPeriodDate
            // 
            this.rdPeriodDate.AutoSize = true;
            this.rdPeriodDate.Enabled = false;
            this.rdPeriodDate.Location = new System.Drawing.Point(13, 54);
            this.rdPeriodDate.Name = "rdPeriodDate";
            this.rdPeriodDate.Size = new System.Drawing.Size(74, 17);
            this.rdPeriodDate.TabIndex = 9;
            this.rdPeriodDate.Text = "Date From";
            this.rdPeriodDate.UseVisualStyleBackColor = true;
            // 
            // rdPeriodMonth
            // 
            this.rdPeriodMonth.AutoSize = true;
            this.rdPeriodMonth.Checked = true;
            this.rdPeriodMonth.Location = new System.Drawing.Point(13, 19);
            this.rdPeriodMonth.Name = "rdPeriodMonth";
            this.rdPeriodMonth.Size = new System.Drawing.Size(55, 17);
            this.rdPeriodMonth.TabIndex = 9;
            this.rdPeriodMonth.TabStop = true;
            this.rdPeriodMonth.Text = "Month";
            this.rdPeriodMonth.UseVisualStyleBackColor = true;
            // 
            // cboYear
            // 
            this.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Items.AddRange(new object[] {
            "2015",
            "2014",
            "2013",
            "2012",
            "2011",
            "2010",
            "2009",
            "2008",
            "2007",
            "2006"});
            this.cboYear.Location = new System.Drawing.Point(189, 19);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(76, 21);
            this.cboYear.TabIndex = 9;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // cboMonth
            // 
            this.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cboMonth.Location = new System.Drawing.Point(71, 19);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(112, 21);
            this.cboMonth.TabIndex = 9;
            this.cboMonth.SelectedIndexChanged += new System.EventHandler(this.cboMonth_SelectedIndexChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(94, 53);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(171, 20);
            this.dtpFrom.TabIndex = 7;
            // 
            // lbDateTo
            // 
            this.lbDateTo.AutoSize = true;
            this.lbDateTo.Enabled = false;
            this.lbDateTo.Location = new System.Drawing.Point(36, 84);
            this.lbDateTo.Name = "lbDateTo";
            this.lbDateTo.Size = new System.Drawing.Size(46, 13);
            this.lbDateTo.TabIndex = 4;
            this.lbDateTo.Text = "Date To";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Enabled = false;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(94, 80);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(171, 20);
            this.dtpTo.TabIndex = 8;
            // 
            // cboMediaSubType
            // 
            this.cboMediaSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMediaSubType.FormattingEnabled = true;
            this.cboMediaSubType.Items.AddRange(new object[] {
            "TV",
            "TS",
            "CS",
            "TV,TS",
            "TV,CS",
            "TS,CS",
            "TV,TS,CS"});
            this.cboMediaSubType.Location = new System.Drawing.Point(607, 20);
            this.cboMediaSubType.Name = "cboMediaSubType";
            this.cboMediaSubType.Size = new System.Drawing.Size(104, 21);
            this.cboMediaSubType.TabIndex = 9;
            this.cboMediaSubType.SelectedIndexChanged += new System.EventHandler(this.cboMediaSubType_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.gvClient);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(399, 224);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Client";
            // 
            // gvClient
            // 
            this.gvClient.AllowUserToAddRows = false;
            this.gvClient.AllowUserToDeleteRows = false;
            this.gvClient.AllowUserToResizeRows = false;
            this.gvClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gvClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvClient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Client});
            this.gvClient.Location = new System.Drawing.Point(7, 18);
            this.gvClient.MultiSelect = false;
            this.gvClient.Name = "gvClient";
            this.gvClient.RowHeadersVisible = false;
            this.gvClient.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvClient.Size = new System.Drawing.Size(386, 200);
            this.gvClient.TabIndex = 10;
            this.gvClient.SelectionChanged += new System.EventHandler(this.gvClient_SelectionChanged);
            // 
            // Client
            // 
            this.Client.DataPropertyName = "ClientName";
            this.Client.FillWeight = 300F;
            this.Client.HeaderText = "Client";
            this.Client.Name = "Client";
            this.Client.ReadOnly = true;
            this.Client.Width = 360;
            // 
            // picThumbnail
            // 
            this.picThumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picThumbnail.BackColor = System.Drawing.Color.White;
            this.picThumbnail.Location = new System.Drawing.Point(6, 378);
            this.picThumbnail.Name = "picThumbnail";
            this.picThumbnail.Size = new System.Drawing.Size(705, 300);
            this.picThumbnail.TabIndex = 0;
            this.picThumbnail.TabStop = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
            // 
            // MPA006_ExportReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 692);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MPA006_ExportReport";
            this.Text = "4. - Export Report";
            this.Load += new System.EventHandler(this.MPA006_ExportReport_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvBuyingBrief)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picThumbnail;
        private System.Windows.Forms.TreeView tvReportName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboMediaSubType;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lbDateTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView gvClient;
        private System.Windows.Forms.RadioButton rdPeriodDate;
        private System.Windows.Forms.RadioButton rdPeriodMonth;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView gvBuyingBrief;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUnCheckAll;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}