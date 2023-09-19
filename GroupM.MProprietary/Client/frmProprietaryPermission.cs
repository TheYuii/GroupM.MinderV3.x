using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MProprietary
{
    public partial class frmProprietaryPermission : Form
    {
        public frmProprietaryPermission(DataRow dr)
        {
            InitializeComponent();
            drGroupProp = dr;
            groupID = (int)dr["GroupProprietaryId"];
        }

        private DBAccess connect = new DBAccess();
        private DataRow drGroupProp;
        private int groupID = 0;
        private DataTable dt = new DataTable();

        private void frmProprietaryPermission_Load(object sender, EventArgs e)
        {
            txtGroupName.Text = drGroupProp["GroupProprietaryName"].ToString();
            dt = connect.SelectProprietaryMapping(groupID);
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;
        }
    }
}
