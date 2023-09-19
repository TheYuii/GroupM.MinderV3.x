using GroupM.UTL;
using GroupM.DBAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App
{
    public partial class frmLogin : Form
    {

        #region ### Constructor ###
        public frmLogin()
        {
            InitializeComponent();
            this.Text += " Version: " + GlobalVariable.Version;
            Permission = false;
        }
        #endregion

        #region ### Member ###
        DBManager db = new DBManager();
        #endregion

        #region ### Properties ###
        public bool Permission { get; set; }
        #endregion

        #region ### Method ###
        public void CheckPermission()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (string.IsNullOrEmpty(this.txtUserName.Text) || string.IsNullOrEmpty(this.txtPassword.Text.Trim()))
                {
                    GMessage.MessageWarning("Please enter username and password.");
                    this.txtPassword.SelectAll();
                    this.txtPassword.Focus();
                    return;
                }

                if (!UserPermisstion.AuthenticateUser(txtUserName.Text, txtPassword.Text))
                {
                    GMessage.MessageWarning("Username and Password are not match.\nPlease use username\\password for accessing your computer.\nExample: Firstname.L");
                    this.txtPassword.Focus();
                    this.txtPassword.SelectAll();
                    this.txtPassword.Focus();
                    return;
                }
                else
                {
                    //string strCommand = string.Empty;
                    //strCommand = " SELECT * FROM UserProfile WHERE Username = '" + this.txtUserName.Text.Trim() + "'";
                    //DataTable dt = DBAccess.DBManager.SelectData(strCommand);
                    DataTable dt = db.CheckUserProfileExisting(this.txtUserName.Text.Trim());
                    if (dt.Rows.Count > 0)
                    {
                        SetCurrentUser();
                        //if (Convert.ToBoolean(dt.Rows[0]["IsActive"]) == true)
                        //{
                        //    SetCurrentUser();
                        //}
                        //else
                        //{
                        //    GMessage.MessageWarning("Username: " + this.txtUserName.Text.Trim() + " is inactive.");
                        //}
                    }
                    else
                    {

                        dt = db.CheckUserProfileExistingMainApp(this.txtUserName.Text.Trim());

                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["IsActive"]) == true)
                            {
                                SetCurrentUser();
                                return;
                            }
                        }
                        GMessage.MessageWarning("Not found username: " + this.txtUserName.Text.Trim() + " in system.");
                    }
                }

            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SetCurrentUser()
        {
            UIConstant.USERID = this.txtUserName.Text.Trim();
            UIConstant.PASSWORD = this.txtPassword.Text.Trim();

            GroupM.Minder.Connection.USERID = this.txtUserName.Text.Trim();
            GroupM.Minder.Connection.PASSWORD = this.txtPassword.Text.Trim();

            GlobalVariable.UserName = this.txtUserName.Text.Trim();
            Permission = true;
            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region ### Event ###
        // ## Form ## 
        private void Login_Load(object sender, EventArgs e)
        {

            DBAccess.DBManager m_db = new DBAccess.DBManager();
            if (m_db.SetScreenPermission("Access without password", this.txtUserName.Text)||GlobalVariable.DevlopmentMode)
            {
                if (GlobalVariable.DevlopmentMode)
                    txtUserName.Text = "Developer.1";
                SetCurrentUser();
                return;
            }
            lblDatabaseAddress.Text = GlobalVariable.DatabaseName;
            lblServiceAddress.Text = GlobalVariable.DatabaseServer;
            lblPublishVersion.Text = GlobalVariable.Version;
            llblPublishLocation.Text = Environment.CurrentDirectory;
            llblSupportEmail.Text = GlobalVariable.SupportEmail;

            txtUserName.Text = System.Environment.UserName;
            txtPassword.Select();

            if (UIConstant.DevelopmentMode)
            {
                txtPassword.Text = "fank";
                btnLogIn.PerformClick();
            }

        }

        // ## Button ##
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            this.CheckPermission();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Application.Exit();
        }
        private void btnHide_Click(object sender, EventArgs e)
        {
            if (this.Height == 560)
            {
                this.Size = new Size(this.Width, 375);
            }
        }

        // ## Panel ##
        private void pnlSpliter_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = (Panel)sender;
            Pen p = new Pen(SystemBrushes.ActiveCaption);

            e.Graphics.DrawLine(p, new Point(0, 0), new Point(pnl.Width - 1, 0));
            e.Graphics.DrawLine(p, new Point(0, pnl.Height - 1), new Point(pnl.Width - 1, pnl.Height - 1));
        }

        // ## Label ##
        private void lblInformation_Click(object sender, EventArgs e)
        {
            if (this.Height == 375)
            {
                this.Size = new Size(this.Width, 560);
            }
            else
            {
                this.Size = new Size(this.Width, 375);
            }
        }

        // ## Control ##
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.CheckPermission();
            }
        }

        #endregion

    }
}
