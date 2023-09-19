using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.CustomControl.Common
{
    public partial class CustomButton : Button
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        public CustomButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
