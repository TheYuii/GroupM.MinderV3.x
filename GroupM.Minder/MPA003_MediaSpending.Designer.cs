namespace GRM.MPA
{
    partial class MPA003_MediaSpending
    {
        #region Windows Form Designer generated code
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private AxDynamiCubeLib.AxDCube axDetail;
        private System.Windows.Forms.Button btnTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbCondition;
        private System.Windows.Forms.Button btnExport;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPA003_MediaSpending));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.axDetail = new AxDynamiCubeLib.AxDCube();
            this.btnTotal = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbCondition = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.srcData = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lbSportMatch = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcData)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pre and Post Buy Report";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.axDetail);
            this.panel1.Location = new System.Drawing.Point(4, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1004, 627);
            this.panel1.TabIndex = 4;
            // 
            // axDetail
            // 
            this.axDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axDetail.Enabled = true;
            this.axDetail.Location = new System.Drawing.Point(3, 3);
            this.axDetail.Name = "axDetail";
            this.axDetail.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDetail.OcxState")));
            this.axDetail.Size = new System.Drawing.Size(998, 615);
            this.axDetail.TabIndex = 1;
            this.axDetail.FetchAttributes += new AxDynamiCubeLib.IDCubeEvents_FetchAttributesEventHandler(this.axDetail_FetchAttributes);
            this.axDetail.FetchData += new System.EventHandler(this.axDetail_FetchData);
            // 
            // btnTotal
            // 
            this.btnTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTotal.Location = new System.Drawing.Point(932, 18);
            this.btnTotal.Name = "btnTotal";
            this.btnTotal.Size = new System.Drawing.Size(75, 27);
            this.btnTotal.TabIndex = 5;
            this.btnTotal.Text = "Hide Total";
            this.btnTotal.UseVisualStyleBackColor = true;
            this.btnTotal.Click += new System.EventHandler(this.btnTotal_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Condition Date :";
            // 
            // lbCondition
            // 
            this.lbCondition.AutoSize = true;
            this.lbCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbCondition.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbCondition.Location = new System.Drawing.Point(83, 36);
            this.lbCondition.Name = "lbCondition";
            this.lbCondition.Size = new System.Drawing.Size(38, 13);
            this.lbCondition.TabIndex = 7;
            this.lbCondition.Text = "NONE";
            this.lbCondition.Click += new System.EventHandler(this.lbCondition_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Image = global::GRM.MPA.Properties.Resources.excel;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(821, 18);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(105, 27);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "Minder Pre-Buy Report.xls";
            this.saveFileDialog.Filter = "Excel Files (*.xls)|*.xls";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Spot Match Range :";
            // 
            // lbSportMatch
            // 
            this.lbSportMatch.AutoSize = true;
            this.lbSportMatch.Location = new System.Drawing.Point(337, 37);
            this.lbSportMatch.Name = "lbSportMatch";
            this.lbSportMatch.Size = new System.Drawing.Size(38, 13);
            this.lbSportMatch.TabIndex = 6;
            this.lbSportMatch.Text = "NONE";
            // 
            // button1
            // 
            this.button1.Image = global::GRM.MPA.Properties.Resources.excel;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(210, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 27);
            this.button1.TabIndex = 8;
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // MPA003_MediaSpending
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1016, 692);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lbCondition);
            this.Controls.Add(this.lbSportMatch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnTotal);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MPA003_MediaSpending";
            this.Text = "MPA003 - Pre and Post Buy Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MPA003_MediaSpending_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.srcData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbSportMatch;
        private System.Windows.Forms.Button button1;

    }
}
