﻿using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using OoyalaPlugin.WP_API_Settings.APISettings;
using System.Collections;

namespace OoyalaPlugin.AssetsDetailsView
{
    [ToolboxItemAttribute(false)]
    public class AssetsDetailsView : WebPart
    {
        #region Web Part Properties

        [WebBrowsable(true)]
        [WebDisplayName("Show Search")]
        [WebDescription("Enable Search Feature")]
        [Category("Media Assets - Details View")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_ShowSearch { get; set; }

        private int wp_pagesize;
        [WebBrowsable(true)]
        [WebDisplayName("Page Limit")]
        [WebDescription("No of Assets per Page. Page Limit should be greater than 0")]
        [Category("Media Assets - Details View")]
        [Personalizable(PersonalizationScope.User)]
        public int WP_PageSize
        {
            get
            {
                return wp_pagesize;
            }
            set
            {
                if (!((value >= 0) && (value <= 1000)))
                {
                    throw new Microsoft.SharePoint.WebPartPages.WebPartPageUserException("Page size should not be negative and greater than 1000");
                }
                wp_pagesize = value;
            }
        }

        [WebBrowsable(true)]
        [WebDisplayName("Show Custom Metadata")]
        [WebDescription("Show Custom Metadata")]
        [Category("Media Assets - Details View")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_ShowCustomMetadata { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("Show Labels")]
        [WebDescription("Show Labels")]
        [Category("Media Assets - Details View")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_ShowLabels { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("Allow Edit ")]
        [WebDescription("Allow to Edit")]
        [Category("Media Assets - Details View")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_AllowEdit { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("Allow Delete")]
        [WebDescription("Allow to delete")]
        [Category("Media Assets - Details View")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_AllowDelete { get; set; }
        #endregion

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/OoyalaPlugin/AssetsDetailsView/AssetsDetailsViewUserControl.ascx";

        protected override void CreateChildControls()
        {
            Hashtable hs = new Hashtable();
            int result = LoginEntity.LoadConfiguration(SPContext.Current.Web.CurrentUser.LoginName, ref hs);
            if (result == 1)
            {
                AssetsDetailsViewUserControl control = (AssetsDetailsViewUserControl)Page.LoadControl(_ascxPath);
                control.PageLimit = WP_PageSize;
                control.ShowSearch = WP_ShowSearch;
                control.ShowCustomMetadata = WP_ShowCustomMetadata;
                control.ShowLabels = WP_ShowLabels;
                control.AllowDelete = WP_AllowDelete;
                control.AllowEdit = WP_AllowEdit;
                if (hs != null)
                {
                    if(hs.Contains("APIKey"))
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

    class AssetDetailEditorPart : EditorPart
    {
        public override bool ApplyChanges()
        {
            return true;
        }
        public override void SyncChanges()
        {
            EnsureChildControls();

            string _ascxPathAPI = @"~/_CONTROLTEMPLATES/OoyalaPlugin/AssetsDetailsView/AssetsDetailsViewUserControl.ascx";
            APISettingsUserControl control = (APISettingsUserControl)Page.LoadControl(_ascxPathAPI);
            if (control != null)
            {
               
                
            }
        }
    }

}
    


