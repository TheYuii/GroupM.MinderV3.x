namespace GroupM.Minder
{
    partial class AutoGrant_Reject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoGrant_Reject));
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdReason1 = new System.Windows.Forms.RadioButton();
            this.rdReason2 = new System.Windows.Forms.RadioButton();
            this.rdOther = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txtReason
            // 
            this.txtReason.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReason.Location = new System.Drawing.Point(12, 81);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(341, 72);
            this.txtReason.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(170, 159);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(89, 21);
            this.btnSubmit.TabIndex = 32;
            this.btnSubmit.Text = "✓  SUBMIT";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(265, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 21);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "✗  CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdReason1
            // 
            this.rdReason1.AutoSize = true;
            this.rdReason1.Location = new System.Drawing.Point(12, 12);
            this.rdReason1.Name = "rdReason1";
            this.rdReason1.Size = new System.Drawing.Size(121, 17);
            this.rdReason1.TabIndex = 34;
            this.rdReason1.TabStop = true;
            this.rdReason1.Text = "Reason 1 (เหตุผล 1)";
            this.rdReason1.UseVisualStyleBackColor = true;
            this.rdReason1.CheckedChanged += new System.EventHandler(this.rdReason1_CheckedChanged);
            // 
            // rdReason2
            // 
            this.rdReason2.AutoSize = true;
            this.rdReason2.Location = new System.Drawing.Point(12, 35);
            this.rdReason2.Name = "rdReason2";
            this.rdReason2.Size = new System.Drawing.Size(121, 17);
            this.rdReason2.TabIndex = 35;
            this.rdReason2.TabStop = true;
            this.rdReason2.Text = "Reason 2 (เหตุผล 2)";
            this.rdReason2.UseVisualStyleBackColor = true;
            this.rdReason2.CheckedChanged += new System.EventHandler(this.rdReason2_CheckedChanged);
            // 
            // rdOther
            // 
            this.rdOther.AutoSize = true;
            this.rdOther.Location = new System.Drawing.Point(12, 58);
            this.rdOther.Name = "rdOther";
            this.rdOther.Size = new System.Drawing.Size(128, 17);
            this.rdOther.TabIndex = 36;
            this.rdOther.TabStop = true;
            this.rdOther.Text = "Other - Please specify";
            this.rdOther.UseVisualStyleBackColor = true;
            this.rdOther.CheckedChanged += new System.EventHandler(this.rdOther_CheckedChanged);
            // 
            // AutoGrant_Reject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 192);
            this.Controls.Add(this.rdOther);
            this.Controls.Add(this.rdReason2);
            this.Controls.Add(this.rdReason1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtReason);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AutoGrant_Reject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reject Reason";
            this.Load += new System.EventHandler(this.AutoGrant_Reject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdReason1;
        private System.Windows.Forms.RadioButton rdReason2;
        private System.Windows.Forms.RadioButton rdOther;
    }
}