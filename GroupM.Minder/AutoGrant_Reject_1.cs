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
    public partial class AutoGrant_Reject_1 : Form
    {
        private string username = "";
        private APIManager m_api;
        private string strRequestNo = "";
        private string strApproverID = "";
        private string strApproverUser = null;
        private string strDelegateUser = null;

        public AutoGrant_Reject_1(string Username, string RequestNo, string ApproverID, string ApproverUser, string DelegateUser)
        {
            InitializeComponent();
            username = Username;
            strRequestNo = RequestNo;
            strApproverID = ApproverID;
            strApproverUser = ApproverUser;
            strDelegateUser = DelegateUser;
            m_api = new APIManager();
        }

        #region function
        private string CreateJsonReject()
        {
            // variable
            string json = "";
            RequestReject reject = new RequestReject();
            reject.Request_No = strRequestNo;
            reject.Approver_ID = strApproverID;
            reject.Reject_Reason = "";
            reject.Reject_Detail = txtReason.Text;
            reject.Approver_UserID = strApproverUser;
            reject.Delegate_UserID = strDelegateUser;
            reject.Approve_Result = "RJ";
            // convert object to json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(reject);
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
                string json = CreateJsonReject();
                string reject = m_api.APIRequestReject(user, json);
                if (reject == "Call API Unsuccessful.")
                    GMessage.MessageError("Some thing is wrong, Your request is failed to approve.");
                else
                    GMessage.MessageInfo(reject);
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
