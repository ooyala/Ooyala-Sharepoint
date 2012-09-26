using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using OoyalaData;
using System.ComponentModel;
using Microsoft.SharePoint;
using System.Collections;

namespace OoyalaPlugin.WP_API_Settings.APISettings
{
    public partial class APISettingsUserControl : UserControl
    {
        #region Public Properties
        public string APIErrorMessage
        {
            get { return lblResults.Text; }
            set
            {
                lblResults.Text = value;
            }
        }

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

        public string PartnerCode
        {
            get { return txtPartnerCode.Text; }
            set { txtPartnerCode.Text = value; }
        }

        #endregion

        #region Events
                
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string partnerCode = txtPartnerCode.Text.ToUpper();
            string apiKey = txtAPIKey.Text.ToUpper();
            
            int stPos = apiKey.LastIndexOf(".");

            if (stPos == -1)
            {
                lblResults.Text = "<font color=red>Partner Code and API Key are mismatching!</font>";
                return;
            }
            
            if (partnerCode == apiKey.Substring(0, stPos))
            {
                SaveDBConfiguration();
            }
            else
            {
                lblResults.Text = "<font color=red>Partner Code and API Key are mismatching!</font>";
            }
        }

        protected void btnReset_Click(object sender, ImageClickEventArgs e)
        {
            txtAPIKey.Text = String.Empty;
            txtPartnerCode.Text = String.Empty;
            txtSecretKey.Text = String.Empty;
            lblResults.Text = string.Empty;
        }

        #endregion

        #region Private Methods

        private void SaveDBConfiguration()
        {
            string PartnerCode = txtPartnerCode.Text;
            string SecretKey = txtSecretKey.Text;
            string APIKey = txtAPIKey.Text;

            bool isValid = OoyalaMediaUtils.ValidateAPIInfo(SecretKey, APIKey);
            if (isValid)
            {
                string UserID = SPContext.Current.Web.CurrentUser.LoginName;

                if (LoginEntity.SaveConfiguration(PartnerCode, SecretKey, APIKey, UserID))
                    lblResults.Text = "<font color=green>API Info is saved in DB successfully!</font>";
                else
                    lblResults.Text = "<font color=red>API Info is not able to save in Database<font>";
            }
            else
            {
                lblResults.Text = "<font color=red>Please provide valid API details!</font>";
            }
        }

        #endregion

    }
}
