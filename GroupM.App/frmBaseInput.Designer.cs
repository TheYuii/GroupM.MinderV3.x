namespace GroupM.App
{
    partial class frmBaseInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBaseInput));
            this.tsbControl = new GroupM.CustomControl.Common.ToolBarStrip(this.components);
            this.btnSave = new GroupM.CustomControl.Common.ToolBarStripButton(this.components);
            this.tsbControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsbControl
            // 
            this.tsbControl.AutoSize = false;
            this.tsbControl.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tsbControl.BackgroundImage = global::GroupM.App.Properties.Resources.bg;
            this.tsbControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tsbControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave});
            this.tsbControl.Location = new System.Drawing.Point(0, 0);
            this.tsbControl.Name = "tsbControl";
            this.tsbControl.Size = new System.Drawing.Size(784, 30);
            this.tsbControl.TabIndex = 0;
            this.tsbControl.Text = "toolBarStrip1";
            this.tsbControl.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsbControl_ItemClicked);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::GroupM.App.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 27);
            this.btnSave.Text = "Save";
            // 
            // frmBaseInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.tsbControl);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBaseInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Base Input";
            this.Load += new System.EventHandler(this.frmBaseInput_Load);
            this.tsbControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected CustomControl.Common.ToolBarStrip tsbControl;
        protected CustomControl.Common.ToolBarStripButton btnSave;

    }
}