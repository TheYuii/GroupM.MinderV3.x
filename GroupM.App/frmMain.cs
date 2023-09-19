using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroupM.DBAccess;
using GroupM.UTL;
using GroupM.Minder;
using MProprietary;

namespace GroupM.App
{
    public partial class frmMain : Form
    {

        #region ### Constructor ###
        public frmMain()
        {
            InitializeComponent();

            SetPermission();


            this.HideItemStrip();

            this.navigationPane1.HideNavigation();
            this.navigationPane1.GenerateMenu(this.mnsMenu, null);

            this.lblDatabaseName.Text = "Server: " + GlobalVariable.DatabaseServer + "     |  Database: " + GlobalVariable.DatabaseName;
            this.lblVersion.Text = "Version:" + GlobalVariable.Version;
            IsLockOut = false;
        }
        #endregion

        #region ### Member ###
        DBManager db = new DBManager();
        public bool IsLockOut { get; set; }
        #endregion

        #region ### Method ###
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
        public Form LoadChildForm(string strAssemblyFile, string strFormType, params object[] args)
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
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception Message", MessageBoxButtons.OK);
                return null;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        public Form LoadChildForm(System.Type frmType, params object[] args)
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
                    frmChild.Icon = new Icon(Directory.GetCurrentDirectory() + "\\App.ico"
                        );
                    frmChild.WindowState = state;
                }
                // Activate form
                frmChild.Show();
                frmChild.Activate();
                frmChild.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception Message", MessageBoxButtons.OK);
            }
            this.Cursor = Cursors.Default;

            return frmChild;
        }
        public Form LoadChildFormDialog(System.Type frmType, params object[] args)
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
                    frmChild.Icon = new Icon(Directory.GetCurrentDirectory() + "\\App.ico");
                    frmChild.WindowState = FormWindowState.Normal;
                    //frmChild.MdiParent = this;
                }
                //Activate form		
                frmChild.ShowDialog();
                frmChild.Activate();
                frmChild.BringToFront();

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Exception Message", MessageBoxButtons.OK);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return frmChild;
        }

        public void OpenForm(Form frm, params object[] args)
        {
            var x = frm.GetType();
            Form form = (Form)Activator.CreateInstance(frm.GetType(), args);
            this.tabForm.OpenForm(form);

            db.InsertLog(GlobalVariable.UserName, SystemInformation.ComputerName, frm.Text.Trim(),  "OpenForm","");
        }
        public DialogResult OpenFormDialog(Form frm, params object[] args)
        {
            Form form = (Form)Activator.CreateInstance(frm.GetType(), args);
            form.StartPosition = FormStartPosition.CenterScreen;
            var result = form.ShowDialog();
            db.InsertLog(GlobalVariable.UserName, SystemInformation.ComputerName, frm.Text.Trim(), "OpenFormDialog", "");
            return result;
        }


        private void HideItemStrip()
        {
            foreach (ToolStripMenuItem item in this.mnsMenu.Items)
            {
                //item.Visible = false;
                foreach (ToolStripMenuItem dropDownItems in item.DropDownItems)
                {
                    //dropDownItems.Visible = false;
                    if (dropDownItems.HasDropDownItems)
                    {
                        HideSubItemStrip(dropDownItems);
                    }
                }
            }
        }
        private void HideSubItemStrip(ToolStripMenuItem item)
        {
            foreach (ToolStripMenuItem dropDownItems in item.DropDownItems)
            {
                //dropDownItems.Visible = false;
                if (dropDownItems.HasDropDownItems)
                {
                    HideSubItemStrip(dropDownItems);
                }
            }
        }

        private void ShowSubItemStrip(string strText, ToolStripMenuItem item)
        {
            foreach (ToolStripMenuItem dropDownItems in item.DropDownItems)
            {
                if (dropDownItems.HasDropDownItems)
                {
                    HideSubItemStrip(dropDownItems);
                }
                else
                {
                    if (strText == dropDownItems.Text)
                    {
                        dropDownItems.Visible = true;
                    }
                }
            }
        }
        public void SetPermission()
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" SELECT  UserMenu.UserMenuId, UserMenu.UserMenuName, UserMenu.CreateBy, UserMenu.CreateDate, UserMenu.UpdateBy, 
                                        UserMenu.UpdateDate
                                FROM    UserMenu 
                                INNER JOIN UserGroupMenuMapping ON UserMenu.UserMenuId = UserGroupMenuMapping.UserMenuID 
                                INNER JOIN UserGroup ON UserGroupMenuMapping.UserGroupID = UserGroup.UserGroupID 
                                INNER JOIN UserGroupMapping ON UserGroup.UserGroupID = UserGroupMapping.UserGroupID 
                                INNER JOIN UserProfile ON UserGroupMapping.UserProfileID = UserProfile.UserProfileID  
                                WHERE   UserProfile.UserName = '" + GlobalVariable.UserName + @"'";
                //var dt = DBManager.SelectData(strCommand);
                DataTable dt = db.SelectNoParameter(strSQL);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (ToolStripMenuItem item in this.mnsMenu.Items)
                        {
                            string menuName = "";
                            if (row["UserMenuName"].ToString().Split('-').Count() == 1)
                            {
                                menuName = row["UserMenuName"].ToString();
                            }
                            else
                            {
                                menuName = row["UserMenuName"].ToString().Split('-')[1];
                            }

                            if (menuName == item.Text)
                            {
                                item.Visible = true;
                            }
                            if (item.DropDownItems.Count > 0)
                            {
                                ShowSubItemStrip(menuName, item);
                            }

                        }
                    }
                }
                this.navigationPane1.MenuStrip = null;
                this.navigationPane1.MenuStrip = this.mnsMenu;
            }
            catch (Exception)
            {
                throw;
            }
        }
        ArrayList arPermission = new ArrayList();
        private void LoadMenuPermission()
        {
            DBManager db = new DBManager();
            string strSelect = @"
select UserGroupMenuMapping.*,UserMenu.UserMenuName 
from UserGroupMenuMapping 
LEFT JOIN UserMenu 
    ON UserGroupMenuMapping.UserMenuID = UserMenu.UserMenuId
where  UserGroupMenuMapping.UserGroupID IN (
    select UserGroupID from UserGroupMapping where UserProfileID = (
    select UserProfileID from userprofile where username = '" + GlobalVariable.UserName + "' ))";
            DataTable dt = db.SelectNoParameter(strSelect);
            arPermission.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arPermission.Add(dt.Rows[i]["UserMenuName"].ToString());
            }

            for (int i = 0; i < mnsMenu.Items.Count; i++)
            {
                mnsMenu.Items[i].Visible = true;
                string strMenuHeader = mnsMenu.Items[i].Text;
                if (strMenuHeader == "Setting") mnsMenu.Items[i].Visible = false;
                else mnsMenu.Items[i].Visible = true;
                //Have No Permission
            //    string strMenuHeader = mnsMenu.Items[i].Text;

            //    if (strMenuHeader == "File"
            //        || strMenuHeader == "Window"
            //        || arPermission.Contains(strMenuHeader))
            //        mnsMenu.Items[i].Visible = true;
            //    else
            //    {
            //        mnsMenu.Items[i].Visible = false;
            //        continue;
            //    }
                string strMenuName = "";

                for (int j = 0; j < ((ToolStripMenuItem)mnsMenu.Items[i]).DropDownItems.Count; j++)
                {
                    ToolStripMenuItem subMenu = (ToolStripMenuItem)((ToolStripMenuItem)mnsMenu.Items[i]).DropDownItems[j];
                    strMenuName = strMenuHeader + "-" + subMenu.Text;
                    subMenu.Visible = true;

                    //if (strMenuHeader == "File"
                    //    || strMenuHeader == "Window"
                    //    || arPermission.Contains(strMenuName))
                    //    subMenu.Visible = true;
                    //else
                    //{
                    //    subMenu.Visible = false;
                    //    continue;
                    //}

                    //for (int k = 0; k < subMenu.DropDownItems.Count; k++)
                    //{
                    //    ToolStripMenuItem subSubMenu = (ToolStripMenuItem)(subMenu).DropDownItems[k];
                    //    strMenuName = subMenu.Text + "-" + subSubMenu.Text;

                    //    if (strMenuHeader == "File"
                    //        || strMenuHeader == "Window"
                    //        || arPermission.Contains(strMenuName))
                    //        subSubMenu.Visible = true;
                    //    else
                    //    {
                    //        subSubMenu.Visible = false;
                    //        continue;
                    //    }
                    //}

                }
            }
            this.navigationPane1.MenuStrip = null;
            this.navigationPane1.MenuStrip = this.mnsMenu;
            GC.Collect();
        }

        private void Login()
        {
            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.navigationPane1.ShowNavigation();
                this.lblLoginName.Text = GlobalVariable.UserName;
                this.SetPermission();
            }
            else
            {
                Application.Exit();
            }
        }
        #endregion

        #region ### Event ###
        // Form Load
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.navigationPane1.ShowNavigation();
            this.lblLoginName.Text = GlobalVariable.UserName;
            this.LoadMenuPermission();
            //this.OpenForm(new Master.MasterTest.frmTestMasterDisplay(), null);
        }
        #endregion

        private void userManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenForm(new Setting.UserManagement.frmUserManagementDisplay());
        }

        private void groupsManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenForm(new Setting.GroupManagement.frmGroupManagementDisplay());
        }

        private void tabForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabForm.SelectedTab != null)
                this.lblScreenName.Text = this.tabForm.SelectedTab.Text;
            else
                this.lblScreenName.Text = string.Empty;
        }

        private void lblLoginName_Click(object sender, EventArgs e)
        {

        }

        private void lblLogout_Click(object sender, EventArgs e)
        {
            if (GMessage.MessageComfirm("Do you want to Log Out?") == System.Windows.Forms.DialogResult.Yes)
            {
                this.tabForm.CloseAllTab();
                //this.navigationPane1.HideNavigation();
                IsLockOut = true;
                Close();
            }
        }

        private void tabForm_Selecting(object sender, TabControlCancelEventArgs e)
        {

        }

        private void tabForm_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblLogout_Click(null, null);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsLockOut != true)
            {
                if (GMessage.MessageComfirm("Do you want to exit?") != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                    IsLockOut = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void reportReportTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenFormDialog(new frmReport());
        }

        private void tabForm_ControlAdded(object sender, ControlEventArgs e)
        {
            Control ctl = e.Control;
            if (ctl.GetType() == typeof(TabPage))
            {


                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = ctl.Text;
                item.Name = ctl.Name;
                item.Tag = e.Control.Controls[0];
                item.Click += item_Click;
                this.windowToolStripMenuItem.DropDownItems.Add(item);

                this.lblScreenName.Text = ctl.Text;
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ctl = (ToolStripMenuItem)sender;
            Form frm = (Form)ctl.Tag;
            tabForm.OpenForm(frm);

        }

        private void tabForm_ControlRemoved(object sender, ControlEventArgs e)
        {
            Control ctl = e.Control;
            if (ctl.GetType() == typeof(TabPage))
            {
                Form frm = (Form)ctl.Controls[0];

                foreach (ToolStripMenuItem menu in this.windowToolStripMenuItem.DropDownItems)
                {
                    if (menu.Name == "page" + frm.Name)
                    {
                        this.windowToolStripMenuItem.DropDownItems.Remove(menu);
                        return;
                    }
                }
            }

        }

        private void exportPivotExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportPivotTable.ExportPivot expPivot = new ExportPivotTable.ExportPivot();
            expPivot.ShowDialog();
        }

        private void tabForm_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void preAndPosByReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA003_MediaSpending2(UIConstant.USERID, UIConstant.PASSWORD),UIConstant.USERID,UIConstant.PASSWORD);
            this.Cursor = Cursors.Default;
        }

        private void cPRPMorniteringByWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA005_CPRPMonitoringByWeek(UIConstant.USERID, UIConstant.PASSWORD), UIConstant.USERID, UIConstant.PASSWORD);
            this.Cursor = Cursors.Default;
        }

        private void planCPRPByToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA004_PlanCPRPByBuyingBrief(UIConstant.USERID, UIConstant.PASSWORD), UIConstant.USERID, UIConstant.PASSWORD);
            this.Cursor = Cursors.Default;
        }

        private void aGBReformattingExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Import_AGB_File AGBFile = new Import_AGB_File();
            AGBFile.ShowDialog();
            this.Cursor = Cursors.Default;
        }

        private void tVCarlendarReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA008_TVCalendar(UIConstant.USERID, UIConstant.PASSWORD), UIConstant.USERID, UIConstant.PASSWORD);
            this.Cursor = Cursors.Default;
        }

        private void reachPreReachByWeeklyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA009_Reach_ByWeek());
            this.Cursor = Cursors.Default;
        }

        private void reachSingleBuyingBriefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA009_Reach());
            this.Cursor = Cursors.Default;
        }

        private void reachSummaryGroupOfBuyingBriefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA011_Reach_Summary());
            this.Cursor = Cursors.Default;
        }

        private void createOwnDayPartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new MPA007_CreateOwnDayPart());
            this.Cursor = Cursors.Default;
        }

        private void OfficeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_OfficeList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_ClientList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }

        private void vendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_VendorList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }

        private void MediaSubTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_MediaSubTypeList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }

        private void proprietaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProprietaryMaster frm = new frmProprietaryMaster();
            frm.Show();
        }

        private void proprietaryGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProprietary frm = new frmProprietary();
            frm.Show();
        }

        private void BuyingBriefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.OpenForm(new Implementation_BuyingBriefList());
            this.Cursor = Cursors.Default;
        }

        private void optInReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaInvestment_Opt_in_Report frm = new MediaInvestment_Opt_in_Report();
            frm.Show();
        }

        private void exportToAdeptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Financial_ExportToAdept(GlobalVariable.UserName));
            this.Cursor = Cursors.Default;
        }

        private void GPMInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Financial_GPMInvoice frm = new Financial_GPMInvoice();
            frm.Show();
        }

        private void autoGrantPermissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoGrant_Main frm = new AutoGrant_Main(GlobalVariable.UserName);
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void mediaTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_MediaTypeList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }

        private void adeptMediaTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_AdeptMediaTypeList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tabForm.OpenForm(new Master_ProductList(GlobalVariable.UserName, "Edit"));
            this.Cursor = Cursors.Default;
        }
    }
}
