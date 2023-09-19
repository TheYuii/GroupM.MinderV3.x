using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.CustomControl.Common
{
    public partial class CustomComboboxDetailForm : Form
    {

        #region ### Constant ###
        public CustomComboboxDetailForm()
        {
            InitializeComponent();
        }
        #endregion

        #region ### Properties ###

        public int ccbWidth { get; set; }

        public string strMediaType { get; set; }
        public int SelectedIndex { get; set; }
        public List<string> SelectedValue
        {
            get
            {
                List<string> result = new List<string>();
                foreach (TreeNode root in trvData.Nodes)
                {
                    if (root.Name == "ndAll")
                    {
                        foreach (TreeNode node in root.Nodes)
                        {
                            if (node.Checked == true) result.Add(node.Text);
                        }
                    }
                }
                return result;
            }
        }
        public string SelectedNode
        {
            get
            {
                string result = string.Empty;
                foreach (TreeNode node in trvData.Nodes["ndAll"].Nodes)
                {
                    if (node.Checked == true)
                    {
                        result += node.Tag.ToString() + ",";
                    }
                }
                if (result.Length > 1) result = result.Substring(0, result.Length - 1);
                //if (result.Length > 1) result = result.Substring(0, result.Length - 1).Replace(",", "','");
                return result;
            }
        }
        #endregion

        #region ### Method ###
        public void OnBinding(DataTable data, string displayName, string valueName,bool selectAll)
        {
            this.trvData.Nodes["ndAll"].Nodes.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                TreeNode node = new TreeNode();
                node.Name = "node" + data.Rows[i][displayName].ToString();
                node.Text = data.Rows[i][displayName].ToString();
                node.Tag = data.Rows[i][valueName].ToString();
                trvData.Nodes["ndAll"].Nodes.Add(node);
                if (selectAll) node.Checked = true;
            }
            if (selectAll) this.trvData.Nodes["ndAll"].Checked = true;
            this.trvData.ExpandAll();
        }
        #endregion

        #region ### Event ###
        public delegate void OnNodeChecked(object sender, TreeViewEventArgs e);
        public event OnNodeChecked NodeChecked;

        private void trvData_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name == "ndAll")
            {
                if (NodeChecked != null) NodeChecked(sender, e);
                if (e.Action == TreeViewAction.ByMouse)
                {
                    foreach (TreeNode node in e.Node.Nodes)
                    {
                        node.Checked = e.Node.Checked;
                        if (NodeChecked != null) NodeChecked(sender, e);
                    }
                }
            }
            else
            {
                if (NodeChecked != null) NodeChecked(sender, e);
                if (e.Action == TreeViewAction.ByMouse)
                {
                    var parent = e.Node.Parent;
                    int count = 0;
                    for (int i = 0; i < parent.Nodes.Count; i++)
                    {
                        if (parent.Nodes[i].Checked == true) count++;
                    }
                    if (parent.Nodes.Count == count)
                    {
                        parent.Checked = true;
                    }
                    else
                    {
                        parent.Checked = false;
                    }
                }
            }

        }

        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CustomComboboxDetailForm_Activated(object sender, EventArgs e)
        {
            this.trvData.ExpandAll();
        }

        private void CustomComboboxDetailForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        private void trvData_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Hide();
        }

        private void CustomComboboxDetailForm_Load(object sender, EventArgs e)
        {
            InitialForm();
        }

        private void InitialForm()
        {
            this.Width = ccbWidth + (ccbWidth / 2);
        }

    }
}
