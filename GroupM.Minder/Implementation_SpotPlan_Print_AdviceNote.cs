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
    public partial class Implementation_SpotPlan_Print_AdviceNote : Form
    {
        public DataTable dtSpontEdit = null;
        public string username;

        public Implementation_SpotPlan_Print_AdviceNote()
        {
            InitializeComponent();
        }

        private void Implementation_SpotPlan_Print_AdviceNote_Load(object sender, EventArgs e)
        {
            InitControl();
        }
        private void RefreshGvSpotPlanEdit()
        {
            gvSpotPlanEdit.AutoGenerateColumns = false;
            DataTable dt = dtSpontEdit.Clone();
            if (!chkShowX.Checked)
            {
                DataRow[] dr = dtSpontEdit.Select("Team <> 'x'");
                for (int i = 0; i < dr.Length; i++)
                {
                    DataRow drRow = dt.NewRow();
                    drRow.ItemArray = dr[i].ItemArray;
                    dt.Rows.Add(drRow);
                }
            }
            else
            {
                dt = dtSpontEdit.Copy();
            }
            gvSpotPlanEdit.DataSource = dt;
        }

        private void InitControl()
        {

            RefreshGvSpotPlanEdit();
            chkCheckAllSpotPlanEdit.Checked = true;

            var l = dtSpontEdit.AsEnumerable()
                               .GroupBy(row => new
                               {
                                   Print_DateTime = row.Field<DateTime?>("Print_DateTime")
                               }).Select(g => new { g.Key.Print_DateTime }).ToList();

            gvPrint.AutoGenerateColumns = false;
            gvPrint.DataSource = l;


            foreach (DataGridViewRow item in gvSpotPlanEdit.Rows)
            {
                item.Cells[0].Value = true;
            }
            foreach (DataGridViewRow item in gvPrint.Rows)
            {
                item.Cells[0].Value = true;
            }


        }

        private void chkCheckAllSpotPlanEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckAllSpotPlanEdit.Checked)
            {
                foreach (DataGridViewRow item in gvSpotPlanEdit.Rows)
                {
                    item.Cells[0].Value = true;
                }
            }
            else {
                foreach (DataGridViewRow item in gvSpotPlanEdit.Rows)
                {
                    item.Cells[0].Value = false;
                }

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;

        }

        private void gvPrint_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                bool chkBoxValue = (bool)gvPrint.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                DateTime? printValue = (DateTime?)gvPrint.Rows[e.RowIndex].Cells[1].Value;

                for (int i = 0; i < gvSpotPlanEdit.Rows.Count; i++)
                {
                    DataRow dr = ((DataRowView)gvSpotPlanEdit.Rows[i].DataBoundItem).Row;
                    if (printValue == null)
                    {
                        if (dr["Print_DateTime"] == DBNull.Value)
                        {
                            gvSpotPlanEdit.Rows[i].Cells[e.ColumnIndex].Value = chkBoxValue;
                        }
                    }
                    else
                    {
                        if (dr["Print_DateTime"] == DBNull.Value)
                            continue;
                        if (Convert.ToDateTime(dr["Print_DateTime"]) == Convert.ToDateTime(printValue))
                        {
                            gvSpotPlanEdit.Rows[i].Cells[e.ColumnIndex].Value = chkBoxValue;
                        }
                    }
                   
                }
            }
        }

        private void gvPrint_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gvPrint.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void chkShowX_CheckedChanged(object sender, EventArgs e)
        {
            RefreshGvSpotPlanEdit();
            foreach (DataGridViewRow item in gvSpotPlanEdit.Rows)
            {
                item.Cells[0].Value = true;
            }
            foreach (DataGridViewRow item in gvPrint.Rows)
            {
                item.Cells[0].Value = true;
            }
        }

        private void btnPrint_MouseHover(object sender, EventArgs e)
        {
            lbMessageSave.ForeColor = Color.Red;
        }

        private void btnPrint_MouseLeave(object sender, EventArgs e)
        {
            lbMessageSave.ForeColor = Color.Black;
        }
    }
}
