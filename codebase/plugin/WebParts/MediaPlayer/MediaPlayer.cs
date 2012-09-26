using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;
using System.Text;
using OoyalaPlugin.WP_API_Settings.APISettings;

namespace OoyalaPlugin.MediaPlayer
{
    [ToolboxItemAttribute(false)]
    public class MediaPlayer : WebPart, IWebEditable
    {
        #region Web Part Properties

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public string EmbedCode { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public string PlayerID { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public bool AlwaysShowScrubber { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public bool Autoplay { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public bool Replay { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public string APIKey { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public string SecretKey { get; set; }
        #endregion

        public override WebPartExportMode ExportMode
        {
            get { return WebPartExportMode.None; }
            set { }
        }

        EditorPartCollection IWebEditable.CreateEditorParts()
        {
            List<EditorPart> editors = new List<EditorPart>();
            MediaPlayerEditorPart editorPart = new MediaPlayerEditorPart();
            editorPart.ID = this.ID + "_editorPart";
            Hashtable hs = new Hashtable();
            int result = LoginEntity.LoadConfiguration(SPContext.Current.Web.CurrentUser.LoginName, ref hs);
            if (result == 1)
            {
                if (hs != null)
                {
                    if (hs.Contains("APIKey"))
                    {
                        editorPart.APIKey = hs["APIKey"].ToString();
                    }
                    if (hs.Contains("SecretKey"))
                    {
                        editorPart.SecretKey = hs["SecretKey"].ToString();
                    }
                }
                editors.Add(editorPart);
                return new EditorPartCollection(editors);
            }
            else
            {
                return null;
            }
        }

        object IWebEditable.WebBrowsableObject
        {
            get { return this; }
        }

        protected override void CreateChildControls()
        {

            Hashtable hs = new Hashtable();
            int result = LoginEntity.LoadConfiguration(SPContext.Current.Web.CurrentUser.LoginName, ref hs);
            if (result != 1)
            {
                string _ascxPathAPI = @"~/_CONTROLTEMPLATES/OoyalaPlugin.WP_API_Settings/APISettings/APISettingsUserControl.ascx";
                APISettingsUserControl control = (APISettingsUserControl)Page.LoadControl(_ascxPathAPI);
                if (hs != null)
                {
                    if (hs.Contains("APIKey"))
                    {
                        APIKey = hs["APIKey"].ToString();
                        control.APIKey = APIKey;
                    }
                    if (hs.Contains("SecretKey"))
                    {
                        SecretKey = hs["SecretKey"].ToString();
                        control.SecretKey = SecretKey;
                    }
                    if (hs.Contains("PartnerCode"))
                        control.PartnerCode = hs["PartnerCode"].ToString();
                }
                control.APIErrorMessage = "API Settings are not configured properly";
                Controls.Add(control);
                return;
            }

            if (!string.IsNullOrEmpty(this.EmbedCode))
            {
                StringBuilder scriptTagBuilder = new StringBuilder();
                scriptTagBuilder.Append("<script src=\"https://player.ooyala.com/player.js?embedCode=");
                scriptTagBuilder.Append(this.EmbedCode);
                scriptTagBuilder.AppendFormat("&alwaysShowScrubber={0}", (this.AlwaysShowScrubber ? "true" : "false"));
                scriptTagBuilder.AppendFormat("&autoplay={0}", (this.Autoplay ? 1 : 0));
                scriptTagBuilder.AppendFormat("&loop={0}", (this.Replay ? 1 : 0));
                if (!string.IsNullOrEmpty(this.PlayerID))
                {
                    scriptTagBuilder.AppendFormat("&playerId={0}", this.PlayerID);
                }
                scriptTagBuilder.Append("&width=675\"></script>");
                this.Controls.Add(new LiteralControl(scriptTagBuilder.ToString()));
            }
        }
    }

    class MediaPlayerEditorPart : EditorPart
    {
        protected TextBox mEmbedCode;
        protected DropDownList mAssetSelectionList, mPlayerList;
        protected Panel errorMessage;
        protected Panel mainPanel;
        protected CheckBox mAlwaysShowScrubber;
        protected CheckBox mAutoplay;
        protected CheckBox mReplay;

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public string APIKey { get; set; }

        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.User)]
        public string SecretKey { get; set; }

        protected override void CreateChildControls()
        {
            mainPanel = new Panel();
            errorMessage = new Panel();
            mainPanel.Controls.Add(errorMessage);

            mAssetSelectionList = new DropDownList() { ID = "AssetSelectionList" };
            mEmbedCode = new TextBox() { ID = "embedCode" };
            mPlayerList = new DropDownList() { ID = "PlayerSelectionList" };
            mAlwaysShowScrubber = new CheckBox() { ID = "AlwaysShowScrubber" };
            mAutoplay = new CheckBox() { ID = "autoplay" };
            mReplay = new CheckBox() { ID = "replay" };

            mainPanel.Controls.Add(new Label() { Text = "Select a Video :<br />", AssociatedControlID = mAssetSelectionList.ID });
            mainPanel.Controls.Add(mAssetSelectionList);

            mainPanel.Controls.Add(new Label() { Text = "Or enter the embed code:<br />", AssociatedControlID = mEmbedCode.ID });
            mainPanel.Controls.Add(mEmbedCode);

            mainPanel.Controls.Add(new Label() { Text = "<br/>Select a Player:<br />", AssociatedControlID = mPlayerList.ID });
            mainPanel.Controls.Add(mPlayerList);
            mainPanel.Controls.Add(new LiteralControl("<br/><br/>Player Options:<br />"));
            mainPanel.Controls.Add(new LiteralControl("<br />"));
            mainPanel.Controls.Add(mAlwaysShowScrubber);
            mainPanel.Controls.Add(new Label() { Text = "Always show scrubber bar?", AssociatedControlID = mAlwaysShowScrubber.ID });
            mainPanel.Controls.Add(new LiteralControl("<br />"));
            mainPanel.Controls.Add(mAutoplay);
            mainPanel.Controls.Add(new Label() { Text = "Enable autoplay?", AssociatedControlID = mAutoplay.ID });
            mainPanel.Controls.Add(new LiteralControl("<br />"));
            mainPanel.Controls.Add(mReplay);
            mainPanel.Controls.Add(new Label() { Text = "Enable auto replay?", AssociatedControlID = mReplay.ID });
            mainPanel.Controls.Add(new LiteralControl("<br /><br />"));
            populateAssetSelection();
            this.Title = "Media Player Settings";
            this.Controls.Add(mainPanel);
        }

        public override bool ApplyChanges()
        {
            EnsureChildControls();
            MediaPlayer webPart = this.WebPartToEdit as MediaPlayer;
            if (webPart != null)
            {
                if (string.IsNullOrEmpty(mEmbedCode.Text) || (mEmbedCode.Text == mAssetSelectionList.SelectedValue))
                {
                    webPart.EmbedCode = mAssetSelectionList.SelectedValue;
                }
                else
                {
                    webPart.EmbedCode = mEmbedCode.Text;
                }
                webPart.PlayerID = mPlayerList.SelectedValue;
                webPart.AlwaysShowScrubber = mAlwaysShowScrubber.Checked;
                webPart.Autoplay = mAutoplay.Checked;
                webPart.Replay = mReplay.Checked;
            }
            return true;
        }

        public override void SyncChanges()
        {
            EnsureChildControls();

            MediaPlayer webPart = this.WebPartToEdit as MediaPlayer;
            if (webPart != null)
            {
                mAssetSelectionList.SelectedValue = webPart.EmbedCode;
                mEmbedCode.Text = string.Empty;
                mPlayerList.SelectedValue = webPart.PlayerID;
                mAlwaysShowScrubber.Checked = webPart.AlwaysShowScrubber;
                mAutoplay.Checked = webPart.Autoplay;
                mReplay.Checked = webPart.Replay;
            }
        }

        private void populateAssetSelection()
        {
            try
            {
                var assets = OoyalaMediaUtils.GetAllAssets(SecretKey, APIKey);
                var players = OoyalaMediaUtils.GetAllPlayers(SecretKey, APIKey);
                if (assets == null)
                {
                    mAssetSelectionList.Enabled = false;
                    errorMessage.Controls.Add(new Label() { Text = "No Assets returned. Is the plug-in installed with the correct API Key and Secret?" });
                    return;
                }
                if (players == null)
                {
                    mPlayerList.Enabled = false;
                    errorMessage.Controls.Add(new Label() { Text = "No Media Players returned. Is the plug-in installed with the correct API Key and Secret?" });
                    return;
                }
                mAssetSelectionList.DataSource = assets;
                mAssetSelectionList.DataTextField = "name";
                mAssetSelectionList.DataValueField = "embed_code";
                mAssetSelectionList.DataBind();

                mPlayerList.DataSource = players;
                mPlayerList.DataTextField = "name";
                mPlayerList.DataValueField = "id";
                mPlayerList.DataBind();
            }
            catch
            {
                errorMessage.Controls.Add(new Label() { Text = "Error has occured. Is the plug-in installed with the correct API Key and Secret?" });
            }
        }
    }

}
