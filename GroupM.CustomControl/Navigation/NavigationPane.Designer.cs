namespace GroupM.CustomControl.Navigation
{
    partial class NavigationPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.pnlItem = new System.Windows.Forms.Panel();
            this.pnlTreeMenu = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnHeader = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnlNavigation.SuspendLayout();
            this.pnlItem.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.Controls.Add(this.pnlItem);
            this.pnlNavigation.Controls.Add(this.pnlHeader);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNavigation.Location = new System.Drawing.Point(1, 1);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(248, 498);
            this.pnlNavigation.TabIndex = 4;
            // 
            // pnlItem
            // 
            this.pnlItem.Controls.Add(this.pnlTreeMenu);
            this.pnlItem.Controls.Add(this.pnlFooter);
            this.pnlItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlItem.Location = new System.Drawing.Point(0, 25);
            this.pnlItem.Name = "pnlItem";
            this.pnlItem.Size = new System.Drawing.Size(248, 473);
            this.pnlItem.TabIndex = 3;
            // 
            // pnlTreeMenu
            // 
            this.pnlTreeMenu.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlTreeMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlTreeMenu.Name = "pnlTreeMenu";
            this.pnlTreeMenu.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pnlTreeMenu.Size = new System.Drawing.Size(248, 473);
            this.pnlTreeMenu.TabIndex = 1;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 473);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(248, 0);
            this.pnlFooter.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlHeader.BackgroundImage = global::GroupM.CustomControl.Properties.Resources.GardientHeader;
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.btnHeader);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(248, 25);
            this.pnlHeader.TabIndex = 1;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // btnHeader
            // 
            this.btnHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHeader.BackColor = System.Drawing.Color.Transparent;
            this.btnHeader.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnHeader.FlatAppearance.BorderSize = 0;
            this.btnHeader.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnHeader.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeader.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnHeader.Image = global::GroupM.CustomControl.Properties.Resources.arrow_left;
            this.btnHeader.Location = new System.Drawing.Point(221, 1);
            this.btnHeader.Name = "btnHeader";
            this.btnHeader.Size = new System.Drawing.Size(25, 23);
            this.btnHeader.TabIndex = 1;
            this.btnHeader.Tag = "show";
            this.btnHeader.UseVisualStyleBackColor = false;
            this.btnHeader.Click += new System.EventHandler(this.btnHeader_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoEllipsis = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblHeader.Location = new System.Drawing.Point(15, 5);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(195, 15);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Title Text";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NavigationPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.pnlNavigation);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Name = "NavigationPane";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(250, 500);
            this.pnlNavigation.ResumeLayout(false);
            this.pnlItem.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Panel pnlItem;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlTreeMenu;

    }
}
