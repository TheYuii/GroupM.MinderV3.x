namespace GroupM.Minder
{
    partial class MediaInvestment_Opt_in_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaInvestment_Opt_in_Report));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtOptInEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtOptInStartDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cboProprietaryGroup = new System.Windows.Forms.ComboBox();
            this.txtAgency = new System.Windows.Forms.TextBox();
            this.cboAgencyCode = new System.Windows.Forms.ComboBox();
            this.cboRegion = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Opt-in Period :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Proprietary Group :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Agency :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Region :";
            // 
            // dtOptInEndDate
            // 
            this.dtOptInEndDate.CustomFormat = "dd/MM/yyyy";
            this.dtOptInEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtOptInEndDate.Location = new System.Drawing.Point(264, 14);
            this.dtOptInEndDate.Name = "dtOptInEndDate";
            this.dtOptInEndDate.Size = new System.Drawing.Size(120, 20);
            this.dtOptInEndDate.TabIndex = 3;
            // 
            // dtOptInStartDate
            // 
            this.dtOptInStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtOptInStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtOptInStartDate.Location = new System.Drawing.Point(113, 14);
            this.dtOptInStartDate.Name = "dtOptInStartDate";
            this.dtOptInStartDate.Size = new System.Drawing.Size(120, 20);
            this.dtOptInStartDate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "-";
            // 
            // cboProprietaryGroup
            // 
            this.cboProprietaryGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProprietaryGroup.FormattingEnabled = true;
            this.cboProprietaryGroup.Location = new System.Drawing.Point(113, 40);
            this.cboProprietaryGroup.Name = "cboProprietaryGroup";
            this.cboProprietaryGroup.Size = new System.Drawing.Size(271, 21);
            this.cboProprietaryGroup.TabIndex = 5;
            // 
            // txtAgency
            // 
            this.txtAgency.BackColor = System.Drawing.SystemColors.Control;
            this.txtAgency.Location = new System.Drawing.Point(188, 67);
            this.txtAgency.Name = "txtAgency";
            this.txtAgency.ReadOnly = true;
            this.txtAgency.Size = new System.Drawing.Size(196, 20);
            this.txtAgency.TabIndex = 8;
            // 
            // cboAgencyCode
            // 
            this.cboAgencyCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAgencyCode.FormattingEnabled = true;
            this.cboAgencyCode.Location = new System.Drawing.Point(113, 67);
            this.cboAgencyCode.Name = "cboAgencyCode";
            this.cboAgencyCode.Size = new System.Drawing.Size(69, 21);
            this.cboAgencyCode.TabIndex = 7;
            this.cboAgencyCode.SelectedIndexChanged += new System.EventHandler(this.cboAgencyCode_SelectedIndexChanged);
            // 
            // cboRegion
            // 
            this.cboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRegion.FormattingEnabled = true;
            this.cboRegion.Items.AddRange(new object[] {
            "All",
            "Local",
            "Global"});
            this.cboRegion.Location = new System.Drawing.Point(113, 94);
            this.cboRegion.Name = "cboRegion";
            this.cboRegion.Size = new System.Drawing.Size(271, 21);
            this.cboRegion.TabIndex = 10;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(228, 148);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(309, 148);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "All",
            "Active",
            "Inactive"});
            this.cboStatus.Location = new System.Drawing.Point(113, 121);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(271, 21);
            this.cboStatus.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Client Status :";
            // 
            // MediaInvestment_Opt_in_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 189);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cboRegion);
            this.Controls.Add(this.cboAgencyCode);
            this.Controls.Add(this.txtAgency);
            this.Controls.Add(this.cboProprietaryGroup);
            this.Controls.Add(this.dtOptInEndDate);
            this.Controls.Add(this.dtOptInStartDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MediaInvestment_Opt_in_Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opt-in Report";
            this.Load += new System.EventHandler(this.MediaInvestment_Opt_in_Report_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.DateTimePicker dtOptInEndDate;
        public System.Windows.Forms.DateTimePicker dtOptInStartDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboProprietaryGroup;
        private System.Windows.Forms.TextBox txtAgency;
        private System.Windows.Forms.ComboBox cboAgencyCode;
        private System.Windows.Forms.ComboBox cboRegion;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label6;
    }
}