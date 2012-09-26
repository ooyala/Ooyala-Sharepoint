using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Collections;

namespace OoyalaPlugin.WP_API_Settings.APISettings
{
    [ToolboxItemAttribute(false)]
    public class APISettings : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/OoyalaPlugin.WP_API_Settings/APISettings/APISettingsUserControl.ascx";

        protected override void CreateChildControls()
        {
            Hashtable hs = new Hashtable();
            int result = LoginEntity.LoadConfiguration(SPContext.Current.Web.CurrentUser.LoginName, ref hs);
            APISettingsUserControl control = (APISettingsUserControl)Page.LoadControl(_ascxPath);
            
            if (result == 1)
                control.APIErrorMessage = string.Empty;
            else
                control.APIErrorMessage = "API Settings are not configured properly";

            if (hs != null)
            {
                if (hs.Contains("APIKey"))
                    control.APIKey = hs["APIKey"].ToString();
                if (hs.Contains("SecretKey"))
                    control.SecretKey = hs["SecretKey"].ToString();
                if (hs.Contains("PartnerCode"))
                    control.PartnerCode = hs["PartnerCode"].ToString();
            }            

            Controls.Add(control);
        }        
    }
   
}
