using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using GroupM.UTL;

namespace  GroupM.Minder
{
    public partial class MPA012_SaveTemplate : Form
    {
        string m_strCreateBy = "";
        string m_strTempalteScreenName = "";
        public string TemplateName { get; set; }
        public MPA012_SaveTemplate(string strCreateBy,string strTemplateScreenName)
        {
            InitializeComponent();
            m_strCreateBy = strCreateBy;
            m_strTempalteScreenName = strTemplateScreenName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if(txtTemplateName.Text.Trim() == "")
            {
                GMessage.MessageWarning("Template name can't be blank.");
            }
            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMPA);
            string strSelect = string.Format(@"select count(1) ExistsCount from template where createby = '{0}' and templatename = '{1}' and templatescreenname = '{2}'", m_strCreateBy, txtTemplateName.Text, m_strTempalteScreenName);
            SqlCommand comm = new SqlCommand(strSelect ,conn);
            conn.Open();
            int iExists = Convert.ToInt32(comm.ExecuteScalar());
            TemplateName = txtTemplateName.Text;
            if(iExists == 0)
            {                
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                if(GMessage.MessageComfirm("The template name alreday EXISTS , Do you want to replace?") == System.Windows.Forms.DialogResult.Yes)
                    DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            conn.Close();
        }

        private void txtTemplateName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveAs.PerformClick();
            }
        }
    }
}
