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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.Data.SqlClient;
using System.Configuration;

namespace SharePointInstaller
{
    public partial class APISettings : InstallerControl
    {
        string SPUserName = string.Empty;
        public APISettings()
        {
            InitializeComponent();
            this.Load += new EventHandler(APISettings_Load);
            SPUserName = System.Environment.MachineName + "\\" + Environment.UserName;
            txtUserID.Text = SPUserName;           
        }

        private void APISettings_Load(object sender, EventArgs e)
        {
            Form.NextButton.Enabled = false;
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(txtUserID.Text) ||  (string.IsNullOrEmpty(txtSecretKey.Text)) || (string.IsNullOrEmpty(txtAPIKey.Text))))
            {
                MessageBox.Show("All fields are mandatory, Please enter the required values","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if(!txtUserID.Text.Equals(SPUserName))
            {
                MessageBox.Show("Please check the Sharepoint user id","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            SaveDBConfiguration();
        }

        #region Private Methods
              
        private void SaveDBConfiguration()
        {
            string PartnerCode = string.Empty;string SecretKey=string.Empty; string APIKey=string.Empty;

            //PartnerCode = txtPartnerCode.Text;
            SecretKey = txtSecretKey.Text;
            APIKey = txtAPIKey.Text;
     
            bool isValid = ValidateAPIInfo(SecretKey, APIKey);
            if (isValid)
            {
                string UserID = txtUserID.Text;

                if (SaveConfiguration(PartnerCode, SecretKey, APIKey, UserID))
                {
                    Form.NextButton.Enabled = true;
                    MessageBox.Show("API Info is saved in DB successfully!");
                }
                else
                    MessageBox.Show("API Info is not able to save in Database!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Error: Please provide valid API details!","Information",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion

        /// <summary>
        /// Save User specific API Info to Database.
        /// </summary>
        /// <param name="isNew">Is New User's API info?</param>
        /// <param name="PartnerCode">Backlot Partner Code</param>
        /// <param name="SecretKey">Backlot Secret Key</param>
        /// <param name="APIKey">Backlot API Key</param>
        /// <param name="UserID">Logged in User</param>
        /// <returns>API Info are properly saved in DB or not. </returns>
        public static bool SaveConfiguration(string PartnerCode, string SecretKey, string APIKey, string UserID)
        {
            string queryString = "CreateOrModifyAPISettings";

            string ConnectionString = OoyalaConnectStringBuilder.BuildConnectionString();

            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(queryString, con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (APIKey.IndexOf(".") > 0)
                PartnerCode = APIKey.Substring(0, APIKey.IndexOf("."));
            cmd.Parameters.AddWithValue("PartnerCode", PartnerCode);
            cmd.Parameters.AddWithValue("SecretKey", SecretKey);
            cmd.Parameters.AddWithValue("APIKey", APIKey);
            cmd.Parameters.AddWithValue("UserID", UserID);
            using (con)
            {
                con.Open();
                return (cmd.ExecuteNonQuery() == 1);
            }
        }

        /// <summary>
        /// Validate API Key and Secret Key by using V2 API
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="APIKey"></param>
        /// <returns>Whether API and Secret Key are valid or not.</returns>
        public static bool ValidateAPIInfo(string secretKey, string APIKey)
        {
            System.Collections.Generic.Dictionary<string, string> parameters = new System.Collections.Generic.Dictionary<string, string>();
            string expireValue = DateTime.Now.AddHours(1).Ticks.ToString();
            parameters.Add("api_key", APIKey);
            parameters.Add("expires", expireValue);
            parameters.Add("limit", "1");
            var res = OoyalaAPI.getJSON(secretKey, APIKey, "players", parameters);
            return (res != null);
        }

        
    }
}
