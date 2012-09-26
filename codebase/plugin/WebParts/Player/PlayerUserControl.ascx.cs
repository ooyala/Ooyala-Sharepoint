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
