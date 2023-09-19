namespace  GroupM.Minder
{
    partial class MPA010_Reach_Weekly_SearchCondition_1
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
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.btnRefreshBB = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.cboMediaSubType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.rdDraft = new System.Windows.Forms.RadioButton();
            this.rdLastest = new System.Windows.Forms.RadioButton();
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
            this.Column3,
            this.Column6,
            this.Column7,
            this.Column4,
            this.Client,
            this.Column1,
            this.Column5});
            this.gvClient.Location = new System.Drawing.Point(6, 81);
            this.gvClient.MultiSelect = false;
            this.gvClient.Name = "gvClient";
            this.gvClient.RowHeadersVisible = false;
            this.gvClient.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvClient.Size = new System.Drawing.Size(704, 608);
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
            // Column3
            // 
            this.Column3.DataPropertyName = "Version";
            this.Column3.HeaderText = "V";
            this.Column3.Name = "Column3";
            this.Column3.Width = 20;
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
            this.Client.ReadOnly = true;
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
            this.label2.Location = new System.Drawing.Point(9, 9);
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
            this.dtEndDate.Location = new System.Drawing.Point(269, 5);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(127, 20);
            this.dtEndDate.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 9);
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
            // btnRefreshBB
            // 
            this.btnRefreshBB.Location = new System.Drawing.Point(568, 31);
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
            this.btnProcess.Text = "Next >>";
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
            this.cboMediaSubType.Location = new System.Drawing.Point(568, 5);
            this.cboMediaSubType.Name = "cboMediaSubType";
            this.cboMediaSubType.Size = new System.Drawing.Size(94, 21);
            this.cboMediaSubType.TabIndex = 27;
            this.cboMediaSubType.SelectedIndexChanged += new System.EventHandler(this.cboMediaSubType_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(481, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Media Sub Type";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Gold;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox1.Location = new System.Drawing.Point(0, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(710, 19);
            this.textBox1.TabIndex = 28;
            this.textBox1.Text = "   Step 1 : Select Brief";
            // 
            // rdDraft
            // 
            this.rdDraft.AutoSize = true;
            this.rdDraft.Checked = true;
            this.rdDraft.Location = new System.Drawing.Point(13, 33);
            this.rdDraft.Name = "rdDraft";
            this.rdDraft.Size = new System.Drawing.Size(48, 17);
            this.rdDraft.TabIndex = 29;
            this.rdDraft.TabStop = true;
            this.rdDraft.Text = "Draft";
            this.rdDraft.UseVisualStyleBackColor = true;
            // 
            // rdLastest
            // 
            this.rdLastest.AutoSize = true;
            this.rdLastest.Location = new System.Drawing.Point(67, 33);
            this.rdLastest.Name = "rdLastest";
            this.rdLastest.Size = new System.Drawing.Size(150, 17);
            this.rdLastest.TabIndex = 29;
            this.rdLastest.Text = "Lastest (Executing/Actual)";
            this.rdLastest.UseVisualStyleBackColor = true;
            // 
            // MPA010_Reach_Weekly_SearchCondition_1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 727);
            this.Controls.Add(this.rdLastest);
            this.Controls.Add(this.rdDraft);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cboMediaSubType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnRefreshBB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtEndDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.chkUnCheckAll);
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.gvClient);
            this.Name = "MPA010_Reach_Weekly_SearchCondition_1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MPA010 - Step 1 : Select Brief";
            this.Load += new System.EventHandler(this.MPA010_Reach_Weekly_SearchCondition_1_Load);
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
        private System.Windows.Forms.Button btnRefreshBB;
        private System.Windows.Forms.Button btnProcess;
        public System.Windows.Forms.DataGridView gvClient;
        public System.Windows.Forms.DateTimePicker dtEndDate;
        public System.Windows.Forms.DateTimePicker dtStartDate;
        private System.Windows.Forms.ComboBox cboMediaSubType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton rdLastest;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BuyingBriefID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        public System.Windows.Forms.RadioButton rdDraft;
    }
}