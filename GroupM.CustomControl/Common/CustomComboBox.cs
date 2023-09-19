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
    public partial class CustomComboBox : ComboBox
    {

        #region ### Constructor ###
        public CustomComboBox()
        {
            InitializeComponent();
        }
        public CustomComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #endregion

        #region ### Propertiees ###

        private bool _IsRequireField;
        public bool IsRequireField
        {
            get { return _IsRequireField; }
            set
            {
                _IsRequireField = value;
                if (value == true)
                {
                    this.BackColor = Color.FromArgb(255, 255, 192);
                }
                else
                {
                    this.BackColor = SystemColors.Window;
                }
            }
        }

        private string _IsRequierFieldMessage;
        public string IsRequierFieldMessage
        {
            get { return _IsRequierFieldMessage; }
            set { _IsRequierFieldMessage = value; }
        }

        #endregion

        #region ### Method ###
        public void BindingData(DataTable dt)
        {
            this.BeginUpdate();
            this.DisplayMember = "display";
            this.ValueMember = "value";
            this.DataSource = dt;
            this.EndUpdate();
        }

        public void BindingData(DataTable dt, string optional)
        {
            DataRow dr = dt.NewRow();
            dr["display"] = optional;
            dr["value"] = optional;
            dt.Rows.InsertAt(dr, 0);

            this.BeginUpdate();
            this.DisplayMember = "display";
            this.ValueMember = "value";
            this.DataSource = dt;
            this.EndUpdate();
        }
        #endregion

    }
}
