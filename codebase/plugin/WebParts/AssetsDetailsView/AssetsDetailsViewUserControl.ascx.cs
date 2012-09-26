using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections;
using OoyalaData.Assets;
using OoyalaPlugin.AssetsThumbnailView.Labels;
using System.Data;
using Microsoft.SharePoint;

namespace OoyalaPlugin.AssetsDetailsView
{
    public partial class AssetsDetailsViewUserControl : UserControl
    {
        #region Public Properties

        public bool ShowSearch { get; set; }
        public int PageLimit { get; set; }
        public bool ShowCustomMetadata { get; set; }
        public bool ShowLabels { get; set; }

        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }

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
            btnNext.Attributes.Add("onclick", "return isEmptyCustomMetadata();");
            btnPrev.Attributes.Add("onclick", "return isEmptyCustomMetadata();");

            if (!IsPostBack)
            {
                InitPageSettings();
                GridColumnSettings();
                DetailsViewBindData();
            }
        }

        protected void dtgAssets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAssetType = (Label)e.Row.FindControl("lblAssetType");
                string imgAssetTypePath = string.Empty;
                switch (lblAssetType.Text.ToLower())
                {
                    case "video":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/videoAsset.png";
                        break;
                    case "remote_asset":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/remoteAsset.png";
                        break;
                    case "youtube":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/youtubeAsset.png";
                        break;
                    case "live_stream":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/LiveStreamAsset.png";
                        break;
                    case "channel":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/channelAsset.png";
                        break;
                    case "channel_set":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/channelSetAsset.png";
                        break;
                    case "ad":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/AdAsset.png";
                        break;
                    case "video_ad":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/VideoAdAsset.png";
                        break;
                    case "audio":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/AudioAsset.png";
                        break;
                    case "live_audio":
                        imgAssetTypePath = @"~/_layouts/images/OoyalaPlugin/LiveStreamAsset.png";
                        break;
                }

                System.Web.UI.WebControls.Image imgAssetType = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgAssetType");
                imgAssetType.Height = Unit.Pixel(32);
                imgAssetType.Width = Unit.Pixel(32);
                imgAssetType.ToolTip = lblAssetType.Text.ToUpper();
                imgAssetType.ImageUrl = imgAssetTypePath;
                imgAssetType.AlternateText = lblAssetType.Text.ToUpper();

                Label lblDuration = (Label)e.Row.FindControl("lblDuration");
                string millisecs = lblDuration.Text;
                double msec = Convert.ToDouble(millisecs) / 1000;
                TimeSpan t = TimeSpan.FromSeconds(msec);

                string strDuration = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds
                                        );
                lblDuration.Text = strDuration;

                Label lblUploaded = (Label)e.Row.FindControl("lblUploaded");
                string strUploaded = lblUploaded.Text.Substring(0, 10);
                DateTime dtUploaded = DateTime.ParseExact(strUploaded, "yyyy-MM-dd",
                                  CultureInfo.InvariantCulture);
                lblUploaded.Text = dtUploaded.ToString("ddd MMM dd yyyy");

                Label lblLastUpdated = (Label)e.Row.FindControl("lblUpdated");
                string strLastUpdated = lblLastUpdated.Text.Substring(0, 10);
                DateTime dtLastUpdated = DateTime.ParseExact(strLastUpdated, "yyyy-MM-dd",
                                  CultureInfo.InvariantCulture);
                lblLastUpdated.Text = dtLastUpdated.ToString("ddd MMM dd yyyy");
                
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                lblStatus.Text = lblStatus.Text.ToUpper();

            }
        }
        protected void dtgAssets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblResults.Text = string.Empty;
            dtgAssets.EditIndex = e.NewEditIndex;
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            DetailsViewBindData(pageCount);
            EditMediaAsset(e.NewEditIndex);
        }
        protected void dtgAssets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblResults.Text = string.Empty;
            dtgAssets.EditIndex = -1;
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            DetailsViewBindData(pageCount);
        }
        protected void dtgAssets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblResults.Text = string.Empty;
            SaveMediaAssetSettings(e.RowIndex);
            dtgAssets.EditIndex = -1;
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            DetailsViewBindData(pageCount);
        }
        protected void dtgAssets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblResults.Text = string.Empty;
            string id = (string)dtgAssets.DataKeys[e.RowIndex].Value.ToString();
            DeleteMediaAsset(id);
            dtgAssets.EditIndex = -1;
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            DetailsViewBindData(pageCount);
        }
        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            lblResults.Text = string.Empty;
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            pageCount = pageCount + 1;
            ViewState["PageCount"] = pageCount;
            dtgAssets.EditIndex = -1;
            DetailsViewBindData(pageCount);
        }
        protected void btnPrev_Click(object sender, ImageClickEventArgs e)
        {
            lblResults.Text = string.Empty;
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            if (pageCount > 1)
                pageCount = pageCount - 1;
            ViewState["PageCount"] = pageCount;
            dtgAssets.EditIndex = -1;
            DetailsViewBindData(pageCount);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            SearchSettings();
            dtgAssets.EditIndex = -1;
            DetailsViewBindData();
            lblResults.Text = string.Empty;
        }

        #endregion

        #region Private Methods

        private void DetailsViewBindData(int PageCount)
        {
            if (PageCount == 1)
                btnPrev.Visible = false;
            else
                btnPrev.Visible = true;

            int count = Convert.ToInt32(ViewState["MaxPageCount"]);
            if (PageCount >= count)
                btnNext.Visible = false;
            else
                btnNext.Visible = true;

            DetailsViewBindData();
        }

        private void DetailsViewBindData()
        {
            int pageSize = (PageLimit >= 10) ? PageLimit : 10;

            const string strPageToken = "page_token=";
            string searchText, searchBy, nextpage_url, page_token, metaDataSearch; 

            searchText = txtSearch.Text.Equals("Enter Search Text")? string.Empty: Server.HtmlEncode(WPHelper.escapteChar(txtSearch.Text.Trim()));
            searchBy = ddlSearch.SelectedItem.Value;
            metaDataSearch = txtCustom.Text.Equals("Enter Metadata Key")?string.Empty : Server.HtmlEncode(WPHelper.escapteChar(txtCustom.Text.Trim()));

            //if (searchBy == "metadata")
            //{
            //    searchBy = "metadata." + metaDataSearch;
            //}

            if (searchBy == "metadata")
            {
                if (string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(metaDataSearch))
                {
                    searchBy = "metadata." + metaDataSearch;
                }
                if (string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(metaDataSearch))
                {
                    searchBy = string.Empty; // if both text empty means it is like general search, no need come for where conditions
                }
                if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(metaDataSearch))
                {
                    searchBy = "metadata." + metaDataSearch; 
                }
            }
            

            int currentPage = (int)ViewState["PageCount"];
            lblPageCount.Text = "Page No :" + currentPage.ToString();

            if (ViewState["PageToken" + (currentPage - 1).ToString()] != null)
                page_token = ViewState["PageToken" + (currentPage - 1).ToString()].ToString();
            else
                page_token = "";

            string APIKey, SecretKey;
            try
            {
                APIKey = txtAPIKey.Text;
                SecretKey = txtSecretKey.Text;
                OoyalaAssetDataResult cdata = new OoyalaAssetDataResult();
                cdata = OoyalaMediaUtils.GetLimitedAssets(SecretKey, APIKey, pageSize, searchBy, searchText, page_token, true, true);

                nextpage_url = cdata.next_page;
                if (nextpage_url != null)
                {
                    if (nextpage_url.IndexOf("page_token=") >= 0)
                        page_token = nextpage_url.Substring(nextpage_url.IndexOf(strPageToken) + strPageToken.Length);
                    else
                        page_token = "";
                }
                else
                    page_token = "";

                ViewState["PageToken" + currentPage.ToString()] = page_token;
                ViewState["MaxPageCount"] = currentPage;

                if (!string.IsNullOrEmpty(cdata.next_page))
                    btnNext.Visible = true;
                else
                    btnNext.Visible = false;

                var assetPreviewList = from itm in cdata.items select itm;

                dtgAssets.DataSource = assetPreviewList;
                dtgAssets.DataKeyNames = new string[] { "embed_code" };
                dtgAssets.DataBind();
                if (assetPreviewList.Count() == 0)
                {
                    lblPageCount.Text = string.Empty;
                }
            }
            catch(Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void DeleteMediaAsset(string id)
        {
            string APIKey, SecretKey;
            try
            {
                APIKey = txtAPIKey.Text;
                SecretKey = txtSecretKey.Text;
                var res=OoyalaMediaUtils.DeleteMediaAsset(SecretKey, APIKey, id);
                if (!res)
                {
                    lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                    WPHelper.permissionErrorMsg = string.Empty;
                    return;
                }
                
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }

        }

        private void SaveMediaAssetSettings(int EditRowIndex)
        {
            string id = (string)dtgAssets.DataKeys[EditRowIndex].Value.ToString();
            string APIKey, SecretKey;
            try
            {
                APIKey = txtAPIKey.Text;
                SecretKey = txtSecretKey.Text;

                if (AllowEdit)
                {
                    Hashtable hs = new Hashtable();
                    TextBox txtAssetName = (TextBox)dtgAssets.Rows[EditRowIndex].FindControl("txtAssetName");
                    if (!string.IsNullOrEmpty(txtAssetName.Text))
                        hs.Add("name", txtAssetName.Text);

                    TextBox txtAssetDesc = (TextBox)dtgAssets.Rows[EditRowIndex].FindControl("txtAssetDesc");
                    if (!string.IsNullOrEmpty(txtAssetDesc.Text))
                        hs.Add("description", txtAssetDesc.Text);
                    else
                        hs.Add("description", txtAssetDesc.Text);

                    var res=OoyalaMediaUtils.ApplyMetadataToAssets(SecretKey, APIKey, id, hs);
                    if(string.IsNullOrEmpty(res))
                    {
                        lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                        WPHelper.permissionErrorMsg = string.Empty;
                        return;
                    }

                    if (ShowLabels)
                    {
                        TreeView trvAsset = (TreeView)dtgAssets.Rows[EditRowIndex].FindControl("trvAsset");
                        ArrayList arl = new ArrayList();
                        foreach (TreeNode tn in trvAsset.CheckedNodes)
                        {
                            if (!string.IsNullOrEmpty(tn.Value))
                            {
                                arl.Add(tn.Value);
                            }
                        }
                        OoyalaMediaUtils.DeleteLabelsFromAssets(SecretKey, APIKey, id);
                        OoyalaMediaUtils.ApplyLabelsToAssets(SecretKey, APIKey, id, arl);
                    }

                    if (ShowCustomMetadata)
                    {
                        GridView dtgMetadata = (GridView)dtgAssets.Rows[EditRowIndex].FindControl("dtgMetadata");
                        Hashtable hsMD = new Hashtable();
                        foreach (GridViewRow rw in dtgMetadata.Rows)
                        {
                            TextBox txtCustomMetadataKey = (TextBox)rw.FindControl("txtCustomMetadataKey");
                            TextBox txtCustomMetadataValue = (TextBox)rw.FindControl("txtCustomMetadataValue");
                            if (!(string.IsNullOrEmpty(txtCustomMetadataKey.Text) && string.IsNullOrEmpty(txtCustomMetadataValue.Text)))
                            {
                                if (!hsMD.ContainsKey(txtCustomMetadataKey.Text))
                                {
                                    hsMD.Add(txtCustomMetadataKey.Text, txtCustomMetadataValue.Text);
                                }
                            }
                        }
                        OoyalaMediaUtils.ApplyCustomMetadataToAssets(SecretKey, APIKey, id, hsMD);
                    }
                    
                }
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void AddLevel(ref TreeView trv, ref TreeNode tn, List<OoyalaData.Labels.Item> source, List<OoyalaData.Labels.Item> checkedSource, string parent)
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

                List<string> inList = (from inItm in checkedSource
                                       where inItm.id == nd.Value
                                       select inItm.id).ToList();

                if (inList.Count == 1)
                    nd.Checked = true;
                else
                    nd.Checked = false;

                AddLevel(ref trv, ref nd, source, checkedSource, itm.id);

                if (string.IsNullOrEmpty(parent))
                    trv.Nodes.Add(nd);
                else
                    tn.ChildNodes.Add(nd); 
            }
        }

        private void EditMediaAsset(int NewEditIndex)
        {
            //if (!(ShowLabels && ShowCustomMetadata)) return;

            string APIKey, SecretKey;
            try
            {
                string id = (string)dtgAssets.DataKeys[NewEditIndex].Value;
                APIKey = txtAPIKey.Text;
                SecretKey = txtSecretKey.Text;

                if (ShowLabels)
                {
                    TreeView trvAsset = (TreeView)dtgAssets.Rows[NewEditIndex].FindControl("trvAsset");
                    List<OoyalaData.Labels.Item> allLabels = OoyalaMediaUtils.GetAllLabels(SecretKey, APIKey);
                    List<OoyalaData.Labels.Item> checkedSource = OoyalaMediaUtils.GetAssetLabels(SecretKey, APIKey, id);

                    TreeNode nd = new TreeNode();
                    AddLevel(ref trvAsset, ref nd, allLabels, checkedSource, null);
                    //trvAsset.ExpandAll();
                    trvAsset.CollapseAll();
                }

                if (ShowCustomMetadata)
                {
                    GridView dtgMetadata = (GridView)dtgAssets.Rows[NewEditIndex].FindControl("dtgMetadata");
                    Hashtable hsMD = OoyalaMediaUtils.GetAssetCustomMetadata(SecretKey, APIKey, id);
                    DataTable tbl = CreateMetadataTable(hsMD);
                    dtgMetadata.DataSource = tbl;
                    dtgMetadata.DataBind();
                }
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }

        }

        private DataTable CreateMetadataTable(Hashtable hsMD)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Key");
            tbl.Columns.Add("Value");

            foreach (DictionaryEntry dic in hsMD)
            {
                DataRow rw = tbl.NewRow();
                rw["Key"] = dic.Key;
                rw["Value"] = dic.Value;
                tbl.Rows.Add(rw);
            }
            for (int i = hsMD.Count; i < 5; i++)
            {
                tbl.Rows.Add(tbl.NewRow());
            }
            return tbl;
        }

        private void SearchSettings()
        {
            btnPrev.Visible = false;
            btnNext.Visible = false;

            ViewState["PageCount"] = 1;
            int count = Convert.ToInt32(ViewState["MaxPageCount"]);
            for (int i = 0; i < count; i++)
            {
                ViewState["PageToken" + i.ToString()] = string.Empty;
            }
            ViewState["MaxPageCount"] = "1";
        }

        private void PropertySettings()
        {
            pnlSearch.Visible = ShowSearch;

            if (ddlSearch.SelectedValue.Equals("metadata"))
            {
                this.txtCustom.Attributes["style"]="display:'';";
                //this.txtSearch.Text = "Enter Search Text";

            }
            else
            {
                this.txtCustom.Attributes["style"] = "display:none;";
            }
        }

        private void InitPageSettings()
        {
            btnPrev.Visible = false;
            ViewState["MaxPageCount"] = 1;
            ViewState["PageCount"] = 1;
            dtgAssets.Columns[0].Visible = AllowDelete;
            dtgAssets.Columns[1].Visible = AllowEdit;
        }

        private void GridColumnSettings()
        {
            dtgAssets.Columns[0].ItemStyle.Width = Unit.Percentage(3);  //Delete 
            dtgAssets.Columns[1].ItemStyle.Width = Unit.Percentage(4);  //Edit
            dtgAssets.Columns[2].ItemStyle.Width = Unit.Percentage(20); //Name
            dtgAssets.Columns[3].ItemStyle.Width = Unit.Percentage(25); //Description
            dtgAssets.Columns[4].ItemStyle.Width = Unit.Percentage(5);  //Status
            dtgAssets.Columns[5].ItemStyle.Width = Unit.Percentage(15); //File Name
            dtgAssets.Columns[6].ItemStyle.Width = Unit.Percentage(6);  //Asset Type
            dtgAssets.Columns[7].ItemStyle.Width = Unit.Percentage(5);  //Duration
            dtgAssets.Columns[8].ItemStyle.Width = Unit.Percentage(9);  //Uploaded At
            dtgAssets.Columns[9].ItemStyle.Width = Unit.Percentage(9);  //Updated At
        }

        #endregion
    }
}
