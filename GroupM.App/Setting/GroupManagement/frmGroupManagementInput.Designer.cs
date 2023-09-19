namespace GroupM.App.Setting.GroupManagement
{
    partial class frmGroupManagementInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupManagementInput));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabUserGroup = new System.Windows.Forms.TabControl();
            this.tabGroup = new System.Windows.Forms.TabPage();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.txtDescription = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.txtUserGroupName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblUserGroupName = new System.Windows.Forms.Label();
            this.tabMember = new System.Windows.Forms.TabPage();
            this.cmbStatus = new GroupM.CustomControl.Common.CustomComboBox(this.components);
            this.btnSearchUserGroup = new GroupM.CustomControl.Common.CustomButton(this.components);
            this.gvUserProfile = new GroupM.CustomControl.Common.CustomDataGridView(this.components);
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colUserGorupMappingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserProfileID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUserName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.tabUserMenu = new System.Windows.Forms.TabPage();
            this.btnSearchMenuName = new GroupM.CustomControl.Common.CustomButton(this.components);
            this.gvMenuName = new GroupM.CustomControl.Common.CustomDataGridView(this.components);
            this.colMenuSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colUserGroupMenuMappingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMenuID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMenuName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtMenuName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.lblMenuName = new System.Windows.Forms.Label();
            this.tabUserScreen = new System.Windows.Forms.TabPage();
            this.btnSearchScreenName = new GroupM.CustomControl.Common.CustomButton(this.components);
            this.gvScreenName = new GroupM.CustomControl.Common.CustomDataGridView(this.components);
            this.colScreenSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colScreenID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScreenName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtScreenName = new GroupM.CustomControl.Common.CustomTextBox(this.components);
            this.lblScreenName = new System.Windows.Forms.Label();
            this.tsbControl.SuspendLayout();
            this.tabUserGroup.SuspendLayout();
            this.tabGroup.SuspendLayout();
            this.tabMember.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUserProfile)).BeginInit();
            this.tabUserMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMenuName)).BeginInit();
            this.tabUserScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvScreenName)).BeginInit();
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
            // tabUserGroup
            // 
            this.tabUserGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabUserGroup.Controls.Add(this.tabGroup);
            this.tabUserGroup.Controls.Add(this.tabMember);
            this.tabUserGroup.Controls.Add(this.tabUserMenu);
            this.tabUserGroup.Controls.Add(this.tabUserScreen);
            this.tabUserGroup.Location = new System.Drawing.Point(20, 50);
            this.tabUserGroup.Name = "tabUserGroup";
            this.tabUserGroup.SelectedIndex = 0;
            this.tabUserGroup.Size = new System.Drawing.Size(750, 295);
            this.tabUserGroup.TabIndex = 0;
            // 
            // tabGroup
            // 
            this.tabGroup.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabGroup.Controls.Add(this.chkActive);
            this.tabGroup.Controls.Add(this.lblActive);
            this.tabGroup.Controls.Add(this.txtDescription);
            this.tabGroup.Controls.Add(this.txtUserGroupName);
            this.tabGroup.Controls.Add(this.lblDescription);
            this.tabGroup.Controls.Add(this.lblUserGroupName);
            this.tabGroup.Location = new System.Drawing.Point(4, 23);
            this.tabGroup.Name = "tabGroup";
            this.tabGroup.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroup.Size = new System.Drawing.Size(742, 268);
            this.tabGroup.TabIndex = 0;
            this.tabGroup.Text = "Group";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(141, 179);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 2;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Location = new System.Drawing.Point(25, 180);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(45, 14);
            this.lblActive.TabIndex = 6;
            this.lblActive.Text = "Active:";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.Window;
            this.txtDescription.IsRequireField = false;
            this.txtDescription.Location = new System.Drawing.Point(141, 47);
            this.txtDescription.MaxLength = 500;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(568, 120);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // txtUserGroupName
            // 
            this.txtUserGroupName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtUserGroupName.IsRequireField = true;
            this.txtUserGroupName.Location = new System.Drawing.Point(141, 17);
            this.txtUserGroupName.MaxLength = 100;
            this.txtUserGroupName.Name = "txtUserGroupName";
            this.txtUserGroupName.Size = new System.Drawing.Size(568, 22);
            this.txtUserGroupName.TabIndex = 0;
            this.txtUserGroupName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(25, 50);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(71, 14);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Description:";
            // 
            // lblUserGroupName
            // 
            this.lblUserGroupName.AutoSize = true;
            this.lblUserGroupName.Location = new System.Drawing.Point(25, 20);
            this.lblUserGroupName.Name = "lblUserGroupName";
            this.lblUserGroupName.Size = new System.Drawing.Size(107, 14);
            this.lblUserGroupName.TabIndex = 0;
            this.lblUserGroupName.Text = "User Group Name:";
            // 
            // tabMember
            // 
            this.tabMember.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabMember.Controls.Add(this.cmbStatus);
            this.tabMember.Controls.Add(this.btnSearchUserGroup);
            this.tabMember.Controls.Add(this.gvUserProfile);
            this.tabMember.Controls.Add(this.txtUserName);
            this.tabMember.Controls.Add(this.lblStatus);
            this.tabMember.Controls.Add(this.lblUserName);
            this.tabMember.Location = new System.Drawing.Point(4, 23);
            this.tabMember.Name = "tabMember";
            this.tabMember.Padding = new System.Windows.Forms.Padding(3);
            this.tabMember.Size = new System.Drawing.Size(742, 268);
            this.tabMember.TabIndex = 1;
            this.tabMember.Text = "Member";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "All",
            "Active",
            "InActive"});
            this.cmbStatus.Location = new System.Drawing.Point(414, 17);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(200, 22);
            this.cmbStatus.TabIndex = 8;
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
            // gvUserProfile
            // 
            this.gvUserProfile.AllowUserToAddRows = false;
            this.gvUserProfile.AllowUserToDeleteRows = false;
            this.gvUserProfile.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.gvUserProfile.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvUserProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvUserProfile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvUserProfile.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.gvUserProfile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvUserProfile.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvUserProfile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvUserProfile.ColumnHeadersHeight = 25;
            this.gvUserProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvUserProfile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colUserGorupMappingID,
            this.colUserProfileID,
            this.colUserName,
            this.colDomain,
            this.colUserStatus});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvUserProfile.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvUserProfile.EnableHeadersVisualStyles = false;
            this.gvUserProfile.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gvUserProfile.Location = new System.Drawing.Point(19, 55);
            this.gvUserProfile.Name = "gvUserProfile";
            this.gvUserProfile.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvUserProfile.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvUserProfile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvUserProfile.Size = new System.Drawing.Size(704, 200);
            this.gvUserProfile.TabIndex = 6;
            // 
            // colSelected
            // 
            this.colSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSelected.DataPropertyName = "Selected";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "0";
            this.colSelected.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSelected.FillWeight = 71.06599F;
            this.colSelected.HeaderText = "";
            this.colSelected.Name = "colSelected";
            this.colSelected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSelected.Width = 35;
            // 
            // colUserGorupMappingID
            // 
            this.colUserGorupMappingID.DataPropertyName = "UserGroupMappingID";
            this.colUserGorupMappingID.HeaderText = "User Group Mapping ID";
            this.colUserGorupMappingID.Name = "colUserGorupMappingID";
            this.colUserGorupMappingID.Visible = false;
            // 
            // colUserProfileID
            // 
            this.colUserProfileID.DataPropertyName = "UserProfileID";
            this.colUserProfileID.HeaderText = "User Profile ID";
            this.colUserProfileID.Name = "colUserProfileID";
            this.colUserProfileID.Visible = false;
            // 
            // colUserName
            // 
            this.colUserName.DataPropertyName = "UserName";
            this.colUserName.FillWeight = 109.6447F;
            this.colUserName.HeaderText = "User Name";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            // 
            // colDomain
            // 
            this.colDomain.DataPropertyName = "Domain";
            this.colDomain.FillWeight = 109.6447F;
            this.colDomain.HeaderText = "Domain";
            this.colDomain.Name = "colDomain";
            this.colDomain.ReadOnly = true;
            // 
            // colUserStatus
            // 
            this.colUserStatus.DataPropertyName = "IsActive";
            this.colUserStatus.FillWeight = 109.6447F;
            this.colUserStatus.HeaderText = "User Status";
            this.colUserStatus.Name = "colUserStatus";
            this.colUserStatus.ReadOnly = true;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.SystemColors.Window;
            this.txtUserName.IsRequireField = false;
            this.txtUserName.Location = new System.Drawing.Point(120, 17);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(200, 22);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(352, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 14);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(25, 20);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(70, 14);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "User Name:";
            // 
            // tabUserMenu
            // 
            this.tabUserMenu.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabUserMenu.Controls.Add(this.btnSearchMenuName);
            this.tabUserMenu.Controls.Add(this.gvMenuName);
            this.tabUserMenu.Controls.Add(this.txtMenuName);
            this.tabUserMenu.Controls.Add(this.lblMenuName);
            this.tabUserMenu.Location = new System.Drawing.Point(4, 23);
            this.tabUserMenu.Name = "tabUserMenu";
            this.tabUserMenu.Size = new System.Drawing.Size(742, 268);
            this.tabUserMenu.TabIndex = 2;
            this.tabUserMenu.Text = "User Menu";
            // 
            // btnSearchMenuName
            // 
            this.btnSearchMenuName.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSearchMenuName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearchMenuName.BackgroundImage")));
            this.btnSearchMenuName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchMenuName.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearchMenuName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSearchMenuName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnSearchMenuName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchMenuName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSearchMenuName.Image = global::GroupM.App.Properties.Resources.search;
            this.btnSearchMenuName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchMenuName.Location = new System.Drawing.Point(648, 12);
            this.btnSearchMenuName.Name = "btnSearchMenuName";
            this.btnSearchMenuName.Size = new System.Drawing.Size(75, 30);
            this.btnSearchMenuName.TabIndex = 11;
            this.btnSearchMenuName.Text = "Search";
            this.btnSearchMenuName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchMenuName.UseVisualStyleBackColor = false;
            this.btnSearchMenuName.Click += new System.EventHandler(this.btnSearchMenuName_Click);
            // 
            // gvMenuName
            // 
            this.gvMenuName.AllowUserToAddRows = false;
            this.gvMenuName.AllowUserToDeleteRows = false;
            this.gvMenuName.AllowUserToOrderColumns = true;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.gvMenuName.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.gvMenuName.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvMenuName.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.gvMenuName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvMenuName.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMenuName.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gvMenuName.ColumnHeadersHeight = 25;
            this.gvMenuName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvMenuName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMenuSelected,
            this.colUserGroupMenuMappingID,
            this.colMenuID,
            this.colMenuName});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvMenuName.DefaultCellStyle = dataGridViewCellStyle9;
            this.gvMenuName.EnableHeadersVisualStyles = false;
            this.gvMenuName.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.gvMenuName.Location = new System.Drawing.Point(19, 55);
            this.gvMenuName.Name = "gvMenuName";
            this.gvMenuName.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMenuName.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gvMenuName.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMenuName.Size = new System.Drawing.Size(704, 200);
            this.gvMenuName.TabIndex = 10;
            // 
            // colMenuSelected
            // 
            this.colMenuSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colMenuSelected.DataPropertyName = "Selected";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "0";
            this.colMenuSelected.DefaultCellStyle = dataGridViewCellStyle8;
            this.colMenuSelected.FillWeight = 35.53299F;
            this.colMenuSelected.HeaderText = "";
            this.colMenuSelected.Name = "colMenuSelected";
            this.colMenuSelected.Width = 35;
            // 
            // colUserGroupMenuMappingID
            // 
            this.colUserGroupMenuMappingID.DataPropertyName = "UserGroupMenuMappingID";
            this.colUserGroupMenuMappingID.HeaderText = "User Group Menu Mapping ID";
            this.colUserGroupMenuMappingID.Name = "colUserGroupMenuMappingID";
            this.colUserGroupMenuMappingID.Visible = false;
            // 
            // colMenuID
            // 
            this.colMenuID.DataPropertyName = "UserMenuID";
            this.colMenuID.HeaderText = "Menu ID";
            this.colMenuID.Name = "colMenuID";
            this.colMenuID.Visible = false;
            // 
            // colMenuName
            // 
            this.colMenuName.DataPropertyName = "UserMenuName";
            this.colMenuName.FillWeight = 164.467F;
            this.colMenuName.HeaderText = "MenuName";
            this.colMenuName.Name = "colMenuName";
            // 
            // txtMenuName
            // 
            this.txtMenuName.BackColor = System.Drawing.SystemColors.Window;
            this.txtMenuName.IsRequireField = false;
            this.txtMenuName.Location = new System.Drawing.Point(120, 17);
            this.txtMenuName.Name = "txtMenuName";
            this.txtMenuName.Size = new System.Drawing.Size(508, 22);
            this.txtMenuName.TabIndex = 8;
            this.txtMenuName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // lblMenuName
            // 
            this.lblMenuName.AutoSize = true;
            this.lblMenuName.Location = new System.Drawing.Point(25, 20);
            this.lblMenuName.Name = "lblMenuName";
            this.lblMenuName.Size = new System.Drawing.Size(76, 14);
            this.lblMenuName.TabIndex = 9;
            this.lblMenuName.Text = "Menu Name:";
            // 
            // tabUserScreen
            // 
            this.tabUserScreen.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabUserScreen.Controls.Add(this.btnSearchScreenName);
            this.tabUserScreen.Controls.Add(this.gvScreenName);
            this.tabUserScreen.Controls.Add(this.txtScreenName);
            this.tabUserScreen.Controls.Add(this.lblScreenName);
            this.tabUserScreen.Location = new System.Drawing.Point(4, 23);
            this.tabUserScreen.Name = "tabUserScreen";
            this.tabUserScreen.Size = new System.Drawing.Size(742, 268);
            this.tabUserScreen.TabIndex = 3;
            this.tabUserScreen.Text = "User Screen";
            // 
            // btnSearchScreenName
            // 
            this.btnSearchScreenName.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSearchScreenName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearchScreenName.BackgroundImage")));
            this.btnSearchScreenName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchScreenName.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearchScreenName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSearchScreenName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnSearchScreenName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchScreenName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnSearchScreenName.Image = global::GroupM.App.Properties.Resources.search;
            this.btnSearchScreenName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchScreenName.Location = new System.Drawing.Point(648, 12);
            this.btnSearchScreenName.Name = "btnSearchScreenName";
            this.btnSearchScreenName.Size = new System.Drawing.Size(75, 30);
            this.btnSearchScreenName.TabIndex = 15;
            this.btnSearchScreenName.Text = "Search";
            this.btnSearchScreenName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchScreenName.UseVisualStyleBackColor = false;
            this.btnSearchScreenName.Click += new System.EventHandler(this.btnSearchScreenName_Click);
            // 
            // gvScreenName
            // 
            this.gvScreenName.AllowUserToAddRows = false;
            this.gvScreenName.AllowUserToDeleteRows = false;
            this.gvScreenName.AllowUserToOrderColumns = true;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            this.gvScreenName.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.gvScreenName.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvScreenName.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            this.gvScreenName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvScreenName.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvScreenName.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gvScreenName.ColumnHeadersHeight = 25;
            this.gvScreenName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvScreenName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colScreenSelected,
            this.colScreenID,
            this.colScreenName});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvScreenName.DefaultCellStyle = dataGridViewCellStyle14;
            this.gvScreenName.EnableHeadersVisualStyles = false;
            this.gvScreenName.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.gvScreenName.Location = new System.Drawing.Point(19, 55);
            this.gvScreenName.Name = "gvScreenName";
            this.gvScreenName.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvScreenName.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.gvScreenName.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvScreenName.Size = new System.Drawing.Size(704, 200);
            this.gvScreenName.TabIndex = 14;
            // 
            // colScreenSelected
            // 
            this.colScreenSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colScreenSelected.DataPropertyName = "Selected";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.NullValue = "0";
            this.colScreenSelected.DefaultCellStyle = dataGridViewCellStyle13;
            this.colScreenSelected.HeaderText = "";
            this.colScreenSelected.Name = "colScreenSelected";
            this.colScreenSelected.Width = 35;
            // 
            // colScreenID
            // 
            this.colScreenID.DataPropertyName = "UserScreenID";
            this.colScreenID.HeaderText = "Screen ID";
            this.colScreenID.Name = "colScreenID";
            this.colScreenID.Visible = false;
            // 
            // colScreenName
            // 
            this.colScreenName.DataPropertyName = "UserScreenName";
            this.colScreenName.HeaderText = "Screen Name";
            this.colScreenName.Name = "colScreenName";
            // 
            // txtScreenName
            // 
            this.txtScreenName.BackColor = System.Drawing.SystemColors.Window;
            this.txtScreenName.IsRequireField = false;
            this.txtScreenName.Location = new System.Drawing.Point(120, 17);
            this.txtScreenName.Name = "txtScreenName";
            this.txtScreenName.Size = new System.Drawing.Size(508, 22);
            this.txtScreenName.TabIndex = 12;
            this.txtScreenName.TextBoxType = GroupM.CustomControl.Common.CustomTextBox.eTextBoxType.Text;
            // 
            // lblScreenName
            // 
            this.lblScreenName.AutoSize = true;
            this.lblScreenName.Location = new System.Drawing.Point(25, 20);
            this.lblScreenName.Name = "lblScreenName";
            this.lblScreenName.Size = new System.Drawing.Size(84, 14);
            this.lblScreenName.TabIndex = 13;
            this.lblScreenName.Text = "Screen Name:";
            // 
            // frmGroupManagementInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.Controls.Add(this.tabUserGroup);
            this.Name = "frmGroupManagementInput";
            this.Text = "Group Management";
            this.Load += new System.EventHandler(this.frmGroupManagementInput_Load);
            this.Controls.SetChildIndex(this.tsbControl, 0);
            this.Controls.SetChildIndex(this.tabUserGroup, 0);
            this.tsbControl.ResumeLayout(false);
            this.tabUserGroup.ResumeLayout(false);
            this.tabGroup.ResumeLayout(false);
            this.tabGroup.PerformLayout();
            this.tabMember.ResumeLayout(false);
            this.tabMember.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUserProfile)).EndInit();
            this.tabUserMenu.ResumeLayout(false);
            this.tabUserMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMenuName)).EndInit();
            this.tabUserScreen.ResumeLayout(false);
            this.tabUserScreen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvScreenName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabUserGroup;
        private System.Windows.Forms.TabPage tabGroup;
        private CustomControl.Common.CustomTextBox txtDescription;
        private CustomControl.Common.CustomTextBox txtUserGroupName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblUserGroupName;
        private System.Windows.Forms.TabPage tabMember;
        private CustomControl.Common.CustomButton btnSearchUserGroup;
        private CustomControl.Common.CustomDataGridView gvUserProfile;
        private CustomControl.Common.CustomTextBox txtUserName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblUserName;
        private CustomControl.Common.CustomComboBox cmbStatus;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.TabPage tabUserMenu;
        private System.Windows.Forms.TabPage tabUserScreen;
        private CustomControl.Common.CustomButton btnSearchMenuName;
        private CustomControl.Common.CustomDataGridView gvMenuName;
        private CustomControl.Common.CustomTextBox txtMenuName;
        private System.Windows.Forms.Label lblMenuName;
        private CustomControl.Common.CustomButton btnSearchScreenName;
        private CustomControl.Common.CustomDataGridView gvScreenName;
        private CustomControl.Common.CustomTextBox txtScreenName;
        private System.Windows.Forms.Label lblScreenName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colScreenSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScreenID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScreenName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserGorupMappingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserProfileID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDomain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMenuSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserGroupMenuMappingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMenuID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMenuName;
    }
}