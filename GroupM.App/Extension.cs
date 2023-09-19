using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App
{
    public static class Extension
    {
        #region ### Common Control ###

        #endregion

        #region ### DataGridView ###
        public static void SetDefaultTemplate(this DataGridView gv)
        {
            gv.BorderStyle = BorderStyle.None;
            gv.EnableHeadersVisualStyles = false;

            // color
            gv.BackgroundColor = SystemColors.GradientInactiveCaption;
            gv.GridColor = SystemColors.ActiveCaption;

            gv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.GradientActiveCaption;
            gv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gv.ColumnHeadersHeight = 25;
            gv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            gv.RowHeadersDefaultCellStyle.BackColor = SystemColors.GradientActiveCaption;

            // Event
            gv.RowPostPaint += new DataGridViewRowPostPaintEventHandler(gv_RowPostPaint);
        }

        // ## Paint row number on DataGridView ##
        private static void gv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Near
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top + 3, grid.RowHeadersWidth - 10, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, gv.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        #endregion
    }
}
