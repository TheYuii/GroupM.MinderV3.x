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
    public partial class AutoGrant_Reject : Form
    {
        private string username = "";
        private APIManager m_api;
        private string strRequestNo = "";
        private string strApproverID = "";
        private string strApproverUser = null;
        private string strDelegateUser = null;

        public AutoGrant_Reject(string Username, string RequestNo, string ApproverID, string ApproverUser, string DelegateUser)
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
        private void CheckEnableTextBox()
        {
            if (rdReason1.Checked == true || rdReason2.Checked == true)
            {
                txtReason.Text = "";
                txtReason.Enabled = false;
            }
            else if (rdOther.Checked == true)
            {
                txtReason.Enabled = true;
            }
        }

        private string CreateJsonReject()
        {
            // variable
            string json = "";
            RequestReject reject = new RequestReject();
            reject.Request_No = strRequestNo;
            reject.Approver_ID = strApproverID;
            if (rdReason1.Checked == true)
            {
                reject.Reject_Reason = rdReason1.Text;
                reject.Reject_Detail = "";
            }
            else if (rdReason2.Checked == true)
            {
                reject.Reject_Reason = rdReason2.Text;
                reject.Reject_Detail = "";
            }
            else if (rdOther.Checked == true)
            {
                reject.Reject_Reason = "";
                reject.Reject_Detail = txtReason.Text;
            }
            reject.Approver_UserID = strApproverUser;
            reject.Delegate_UserID = strDelegateUser;
            reject.Approve_Result = "RJ";
            // convert object to json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(reject);
            return json;
        }
        #endregion

        private void AutoGrant_Reject_Load(object sender, EventArgs e)
        {
            rdReason1.Checked = true;
        }

        private void rdReason1_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnableTextBox();
        }

        private void rdReason2_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnableTextBox();
        }

        private void rdOther_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnableTextBox();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (rdOther.Checked == true && txtReason.Text == "")
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
