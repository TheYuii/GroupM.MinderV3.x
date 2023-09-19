using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GroupM.CustomControl.TabMdi
{
    public partial class CustomTabControl : TabControl
    {

        #region ### Constructor ###
        public CustomTabControl()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseLeave += OnMouseLeave;

            // Add any initialization after the InitializeComponent() call.
            this.DrawMode = TabDrawMode.OwnerDrawFixed;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

        }

        protected override void OnMouseHover(EventArgs e)
        {


        }
        #endregion

        #region ### Colors ###

        Color clrBgGradientLeft = SystemColors.GradientActiveCaption;
        Color clrBgGradientRight = SystemColors.GradientActiveCaption;

        Color clrBorderBorder = Color.Orange;
        Color clrBorderFill = Color.Orange;

        Color clrBorderHighlight = SystemColors.GradientActiveCaption;
        Color clrTabBorder = SystemColors.ActiveCaption;
        Color clrTabShadow = SystemColors.ActiveCaption;
        Color clrTabGradientTop = SystemColors.ButtonFace;
        Color clrTabGradientBottom = SystemColors.GradientActiveCaption;

        #endregion

        #region ### Constants ###
        int tabHeight = 30;
        #endregion

        #region ### Event ###
        int pageHover = -1;
        Image imgClose = global::GroupM.CustomControl.Properties.Resources.close_black;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //Rectangles
            Rectangle totalRect = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle rectBorderFill = new Rectangle(3, tabHeight - 6, totalRect.Width - 6, totalRect.Height - tabHeight + 3);

            Brush b = new SolidBrush(SystemColors.ActiveCaption);
            e.Graphics.FillRectangle(b, rectBorderFill);

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //Brushes / pens
            SolidBrush borderFill = new SolidBrush(clrBorderFill);
            Pen borderBorder = new Pen(clrBorderBorder);
            Pen tabShadow = new Pen(clrTabShadow);
            Pen inactiveTabBorder = new Pen(clrTabBorder);
            LinearGradientBrush bTabGrad = null;
            Bitmap bm = new Bitmap(1, 1);

            // Draw inactive tab headers
            // Drawing in reverse order so they overlap properly
            Rectangle r = default(Rectangle);

            // Draw inactive tab header
            #region # Draw inactive tab header #
            bm.SetPixel(0, 0, clrTabBorder);

            for (int i = this.TabPages.Count - 1; i >= 0; i += -1)
            {
                r = this.GetTabRect(i);

                if (this.SelectedIndex != i)
                {
                    r = new Rectangle(r.X, r.Y, r.Width - 2, r.Height);
                    bTabGrad = new LinearGradientBrush(new Rectangle(r.X, r.Y, r.Width, r.Height + 1), clrTabGradientTop, clrTabGradientBottom, LinearGradientMode.Vertical);
                    e.Graphics.FillRectangle(bTabGrad, r.X, r.Y, r.Width, r.Height + 1);
                    //e.Graphics.FillRectangle(bTabGrad, r.X, r.Y + 3, 2, 11);
                    e.Graphics.DrawImageUnscaled(bm, 0, 0);
                    e.Graphics.DrawLine(inactiveTabBorder, r.Left, r.Bottom, r.Left, r.Top);
                    e.Graphics.DrawLine(inactiveTabBorder, r.Left, r.Top, r.Right, r.Top);
                    e.Graphics.DrawLine(inactiveTabBorder, r.Right, r.Top, r.Right, r.Bottom);
                    e.Graphics.DrawImageUnscaled(bm, r.Right - 1, r.Top + 1);

                    e.Graphics.DrawString(this.TabPages[i].Text, this.Font, Brushes.Black, r.X + 3, r.Y + 3);
                    //e.Graphics.DrawString("x", new Font("MS Sans Serif", Convert.ToSingle(11), FontStyle.Bold), Brushes.Black, r.X + r.Width - 15, r.Y + 1);
                    if (pageHover == i)
                    {
                        e.Graphics.DrawImage(global::GroupM.CustomControl.Properties.Resources.close_hover, r.X + r.Width - 15, r.Y + 4, 14, 14);
                    }
                    else
                    {
                        e.Graphics.DrawImage(global::GroupM.CustomControl.Properties.Resources.close_black, r.X + r.Width - 15, r.Y + 4, 14, 14);
                    }
                }
            }
            #endregion


            // Draw active tab header
            #region # Draw active tab header #
            if (this.SelectedIndex != -1)
            {
                bm.SetPixel(0, 0, clrBorderBorder);

                //Resize rectangle
                r = this.GetTabRect(this.SelectedIndex);
                r = new Rectangle(r.X, r.Y, r.Width - 3, r.Height);

                // Sloping part
                bTabGrad = new LinearGradientBrush(new Rectangle(r.X, r.Y + 1, r.Width, r.Height + 1), clrTabGradientTop, clrBorderFill, LinearGradientMode.Vertical);
                e.Graphics.FillRectangle(bTabGrad, r.X, r.Y, r.Width, r.Height + 2);
                //e.Graphics.FillRectangle(bTabGrad, r.X + 14, r.Y + 3, 2, 14);

                // Border
                e.Graphics.DrawLine(borderBorder, r.Left, r.Bottom, r.Left, r.Top);
                e.Graphics.DrawLine(borderBorder, r.Left, r.Top, r.Right, r.Top);
                e.Graphics.DrawLine(borderBorder, r.Right, r.Top, r.Right, r.Bottom);
                e.Graphics.DrawImageUnscaled(bm, r.Right - 1, r.Top + 1);

                e.Graphics.DrawString(this.TabPages[this.SelectedIndex].Text, new Font(this.Font, FontStyle.Regular), Brushes.Black, r.X + 3, r.Y + 3);
                //e.Graphics.DrawString("x", new Font("MS Sans Serif", Convert.ToSingle(11), FontStyle.Bold), Brushes.Black, r.X + r.Width - 15, r.Y + 1);
                if (pageHover == this.SelectedIndex)
                {
                    e.Graphics.DrawImage(global::GroupM.CustomControl.Properties.Resources.close_hover, r.X + r.Width - 15, r.Y + 4, 14, 14);
                }
                else
                {
                    e.Graphics.DrawImage(global::GroupM.CustomControl.Properties.Resources.close_black, r.X + r.Width - 15, r.Y + 4, 14, 14);
                }

            }
            #endregion

            base.OnPaint(e);

        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            Rectangle mouseRect = new Rectangle(e.X, e.Y, 1, 1);
            int tabWidth = 0;
            for (int i = 0; i < this.TabCount; i++)
            {
                tabWidth += this.GetTabRect(i).Width;
                var tab1 = this.GetTabRect(i).IntersectsWith(mouseRect);
                if (this.GetTabRect(i).IntersectsWith(mouseRect))
                {
                    if ((mouseRect.X < (tabWidth - 2)) && (mouseRect.X > (tabWidth - 15)))
                    {
                        this.TabPages.RemoveAt(i);
                    }
                    break;
                }
            }
            base.OnMouseClick(e);
        }
        #endregion

        #region ### Method ###
        public void OpenForm(Form frm)
        {
            if (this.TabPages.ContainsKey("page" + frm.Name) == false)
            {
                TabPage page = new TabPage();

                string frmName = string.Empty;
                if (frm.Text.ToString().Split('-').Length == 1)
                {
                    frmName = frm.Text.Split('-')[0];
                }
                else
                {
                    frmName = frm.Text.Split('-')[1].Trim();
                }

                page.Text = frmName;
                page.Name = "page" + frm.Name;

                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.FormClosing += frm_FormClosing;
                page.Controls.Add(frm);
                //this.SelectedTab = null;
                //this.SelectedTab = page;

                this.TabPages.Add(page);
                this.SelectedTab = page;
            }
            else
            {
                var page = this.TabPages["page" + frm.Name];
                if (page != null) this.SelectedTab = page;
            }


        }

        void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form frm = (Form)sender;
            foreach (TabPage page in this.TabPages)
            {
                if (page.Name == "page" + frm.Name)
                {
                    this.TabPages.Remove(page);
                    return;
                }
            }
        }
        public void CloseAllTab()
        {
            for (int i = 0; i < this.TabPages.Count; )
            {
                this.TabPages.RemoveAt(0);
            }
        }
        #endregion

        #region ### Move tab page ###
        private TabPage m_DraggedTab;
        private bool _isClose = false;

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            m_DraggedTab = TabAt(e.Location);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Rectangle mouseRect = new Rectangle(e.X, e.Y, 1, 1);
            int tabWidth = 0;
            pageHover = -1;
            for (int i = 0; i < this.TabCount; i++)
            {
                tabWidth += this.GetTabRect(i).Width;
                var tab1 = this.GetTabRect(i).IntersectsWith(mouseRect);
                if (this.GetTabRect(i).IntersectsWith(mouseRect))
                {

                    if ((mouseRect.X < (tabWidth - 2)) && (mouseRect.X > (tabWidth - 15)))
                    {
                        pageHover = i;
                        if (this._isClose == false)
                        {
                            this._isClose = true;
                            this.Refresh();
                        }
                        break;
                    }
                }
            }

            if (pageHover == -1)
            {
                if (this._isClose == true)
                {
                    this._isClose = false;
                    this.Refresh();
                }
            }

            if (e.Button != MouseButtons.Left || m_DraggedTab == null)
            {
                return;
            }

            TabPage tab = TabAt(e.Location);

            if (tab == null || tab == m_DraggedTab)
            {
                return;
            }

            Swap(m_DraggedTab, tab);
            SelectedTab = m_DraggedTab;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            pageHover = -1;
            if (this._isClose == true)
            {
                this._isClose = false;
                this.Refresh();
            }
            //this.Refresh();
        }


        private TabPage TabAt(Point position)
        {
            int count = TabCount;

            for (int i = 0; i < count; i++)
            {
                if (GetTabRect(i).Contains(position))
                {
                    return TabPages[i];
                }
            }

            return null;
        }

        private void Swap(TabPage a, TabPage b)
        {
            int i = TabPages.IndexOf(a);
            int j = TabPages.IndexOf(b);
            TabPages[i] = b;
            TabPages[j] = a;
        }
        #endregion

    }

}