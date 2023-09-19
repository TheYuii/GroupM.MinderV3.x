using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Deployment.Application;



namespace  GroupM.Minder
{

	public class MainApp 
	{

		//private static SqlConnection m_database = null;
        //private static Icon m_icoApp = null;
        private static string m_strUserID = string.Empty;

		[STAThread]
		static void Main(string[] args) {

			try
			{
                if (args != null && args.Length > 0)
                {
                    args = args[0].Split(new char[] { ',' });
                }
				// Load Icon
				//m_icoApp = new Icon(typeof(GRM.SRT.Main.MainFrame), "App.ico");
				AppConfig.ApplicationPath = System.Environment.CurrentDirectory;

                m_strUserID = "admin";
                
                
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MPA001_Login login = new MPA001_Login();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string[] inputArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                if (inputArgs != null && inputArgs.Length > 0)
                {
                    args = inputArgs[0].Split(new char[] { ',' });
                }
            }
            //GRM.UTL.GMessage.MessageInfo(args.Length.ToString());
            if (args.Length > 1)
            {
                login.txtUserName.Text = args[0];
                login.txtPassword.Text = args[1];
                login.CheckPermisstion();
            }
            else
            {
                Application.Run(login);
            }
            if (login.Permission)
            {
                //MPA003_MediaSpending frm = new MPA003_MediaSpending();
                //frm.UserName = login.UserName;
                //frm.Password = login.Password;
                MPA000_MainFrame frame = new MPA000_MainFrame();
                frame.UserName = login.UserName;
                frame.Password = login.Password;
                frame.Location = AppConfig.Location;
                frame.WindowState = AppConfig.WindowState;
                frame.Size = AppConfig.Size;
                frame.LoadChildForm(typeof(Implementation_BuyingBriefList), login.UserName, login.Password);
                //frame.LoadChildForm(typeof(MPA009_Reach_ByWeek));
                //frame.LoadChildForm(typeof(MPA011_Reach_Summary));
                
                Application.Run(frame);
                //m_database.Close();
                GC.Collect();
            }
                 
                
            }
			catch(Exception ex)
			{
				MessageBox.Show(ex.StackTrace, "ERROR", MessageBoxButtons.OK);
			}
		}
	}

}