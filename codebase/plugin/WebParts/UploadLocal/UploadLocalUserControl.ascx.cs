using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Net;
using System.Data;
using OoyalaPlugin.AssetsThumbnailView.Labels;
using Microsoft.SharePoint;

namespace OoyalaPlugin.UploadLocal
{
    public partial class UploadLocalUserControl : UserControl
    {
        #region Constants
        double UPLOAD_LIMIT = 1024 * 1024;
        #endregion

        #region Properties

        public bool ShowCustomMetadata { get; set; }
        public bool ShowLabels { get; set; }

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

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PropertySettings();

            if (!IsPostBack)
            {
                //fuAsset.Attributes.Add("onchange", "return checkFileExtension(this);");
                BindMetadataGrid();
                BindTreviewData();
            }
            lblResults.Text = string.Empty;
        }

        //protected void UploadButton_Click(object sender, ImageClickEventArgs e)
        //{
        //    UPLOAD_LIMIT = UPLOAD_LIMIT * WPHelper.fileSize();

        //    if (fuAsset.HasFile)
        //    {
        //        if (fuAsset.PostedFile.ContentLength > UPLOAD_LIMIT)
        //        {
        //            lblResults.Text = "<font color=red>The file could not be uploaded. File size should not be greater than " + Convert.ToString(WPHelper.fileSize()) + " MB</font>";
        //            ResetFields();
        //            return;
        //        }
        //        SaveMediaAssetSettings();
        //    }
        //    else
        //    {
        //        lblResults.Text = "<font color=red>Please select a Media Asset file.</font>";
        //    }
        //}

        //protected void ResetButton_Click(object sender, ImageClickEventArgs e)
        //{
        //    ResetFields();
        //}

        #endregion

        #region Private Methods

        private void BindMetadataGrid()
        {
            DataTable tbl = CreateMetadataTable();
            dtgMetadata.DataSource = tbl;
            dtgMetadata.DataBind();
        }

        private void BindTreviewData()
        {
            try
            {
                List<OoyalaData.Labels.Item> labels = OoyalaMediaUtils.GetAllLabels(SecretKey, APIKey);
                trvAsset.Nodes.Clear();
                TreeNode nd = new TreeNode();
                //trvAsset.Nodes.Add(nd);
                AddLevel(ref nd, labels, null);
                trvAsset.CollapseAll();
                //trvAsset.ExpandAll();
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void AddLevel(ref TreeNode tn, List<OoyalaData.Labels.Item> source, string parent)
        {
            var list = from itm in source
                       where itm.parent_id == parent
                       select itm;

            foreach (OoyalaData.Labels.Item itm in list)
            {
                TreeNode nd = new TreeNode();
                nd.Text = itm.name;
                nd.Value = itm.id;
                nd.SelectAction = TreeNodeSelectAction.None;
                AddLevel(ref nd, source, itm.id);
                if (string.IsNullOrEmpty(parent))
                    trvAsset.Nodes.Add(nd);
                else
                    tn.ChildNodes.Add(nd);  
            }
        }

        private DataTable CreateMetadataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Key");
            tbl.Columns.Add("Value");
            for (int i = 0; i < 5; i++)
            {
                tbl.Rows.Add(tbl.NewRow());
            }
            return tbl;
        }

        private void SaveMediaAssetSettings()
        {
            try
            {
                string filename = Path.GetFileName(fuAsset.FileName);
                Hashtable hs = new Hashtable();

                if (!string.IsNullOrEmpty(txtTitle.Text))
                    hs.Add("name", txtTitle.Text);
                else
                    hs.Add("name", filename);

                hs.Add("file_name", filename);                                

                string astType = Path.GetExtension(filename).ToUpper();
                bool isValidType = WPHelper.fileType(astType);

                //switch (astType)
                //{
                //    case ".AVI":
                //    case ".MPG":
                //    case ".MOV":
                //    case ".RM":
                //    case ".MP4":
                //    case ".WMV":
                //    case ".WM":
                //        hs.Add("asset_type", "video");
                //        break;
                //    case ".WAV":
                //    case ".AIF":
                //    case ".MP3":
                //    case ".MID":
                //    case ".RA":
                //    case ".AAC":
                //        hs.Add("asset_type", "audio");
                //        break;
                //    default:
                //        isValidType = true;
                //        break;
                //}

                if (!isValidType)
                {
                    lblResults.Text = "<font color=red>File Type is not supported!</font>";
                    ResetFields();
                    return;
                }
                else
                {
                    hs.Add("asset_type", "video");
                }

                hs.Add("file_size", fuAsset.PostedFile.ContentLength.ToString());

                if (!string.IsNullOrEmpty(txtDesc.Text))
                    hs.Add("description", txtDesc.Text);

                if (!string.IsNullOrEmpty(ddlPostUploadStatus.SelectedValue))
                    hs.Add("post_processing_status", ddlPostUploadStatus.SelectedValue);

                string id = OoyalaMediaUtils.NewMediaAsset(SecretKey, APIKey, hs);

                if (string.IsNullOrEmpty(id))
                {
                    lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                    WPHelper.permissionErrorMsg = string.Empty;
                    ResetFields();
                    return;
                }
                
                //if (string.IsNullOrEmpty(id))
                //{
                //    lblResults.Text = "<font color=red>Not able to create New Media Asset</font>";
                //    ResetFields();
                //    return;
                //}

                OoyalaMediaUtils.NewMediaUploadStream(SecretKey, APIKey, id, fuAsset.PostedFile.InputStream);

                if (ShowLabels)
                {
                    ArrayList arl = new ArrayList();
                    bool isAvailable =false;
                    foreach (TreeNode tn in trvAsset.CheckedNodes)
                    {
                        if (!string.IsNullOrEmpty(tn.Value))
                        {
                            arl.Add(tn.Value);
                            isAvailable = true;
                        }
                    }
                    if (isAvailable)
                    {
                        OoyalaMediaUtils.ApplyLabelsToAssets(SecretKey, APIKey, id, arl);
                    }
                }

                if (ShowCustomMetadata)
                {
                    Hashtable hsMD = new Hashtable();
                    bool isAvailable = false;
                    foreach (GridViewRow rw in dtgMetadata.Rows)
                    {
                        TextBox txtCustomMetadataKey = (TextBox)rw.FindControl("txtCustomMetadataKey");
                        TextBox txtCustomMetadataValue = (TextBox)rw.FindControl("txtCustomMetadataValue");
                        if (!(string.IsNullOrEmpty(txtCustomMetadataKey.Text) && string.IsNullOrEmpty(txtCustomMetadataValue.Text)))
                        {
                            isAvailable = true;
                            if (!hsMD.ContainsKey(txtCustomMetadataKey.Text))
                            {                                
                                hsMD.Add(txtCustomMetadataKey.Text, txtCustomMetadataValue.Text);
                            }
                        }
                    }
                    if (isAvailable)
                    {
                        OoyalaMediaUtils.ApplyCustomMetadataToAssets(SecretKey, APIKey, id, hsMD);
                    }
                }

                OoyalaMediaUtils.SetMediaFileStatus(SecretKey, APIKey, id, "uploaded");
                lblResults.Text = "<font color=green>File Successfully Uploaded!</font>";
                ResetFields();

            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void PropertySettings()
        {
            lblCustomMetadata.Visible = ShowCustomMetadata;
            dtgMetadata.Visible = ShowCustomMetadata;

            lblLabels.Visible = ShowLabels;
            trvAsset.Visible = ShowLabels;
            lblPanel.Visible = ShowLabels;
        }

        private void ResetFields()
        {
            txtTitle.Text = string.Empty;
            txtDesc.Text = string.Empty;

            this.ddlPostUploadStatus.SelectedIndex = 0;

            while (trvAsset.CheckedNodes.Count > 0)
            {
                trvAsset.CheckedNodes[0].Checked = false;
            }

            foreach (GridViewRow rw in dtgMetadata.Rows)
            {
                TextBox txtCustomMetadataKey = (TextBox)rw.FindControl("txtCustomMetadataKey");
                TextBox txtCustomMetadataValue = (TextBox)rw.FindControl("txtCustomMetadataValue");
                txtCustomMetadataKey.Text = string.Empty;
                txtCustomMetadataValue.Text = string.Empty;
            }
        }

        #endregion

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            UPLOAD_LIMIT = UPLOAD_LIMIT * WPHelper.fileSize();

            if (fuAsset.HasFile)
            {
                if (fuAsset.PostedFile.ContentLength > UPLOAD_LIMIT)
                {
                    lblResults.Text = "<font color=red>The file could not be uploaded. File size should not be greater than " + Convert.ToString(WPHelper.fileSize()) + " MB</font>";
                    ResetFields();
                    return;
                }
                SaveMediaAssetSettings();
            }
            else
            {
                lblResults.Text = "<font color=red>Please select a Media Asset file.</font>";
            }
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            ResetFields();
        }
    }
}
