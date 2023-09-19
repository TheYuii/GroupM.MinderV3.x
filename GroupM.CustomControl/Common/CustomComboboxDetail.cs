using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.CustomControl.Common
{
    public partial class CustomComboboxDetail : ComboBox
    {

        #region ### Constant ###
        public CustomComboboxDetail()
        {
            InitializeComponent();
            form = new CustomComboboxDetailForm();
        }

        public CustomComboboxDetail(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            form = new CustomComboboxDetailForm();
        }
        #endregion

        #region ### Properties ###
        CustomComboboxDetailForm form;
        object listData = new object();

        public string ss { get; set; }

        #endregion

        #region ### Method ###
        public void BindingData(DataTable data, string display, string value, bool selectAll)
        {
            form.OnBinding(data, display, value, selectAll);
            if (!string.IsNullOrEmpty(form.SelectedNode))
                this.Text = form.SelectedNode;
            else
                this.Text = "-- Please select data. --";
        }
        #endregion

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var controlPoint = this.PointToScreen(Point.Empty);

            //form.Width = this.DropDownWidth;

            form.ccbWidth = this.DropDownWidth;

            form.Location = new Point(controlPoint.X, controlPoint.Y + this.Height);
            form.NodeChecked += form_NodeChecked;
            form.Deactivate += form_Deactivate;

            form.Show();
            //form.ShowDialog();            
        }

        public delegate void DeactivatedHandler();
        public event DeactivatedHandler e_Deactivated;

        public void form_Deactivate(object sender, EventArgs e)
        {
            e_Deactivated();
            form.Hide();
        }

        public void form_NodeChecked(object sender, TreeViewEventArgs e)
        {
            this.Text = form.SelectedNode;
            if (string.IsNullOrEmpty(this.Text.Trim()))
            {
                this.Text = "-- Please select data. --";
            }
            this.OnSelectedValueChanged(null);
        }

        List<string> getValue;
        public List<string> GetListValue { get { return form.SelectedValue; } }
        public string GetStringValue
        {
            get
            {
                string result = string.Empty;
                foreach (var item in form.SelectedValue)
                {
                    result += item.Trim() + ",";
                }
                if (result.Length > 1) result = result.Substring(0, result.Length - 1).Replace(",", "','");
                return result;
            }
        }
        public string GetNodeValue
        {
            get
            {
                return form.SelectedNode;
            }
        }
        private void CustomComboboxDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


    }
}
