using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Design;
using System.Windows.Forms.Design;
using System.Drawing;

namespace GroupM.CustomControl.Common
{
    [Designer(typeof(ControlDesigner))]
    public partial class CustomDataGridView : DataGridView
    {
        public CustomDataGridView()
        {
            InitializeComponent();
            this.TopLeftHeaderCell.Value = "ID";            
        }

        public CustomDataGridView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.TopLeftHeaderCell.Value = "ID";
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Near
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top + 3, this.RowHeadersWidth - 10, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
            base.OnRowPostPaint(e);
        }

    }
}
