using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Core.Objects;

namespace GroupM.App.Master
{
    public partial class frmBaseMasterDisplay<TEntity, TFormInput> : Form
        where TEntity : class
        where TFormInput : frmBaseMasterInput<TEntity>
    {

        #region ### Constructor ###
        public frmBaseMasterDisplay()
        {
            InitializeComponent();
            this.FormInput = this.GetFormInput();
        }
        #endregion

        #region ### Properties ###
        private List<TEntity> list = new List<TEntity>();
        private TEntity current;

        private frmBaseMasterInput<TEntity> GetFormInput()
        {
            frmBaseMasterInput<TEntity> result = null;
            Type typeParameterType = typeof(TFormInput);
            result = (frmBaseMasterInput<TEntity>)Activator.CreateInstance(typeParameterType);
            return result;
        }

        private frmBaseMasterInput<TEntity> FormInput { get; set; }
        #endregion

        #region ### Method ###
        public virtual void OnLoadingData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var taskSchedule = TaskScheduler.FromCurrentSynchronizationContext();
                var myTask = Task<IEnumerable<TEntity>>.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(10000);
                    using (Model.DbModel db = new Model.DbModel())
                    {
                        IQueryable<TEntity> query = db.Set<TEntity>();
                        return query.ToList();
                    }
                });

                myTask.ContinueWith(x =>
                    {

                        this.bs.DataSource = x.Result;

                        this.gvData.AutoGenerateColumns = false;
                        this.gvData.DataSource = bs;
                    }, taskSchedule);
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

        public virtual void OnCommandNewData()
        {
            using (Model.DbModel db = new Model.DbModel())
            {
                current = db.Set<TEntity>().Create();
                typeof(TEntity).GetProperty("CreateBy").SetValue(current, GlobalVariable.UserName, null);
            }

            if (this.FormInput != null)
            {
                this.FormInput.Owner = this;
                this.FormInput.entity = current;
                this.FormInput._action = frmBaseMasterInput<TEntity>.Action.Add;
                this.FormInput.ShowDialog();
            }
        }
        public virtual void OnCommandEditData()
        {
            if (this.FormInput != null)
            {

                using (Model.DbModel db = new Model.DbModel())
                {
                    current = db.Set<TEntity>().Create();
                    typeof(TEntity).GetProperty("UpdateBy").SetValue(current, GlobalVariable.UserName, null);
                }

                Type t = current.GetType();
                foreach (var propInfo in t.GetProperties())
                {
                    var value = typeof(TEntity).GetProperty(propInfo.Name).GetValue(this.bs.Current);
                    propInfo.SetValue(current, value, null);
                }

                if (this.bs.Current != null)
                {

                    this.FormInput.entity = current; // (TEntity)this.bs.Current;
                }
                this.FormInput._action = frmBaseMasterInput<TEntity>.Action.Edit;
                this.FormInput.ShowDialog();
            }
        }
        public virtual void OnCommandDeleteData()
        {
            using (Model.DbModel db = new Model.DbModel())
            {
                if (this.bs.Current != null)
                {
                    current = (TEntity)this.bs.Current;
                    db.Entry(current).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region ### Event ###
        private void frmBaseMasterDisplay_Load(object sender, EventArgs e)
        {

        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            this.OnLoadingData();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.OnCommandNewData();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            this.OnCommandEditData();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            this.OnCommandDeleteData();
        }

        #endregion

    }
}
