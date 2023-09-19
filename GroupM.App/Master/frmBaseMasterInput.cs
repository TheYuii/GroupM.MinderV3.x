using GroupM.CustomControl.Common;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App.Master
{
    public partial class frmBaseMasterInput<TEntity> : Form where TEntity : class
    {

        #region ### Constructor ###
        public frmBaseMasterInput()
        {
            InitializeComponent();
        }
        #endregion

        #region ### Properties ###
        public TEntity entity;

        public Action _action { get; set; }
        public enum Action
        {
            Add,
            Edit,
        }

        List<Control> listControl = new List<Control>();
        #endregion

        #region ### Method ###

        protected virtual void OnBindingData()
        {

            //this.txtCode.DataBindings.Clear();
            //this.txtName.DataBindings.Clear();
            //this.txtDescription.DataBindings.Clear();
            //this.chkActive.DataBindings.Clear();

            //this.txtCode.DataBindings.Add("Text", entity, "Code", true, DataSourceUpdateMode.OnPropertyChanged);
            //this.txtName.DataBindings.Add("Text", entity, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            //this.txtDescription.DataBindings.Add("Text", entity, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
            //this.chkActive.DataBindings.Add("Checked", entity, "Active", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void GetAllControl(Control.ControlCollection parrent)
        {

            foreach (Control item in parrent)
            {
                if (item.Controls.Count > 0)
                {
                    GetAllControl(item.Controls);
                }
                listControl.Add(item);
            }
        }

        protected virtual bool OnValidateRequireField()
        {
            bool result = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.GetAllControl(this.Controls);

                foreach (Control ctl in listControl)
                {
                    if (ctl.GetType() == typeof(CustomTextBox))
                    {
                        CustomTextBox txt = (CustomTextBox)ctl;
                        if (txt.IsRequireField == true && string.IsNullOrEmpty(txt.Text))
                        {
                            GMessage.MessageWarning(txt.IsRequierFieldMessage);
                            txt.Focus();
                            return false;
                        }
                    }
                    else if (ctl.GetType() == typeof(CustomComboBox))
                    {
                        CustomComboBox cmb = (CustomComboBox)ctl;
                        if (cmb.IsRequireField == true && cmb.SelectedIndex < 0)
                        {
                            GMessage.MessageWarning(cmb.IsRequierFieldMessage);
                            cmb.Focus();
                            return false;
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception Message", MessageBoxButtons.OK);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return result;
        }

        protected virtual void OnValidateData()
        {

        }

        protected virtual void OnSaveData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (Model.DbModel model = new Model.DbModel())
                {
                    if (this._action == Action.Add)
                    {
                        model.Entry(entity).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        model.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                    }
                    model.SaveChanges();
                }
                GMessage.MessageInfo("Save successefully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception Message", MessageBoxButtons.OK);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        #endregion

        #region ### Event ###
        private void frmBaseMasterInput_Load(object sender, EventArgs e)
        {
            this.OnBindingData();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            this.listControl.Clear();
            if (this.OnValidateRequireField() == true)
            {
                this.OnSaveData();
            }
        }
        #endregion

    }
}
