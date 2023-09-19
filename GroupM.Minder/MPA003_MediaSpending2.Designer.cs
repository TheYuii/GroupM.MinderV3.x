namespace  GroupM.Minder
{
    partial class MPA003_MediaSpending2
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbCondition;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPA003_MediaSpending2));
            this.label2 = new System.Windows.Forms.Label();
            this.lbCondition = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.lbSportMatch = new System.Windows.Forms.Label();
            this.gvDetail = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.gvDesc = new System.Windows.Forms.DataGridView();
            this.colAgencyName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rdoHighSpeed = new System.Windows.Forms.RadioButton();
            this.rdoRealTime = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSeparate1 = new System.Windows.Forms.Label();
            this.chkDecimal = new DevExpress.XtraEditors.CheckEdit();
            this.btnCreateOwnDayPart = new DevExpress.XtraEditors.SimpleButton();
            this.txtLoadTemplate = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.chkHideColTotal = new DevExpress.XtraEditors.CheckEdit();
            this.chkHideRowTotal = new DevExpress.XtraEditors.CheckEdit();
            this.btnRefreshData = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.srcData = new System.Windows.Forms.BindingSource(this.components);
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDesc)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDecimal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoadTemplate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideColTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideRowTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcData)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Condition Date :";
            this.label2.Visible = false;
            // 
            // lbCondition
            // 
            this.lbCondition.AutoSize = true;
            this.lbCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbCondition.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbCondition.Location = new System.Drawing.Point(349, 99);
            this.lbCondition.Name = "lbCondition";
            this.lbCondition.Size = new System.Drawing.Size(38, 13);
            this.lbCondition.TabIndex = 7;
            this.lbCondition.Text = "NONE";
            this.lbCondition.Visible = false;
            this.lbCondition.Click += new System.EventHandler(this.lbCondition_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "Minder Pre-Buy Report.xls";
            this.saveFileDialog.Filter = "Excel file(*.xlsx)|*.xlsx|Excel file - unmerge row(*.xlsx)|*.xlsx";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(393, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Spot Match Range :";
            this.label3.Visible = false;
            // 
            // lbSportMatch
            // 
            this.lbSportMatch.AutoSize = true;
            this.lbSportMatch.Location = new System.Drawing.Point(502, 99);
            this.lbSportMatch.Name = "lbSportMatch";
            this.lbSportMatch.Size = new System.Drawing.Size(38, 13);
            this.lbSportMatch.TabIndex = 6;
            this.lbSportMatch.Text = "NONE";
            this.lbSportMatch.Visible = false;
            // 
            // gvDetail
            // 
            this.gvDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvDetail.Location = new System.Drawing.Point(4, 124);
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsBehavior.ApplyBestFitOnFieldDragging = true;
            this.gvDetail.OptionsBehavior.BestFitConsiderCustomAppearance = true;
            this.gvDetail.Size = new System.Drawing.Size(1012, 566);
            this.gvDetail.TabIndex = 9;
            this.gvDetail.CustomSummary += new DevExpress.XtraPivotGrid.PivotGridCustomSummaryEventHandler(this.gvDetail_CustomSummary);
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
            this.gvDesc.Location = new System.Drawing.Point(4, 6);
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
            this.gvDesc.TabIndex = 4;
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
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Gold;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox1.Location = new System.Drawing.Point(-2, 99);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(1018, 19);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "   Pre and Post Buy Report";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(6, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(89, 23);
            this.btnLoad.TabIndex = 13;
            this.btnLoad.Text = "Load Template";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(114, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(895, 87);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gvDesc);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(887, 61);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Criteria";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rdoHighSpeed);
            this.tabPage2.Controls.Add(this.rdoRealTime);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.lblSeparate1);
            this.tabPage2.Controls.Add(this.chkDecimal);
            this.tabPage2.Controls.Add(this.btnCreateOwnDayPart);
            this.tabPage2.Controls.Add(this.txtLoadTemplate);
            this.tabPage2.Controls.Add(this.btnSave);
            this.tabPage2.Controls.Add(this.btnLoad);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(887, 61);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tools";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rdoHighSpeed
            // 
            this.rdoHighSpeed.AutoSize = true;
            this.rdoHighSpeed.Location = new System.Drawing.Point(539, 9);
            this.rdoHighSpeed.Name = "rdoHighSpeed";
            this.rdoHighSpeed.Size = new System.Drawing.Size(81, 17);
            this.rdoHighSpeed.TabIndex = 21;
            this.rdoHighSpeed.Text = "High Speed";
            this.rdoHighSpeed.UseVisualStyleBackColor = true;
            this.rdoHighSpeed.Visible = false;
            // 
            // rdoRealTime
            // 
            this.rdoRealTime.AutoSize = true;
            this.rdoRealTime.Checked = true;
            this.rdoRealTime.Location = new System.Drawing.Point(539, 33);
            this.rdoRealTime.Name = "rdoRealTime";
            this.rdoRealTime.Size = new System.Drawing.Size(306, 17);
            this.rdoRealTime.TabIndex = 21;
            this.rdoRealTime.TabStop = true;
            this.rdoRealTime.Text = "Real Time (The data will come out very SLOW in this mode)";
            this.rdoRealTime.UseVisualStyleBackColor = true;
            this.rdoRealTime.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(525, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 49);
            this.label1.TabIndex = 20;
            this.label1.Visible = false;
            // 
            // lblSeparate1
            // 
            this.lblSeparate1.BackColor = System.Drawing.Color.DimGray;
            this.lblSeparate1.Location = new System.Drawing.Point(365, 5);
            this.lblSeparate1.Name = "lblSeparate1";
            this.lblSeparate1.Size = new System.Drawing.Size(1, 49);
            this.lblSeparate1.TabIndex = 19;
            // 
            // chkDecimal
            // 
            this.chkDecimal.EditValue = true;
            this.chkDecimal.Location = new System.Drawing.Point(377, 36);
            this.chkDecimal.Name = "chkDecimal";
            this.chkDecimal.Properties.Caption = "Show Cost Decimal";
            this.chkDecimal.Size = new System.Drawing.Size(117, 19);
            this.chkDecimal.TabIndex = 18;
            this.chkDecimal.CheckedChanged += new System.EventHandler(this.chkDecimal_CheckedChanged);
            // 
            // btnCreateOwnDayPart
            // 
            this.btnCreateOwnDayPart.Image = global::GroupM.Minder.Properties.Resources.add;
            this.btnCreateOwnDayPart.Location = new System.Drawing.Point(377, 6);
            this.btnCreateOwnDayPart.Name = "btnCreateOwnDayPart";
            this.btnCreateOwnDayPart.Size = new System.Drawing.Size(137, 21);
            this.btnCreateOwnDayPart.TabIndex = 17;
            this.btnCreateOwnDayPart.Text = "Create Own Day Part";
            this.btnCreateOwnDayPart.Click += new System.EventHandler(this.btnCreateOwnDayPart_Click);
            // 
            // txtLoadTemplate
            // 
            this.txtLoadTemplate.Location = new System.Drawing.Point(101, 9);
            this.txtLoadTemplate.Name = "txtLoadTemplate";
            this.txtLoadTemplate.Size = new System.Drawing.Size(248, 20);
            this.txtLoadTemplate.TabIndex = 14;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(7, 33);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save Template";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkHideColTotal
            // 
            this.chkHideColTotal.Location = new System.Drawing.Point(7, 77);
            this.chkHideColTotal.Name = "chkHideColTotal";
            this.chkHideColTotal.Properties.Caption = "Hide Col Total";
            this.chkHideColTotal.Size = new System.Drawing.Size(101, 19);
            this.chkHideColTotal.TabIndex = 16;
            this.chkHideColTotal.CheckedChanged += new System.EventHandler(this.chkHideColTotal_CheckedChanged);
            // 
            // chkHideRowTotal
            // 
            this.chkHideRowTotal.EditValue = true;
            this.chkHideRowTotal.Location = new System.Drawing.Point(7, 56);
            this.chkHideRowTotal.Name = "chkHideRowTotal";
            this.chkHideRowTotal.Properties.Caption = "Hide Row Total";
            this.chkHideRowTotal.Size = new System.Drawing.Size(101, 19);
            this.chkHideRowTotal.TabIndex = 15;
            this.chkHideRowTotal.CheckedChanged += new System.EventHandler(this.chkHideRowTotal_CheckedChanged);
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Image = global::GroupM.Minder.Properties.Resources.refresh;
            this.btnRefreshData.Location = new System.Drawing.Point(7, 6);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(101, 21);
            this.btnRefreshData.TabIndex = 17;
            this.btnRefreshData.Text = "Refresh Data";
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = global::GroupM.Minder.Properties.Resources.ic_list_mimetype_excel;
            this.btnExport.Location = new System.Drawing.Point(7, 31);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(101, 23);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click_1);
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // MPA003_MediaSpending2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1016, 692);
            this.Controls.Add(this.btnRefreshData);
            this.Controls.Add(this.chkHideColTotal);
            this.Controls.Add(this.chkHideRowTotal);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.gvDetail);
            this.Controls.Add(this.lbCondition);
            this.Controls.Add(this.lbSportMatch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MPA003_MediaSpending2";
            this.Text = "MPA003 - Pre and Post Buy Report";
            this.Load += new System.EventHandler(this.MPA003_MediaSpending2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDesc)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDecimal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoadTemplate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideColTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHideRowTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbSportMatch;
        private DevExpress.XtraPivotGrid.PivotGridControl gvDetail;
        private System.Windows.Forms.DataGridView gvDesc;
        private System.Windows.Forms.TextBox textBox1;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.DataGridViewButtonColumn colAgencyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private DevExpress.XtraEditors.TextEdit txtLoadTemplate;
        private DevExpress.XtraEditors.SimpleButton btnRefreshData;
        private DevExpress.XtraEditors.CheckEdit chkHideColTotal;
        private DevExpress.XtraEditors.CheckEdit chkHideRowTotal;
        private DevExpress.XtraEditors.SimpleButton btnCreateOwnDayPart;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private DevExpress.XtraEditors.CheckEdit chkDecimal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSeparate1;
        private System.Windows.Forms.RadioButton rdoHighSpeed;
        private System.Windows.Forms.RadioButton rdoRealTime;

    }
}
