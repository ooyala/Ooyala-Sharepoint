using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web.Script.Serialization;
using Microsoft.SharePoint;

namespace OoyalaPlugin.MediaPlayerSettings
{
    public partial class MediaPlayerSettingsUserControl : UserControl
    {
        #region Properties

        public bool IsAllowAdd { get; set; }
        public bool IsAllowDelete { get; set; }
        public bool IsAllowUpdate { get; set; }

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

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PropertySettings();

            if (!IsPostBack)
            {
                BindData();
            }
            lblResults.Text = string.Empty;

        }

        protected void playerGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && playerGrid.EditIndex == -1)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkDefault");
                if (chk.Checked == true)
                {
                    System.Web.UI.WebControls.Image deleteImage = (Image)e.Row.FindControl("imgdelete");
                    deleteImage.Visible= false;
                    //deleteImage.Enabled = false;
                }
            }
        }

        protected void playerGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CheckBox chk;
            ImageButton imgBtn;
            EditMediaPlayer(e.NewEditIndex);
            if (playerGrid.EditIndex > 0)
            {
                foreach (GridViewRow row in playerGrid.Rows)
                {
                    chk = (CheckBox)(row.Cells[0].Controls[1]);
                    if (chk.Checked == true)
                    {
                        imgBtn = (ImageButton)(row.Cells[4].Controls[1]) != null ? (ImageButton)(row.Cells[4].Controls[1]) : null;
                        if (imgBtn != null)
                        {
                            imgBtn.Visible = false;
                        }
                        return;
                    }
                }
            }
        }

        protected void playerGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            playerGrid.EditIndex = -1;
            BindData();
        }

        protected void playerGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            SaveMediaPlayerSettings(e.RowIndex);
            playerGrid.EditIndex = -1;
            BindData();
        }
        protected void playerGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = (string)playerGrid.DataKeys[e.RowIndex].Value.ToString();
            playerGrid.EditIndex = -1;
            DeleteMediaPlayer(id);
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            playerGrid.EditIndex = -1;
            AddNewMediaPlayer();
            
        }

        #endregion

        #region Private Methods

        private void BindData()
        {
            try
            {
                List<OoyalaData.Players.Item> players = OoyalaMediaUtils.GetAllPlayers(SecretKey, APIKey);

                var playerlist = from itm in players
                                 orderby itm.is_default descending, itm.name ascending
                                 //orderby itm.name ascending
                                 select new { itm.id, itm.name, itm.is_default };
                playerGrid.DataSource = playerlist;
                playerGrid.DataKeyNames = new string[] { "id" };
                playerGrid.DataBind();

            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private OoyalaData.Players.Item PlayerSettingsToHashTable(string playerId, int EditRowIndex)
        {
            try
            {
                CheckBox chkShowShareDigg = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareDigg");
                CheckBox chkShowShareEmailToFriend = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareEmailToFriend");
                CheckBox chkShowShareFacebook = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareFacebook");
                CheckBox chkAdCountdown = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkAdCountdown");
                CheckBox chkShowBitrateButton = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowBitrateButton");
                CheckBox chkShowChannelButton = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowChannelButton");
                CheckBox chkShowShareGrabEmbedCode = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareGrabEmbedCode");
                CheckBox chkReplay = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkReplay");
                CheckBox chkShowInfoButton = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowInfoButton");
                CheckBox chkShowInfoExposeDescription = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowInfoExposeDescription");
                CheckBox chkShowInfoExposeProvider = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowInfoExposeProvider");
                CheckBox chkShowInfoExposeTitle = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowInfoExposeTitle");
                CheckBox chkShowShareButton = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareButton");
                CheckBox chkShowVolumeButton = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowVolumeButton");
                CheckBox chkShowShareToTwitter = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareToTwitter");
                CheckBox chkShowShareLinkURL = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkShowShareLinkURL");
                CheckBox chkAlwaysShowScrubber = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkAlwaysShowScrubber");
                CheckBox chkBufferOnPause = (CheckBox)playerGrid.Rows[EditRowIndex].FindControl("chkBufferOnPause");
                TextBox txtplayerName = (TextBox)playerGrid.Rows[EditRowIndex].FindControl("txtplayerName");

                OoyalaData.Players.Item player = OoyalaMediaUtils.GetPlayer(SecretKey, APIKey, playerId);
                player.name = txtplayerName.Text;

                player.ooyala_branding.digg_sharing = chkShowShareDigg.Checked;
                player.ooyala_branding.email_sharing = chkShowShareEmailToFriend.Checked;
                player.ooyala_branding.facebook_sharing = chkShowShareFacebook.Checked;
                player.ooyala_branding.show_ad_countdown = chkAdCountdown.Checked;
                player.ooyala_branding.show_bitrate_button = chkShowBitrateButton.Checked;
                player.ooyala_branding.show_channel_button = chkShowChannelButton.Checked;
                player.ooyala_branding.show_embed_button = chkShowShareGrabEmbedCode.Checked;
                player.ooyala_branding.show_end_screen_replay_button = chkReplay.Checked;
                player.ooyala_branding.show_info_button = chkShowInfoButton.Checked;
                player.ooyala_branding.show_info_screen_description = chkShowInfoExposeDescription.Checked;
                player.ooyala_branding.show_info_screen_homepage_link = chkShowInfoExposeProvider.Checked;
                player.ooyala_branding.show_info_screen_title = chkShowInfoExposeTitle.Checked;
                player.ooyala_branding.show_share_button = chkShowShareButton.Checked;
                player.ooyala_branding.show_volume_button = chkShowVolumeButton.Checked;
                player.ooyala_branding.twitter_sharing = chkShowShareToTwitter.Checked;
                player.ooyala_branding.url_sharing = chkShowShareLinkURL.Checked;
                player.scrubber.always_show = chkAlwaysShowScrubber.Checked;
                player.playback.buffer_on_pause = chkBufferOnPause.Checked;

                return player;
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
                return null;
            }
        }

        private void LoadSelectedPlayerSettings(OoyalaData.Players.Item player, int NewEditIndex)
        {
            CheckBox chkShowShareDigg = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareDigg");
            CheckBox chkShowShareEmailToFriend = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareEmailToFriend");
            CheckBox chkShowShareFacebook = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareFacebook");
            CheckBox chkAdCountdown = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkAdCountdown");
            CheckBox chkShowBitrateButton = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowBitrateButton");
            CheckBox chkShowChannelButton = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowChannelButton");
            CheckBox chkShowShareGrabEmbedCode = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareGrabEmbedCode");
            CheckBox chkReplay = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkReplay");
            CheckBox chkShowInfoButton = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowInfoButton");
            CheckBox chkShowInfoExposeDescription = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowInfoExposeDescription");
            CheckBox chkShowInfoExposeProvider = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowInfoExposeProvider");
            CheckBox chkShowInfoExposeTitle = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowInfoExposeTitle");
            CheckBox chkShowShareButton = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareButton");
            CheckBox chkShowVolumeButton = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowVolumeButton");
            CheckBox chkShowShareToTwitter = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareToTwitter");
            CheckBox chkShowShareLinkURL = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkShowShareLinkURL");
            CheckBox chkAlwaysShowScrubber = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkAlwaysShowScrubber");
            CheckBox chkBufferOnPause = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkBufferOnPause");
            TextBox txtplayerName = (TextBox)playerGrid.Rows[NewEditIndex].FindControl("txtplayerName");

            CheckBox chkPlayerGrid = (CheckBox)playerGrid.Rows[NewEditIndex].FindControl("chkDefault");
            if (chkPlayerGrid.Checked)
            {
                txtplayerName.Enabled = false;
            }

            txtplayerName.Text = player.name;

            if (player.ooyala_branding != null)
            {
                chkShowShareDigg.Checked = player.ooyala_branding.digg_sharing;
                chkShowShareEmailToFriend.Checked = player.ooyala_branding.email_sharing;
                chkShowShareFacebook.Checked = player.ooyala_branding.facebook_sharing;
                chkAdCountdown.Checked = player.ooyala_branding.show_ad_countdown;
                chkShowBitrateButton.Checked = player.ooyala_branding.show_bitrate_button;
                chkShowChannelButton.Checked = player.ooyala_branding.show_channel_button;
                chkShowShareGrabEmbedCode.Checked = player.ooyala_branding.show_embed_button;
                chkReplay.Checked = player.ooyala_branding.show_end_screen_replay_button;
                chkShowInfoButton.Checked = player.ooyala_branding.show_info_button;
                chkShowInfoExposeDescription.Checked = player.ooyala_branding.show_info_screen_description;
                chkShowInfoExposeProvider.Checked = player.ooyala_branding.show_info_screen_homepage_link;
                chkShowInfoExposeTitle.Checked = player.ooyala_branding.show_info_screen_title;
                chkShowShareButton.Checked = player.ooyala_branding.show_share_button;
                chkShowVolumeButton.Checked = player.ooyala_branding.show_volume_button;
                chkShowShareToTwitter.Checked = player.ooyala_branding.twitter_sharing;
                chkShowShareLinkURL.Checked = player.ooyala_branding.url_sharing;
            }

            if (player.scrubber != null)
            {
                chkAlwaysShowScrubber.Checked = player.scrubber.always_show;
            }

            if (player.playback != null)
            {
                chkBufferOnPause.Checked = player.playback.buffer_on_pause;
            }
        }

        private void DeleteMediaPlayer(string id)
        {
            try
            {
                var res=OoyalaMediaUtils.DeletePlayer(SecretKey, APIKey, id);
                if (!res)
                {
                    lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                    WPHelper.permissionErrorMsg = string.Empty;
                    return;
                }
                BindData();
                Response.Redirect(SPContext.Current.File.Name);
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void SaveMediaPlayerSettings(int EditRowIndex)
        {
            string id = (string)playerGrid.DataKeys[EditRowIndex].Value.ToString();
            try
            {
                OoyalaData.Players.Item player = PlayerSettingsToHashTable(id, EditRowIndex);
                var res=OoyalaMediaUtils.EditPlayer(SecretKey, APIKey, player);
                if (string.IsNullOrEmpty(res))
                {
                    lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                    WPHelper.permissionErrorMsg = string.Empty;
                    return;
                }

                playerGrid.EditIndex = -1;
                BindData();
                Response.Redirect(SPContext.Current.File.Name);
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void EditMediaPlayer(int NewEditIndex)
        {
            try
            {
                string id = (string)playerGrid.DataKeys[NewEditIndex].Value;
                playerGrid.EditIndex = NewEditIndex;
                BindData();
                OoyalaData.Players.Item itm = OoyalaMediaUtils.GetPlayer(SecretKey, APIKey, id);
                LoadSelectedPlayerSettings(itm, NewEditIndex);
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void AddNewMediaPlayer()
        {
            if (!IsAllowAdd) return;
            try
            {
                var res=OoyalaMediaUtils.AddNewPlayer(SecretKey, APIKey);
                if (string.IsNullOrEmpty(res))
                {
                    lblResults.Text = "<font color=red> " + WPHelper.permissionErrorMsg + " </font>";
                    WPHelper.permissionErrorMsg = string.Empty;
                    return;
                }
                BindData();
                Response.Redirect(SPContext.Current.File.Name);
            }
            catch (Exception e)
            {
                lblResults.Text = e.Message;
            }
        }

        private void PropertySettings()
        {
            btnAdd.Visible = IsAllowAdd;
            playerGrid.Columns[0].Visible = false;
            playerGrid.Columns[3].Visible = IsAllowUpdate;
            playerGrid.Columns[4].Visible = IsAllowDelete;
            lblAdd.Visible = IsAllowAdd;
            btnAdd.Visible = IsAllowAdd;

        }

        #endregion

    }
}
