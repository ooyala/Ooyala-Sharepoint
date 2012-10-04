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
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using OoyalaPlugin.WP_API_Settings.APISettings;
using System.Collections;


namespace OoyalaPlugin.AssetsThumbnailView
{
    [ToolboxItemAttribute(false)]
    public class AssetsThumbnailView : WebPart
    {
        #region Web Part Properties

        [WebBrowsable(true)]
        [WebDisplayName("Show Search")]
        [WebDescription("Enable Search Feature")]
        [Category("Media Assets - Thumbnail View")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_ShowSearch { get; set; }

        private int wp_columsize;
        [WebBrowsable(true)]
        [WebDisplayName("No of Columns/Page ")]
        [WebDescription("No of Columns per Page. Columns should be greater than 0")]
        [Category("Media Assets - Thumbnail View")]
        [Personalizable(PersonalizationScope.User)]
        public int WP_ColumnSize 
        {
            get
            {
                return wp_columsize;
            }
            set
            {
                if (!((value >= 0) && (value <= 1000)))
                {
                    throw new Microsoft.SharePoint.WebPartPages.WebPartPageUserException("Column size should not be negative and greater than 1000");
                }
                wp_columsize = value;
            }
        }

        private int wp_rowsize;
        [WebBrowsable(true)]
        [WebDisplayName("No of Rows")]
        [WebDescription("No of Rows per Page. Rows should be greater than 0")]
        [Category("Media Assets - Thumbnail View")]
        [Personalizable(PersonalizationScope.User)]
        public int WP_RowSize 
        { 
            get 
            {
                return wp_rowsize;
            }  
            set
            {
                if (!((value >= 0) && (value <= 1000)))
                {
                    throw new Microsoft.SharePoint.WebPartPages.WebPartPageUserException("Row size should not be negative and greater than 1000");
                }
                wp_rowsize = value;
            }
        }

        #endregion

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/OoyalaPlugin/AssetsThumbnailView/AssetsThumbnailViewUserControl.ascx";

        protected override void CreateChildControls()
        {
            Hashtable hs = new Hashtable();
            int result = LoginEntity.LoadConfiguration(SPContext.Current.Web.CurrentUser.LoginName, ref hs);
            if (result == 1)
            {
                AssetsThumbnailViewUserControl control = (AssetsThumbnailViewUserControl)Page.LoadControl(_ascxPath);
                control.PageColumns = WP_ColumnSize;
                control.PageRows = WP_RowSize;
                control.ShowSearch = WP_ShowSearch;
                if (hs != null)
                {
                    if (hs.Contains("APIKey"))
                        control.APIKey = hs["APIKey"].ToString();
                    if (hs.Contains("SecretKey"))
                        control.SecretKey = hs["SecretKey"].ToString();
                    }
                Controls.Add(control);
            }
            else
            {
                string _ascxPathAPI = @"~/_CONTROLTEMPLATES/OoyalaPlugin.WP_API_Settings/APISettings/APISettingsUserControl.ascx";
                APISettingsUserControl control = (APISettingsUserControl)Page.LoadControl(_ascxPathAPI);
                if (hs != null)
                {
                    if (hs.Contains("APIKey"))
                        control.APIKey = hs["APIKey"].ToString();
                    if (hs.Contains("SecretKey"))
                        control.SecretKey = hs["SecretKey"].ToString();
                    if (hs.Contains("PartnerCode"))
                        control.PartnerCode = hs["PartnerCode"].ToString();
                }
                control.APIErrorMessage = "API Settings are not configured properly";
                Controls.Add(control);
                return;
            }
        }
    }
}
