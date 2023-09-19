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
    [Designer(typeof(System.ComponentModel.Design.ComponentDesigner))]
    public partial class ToolBarStripButton : ToolStripButton
    {
        public ToolBarStripButton()
        {
            InitializeComponent();
        }

        public ToolBarStripButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
