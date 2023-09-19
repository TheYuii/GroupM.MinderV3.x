namespace GroupM.App.ExportPivotTable
{
    partial class ExportPivot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportPivot));
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExportTVBuyingPattern = new DevExpress.XtraEditors.SimpleButton();
            this.btnBrowseDetail = new DevExpress.XtraEditors.SimpleButton();
            this.txtPathFileHeader = new System.Windows.Forms.TextBox();
            this.txtPathFileDetial = new System.Windows.Forms.TextBox();
            this.btnBrowseHeader = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.RoyalBlue;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseForeColor = true;
            this.simpleButton1.Location = new System.Drawing.Point(22, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(268, 188);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Export Pivot Excel";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExportTVBuyingPattern);
            this.groupBox1.Controls.Add(this.btnBrowseDetail);
            this.groupBox1.Controls.Add(this.txtPathFileHeader);
            this.groupBox1.Controls.Add(this.txtPathFileDetial);
            this.groupBox1.Controls.Add(this.btnBrowseHeader);
            this.groupBox1.Location = new System.Drawing.Point(296, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 188);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export TV Buying Pattern";
            // 
            // btnExportTVBuyingPattern
            // 
            this.btnExportTVBuyingPattern.Appearance.Font = new System.Drawing.Font("Tw Cen MT Condensed", 16F, System.Drawing.FontStyle.Bold);
            this.btnExportTVBuyingPattern.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnExportTVBuyingPattern.Appearance.Options.UseFont = true;
            this.btnExportTVBuyingPattern.Appearance.Options.UseForeColor = true;
            this.btnExportTVBuyingPattern.Location = new System.Drawing.Point(22, 25);
            this.btnExportTVBuyingPattern.Name = "btnExportTVBuyingPattern";
            this.btnExportTVBuyingPattern.Size = new System.Drawing.Size(496, 56);
            this.btnExportTVBuyingPattern.TabIndex = 1;
            this.btnExportTVBuyingPattern.Text = "Export TV Buying Pattern";
            this.btnExportTVBuyingPattern.Click += new System.EventHandler(this.btnExportTVBuyingPattern_Click);
            // 
            // btnBrowseDetail
            // 
            this.btnBrowseDetail.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseDetail.Image")));
            this.btnBrowseDetail.Location = new System.Drawing.Point(364, 134);
            this.btnBrowseDetail.Name = "btnBrowseDetail";
            this.btnBrowseDetail.Size = new System.Drawing.Size(154, 34);
            this.btnBrowseDetail.TabIndex = 5;
            this.btnBrowseDetail.Text = "Browse Detail . . . ";
            this.btnBrowseDetail.Click += new System.EventHandler(this.btnBrowseDetail_Click);
            // 
            // txtPathFileHeader
            // 
            this.txtPathFileHeader.Location = new System.Drawing.Point(22, 101);
            this.txtPathFileHeader.Name = "txtPathFileHeader";
            this.txtPathFileHeader.Size = new System.Drawing.Size(336, 20);
            this.txtPathFileHeader.TabIndex = 2;
            // 
            // txtPathFileDetial
            // 
            this.txtPathFileDetial.Location = new System.Drawing.Point(22, 141);
            this.txtPathFileDetial.Name = "txtPathFileDetial";
            this.txtPathFileDetial.Size = new System.Drawing.Size(336, 20);
            this.txtPathFileDetial.TabIndex = 4;
            // 
            // btnBrowseHeader
            // 
            this.btnBrowseHeader.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseHeader.Image")));
            this.btnBrowseHeader.Location = new System.Drawing.Point(364, 94);
            this.btnBrowseHeader.Name = "btnBrowseHeader";
            this.btnBrowseHeader.Size = new System.Drawing.Size(154, 34);
            this.btnBrowseHeader.TabIndex = 3;
            this.btnBrowseHeader.Text = "Browse Header . . . ";
            this.btnBrowseHeader.Click += new System.EventHandler(this.btnBrowseHeader_Click);
            // 
            // ExportPivot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 212);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.simpleButton1);
            this.Name = "ExportPivot";
            this.Text = "ExportPivot";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnExportTVBuyingPattern;
        private DevExpress.XtraEditors.SimpleButton btnBrowseDetail;
        private System.Windows.Forms.TextBox txtPathFileHeader;
        private System.Windows.Forms.TextBox txtPathFileDetial;
        private DevExpress.XtraEditors.SimpleButton btnBrowseHeader;
    }
}