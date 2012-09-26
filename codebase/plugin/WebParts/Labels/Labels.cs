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

namespace OoyalaPlugin.AssetsThumbnailView.Labels
{
    [ToolboxItemAttribute(false)]
    public class Labels : WebPart
    {
        #region Web Part Properties

        [WebBrowsable(true)]
        [WebDisplayName("Allow Add New Labels")]
        [WebDescription("Allow to add new Label")]
        [Category("Backlot Labels Taxonomy")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_AllowAddNew { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("Allow Edit Labels")]
        [WebDescription("Allow to Edit existing Labels")]
        [Category("Backlot Labels Taxonomy")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_AllowEdit { get; set; }

        [WebBrowsable(true)]
        [WebDisplayName("Allow Delete Labels")]
        [WebDescription("Allow to delete existing Labels")]
        [Category("Backlot Labels Taxonomy")]
        [Personalizable(PersonalizationScope.User)]
        public bool WP_AllowDelete { get; set; }

        #endregion

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/OoyalaPlugin.AssetsThumbnailView/Labels/LabelsUserControl.ascx";

        protected override void CreateChildControls()
        {
            Hashtable hs = new Hashtable();
            int result = LoginEntity.LoadConfiguration(SPContext.Current.Web.CurrentUser.LoginName, ref hs);
            if (result == 1)
            {
                LabelsUserControl control = (LabelsUserControl)Page.LoadControl(_ascxPath);
                control.IsAllowAdd = WP_AllowAddNew;
                control.IsAllowDelete = WP_AllowDelete;
                control.IsAllowUpdate = WP_AllowEdit;
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
