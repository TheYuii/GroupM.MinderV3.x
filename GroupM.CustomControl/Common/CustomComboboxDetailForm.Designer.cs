namespace GroupM.CustomControl.Common
{
    partial class CustomComboboxDetailForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("All");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomComboboxDetailForm));
            this.trvData = new System.Windows.Forms.TreeView();
            this.btnOK = new GroupM.CustomControl.Common.CustomButton(this.components);
            this.SuspendLayout();
            // 
            // trvData
            // 
            this.trvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvData.CheckBoxes = true;
            this.trvData.Location = new System.Drawing.Point(1, 1);
            this.trvData.Name = "trvData";
            treeNode1.Name = "ndAll";
            treeNode1.Text = "All";
            this.trvData.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.trvData.ShowPlusMinus = false;
            this.trvData.Size = new System.Drawing.Size(394, 244);
            this.trvData.TabIndex = 0;
            this.trvData.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvData_AfterCheck);
            this.trvData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.trvData_KeyPress);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(209)))), ((int)(((byte)(234)))));
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnOK.Image = global::GroupM.CustomControl.Properties.Resources.check;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(309, 251);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // CustomComboboxDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(396, 285);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.trvData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "CustomComboboxDetailForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CustomComboboxDetailForm";
            this.Activated += new System.EventHandler(this.CustomComboboxDetailForm_Activated);
            this.Load += new System.EventHandler(this.CustomComboboxDetailForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CustomComboboxDetailForm_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trvData;
        private CustomButton btnOK;
    }
}