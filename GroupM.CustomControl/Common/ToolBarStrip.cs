using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GroupM.CustomControl.Common
{
    [Designer(typeof(ControlDesigner))]
    public partial class ToolBarStrip : ToolStrip
    {
        public ToolBarStrip()
        {
            InitializeComponent();
        }

        public ToolBarStrip(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
