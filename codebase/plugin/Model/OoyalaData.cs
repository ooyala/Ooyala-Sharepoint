using System.Collections.Generic;

namespace OoyalaData
{
    #region Labels

    namespace Labels
    {
        /// <summary>
        /// Used to store Label Structure based on JSON Results
        /// </summary>
        public class Item
        {
            public string name { get; set; }
            public string id { get; set; }
            public string parent_id { get; set; }
            public string full_name { get; set; }
        }

        /// <summary>
        /// Used to store List of Labels 
        /// </summary>
        public class OoyalaLabelDataResult
        {
            public List<Item> items { get; set; }
        }
    }

    #endregion

    #region Media Assets
    namespace Assets
    {
        /// <summary>
        /// Used to store Media Asset Structure based on JSON Results
        /// </summary>
        public class Item
        {
            public string asset_type { get; set; }
            public int duration { get; set; }
            public string name { get; set; }
            public string preview_image_url { get; set; }
            public string created_at { get; set; }
            public string embed_code { get; set; }
            public string youtube_id { get; set; }
            public string updated_at { get; set; }
            public string original_file_name { get; set; }
            public object external_id { get; set; }
            public object hosted_at { get; set; }
            public string description { get; set; }
            public string status { get; set; }
            public string serving_url { get; set; }
        }

        /// <summary>
        /// Used to store List of Media Assets
        /// </summary>
        public class OoyalaAssetDataResult
        {
            public List<Item> items { get; set; }
            public string next_page { get; set; }
        }

    }
    #endregion

    #region Media Players

    namespace Players
    {
        #region Media Player Supporting Classes

        public class Audio
        {
            public bool show_download { get; set; }
        }

        public class OoyalaBranding
        {
            public bool show_info_screen_homepage_link { get; set; }
            public bool twitter_sharing { get; set; }
            public bool show_share_button { get; set; }
            public bool show_ad_countdown { get; set; }
            public bool show_info_screen_title { get; set; }
            public bool url_sharing { get; set; }
            public bool show_info_button { get; set; }
            public bool facebook_sharing { get; set; }
            public bool email_sharing { get; set; }
            public bool digg_sharing { get; set; }
            public bool show_bitrate_button { get; set; }
            public bool show_info_screen_description { get; set; }
            public bool show_embed_button { get; set; }
            public bool show_channel_button { get; set; }
            public bool enable_error_screen { get; set; }
            public string accent_color { get; set; }
            public bool show_end_screen_replay_button { get; set; }
            public bool show_volume_button { get; set; }
        }

        public class RelatedVideos
        {
            public string click_behavior { get; set; }
            public string order { get; set; }
            public string sort { get; set; }
            public object labels { get; set; }
            public string source { get; set; }
        }

        public class Scrubber
        {
            public bool always_show { get; set; }
            public object image_url { get; set; }
        }

        public class Playback
        {
            public bool buffer_on_pause { get; set; }
        }

        public class Watermark
        {
            public object image_url { get; set; }
            public string click_url { get; set; }
            public double alpha { get; set; }
        }
        #endregion

        #region Media Player Settings

        /// <summary>
        /// Used to store Media Player Structure based on JSON Results
        /// </summary>
        public class Item
        {
            public Audio audio { get; set; }
            public string name { get; set; }
            public OoyalaBranding ooyala_branding { get; set; }
            public bool is_default { get; set; }
            public RelatedVideos related_videos { get; set; }
            public string provider_homepage_url { get; set; }
            public Scrubber scrubber { get; set; }
            public string id { get; set; }
            public Playback playback { get; set; }
            public Watermark watermark { get; set; }
            public string default_closed_caption_language { get; set; }
        }

        /// <summary>
        /// Used to store List of Media Players
        /// </summary>
        public class OoyalaPlayerDataResult
        {
            public List<Item> items { get; set; }
        }

        #endregion
    }
    #endregion

}