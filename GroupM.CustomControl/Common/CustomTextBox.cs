using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.CustomControl.Common
{
    public partial class CustomTextBox : TextBox
    {
        #region ### Constructor ###
        public CustomTextBox()
        {
            InitializeComponent();
        }

        public CustomTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        #endregion

        #region ### Enum ###
        public enum eTextBoxType
        {
            Text,
            Number,
            Decimal,
        }
        #endregion

        #region ### Member ###
        public eTextBoxType TextBoxType { get; set; }

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

        #region ### Event ###
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (this.TextBoxType == eTextBoxType.Number)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
            }
            else if (this.TextBoxType == eTextBoxType.Decimal)
            {
                if ((e.KeyChar == '.') && (this.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }

            base.OnKeyPress(e);
        }
        #endregion

    }
}
