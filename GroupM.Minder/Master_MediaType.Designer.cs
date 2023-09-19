namespace GroupM.Minder
{
    partial class Master_MediaType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Master_MediaType));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMediaTypeCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.txtMasterMediaTypeCode = new System.Windows.Forms.TextBox();
            this.txtMasterMediaType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rdInactive = new System.Windows.Forms.RadioButton();
            this.rdActive = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dtInactiveDate = new System.Windows.Forms.DateTimePicker();
            this.chkMaster = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboMediaTypeGroup = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.cancelToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(513, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::GroupM.Minder.Properties.Resources.save_alt;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.ToolTipText = "Save (Ctrl + S)";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Image = global::GroupM.Minder.Properties.Resources.delete_alt;
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.cancelToolStripMenuItem.Text = "Cancel";
            this.cancelToolStripMenuItem.ToolTipText = "Cancel (Esc)";
            this.cancelToolStripMenuItem.Click += new System.EventHandler(this.CancelToolStripMenuItem_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.Red;
            this.label29.Location = new System.Drawing.Point(109, 68);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(11, 13);
            this.label29.TabIndex = 4;
            this.label29.Text = "*";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.Color.Red;
            this.label28.Location = new System.Drawing.Point(109, 42);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(11, 13);
            this.label28.TabIndex = 1;
            this.label28.Text = "*";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(124, 64);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(370, 20);
            this.txtDisplayName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Display Name";
            // 
            // txtMediaTypeCode
            // 
            this.txtMediaTypeCode.Location = new System.Drawing.Point(124, 38);
            this.txtMediaTypeCode.MaxLength = 2;
            this.txtMediaTypeCode.Name = "txtMediaTypeCode";
            this.txtMediaTypeCode.Size = new System.Drawing.Size(69, 20);
            this.txtMediaTypeCode.TabIndex = 2;
            this.txtMediaTypeCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtMediaTypeCode_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Media Type Code";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.Red;
            this.label32.Location = new System.Drawing.Point(106, 144);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(11, 13);
            this.label32.TabIndex = 10;
            this.label32.Text = "*";
            // 
            // txtMasterMediaTypeCode
            // 
            this.txtMasterMediaTypeCode.Location = new System.Drawing.Point(422, 140);
            this.txtMasterMediaTypeCode.Name = "txtMasterMediaTypeCode";
            this.txtMasterMediaTypeCode.ReadOnly = true;
            this.txtMasterMediaTypeCode.Size = new System.Drawing.Size(69, 20);
            this.txtMasterMediaTypeCode.TabIndex = 12;
            // 
            // txtMasterMediaType
            // 
            this.txtMasterMediaType.BackColor = System.Drawing.Color.AntiqueWhite;
            this.txtMasterMediaType.Location = new System.Drawing.Point(121, 140);
            this.txtMasterMediaType.Name = "txtMasterMediaType";
            this.txtMasterMediaType.Size = new System.Drawing.Size(295, 20);
            this.txtMasterMediaType.TabIndex = 11;
            this.txtMasterMediaType.Click += new System.EventHandler(this.TxtMediaType_Click);
            this.txtMasterMediaType.TextChanged += new System.EventHandler(this.TxtMediaType_TextChanged);
            this.txtMasterMediaType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtMediaType_KeyPress);
            this.txtMasterMediaType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtMediaType_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Master Media Type";
            // 
            // rdInactive
            // 
            this.rdInactive.AutoSize = true;
            this.rdInactive.Location = new System.Drawing.Point(187, 195);
            this.rdInactive.Name = "rdInactive";
            this.rdInactive.Size = new System.Drawing.Size(63, 17);
            this.rdInactive.TabIndex = 18;
            this.rdInactive.Text = "Inactive";
            this.rdInactive.UseVisualStyleBackColor = true;
            this.rdInactive.CheckedChanged += new System.EventHandler(this.RdInactive_CheckedChanged);
            // 
            // rdActive
            // 
            this.rdActive.AutoSize = true;
            this.rdActive.Checked = true;
            this.rdActive.Location = new System.Drawing.Point(121, 195);
            this.rdActive.Name = "rdActive";
            this.rdActive.Size = new System.Drawing.Size(55, 17);
            this.rdActive.TabIndex = 17;
            this.rdActive.TabStop = true;
            this.rdActive.Text = "Active";
            this.rdActive.UseVisualStyleBackColor = true;
            this.rdActive.CheckedChanged += new System.EventHandler(this.RdActive_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Status";
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::GroupM.Minder.Properties.Resources.delete1;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(12, 223);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(83, 23);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "DELETE";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // dtInactiveDate
            // 
            this.dtInactiveDate.CustomFormat = "dd/MM/yyyy";
            this.dtInactiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtInactiveDate.Location = new System.Drawing.Point(256, 193);
            this.dtInactiveDate.Name = "dtInactiveDate";
            this.dtInactiveDate.Size = new System.Drawing.Size(97, 20);
            this.dtInactiveDate.TabIndex = 19;
            // 
            // chkMaster
            // 
            this.chkMaster.AutoSize = true;
            this.chkMaster.Location = new System.Drawing.Point(124, 116);
            this.chkMaster.Name = "chkMaster";
            this.chkMaster.Size = new System.Drawing.Size(117, 17);
            this.chkMaster.TabIndex = 8;
            this.chkMaster.Text = "Master Media Type";
            this.chkMaster.UseVisualStyleBackColor = true;
            this.chkMaster.CheckedChanged += new System.EventHandler(this.chkMaster_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(124, 90);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(370, 20);
            this.txtDescription.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(107, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "*";
            // 
            // cboMediaTypeGroup
            // 
            this.cboMediaTypeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMediaTypeGroup.FormattingEnabled = true;
            this.cboMediaTypeGroup.Location = new System.Drawing.Point(121, 166);
            this.cboMediaTypeGroup.Name = "cboMediaTypeGroup";
            this.cboMediaTypeGroup.Size = new System.Drawing.Size(295, 21);
            this.cboMediaTypeGroup.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Media Type Group";
            // 
            // Master_MediaType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 258);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboMediaTypeGroup);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkMaster);
            this.Controls.Add(this.dtInactiveDate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.rdInactive);
            this.Controls.Add(this.rdActive);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.txtMasterMediaTypeCode);
            this.Controls.Add(this.txtMasterMediaType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMediaTypeCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Master_MediaType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Type";
            this.Load += new System.EventHandler(this.Master_MediaType_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMediaTypeCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtMasterMediaTypeCode;
        private System.Windows.Forms.TextBox txtMasterMediaType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdInactive;
        private System.Windows.Forms.RadioButton rdActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDelete;
        public System.Windows.Forms.DateTimePicker dtInactiveDate;
        private System.Windows.Forms.CheckBox chkMaster;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboMediaTypeGroup;
        private System.Windows.Forms.Label label6;
    }
}