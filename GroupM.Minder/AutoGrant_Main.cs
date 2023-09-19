using GroupM.Minder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class AutoGrant_Main : Form
    {
        private string username = "";

        public AutoGrant_Main(string Username)
        {
            InitializeComponent();
            username = Username.Replace(".", "");
        }

        #region function
        private void OpenForm(Form frm)
        {
            panelMDI.Controls.Clear();
            frm.TopLevel = false;
            frm.Visible = true;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            panelMDI.Controls.Add(frm);
        }

        public void Home()
        {
            linkLavel1.LinkVisited = false;
            linkLavel2.Text = "";
            linkLavel3.Text = "";
            AutoGrant_RequestList frm = new AutoGrant_RequestList("Edit", username);
            frm.frmMain = this;
            OpenForm(frm);
        }

        public void RequestList(bool load)
        {
            linkLavel1.LinkVisited = true;
            linkLavel2.LinkVisited = false;
            linkLavel2.Text = "Request List";
            linkLavel3.Text = "";
            if (load == true)
            {
                AutoGrant_RequestList frm = new AutoGrant_RequestList("Edit", username);
                frm.frmMain = this;
                OpenForm(frm);
            }
        }

        public void RequestDetail()
        {
            linkLavel1.LinkVisited = true;
            linkLavel2.LinkVisited = true;
            linkLavel3.LinkVisited = false;
            linkLavel2.Text = "Request List  >";
            linkLavel3.Text = "Request Detail";
        }

        public void CreateNewRequest()
        {
            linkLavel1.LinkVisited = true;
            linkLavel2.LinkVisited = false;
            linkLavel2.Text = "New Request";
            linkLavel3.Text = "";
            AutoGrant_Request frm = new AutoGrant_Request("Edit", username);
            OpenForm(frm);
        }

        public void ApproveList(bool load)
        {
            linkLavel1.LinkVisited = true;
            linkLavel2.LinkVisited = false;
            linkLavel2.Text = "Approval List";
            linkLavel3.Text = "";
            if (load == true)
            {
                AutoGrant_Approve frm = new AutoGrant_Approve("Edit", username);
                frm.frmMain = this;
                OpenForm(frm);
            }
        }

        public void ApproveDetail()
        {
            linkLavel1.LinkVisited = true;
            linkLavel2.LinkVisited = true;
            linkLavel3.LinkVisited = false;
            linkLavel2.Text = "Approval List  >";
            linkLavel3.Text = "Approval Detail";
        }
        #endregion

        private void AutoGrant_Main_Load(object sender, EventArgs e)
        {
            Home();
        }

        #region menu
        private void pcbRequestList_Click(object sender, EventArgs e)
        {
            RequestList(true);
        }

        private void pcbRequestList_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pcbRequestList, "Request List");
        }

        private void pcbCreateRequest_Click(object sender, EventArgs e)
        {
            CreateNewRequest();
        }

        private void pcbCreateRequest_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pcbCreateRequest, "Create Request");
        }

        private void pcbApproveList_Click(object sender, EventArgs e)
        {
            ApproveList(true);
        }

        private void pcbApproveList_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pcbApproveList, "Approval List");
        }
        #endregion

        #region navigation
        private void linkLavel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Home();
        }

        private void linkLavel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLavel2.Text == "Request List  >")
            {
                RequestList(true);
            }
            if (linkLavel2.Text == "Approval List  >")
            {
                ApproveList(true);
            }
        }
        #endregion

    }
}
