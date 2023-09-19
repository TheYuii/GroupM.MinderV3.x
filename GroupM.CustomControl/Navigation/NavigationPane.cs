using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.CustomControl.Navigation
{
    public partial class NavigationPane : UserControl
    {

        #region ### Constructor ###
        public NavigationPane()
        {
            InitializeComponent();
        }
        #endregion

        #region ### Member ###
        private DataTable _tableMenu = null;
        private MenuStrip _menuStrip = null;
        
        public DataTable TableMenu
        {
            set
            {
                _tableMenu = value;
            }
        }

        public MenuStrip MenuStrip
        {
            get { return _menuStrip; }
            set
            {
                _menuStrip = value;
                this.GenerateMenu(_menuStrip, _tableMenu);
            }
        }
        #endregion

        #region ### Method ###
        private void ClearMenu()
        {
            try
            {
                for (int i = 0; i < this.pnlFooter.Controls.Count;)
                {
                    this.pnlFooter.Controls.RemoveAt(0);
                }
                this.pnlFooter.Size = new Size(0, 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GenerateMenu(MenuStrip mns, DataTable dt)
        {
            this.ClearMenu();

            if (mns == null) return;

            try
            {
                for (int i = 0; i < mns.Items.Count; i++)
                {
                    ToolStripItem ctl = mns.Items[i];
                    if (ctl.GetType() == typeof(ToolStripMenuItem))
                    {
                        if (ctl.Text == "File" || ctl.Text == "Window")
                            continue;
                        if (ctl.Visible == true)
                        {
                            ToolStripMenuItem item = (ToolStripMenuItem)ctl;
                            // Create header button.
                            #region # Create Header Button #
                            Button btn = new Button();
                            btn.Font = new System.Drawing.Font("Tahoma", Convert.ToSingle(9), FontStyle.Regular);
                            btn.Text = "          " + item.Text.Trim();
                            btn.TextAlign = ContentAlignment.MiddleLeft;
                            btn.Image = (item.Image == null ? global::GroupM.CustomControl.Properties.Resources.desktop : item.Image);
                            btn.ImageAlign = ContentAlignment.MiddleLeft;
                            btn.BackColor = SystemColors.GradientActiveCaption;
                            btn.BackgroundImage = global::GroupM.CustomControl.Properties.Resources.GardientHeader;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 128, 0);
                            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 192, 128);
                            btn.FlatAppearance.BorderColor = SystemColors.GradientActiveCaption;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.Dock = DockStyle.Top;
                            btn.Size = new System.Drawing.Size(0, 40);
                            btn.Paint += new PaintEventHandler(btn_Paint);
                            btn.Click += new EventHandler(btn_Click);
                            this.pnlFooter.Controls.Add(btn);
                            this.pnlFooter.Size = new System.Drawing.Size(0, this.pnlFooter.Height + 40);
                            btn.BringToFront();
                            #endregion

                            // Create Treeview.
                            string str = string.Empty;
                            #region # Create Treeview #
                            TreeView trv = new TreeView();
                            trv.BackColor = SystemColors.InactiveBorder;
                            trv.Dock = DockStyle.Fill;
                            trv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                            trv.ShowPlusMinus = false;
                            trv.FullRowSelect = true;
                            trv.NodeMouseClick += new TreeNodeMouseClickEventHandler(trv_NodeMouseClick);
                            trv.ItemHeight = 20;
                            trv.Font = new Font("Tahoma", Convert.ToSingle(10), FontStyle.Regular);
                            if (item.DropDownItems.Count > 0)
                            {
                                foreach (ToolStripMenuItem dropDownItem in item.DropDownItems)
                                {
                                    if (dt == null)
                                    {
                                        TreeNode node = new TreeNode();
                                        node.Text = dropDownItem.Text;
                                        node.Tag = dropDownItem;
                                        trv.Nodes.Add(node);
                                        if (dropDownItem.HasDropDownItems)
                                        {
                                            AddDropDownItem(node, dropDownItem, dt);
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            if (item.Text + " - " + dropDownItem.Text == row["UserMenuName"].ToString())
                                            {
                                                TreeNode node = new TreeNode();
                                                node.Text = dropDownItem.Text;
                                                node.Tag = dropDownItem;
                                                trv.Nodes.Add(node);
                                                if (dropDownItem.HasDropDownItems)
                                                {
                                                    AddDropDownItem(node, dropDownItem, dt);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            trv.ExpandAll();
                            btn.Tag = trv;
                            #endregion

                            if (i == 0)
                            {
                                btn.BackColor = Color.FromArgb(255, 128, 0);
                                btn_Click(btn, new EventArgs());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddDropDownItem(TreeNode node, ToolStripMenuItem item, DataTable dt)
        {
            try
            {
                foreach (ToolStripMenuItem dropDownItems in item.DropDownItems)
                {
                    if (dt == null)
                    {
                        TreeNode subNode = new TreeNode();
                        subNode.Text = dropDownItems.Text;
                        subNode.Tag = dropDownItems;
                        node.Nodes.Add(subNode);
                        if (dropDownItems.HasDropDownItems)
                        {
                            AddDropDownItem(subNode, dropDownItems, dt);
                        }
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (item.Text + " - " + dropDownItems.Text == row["UserMenuName"].ToString())
                            {
                                TreeNode subNode = new TreeNode();
                                subNode.Text = dropDownItems.Text;
                                subNode.Tag = dropDownItems;
                                node.Nodes.Add(subNode);
                                if (dropDownItems.HasDropDownItems)
                                {
                                    AddDropDownItem(subNode, dropDownItems, dt);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ShowNavigation()
        {
            this.btnHeader.Tag = "show";

            //for (int i = this.pnlNavigation.Size.Width; i < 250; )
            //{
            //    this.pnlNavigation.Size = new Size(i, this.Height);
            //    i += 5;
            //    Application.DoEvents();
            //}

            this.Size = new Size(250, this.Size.Height);
            this.pnlItem.Visible = true;
            this.pnlHeader.Dock = DockStyle.Top;
            this.btnHeader.Image = global::GroupM.CustomControl.Properties.Resources.arrow_left;
            this.lblHeader.Show();
        }

        public void HideNavigation()
        {
            this.btnHeader.Tag = "hide";

            for (int i = this.pnlNavigation.Size.Width; i > 40;)
            {
                this.pnlNavigation.Size = new Size(i, this.pnlNavigation.Size.Height);
                i -= 20;
                Application.DoEvents();
            }

            this.Size = new Size(40, this.Size.Height);
            this.pnlItem.Visible = false;
            this.pnlHeader.Dock = DockStyle.Fill;
            this.btnHeader.Image = global::GroupM.CustomControl.Properties.Resources.arrow_right;
            this.lblHeader.Hide();
        }
        #endregion

        #region ### Event ###
        // New event
        public delegate void NodeClick(object source, NodeClickEventArgs e);

        public class NodeClickEventArgs : EventArgs
        {
            private string EventInfo;

            public NodeClickEventArgs(string Text)
            {
                EventInfo = Text;
            }

            public string GetInfo()
            {
                return EventInfo;
            }
        }

        private void btnHeader_Click(object sender, EventArgs e)
        {
            // Click to hide.
            if (this.btnHeader.Tag.ToString().ToLower() == "show")
            {
                this.btnHeader.Tag = "hide";

                for (int i = this.pnlNavigation.Size.Width; i > 40;)
                {
                    this.pnlNavigation.Size = new Size(i, this.pnlNavigation.Size.Height);
                    i -= 20;
                    Application.DoEvents();
                }

                this.Size = new Size(40, this.Size.Height);
                this.pnlItem.Visible = false;
                this.pnlHeader.Dock = DockStyle.Fill;
                this.btnHeader.Image = global::GroupM.CustomControl.Properties.Resources.arrow_right;
                this.lblHeader.Hide();

            }
            // Click to show.
            else if (this.btnHeader.Tag.ToString().ToLower() == "hide")
            {
                this.btnHeader.Tag = "show";

                //for (int i = this.pnlNavigation.Size.Width; i < 250; )
                //{
                //    this.pnlNavigation.Size = new Size(i, this.Height);
                //    i += 5;
                //    Application.DoEvents();
                //}

                this.Size = new Size(250, this.Size.Height);
                this.pnlItem.Visible = true;
                this.pnlHeader.Dock = DockStyle.Top;
                this.btnHeader.Image = global::GroupM.CustomControl.Properties.Resources.arrow_left;
                this.lblHeader.Show();
            }
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            Control ctl = (Panel)sender;
            Pen p = new Pen(SystemBrushes.ActiveCaption);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            e.Graphics.DrawLine(p, 0, ctl.Height - 1, ctl.Width, ctl.Height - 1);

            //if (this.btnHeader.Tag.ToString().ToLower() == "hide")
            //{
            //    StringFormat format = new StringFormat();
            //    format.Alignment = StringAlignment.Center;

            //    SizeF sz = e.Graphics.VisibleClipBounds.Size;
            //    //90 degrees
            //    e.Graphics.TranslateTransform(sz.Width, 0);
            //    e.Graphics.RotateTransform(90);
            //    e.Graphics.DrawString(this.lblHeader.Text, this.lblHeader.Font, new SolidBrush(this.lblHeader.ForeColor), new RectangleF(0, 0, sz.Height, sz.Width), format);
            //    e.Graphics.ResetTransform();
            //}
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            this.lblHeader.Text = btn.Text.TrimStart();

            foreach (Control ctl in this.pnlFooter.Controls)
            {
                if (ctl.GetType() == typeof(Button))
                {
                    ctl.BackColor = SystemColors.GradientActiveCaption;
                }
            }

            btn.BackColor = Color.FromArgb(255, 128, 0);

            if (this.pnlTreeMenu.Controls.Count > 0) this.pnlTreeMenu.Controls.RemoveAt(0);
            this.pnlTreeMenu.Controls.Add((TreeView)btn.Tag);
        }

        private void btn_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            Pen p = new Pen(SystemBrushes.ActiveCaption);

            // Top
            e.Graphics.DrawLine(p, new Point(0, 0), new Point(btn.Width - 1, 0));

            // Bottom
            //e.Graphics.DrawLine(p, new Point(0, btn.Height -1), new Point(btn.Width, btn.Height -1));
        }

        private void trv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = (TreeNode)e.Node;
            if (e.Node.Tag != null)
            {
                ToolStripMenuItem data = (ToolStripMenuItem)node.Tag;
                data.PerformClick();
            }
        }
        #endregion

    }
}
