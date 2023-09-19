namespace  GroupM.Minder
{
    partial class MPA010_Reach_SearchCondition
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
            this.gvClient = new System.Windows.Forms.DataGridView();
            this.BuyingBriefID = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkCheckAll = new System.Windows.Forms.Button();
            this.chkUnCheckAll = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.dtEndDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtBaseDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDOWBaseDate = new System.Windows.Forms.TextBox();
            this.txtDOWEndDate = new System.Windows.Forms.TextBox();
            this.txtDiffDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbAvailable = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnRefreshBB = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.cboMediaSubType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).BeginInit();
            this.SuspendLayout();
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
            this.Column6,
            this.Column7,
            this.Column4,
            this.Client,
            this.Column1,
            this.Column5});
            this.gvClient.Location = new System.Drawing.Point(6, 119);
            this.gvClient.MultiSelect = false;
            this.gvClient.Name = "gvClient";
            this.gvClient.RowHeadersVisible = false;
            this.gvClient.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvClient.Size = new System.Drawing.Size(704, 567);
            this.gvClient.TabIndex = 12;
            this.gvClient.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvClient_CellDoubleClick);
            // 
            // BuyingBriefID
            // 
            this.BuyingBriefID.DataPropertyName = "chk";
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
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "MediaSubType";
            this.Column6.HeaderText = "MT";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 30;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "Target";
            this.Column7.HeaderText = "Target";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ProductName";
            this.Column4.HeaderText = "Product";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 150;
            // 
            // Client
            // 
            this.Client.DataPropertyName = "ClientName";
            this.Client.FillWeight = 300F;
            this.Client.HeaderText = "Client";
            this.Client.Name = "Client";
            this.Client.Width = 150;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "StartDate";
            this.Column1.HeaderText = "StartDate";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 60;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "EndDate";
            this.Column5.HeaderText = "EndDate";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 60;
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCheckAll.Location = new System.Drawing.Point(12, 695);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Size = new System.Drawing.Size(75, 23);
            this.chkCheckAll.TabIndex = 15;
            this.chkCheckAll.Text = "Check All";
            this.chkCheckAll.UseVisualStyleBackColor = true;
            this.chkCheckAll.Click += new System.EventHandler(this.chkCheckAll_Click);
            // 
            // chkUnCheckAll
            // 
            this.chkUnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkUnCheckAll.Location = new System.Drawing.Point(93, 695);
            this.chkUnCheckAll.Name = "chkUnCheckAll";
            this.chkUnCheckAll.Size = new System.Drawing.Size(75, 23);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Start Date";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dtEndDate
            // 
            this.dtEndDate.CustomFormat = "dd/MM/yyyy";
            this.dtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEndDate.Location = new System.Drawing.Point(74, 34);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(127, 20);
            this.dtEndDate.TabIndex = 21;
            this.dtEndDate.ValueChanged += new System.EventHandler(this.dtEndDate_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "End Date";
            // 
            // dtStartDate
            // 
            this.dtStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStartDate.Location = new System.Drawing.Point(74, 5);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(127, 20);
            this.dtStartDate.TabIndex = 20;
            this.dtStartDate.ValueChanged += new System.EventHandler(this.dtStartDate_ValueChanged);
            // 
            // dtBaseDate
            // 
            this.dtBaseDate.CustomFormat = "dd/MM/yyyy";
            this.dtBaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtBaseDate.Location = new System.Drawing.Point(269, 34);
            this.dtBaseDate.Name = "dtBaseDate";
            this.dtBaseDate.Size = new System.Drawing.Size(127, 20);
            this.dtBaseDate.TabIndex = 20;
            this.dtBaseDate.ValueChanged += new System.EventHandler(this.dtBaseDate_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.OrangeRed;
            this.label4.Location = new System.Drawing.Point(207, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Base Date";
            this.label4.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.OrangeRed;
            this.label1.Location = new System.Drawing.Point(238, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Day";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Day";
            // 
            // txtDOWBaseDate
            // 
            this.txtDOWBaseDate.BackColor = System.Drawing.Color.LightBlue;
            this.txtDOWBaseDate.Location = new System.Drawing.Point(269, 61);
            this.txtDOWBaseDate.Name = "txtDOWBaseDate";
            this.txtDOWBaseDate.ReadOnly = true;
            this.txtDOWBaseDate.Size = new System.Drawing.Size(127, 20);
            this.txtDOWBaseDate.TabIndex = 23;
            // 
            // txtDOWEndDate
            // 
            this.txtDOWEndDate.BackColor = System.Drawing.Color.LightBlue;
            this.txtDOWEndDate.Location = new System.Drawing.Point(72, 61);
            this.txtDOWEndDate.Name = "txtDOWEndDate";
            this.txtDOWEndDate.ReadOnly = true;
            this.txtDOWEndDate.Size = new System.Drawing.Size(127, 20);
            this.txtDOWEndDate.TabIndex = 23;
            // 
            // txtDiffDate
            // 
            this.txtDiffDate.Location = new System.Drawing.Point(269, 87);
            this.txtDiffDate.Name = "txtDiffDate";
            this.txtDiffDate.ReadOnly = true;
            this.txtDiffDate.Size = new System.Drawing.Size(127, 20);
            this.txtDiffDate.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.OrangeRed;
            this.label6.Location = new System.Drawing.Point(195, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Different Day";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.OrangeRed;
            this.label7.Location = new System.Drawing.Point(402, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "*Map with \"End Date\"";
            this.label7.Click += new System.EventHandler(this.label2_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.OrangeRed;
            this.label8.Location = new System.Drawing.Point(402, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "*Must same as Day of End Date";
            this.label8.Click += new System.EventHandler(this.label2_Click);
            // 
            // lbAvailable
            // 
            this.lbAvailable.AutoSize = true;
            this.lbAvailable.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbAvailable.Location = new System.Drawing.Point(511, 38);
            this.lbAvailable.Name = "lbAvailable";
            this.lbAvailable.Size = new System.Drawing.Size(96, 13);
            this.lbAvailable.TabIndex = 19;
            this.lbAvailable.Text = "*Selected Date by ";
            this.lbAvailable.Click += new System.EventHandler(this.label2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.OrangeRed;
            this.label9.Location = new System.Drawing.Point(402, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Day(s)";
            // 
            // btnRefreshBB
            // 
            this.btnRefreshBB.Location = new System.Drawing.Point(568, 89);
            this.btnRefreshBB.Name = "btnRefreshBB";
            this.btnRefreshBB.Size = new System.Drawing.Size(142, 23);
            this.btnRefreshBB.TabIndex = 24;
            this.btnRefreshBB.Text = "Refressh Buying Brief List";
            this.btnRefreshBB.UseVisualStyleBackColor = true;
            this.btnRefreshBB.Click += new System.EventHandler(this.btnRefreshBB_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcess.Location = new System.Drawing.Point(591, 695);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(109, 23);
            this.btnProcess.TabIndex = 25;
            this.btnProcess.Text = "Process Selected Brief";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
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
            this.cboMediaSubType.Location = new System.Drawing.Point(74, 86);
            this.cboMediaSubType.Name = "cboMediaSubType";
            this.cboMediaSubType.Size = new System.Drawing.Size(94, 21);
            this.cboMediaSubType.TabIndex = 27;
            this.cboMediaSubType.SelectedIndexChanged += new System.EventHandler(this.cboMediaSubType_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 26);
            this.label10.TabIndex = 26;
            this.label10.Text = "Media Sub \r\nType";
            // 
            // MPA010_Reach_SearchCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 727);
            this.Controls.Add(this.cboMediaSubType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnRefreshBB);
            this.Controls.Add(this.txtDiffDate);
            this.Controls.Add(this.txtDOWEndDate);
            this.Controls.Add(this.txtDOWBaseDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbAvailable);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtEndDate);
            this.Controls.Add(this.dtBaseDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.chkUnCheckAll);
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.gvClient);
            this.Name = "MPA010_Reach_SearchCondition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MPA010 - Selected Brief to Process Reach";
            this.Load += new System.EventHandler(this.MPA010_Reach_SearchCondition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvClient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button chkCheckAll;
        private System.Windows.Forms.Button chkUnCheckAll;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtBaseDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDOWBaseDate;
        private System.Windows.Forms.TextBox txtDOWEndDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbAvailable;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnRefreshBB;
        private System.Windows.Forms.Button btnProcess;
        public System.Windows.Forms.DataGridView gvClient;
        public System.Windows.Forms.DateTimePicker dtEndDate;
        public System.Windows.Forms.DateTimePicker dtStartDate;
        public System.Windows.Forms.TextBox txtDiffDate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BuyingBriefID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.ComboBox cboMediaSubType;
        private System.Windows.Forms.Label label10;
    }
}