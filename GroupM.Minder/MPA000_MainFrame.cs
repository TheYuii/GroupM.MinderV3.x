using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Configuration;

namespace  GroupM.Minder
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public partial class MPA000_MainFrame : System.Windows.Forms.Form
	{
        public enum ExitModes{Terminate, Logout }
        private string m_strUser;
        public string UserName
        {
            get { return m_strUser; }
            set { m_strUser = value; }
        }
        private string m_strPass;
        public string Password
        {
            get { return m_strPass; }
            set { m_strPass = value; }
        }

       	public MPA000_MainFrame()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		#region Windows Form Designer generated code

		#endregion

        #region User Define Fucntion
		private Form GetExistingForm(Type frmType)
		{
			// check form is already open
			foreach (Form frm in this.MdiChildren)
			{
				if (frm.GetType() == frmType)
				{
					return frm;
				}
			}
			return null;
		}
		public Form LoadChildForm(string strAssemblyFile, string strFormType, params object []args)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

				Assembly asm = Assembly.LoadFile(strAssemblyFile);
				if (asm == null)
					return null;
				Type type = asm.GetType(strFormType);
				if (type == null)
					return null;
			
				return LoadChildForm(type, args);


			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message,"Exception Message",MessageBoxButtons.OK);
				return null;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}
		public Form LoadChildForm(System.Type frmType, params object []args)
		{
			this.Cursor = Cursors.WaitCursor;
			Form frmChild = null;
			try
			{
				// Get opened form
				frmChild = GetExistingForm(frmType);
				// Get window state of active child form
				Form frmActive = this.ActiveMdiChild;
				FormWindowState state = FormWindowState.Maximized;
				if (frmActive != null)
				{
					state = frmActive.WindowState;
				}
				// if not found exist form, so create new.
				if (frmChild == null)
				{
					frmChild = (Form)Activator.CreateInstance(frmType, args);
					frmChild.MdiParent = this;
					//frmChild.Icon =  new Icon("App.ico");
					frmChild.WindowState = state;
				}
				// Activate form
				frmChild.Show();
				frmChild.Activate();
				frmChild.BringToFront();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message,"Exception Message",MessageBoxButtons.OK);
			}
			this.Cursor = Cursors.Default;

			return frmChild;
		}
		public Form LoadChildFormDialog(System.Type frmType, params object []args)
		{
			this.Cursor = Cursors.WaitCursor;
			Form frmChild = null;
			try
			{
				//Get opened form
				frmChild = GetExistingForm(frmType);
				// if not found exist form, so create new.
				if (frmChild == null)
				{
					frmChild = (Form)Activator.CreateInstance(frmType, args);
					//frmChild.Icon =  new Icon("App.ico");
					frmChild.WindowState = FormWindowState.Normal;
					//frmChild.MdiParent = this;
				}
				//Activate form		
				frmChild.ShowDialog();
				frmChild.Activate();
				frmChild.BringToFront();
				
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message,"Exception Message",MessageBoxButtons.OK);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
			return frmChild;
		}
        public MPA000_MainFrame.ExitModes ExitMode
        {
            get
            {
                return m_exitMode;
            }
        }
        #endregion

        #region Event Handle
        private void MainFrame_Load(object sender, EventArgs e)
        {
            //UpdateMenuBehavior();
            this.statusBarPanelUser.Text = UserName;
            //this.statusBarPanelScreen.Text = "";
            this.statusBarPanelDatabase.Text = ConfigurationManager.AppSettings["DBName"];
            this.statusBarPanelVersion.Text = "Version : " + Application.ProductVersion;
        }
        private void MainFrame_MdiChildActivate(object sender, EventArgs e)
        {

            try
            {
                if (this.ActiveMdiChild != null)
                {
                    this.statusBarPanelScreen.Text = this.ActiveMdiChild.Text;
                }
                else
                {
                    this.statusBarPanelScreen.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message,"Exception Message",MessageBoxButtons.OK);
            }
        }
		private void menuItem4_Click(object sender, System.EventArgs e)
		{
            //switch (AppConfig.DatabaseType)
            //{
            //    case DatabaseTypes.MicrosoftSqlServer:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM010_UserMaintenance), new UserSqlDAO(MainApp.Database), new UserGroupsSqlDAO(MainApp.Database));
            //        break;
            //    case DatabaseTypes.MicrosoftAccess:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM010_UserMaintenance), new UserAccessDAO(MainApp.Database), new UserGroupsAccessDAO(MainApp.Database));
            //        break;
            //}
        }
        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void menuItem3_Click(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA003_MediaSpending2),UserName,Password);
        }
        private void menuItem5_Click(object sender, EventArgs e)
        {
            //switch (AppConfig.DatabaseType)
            //{
            //    case DatabaseTypes.MicrosoftSqlServer:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM020_ScreenMaintenance), new ScreensSqlDAO(MainApp.Database));
            //        break;
            //    case DatabaseTypes.MicrosoftAccess:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM020_ScreenMaintenance), new ScreensAccessDAO(MainApp.Database));
            //        break;
            //}
        }
        private void menuItem6_Click(object sender, EventArgs e)
        {
            //switch (AppConfig.DatabaseType)
            //{
            //    case DatabaseTypes.MicrosoftSqlServer:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM030_UserGroupMaintenance), new UserGroupsSqlDAO(MainApp.Database));
            //        break;
            //    case DatabaseTypes.MicrosoftAccess:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM030_UserGroupMaintenance), new UserGroupsAccessDAO(MainApp.Database));
            //        break;
            //}
        }
        private void menuItem7_Click(object sender, EventArgs e)
        {
            //switch (AppConfig.DatabaseType)
            //{
            //    case DatabaseTypes.MicrosoftSqlServer:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM040_SecurityMapping)
            //                    , new SecuritySqlDAO(MainApp.Database)
            //                    , new UserSqlDAO(MainApp.Database)
            //                    , new UserGroupsSqlDAO(MainApp.Database)
            //                    , new ScreensSqlDAO(MainApp.Database)); 
            //        break;
            //    case DatabaseTypes.MicrosoftAccess:
            //        LoadChildForm(typeof(GRM.SRT.Forms.Admin.frmADM040_SecurityMapping)
            //                    , new SecurityAccessDAO(MainApp.Database)
            //                    , new UserAccessDAO(MainApp.Database)
            //                    , new UserGroupsAccessDAO(MainApp.Database)
            //                    , new ScreensAccessDAO(MainApp.Database)); 
            //        break;
            //}
        }
        private void menuItem8_Click(object sender, EventArgs e)
        {
            m_exitMode = ExitModes.Logout;
            this.Close();
        }
        #endregion

        private void menuItem4_Click_1(object sender, EventArgs e)
        {
            //LoadChildForm(typeof(ConvertToUnicode));
        }

        private void menuItem5_Click_1(object sender, EventArgs e)
        {
            //new Config().ShowDialog();
        }

        private void menuItem6_Click_1(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA004_PlanCPRPByBuyingBrief), UserName, Password);
        }

        private void menuItem4_Click_2(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA005_CPRPMonitoringByWeek), UserName, Password);
        }

        private void menuItem7_Click_1(object sender, EventArgs e)
        {
            //LoadChildForm(typeof(MPA006_ExportReport), UserName, Password);

            Import_AGB_File ImportAGBFile = new Import_AGB_File();
            ImportAGBFile.ShowDialog();

            //LoadChildForm(typeof(MPA006_ExportReport), UserName, Password);
        }

        private void menuItem8_Click_1(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA007_CreateOwnDayPart));
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            LoadChildFormDialog(typeof(MPA008_TVCalendar), UserName, Password);
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA009_Reach));
        }

        private void menuItem12_Click(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA011_Reach_Summary));
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            //LoadChildForm(typeof(MPA003_MediaSpending), UserName, Password);
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA009_Reach_ByWeek));
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
            LoadChildForm(typeof(MPA009_Reach_Test));
        }

	}
}
