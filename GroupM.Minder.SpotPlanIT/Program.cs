using GroupM.DBAccess;
using GroupM.UTL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder.SpotPlanIT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            try
            {
                //Load Connection
                Connection.ConnectionStringMinder = UIConstant.ConnectionStringMinder;
                Connection.ConnectionStringMPA = UIConstant.ConnectionStringMPA;
                Connection.ConnectionStringPath = UIConstant.ConnectionStringPath;

                // Load Icon
                //Icon m_icoApp = new Icon(typeof(GRM.SRT.Main.MainFrame), "App.ico");
                AppConfig.ApplicationPath = System.Environment.CurrentDirectory;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //Spot plan : IT
                string[] s = { "{ " +
                    "\"userName\":\"PINYAPATTE\", " +
                    "\"screenName\":\"Implementation_SpotPlan_IT\", " +
                    "\"screenPermission\":\"Edit\", " +
                    "\"BB\":\"2022050101\", " +
                    "\"v\":\"A\", " +
                    "\"s\":\"5\", " +
                    "\"mt\":\"D7\" }" };
                args = s;

                if (args.Length > 0)
                {
                    DBManager m_db = new DBManager();
                    ParameterMinder parameter = JsonConvert.DeserializeObject<ParameterMinder>(args[0]);
                    GlobalVariable.UserName = parameter.userName;
                    string menu = parameter.screenName;
                    string permission = parameter.screenPermission;
                    if (menu == "Implementation_SpotPlan_IT")
                    {
                        string BB = parameter.BB;
                        string v = parameter.v;
                        int status = parameter.s;
                        string mt = parameter.mt;
                        bool validateScreenPermission = m_db.ValidatePermissionScreenSpotPlan(BB, v, permission);
                        if (validateScreenPermission == true)
                        {
                            bool validateBBPermission = m_db.ValidatePermissionBB(BB, GlobalVariable.UserName);
                            if (validateBBPermission == true)
                            {
                                if (permission == "Edit")
                                {
                                    DataTable dt = m_db.CheckOpenSpotPlan(BB, v, GlobalVariable.UserName);
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows[0]["Open"].ToString() == "True")
                                        {
                                            Implementation_SpotPlan_IT frm = new Implementation_SpotPlan_IT(GlobalVariable.UserName, permission, BB, v, status, mt);
                                            frm.WindowState = FormWindowState.Maximized;
                                            frm.ShowDialog();
                                        }
                                        else
                                        {
                                            if (GMessage.MessageComfirm(dt.Rows[0]["Message"].ToString()) == DialogResult.Yes)
                                            {
                                                Implementation_SpotPlan_IT frm = new Implementation_SpotPlan_IT(GlobalVariable.UserName, "View", BB, v, status, mt);
                                                frm.WindowState = FormWindowState.Maximized;
                                                frm.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Implementation_SpotPlan_IT frm = new Implementation_SpotPlan_IT(GlobalVariable.UserName, permission, BB, v, status, mt);
                                    frm.WindowState = FormWindowState.Maximized;
                                    frm.ShowDialog();
                                }
                            }
                            else
                            {
                                GMessage.MessageWarning("You don't have Buying Brief Permission.");
                            }
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
    }
}
