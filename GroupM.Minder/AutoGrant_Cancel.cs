using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class AutoGrant_Cancel : Form
    {
        private string username = "";
        private APIManager m_api;
        private string strRequestNo = "";

        public AutoGrant_Cancel(string Username, string RequestNo)
        {
            InitializeComponent();
            username = Username;
            strRequestNo = RequestNo;
            m_api = new APIManager();
        }

        #region function
        private string CreateJsonCancel()
        {
            // variable
            string json = "";
            RequestCancel cancel = new RequestCancel();
            cancel.Request_No = strRequestNo;
            cancel.Cancel_Reason = txtReason.Text;
            // convert object to json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(cancel);
            return json;
        }
        #endregion

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtReason.Text == "")
            {
                GMessage.MessageWarning("Please input your reason.");
                txtReason.Focus();
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                string user = username;
                string json = CreateJsonCancel();
                string cancel = m_api.APICancelRequest(user, json);
                if (cancel == "Call API Unsuccessful.")
                    GMessage.MessageError("Some thing is wrong, Your request is failed to cancel.");
                else
                    GMessage.MessageInfo(cancel);
                Cursor = Cursors.Default;
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
