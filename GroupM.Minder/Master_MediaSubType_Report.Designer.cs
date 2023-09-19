namespace GroupM.Minder
{
    partial class Master_MediaSubType_Report
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Master_MediaSubType_Report));
            this.cboMediaTypeCode = new System.Windows.Forms.ComboBox();
            this.txtMediaType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboMediaTypeCode
            // 
            this.cboMediaTypeCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMediaTypeCode.FormattingEnabled = true;
            this.cboMediaTypeCode.Location = new System.Drawing.Point(92, 23);
            this.cboMediaTypeCode.Name = "cboMediaTypeCode";
            this.cboMediaTypeCode.Size = new System.Drawing.Size(69, 21);
            this.cboMediaTypeCode.TabIndex = 10;
            this.cboMediaTypeCode.SelectedIndexChanged += new System.EventHandler(this.cboMediaTypeCode_SelectedIndexChanged);
            // 
            // txtMediaType
            // 
            this.txtMediaType.BackColor = System.Drawing.SystemColors.Control;
            this.txtMediaType.Location = new System.Drawing.Point(167, 23);
            this.txtMediaType.Name = "txtMediaType";
            this.txtMediaType.ReadOnly = true;
            this.txtMediaType.Size = new System.Drawing.Size(196, 20);
            this.txtMediaType.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Media Type :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(288, 54);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(207, 54);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 13;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // Master_MediaSubType_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 101);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cboMediaTypeCode);
            this.Controls.Add(this.txtMediaType);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Master_MediaSubType_Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Sub Type Business Definition";
            this.Load += new System.EventHandler(this.Master_MediaSubType_Report_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboMediaTypeCode;
        private System.Windows.Forms.TextBox txtMediaType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
    }
}