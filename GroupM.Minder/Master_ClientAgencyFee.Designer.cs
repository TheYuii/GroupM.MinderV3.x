namespace GroupM.Minder
{
    partial class Master_ClientAgencyFee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Master_ClientAgencyFee));
            this.btnAdd = new System.Windows.Forms.Button();
            this.rdMediaType = new System.Windows.Forms.RadioButton();
            this.cboMediaType = new System.Windows.Forms.ComboBox();
            this.rdMediaSubType = new System.Windows.Forms.RadioButton();
            this.cboMediaSubType = new System.Windows.Forms.ComboBox();
            this.rdOutdoorCostType = new System.Windows.Forms.RadioButton();
            this.cboOutdoorCostType = new System.Windows.Forms.ComboBox();
            this.rdXaxis = new System.Windows.Forms.RadioButton();
            this.rdINCA = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(354, 146);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 28);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // rdMediaType
            // 
            this.rdMediaType.AutoSize = true;
            this.rdMediaType.Checked = true;
            this.rdMediaType.Location = new System.Drawing.Point(9, 14);
            this.rdMediaType.Name = "rdMediaType";
            this.rdMediaType.Size = new System.Drawing.Size(103, 21);
            this.rdMediaType.TabIndex = 7;
            this.rdMediaType.TabStop = true;
            this.rdMediaType.Text = "Media Type";
            this.rdMediaType.UseVisualStyleBackColor = true;
            this.rdMediaType.CheckedChanged += new System.EventHandler(this.rdMediaType_CheckedChanged);
            // 
            // cboMediaType
            // 
            this.cboMediaType.FormattingEnabled = true;
            this.cboMediaType.Location = new System.Drawing.Point(160, 12);
            this.cboMediaType.Name = "cboMediaType";
            this.cboMediaType.Size = new System.Drawing.Size(300, 24);
            this.cboMediaType.TabIndex = 8;
            // 
            // rdMediaSubType
            // 
            this.rdMediaSubType.AutoSize = true;
            this.rdMediaSubType.Location = new System.Drawing.Point(9, 44);
            this.rdMediaSubType.Name = "rdMediaSubType";
            this.rdMediaSubType.Size = new System.Drawing.Size(132, 21);
            this.rdMediaSubType.TabIndex = 7;
            this.rdMediaSubType.Text = "Media Sub Type";
            this.rdMediaSubType.UseVisualStyleBackColor = true;
            this.rdMediaSubType.CheckedChanged += new System.EventHandler(this.rdMediaSubType_CheckedChanged);
            // 
            // cboMediaSubType
            // 
            this.cboMediaSubType.Enabled = false;
            this.cboMediaSubType.FormattingEnabled = true;
            this.cboMediaSubType.Location = new System.Drawing.Point(160, 42);
            this.cboMediaSubType.Name = "cboMediaSubType";
            this.cboMediaSubType.Size = new System.Drawing.Size(300, 24);
            this.cboMediaSubType.TabIndex = 8;
            // 
            // rdOutdoorCostType
            // 
            this.rdOutdoorCostType.AutoSize = true;
            this.rdOutdoorCostType.Location = new System.Drawing.Point(9, 74);
            this.rdOutdoorCostType.Name = "rdOutdoorCostType";
            this.rdOutdoorCostType.Size = new System.Drawing.Size(149, 21);
            this.rdOutdoorCostType.TabIndex = 7;
            this.rdOutdoorCostType.Text = "Outdoor Cost Type";
            this.rdOutdoorCostType.UseVisualStyleBackColor = true;
            this.rdOutdoorCostType.CheckedChanged += new System.EventHandler(this.rdOutdoorCostType_CheckedChanged);
            // 
            // cboOutdoorCostType
            // 
            this.cboOutdoorCostType.Enabled = false;
            this.cboOutdoorCostType.FormattingEnabled = true;
            this.cboOutdoorCostType.Items.AddRange(new object[] {
            "Cost of Advertising",
            "Cost of Electricity",
            "Cost of Installation",
            "Cost of Production",
            "Miscellaneous",
            "Others",
            "Signboard Tax"});
            this.cboOutdoorCostType.Location = new System.Drawing.Point(160, 72);
            this.cboOutdoorCostType.Name = "cboOutdoorCostType";
            this.cboOutdoorCostType.Size = new System.Drawing.Size(300, 24);
            this.cboOutdoorCostType.TabIndex = 8;
            // 
            // rdXaxis
            // 
            this.rdXaxis.AutoSize = true;
            this.rdXaxis.Location = new System.Drawing.Point(9, 102);
            this.rdXaxis.Name = "rdXaxis";
            this.rdXaxis.Size = new System.Drawing.Size(62, 21);
            this.rdXaxis.TabIndex = 7;
            this.rdXaxis.Text = "Xaxis";
            this.rdXaxis.UseVisualStyleBackColor = true;
            this.rdXaxis.CheckedChanged += new System.EventHandler(this.rdXaxis_CheckedChanged);
            // 
            // rdINCA
            // 
            this.rdINCA.AutoSize = true;
            this.rdINCA.Location = new System.Drawing.Point(9, 130);
            this.rdINCA.Name = "rdINCA";
            this.rdINCA.Size = new System.Drawing.Size(60, 21);
            this.rdINCA.TabIndex = 7;
            this.rdINCA.Text = "INCA";
            this.rdINCA.UseVisualStyleBackColor = true;
            this.rdINCA.CheckedChanged += new System.EventHandler(this.rdINCA_CheckedChanged);
            // 
            // Master_ClientAgencyFee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 186);
            this.Controls.Add(this.cboOutdoorCostType);
            this.Controls.Add(this.cboMediaSubType);
            this.Controls.Add(this.rdINCA);
            this.Controls.Add(this.rdXaxis);
            this.Controls.Add(this.rdOutdoorCostType);
            this.Controls.Add(this.rdMediaSubType);
            this.Controls.Add(this.cboMediaType);
            this.Controls.Add(this.rdMediaType);
            this.Controls.Add(this.btnAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Master_ClientAgencyFee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Specific Fee";
            this.Load += new System.EventHandler(this.Master_ClientAgencyFee_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.RadioButton rdMediaType;
        public System.Windows.Forms.ComboBox cboMediaType;
        public System.Windows.Forms.RadioButton rdMediaSubType;
        public System.Windows.Forms.ComboBox cboMediaSubType;
        public System.Windows.Forms.RadioButton rdOutdoorCostType;
        public System.Windows.Forms.ComboBox cboOutdoorCostType;
        public System.Windows.Forms.RadioButton rdXaxis;
        public System.Windows.Forms.RadioButton rdINCA;
    }
}