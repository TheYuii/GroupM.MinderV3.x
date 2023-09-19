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
    public partial class MPA013_LoadTemplate : Form
    {
        public MPA013_LoadTemplate()
        {
            InitializeComponent();
        }

        private void gvDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (gvDetail.Rows.Count > 0)
                {
                    contextMenuStrip.Show((Control)sender, new Point(e.X,e.Y));
                }
            }
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ContextMenuStrip obj = (ContextMenuStrip)sender;
                DataGridView gv = (DataGridView)obj.SourceControl;
                if (gv.SelectedCells.Count > 0)
                {
                    if (gv.Rows[gv.SelectedCells[0].RowIndex].IsNewRow == false)
                    {
                        if (GMessage.MessageComfirm("Are you sure to DELETE this template?") == System.Windows.Forms.DialogResult.Yes)
                        {
                            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMPA);
                            string strSelect = string.Format(@"DELETE FROM Template WHERE TemplateName = @TemplateName and CreateBy = @CreateBy");
                            SqlCommand comm = new SqlCommand(strSelect, conn);
                            comm.Parameters.Add("@TemplateName", SqlDbType.VarChar).Value = gv.SelectedCells[1].Value.ToString();
                            comm.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = Connection.USERID;
                            conn.Open();
                            comm.ExecuteNonQuery();
                            conn.Close();
                            gv.Rows.RemoveAt(gv.SelectedCells[0].RowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }

        private void MPA013_LoadTemplate_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Connection.ConnectionStringMPA);
                string strSelect = string.Format(@"SELECT * FROM Template WHERE TemplateScreenName = 'MediaSpending' AND CreateBy = @CreateBy");
                SqlDataAdapter sda = new SqlDataAdapter(strSelect, conn);
                sda.SelectCommand.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = Connection.USERID;
                DataSet ds = new DataSet();
                sda.Fill(ds);

                gvDetail.AutoGenerateColumns = false;
                gvDetail.DataSource = ds.Tables[0];


                DataGridViewImageColumn img = (DataGridViewImageColumn)gvDetail.Columns[0];
                Image image = global::GroupM.Minder.Properties.Resources.delete1;
                img.Image = image;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            
        }

        private void gvDetail_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (gvDetail.Rows.Count > 0 && e.ColumnIndex == 1)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvDetail.Rows.Count > 0 && e.ColumnIndex == 0)
            {
                if (GMessage.MessageComfirm("Are you sure to DELETE this template?") == System.Windows.Forms.DialogResult.Yes)
                {
                    SqlConnection conn = new SqlConnection(Connection.ConnectionStringMPA);
                    string strSelect = string.Format(@"DELETE FROM Template WHERE TemplateName = @TemplateName and CreateBy = @CreateBy");
                    SqlCommand comm = new SqlCommand(strSelect, conn);
                    comm.Parameters.Add("@TemplateName", SqlDbType.VarChar).Value = gvDetail.Rows[e.RowIndex].Cells[1].Value.ToString();
                    comm.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = Connection.USERID;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                    gvDetail.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
    }
}
