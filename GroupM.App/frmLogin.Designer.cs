namespace GroupM.App
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.pnlInformation = new System.Windows.Forms.Panel();
            this.llblSupportEmail = new System.Windows.Forms.LinkLabel();
            this.llblPublishLocation = new System.Windows.Forms.LinkLabel();
            this.lblPublishVersion = new System.Windows.Forms.TextBox();
            this.lblServiceAddress = new System.Windows.Forms.TextBox();
            this.lblDatabaseAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblInformation = new System.Windows.Forms.LinkLabel();
            this.pnlSpliter = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.btnHide = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlInformation.SuspendLayout();
            this.pnlSpliter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Location = new System.Drawing.Point(449, 221);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogIn
            // 
            this.btnLogIn.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnLogIn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogIn.BackgroundImage")));
            this.btnLogIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogIn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnLogIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnLogIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnLogIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogIn.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnLogIn.ForeColor = System.Drawing.Color.Black;
            this.btnLogIn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogIn.Location = new System.Drawing.Point(361, 221);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(80, 30);
            this.btnLogIn.TabIndex = 2;
            this.btnLogIn.Text = "OK";
            this.btnLogIn.UseVisualStyleBackColor = false;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(66, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 44);
            this.label2.TabIndex = 6;
            this.label2.Text = "Minder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(322, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Login to Minder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(321, 182);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(208, 22);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(321, 144);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(208, 22);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // pnlInformation
            // 
            this.pnlInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInformation.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlInformation.Controls.Add(this.llblSupportEmail);
            this.pnlInformation.Controls.Add(this.llblPublishLocation);
            this.pnlInformation.Controls.Add(this.lblPublishVersion);
            this.pnlInformation.Controls.Add(this.lblServiceAddress);
            this.pnlInformation.Controls.Add(this.lblDatabaseAddress);
            this.pnlInformation.Controls.Add(this.label8);
            this.pnlInformation.Controls.Add(this.label7);
            this.pnlInformation.Controls.Add(this.label6);
            this.pnlInformation.Controls.Add(this.label5);
            this.pnlInformation.Controls.Add(this.label3);
            this.pnlInformation.Location = new System.Drawing.Point(0, 367);
            this.pnlInformation.Name = "pnlInformation";
            this.pnlInformation.Size = new System.Drawing.Size(584, 155);
            this.pnlInformation.TabIndex = 18;
            // 
            // llblSupportEmail
            // 
            this.llblSupportEmail.Location = new System.Drawing.Point(134, 129);
            this.llblSupportEmail.Name = "llblSupportEmail";
            this.llblSupportEmail.Size = new System.Drawing.Size(416, 20);
            this.llblSupportEmail.TabIndex = 3;
            this.llblSupportEmail.TabStop = true;
            this.llblSupportEmail.Text = "support@groupm.com";
            // 
            // llblPublishLocation
            // 
            this.llblPublishLocation.Location = new System.Drawing.Point(134, 100);
            this.llblPublishLocation.Name = "llblPublishLocation";
            this.llblPublishLocation.Size = new System.Drawing.Size(416, 20);
            this.llblPublishLocation.TabIndex = 3;
            this.llblPublishLocation.TabStop = true;
            this.llblPublishLocation.Text = "http://www.google.co.th";
            // 
            // lblPublishVersion
            // 
            this.lblPublishVersion.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblPublishVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblPublishVersion.Location = new System.Drawing.Point(137, 69);
            this.lblPublishVersion.Multiline = true;
            this.lblPublishVersion.Name = "lblPublishVersion";
            this.lblPublishVersion.Size = new System.Drawing.Size(413, 20);
            this.lblPublishVersion.TabIndex = 2;
            this.lblPublishVersion.Text = "1.0.0.0";
            // 
            // lblServiceAddress
            // 
            this.lblServiceAddress.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblServiceAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblServiceAddress.Location = new System.Drawing.Point(137, 38);
            this.lblServiceAddress.Multiline = true;
            this.lblServiceAddress.Name = "lblServiceAddress";
            this.lblServiceAddress.Size = new System.Drawing.Size(413, 20);
            this.lblServiceAddress.TabIndex = 2;
            this.lblServiceAddress.Text = "http://bkkappp01102.ad.insidemedia.net:8088/WEBSERVICE/Services/Framework.svc";
            // 
            // lblDatabaseAddress
            // 
            this.lblDatabaseAddress.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblDatabaseAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblDatabaseAddress.Location = new System.Drawing.Point(137, 8);
            this.lblDatabaseAddress.Multiline = true;
            this.lblDatabaseAddress.Name = "lblDatabaseAddress";
            this.lblDatabaseAddress.Size = new System.Drawing.Size(413, 20);
            this.lblDatabaseAddress.TabIndex = 2;
            this.lblDatabaseAddress.Text = "Server=BKKSQLP01102\\SQLINS01_2008R2;DataBase=MatrixDB;Version=2.1.7.007";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(15, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Support Email:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(15, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "Publish Location:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(15, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Publish Version:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(15, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Service Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(15, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Database Address:";
            // 
            // lblInformation
            // 
            this.lblInformation.ActiveLinkColor = System.Drawing.SystemColors.ControlDark;
            this.lblInformation.AutoSize = true;
            this.lblInformation.BackColor = System.Drawing.Color.Transparent;
            this.lblInformation.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblInformation.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblInformation.LinkColor = System.Drawing.SystemColors.ControlDark;
            this.lblInformation.Location = new System.Drawing.Point(26, 316);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(155, 16);
            this.lblInformation.TabIndex = 5;
            this.lblInformation.TabStop = true;
            this.lblInformation.Text = "View Support Information";
            this.lblInformation.Click += new System.EventHandler(this.lblInformation_Click);
            // 
            // pnlSpliter
            // 
            this.pnlSpliter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSpliter.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlSpliter.BackgroundImage = global::GroupM.App.Properties.Resources.bg;
            this.pnlSpliter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlSpliter.Controls.Add(this.label9);
            this.pnlSpliter.Controls.Add(this.btnHide);
            this.pnlSpliter.Controls.Add(this.label4);
            this.pnlSpliter.Location = new System.Drawing.Point(0, 337);
            this.pnlSpliter.Name = "pnlSpliter";
            this.pnlSpliter.Size = new System.Drawing.Size(584, 30);
            this.pnlSpliter.TabIndex = 8;
            this.pnlSpliter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSpliter_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(15, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 16);
            this.label9.TabIndex = 19;
            this.label9.Text = "Dianostic Information";
            // 
            // btnHide
            // 
            this.btnHide.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnHide.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHide.BackgroundImage")));
            this.btnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHide.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnHide.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnHide.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnHide.ForeColor = System.Drawing.Color.Black;
            this.btnHide.Image = ((System.Drawing.Image)(resources.GetObject("btnHide.Image")));
            this.btnHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHide.Location = new System.Drawing.Point(519, 2);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(62, 25);
            this.btnHide.TabIndex = 18;
            this.btnHide.Text = "Hide";
            this.btnHide.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "View Support Information";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GroupM.App.Properties.Resources.bg_login;
            this.ClientSize = new System.Drawing.Size(584, 337);
            this.Controls.Add(this.pnlSpliter);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this.pnlInformation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogIn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Minder";
            this.Load += new System.EventHandler(this.Login_Load);
            this.pnlInformation.ResumeLayout(false);
            this.pnlInformation.PerformLayout();
            this.pnlSpliter.ResumeLayout(false);
            this.pnlSpliter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtPassword;
        public System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Panel pnlInformation;
        private System.Windows.Forms.LinkLabel lblInformation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlSpliter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox lblPublishVersion;
        private System.Windows.Forms.TextBox lblServiceAddress;
        private System.Windows.Forms.TextBox lblDatabaseAddress;
        private System.Windows.Forms.LinkLabel llblSupportEmail;
        private System.Windows.Forms.LinkLabel llblPublishLocation;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Label label9;

    }
}