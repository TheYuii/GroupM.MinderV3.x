using System.ComponentModel;
using System.Windows.Forms;
namespace  GroupM.Minder
{
    partial class MPA000_MainFrame
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPA000_MainFrame));
            this.mnuMainForm = new System.Windows.Forms.MainMenu(this.components);
            this.mainMenu1 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.mnuWindows = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanelUser = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelScreen = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelDatabase = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelVersion = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMainForm
            // 
            this.mnuMainForm.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mainMenu1,
            this.menuItem1,
            this.mnuWindows});
            // 
            // mainMenu1
            // 
            this.mainMenu1.Index = 0;
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem8,
            this.menuItem5,
            this.menuItem9,
            this.menuItem2});
            this.mainMenu1.Text = "File";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 0;
            this.menuItem8.Text = "Create Own Day Part";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click_1);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "Configuration";
            this.menuItem5.Visible = false;
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click_1);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "Exit";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem13,
            this.menuItem4,
            this.menuItem6,
            this.menuItem7,
            this.menuItem10,
            this.menuItem14,
            this.menuItem11,
            this.menuItem12});
            this.menuItem1.Text = "Minder Report";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "0. Pre and Post Buy Report";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "1. Pre and Post Buy Report - Old Version";
            this.menuItem13.Visible = false;
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "2. CPRP Monitoring By Week";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click_2);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 3;
            this.menuItem6.Text = "3. Plan CPRP By Buying Brief";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click_1);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "4. AGB Reformating Excel";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click_1);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 5;
            this.menuItem10.Text = "5. TV Calendar Report";
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 6;
            this.menuItem14.Text = "6. Reach - Pre Reach By Weekly";
            this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 7;
            this.menuItem11.Text = "7. Reach - Single Buying Brief";
            this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 8;
            this.menuItem12.Text = "8. Reach - Summary Group of Buying Brief";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
            // 
            // mnuWindows
            // 
            this.mnuWindows.Index = 2;
            this.mnuWindows.MdiList = true;
            this.mnuWindows.Text = "Windows";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 649);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanelUser,
            this.statusBarPanelScreen,
            this.statusBarPanelDatabase,
            this.statusBarPanelVersion});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1016, 22);
            this.statusBar1.TabIndex = 3;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarPanelUser
            // 
            this.statusBarPanelUser.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelUser.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanelUser.Icon")));
            this.statusBarPanelUser.Name = "statusBarPanelUser";
            this.statusBarPanelUser.Text = "Login Name";
            this.statusBarPanelUser.Width = 96;
            // 
            // statusBarPanelScreen
            // 
            this.statusBarPanelScreen.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanelScreen.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanelScreen.Icon")));
            this.statusBarPanelScreen.Name = "statusBarPanelScreen";
            this.statusBarPanelScreen.Text = "Screen Name";
            this.statusBarPanelScreen.Width = 503;
            // 
            // statusBarPanelDatabase
            // 
            this.statusBarPanelDatabase.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanelDatabase.Icon")));
            this.statusBarPanelDatabase.Name = "statusBarPanelDatabase";
            this.statusBarPanelDatabase.Text = "Database Name";
            this.statusBarPanelDatabase.Width = 300;
            // 
            // statusBarPanelVersion
            // 
            this.statusBarPanelVersion.Name = "statusBarPanelVersion";
            this.statusBarPanelVersion.Text = "Version : 1.1.0";
            // 
            // MPA000_MainFrame
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1016, 671);
            this.Controls.Add(this.statusBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mnuMainForm;
            this.Name = "MPA000_MainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Minder Post-Buy Analysis";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainFrame_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainFrame_MdiChildActivate);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelVersion)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MainMenu mnuMainForm;
        private System.Windows.Forms.MenuItem mainMenu1;
        private System.Windows.Forms.MenuItem mnuWindows;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem3;
        private StatusBar statusBar1;
        private StatusBarPanel statusBarPanelUser;
        private StatusBarPanel statusBarPanelScreen;
        private StatusBarPanel statusBarPanelDatabase;
        private StatusBarPanel statusBarPanelVersion;
        private MenuItem menuItem9;
        private MPA000_MainFrame.ExitModes m_exitMode = ExitModes.Terminate;

        #endregion

        private MenuItem menuItem6;
        private MenuItem menuItem5;
        private MenuItem menuItem4;
        private MenuItem menuItem7;
        private MenuItem menuItem8;
        private MenuItem menuItem10;
        private MenuItem menuItem11;
        private MenuItem menuItem12;
        private MenuItem menuItem13;
        private MenuItem menuItem14;

    }
}

