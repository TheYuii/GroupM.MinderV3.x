namespace GroupM.App
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabForm = new GroupM.CustomControl.TabMdi.CustomTabControl();
            this.navigationPane1 = new GroupM.CustomControl.Navigation.NavigationPane();
            this.mnsMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createOwnDayPartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaSpendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preAndPosByReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPRPMorniteringByWeekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planCPRPByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aGBReformattingExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tVCarlendarReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reachPreReachByWeeklyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reachSingleBuyingBriefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reachSummaryGroupOfBuyingBriefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vendorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaSubTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adeptMediaTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proprietaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proprietaryGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoGrantPermissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.implementationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buyingBriefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaInvestmentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.optInReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.financialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToAdeptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPMInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaInvestmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expenditureReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.userManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stsStatus = new System.Windows.Forms.StatusStrip();
            this.lblLoginName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScreenName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpace = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDatabaseName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLogout = new System.Windows.Forms.ToolStripStatusLabel();
            this.productToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.mnsMenu.SuspendLayout();
            this.stsStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabForm);
            this.panel1.Controls.Add(this.navigationPane1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1880, 931);
            this.panel1.TabIndex = 7;
            // 
            // tabForm
            // 
            this.tabForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabForm.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabForm.Location = new System.Drawing.Point(333, 0);
            this.tabForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabForm.Name = "tabForm";
            this.tabForm.SelectedIndex = 0;
            this.tabForm.Size = new System.Drawing.Size(1547, 931);
            this.tabForm.TabIndex = 1;
            this.tabForm.SelectedIndexChanged += new System.EventHandler(this.tabForm_SelectedIndexChanged);
            this.tabForm.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabForm_Selected);
            this.tabForm.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabForm_ControlAdded);
            this.tabForm.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.tabForm_ControlRemoved);
            // 
            // navigationPane1
            // 
            this.navigationPane1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Left;
            this.navigationPane1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.navigationPane1.Location = new System.Drawing.Point(0, 0);
            this.navigationPane1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.navigationPane1.MenuStrip = this.mnsMenu;
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.Padding = new System.Windows.Forms.Padding(1);
            this.navigationPane1.Size = new System.Drawing.Size(333, 931);
            this.navigationPane1.TabIndex = 2;
            this.navigationPane1.Visible = false;
            this.navigationPane1.Click += new System.EventHandler(this.item_Click);
            // 
            // mnsMenu
            // 
            this.mnsMenu.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.mnsMenu.BackgroundImage = global::GroupM.App.Properties.Resources.bg;
            this.mnsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mediaSpendingToolStripMenuItem,
            this.masterToolStripMenuItem,
            this.implementationToolStripMenuItem,
            this.mediaInvestmentToolStripMenuItem1,
            this.financialToolStripMenuItem,
            this.mediaInvestmentToolStripMenuItem,
            this.tsmiSetting,
            this.windowToolStripMenuItem});
            this.mnsMenu.Location = new System.Drawing.Point(0, 0);
            this.mnsMenu.Name = "mnsMenu";
            this.mnsMenu.Size = new System.Drawing.Size(1880, 38);
            this.mnsMenu.TabIndex = 9;
            this.mnsMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createOwnDayPartToolStripMenuItem,
            this.logOutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // createOwnDayPartToolStripMenuItem
            // 
            this.createOwnDayPartToolStripMenuItem.Name = "createOwnDayPartToolStripMenuItem";
            this.createOwnDayPartToolStripMenuItem.Size = new System.Drawing.Size(228, 26);
            this.createOwnDayPartToolStripMenuItem.Text = "Create Own Day Part";
            this.createOwnDayPartToolStripMenuItem.Click += new System.EventHandler(this.createOwnDayPartToolStripMenuItem_Click);
            // 
            // logOutToolStripMenuItem
            // 
            this.logOutToolStripMenuItem.Name = "logOutToolStripMenuItem";
            this.logOutToolStripMenuItem.Size = new System.Drawing.Size(228, 26);
            this.logOutToolStripMenuItem.Text = "Log Out";
            this.logOutToolStripMenuItem.Click += new System.EventHandler(this.logOutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(228, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mediaSpendingToolStripMenuItem
            // 
            this.mediaSpendingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preAndPosByReportToolStripMenuItem,
            this.cPRPMorniteringByWeekToolStripMenuItem,
            this.planCPRPByToolStripMenuItem,
            this.aGBReformattingExcelToolStripMenuItem,
            this.tVCarlendarReportToolStripMenuItem,
            this.reachPreReachByWeeklyToolStripMenuItem,
            this.reachSingleBuyingBriefToolStripMenuItem,
            this.reachSummaryGroupOfBuyingBriefToolStripMenuItem});
            this.mediaSpendingToolStripMenuItem.Name = "mediaSpendingToolStripMenuItem";
            this.mediaSpendingToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.mediaSpendingToolStripMenuItem.Text = "Minder Report";
            // 
            // preAndPosByReportToolStripMenuItem
            // 
            this.preAndPosByReportToolStripMenuItem.Name = "preAndPosByReportToolStripMenuItem";
            this.preAndPosByReportToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.preAndPosByReportToolStripMenuItem.Text = "Pre and Pos By Report";
            this.preAndPosByReportToolStripMenuItem.Click += new System.EventHandler(this.preAndPosByReportToolStripMenuItem_Click);
            // 
            // cPRPMorniteringByWeekToolStripMenuItem
            // 
            this.cPRPMorniteringByWeekToolStripMenuItem.Name = "cPRPMorniteringByWeekToolStripMenuItem";
            this.cPRPMorniteringByWeekToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.cPRPMorniteringByWeekToolStripMenuItem.Text = "CPRP Monitoring By Week";
            this.cPRPMorniteringByWeekToolStripMenuItem.Click += new System.EventHandler(this.cPRPMorniteringByWeekToolStripMenuItem_Click);
            // 
            // planCPRPByToolStripMenuItem
            // 
            this.planCPRPByToolStripMenuItem.Name = "planCPRPByToolStripMenuItem";
            this.planCPRPByToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.planCPRPByToolStripMenuItem.Text = "Plan CPRP By Buying Brief";
            this.planCPRPByToolStripMenuItem.Click += new System.EventHandler(this.planCPRPByToolStripMenuItem_Click);
            // 
            // aGBReformattingExcelToolStripMenuItem
            // 
            this.aGBReformattingExcelToolStripMenuItem.Name = "aGBReformattingExcelToolStripMenuItem";
            this.aGBReformattingExcelToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.aGBReformattingExcelToolStripMenuItem.Text = "AGB Reformating Excel";
            this.aGBReformattingExcelToolStripMenuItem.Click += new System.EventHandler(this.aGBReformattingExcelToolStripMenuItem_Click);
            // 
            // tVCarlendarReportToolStripMenuItem
            // 
            this.tVCarlendarReportToolStripMenuItem.Name = "tVCarlendarReportToolStripMenuItem";
            this.tVCarlendarReportToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.tVCarlendarReportToolStripMenuItem.Text = "TV Calendar Report";
            this.tVCarlendarReportToolStripMenuItem.Click += new System.EventHandler(this.tVCarlendarReportToolStripMenuItem_Click);
            // 
            // reachPreReachByWeeklyToolStripMenuItem
            // 
            this.reachPreReachByWeeklyToolStripMenuItem.Name = "reachPreReachByWeeklyToolStripMenuItem";
            this.reachPreReachByWeeklyToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.reachPreReachByWeeklyToolStripMenuItem.Text = "Reach - Pre Reach By Weekly";
            this.reachPreReachByWeeklyToolStripMenuItem.Click += new System.EventHandler(this.reachPreReachByWeeklyToolStripMenuItem_Click);
            // 
            // reachSingleBuyingBriefToolStripMenuItem
            // 
            this.reachSingleBuyingBriefToolStripMenuItem.Name = "reachSingleBuyingBriefToolStripMenuItem";
            this.reachSingleBuyingBriefToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.reachSingleBuyingBriefToolStripMenuItem.Text = "Reach - Single Buying Brief";
            this.reachSingleBuyingBriefToolStripMenuItem.Click += new System.EventHandler(this.reachSingleBuyingBriefToolStripMenuItem_Click);
            // 
            // reachSummaryGroupOfBuyingBriefToolStripMenuItem
            // 
            this.reachSummaryGroupOfBuyingBriefToolStripMenuItem.Name = "reachSummaryGroupOfBuyingBriefToolStripMenuItem";
            this.reachSummaryGroupOfBuyingBriefToolStripMenuItem.Size = new System.Drawing.Size(355, 26);
            this.reachSummaryGroupOfBuyingBriefToolStripMenuItem.Text = "Reach - Summary Group of Buying Brief";
            this.reachSummaryGroupOfBuyingBriefToolStripMenuItem.Click += new System.EventHandler(this.reachSummaryGroupOfBuyingBriefToolStripMenuItem_Click);
            // 
            // masterToolStripMenuItem
            // 
            this.masterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.officeToolStripMenuItem,
            this.clientToolStripMenuItem,
            this.productToolStripMenuItem,
            this.vendorToolStripMenuItem,
            this.mediaTypeToolStripMenuItem,
            this.mediaSubTypeToolStripMenuItem,
            this.adeptMediaTypeToolStripMenuItem,
            this.proprietaryToolStripMenuItem,
            this.proprietaryGroupToolStripMenuItem,
            this.autoGrantPermissionToolStripMenuItem});
            this.masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            this.masterToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.masterToolStripMenuItem.Text = "Master";
            // 
            // officeToolStripMenuItem
            // 
            this.officeToolStripMenuItem.Name = "officeToolStripMenuItem";
            this.officeToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.officeToolStripMenuItem.Text = "Office";
            this.officeToolStripMenuItem.Click += new System.EventHandler(this.OfficeToolStripMenuItem_Click);
            // 
            // clientToolStripMenuItem
            // 
            this.clientToolStripMenuItem.Name = "clientToolStripMenuItem";
            this.clientToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.clientToolStripMenuItem.Text = "Client";
            this.clientToolStripMenuItem.Click += new System.EventHandler(this.clientToolStripMenuItem_Click);
            // 
            // vendorToolStripMenuItem
            // 
            this.vendorToolStripMenuItem.Name = "vendorToolStripMenuItem";
            this.vendorToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.vendorToolStripMenuItem.Text = "Vendor";
            this.vendorToolStripMenuItem.Click += new System.EventHandler(this.vendorToolStripMenuItem_Click);
            // 
            // mediaTypeToolStripMenuItem
            // 
            this.mediaTypeToolStripMenuItem.Name = "mediaTypeToolStripMenuItem";
            this.mediaTypeToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.mediaTypeToolStripMenuItem.Text = "Media Type";
            this.mediaTypeToolStripMenuItem.Click += new System.EventHandler(this.mediaTypeToolStripMenuItem_Click);
            // 
            // mediaSubTypeToolStripMenuItem
            // 
            this.mediaSubTypeToolStripMenuItem.Name = "mediaSubTypeToolStripMenuItem";
            this.mediaSubTypeToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.mediaSubTypeToolStripMenuItem.Text = "Media Sub Type";
            this.mediaSubTypeToolStripMenuItem.Click += new System.EventHandler(this.MediaSubTypeToolStripMenuItem_Click);
            // 
            // adeptMediaTypeToolStripMenuItem
            // 
            this.adeptMediaTypeToolStripMenuItem.Name = "adeptMediaTypeToolStripMenuItem";
            this.adeptMediaTypeToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.adeptMediaTypeToolStripMenuItem.Text = "Adept Media Type";
            this.adeptMediaTypeToolStripMenuItem.Click += new System.EventHandler(this.adeptMediaTypeToolStripMenuItem_Click);
            // 
            // proprietaryToolStripMenuItem
            // 
            this.proprietaryToolStripMenuItem.Name = "proprietaryToolStripMenuItem";
            this.proprietaryToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.proprietaryToolStripMenuItem.Text = "Proprietary";
            this.proprietaryToolStripMenuItem.Click += new System.EventHandler(this.proprietaryToolStripMenuItem_Click);
            // 
            // proprietaryGroupToolStripMenuItem
            // 
            this.proprietaryGroupToolStripMenuItem.Name = "proprietaryGroupToolStripMenuItem";
            this.proprietaryGroupToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.proprietaryGroupToolStripMenuItem.Text = "Proprietary Group";
            this.proprietaryGroupToolStripMenuItem.Click += new System.EventHandler(this.proprietaryGroupToolStripMenuItem_Click);
            // 
            // autoGrantPermissionToolStripMenuItem
            // 
            this.autoGrantPermissionToolStripMenuItem.Name = "autoGrantPermissionToolStripMenuItem";
            this.autoGrantPermissionToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.autoGrantPermissionToolStripMenuItem.Text = "Auto Grant Permission";
            this.autoGrantPermissionToolStripMenuItem.Click += new System.EventHandler(this.autoGrantPermissionToolStripMenuItem_Click);
            // 
            // implementationToolStripMenuItem
            // 
            this.implementationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buyingBriefToolStripMenuItem});
            this.implementationToolStripMenuItem.Name = "implementationToolStripMenuItem";
            this.implementationToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.implementationToolStripMenuItem.Text = "Implementation";
            // 
            // buyingBriefToolStripMenuItem
            // 
            this.buyingBriefToolStripMenuItem.Name = "buyingBriefToolStripMenuItem";
            this.buyingBriefToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.buyingBriefToolStripMenuItem.Text = "Buying Brief";
            this.buyingBriefToolStripMenuItem.Click += new System.EventHandler(this.BuyingBriefToolStripMenuItem_Click);
            // 
            // mediaInvestmentToolStripMenuItem1
            // 
            this.mediaInvestmentToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optInReportToolStripMenuItem});
            this.mediaInvestmentToolStripMenuItem1.Name = "mediaInvestmentToolStripMenuItem1";
            this.mediaInvestmentToolStripMenuItem1.Size = new System.Drawing.Size(141, 24);
            this.mediaInvestmentToolStripMenuItem1.Text = "Media Investment";
            // 
            // optInReportToolStripMenuItem
            // 
            this.optInReportToolStripMenuItem.Name = "optInReportToolStripMenuItem";
            this.optInReportToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.optInReportToolStripMenuItem.Text = "Opt In Report";
            this.optInReportToolStripMenuItem.Click += new System.EventHandler(this.optInReportToolStripMenuItem_Click);
            // 
            // financialToolStripMenuItem
            // 
            this.financialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToAdeptToolStripMenuItem,
            this.gPMInvoiceToolStripMenuItem});
            this.financialToolStripMenuItem.Name = "financialToolStripMenuItem";
            this.financialToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.financialToolStripMenuItem.Text = "Financial";
            // 
            // exportToAdeptToolStripMenuItem
            // 
            this.exportToAdeptToolStripMenuItem.Name = "exportToAdeptToolStripMenuItem";
            this.exportToAdeptToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
            this.exportToAdeptToolStripMenuItem.Text = "Export to Adept";
            this.exportToAdeptToolStripMenuItem.Click += new System.EventHandler(this.exportToAdeptToolStripMenuItem_Click);
            // 
            // gPMInvoiceToolStripMenuItem
            // 
            this.gPMInvoiceToolStripMenuItem.Name = "gPMInvoiceToolStripMenuItem";
            this.gPMInvoiceToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
            this.gPMInvoiceToolStripMenuItem.Text = "GPM Invoice";
            this.gPMInvoiceToolStripMenuItem.Click += new System.EventHandler(this.GPMInvoiceToolStripMenuItem_Click);
            // 
            // mediaInvestmentToolStripMenuItem
            // 
            this.mediaInvestmentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expenditureReportToolStripMenuItem});
            this.mediaInvestmentToolStripMenuItem.Name = "mediaInvestmentToolStripMenuItem";
            this.mediaInvestmentToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.mediaInvestmentToolStripMenuItem.Text = "Report";
            // 
            // expenditureReportToolStripMenuItem
            // 
            this.expenditureReportToolStripMenuItem.Name = "expenditureReportToolStripMenuItem";
            this.expenditureReportToolStripMenuItem.Size = new System.Drawing.Size(220, 26);
            this.expenditureReportToolStripMenuItem.Text = "Expenditure Report";
            // 
            // tsmiSetting
            // 
            this.tsmiSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManagerToolStripMenuItem,
            this.groupsManagementToolStripMenuItem});
            this.tsmiSetting.Name = "tsmiSetting";
            this.tsmiSetting.Size = new System.Drawing.Size(70, 24);
            this.tsmiSetting.Text = "Setting";
            // 
            // userManagerToolStripMenuItem
            // 
            this.userManagerToolStripMenuItem.Name = "userManagerToolStripMenuItem";
            this.userManagerToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.userManagerToolStripMenuItem.Text = "User Management";
            this.userManagerToolStripMenuItem.Click += new System.EventHandler(this.userManagerToolStripMenuItem_Click);
            // 
            // groupsManagementToolStripMenuItem
            // 
            this.groupsManagementToolStripMenuItem.Name = "groupsManagementToolStripMenuItem";
            this.groupsManagementToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.groupsManagementToolStripMenuItem.Text = "Groups Management";
            this.groupsManagementToolStripMenuItem.Click += new System.EventHandler(this.groupsManagementToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // stsStatus
            // 
            this.stsStatus.AutoSize = false;
            this.stsStatus.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.stsStatus.BackgroundImage = global::GroupM.App.Properties.Resources.bg;
            this.stsStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.stsStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLoginName,
            this.toolStripStatusLabel3,
            this.lblScreenName,
            this.lblSpace,
            this.toolStripStatusLabel4,
            this.lblDatabaseName,
            this.toolStripStatusLabel5,
            this.lblVersion,
            this.lblLogout});
            this.stsStatus.Location = new System.Drawing.Point(0, 783);
            this.stsStatus.Name = "stsStatus";
            this.stsStatus.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.stsStatus.Size = new System.Drawing.Size(1504, 31);
            this.stsStatus.TabIndex = 8;
            this.stsStatus.Text = "statusStrip1";
            // 
            // lblLoginName
            // 
            this.lblLoginName.AutoSize = false;
            this.lblLoginName.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginName.Image = ((System.Drawing.Image)(resources.GetObject("lblLoginName.Image")));
            this.lblLoginName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLoginName.Name = "lblLoginName";
            this.lblLoginName.Size = new System.Drawing.Size(150, 25);
            this.lblLoginName.Text = "Login Name";
            this.lblLoginName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(13, 25);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // lblScreenName
            // 
            this.lblScreenName.AutoSize = false;
            this.lblScreenName.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenName.Image = ((System.Drawing.Image)(resources.GetObject("lblScreenName.Image")));
            this.lblScreenName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblScreenName.Name = "lblScreenName";
            this.lblScreenName.Size = new System.Drawing.Size(400, 25);
            this.lblScreenName.Text = "Screen Name";
            this.lblScreenName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSpace
            // 
            this.lblSpace.AutoSize = false;
            this.lblSpace.BackColor = System.Drawing.Color.Transparent;
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Size = new System.Drawing.Size(183, 25);
            this.lblSpace.Spring = true;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(13, 25);
            this.toolStripStatusLabel4.Text = "|";
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.AutoSize = false;
            this.lblDatabaseName.BackColor = System.Drawing.Color.Transparent;
            this.lblDatabaseName.Image = ((System.Drawing.Image)(resources.GetObject("lblDatabaseName.Image")));
            this.lblDatabaseName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(500, 25);
            this.lblDatabaseName.Text = "Database Name";
            this.lblDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(13, 25);
            this.toolStripStatusLabel5.Text = "|";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = false;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(130, 25);
            this.lblVersion.Text = "Version 1.0.0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLogout
            // 
            this.lblLogout.BackColor = System.Drawing.Color.Transparent;
            this.lblLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogout.ForeColor = System.Drawing.Color.Blue;
            this.lblLogout.Image = global::GroupM.App.Properties.Resources.logout;
            this.lblLogout.IsLink = true;
            this.lblLogout.Name = "lblLogout";
            this.lblLogout.Size = new System.Drawing.Size(82, 25);
            this.lblLogout.Text = "Log Out";
            this.lblLogout.Click += new System.EventHandler(this.lblLogout_Click);
            // 
            // productToolStripMenuItem
            // 
            this.productToolStripMenuItem.Name = "productToolStripMenuItem";
            this.productToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.productToolStripMenuItem.Text = "Product";
            this.productToolStripMenuItem.Click += new System.EventHandler(this.productToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1504, 814);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stsStatus);
            this.Controls.Add(this.mnsMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMain";
            this.Text = "Minder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.mnsMenu.ResumeLayout(false);
            this.mnsMenu.PerformLayout();
            this.stsStatus.ResumeLayout(false);
            this.stsStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip stsStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblLoginName;
        private System.Windows.Forms.ToolStripStatusLabel lblScreenName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel lblDatabaseName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel lblVersion;
        private System.Windows.Forms.ToolStripStatusLabel lblSpace;
        private System.Windows.Forms.MenuStrip mnsMenu;
        private CustomControl.TabMdi.CustomTabControl tabForm;
        private CustomControl.Navigation.NavigationPane navigationPane1;
        private System.Windows.Forms.ToolStripStatusLabel lblLogout;
        private System.Windows.Forms.ToolStripMenuItem tsmiSetting;
        private System.Windows.Forms.ToolStripMenuItem userManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupsManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createOwnDayPartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaSpendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preAndPosByReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cPRPMorniteringByWeekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem planCPRPByToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aGBReformattingExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tVCarlendarReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reachPreReachByWeeklyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reachSingleBuyingBriefToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reachSummaryGroupOfBuyingBriefToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem implementationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buyingBriefToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem financialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToAdeptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaInvestmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expenditureReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaInvestmentToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optInReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPMInvoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaSubTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vendorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proprietaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proprietaryGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoGrantPermissionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adeptMediaTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productToolStripMenuItem;
    }
}