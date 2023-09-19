using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App.Master.MasterTest
{
    public partial class frmTestMasterDisplay : frmTestMasterDisplayTemp
    {
        public frmTestMasterDisplay()
        {
            InitializeComponent();
        }
    }

    public class frmTestMasterDisplayTemp : frmBaseMasterDisplay<Model.TableTest, frmTestMasterInput> { }

}
