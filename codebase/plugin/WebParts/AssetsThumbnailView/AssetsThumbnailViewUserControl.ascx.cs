/*
Copyright (c) 2012, Ooyala, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following
conditions are met:
     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer 
        in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using OoyalaData.Assets;
using Microsoft.SharePoint;

namespace OoyalaPlugin.AssetsThumbnailView
{
    public partial class AssetsThumbnailViewUserControl : UserControl
    {
        #region Public Properties

        public bool ShowSearch { get; set; }
        public int PageColumns { get; set; }
        public int PageRows { get; set; }

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
                ThumbnailViewBindData();
            }
        }
     
        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            pageCount = pageCount + 1;
            ViewState["PageCount"] = pageCount;
            ThumbnailViewBindData(pageCount);
        }

        protected void btnPrev_Click(object sender, ImageClickEventArgs e)
        {
            int pageCount = Convert.ToInt16(ViewState["PageCount"]);
            if (pageCount > 1)
                pageCount = pageCount - 1;
            ViewState["PageCount"] = pageCount;
            ThumbnailViewBindData(pageCount);
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblResults.Text = string.Empty;
            lblDataListMessage.Text = string.Empty;
            SearchSettings();
            ThumbnailViewBindData();
        }

        protected void dtlAsssets_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litAsset = (Literal)e.Item.FindControl("litAsset");
                string asset = litAsset.Text;
                if (asset.Length >= 25)
                    litAsset.Text = asset.Substring(0, 25) + "...";
            }
        }

        #endregion 

        #region Private Methods

        private void ThumbnailViewBindData(int PageCount)
        {
            if (PageCount == 1)
                btnPrev.Visible = false;
            else
                btnPrev.Visible = true;

            int count = (int)ViewState["MaxPageCount"];
            if (PageCount >= count)
                btnNext.Visible = false;
            else
                btnNext.Visible = true;

            ThumbnailViewBindData();
        }
        
        private void ThumbnailViewBindData()
        {
            int pageSize = PageColumns * PageRows;
            const string strPageToken = "page_token=";
            string searchText, searchBy, nextpage_url, page_token, metaDataSearch;
            searchText = txtSearch.Text;
            searchBy = ddlSearch.SelectedItem.Value;

            searchText = txtSearch.Text.Equals("Enter Search Text") ? string.Empty : Server.HtmlEncode(WPHelper.escapteChar(txtSearch.Text.Trim()));
            searchBy = ddlSearch.SelectedItem.Value;
            metaDataSearch = txtCustom.Text.Equals("Enter Metadata Key") ? string.Empty : Server.HtmlEncode(WPHelper.escapteChar(txtCustom.Text.Trim()));

            if (searchBy == "metadata")
            {
                if (string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(metaDataSearch))
                {
                    searchBy = "metadata." + metaDataSearch;
                }
                if(string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(metaDataSearch))
                {
                    searchBy = string.Empty; // if both text empty means it is like general search
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

            try
            {
                OoyalaAssetDataResult cdata = new OoyalaAssetDataResult();
                cdata = OoyalaMediaUtils.GetLimitedAssets(SecretKey, APIKey, pageSize, searchBy, searchText, page_token,false,false);

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

                var assetPreviewList = from itm in cdata.items
                                       select new
                                       {
                                           itm.name,
                                           itm.preview_image_url
                                       };
                dtlAsssets.DataSource = assetPreviewList;
                dtlAsssets.RepeatColumns = PageColumns;
                dtlAsssets.DataBind();

                if (assetPreviewList.Count() == 0)
                {
                    lblPageCount.Text = string.Empty;
                    tblMessage.Visible = true;
                    lblDataListMessage.Text = "No Data Found!";
                }
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
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
            tblMessage.Visible = false;
            PageColumns = (PageColumns >= 1) ? PageColumns : 8;
            PageRows = (PageRows >= 1) ? PageRows : 2;

            if (ddlSearch.SelectedValue.Equals("metadata"))
            {
                this.txtCustom.Attributes["style"] = "display:'';";
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
        }

        #endregion
    }
}
