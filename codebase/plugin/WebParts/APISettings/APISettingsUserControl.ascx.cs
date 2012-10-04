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
