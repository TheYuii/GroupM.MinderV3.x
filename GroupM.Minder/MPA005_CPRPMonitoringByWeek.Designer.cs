namespace  GroupM.Minder
{
    partial class MPA005_CPRPMonitoringByWeek
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.BindingSource srcData;
        private System.ComponentModel.IContainer components;

        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.srcData = new System.Windows.Forms.BindingSource(this.components);
            this.gvDetail = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gvDesc = new System.Windows.Forms.DataGridView();
            this.colAgencyName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbSportMatch = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbCondition = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefreshData = new DevExpress.XtraEditors.SimpleButton();
            this.chkHideColTotal = new DevExpress.XtraEditors.CheckEdit();
            this.chkHideRowTotal = new DevExpress.XtraEditors.CheckEdit();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.srcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideColTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideRowTotal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "Minder Pre-Buy Report.xls";
            this.saveFileDialog.Filter = "Excel Files (*.xls)|*.xls";
            // 
            // gvDetail
            // 
            this.gvDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvDetail.Location = new System.Drawing.Point(0, 118);
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.Size = new System.Drawing.Size(1016, 574);
            this.gvDetail.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Pink;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox1.Location = new System.Drawing.Point(0, 98);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(1016, 19);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "  CPRP Monitoring By Week";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(895, 90);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gvDesc);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(887, 64);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Criteria";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gvDesc
            // 
            this.gvDesc.AllowUserToAddRows = false;
            this.gvDesc.AllowUserToDeleteRows = false;
            this.gvDesc.AllowUserToResizeColumns = false;
            this.gvDesc.AllowUserToResizeRows = false;
            this.gvDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvDesc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvDesc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDesc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAgencyName,
            this.From,
            this.Column9,
            this.Column1,
            this.Column8});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvDesc.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvDesc.EnableHeadersVisualStyles = false;
            this.gvDesc.Location = new System.Drawing.Point(2, 8);
            this.gvDesc.Name = "gvDesc";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvDesc.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gvDesc.RowHeadersVisible = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            this.gvDesc.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.gvDesc.Size = new System.Drawing.Size(883, 49);
            this.gvDesc.TabIndex = 5;
            this.gvDesc.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvDesc_CellMouseClick);
            // 
            // colAgencyName
            // 
            this.colAgencyName.DataPropertyName = "Condition";
            this.colAgencyName.HeaderText = "Condition";
            this.colAgencyName.Name = "colAgencyName";
            this.colAgencyName.ReadOnly = true;
            this.colAgencyName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAgencyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAgencyName.Width = 55;
            // 
            // From
            // 
            this.From.DataPropertyName = "From";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.From.DefaultCellStyle = dataGridViewCellStyle2;
            this.From.HeaderText = "From";
            this.From.Name = "From";
            this.From.ReadOnly = true;
            this.From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.From.Width = 80;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "To";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.Column9.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column9.HeaderText = "To";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column9.Width = 80;
            // 
            // Column1
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column1.HeaderText = "Matched";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column8.DataPropertyName = "ClientName";
            this.Column8.HeaderText = "Client Name";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // lbSportMatch
            // 
            this.lbSportMatch.AutoSize = true;
            this.lbSportMatch.Location = new System.Drawing.Point(476, 98);
            this.lbSportMatch.Name = "lbSportMatch";
            this.lbSportMatch.Size = new System.Drawing.Size(38, 13);
            this.lbSportMatch.TabIndex = 10;
            this.lbSportMatch.Text = "NONE";
            this.lbSportMatch.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(376, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Spot Match Range :";
            this.label3.Visible = false;
            // 
            // lbCondition
            // 
            this.lbCondition.AutoSize = true;
            this.lbCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbCondition.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbCondition.Location = new System.Drawing.Point(332, 98);
            this.lbCondition.Name = "lbCondition";
            this.lbCondition.Size = new System.Drawing.Size(38, 13);
            this.lbCondition.TabIndex = 7;
            this.lbCondition.Text = "NONE";
            this.lbCondition.Visible = false;
            this.lbCondition.Click += new System.EventHandler(this.lbCondition_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Condition Date :";
            this.label2.Visible = false;
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshData.Image = global::GroupM.Minder.Properties.Resources.refresh;
            this.btnRefreshData.Location = new System.Drawing.Point(908, 4);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(101, 21);
            this.btnRefreshData.TabIndex = 21;
            this.btnRefreshData.Text = "Refresh Data";
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // chkHideColTotal
            // 
            this.chkHideColTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHideColTotal.Location = new System.Drawing.Point(908, 75);
            this.chkHideColTotal.Name = "chkHideColTotal";
            this.chkHideColTotal.Properties.Caption = "Hide Col Total";
            this.chkHideColTotal.Size = new System.Drawing.Size(101, 19);
            this.chkHideColTotal.TabIndex = 20;
            this.chkHideColTotal.CheckedChanged += new System.EventHandler(this.chkHideColTotal_CheckedChanged);
            // 
            // chkHideRowTotal
            // 
            this.chkHideRowTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHideRowTotal.EditValue = true;
            this.chkHideRowTotal.Location = new System.Drawing.Point(908, 54);
            this.chkHideRowTotal.Name = "chkHideRowTotal";
            this.chkHideRowTotal.Properties.Caption = "Hide Row Total";
            this.chkHideRowTotal.Size = new System.Drawing.Size(101, 19);
            this.chkHideRowTotal.TabIndex = 19;
            this.chkHideRowTotal.CheckedChanged += new System.EventHandler(this.chkHideRowTotal_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Image = global::GroupM.Minder.Properties.Resources.ic_list_mimetype_excel;
            this.btnExport.Location = new System.Drawing.Point(908, 29);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(101, 23);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click_1);
            // 
            // MPA005_CPRPMonitoringByWeek
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1016, 692);
            this.Controls.Add(this.btnRefreshData);
            this.Controls.Add(this.chkHideColTotal);
            this.Controls.Add(this.chkHideRowTotal);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gvDetail);
            this.Controls.Add(this.lbSportMatch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbCondition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Name = "MPA005_CPRPMonitoringByWeek";
            this.Text = "MPA005 - CPRP Monitoring By Week";
            this.Load += new System.EventHandler(this.MPA003_MediaSpending_Load);
            ((System.ComponentModel.ISupportInitialize)(this.srcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideColTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideRowTotal.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private DevExpress.XtraPivotGrid.PivotGridControl gvDetail;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lbSportMatch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbCondition;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnRefreshData;
        private DevExpress.XtraEditors.CheckEdit chkHideColTotal;
        private DevExpress.XtraEditors.CheckEdit chkHideRowTotal;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private System.Windows.Forms.DataGridView gvDesc;
        private System.Windows.Forms.DataGridViewButtonColumn colAgencyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}
