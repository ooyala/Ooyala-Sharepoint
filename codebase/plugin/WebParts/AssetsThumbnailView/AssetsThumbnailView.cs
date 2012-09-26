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
