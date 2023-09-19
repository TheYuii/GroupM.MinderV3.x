using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroupM.DBAccess;
using GroupM.UTL;
using Newtonsoft.Json;

namespace GroupM.App
{
    static class MainApp
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                //Load Connection
                GroupM.Minder.Connection.ConnectionStringMinder = UIConstant.ConnectionStringMinder;
                //GroupM.Minder.Connection.ConnectionStringMPA = UIConstant.ConnectionStringMPA;
                GroupM.Minder.Connection.ConnectionStringPath = UIConstant.ConnectionStringPath;

                // Load Icon
                //Icon m_icoApp = new Icon(typeof(GRM.SRT.Main.MainFrame), "App.ico");
                AppConfig.ApplicationPath = Environment.CurrentDirectory;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //============================ Test Login ==============================
                //GroupM.Allocator.Master.AllocatorConstants.UserName = "pariwat.k";
                //============================ End Login ==========================

                //Spot plan : FC
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV_FC\", " +
                //    "\"screenPermission\":\"View\", " +
                //    "\"BB\":\"2023040048\", " +
                //    "\"v\":\"A1\", " +
                //    "\"s\":\"4\", " +
                //    "\"mst\":\"TVDT\" }" };
                //args = s;

                //Spot plan : IT
                //string[] s = { "{ " +
                //    "\"userName\":\"PINYAPATTE\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_IT\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2023060999\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"4\", " +
                //    "\"mt\":\"DN\" }" };
                //args = s;
                //2020122076

                //Spot plan : CO
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV_FC\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2022011963\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"mst\":\"FC\" }" };
                //args = s;

                //Print SC - FC
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV_FC_Print\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2021081048\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"4\", " +
                //    "\"SD\":\"20210801\", " +
                //    "\"ED\":\"20210831\", " +
                //    "\"Print\":\"SC\", " +
                //    "\"Select\":\"'FC001'\", " +
                //    "\"Template\":\"R:/Minder/Test/Report/Schedule_FC.xltx\", " +
                //    "\"Parameter\":\"R:/Minder/Test/SIT/Parameter.mdb\", " +
                //    "\"mst\":\"FC\" }" };
                //args = s;

                //Print SC - CO
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV_FC_Print\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2020122076\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"4\", " +
                //    "\"SD\":\"20201201\", " +
                //    "\"ED\":\"20201231\", " +
                //    "\"Print\":\"SC\", " +
                //    "\"Select\":\"'CO001'\", " +
                //    "\"Template\":\"R:/Minder/Test/Report/Schedule_FC.xltx\", " +
                //    "\"Parameter\":\"R:/Minder/Test/SIT/Parameter.mdb\", " +
                //    "\"mst\":\"CO\" }" };
                //args = s;

                //Print PO - FC
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV_FC_Print\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2021062344\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"SD\":\"20210601\", " +
                //    "\"ED\":\"20210630\", " +
                //    "\"Print\":\"PO\", " +
                //    "\"Select\":\"'MMINTE'\", " +
                //    "\"Template\":\"R:/Minder/Test/Report/PO_FC.xltx\", " +
                //    "\"Parameter\":\"R:/Minder/Test/SIT/Parameter.mdb\", " +
                //    "\"mst\":\"FC\" }" };
                //args = s;

                //Print PO - CO
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV_FC_Print\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2020122076\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"SD\":\"20201201\", " +
                //    "\"ED\":\"20201231\", " +
                //    "\"Print\":\"PO\", " +
                //    "\"Select\":\"'MT086'\", " +
                //    "\"Template\":\"R:/Minder/Test/Report/PO_FC.xltx\", " +
                //    "\"Parameter\":\"R:/Minder/Test/SIT/Parameter.mdb\", " +
                //    "\"mst\":\"CO\" }" };
                //args = s;

                //OnAirChecking
                //string[] s = { "{ " +
                //    "\"userName\":\"PINYAPATTE\", " +
                //    "\"screenName\":\"Master_ProductList\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2022020390\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"mst\":\"IT\" }" };
                //args = s;

                //Spot plan : Multiple Media type - IT
                //string[] s = { "{ " +
                //    "\"userName\":\"PINYAPATTE\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_IT\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2023080291\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"mmt\":\"IT\" }" };
                //args = s;
                //2020122076

                //Spot plan : TV
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_TV\", " +
                //    "\"screenPermission\":\"View\", " +
                //    "\"BB\":\"2023030525\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"mst\":\"FC\" }" };
                //args = s;

                //Spot plan : ES
                //string[] s = { "{ " +
                //    "\"userName\":\"CHAIWATI\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_ES\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2023100443\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"0\", " +
                //    "\"mt\":\"ES\" }" };
                //args = s;

                //Spot plan : OD
                //string[] s = { "{ " +
                //    "\"userName\":\"SIWAP\", " +
                //    "\"screenName\":\"Implementation_SpotPlan_OD\", " +
                //    "\"screenPermission\":\"Edit\", " +
                //    "\"BB\":\"2099011802\", " +
                //    "\"v\":\"A\", " +
                //    "\"s\":\"5\", " +
                //    "\"mt\":\"OD\" }" };
                //args = s;
                //New
                //string[] s = {"{" +
                //        "\"userName\":\"CHAIWATI\"," +
                //        "\"screenName\":\"Implementation_SpotPlan_OD\", " +
                //        "\"screenPermission\":\"Edit\", " +
                //        "\"BB\":\"2023090754\", " +
                //        "\"v\":\"A\", " +
                //        "\"s\":\"0\", " +
                //        "\"mt\":\"OD\"}" };
                //args = s;

               

                if (args.Length > 0)
                {
                    DBManager m_db = new DBManager();
                    ParameterMinder parameter = JsonConvert.DeserializeObject<ParameterMinder>(args[0]);
                    GlobalVariable.UserName = parameter.userName;
                    string menu = parameter.screenName;
                    string permission = parameter.screenPermission;
                    if (menu == "Master_OfficeList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_OfficeList frm = new Minder.Master_OfficeList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                    if (menu == "Master_ClientList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_ClientList frm = new Minder.Master_ClientList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }

                    if (menu == "Master_ProductList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_ProductList frm = new Minder.Master_ProductList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                    if (menu == "Master_VendorList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_VendorList frm = new Minder.Master_VendorList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                    if (menu == "Master_MediaTypeList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_MediaTypeList frm = new Minder.Master_MediaTypeList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                    if (menu == "Master_MediaSubTypeList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_MediaSubTypeList frm = new Minder.Master_MediaSubTypeList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                    if (menu == "Master_AdeptMediaTypeList")
                    {
                        bool validateScreenPermission = m_db.ValidatePermissionScreenMaster(menu, permission, GlobalVariable.UserName);
                        if (validateScreenPermission == true)
                        {
                            Minder.Master_AdeptMediaTypeList frm = new Minder.Master_AdeptMediaTypeList(GlobalVariable.UserName, permission);
                            frm.WindowState = FormWindowState.Maximized;
                            frm.ShowDialog();
                        }
                        else
                        {
                            GMessage.MessageWarning("Invalid Screen Mode Permission.");
                        }
                    }
                    if (menu == "Implementation_BuyingBriefList")
                    {
                        Minder.Implementation_BuyingBriefList frm = new Minder.Implementation_BuyingBriefList();
                        frm.WindowState = FormWindowState.Maximized;
                        frm.ShowDialog();
                    }
                    if (menu == "Implementation_BuyingBrief_IncidentTrackingList")
                    {
                        string BB = parameter.BB;
                        Minder.Implementation_BuyingBrief_IncidentTrackingList frm = new Minder.Implementation_BuyingBrief_IncidentTrackingList(GlobalVariable.UserName, BB);
                        frm.WindowState = FormWindowState.Maximized;
                        frm.ShowDialog();
                    }
                    if (menu == "Implementation_SpotPlan_TV_FC")
                    {
                        string BB = parameter.BB;
                        string v = parameter.v;
                        int status = parameter.s;
                        string mst = parameter.mst;
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
                                            Minder.Implementation_SpotPlan_TV_FC frm = new Minder.Implementation_SpotPlan_TV_FC(GlobalVariable.UserName, permission, BB, v, status, mst);
                                            frm.WindowState = FormWindowState.Maximized;
                                            frm.ShowDialog();
                                        }
                                        else
                                        {
                                            if (GMessage.MessageComfirm(dt.Rows[0]["Message"].ToString()) == DialogResult.Yes)
                                            {
                                                Minder.Implementation_SpotPlan_TV_FC frm = new Minder.Implementation_SpotPlan_TV_FC(GlobalVariable.UserName, "View", BB, v, status, mst);
                                                frm.WindowState = FormWindowState.Maximized;
                                                frm.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Minder.Implementation_SpotPlan_TV_FC frm = new Minder.Implementation_SpotPlan_TV_FC(GlobalVariable.UserName, permission, BB, v, status, mst);
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
                    //if (menu == "Implementation_SpotPlan_TV_FC_Print")
                    //{
                    //    string BB = parameter.BB;
                    //    string v = parameter.v;
                    //    string SD = parameter.SD;
                    //    string ED = parameter.ED;
                    //    string Type = parameter.Print;
                    //    string Select = parameter.Select;
                    //    string Template = parameter.Template;
                    //    string Parameter = parameter.Parameter;
                    //    string mst = parameter.mst;
                    //    if (Type == "SC")
                    //    {
                    //        Minder.Implementation_SpotPlan_Print_Schedule PSC = new Minder.Implementation_SpotPlan_Print_Schedule("", "", "", "", "", "", "", "", "","");
                    //        PSC.PrintMediaSchedule(Template, BB, v, SD, ED, Select, mst);
                    //    }
                    //    if (Type == "PO")
                    //    {
                    //        Minder.Implementation_SpotPlan_Print_PO PPO = new Minder.Implementation_SpotPlan_Print_PO("", "", "", "", "", "", "","");
                    //        PPO.PrintPO(Template, Parameter, BB, v, SD, ED, Select, mst);
                    //    }
                    //}
                    if (menu == "Implementation_SpotPlan_TV")
                    {
                        string BB = parameter.BB;
                        string v = parameter.v;
                        int status = parameter.s;
                        string mst = parameter.mst;
                        bool validateScreenPermission = m_db.ValidatePermissionScreenSpotPlan(BB, v, permission);
                        //validateScreenPermission = true;
                        if (validateScreenPermission == true)
                        {
                            bool validateBBPermission = m_db.ValidatePermissionBB(BB, GlobalVariable.UserName);
                            validateBBPermission = true;
                            if (validateBBPermission == true)
                            {
                                if (permission == "Edit")
                                {
                                    DataTable dt = m_db.CheckOpenSpotPlan(BB, v, GlobalVariable.UserName);
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows[0]["Open"].ToString() == "True")
                                        {
                                            Minder.Implementation_SpotPlan_TV frm = new Minder.Implementation_SpotPlan_TV(GlobalVariable.UserName, permission, BB, v, status, mst);
                                            frm.WindowState = FormWindowState.Maximized;
                                            frm.ShowDialog();
                                        }
                                        else
                                        {
                                            if (GMessage.MessageComfirm(dt.Rows[0]["Message"].ToString()) == DialogResult.Yes)
                                            {
                                                Minder.Implementation_SpotPlan_TV frm = new Minder.Implementation_SpotPlan_TV(GlobalVariable.UserName, "View", BB, v, status, mst);
                                                frm.WindowState = FormWindowState.Maximized;
                                                frm.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Minder.Implementation_SpotPlan_TV frm = new Minder.Implementation_SpotPlan_TV(GlobalVariable.UserName, permission, BB, v, status, mst);
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
                    if (menu == "Implementation_SpotPlan_IT")
                    {
                        string BB = parameter.BB;
                        string v = parameter.v;
                        int status = parameter.s;
                        string mt = parameter.mt;
                        //Master Media Type - Multiply Media Type Feature
                        string mmt = parameter.mmt;

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
                                            Minder.Implementation_SpotPlan_IT frm;
                                            if(mmt == null)
                                                frm = new Minder.Implementation_SpotPlan_IT(GlobalVariable.UserName, permission, BB, v, status, mt);
                                            else
                                                frm = new Minder.Implementation_SpotPlan_IT(GlobalVariable.UserName, permission, BB, v, status, mt,mmt);
                                            frm.WindowState = FormWindowState.Maximized;
                                            frm.ShowDialog();
                                        }
                                        else
                                        {
                                            if (GMessage.MessageComfirm(dt.Rows[0]["Message"].ToString()) == DialogResult.Yes)
                                            {
                                                Minder.Implementation_SpotPlan_IT frm;
                                                if (mmt == null)
                                                    frm = new Minder.Implementation_SpotPlan_IT(GlobalVariable.UserName, "View", BB, v, status, mt);
                                                else
                                                    frm = new Minder.Implementation_SpotPlan_IT(GlobalVariable.UserName, "View", BB, v, status, mt,mmt);
                                                frm.WindowState = FormWindowState.Maximized;
                                                frm.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Minder.Implementation_SpotPlan_IT frm = new Minder.Implementation_SpotPlan_IT(GlobalVariable.UserName, permission, BB, v, status, mt);
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
                    if (menu == "Implementation_SpotPlan_ES")
                    {
                        string BB = parameter.BB;
                        string v = parameter.v;
                        int status = parameter.s;
                        string mt = parameter.mt;
                        //Master Media Type - Multiply Media Type Feature
                        string mmt = parameter.mmt;

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
                                            Minder.Implementation_SpotPlan_ES frm;
                                            if (mmt == null)
                                                frm = new Minder.Implementation_SpotPlan_ES(GlobalVariable.UserName, permission, BB, v, status, mt);
                                            else
                                                frm = new Minder.Implementation_SpotPlan_ES(GlobalVariable.UserName, permission, BB, v, status, mt, mmt);
                                            frm.WindowState = FormWindowState.Maximized;
                                            frm.ShowDialog();
                                        }
                                        else
                                        {
                                            if (GMessage.MessageComfirm(dt.Rows[0]["Message"].ToString()) == DialogResult.Yes)
                                            {
                                                Minder.Implementation_SpotPlan_ES frm;
                                                if (mmt == null)
                                                    frm = new Minder.Implementation_SpotPlan_ES(GlobalVariable.UserName, "View", BB, v, status, mt);
                                                else
                                                    frm = new Minder.Implementation_SpotPlan_ES(GlobalVariable.UserName, "View", BB, v, status, mt, mmt);
                                                frm.WindowState = FormWindowState.Maximized;
                                                frm.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Minder.Implementation_SpotPlan_ES frm = new Minder.Implementation_SpotPlan_ES(GlobalVariable.UserName, permission, BB, v, status, mt);
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
                    if (menu == "Implementation_SpotPlan_OD")
                    {
                        string BB = parameter.BB;
                        string v = parameter.v;
                        int status = parameter.s;
                        string mt = parameter.mt;
                        bool validateScreenPermission = m_db.ValidatePermissionScreenSpotPlan(BB, v, permission);
                        validateScreenPermission = true;
                        if (validateScreenPermission == true)
                        {
                            bool validateBBPermission = m_db.ValidatePermissionBB(BB, GlobalVariable.UserName);
                            validateBBPermission = true;
                            if (validateBBPermission == true)
                            {
                                if (permission == "Edit")
                                {
                                    DataTable dt = m_db.CheckOpenSpotPlan(BB, v, GlobalVariable.UserName);
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows[0]["Open"].ToString() == "True")
                                        {
                                            Minder.Implementation_SpotPlan_OD frm = new Minder.Implementation_SpotPlan_OD(GlobalVariable.UserName, permission, BB, v, status, mt);
                                            frm.WindowState = FormWindowState.Maximized;
                                            frm.ShowDialog();
                                        }
                                        else
                                        {
                                            if (GMessage.MessageComfirm(dt.Rows[0]["Message"].ToString()) == DialogResult.Yes)
                                            {
                                                Minder.Implementation_SpotPlan_OD frm = new Minder.Implementation_SpotPlan_OD(GlobalVariable.UserName, "View", BB, v, status, mt);
                                                frm.WindowState = FormWindowState.Maximized;
                                                frm.ShowDialog();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Minder.Implementation_SpotPlan_OD frm = new Minder.Implementation_SpotPlan_OD(GlobalVariable.UserName, permission, BB, v, status, mt);
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
                    if (menu == "Implementation_OnAirCheckingReport")
                    {
                        Minder.Implementation_OnAirCheckingReport frm = new Minder.Implementation_OnAirCheckingReport();
                        frm.ShowDialog();
                    }
                    if (menu == "MediaInvestment_Opt_in_Report")
                    {
                        Minder.MediaInvestment_Opt_in_Report frm = new Minder.MediaInvestment_Opt_in_Report();
                        frm.ShowDialog();
                    }
                    if (menu == "Financial_ExportToAdept")
                    {
                        Minder.Financial_ExportToAdept frm = new Minder.Financial_ExportToAdept(GlobalVariable.UserName);
                        frm.Text += " - Server : " + GlobalVariable.DatabaseServer + ", Database : " + GlobalVariable.DatabaseName;
                        frm.WindowState = FormWindowState.Maximized;
                        frm.ShowDialog();
                    }
                    if (menu == "Financial_GPMInvoice")
                    {
                        Minder.Financial_GPMInvoice frm = new Minder.Financial_GPMInvoice();
                        frm.ShowDialog();
                    }
                    if (menu == "Auto_Grant_Permission")
                    {
                        Minder.AutoGrant_Main frm = new Minder.AutoGrant_Main(GlobalVariable.UserName);
                        frm.WindowState = FormWindowState.Maximized;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    while (true)
                    {
                        frmLogin login = new frmLogin();
                        login.Icon = GlobalVariable.AppIcon;
                        if (ApplicationDeployment.IsNetworkDeployed)
                        {
                            var inputArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                            if (inputArgs != null && inputArgs.Length > 0)
                            {
                                args = inputArgs[0].Split(new char[] { ',' });
                            }
                        }

                        if (args.Length > 1)
                        {
                            login.txtUserName.Text = args[0];
                            login.txtPassword.Text = args[1];
                            login.CheckPermission();
                        }
                        else
                        {
                            //Application.Run(login);
                            if (login.ShowDialog() == DialogResult.OK)
                            {
                                if (login.Permission)
                                {
                                    frmMain frame = new frmMain();
                                    frame.Location = AppConfig.Location;
                                    frame.WindowState = AppConfig.WindowState;
                                    frame.Size = AppConfig.Size;
                                    frame.Icon = GlobalVariable.AppIcon;
                                    frame.Text = frame.Text + " - " + AppConfig.ApplicationPath;
                                    //Minder.AutoGrant_Request frm = new Minder.AutoGrant_Request();
                                    //frm.UserName = login.UserName;
                                    //frm.Password = login.Password;
                                    //frame.LoadChildForm(typeof(MPA003_MediaSpending), login.UserName, login.Password);

                                    DBManager db = new DBManager();
                                    db.InsertLog(GlobalVariable.UserName, SystemInformation.ComputerName, "Login", "Log in", "");

                                    Application.Run(frame);
                                    if (!frame.IsLockOut)
                                        break;
                                    GC.Collect();
                                }
                            }
                            else { break; }
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
