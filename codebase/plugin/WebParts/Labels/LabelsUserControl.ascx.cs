using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;
using System.ComponentModel;
using Microsoft.SharePoint;

namespace OoyalaPlugin.AssetsThumbnailView.Labels
{
    public partial class LabelsUserControl : UserControl
    {
        #region Public Properties

        public bool IsAllowAdd { get; set; }
        public bool IsAllowDelete { get; set; }
        public bool IsAllowUpdate { get; set; }

        public string APIKey
        {
            get { return txtAPIKey.Text; }
            set { txtAPIKey.Text = value; }
        }

        public string SecretKey
        {
            get { return txtSecretKey.Text; }
            set { txtSecretKey.Text = value; }
        }

        [Browsable(true)]
        public TreeView LableTreeView
        {
            get
            {
                return trvView;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PropertySettings();
            if (!IsPostBack)
            {
                TreeViewSettings();
                BindTreviewData();
            }
        }

        protected void AddButton_Click(object sender, ImageClickEventArgs e)
        {
                lblResults.Text = string.Empty;
                txtlblName.Visible = false;
                AddNewLabel();
        }

        protected void EditButton_Click(object sender, ImageClickEventArgs e)
        {
           if (IsAllowUpdate)
            {
                if(trvView.CheckedNodes.Count==1)                
                {
                    pnlSave.Visible = true;
                    imgSave.Visible = true;
                    imgCancel.Visible = true;
                    txtlblName.Text = trvView.CheckedNodes[0].Text;
                    txtlblID.Text = trvView.CheckedNodes[0].Value;
                }
            }
            lblResults.Text = string.Empty;
        }

        protected void SaveButton_Click(object sender, ImageClickEventArgs e)
        {
            SaveLabelSettings();
            pnlSave.Visible = false;
            imgSave.Visible = false;
            imgCancel.Visible = false;
            txtlblName.Text = string.Empty; 
            
        }

        protected void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            txtlblName.Text = string.Empty;            
            pnlSave.Visible = false;
            imgSave.Visible = false;
            imgCancel.Visible = false;
            lblResults.Text = string.Empty;
        }

        protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
        {

            lblResults.Text = string.Empty;
          
            if (trvView.CheckedNodes.Count > 0)
            {
                foreach (TreeNode tn in LableTreeView.CheckedNodes)
                {
                    DeleteLabel(tn.Value);
                }
                pnlSave.Visible = false;
                imgSave.Visible = false;
                imgCancel.Visible = false;
                BindTreviewData();
                
            }
            else
            { lblResults.Text = "<font color=red>No checked items were found for Delete!.</font>"; }
                   
        }
               

        #endregion

        #region Private Methods

        private void BindTreviewData()
        {
            try
            {
                List<OoyalaData.Labels.Item> labels = OoyalaMediaUtils.GetAllLabels(SecretKey, APIKey);
                trvView.Nodes.Clear();
                TreeNode tn = new TreeNode();
                AddLevel(ref tn, labels, null);
                //trvView.ExpandAll();
                trvView.CollapseAll();
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void AddLevel(ref TreeNode tn, List<OoyalaData.Labels.Item> source, string parent)
        {
            var list = from itm in source where itm.parent_id == parent
                       select itm;

            foreach (OoyalaData.Labels.Item itm in list)
            {
                TreeNode nd = new TreeNode();
                nd.Text = itm.name;
                nd.Value = itm.id;
                nd.SelectAction = TreeNodeSelectAction.None;
                AddLevel(ref nd, source, itm.id);
                if (string.IsNullOrEmpty(parent))
                    trvView.Nodes.Add(nd);
                else
                    tn.ChildNodes.Add(nd);                    
            }
        }

        private void AddNewLabel()
        {
            try
            {
                string id = string.Empty;
                Hashtable hs = new Hashtable();
                hs.Add("name", "New Label");

                if (trvView.CheckedNodes.Count == 1)
                {
                    id = trvView.CheckedNodes[0].Value;
                    hs.Add("parent_id", id);
                }
                 
                var res=OoyalaMediaUtils.AddNewLabel(SecretKey, APIKey, hs);
                if (string.IsNullOrEmpty(res))
                {
                    //lblResults.Text = "<font color=red>You do not have permission to perform this operation.</font>";
                    lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg  + " </font>";
                    WPHelper.permissionErrorMsg = string.Empty;
                    return;
                     
                }

                BindTreviewData();
                txtlblName.Text = string.Empty;
                Response.Redirect(SPContext.Current.File.Name);
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void DeleteLabel(string treeNodeValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(treeNodeValue))
                {
                    //OoyalaMediaUtils.DeleteLabel(SecretKey, APIKey, treeNodeValue);
                    var res = OoyalaMediaUtils.DeleteLabel(SecretKey, APIKey, treeNodeValue);
                    if (!res)
                    {
                        lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                        WPHelper.permissionErrorMsg = string.Empty;
                        return;
                    }
                    Response.Redirect(SPContext.Current.File.Name);
                }
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void SaveLabelSettings()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtlblID.Text))
                {
                    string id = txtlblID.Text;
                    Hashtable hs = new Hashtable();
                    hs.Add("name", txtlblName.Text);
                    var res=OoyalaMediaUtils.EditLabel(SecretKey, APIKey, id, hs);
                    if (string.IsNullOrEmpty(res))
                    {
                        lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                        WPHelper.permissionErrorMsg = string.Empty;
                        return;
                    }
                    BindTreviewData();
                    txtlblName.Text = string.Empty;
                    Response.Redirect(SPContext.Current.File.Name);
                }
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void PropertySettings()
        {
            imgAdd.Visible = IsAllowAdd;
            imgSave.Visible = false;
            imgCancel.Visible = false;
            txtlblName.Visible = IsAllowUpdate || IsAllowAdd;
            imgDelete.Visible = IsAllowDelete;
            imgEdit.Visible = IsAllowUpdate;
        }

        private void TreeViewSettings()
        {
            trvView.EnableClientScript = true;
            trvView.PopulateNodesFromClient = true;
            trvView.ShowCheckBoxes = TreeNodeTypes.All;
        }

        private void resetTree()
        {
            while (trvView.CheckedNodes.Count > 0)
            {
                trvView.CheckedNodes[0].Checked = false;
            }
        }
        #endregion        
        
    }
}
