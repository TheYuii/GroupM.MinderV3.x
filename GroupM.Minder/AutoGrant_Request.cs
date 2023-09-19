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
using GroupM.DBAccess;
using GroupM.UTL;

namespace GroupM.Minder
{
    public partial class AutoGrant_Request : Form
    {
        private enum eScreenMode { Add, Edit, View }

        private enum ePermissionOption
        {
            AgencyRequest,
            OfficeRequest,
            ClientRequest,
            CopyPermissionRequest
        }

        private string username = "";
        private string option = "";
        private bool selectChk = false;
        private DataTable dtRequestOtherUser = null;
        private DataTable dtMediaType = null;
        private DataTable dtClient = null;
        List<CopyPermission> listCopyPermission = null;
        private APIManager m_api;

        public AutoGrant_Request(string screenMode, string Username)
        {
            InitializeComponent();
            username = Username;
            m_api = new APIManager();
        }

        #region function
        private void InitialForOptionRequest()
        {
            dtRequestOtherUser = null;
            dtClient = null;
            rdRequestForMe.Checked = true;
            chkSelectAllMediaType.CheckState = CheckState.Unchecked;
            chkCinema.Checked = false;
            chkContentAndActivation.Checked = false;
            chkInternet.Checked = false;
            chkMagazine.Checked = false;
            chkNewspaper.Checked = false;
            chkOutdoor.Checked = false;
            chkRadio.Checked = false;
            chkRetainerFee.Checked = false;
            chkStrategyAndData.Checked = false;
            chkTelevision.Checked = false;
            chkTVSponsorship.Checked = false;
            chkSelectAllMediaType1.CheckState = CheckState.Unchecked;
            chkCinema1.Checked = false;
            chkContentAndActivation1.Checked = false;
            chkInternet1.Checked = false;
            chkMagazine1.Checked = false;
            chkNewspaper1.Checked = false;
            chkOutdoor1.Checked = false;
            chkRadio1.Checked = false;
            chkRetainerFee1.Checked = false;
            chkStrategyAndData1.Checked = false;
            chkTelevision1.Checked = false;
            chkTVSponsorship1.Checked = false;
        }

        private void CheckSelectMediaType()
        {
            if (!selectChk)
            {
                if (chkCinema.Checked == true &&
                chkContentAndActivation.Checked == true &&
                chkInternet.Checked == true &&
                chkMagazine.Checked == true &&
                chkNewspaper.Checked == true &&
                chkOutdoor.Checked == true &&
                chkRadio.Checked == true &&
                chkRetainerFee.Checked == true &&
                chkStrategyAndData.Checked == true &&
                chkTelevision.Checked == true &&
                chkTVSponsorship.Checked == true)
                    chkSelectAllMediaType.CheckState = CheckState.Checked;
                else if (chkCinema.Checked == false &&
                chkContentAndActivation.Checked == false &&
                chkInternet.Checked == false &&
                chkMagazine.Checked == false &&
                chkNewspaper.Checked == false &&
                chkOutdoor.Checked == false &&
                chkRadio.Checked == false &&
                chkRetainerFee.Checked == false &&
                chkStrategyAndData.Checked == false &&
                chkTelevision.Checked == false &&
                chkTVSponsorship.Checked == false)
                    chkSelectAllMediaType.CheckState = CheckState.Unchecked;
                else
                    chkSelectAllMediaType.CheckState = CheckState.Indeterminate;
            }
        }

        private void CheckSelectMediaType1()
        {
            if (!selectChk)
            {
                if (chkCinema1.Checked == true &&
                chkContentAndActivation1.Checked == true &&
                chkInternet1.Checked == true &&
                chkMagazine1.Checked == true &&
                chkNewspaper1.Checked == true &&
                chkOutdoor1.Checked == true &&
                chkRadio1.Checked == true &&
                chkRetainerFee1.Checked == true &&
                chkStrategyAndData1.Checked == true &&
                chkTelevision1.Checked == true &&
                chkTVSponsorship1.Checked == true)
                    chkSelectAllMediaType1.CheckState = CheckState.Checked;
                else if (chkCinema1.Checked == false &&
                chkContentAndActivation1.Checked == false &&
                chkInternet1.Checked == false &&
                chkMagazine1.Checked == false &&
                chkNewspaper1.Checked == false &&
                chkOutdoor1.Checked == false &&
                chkRadio1.Checked == false &&
                chkRetainerFee1.Checked == false &&
                chkStrategyAndData1.Checked == false &&
                chkTelevision1.Checked == false &&
                chkTVSponsorship1.Checked == false)
                    chkSelectAllMediaType1.CheckState = CheckState.Unchecked;
                else
                    chkSelectAllMediaType1.CheckState = CheckState.Indeterminate;
            }
        }

        private bool CheckListUser()
        {
            bool pass = true;
            if (rdRequestForOther.Checked)
            {
                DataTable dtUser = (DataTable)gvRequestOtherTo.DataSource;
                if (dtUser.Rows.Count == 0)
                    pass = false;
            }
            return pass;
        }

        private bool CheckListMediaType()
        {
            bool pass = true;
            if (option == ePermissionOption.CopyPermissionRequest.ToString())
            {
                if (chkSelectAllMediaType1.CheckState == CheckState.Unchecked)
                    pass = false;
            }
            else
            {
                if (chkSelectAllMediaType.CheckState == CheckState.Unchecked)
                    pass = false;
            }
            return pass;
        }

        private bool CheckListPermission()
        {
            bool pass = true;
            DataTable dtPermission = (DataTable)gvClientTo.DataSource;
            if (dtPermission.Rows.Count == 0)
                pass = false;
            return pass;
        }

        private bool CheckSelectedMediaType()
        {
            bool pass = true;
            List<CreateRequestMediaType> listMediaType = new List<CreateRequestMediaType>();
            // create list media type
            if (option == ePermissionOption.CopyPermissionRequest.ToString())
            {
                if (chkStrategyAndData1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "BP";
                    listMediaType.Add(mediaType);
                }
                if (chkCinema1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "CN";
                    listMediaType.Add(mediaType);
                }
                if (chkContentAndActivation1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "ES";
                    listMediaType.Add(mediaType);
                }
                if (chkInternet1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "IT";
                    listMediaType.Add(mediaType);
                }
                if (chkMagazine1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "MG";
                    listMediaType.Add(mediaType);
                }
                if (chkNewspaper1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "NP";
                    listMediaType.Add(mediaType);
                }
                if (chkOutdoor1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "OD";
                    listMediaType.Add(mediaType);
                }
                if (chkRadio1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RD";
                    listMediaType.Add(mediaType);
                }
                if (chkRetainerFee1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RT";
                    listMediaType.Add(mediaType);
                }
                if (chkTVSponsorship1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TS";
                    listMediaType.Add(mediaType);
                }
                if (chkTelevision1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TV";
                    listMediaType.Add(mediaType);
                }
            }
            else
            {
                if (chkStrategyAndData.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "BP";
                    listMediaType.Add(mediaType);
                }
                if (chkCinema.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "CN";
                    listMediaType.Add(mediaType);
                }
                if (chkContentAndActivation.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "ES";
                    listMediaType.Add(mediaType);
                }
                if (chkInternet.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "IT";
                    listMediaType.Add(mediaType);
                }
                if (chkMagazine.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "MG";
                    listMediaType.Add(mediaType);
                }
                if (chkNewspaper.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "NP";
                    listMediaType.Add(mediaType);
                }
                if (chkOutdoor.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "OD";
                    listMediaType.Add(mediaType);
                }
                if (chkRadio.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RD";
                    listMediaType.Add(mediaType);
                }
                if (chkRetainerFee.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RT";
                    listMediaType.Add(mediaType);
                }
                if (chkTVSponsorship.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TS";
                    listMediaType.Add(mediaType);
                }
                if (chkTelevision.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TV";
                    listMediaType.Add(mediaType);
                }
            }
            if (dtMediaType.Rows.Count == listMediaType.Count)
            {
                for (int i = 0; i < dtMediaType.Rows.Count; i++)
                {
                    if (pass)
                    {
                        if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() != listMediaType[i].Media_Type)
                            pass = false;
                    }
                }
            }
            else
            {
                pass = false;
            }
            return pass;
        }

        private string CreateJsonRequest()
        {
            // variable
            string json = "";
            CreateRequest request = new CreateRequest();
            List<CreateRequestUser> listUser = new List<CreateRequestUser>();
            List<CreateRequestMediaType> listMediaType = new List<CreateRequestMediaType>();
            List<CopyPermission> listPermission = new List<CopyPermission>();

            // create list user
            if (rdRequestForMe.Checked)
            {
                DataTable dtUser = dtRequestOtherUser;
                CreateRequestUser user = new CreateRequestUser();
                user.User_ID = dtUser.Rows[0]["User_ID"].ToString();
                user.User_Email = dtUser.Rows[0]["User_Email"].ToString();
                user.Agency_ID = dtUser.Rows[0]["Agency_ID"].ToString();
                user.Office_ID = dtUser.Rows[0]["Office_ID"].ToString();
                listUser.Add(user);
            }
            else
            {
                DataTable dtUser = (DataTable)gvRequestOtherTo.DataSource;
                for (int i = 0; i < dtUser.Rows.Count; i++)
                {
                    CreateRequestUser user = new CreateRequestUser();
                    user.User_ID = dtUser.Rows[i]["User_ID"].ToString();
                    user.User_Email = dtUser.Rows[i]["User_Email"].ToString();
                    user.Agency_ID = dtUser.Rows[i]["Agency_ID"].ToString();
                    user.Office_ID = dtUser.Rows[i]["Office_ID"].ToString();
                    listUser.Add(user);
                }
            }

            // create list media type
            if (option == ePermissionOption.CopyPermissionRequest.ToString())
            {
                if (chkCinema1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "CN";
                    listMediaType.Add(mediaType);
                }
                if (chkContentAndActivation1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "ES";
                    listMediaType.Add(mediaType);
                }
                if (chkInternet1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "IT";
                    listMediaType.Add(mediaType);
                }
                if (chkMagazine1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "MG";
                    listMediaType.Add(mediaType);
                }
                if (chkNewspaper1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "NP";
                    listMediaType.Add(mediaType);
                }
                if (chkOutdoor1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "OD";
                    listMediaType.Add(mediaType);
                }
                if (chkRadio1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RD";
                    listMediaType.Add(mediaType);
                }
                if (chkRetainerFee1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RT";
                    listMediaType.Add(mediaType);
                }
                if (chkStrategyAndData1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "BP";
                    listMediaType.Add(mediaType);
                }
                if (chkTelevision1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TV";
                    listMediaType.Add(mediaType);
                }
                if (chkTVSponsorship1.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TS";
                    listMediaType.Add(mediaType);
                }
            }
            else
            {
                if (chkCinema.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "CN";
                    listMediaType.Add(mediaType);
                }
                if (chkContentAndActivation.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "ES";
                    listMediaType.Add(mediaType);
                }
                if (chkInternet.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "IT";
                    listMediaType.Add(mediaType);
                }
                if (chkMagazine.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "MG";
                    listMediaType.Add(mediaType);
                }
                if (chkNewspaper.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "NP";
                    listMediaType.Add(mediaType);
                }
                if (chkOutdoor.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "OD";
                    listMediaType.Add(mediaType);
                }
                if (chkRadio.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RD";
                    listMediaType.Add(mediaType);
                }
                if (chkRetainerFee.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "RT";
                    listMediaType.Add(mediaType);
                }
                if (chkStrategyAndData.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "BP";
                    listMediaType.Add(mediaType);
                }
                if (chkTelevision.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TV";
                    listMediaType.Add(mediaType);
                }
                if (chkTVSponsorship.Checked)
                {
                    CreateRequestMediaType mediaType = new CreateRequestMediaType();
                    mediaType.Media_Type = "TS";
                    listMediaType.Add(mediaType);
                }
            }

            // create list permission (agency / office / client)
            if (option == ePermissionOption.AgencyRequest.ToString())
            {
                // set request type
                request.Request_Type = "AG";

                // create list agency
                DataTable dtAgency = (DataTable)gvClientTo.DataSource;
                for (int i = 0; i < dtAgency.Rows.Count; i++)
                {
                    Permission permission = new Permission();
                    CopyPermission agencyPermission = new CopyPermission();
                    List<Permission> listAgencyPermission = new List<Permission>();

                    permission.Permission_Code = dtAgency.Rows[i]["Agency_ID"].ToString();
                    permission.Permission_Name = dtAgency.Rows[i]["Agency_Name"].ToString();
                    permission.Permission_Display_Name = dtAgency.Rows[i]["Agency_Display_Name"].ToString();
                    listAgencyPermission.Add(permission);

                    agencyPermission.Agency_ID = dtAgency.Rows[i]["Agency_ID"].ToString();
                    agencyPermission.Agency_Name = dtAgency.Rows[i]["Agency_Name"].ToString();
                    agencyPermission.Approver_UserID = dtAgency.Rows[i]["Approver_UserID"].ToString();
                    agencyPermission.Approver_Name = dtAgency.Rows[i]["Approver_Name"].ToString();
                    agencyPermission.Approver_Email = dtAgency.Rows[i]["Approver_Email"].ToString();
                    agencyPermission.Delegate_UserID = dtAgency.Rows[i]["Delegate_UserID"].ToString();
                    agencyPermission.Permission_Level = "Agency Level";
                    agencyPermission.Permission_List = listAgencyPermission.ToArray();
                    listPermission.Add(agencyPermission);
                }
            }
            else if (option == ePermissionOption.OfficeRequest.ToString())
            {
                // set request type
                request.Request_Type = "OF";

                // create list office
                DataTable dtOffice = (DataTable)gvClientTo.DataSource;
                for (int i = 0; i < dtOffice.Rows.Count; i++)
                {
                    Permission permission = new Permission();
                    CopyPermission officePermission = new CopyPermission();
                    List<Permission> listOfficePermission = new List<Permission>();

                    permission.Permission_Code = dtOffice.Rows[i]["Office_ID"].ToString();
                    permission.Permission_Name = dtOffice.Rows[i]["Office_Name"].ToString();
                    permission.Permission_Display_Name = dtOffice.Rows[i]["Office_Display_Name"].ToString();
                    listOfficePermission.Add(permission);

                    officePermission.Agency_ID = dtOffice.Rows[i]["Agency_ID"].ToString();
                    officePermission.Agency_Name = dtOffice.Rows[i]["Agency_Name"].ToString();
                    officePermission.Office_ID = dtOffice.Rows[i]["Office_ID"].ToString();
                    officePermission.Office_Name = dtOffice.Rows[i]["Office_Name"].ToString();
                    officePermission.Approver_UserID = dtOffice.Rows[i]["Approver_UserID"].ToString();
                    officePermission.Approver_Name = dtOffice.Rows[i]["Approver_Name"].ToString();
                    officePermission.Approver_Email = dtOffice.Rows[i]["Approver_Email"].ToString();
                    officePermission.Delegate_UserID = dtOffice.Rows[i]["Delegate_UserID"].ToString();
                    officePermission.Permission_Level = "Office Level";
                    officePermission.Permission_List = listOfficePermission.ToArray();
                    listPermission.Add(officePermission);
                }
            }
            else if (option == ePermissionOption.ClientRequest.ToString())
            {
                // set request type
                request.Request_Type = "CL";

                // variable
                DataTable dtClient = (DataTable)gvClientTo.DataSource;
                DataTable dtOffice = new DataTable();
                dtOffice.Columns.Add("Agency_ID");
                dtOffice.Columns.Add("Agency_Name");
                dtOffice.Columns.Add("Office_ID");
                dtOffice.Columns.Add("Office_Name");
                dtOffice.Columns.Add("Approver_UserID");
                dtOffice.Columns.Add("Approver_Name");
                dtOffice.Columns.Add("Approver_Email");
                dtOffice.Columns.Add("Delegate_UserID");

                // group office in client data
                foreach (DataRow drClient in dtClient.Rows)
                {
                    bool bExistsOffice = false;
                    for (int i = 0; i < dtOffice.Rows.Count; i++)
                    {
                        if (drClient["Office_ID"].ToString() == dtOffice.Rows[i]["Office_ID"].ToString())
                        {
                            bExistsOffice = true;
                            break;
                        }
                    }
                    if (!bExistsOffice)
                    {
                        DataRow drOffice = dtOffice.NewRow();
                        drOffice["Agency_ID"] = drClient["Agency_ID"].ToString();
                        drOffice["Agency_Name"] = drClient["Agency_Name"].ToString();
                        drOffice["Office_ID"] = drClient["Office_ID"].ToString();
                        drOffice["Office_Name"] = drClient["Office_Name"].ToString();
                        drOffice["Approver_UserID"] = drClient["Approver_UserID"].ToString();
                        drOffice["Approver_Name"] = drClient["Approver_Name"].ToString();
                        drOffice["Approver_Email"] = drClient["Approver_Email"].ToString();
                        drOffice["Delegate_UserID"] = drClient["Delegate_UserID"].ToString();
                        dtOffice.Rows.Add(drOffice);
                    }
                }
                // create list office group and list client in office group
                for (int i = 0; i < dtOffice.Rows.Count; i++)
                {
                    // variable
                    CopyPermission clientPermission = new CopyPermission();
                    List<Permission> listClientPermission = new List<Permission>();

                    // list client in 1 office
                    DataView dv = new DataView(dtClient);
                    dv.RowFilter = @"Office_ID = '" + dtOffice.Rows[i]["Office_ID"].ToString() + @"'";
                    DataTable dtClientOffice = dv.ToTable();

                    // create list client in 1 office
                    for (int j = 0; j < dtClientOffice.Rows.Count; j++)
                    {
                        Permission permission = new Permission();
                        permission.Permission_Code = dtClientOffice.Rows[j]["Client_ID"].ToString();
                        permission.Permission_Name = dtClientOffice.Rows[j]["Client_Name"].ToString();
                        permission.Permission_Display_Name = dtClientOffice.Rows[j]["Client_Display_Name"].ToString();
                        listClientPermission.Add(permission);
                    }

                    // set data 1 office
                    clientPermission.Agency_ID = dtOffice.Rows[i]["Agency_ID"].ToString();
                    clientPermission.Agency_Name = dtOffice.Rows[i]["Agency_Name"].ToString();
                    clientPermission.Office_ID = dtOffice.Rows[i]["Office_ID"].ToString();
                    clientPermission.Office_Name = dtOffice.Rows[i]["Office_Name"].ToString();
                    clientPermission.Approver_UserID = dtOffice.Rows[i]["Approver_UserID"].ToString();
                    clientPermission.Approver_Name = dtOffice.Rows[i]["Approver_Name"].ToString();
                    clientPermission.Approver_Email = dtOffice.Rows[i]["Approver_Email"].ToString();
                    clientPermission.Delegate_UserID = dtOffice.Rows[i]["Delegate_UserID"].ToString();
                    clientPermission.Permission_Level = "Client Level";
                    clientPermission.Permission_List = listClientPermission.ToArray();
                    listPermission.Add(clientPermission);
                }
            }
            else if (option == ePermissionOption.CopyPermissionRequest.ToString())
            {
                // set request type
                request.Request_Type = "CO";

                // create list copy permission
                listPermission.AddRange(listCopyPermission);
            }

            // set request
            request.Request_Status = "PD";
            request.Request_By = username.ToUpper();
            request.listRequestUser = listUser.ToArray();
            request.listRequestMediaType = listMediaType.ToArray();
            request.listRequestPermission = listPermission.ToArray();

            // convert object to json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(request);
            return json;
        }
        #endregion

        #region request type
        private void btnMinderAccessByAgency_Click(object sender, EventArgs e)
        {
            InitialForOptionRequest();
            option = ePermissionOption.AgencyRequest.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }

        private void btnMinderAccessByOffice_Click(object sender, EventArgs e)
        {
            InitialForOptionRequest();
            option = ePermissionOption.OfficeRequest.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }

        private void btnMinderAccessByClient_Click(object sender, EventArgs e)
        {
            InitialForOptionRequest();
            option = ePermissionOption.ClientRequest.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }

        private void btnMinderAccessByCopy_Click(object sender, EventArgs e)
        {
            InitialForOptionRequest();
            option = ePermissionOption.CopyPermissionRequest.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }
        #endregion

        #region access user info
        private void rdRequestForMe_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRequestForMe.Checked)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    dtRequestOtherUser = m_api.APIGetRequestMe(username);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
                gpRequestOther.Visible = false;
            }
        }

        private void rdRequestForOther_CheckedChanged(object sender, EventArgs e)
        {
            if (rdRequestForOther.Checked)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    gvRequestOtherFrom.AutoGenerateColumns = false;
                    gvRequestOtherTo.AutoGenerateColumns = false;
                    gvRequestOtherFrom.DataSource = m_api.APIGetRequestUser(username);
                    dtRequestOtherUser = ((DataTable)gvRequestOtherFrom.DataSource).Copy();
                    gvRequestOtherTo.DataSource = ((DataTable)gvRequestOtherFrom.DataSource).Clone();
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
                gpRequestOther.Visible = true;
            }
        }

        private void txtRequestOther_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtRequestOtherUser.Select(string.Format("User_ID like '%{0}%' OR User_Full_Name like '%{0}%'", txtRequestOther.Text.Replace("'", "")));
                DataTable dtNew = dtRequestOtherUser.Clone();
                foreach (DataRow dr in rowsToCopy)
                {
                    dtNew.Rows.Add(dr.ItemArray);
                }
                foreach (DataRow dr in ((DataTable)gvRequestOtherTo.DataSource).Rows)
                {
                    DataRow[] drDelete = dtNew.Select(string.Format("User_ID = '{0}'", dr["User_ID"].ToString()));
                    if (drDelete.Count() > 0)
                        dtNew.Rows.Remove(drDelete[0]);
                }
                gvRequestOtherFrom.DataSource = dtNew;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (gvRequestOtherFrom.SelectedRows.Count == 0)
                return;
            int iSelect = gvRequestOtherFrom.SelectedRows.Count;
            for (int i = iSelect - 1; i >= 0; i--)
            {
                DataRow dr = ((DataRowView)gvRequestOtherFrom.SelectedRows[i].DataBoundItem).Row;

                DataTable dt = (DataTable)gvRequestOtherTo.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvRequestOtherFrom.DataSource).Rows.Remove(dr);
            }
        }


        private void btnAddAll_Click(object sender, EventArgs e)
        {
            if (gvRequestOtherFrom.SelectedRows.Count == 0)
                return;
            for (int i = 0; ((DataTable)gvRequestOtherFrom.DataSource).Rows.Count > 0; i++)
            {
                DataRow dr = ((DataTable)gvRequestOtherFrom.DataSource).Rows[0];

                DataTable dt = (DataTable)gvRequestOtherTo.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvRequestOtherFrom.DataSource).Rows.Remove(dr);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (gvRequestOtherTo.SelectedRows.Count == 0)
                return;
            int iSelect = gvRequestOtherTo.SelectedRows.Count;
            for (int i = iSelect - 1; i >= 0; i--)
            {
                DataRow dr = ((DataRowView)gvRequestOtherTo.SelectedRows[i].DataBoundItem).Row;

                DataTable dt = (DataTable)gvRequestOtherFrom.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvRequestOtherTo.DataSource).Rows.Remove(dr);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (gvRequestOtherTo.SelectedRows.Count == 0)
                return;
            for (int i = 0; ((DataTable)gvRequestOtherTo.DataSource).Rows.Count > 0; i++)
            {
                DataRow dr = ((DataTable)gvRequestOtherTo.DataSource).Rows[0];

                DataTable dt = (DataTable)gvRequestOtherFrom.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvRequestOtherTo.DataSource).Rows.Remove(dr);
            }
        }

        private void gvRequestOtherFrom_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAdd.PerformClick();
        }

        private void gvRequestOtherTo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRemove.PerformClick();
        }
        #endregion

        #region request detail (agency, office, client)
        private void chkSelectAllMediaType_CheckedChanged(object sender, EventArgs e)
        {
            selectChk = true;
            if (chkSelectAllMediaType.CheckState == CheckState.Checked)
            {
                chkCinema.Checked = true;
                chkContentAndActivation.Checked = true;
                chkInternet.Checked = true;
                chkMagazine.Checked = true;
                chkNewspaper.Checked = true;
                chkOutdoor.Checked = true;
                chkRadio.Checked = true;
                chkRetainerFee.Checked = true;
                chkStrategyAndData.Checked = true;
                chkTelevision.Checked = true;
                chkTVSponsorship.Checked = true;
            }
            else if (chkSelectAllMediaType.CheckState == CheckState.Unchecked)
            {
                chkCinema.Checked = false;
                chkContentAndActivation.Checked = false;
                chkInternet.Checked = false;
                chkMagazine.Checked = false;
                chkNewspaper.Checked = false;
                chkOutdoor.Checked = false;
                chkRadio.Checked = false;
                chkRetainerFee.Checked = false;
                chkStrategyAndData.Checked = false;
                chkTelevision.Checked = false;
                chkTVSponsorship.Checked = false;
            }
            selectChk = false;
        }

        private void chkCinema_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkContentAndActivation_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkInternet_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkMagazine_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkNewspaper_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkOutdoor_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkRadio_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkRetainerFee_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkStrategyAndData_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkTelevision_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void chkTVSponsorship_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType();
        }

        private void txtClient_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtClient.Select(string.Format("Client_Display_Name like '%{0}%' OR Client_ID like '%{0}%'", txtClient.Text.Replace("'", "")));
                DataTable dtNew = dtClient.Clone();
                foreach (DataRow dr in rowsToCopy)
                {
                    dtNew.Rows.Add(dr.ItemArray);
                }
                foreach (DataRow dr in ((DataTable)gvClientTo.DataSource).Rows)
                {
                    DataRow[] drDelete = dtNew.Select(string.Format("Client_ID = '{0}'", dr["Client_ID"].ToString()));
                    if (drDelete.Count() > 0)
                        dtNew.Rows.Remove(drDelete[0]);
                }
                gvClientFrom.DataSource = dtNew;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }

        private void btnClientAdd_Click(object sender, EventArgs e)
        {
            if (gvClientFrom.SelectedRows.Count == 0)
                return;
            int iSelect = gvClientFrom.SelectedRows.Count;
            for (int i = iSelect - 1; i >= 0; i--)
            {
                DataRow dr = ((DataRowView)gvClientFrom.SelectedRows[i].DataBoundItem).Row;

                DataTable dt = (DataTable)gvClientTo.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvClientFrom.DataSource).Rows.Remove(dr);
            }
        }

        private void btnClientAddAll_Click(object sender, EventArgs e)
        {
            if (gvClientFrom.SelectedRows.Count == 0)
                return;
            for (int i = 0; ((DataTable)gvClientFrom.DataSource).Rows.Count > 0; i++)
            {
                DataRow dr = ((DataTable)gvClientFrom.DataSource).Rows[0];

                DataTable dt = (DataTable)gvClientTo.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvClientFrom.DataSource).Rows.Remove(dr);
            }
        }

        private void btnClientRemove_Click(object sender, EventArgs e)
        {
            if (gvClientTo.SelectedRows.Count == 0)
                return;
            int iSelect = gvClientTo.SelectedRows.Count;
            for (int i = iSelect - 1; i >= 0; i--)
            {
                DataRow dr = ((DataRowView)gvClientTo.SelectedRows[i].DataBoundItem).Row;

                DataTable dt = (DataTable)gvClientFrom.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvClientTo.DataSource).Rows.Remove(dr);
            }
        }

        private void btnClientRemoveAll_Click(object sender, EventArgs e)
        {
            if (gvClientTo.SelectedRows.Count == 0)
                return;
            for (int i = 0; ((DataTable)gvClientTo.DataSource).Rows.Count > 0; i++)
            {
                DataRow dr = ((DataTable)gvClientTo.DataSource).Rows[0];

                DataTable dt = (DataTable)gvClientFrom.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvClientTo.DataSource).Rows.Remove(dr);
            }
        }

        private void gvClientFrom_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnClientAdd.PerformClick();
        }

        private void gvClientTo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnClientRemove.PerformClick();
        }
        #endregion

        #region request detail (copy permission)
        private void chkSelectAllMediaType1_CheckedChanged(object sender, EventArgs e)
        {
            selectChk = true;
            if (chkSelectAllMediaType1.CheckState == CheckState.Checked)
            {
                chkCinema1.Checked = true;
                chkContentAndActivation1.Checked = true;
                chkInternet1.Checked = true;
                chkMagazine1.Checked = true;
                chkNewspaper1.Checked = true;
                chkOutdoor1.Checked = true;
                chkRadio1.Checked = true;
                chkRetainerFee1.Checked = true;
                chkStrategyAndData1.Checked = true;
                chkTelevision1.Checked = true;
                chkTVSponsorship1.Checked = true;
            }
            else if (chkSelectAllMediaType1.CheckState == CheckState.Unchecked)
            {
                chkCinema1.Checked = false;
                chkContentAndActivation1.Checked = false;
                chkInternet1.Checked = false;
                chkMagazine1.Checked = false;
                chkNewspaper1.Checked = false;
                chkOutdoor1.Checked = false;
                chkRadio1.Checked = false;
                chkRetainerFee1.Checked = false;
                chkStrategyAndData1.Checked = false;
                chkTelevision1.Checked = false;
                chkTVSponsorship1.Checked = false;
            }
            selectChk = false;
        }

        private void chkCinema1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkContentAndActivation1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkInternet1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkMagazine1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkNewspaper1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkOutdoor1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkRadio1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkRetainerFee1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkStrategyAndData1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkTelevision1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }

        private void chkTVSponsorship1_CheckedChanged(object sender, EventArgs e)
        {
            CheckSelectMediaType1();
        }
        #endregion

        #region center control
        private void tabControlRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[0])
            {
                lbRequstType.ForeColor = Color.CornflowerBlue;
                lbAccessUserInfor.ForeColor = Color.Gray;
                lbRequestDetail.ForeColor = Color.Gray;
                lbSummaryRequst.ForeColor = Color.Gray;
            }
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[1])
            {
                lbRequstType.ForeColor = Color.Gray;
                lbAccessUserInfor.ForeColor = Color.CornflowerBlue;
                lbRequestDetail.ForeColor = Color.Gray;
                lbSummaryRequst.ForeColor = Color.Gray;
                if (option == ePermissionOption.CopyPermissionRequest.ToString())
                {
                    rdRequestForMe.Enabled = false;
                    rdRequestForOther.Checked = true;
                }
                else
                {
                    rdRequestForMe.Enabled = true;
                }
            }
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[2])
            {
                lbRequstType.ForeColor = Color.Gray;
                lbAccessUserInfor.ForeColor = Color.Gray;
                lbRequestDetail.ForeColor = Color.CornflowerBlue;
                lbSummaryRequst.ForeColor = Color.Gray;

                if (dtMediaType == null)
                    dtMediaType = m_api.APIGetMediaTypePermissionByUser(username);

                for (int i = 0; i < dtMediaType.Rows.Count; i++)
                {
                    if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "CN")
                        chkCinema.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "ES")
                        chkContentAndActivation.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "IT")
                        chkInternet.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "MG")
                        chkMagazine.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "NP")
                        chkNewspaper.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "OD")
                        chkOutdoor.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "RD")
                        chkRadio.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "RT")
                        chkRetainerFee.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "BP")
                        chkStrategyAndData.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "TV")
                        chkTelevision.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "TS")
                        chkTVSponsorship.Checked = true;
                }

                if (option == ePermissionOption.AgencyRequest.ToString())
                {
                    lblClientList.Text = "Access Agency List";
                    if (dtClient == null)
                    {
                        try
                        {
                            Cursor = Cursors.WaitCursor;
                            gvClientFrom.AutoGenerateColumns = false;
                            gvClientTo.AutoGenerateColumns = false;
                            ColDisplayName1.DataPropertyName = "Agency_Display_Name";
                            ColDisplayName2.DataPropertyName = "Agency_Display_Name";
                            gvClientFrom.DataSource = m_api.APIGetAllAgency();
                            dtClient = ((DataTable)gvClientFrom.DataSource).Copy();
                            gvClientTo.DataSource = ((DataTable)gvClientFrom.DataSource).Clone();
                        }
                        finally
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
                else if (option == ePermissionOption.OfficeRequest.ToString())
                {
                    lblClientList.Text = "Access Office List";
                    if (dtClient == null)
                    {
                        try
                        {
                            Cursor = Cursors.WaitCursor;
                            gvClientFrom.AutoGenerateColumns = false;
                            gvClientTo.AutoGenerateColumns = false;
                            ColDisplayName1.DataPropertyName = "Office_Display_Name";
                            ColDisplayName2.DataPropertyName = "Office_Display_Name";
                            gvClientFrom.DataSource = m_api.APIGetAllOffice();
                            dtClient = ((DataTable)gvClientFrom.DataSource).Copy();
                            gvClientTo.DataSource = ((DataTable)gvClientFrom.DataSource).Clone();
                        }
                        finally
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
                else if (option == ePermissionOption.ClientRequest.ToString())
                {
                    lblClientList.Text = "Access Client List";
                    if (dtClient == null)
                    {
                        try
                        {
                            Cursor = Cursors.WaitCursor;
                            gvClientFrom.AutoGenerateColumns = false;
                            gvClientTo.AutoGenerateColumns = false;
                            ColDisplayName1.DataPropertyName = "Client_Display_Name";
                            ColDisplayName2.DataPropertyName = "Client_Display_Name";
                            gvClientFrom.DataSource = m_api.APIGetAllClient();
                            dtClient = ((DataTable)gvClientFrom.DataSource).Copy();
                            gvClientTo.DataSource = ((DataTable)gvClientFrom.DataSource).Clone();
                        }
                        finally
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
            }
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[3])
            {
                lbRequstType.ForeColor = Color.Gray;
                lbAccessUserInfor.ForeColor = Color.Gray;
                lbRequestDetail.ForeColor = Color.CornflowerBlue;
                lbSummaryRequst.ForeColor = Color.Gray;

                dtMediaType = m_api.APIGetMediaTypePermissionByUser(username);

                for (int i = 0; i < dtMediaType.Rows.Count; i++)
                {
                    if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "CN")
                        chkCinema1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "ES")
                        chkContentAndActivation1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "IT")
                        chkInternet1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "MG")
                        chkMagazine1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "NP")
                        chkNewspaper1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "OD")
                        chkOutdoor1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "RD")
                        chkRadio1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "RT")
                        chkRetainerFee1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "BP")
                        chkStrategyAndData1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "TV")
                        chkTelevision1.Checked = true;
                    else if (dtMediaType.Rows[i]["Media_Type_Code"].ToString() == "TS")
                        chkTVSponsorship1.Checked = true;
                }

                try
                {
                    Cursor = Cursors.WaitCursor;
                    listCopyPermission = new List<CopyPermission>();
                    List<CopyPermission> listPermission = m_api.APIGetClientPermissionByUser(username);
                    listCopyPermission.AddRange(listPermission);

                    DataTable dtApproval = new DataTable();
                    dtApproval.Columns.Add("ApprovalName");
                    dtApproval.Columns.Add("ClientList");

                    foreach (CopyPermission permission in listPermission)
                    {
                        string groupName = permission.Permission_Level == "Agency Level" ? permission.Agency_Name : permission.Office_Name;
                        DataRow drApprovalName = dtApproval.NewRow();
                        drApprovalName["ApprovalName"] = "Permission : " + permission.Permission_Level + Environment.NewLine + permission.Approver_Name.ToUpper() + Environment.NewLine + groupName;
                        List<Permission> listSubPermission = permission.Permission_List.ToList();
                        foreach (Permission subPermission in listSubPermission)
                        {
                            if (permission.Permission_Level == "Client Level")
                                drApprovalName["ClientList"] = drApprovalName["ClientList"].ToString() + Environment.NewLine + subPermission.Permission_Display_Name;
                            else
                                drApprovalName["ClientList"] = subPermission.Permission_Display_Name;
                        }
                        dtApproval.Rows.Add(drApprovalName);
                    }

                    gvClientCopyPermission.AutoGenerateColumns = false;
                    gvClientCopyPermission.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    gvClientCopyPermission.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    gvClientCopyPermission.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                    gvClientCopyPermission.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    gvClientCopyPermission.DataSource = dtApproval;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[4])
            {
                lbRequstType.ForeColor = Color.Gray;
                lbAccessUserInfor.ForeColor = Color.Gray;
                lbRequestDetail.ForeColor = Color.Gray;
                lbSummaryRequst.ForeColor = Color.CornflowerBlue;

                //===========================
                // Refresh User
                //===========================
                DataTable dtRequestList = new DataTable();
                dtRequestList.Columns.Add("RequestUser");

                if (rdRequestForMe.Checked)
                {
                    DataRow drReqester = dtRequestList.NewRow();
                    drReqester["RequestUser"] = dtRequestOtherUser.Rows[0]["User_Full_Name"];
                    dtRequestList.Rows.Add(drReqester);
                }
                else
                {
                    foreach (DataRow drReqesterList in ((DataTable)gvRequestOtherTo.DataSource).Rows)
                    {
                        DataRow drReqester = dtRequestList.NewRow();
                        drReqester["RequestUser"] = drReqesterList["User_Full_Name"];
                        dtRequestList.Rows.Add(drReqester);
                    }
                }

                gvSummaryRequestUser.AutoGenerateColumns = false;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.DataSource = dtRequestList;

                //===========================
                // Refresh Media Type
                //===========================
                DataTable dtMediaType = new DataTable();
                dtMediaType.Columns.Add("RequestMediaType");
                if (option == ePermissionOption.CopyPermissionRequest.ToString())
                {
                    if (chkCinema1.Checked)
                        dtMediaType.Rows.Add("Cinema");
                    if (chkContentAndActivation1.Checked)
                        dtMediaType.Rows.Add("Content And Activation");
                    if (chkInternet1.Checked)
                        dtMediaType.Rows.Add("Internet");
                    if (chkMagazine1.Checked)
                        dtMediaType.Rows.Add("Magazine");
                    if (chkNewspaper1.Checked)
                        dtMediaType.Rows.Add("Newspaper");
                    if (chkOutdoor1.Checked)
                        dtMediaType.Rows.Add("Outdoor");
                    if (chkRadio1.Checked)
                        dtMediaType.Rows.Add("Radio");
                    if (chkRetainerFee1.Checked)
                        dtMediaType.Rows.Add("Retainer Fee");
                    if (chkStrategyAndData1.Checked)
                        dtMediaType.Rows.Add("Strategy And Data");
                    if (chkTelevision1.Checked)
                        dtMediaType.Rows.Add("Television");
                    if (chkTVSponsorship1.Checked)
                        dtMediaType.Rows.Add("TV Sponsorship");
                }
                else
                {
                    if (chkCinema.Checked)
                        dtMediaType.Rows.Add("Cinema");
                    if (chkContentAndActivation.Checked)
                        dtMediaType.Rows.Add("Content And Activation");
                    if (chkInternet.Checked)
                        dtMediaType.Rows.Add("Internet");
                    if (chkMagazine.Checked)
                        dtMediaType.Rows.Add("Magazine");
                    if (chkNewspaper.Checked)
                        dtMediaType.Rows.Add("Newspaper");
                    if (chkOutdoor.Checked)
                        dtMediaType.Rows.Add("Outdoor");
                    if (chkRadio.Checked)
                        dtMediaType.Rows.Add("Radio");
                    if (chkRetainerFee.Checked)
                        dtMediaType.Rows.Add("Retainer Fee");
                    if (chkStrategyAndData.Checked)
                        dtMediaType.Rows.Add("Strategy And Data");
                    if (chkTelevision.Checked)
                        dtMediaType.Rows.Add("Television");
                    if (chkTVSponsorship.Checked)
                        dtMediaType.Rows.Add("TV Sponsorship");
                }

                gvSummaryMediaType.AutoGenerateColumns = false;
                gvSummaryMediaType.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.DataSource = dtMediaType;

                //===========================
                // Refresh Client
                //===========================
                if (option == ePermissionOption.AgencyRequest.ToString())
                    lblPermissionGroupList.Text = "Access Agency List";
                else if (option == ePermissionOption.OfficeRequest.ToString())
                    lblPermissionGroupList.Text = "Access Office List";
                else if (option == ePermissionOption.ClientRequest.ToString())
                    lblPermissionGroupList.Text = "Access Client List";
                else if (option == ePermissionOption.CopyPermissionRequest.ToString())
                    lblPermissionGroupList.Text = "Access Permission List";

                DataTable dtApproval = new DataTable();
                dtApproval.Columns.Add("ApprovalName");
                dtApproval.Columns.Add("ClientList");

                if (option == ePermissionOption.CopyPermissionRequest.ToString())
                {
                    dtApproval = (DataTable)gvClientCopyPermission.DataSource;
                }
                else
                {
                    foreach (DataRow drClientList in ((DataTable)gvClientTo.DataSource).Rows)
                    {
                        bool bExistsApproval = false;
                        if (option == ePermissionOption.AgencyRequest.ToString())
                        {
                            for (int i = 0; i < dtApproval.Rows.Count; i++)
                            {
                                if (dtApproval.Rows[i]["ApprovalName"].ToString() == drClientList["Approver_Name"].ToString().ToUpper() + Environment.NewLine + drClientList["Agency_Name"].ToString())
                                {
                                    DataRow drApprovalName = dtApproval.Rows[i];
                                    drApprovalName["ClientList"] = drApprovalName["ClientList"].ToString() + Environment.NewLine + drClientList["Agency_Display_Name"];
                                    bExistsApproval = true;
                                    break;
                                }
                            }
                            if (!bExistsApproval)
                            {
                                DataRow drApprovalName = dtApproval.NewRow();
                                drApprovalName["ApprovalName"] = drClientList["Approver_Name"].ToString().ToUpper() + Environment.NewLine + drClientList["Agency_Name"].ToString();
                                drApprovalName["ClientList"] = drClientList["Agency_Display_Name"].ToString();
                                dtApproval.Rows.Add(drApprovalName);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtApproval.Rows.Count; i++)
                            {
                                if (dtApproval.Rows[i]["ApprovalName"].ToString() == drClientList["Approver_Name"].ToString().ToUpper() + Environment.NewLine + drClientList["Office_Name"].ToString())
                                {
                                    DataRow drApprovalName = dtApproval.Rows[i];
                                    if (option == ePermissionOption.OfficeRequest.ToString())
                                        drApprovalName["ClientList"] = drApprovalName["ClientList"].ToString() + Environment.NewLine + drClientList["Office_Display_Name"];
                                    else if (option == ePermissionOption.ClientRequest.ToString())
                                        drApprovalName["ClientList"] = drApprovalName["ClientList"].ToString() + Environment.NewLine + drClientList["Client_Display_Name"];
                                    bExistsApproval = true;
                                    break;
                                }
                            }
                            if (!bExistsApproval)
                            {
                                DataRow drApprovalName = dtApproval.NewRow();
                                drApprovalName["ApprovalName"] = drClientList["Approver_Name"].ToString().ToUpper() + Environment.NewLine + drClientList["Office_Name"].ToString();
                                if (option == ePermissionOption.OfficeRequest.ToString())
                                    drApprovalName["ClientList"] = drClientList["Office_Display_Name"].ToString();
                                else if (option == ePermissionOption.ClientRequest.ToString())
                                    drApprovalName["ClientList"] = drClientList["Client_Display_Name"].ToString();
                                dtApproval.Rows.Add(drApprovalName);
                            }
                        }
                    }
                }

                gvSummaryApprovalRequest.AutoGenerateColumns = false;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gvSummaryApprovalRequest.DataSource = dtApproval;
            }
        }

        private void btnAccessUserInfoNext_Click(object sender, EventArgs e)
        {
            bool validUser = CheckListUser();
            if (validUser)
            {
                if (option == ePermissionOption.CopyPermissionRequest.ToString())
                    tabControlRequest.SelectedTab = tabControlRequest.TabPages[3];
                else
                    tabControlRequest.SelectedTab = tabControlRequest.TabPages[2];
            }
            else
            {
                GMessage.MessageWarning("Please select User.");
            }
        }

        private void btnAccessUserInfoBack_Click(object sender, EventArgs e)
        {
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
        }

        private void btnRequestDetailNext_Click(object sender, EventArgs e)
        {
            bool validMediaType = CheckListMediaType();
            if (validMediaType)
            {
                bool validSelectMediaType = CheckSelectedMediaType();
                if (validSelectMediaType)
                {
                    bool validPermission = CheckListPermission();
                    if (validPermission)
                    {
                        tabControlRequest.SelectedTab = tabControlRequest.TabPages[4];
                    }
                    else
                    {
                        if (option == ePermissionOption.AgencyRequest.ToString())
                            GMessage.MessageWarning("Please select Agency.");
                        else if (option == ePermissionOption.OfficeRequest.ToString())
                            GMessage.MessageWarning("Please select Office.");
                        else if (option == ePermissionOption.ClientRequest.ToString())
                            GMessage.MessageWarning("Please select Client.");
                    }
                }
                else
                {
                    GMessage.MessageWarning("Please contact system admin. รอพี่ชายคิด");
                }
            }
            else
            {
                GMessage.MessageWarning("Please select Media Type.");
            }
        }

        private void btnRequestDetailBack_Click(object sender, EventArgs e)
        {
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }

        private void btnRequestDetailNext1_Click(object sender, EventArgs e)
        {
            bool validMediaType = CheckListMediaType();
            if (validMediaType)
            {
                bool validSelectMediaType = CheckSelectedMediaType();
                if (validSelectMediaType)
                    tabControlRequest.SelectedTab = tabControlRequest.TabPages[4];
                else
                    GMessage.MessageWarning("Please contact system admin. รอพี่ชายคิด");
            }
            else
            {
                GMessage.MessageWarning("Please select Media Type.");
            }
        }

        private void btnRequestDetailBack1_Click(object sender, EventArgs e)
        {
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }

        private void btnSummaryBack_Click(object sender, EventArgs e)
        {
            if (option == ePermissionOption.CopyPermissionRequest.ToString())
                tabControlRequest.SelectedTab = tabControlRequest.TabPages[3];
            else
                tabControlRequest.SelectedTab = tabControlRequest.TabPages[2];
        }

        private void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string json = CreateJsonRequest();
            string requestNo = m_api.APICreateNewRequest(json);
            if (requestNo == "Call API Unsuccessful.")
                GMessage.MessageError("Some thing is wrong, Your request is failed to create.");
            else
                GMessage.MessageInfo("You Request No. is " + requestNo);
            Cursor = Cursors.Default;
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
        }
        #endregion

    }
}
