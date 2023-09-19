namespace  GroupM.Minder
{
    partial class MPA002_SearchCondition
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPA002_SearchCondition));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.gvClient = new System.Windows.Forms.DataGridView();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.llSelectAll = new System.Windows.Forms.LinkLabel();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabClient = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabProduct = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.gvProduct = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabButingBrife = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.dgvBuyingBrief = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtBuyingBriefNo = new System.Windows.Forms.TextBox();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.rdoDate = new System.Windows.Forms.RadioButton();
            this.pnlMonthYearRange = new System.Windows.Forms.Panel();
            this.cbbYearTo = new System.Windows.Forms.ComboBox();
            this.cbbYearFrom = new System.Windows.Forms.ComboBox();
            this.cbbMonthTo = new System.Windows.Forms.ComboBox();
            this.cbbMonthFrom = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoMonthYearRange = new System.Windows.Forms.RadioButton();
            this.pnlMonthYear = new System.Windows.Forms.Panel();
            this.cbbMonth = new System.Windows.Forms.ComboBox();
            this.cbbYear = new System.Windows.Forms.ComboBox();
            this.rdoMonthYear = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoHighSpeed = new System.Windows.Forms.RadioButton();
            this.rdoRealTime = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxMediaSubType = new GroupM.CustomControl.Common.CustomComboboxDetail(this.components);
            this.cbxMediaType = new GroupM.CustomControl.Common.CustomComboboxDetail(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.tabClient.SuspendLayout();
            this.tabProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvProduct)).BeginInit();
            this.tabButingBrife.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyingBrief)).BeginInit();
            this.pnlDate.SuspendLayout();
            this.pnlMonthYearRange.SuspendLayout();
            this.pnlMonthYear.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "To:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(49, 2);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 20);
            this.dtpFrom.TabIndex = 2;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(187, 2);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 20);
            this.dtpTo.TabIndex = 2;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Media Sub Type : ";
            // 
            // gvClient
            // 
            this.gvClient.AllowUserToAddRows = false;
            this.gvClient.AllowUserToDeleteRows = false;
            this.gvClient.AllowUserToResizeRows = false;
            this.gvClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvClient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Client});
            this.gvClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.gvClient.Location = new System.Drawing.Point(3, 3);
            this.gvClient.Name = "gvClient";
            this.gvClient.RowHeadersVisible = false;
            this.gvClient.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvClient.Size = new System.Drawing.Size(458, 378);
            this.gvClient.TabIndex = 11;
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
            // llSelectAll
            // 
            this.llSelectAll.AutoSize = true;
            this.llSelectAll.Location = new System.Drawing.Point(399, 135);
            this.llSelectAll.Name = "llSelectAll";
            this.llSelectAll.Size = new System.Drawing.Size(80, 13);
            this.llSelectAll.TabIndex = 12;
            this.llSelectAll.TabStop = true;
            this.llSelectAll.Text = "Select All Client";
            this.llSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectAll_LinkClicked);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabClient);
            this.mainTabControl.Controls.Add(this.tabProduct);
            this.mainTabControl.Controls.Add(this.tabButingBrife);
            this.mainTabControl.Location = new System.Drawing.Point(17, 164);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(472, 442);
            this.mainTabControl.TabIndex = 13;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            // 
            // tabClient
            // 
            this.tabClient.Controls.Add(this.gvClient);
            this.tabClient.Controls.Add(this.button1);
            this.tabClient.Controls.Add(this.button2);
            this.tabClient.Location = new System.Drawing.Point(4, 22);
            this.tabClient.Name = "tabClient";
            this.tabClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabClient.Size = new System.Drawing.Size(464, 416);
            this.tabClient.TabIndex = 0;
            this.tabClient.Text = "Client";
            this.tabClient.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(302, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Calculate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(383, 387);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabProduct
            // 
            this.tabProduct.Controls.Add(this.button3);
            this.tabProduct.Controls.Add(this.button4);
            this.tabProduct.Controls.Add(this.gvProduct);
            this.tabProduct.Location = new System.Drawing.Point(4, 22);
            this.tabProduct.Name = "tabProduct";
            this.tabProduct.Padding = new System.Windows.Forms.Padding(3);
            this.tabProduct.Size = new System.Drawing.Size(464, 416);
            this.tabProduct.TabIndex = 1;
            this.tabProduct.Text = "Product";
            this.tabProduct.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(302, 387);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Calculate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(383, 387);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // gvProduct
            // 
            this.gvProduct.AllowUserToAddRows = false;
            this.gvProduct.AllowUserToDeleteRows = false;
            this.gvProduct.AllowUserToResizeRows = false;
            this.gvProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.gvProduct.Location = new System.Drawing.Point(3, 3);
            this.gvProduct.Name = "gvProduct";
            this.gvProduct.RowHeadersVisible = false;
            this.gvProduct.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvProduct.Size = new System.Drawing.Size(458, 378);
            this.gvProduct.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ProductName";
            this.dataGridViewTextBoxColumn1.FillWeight = 300F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Product";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 360;
            // 
            // tabButingBrife
            // 
            this.tabButingBrife.Controls.Add(this.label4);
            this.tabButingBrife.Controls.Add(this.button5);
            this.tabButingBrife.Controls.Add(this.button6);
            this.tabButingBrife.Controls.Add(this.dgvBuyingBrief);
            this.tabButingBrife.Controls.Add(this.txtBuyingBriefNo);
            this.tabButingBrife.Location = new System.Drawing.Point(4, 22);
            this.tabButingBrife.Name = "tabButingBrife";
            this.tabButingBrife.Padding = new System.Windows.Forms.Padding(3);
            this.tabButingBrife.Size = new System.Drawing.Size(464, 416);
            this.tabButingBrife.TabIndex = 2;
            this.tabButingBrife.Text = "BB No.";
            this.tabButingBrife.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "BuyingBrief No.";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(302, 387);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "Calculate";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(383, 387);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "Cancel";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button2_Click);
            // 
            // dgvBuyingBrief
            // 
            this.dgvBuyingBrief.AllowUserToAddRows = false;
            this.dgvBuyingBrief.AllowUserToDeleteRows = false;
            this.dgvBuyingBrief.AllowUserToResizeRows = false;
            this.dgvBuyingBrief.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBuyingBrief.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBuyingBrief.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvBuyingBrief.Location = new System.Drawing.Point(3, 32);
            this.dgvBuyingBrief.Name = "dgvBuyingBrief";
            this.dgvBuyingBrief.ReadOnly = true;
            this.dgvBuyingBrief.RowHeadersVisible = false;
            this.dgvBuyingBrief.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBuyingBrief.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBuyingBrief.Size = new System.Drawing.Size(458, 349);
            this.dgvBuyingBrief.TabIndex = 13;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Buying_Brief_ID";
            this.Column1.FillWeight = 50.76142F;
            this.Column1.HeaderText = "BuyingBrief";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Description";
            this.Column2.FillWeight = 149.2386F;
            this.Column2.HeaderText = "Campaign";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // txtBuyingBriefNo
            // 
            this.txtBuyingBriefNo.Location = new System.Drawing.Point(92, 6);
            this.txtBuyingBriefNo.Name = "txtBuyingBriefNo";
            this.txtBuyingBriefNo.Size = new System.Drawing.Size(117, 20);
            this.txtBuyingBriefNo.TabIndex = 0;
            this.txtBuyingBriefNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBuyingBriefNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBuyingBriefNo_KeyUp);
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.label1);
            this.pnlDate.Controls.Add(this.dtpFrom);
            this.pnlDate.Controls.Add(this.label2);
            this.pnlDate.Controls.Add(this.dtpTo);
            this.pnlDate.Location = new System.Drawing.Point(101, 44);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(346, 23);
            this.pnlDate.TabIndex = 14;
            // 
            // rdoDate
            // 
            this.rdoDate.AutoSize = true;
            this.rdoDate.Checked = true;
            this.rdoDate.Location = new System.Drawing.Point(15, 47);
            this.rdoDate.Name = "rdoDate";
            this.rdoDate.Size = new System.Drawing.Size(48, 17);
            this.rdoDate.TabIndex = 15;
            this.rdoDate.TabStop = true;
            this.rdoDate.Text = "Date";
            this.rdoDate.UseVisualStyleBackColor = true;
            this.rdoDate.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // pnlMonthYearRange
            // 
            this.pnlMonthYearRange.Controls.Add(this.cbbYearTo);
            this.pnlMonthYearRange.Controls.Add(this.cbbYearFrom);
            this.pnlMonthYearRange.Controls.Add(this.cbbMonthTo);
            this.pnlMonthYearRange.Controls.Add(this.cbbMonthFrom);
            this.pnlMonthYearRange.Controls.Add(this.label6);
            this.pnlMonthYearRange.Controls.Add(this.label5);
            this.pnlMonthYearRange.Enabled = false;
            this.pnlMonthYearRange.Location = new System.Drawing.Point(101, 71);
            this.pnlMonthYearRange.Name = "pnlMonthYearRange";
            this.pnlMonthYearRange.Size = new System.Drawing.Size(346, 24);
            this.pnlMonthYearRange.TabIndex = 14;
            // 
            // cbbYearTo
            // 
            this.cbbYearTo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbYearTo.FormattingEnabled = true;
            this.cbbYearTo.Location = new System.Drawing.Point(232, 2);
            this.cbbYearTo.Name = "cbbYearTo";
            this.cbbYearTo.Size = new System.Drawing.Size(64, 21);
            this.cbbYearTo.TabIndex = 16;
            this.cbbYearTo.SelectedIndexChanged += new System.EventHandler(this.cbbMonthFrom_SelectedIndexChanged);
            this.cbbYearTo.SelectionChangeCommitted += new System.EventHandler(this.cbbMonthFrom_SelectionChangeCommitted);
            // 
            // cbbYearFrom
            // 
            this.cbbYearFrom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbYearFrom.FormattingEnabled = true;
            this.cbbYearFrom.Location = new System.Drawing.Point(95, 2);
            this.cbbYearFrom.Name = "cbbYearFrom";
            this.cbbYearFrom.Size = new System.Drawing.Size(64, 21);
            this.cbbYearFrom.TabIndex = 16;
            this.cbbYearFrom.SelectedIndexChanged += new System.EventHandler(this.cbbMonthFrom_SelectedIndexChanged);
            this.cbbYearFrom.SelectionChangeCommitted += new System.EventHandler(this.cbbMonthFrom_SelectionChangeCommitted);
            // 
            // cbbMonthTo
            // 
            this.cbbMonthTo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbMonthTo.FormattingEnabled = true;
            this.cbbMonthTo.Location = new System.Drawing.Point(186, 2);
            this.cbbMonthTo.Name = "cbbMonthTo";
            this.cbbMonthTo.Size = new System.Drawing.Size(40, 21);
            this.cbbMonthTo.TabIndex = 16;
            this.cbbMonthTo.SelectedIndexChanged += new System.EventHandler(this.cbbMonthFrom_SelectedIndexChanged);
            this.cbbMonthTo.SelectionChangeCommitted += new System.EventHandler(this.cbbMonthFrom_SelectionChangeCommitted);
            // 
            // cbbMonthFrom
            // 
            this.cbbMonthFrom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbMonthFrom.FormattingEnabled = true;
            this.cbbMonthFrom.Location = new System.Drawing.Point(49, 2);
            this.cbbMonthFrom.Name = "cbbMonthFrom";
            this.cbbMonthFrom.Size = new System.Drawing.Size(40, 21);
            this.cbbMonthFrom.TabIndex = 16;
            this.cbbMonthFrom.SelectedIndexChanged += new System.EventHandler(this.cbbMonthFrom_SelectedIndexChanged);
            this.cbbMonthFrom.SelectionChangeCommitted += new System.EventHandler(this.cbbMonthFrom_SelectionChangeCommitted);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(163, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "To:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "From:";
            // 
            // rdoMonthYearRange
            // 
            this.rdoMonthYearRange.AutoSize = true;
            this.rdoMonthYearRange.Location = new System.Drawing.Point(15, 73);
            this.rdoMonthYearRange.Name = "rdoMonthYearRange";
            this.rdoMonthYearRange.Size = new System.Drawing.Size(82, 17);
            this.rdoMonthYearRange.TabIndex = 15;
            this.rdoMonthYearRange.Text = "Month/Year";
            this.rdoMonthYearRange.UseVisualStyleBackColor = true;
            this.rdoMonthYearRange.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // pnlMonthYear
            // 
            this.pnlMonthYear.Controls.Add(this.cbbMonth);
            this.pnlMonthYear.Controls.Add(this.cbbYear);
            this.pnlMonthYear.Enabled = false;
            this.pnlMonthYear.Location = new System.Drawing.Point(101, 99);
            this.pnlMonthYear.Name = "pnlMonthYear";
            this.pnlMonthYear.Size = new System.Drawing.Size(346, 24);
            this.pnlMonthYear.TabIndex = 14;
            // 
            // cbbMonth
            // 
            this.cbbMonth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbMonth.FormattingEnabled = true;
            this.cbbMonth.Location = new System.Drawing.Point(49, 2);
            this.cbbMonth.Name = "cbbMonth";
            this.cbbMonth.Size = new System.Drawing.Size(40, 21);
            this.cbbMonth.TabIndex = 16;
            this.cbbMonth.SelectedIndexChanged += new System.EventHandler(this.cbbMonthFrom_SelectedIndexChanged);
            this.cbbMonth.SelectionChangeCommitted += new System.EventHandler(this.cbbMonthFrom_SelectionChangeCommitted);
            this.cbbMonth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbbMonth_KeyUp);
            // 
            // cbbYear
            // 
            this.cbbYear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbYear.FormattingEnabled = true;
            this.cbbYear.Location = new System.Drawing.Point(95, 2);
            this.cbbYear.Name = "cbbYear";
            this.cbbYear.Size = new System.Drawing.Size(64, 21);
            this.cbbYear.TabIndex = 16;
            this.cbbYear.SelectedIndexChanged += new System.EventHandler(this.cbbMonthFrom_SelectedIndexChanged);
            this.cbbYear.SelectionChangeCommitted += new System.EventHandler(this.cbbMonthFrom_SelectionChangeCommitted);
            this.cbbYear.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbbYear_KeyUp);
            // 
            // rdoMonthYear
            // 
            this.rdoMonthYear.AutoSize = true;
            this.rdoMonthYear.Location = new System.Drawing.Point(15, 101);
            this.rdoMonthYear.Name = "rdoMonthYear";
            this.rdoMonthYear.Size = new System.Drawing.Size(82, 17);
            this.rdoMonthYear.TabIndex = 15;
            this.rdoMonthYear.Text = "Month/Year";
            this.rdoMonthYear.UseVisualStyleBackColor = true;
            this.rdoMonthYear.CheckedChanged += new System.EventHandler(this.rdo_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.rdoHighSpeed);
            this.panel1.Controls.Add(this.rdoRealTime);
            this.panel1.Location = new System.Drawing.Point(-8, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 29);
            this.panel1.TabIndex = 24;
            // 
            // rdoHighSpeed
            // 
            this.rdoHighSpeed.AutoSize = true;
            this.rdoHighSpeed.Checked = true;
            this.rdoHighSpeed.Location = new System.Drawing.Point(22, 5);
            this.rdoHighSpeed.Name = "rdoHighSpeed";
            this.rdoHighSpeed.Size = new System.Drawing.Size(214, 17);
            this.rdoHighSpeed.TabIndex = 24;
            this.rdoHighSpeed.TabStop = true;
            this.rdoHighSpeed.Text = "High Speed(Updated:06/08/2015 4PM)";
            this.rdoHighSpeed.UseVisualStyleBackColor = true;
            this.rdoHighSpeed.CheckedChanged += new System.EventHandler(this.rdoHighSpeed_CheckedChanged);
            // 
            // rdoRealTime
            // 
            this.rdoRealTime.AutoSize = true;
            this.rdoRealTime.Location = new System.Drawing.Point(288, 5);
            this.rdoRealTime.Name = "rdoRealTime";
            this.rdoRealTime.Size = new System.Drawing.Size(226, 17);
            this.rdoRealTime.TabIndex = 25;
            this.rdoRealTime.Text = "Real Time (Take long time to retrieve data)";
            this.rdoRealTime.UseVisualStyleBackColor = true;
            this.rdoRealTime.CheckedChanged += new System.EventHandler(this.rdoRealTime_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Media Type : ";
            // 
            // cbxMediaSubType
            // 
            this.cbxMediaSubType.DropDownHeight = 1;
            this.cbxMediaSubType.FormattingEnabled = true;
            this.cbxMediaSubType.IntegralHeight = false;
            this.cbxMediaSubType.Location = new System.Drawing.Point(296, 131);
            this.cbxMediaSubType.Name = "cbxMediaSubType";
            this.cbxMediaSubType.Size = new System.Drawing.Size(84, 21);
            this.cbxMediaSubType.ss = null;
            this.cbxMediaSubType.TabIndex = 27;
            this.cbxMediaSubType.e_Deactivated += new GroupM.CustomControl.Common.CustomComboboxDetail.DeactivatedHandler(this.cbxMediaSubType_e_Deactivated);
            // 
            // cbxMediaType
            // 
            this.cbxMediaType.DropDownHeight = 1;
            this.cbxMediaType.FormattingEnabled = true;
            this.cbxMediaType.IntegralHeight = false;
            this.cbxMediaType.Location = new System.Drawing.Point(99, 131);
            this.cbxMediaType.Name = "cbxMediaType";
            this.cbxMediaType.Size = new System.Drawing.Size(84, 21);
            this.cbxMediaType.ss = null;
            this.cbxMediaType.TabIndex = 26;
            this.cbxMediaType.e_Deactivated += new GroupM.CustomControl.Common.CustomComboboxDetail.DeactivatedHandler(this.cbxMediaType_e_Deactivated);
            this.cbxMediaType.Validated += new System.EventHandler(this.cbxMediaType_Validated);
            // 
            // MPA002_SearchCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 618);
            this.Controls.Add(this.cbxMediaSubType);
            this.Controls.Add(this.cbxMediaType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rdoMonthYear);
            this.Controls.Add(this.rdoMonthYearRange);
            this.Controls.Add(this.rdoDate);
            this.Controls.Add(this.pnlMonthYear);
            this.Controls.Add(this.pnlMonthYearRange);
            this.Controls.Add(this.pnlDate);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.llSelectAll);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MPA002_SearchCondition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Condition";
            this.Load += new System.EventHandler(this.MPA002_SearchCondition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.tabClient.ResumeLayout(false);
            this.tabProduct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvProduct)).EndInit();
            this.tabButingBrife.ResumeLayout(false);
            this.tabButingBrife.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuyingBrief)).EndInit();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            this.pnlMonthYearRange.ResumeLayout(false);
            this.pnlMonthYearRange.PerformLayout();
            this.pnlMonthYear.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView gvClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.LinkLabel llSelectAll;
        private System.Windows.Forms.TabPage tabClient;
        private System.Windows.Forms.TabPage tabProduct;
        private System.Windows.Forms.DataGridView gvProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabButingBrife;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DataGridView dgvBuyingBrief;
        private System.Windows.Forms.TextBox txtBuyingBriefNo;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.RadioButton rdoDate;
        private System.Windows.Forms.Panel pnlMonthYearRange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdoMonthYearRange;
        private System.Windows.Forms.Panel pnlMonthYear;
        private System.Windows.Forms.RadioButton rdoMonthYear;
        private System.Windows.Forms.ComboBox cbbYearFrom;
        private System.Windows.Forms.ComboBox cbbMonthFrom;
        private System.Windows.Forms.ComboBox cbbYearTo;
        private System.Windows.Forms.ComboBox cbbMonthTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbbMonth;
        private System.Windows.Forms.ComboBox cbbYear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoHighSpeed;
        private System.Windows.Forms.RadioButton rdoRealTime;
        private System.Windows.Forms.Label label7;
        private GroupM.CustomControl.Common.CustomComboboxDetail cbxMediaType;
        private GroupM.CustomControl.Common.CustomComboboxDetail cbxMediaSubType;
    }
}