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
    public partial class frmTestMasterInput : frmTestMasterInputTemp
    {
        public frmTestMasterInput()
        {
            InitializeComponent();
        }

        protected override void OnBindingData()
        {
            this.customTextBox1.DataBindings.Clear();
            this.customTextBox2.DataBindings.Clear();
            this.customTextBox3.DataBindings.Clear();
            this.customTextBox4.DataBindings.Clear();
            this.customTextBox5.DataBindings.Clear();
            this.customTextBox6.DataBindings.Clear();
            this.customTextBox7.DataBindings.Clear();
            this.customTextBox8.DataBindings.Clear();
            this.customTextBox9.DataBindings.Clear();
            this.customTextBox10.DataBindings.Clear();

            this.customTextBox1.DataBindings.Add("Text", entity, "Field1", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox2.DataBindings.Add("Text", entity, "Field2", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox3.DataBindings.Add("Text", entity, "Field3", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox4.DataBindings.Add("Text", entity, "Field4", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox5.DataBindings.Add("Text", entity, "Field5", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox6.DataBindings.Add("Text", entity, "Field6", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox7.DataBindings.Add("Text", entity, "Field7", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox8.DataBindings.Add("Text", entity, "Field8", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox9.DataBindings.Add("Text", entity, "Field9", true, DataSourceUpdateMode.OnPropertyChanged);
            this.customTextBox10.DataBindings.Add("Text", entity, "Field10", true, DataSourceUpdateMode.OnPropertyChanged);
        }

    }

    public class frmTestMasterInputTemp : frmBaseMasterInput<Model.TableTest> { }

}
