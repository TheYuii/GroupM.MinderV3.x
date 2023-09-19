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
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Master_MediaSubType_Report : Form
    {
        DBManager m_db = null;

        public Master_MediaSubType_Report()
        {
            InitializeComponent();
            m_db = new DBManager();
        }

        private void Master_MediaSubType_Report_Load(object sender, EventArgs e)
        {
            string sql = "select * from Media_Type";
            DataTable dt = m_db.SelectNonParameter(sql);
            cboMediaTypeCode.Items.Add("All");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboMediaTypeCode.Items.Add(dt.Rows[i]["Media_Type"].ToString());
            }
            cboMediaTypeCode.SelectedIndex = 0;
        }

        private void cboMediaTypeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMediaTypeCode.SelectedIndex > 0)
            {
                try
                {
                    DataTable dt = m_db.SelectMediaType(cboMediaTypeCode.Text);
                    txtMediaType.Text = dt.Rows[0]["Short_Name"].ToString();
                }
                catch (Exception ex)
                {
                    GMessage.MessageError(ex.Message);
                }
            }
            else
            {
                txtMediaType.Text = "";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectMediaSubTypeBusinessDefinition(cboMediaTypeCode.Text);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.Cursor = Cursors.WaitCursor;
            ExcelUtil.ExportMSTBusinessDefinition(dt, 1);
            this.Cursor = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
