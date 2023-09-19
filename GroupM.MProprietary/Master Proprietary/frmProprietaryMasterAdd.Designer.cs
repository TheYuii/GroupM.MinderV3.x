namespace MProprietary
{
    partial class frmProprietaryMasterAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProprietaryMasterAdd));
            this.lblVendorID = new System.Windows.Forms.Label();
            this.txtVendorID = new System.Windows.Forms.TextBox();
            this.lblVendorName = new System.Windows.Forms.Label();
            this.txtVendorName = new System.Windows.Forms.TextBox();
            this.lblMediaID = new System.Windows.Forms.Label();
            this.txtMediaID = new System.Windows.Forms.TextBox();
            this.btnAddVendor = new System.Windows.Forms.Button();
            this.btnAddMedia = new System.Windows.Forms.Button();
            this.grpVendor = new System.Windows.Forms.GroupBox();
            this.grpMedia = new System.Windows.Forms.GroupBox();
            this.lblMediaName = new System.Windows.Forms.Label();
            this.txtMediaName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpVendor.SuspendLayout();
            this.grpMedia.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVendorID
            // 
            this.lblVendorID.AutoSize = true;
            this.lblVendorID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblVendorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblVendorID.Location = new System.Drawing.Point(6, 21);
            this.lblVendorID.Name = "lblVendorID";
            this.lblVendorID.Size = new System.Drawing.Size(55, 13);
            this.lblVendorID.TabIndex = 0;
            this.lblVendorID.Text = "Vendor ID";
            // 
            // txtVendorID
            // 
            this.txtVendorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(255)))));
            this.txtVendorID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtVendorID.Location = new System.Drawing.Point(84, 17);
            this.txtVendorID.Name = "txtVendorID";
            this.txtVendorID.ReadOnly = true;
            this.txtVendorID.Size = new System.Drawing.Size(100, 20);
            this.txtVendorID.TabIndex = 1;
            this.txtVendorID.Click += new System.EventHandler(this.txtVendorID_Click);
            // 
            // lblVendorName
            // 
            this.lblVendorName.AutoSize = true;
            this.lblVendorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblVendorName.Location = new System.Drawing.Point(6, 47);
            this.lblVendorName.Name = "lblVendorName";
            this.lblVendorName.Size = new System.Drawing.Size(72, 13);
            this.lblVendorName.TabIndex = 3;
            this.lblVendorName.Text = "Vendor Name";
            // 
            // txtVendorName
            // 
            this.txtVendorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtVendorName.Location = new System.Drawing.Point(84, 43);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.ReadOnly = true;
            this.txtVendorName.Size = new System.Drawing.Size(332, 20);
            this.txtVendorName.TabIndex = 4;
            // 
            // lblMediaID
            // 
            this.lblMediaID.AutoSize = true;
            this.lblMediaID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblMediaID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblMediaID.Location = new System.Drawing.Point(6, 22);
            this.lblMediaID.Name = "lblMediaID";
            this.lblMediaID.Size = new System.Drawing.Size(50, 13);
            this.lblMediaID.TabIndex = 0;
            this.lblMediaID.Text = "Media ID";
            // 
            // txtMediaID
            // 
            this.txtMediaID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(255)))));
            this.txtMediaID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtMediaID.Location = new System.Drawing.Point(84, 18);
            this.txtMediaID.Name = "txtMediaID";
            this.txtMediaID.ReadOnly = true;
            this.txtMediaID.Size = new System.Drawing.Size(100, 20);
            this.txtMediaID.TabIndex = 1;
            this.txtMediaID.Click += new System.EventHandler(this.txtMediaID_Click);
            // 
            // btnAddVendor
            // 
            this.btnAddVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnAddVendor.Image = global::MProprietary.Properties.Resources.btnAdd;
            this.btnAddVendor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddVendor.Location = new System.Drawing.Point(190, 16);
            this.btnAddVendor.Name = "btnAddVendor";
            this.btnAddVendor.Size = new System.Drawing.Size(69, 23);
            this.btnAddVendor.TabIndex = 2;
            this.btnAddVendor.Text = "Browse";
            this.btnAddVendor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddVendor.UseVisualStyleBackColor = true;
            this.btnAddVendor.Click += new System.EventHandler(this.btnAddVendor_Click);
            // 
            // btnAddMedia
            // 
            this.btnAddMedia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnAddMedia.Image = global::MProprietary.Properties.Resources.btnAdd;
            this.btnAddMedia.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddMedia.Location = new System.Drawing.Point(190, 17);
            this.btnAddMedia.Name = "btnAddMedia";
            this.btnAddMedia.Size = new System.Drawing.Size(69, 23);
            this.btnAddMedia.TabIndex = 2;
            this.btnAddMedia.Text = "Browse";
            this.btnAddMedia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddMedia.UseVisualStyleBackColor = true;
            this.btnAddMedia.Click += new System.EventHandler(this.btnAddMedia_Click);
            // 
            // grpVendor
            // 
            this.grpVendor.BackColor = System.Drawing.SystemColors.Control;
            this.grpVendor.Controls.Add(this.lblVendorID);
            this.grpVendor.Controls.Add(this.txtVendorID);
            this.grpVendor.Controls.Add(this.btnAddVendor);
            this.grpVendor.Controls.Add(this.lblVendorName);
            this.grpVendor.Controls.Add(this.txtVendorName);
            this.grpVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.grpVendor.Location = new System.Drawing.Point(12, 12);
            this.grpVendor.Name = "grpVendor";
            this.grpVendor.Size = new System.Drawing.Size(432, 77);
            this.grpVendor.TabIndex = 0;
            this.grpVendor.TabStop = false;
            this.grpVendor.Text = "Vendor";
            // 
            // grpMedia
            // 
            this.grpMedia.BackColor = System.Drawing.SystemColors.Control;
            this.grpMedia.Controls.Add(this.lblMediaName);
            this.grpMedia.Controls.Add(this.txtMediaName);
            this.grpMedia.Controls.Add(this.lblMediaID);
            this.grpMedia.Controls.Add(this.txtMediaID);
            this.grpMedia.Controls.Add(this.btnAddMedia);
            this.grpMedia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.grpMedia.Location = new System.Drawing.Point(12, 95);
            this.grpMedia.Name = "grpMedia";
            this.grpMedia.Size = new System.Drawing.Size(432, 79);
            this.grpMedia.TabIndex = 1;
            this.grpMedia.TabStop = false;
            this.grpMedia.Text = "Media";
            // 
            // lblMediaName
            // 
            this.lblMediaName.AutoSize = true;
            this.lblMediaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblMediaName.Location = new System.Drawing.Point(6, 49);
            this.lblMediaName.Name = "lblMediaName";
            this.lblMediaName.Size = new System.Drawing.Size(67, 13);
            this.lblMediaName.TabIndex = 5;
            this.lblMediaName.Text = "Media Name";
            // 
            // txtMediaName
            // 
            this.txtMediaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.txtMediaName.Location = new System.Drawing.Point(84, 45);
            this.txtMediaName.Name = "txtMediaName";
            this.txtMediaName.ReadOnly = true;
            this.txtMediaName.Size = new System.Drawing.Size(332, 20);
            this.txtMediaName.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::MProprietary.Properties.Resources.btnSave;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(385, 180);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmProprietaryMasterAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(456, 213);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpMedia);
            this.Controls.Add(this.grpVendor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProprietaryMasterAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add new proprietary media";
            this.grpVendor.ResumeLayout(false);
            this.grpVendor.PerformLayout();
            this.grpMedia.ResumeLayout(false);
            this.grpMedia.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblVendorID;
        private System.Windows.Forms.TextBox txtVendorID;
        private System.Windows.Forms.Label lblVendorName;
        private System.Windows.Forms.TextBox txtVendorName;
        private System.Windows.Forms.Label lblMediaID;
        private System.Windows.Forms.TextBox txtMediaID;
        private System.Windows.Forms.Button btnAddVendor;
        private System.Windows.Forms.Button btnAddMedia;
        private System.Windows.Forms.GroupBox grpVendor;
        private System.Windows.Forms.GroupBox grpMedia;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMediaName;
        private System.Windows.Forms.TextBox txtMediaName;
    }
}