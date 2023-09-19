namespace MProprietary
{
    partial class frmProprietaryMapping
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProprietaryMapping));
            this.btnAddClient = new System.Windows.Forms.Button();
            this.gvDetailClient = new System.Windows.Forms.DataGridView();
            this.gvDetailProprietary = new System.Windows.Forms.DataGridView();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.tabData = new System.Windows.Forms.TabControl();
            this.tabProprietary = new System.Windows.Forms.TabPage();
            this.btnAddProprietary = new System.Windows.Forms.Button();
            this.tabClient = new System.Windows.Forms.TabPage();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboContractType = new System.Windows.Forms.ComboBox();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetailClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetailProprietary)).BeginInit();
            this.tabData.SuspendLayout();
            this.tabProprietary.SuspendLayout();
            this.tabClient.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddClient
            // 
            this.btnAddClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddClient.Image = global::MProprietary.Properties.Resources.btnAdd;
            this.btnAddClient.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddClient.Location = new System.Drawing.Point(601, 444);
            this.btnAddClient.Name = "btnAddClient";
            this.btnAddClient.Size = new System.Drawing.Size(55, 23);
            this.btnAddClient.TabIndex = 1;
            this.btnAddClient.Text = "New";
            this.btnAddClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddClient.UseVisualStyleBackColor = true;
            this.btnAddClient.Click += new System.EventHandler(this.btnAddClient_Click);
            // 
            // gvDetailClient
            // 
            this.gvDetailClient.AllowUserToAddRows = false;
            this.gvDetailClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvDetailClient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvDetailClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDetailClient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6});
            this.gvDetailClient.Location = new System.Drawing.Point(6, 6);
            this.gvDetailClient.Name = "gvDetailClient";
            this.gvDetailClient.ReadOnly = true;
            this.gvDetailClient.RowHeadersWidth = 20;
            this.gvDetailClient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDetailClient.Size = new System.Drawing.Size(650, 432);
            this.gvDetailClient.TabIndex = 0;
            this.gvDetailClient.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gvDetailClient_UserDeletingRow);
            // 
            // gvDetailProprietary
            // 
            this.gvDetailProprietary.AllowUserToAddRows = false;
            this.gvDetailProprietary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvDetailProprietary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvDetailProprietary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDetailProprietary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.gvDetailProprietary.Location = new System.Drawing.Point(6, 6);
            this.gvDetailProprietary.Name = "gvDetailProprietary";
            this.gvDetailProprietary.ReadOnly = true;
            this.gvDetailProprietary.RowHeadersWidth = 20;
            this.gvDetailProprietary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDetailProprietary.Size = new System.Drawing.Size(650, 433);
            this.gvDetailProprietary.TabIndex = 0;
            this.gvDetailProprietary.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gvDetailProprietary_UserDeletingRow);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(139, 39);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(539, 20);
            this.txtDescription.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(13, 43);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupName.Location = new System.Drawing.Point(139, 13);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(539, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(13, 16);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(120, 13);
            this.lblGroupName.TabIndex = 0;
            this.lblGroupName.Text = "Proprietary Group Name";
            // 
            // tabData
            // 
            this.tabData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabData.Controls.Add(this.tabProprietary);
            this.tabData.Controls.Add(this.tabClient);
            this.tabData.Location = new System.Drawing.Point(12, 99);
            this.tabData.Name = "tabData";
            this.tabData.SelectedIndex = 0;
            this.tabData.Size = new System.Drawing.Size(670, 500);
            this.tabData.TabIndex = 4;
            // 
            // tabProprietary
            // 
            this.tabProprietary.Controls.Add(this.btnAddProprietary);
            this.tabProprietary.Controls.Add(this.gvDetailProprietary);
            this.tabProprietary.Location = new System.Drawing.Point(4, 22);
            this.tabProprietary.Name = "tabProprietary";
            this.tabProprietary.Padding = new System.Windows.Forms.Padding(3);
            this.tabProprietary.Size = new System.Drawing.Size(662, 474);
            this.tabProprietary.TabIndex = 0;
            this.tabProprietary.Text = "Proprietary Permission";
            this.tabProprietary.UseVisualStyleBackColor = true;
            // 
            // btnAddProprietary
            // 
            this.btnAddProprietary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddProprietary.Image = global::MProprietary.Properties.Resources.btnAdd;
            this.btnAddProprietary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddProprietary.Location = new System.Drawing.Point(601, 445);
            this.btnAddProprietary.Name = "btnAddProprietary";
            this.btnAddProprietary.Size = new System.Drawing.Size(55, 23);
            this.btnAddProprietary.TabIndex = 1;
            this.btnAddProprietary.Text = "New";
            this.btnAddProprietary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddProprietary.UseVisualStyleBackColor = true;
            this.btnAddProprietary.Click += new System.EventHandler(this.btnAddProprietary_Click);
            // 
            // tabClient
            // 
            this.tabClient.Controls.Add(this.btnAddClient);
            this.tabClient.Controls.Add(this.gvDetailClient);
            this.tabClient.Location = new System.Drawing.Point(4, 22);
            this.tabClient.Name = "tabClient";
            this.tabClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabClient.Size = new System.Drawing.Size(662, 474);
            this.tabClient.TabIndex = 1;
            this.tabClient.Text = "Client Member";
            this.tabClient.UseVisualStyleBackColor = true;
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Image = global::MProprietary.Properties.Resources.btnDelete;
            this.btnQuit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuit.Location = new System.Drawing.Point(627, 608);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(55, 23);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "Quit";
            this.btnQuit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::MProprietary.Properties.Resources.btnSave;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(562, 608);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Contract Type";
            // 
            // cboContractType
            // 
            this.cboContractType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboContractType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboContractType.FormattingEnabled = true;
            this.cboContractType.Items.AddRange(new object[] {
            "Opt-in Signed",
            "Letter Signed"});
            this.cboContractType.Location = new System.Drawing.Point(139, 65);
            this.cboContractType.Name = "cboContractType";
            this.cboContractType.Size = new System.Drawing.Size(539, 21);
            this.cboContractType.TabIndex = 8;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Media_Vendor_ID";
            this.Column1.HeaderText = "Vendor ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Media_Vendor_Name";
            this.Column2.HeaderText = "Vendor Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 230;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Media_ID";
            this.Column3.HeaderText = "Media ID";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Media_Name";
            this.Column4.HeaderText = "Media Name";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 270;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Client_Id";
            this.Column5.HeaderText = "Client ID";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Short_Name";
            this.Column6.HeaderText = "Client Name";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 504;
            // 
            // frmProprietaryMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 643);
            this.Controls.Add(this.cboContractType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabData);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.lblGroupName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProprietaryMapping";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proprietary Mapping";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProprietaryMapping_FormClosing);
            this.Load += new System.EventHandler(this.frmProprietaryMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetailClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetailProprietary)).EndInit();
            this.tabData.ResumeLayout(false);
            this.tabProprietary.ResumeLayout(false);
            this.tabClient.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView gvDetailClient;
        public System.Windows.Forms.DataGridView gvDetailProprietary;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.Button btnAddClient;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.TabControl tabData;
        private System.Windows.Forms.TabPage tabProprietary;
        private System.Windows.Forms.TabPage tabClient;
        private System.Windows.Forms.Button btnAddProprietary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboContractType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}