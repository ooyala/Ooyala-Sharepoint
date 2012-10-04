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
using System.Text;
using System.Linq;

namespace OoyalaPlugin.WebParts.MediaPlayer.Player
{
    public partial class PlayerUserControl : UserControl
    {
        #region Public Properties
        
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

        public string ErrorMessage
        {
            get { return lblResults.Text; }
            set
            {
                lblResults.Text = value;
            }
        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMedia();
            }
        }

        #endregion      

        #region Private Methods

        void LoadMedia()
        {
            try
            {
                var assets = OoyalaMediaUtils.GetLiveAssets(SecretKey, APIKey);
                var players = OoyalaMediaUtils.GetAllPlayers(SecretKey, APIKey);
                if (assets == null)
                {
                    ErrorMessage = "<font color=red>No Media Assets found!</font>";
                    return;
                }
                if (players == null)
                {
                    ErrorMessage = "<font color=red>No Media players found!</font>";
                    return;
                }
                ddlMediaAssets.DataSource = assets;
                ddlMediaAssets.DataTextField = "name";
                ddlMediaAssets.DataValueField = "embed_code";
                ddlMediaAssets.DataBind();

                var orderedPlayers = from p in players orderby p.name select p;
                ddlMediaPlayers.DataSource = orderedPlayers;
                ddlMediaPlayers.DataTextField = "name";
                ddlMediaPlayers.DataValueField = "id";
                ddlMediaPlayers.DataBind();
            }
            catch(Exception e)
            {
                ErrorMessage = e.Message;
            }
        }
                

        #endregion      

        protected void btnPlay_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMediaPlayers.SelectedValue) && !string.IsNullOrEmpty(ddlMediaPlayers.SelectedValue))
            {
                StringBuilder scriptTagBuilder = new StringBuilder();

                scriptTagBuilder.Append("<!DOCTYPE html><html><head> <script src=\"http://player.ooyala.com/v3/" + ddlMediaPlayers.SelectedValue + "\"></script></head>");
                scriptTagBuilder.Append("<div id=\"PlayerSection\" style='width:650px;height:380px;'></div>");
                scriptTagBuilder.Append("<script>");
                scriptTagBuilder.Append("var videoPlayer = OO.Player.create('PlayerSection','" + ddlMediaAssets.SelectedValue + "');");
                scriptTagBuilder.Append("</script></html>");
                
                litPlayer.Text = scriptTagBuilder.ToString();
            }
            else
            {
                ErrorMessage = "<font color=red>Assest and Player should not be empty!</font>";
            }
        }
    }
}
