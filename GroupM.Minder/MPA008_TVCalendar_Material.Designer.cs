namespace  GroupM.Minder
{
    partial class MPA008_TVCalendar_Material
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
            this.gvDetail = new System.Windows.Forms.DataGridView();
            this.chkCheckAll = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnUncheck = new System.Windows.Forms.Button();
            this.btnReRunMaterail = new System.Windows.Forms.Button();
            this.BuyingBriefID = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDetail
            // 
            this.gvDetail.AllowUserToAddRows = false;
            this.gvDetail.AllowUserToDeleteRows = false;
            this.gvDetail.AllowUserToResizeRows = false;
            this.gvDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BuyingBriefID,
            this.Column2,
            this.Column9,
            this.colM,
            this.colMaterialName,
            this.colMaterialCode});
            this.gvDetail.Location = new System.Drawing.Point(1, 0);
            this.gvDetail.MultiSelect = false;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.RowHeadersVisible = false;
            this.gvDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDetail.Size = new System.Drawing.Size(703, 387);
            this.gvDetail.TabIndex = 13;
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCheckAll.Location = new System.Drawing.Point(12, 397);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Size = new System.Drawing.Size(73, 23);
            this.chkCheckAll.TabIndex = 17;
            this.chkCheckAll.Text = "Check All";
            this.chkCheckAll.UseVisualStyleBackColor = true;
            this.chkCheckAll.Click += new System.EventHandler(this.chkCheckAll_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(559, 397);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Generate Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnUncheck
            // 
            this.btnUncheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUncheck.Location = new System.Drawing.Point(91, 397);
            this.btnUncheck.Name = "btnUncheck";
            this.btnUncheck.Size = new System.Drawing.Size(73, 23);
            this.btnUncheck.TabIndex = 17;
            this.btnUncheck.Text = "Uncheck All";
            this.btnUncheck.UseVisualStyleBackColor = true;
            this.btnUncheck.Click += new System.EventHandler(this.btnUncheck_Click);
            // 
            // btnReRunMaterail
            // 
            this.btnReRunMaterail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReRunMaterail.Location = new System.Drawing.Point(432, 397);
            this.btnReRunMaterail.Name = "btnReRunMaterail";
            this.btnReRunMaterail.Size = new System.Drawing.Size(121, 23);
            this.btnReRunMaterail.TabIndex = 19;
            this.btnReRunMaterail.Text = "Re-Run Material Key";
            this.btnReRunMaterail.UseVisualStyleBackColor = true;
            this.btnReRunMaterail.Click += new System.EventHandler(this.btnReRunMaterail_Click);
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
            this.Column2.DataPropertyName = "Buying_Brief_ID";
            this.Column2.HeaderText = "BuyingBriefID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "Description";
            this.Column9.HeaderText = "CampaignName";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 225;
            // 
            // colM
            // 
            this.colM.DataPropertyName = "MaterialKey";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.colM.DefaultCellStyle = dataGridViewCellStyle1;
            this.colM.HeaderText = "M";
            this.colM.Name = "colM";
            this.colM.Width = 30;
            // 
            // colMaterialName
            // 
            this.colMaterialName.DataPropertyName = "MaterialName";
            this.colMaterialName.HeaderText = "MaterialName";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.ReadOnly = true;
            this.colMaterialName.Width = 200;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.DataPropertyName = "Material_ID";
            this.colMaterialCode.HeaderText = "MaterialCode";
            this.colMaterialCode.Name = "colMaterialCode";
            // 
            // MPA008_TVCalendar_Material
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 428);
            this.Controls.Add(this.btnReRunMaterail);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnUncheck);
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.gvDetail);
            this.Name = "MPA008_TVCalendar_Material";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Material.";
            this.Load += new System.EventHandler(this.MPA008_TVCalendar_Material_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button chkCheckAll;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.DataGridView gvDetail;
        private System.Windows.Forms.Button btnUncheck;
        private System.Windows.Forms.Button btnReRunMaterail;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BuyingBriefID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn colM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
    }
}