namespace GroupM.App.Setting.UserManagement
{
    partial class frmUserManagementInput
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserManagementInput));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabUserProfile = new System.Windows.Forms.TabControl();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.txtPassword = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtDomain = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.txtLastName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.txtFirstName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.txtDisplayName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.txtUserName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.lblActive = new System.Windows.Forms.Label();
            this.lblLastname = new System.Windows.Forms.Label();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.lblDomain = new System.Windows.Forms.Label();
            this.lblFirstname = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tabMember = new System.Windows.Forms.TabPage();
            this.btnSearchUserGroup = new GroupM.CustomControl.Common.CustomButton(this.components);
            this.gvUserGroup = new GroupM.CustomControl.Common.CustomDataGridView(this.components);
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colUserGroupMappingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserGroupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDescription = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.txtGroupName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tsbControl.SuspendLayout();
            this.tabUserProfile.SuspendLayout();
            this.tabUser.SuspendLayout();
            this.tabMember.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUserGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // tsbControl
            // 
            this.tsbControl.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tabUserProfile
            // 
            this.tabUserProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabUserProfile.Controls.Add(this.tabUser);
            this.tabUserProfile.Controls.Add(this.tabMember);
            this.tabUserProfile.Location = new System.Drawing.Point(20, 50);
            this.tabUserProfile.Name = "tabUserProfile";
            this.tabUserProfile.SelectedIndex = 0;
            this.tabUserProfile.Size = new System.Drawing.Size(750, 295);
            this.tabUserProfile.TabIndex = 0;
            // 
            // tabUser
            // 
            this.tabUser.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabUser.Controls.Add(this.txtPassword);
            this.tabUser.Controls.Add(this.label3);
            this.tabUser.Controls.Add(this.chkActive);
            this.tabUser.Controls.Add(this.txtDomain);
            this.tabUser.Controls.Add(this.txtLastName);
            this.tabUser.Controls.Add(this.txtFirstName);
            this.tabUser.Controls.Add(this.txtDisplayName);
            this.tabUser.Controls.Add(this.txtUserName);
            this.tabUser.Controls.Add(this.lblActive);
            this.tabUser.Controls.Add(this.lblLastname);
            this.tabUser.Controls.Add(this.lblDisplayName);
            this.tabUser.Controls.Add(this.lblDomain);
            this.tabUser.Controls.Add(this.lblFirstname);
            this.tabUser.Controls.Add(this.lblUsername);
            this.tabUser.Location = new System.Drawing.Point(4, 23);
            this.tabUser.Name = "tabUser";
            this.tabUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabUser.Size = new System.Drawing.Size(742, 268);
            this.tabUser.TabIndex = 0;
            this.tabUser.Text = "User";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.Window;
            this.txtPassword.IsRequireField = false;
            this.txtPassword.Location = new System.Drawing.Point(120, 79);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 22);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Local Password:";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(490, 114);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 5;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // txtDomain
            // 
            this.txtDomain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtDomain.IsRequireField = true;
            this.txtDomain.Location = new System.Drawing.Point(120, 112);
            this.txtDomain.MaxLength = 100;
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(200, 22);
            this.txtDomain.TabIndex = 4;
            this.txtDomain.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // txtLastName
            // 
            this.txtLastName.BackColor = System.Drawing.SystemColors.Window;
            this.txtLastName.IsRequireField = false;
            this.txtLastName.Location = new System.Drawing.Point(490, 47);
            this.txtLastName.MaxLength = 100;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(200, 22);
            this.txtLastName.TabIndex = 3;
            this.txtLastName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // txtFirstName
            // 
            this.txtFirstName.BackColor = System.Drawing.SystemColors.Window;
            this.txtFirstName.IsRequireField = false;
            this.txtFirstName.Location = new System.Drawing.Point(120, 47);
            this.txtFirstName.MaxLength = 100;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(200, 22);
            this.txtFirstName.TabIndex = 2;
            this.txtFirstName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.BackColor = System.Drawing.SystemColors.Window;
            this.txtDisplayName.IsRequireField = false;
            this.txtDisplayName.Location = new System.Drawing.Point(490, 17);
            this.txtDisplayName.MaxLength = 100;
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(200, 22);
            this.txtDisplayName.TabIndex = 1;
            this.txtDisplayName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtUserName.IsRequireField = true;
            this.txtUserName.Location = new System.Drawing.Point(120, 17);
            this.txtUserName.MaxLength = 100;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 22);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Location = new System.Drawing.Point(374, 115);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(45, 14);
            this.lblActive.TabIndex = 0;
            this.lblActive.Text = "Active:";
            // 
            // lblLastname
            // 
            this.lblLastname.AutoSize = true;
            this.lblLastname.Location = new System.Drawing.Point(374, 50);
            this.lblLastname.Name = "lblLastname";
            this.lblLastname.Size = new System.Drawing.Size(68, 14);
            this.lblLastname.TabIndex = 0;
            this.lblLastname.Text = "Last Name:";
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(375, 20);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(82, 14);
            this.lblDisplayName.TabIndex = 0;
            this.lblDisplayName.Text = "Display Name:";
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(25, 115);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(51, 14);
            this.lblDomain.TabIndex = 0;
            this.lblDomain.Text = "Domain:";
            // 
            // lblFirstname
            // 
            this.lblFirstname.AutoSize = true;
            this.lblFirstname.Location = new System.Drawing.Point(25, 50);
            this.lblFirstname.Name = "lblFirstname";
            this.lblFirstname.Size = new System.Drawing.Size(68, 14);
            this.lblFirstname.TabIndex = 0;
            this.lblFirstname.Text = "First Name:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(25, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(70, 14);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "User Name:";
            // 
            // tabMember
            // 
            this.tabMember.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabMember.Controls.Add(this.btnSearchUserGroup);
            this.tabMember.Controls.Add(this.gvUserGroup);
            this.tabMember.Controls.Add(this.txtDescription);
            this.tabMember.Controls.Add(this.txtGroupName);
            this.tabMember.Controls.Add(this.label1);
            this.tabMember.Controls.Add(this.label2);
            this.tabMember.Location = new System.Drawing.Point(4, 23);
            this.tabMember.Name = "tabMember";
            this.tabMember.Padding = new System.Windows.Forms.Padding(3);
            this.tabMember.Size = new System.Drawing.Size(742, 268);
            this.tabMember.TabIndex = 1;
            this.tabMember.Text = "Member Of";
            // 
            // btnSearchUserGroup
            // 
            this.btnSearchUserGroup.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSearchUserGroup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearchUserGroup.BackgroundImage")));
            this.btnSearchUserGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchUserGroup.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearchUserGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSearchUserGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnSearchUserGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchUserGroup.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSearchUserGroup.Image = global::GroupM.App.Properties.Resources.search;
            this.btnSearchUserGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchUserGroup.Location = new System.Drawing.Point(648, 12);
            this.btnSearchUserGroup.Name = "btnSearchUserGroup";
            this.btnSearchUserGroup.Size = new System.Drawing.Size(75, 30);
            this.btnSearchUserGroup.TabIndex = 7;
            this.btnSearchUserGroup.Text = "Search";
            this.btnSearchUserGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchUserGroup.UseVisualStyleBackColor = false;
            this.btnSearchUserGroup.Click += new System.EventHandler(this.btnSearchUserGroup_Click);
            // 
            // gvUserGroup
            // 
            this.gvUserGroup.AllowUserToAddRows = false;
            this.gvUserGroup.AllowUserToDeleteRows = false;
            this.gvUserGroup.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.gvUserGroup.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvUserGroup.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.gvUserGroup.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvUserGroup.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvUserGroup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvUserGroup.ColumnHeadersHeight = 25;
            this.gvUserGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvUserGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colUserGroupMappingID,
            this.colUserGroupID,
            this.colUserGroupName});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvUserGroup.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvUserGroup.EnableHeadersVisualStyles = false;
            this.gvUserGroup.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gvUserGroup.Location = new System.Drawing.Point(19, 55);
            this.gvUserGroup.Name = "gvUserGroup";
            this.gvUserGroup.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvUserGroup.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvUserGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvUserGroup.Size = new System.Drawing.Size(704, 200);
            this.gvUserGroup.TabIndex = 6;
            // 
            // colSelected
            // 
            this.colSelected.DataPropertyName = "Selected";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "0";
            this.colSelected.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSelected.HeaderText = "";
            this.colSelected.Name = "colSelected";
            this.colSelected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSelected.Width = 35;
            // 
            // colUserGroupMappingID
            // 
            this.colUserGroupMappingID.DataPropertyName = "UserGroupMappingID";
            this.colUserGroupMappingID.HeaderText = "UserGroupMappingID";
            this.colUserGroupMappingID.Name = "colUserGroupMappingID";
            this.colUserGroupMappingID.Visible = false;
            // 
            // colUserGroupID
            // 
            this.colUserGroupID.DataPropertyName = "UserGroupID";
            this.colUserGroupID.HeaderText = "User Group ID";
            this.colUserGroupID.Name = "colUserGroupID";
            this.colUserGroupID.Visible = false;
            // 
            // colUserGroupName
            // 
            this.colUserGroupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colUserGroupName.DataPropertyName = "UserGroupName";
            this.colUserGroupName.HeaderText = "User Group Name";
            this.colUserGroupName.Name = "colUserGroupName";
            this.colUserGroupName.ReadOnly = true;
            this.colUserGroupName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUserGroupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.Window;
            this.txtDescription.IsRequireField = false;
            this.txtDescription.Location = new System.Drawing.Point(427, 17);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 22);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // txtGroupName
            // 
            this.txtGroupName.BackColor = System.Drawing.SystemColors.Window;
            this.txtGroupName.IsRequireField = false;
            this.txtGroupName.Location = new System.Drawing.Point(120, 17);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(200, 22);
            this.txtGroupName.TabIndex = 2;
            this.txtGroupName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(337, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Group Name:";
            // 
            // frmUserManagementInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.Controls.Add(this.tabUserProfile);
            this.Name = "frmUserManagementInput";
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.frmUserManagementInput_Load);
            this.Controls.SetChildIndex(this.tsbControl, 0);
            this.Controls.SetChildIndex(this.tabUserProfile, 0);
            this.tsbControl.ResumeLayout(false);
            this.tabUserProfile.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.tabUser.PerformLayout();
            this.tabMember.ResumeLayout(false);
            this.tabMember.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUserGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabUserProfile;
        private System.Windows.Forms.TabPage tabUser;
        private System.Windows.Forms.TabPage tabMember;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblFirstname;
        private CustomControl.Common.CustomTextBox txtUserName;
        private CustomControl.Common.CustomTextBox txtFirstName;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Label lblLastname;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Label lblDomain;
        private CustomControl.Common.CustomTextBox txtDomain;
        private System.Windows.Forms.CheckBox chkActive;
        private CustomControl.Common.CustomTextBox txtLastName;
        private CustomControl.Common.CustomTextBox txtDisplayName;
        private CustomControl.Common.CustomTextBox txtDescription;
        private CustomControl.Common.CustomTextBox txtGroupName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private CustomControl.Common.CustomDataGridView gvUserGroup;
        private CustomControl.Common.CustomButton btnSearchUserGroup;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserGroupMappingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserGroupID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserGroupName;
        private CustomControl.Common.CustomTextBox txtPassword;
        private System.Windows.Forms.Label label3;
    }
}