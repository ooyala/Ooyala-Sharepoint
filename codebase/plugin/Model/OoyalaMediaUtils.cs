using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using Microsoft.SharePoint;

/// <summary>
/// Static Class is used to manipulate Media Players, Labels and Media Assets
/// </summary>
public static class OoyalaMediaUtils
{
    #region Constants

    private const string pathAsset = "assets";
    private const string formatAsset = "assets/{0}";
    private const string formatUploadURLs = "assets/{0}/uploading_urls";
    private const string formatStatus = "assets/{0}/upload_status";
    private const string formatSearch = "{0}='{1}'";
    private const string formatMetadata = "assets/{0}/metadata";
    private const string pathLabel = "labels";
    private const string formatLabels = "assets/{0}/labels";
    private const string pathPlayer = "players";
    private const int UPLOAD_TIMEOUT = 1000 * 3600;

    #endregion

    #region Static Constructor for Attaching Validation Cerficate Event Handler

    static OoyalaMediaUtils()
    {
        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateFbCertificate);
    }

    #endregion

    #region Methods for Lables

    /// <summary>
    /// Get all available Labels in Ooyala by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <returns>List of Labels</returns>
    public static List<OoyalaData.Labels.Item> GetAllLabels(string SecretKey, string APIKey)
    {
        var list = new List<OoyalaData.Labels.Item>();
        OoyalaData.Labels.OoyalaLabelDataResult odr = new OoyalaData.Labels.OoyalaLabelDataResult();
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, pathLabel, null);
        odr = JsonHelper.JsonDeserialize<OoyalaData.Labels.OoyalaLabelDataResult>(json);
        list.AddRange(odr.items);
        return list;
    }

    /// <summary>
    /// Add a New Label by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="hs">Label Settings in Hashtable Format</param>
    public static string AddNewLabel(string SecretKey, string APIKey, Hashtable hs)
    {
        var res = OoyalaAPI.post(SecretKey, APIKey, pathLabel, hs);
        if (res != null)
            return res["name"].ToString();
        else
            return null;
    }

    /// <summary>
    /// Update Label Settings in Ooyala based on given Label ID by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Label ID</param>
    /// <param name="hs">Label Settings in Hashtable Format</param>
    public static string EditLabel(string SecretKey, string APIKey, string id, Hashtable hs)
    {
        var res = OoyalaAPI.patch(SecretKey, APIKey, pathLabel + "/" + id, hs);
        if (res != null)
            return res["name"].ToString();
        else
            return null;
    }

    /// <summary>
    /// Delete available Label based on Lable ID by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Label ID</param>
    public static bool DeleteLabel(string SecretKey, string APIKey, string id)
    {
        var res = OoyalaAPI.delete(SecretKey, APIKey, pathLabel + "/" + id);
        return res;
    }

    #endregion

    #region Methods for Media Players

    /// <summary>
    /// Get All available Media Players from Ooyala by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <returns>List of Media Players</returns>
    public static List<OoyalaData.Players.Item> GetAllPlayers(string SecretKey, string APIKey)
    {
        var list = new List<OoyalaData.Players.Item>();
        OoyalaData.Players.OoyalaPlayerDataResult odr = new OoyalaData.Players.OoyalaPlayerDataResult();
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, pathPlayer, null);
        odr = JsonHelper.JsonDeserialize<OoyalaData.Players.OoyalaPlayerDataResult>(json);
        list.AddRange(odr.items);
        return list;
    }

    /// <summary>
    /// Add a New Media Player with Player Name as NewPlayer using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    public static string AddNewPlayer(string SecretKey, string APIKey)
    {
        Hashtable hs = new Hashtable();
        hs.Add("name", "New Player");
        var res=OoyalaAPI.post(SecretKey, APIKey, pathPlayer, hs);
        if (res != null)
            return res["name"].ToString();
        else
            return null;
    }

    /// <summary>
    /// Update Player Settings in Ooyala using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>    
    /// <param name="player">Media Player Object</param>
    public static string EditPlayer(string SecretKey, string APIKey, OoyalaData.Players.Item player)
    {
        string jsonstr = JsonHelper.JsonSerializer<OoyalaData.Players.Item>(player);
        Hashtable hs = (Hashtable)JSON.JsonDecode(jsonstr);
        var res=OoyalaAPI.patch(SecretKey, APIKey, pathPlayer + "/" + player.id, hs);
        if (res != null)
            return res["name"].ToString();
        else
            return null;
    }

    /// <summary>
    /// Delete Media Player based on Player ID using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Player ID</param>
    public static bool DeletePlayer(string SecretKey, string APIKey, string id)
    {
        var res=OoyalaAPI.delete(SecretKey, APIKey, pathPlayer + "/" + id);
        return res;
    }

    /// <summary>
    /// Get Media Player based on Player ID
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Player ID</param>
    /// <returns></returns>
    public static OoyalaData.Players.Item GetPlayer(string SecretKey, string APIKey, String id)
    {
        OoyalaData.Players.Item itm = new OoyalaData.Players.Item();
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, pathPlayer + "/" + id, null);
        itm = JsonHelper.JsonDeserialize<OoyalaData.Players.Item>(json);
        return itm;
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

    #endregion
    
    #region Methods for Media Asset

    /// <summary>
    /// Get All available Media assets by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <returns>List of Media Assets</returns>
    public static List<OoyalaData.Assets.Item> GetAllAssets(string SecretKey, string APIKey)
    {
        var list = new List<OoyalaData.Assets.Item>();
        OoyalaData.Assets.OoyalaAssetDataResult odr = new OoyalaData.Assets.OoyalaAssetDataResult();
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, pathAsset, null);
        odr = JsonHelper.JsonDeserialize<OoyalaData.Assets.OoyalaAssetDataResult>(json);
        list.AddRange(odr.items);
        return list;
    }

    /// <summary>
    /// Get Media Assets based on Search Creteria and Page Limit by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="limit">No of Media Asset needs to be return</param>
    /// <param name="searchBy">Search By String(e.g., Name, Description, Label, Status)</param>
    /// <param name="searchText">Search Text</param>
    /// <param name="pageToken">Next Page Token, it is used to support Previous, Next Page Results</param>
    /// <returns>List of Media Assets</returns>
    public static OoyalaData.Assets.OoyalaAssetDataResult GetLimitedAssets(string SecretKey, string APIKey, int limit, string searchBy, string searchText, string pageToken, bool IncludeMetadata, bool IncludeLabels)
    {
        System.Collections.Generic.Dictionary<string, string> parameters = new System.Collections.Generic.Dictionary<string, string>();
        string expireValue = DateTime.Now.AddHours(1).Ticks.ToString();
        parameters.Add("api_key", APIKey);
        parameters.Add("expires", expireValue);
        parameters.Add("limit", limit.ToString());

        if (IncludeLabels && IncludeMetadata)
            parameters.Add("include", "metadata,labels");
        else if (IncludeLabels && !IncludeMetadata)
            parameters.Add("include", "metadata");
        else if (!IncludeLabels && IncludeMetadata)
            parameters.Add("include", "labels");

        if (searchBy.Contains("metadata")) //for metadata when searchtext is empty value search
            parameters.Add("where", string.Format(formatSearch, searchBy, searchText));
        else if(!string.IsNullOrEmpty(searchText))
            parameters.Add("where", string.Format(formatSearch, searchBy, searchText));

        if (!string.IsNullOrEmpty(pageToken))
            parameters.Add("page_token", pageToken);

        parameters.Add("orderby", "name");
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, pathAsset, parameters);
        return JsonHelper.JsonDeserialize<OoyalaData.Assets.OoyalaAssetDataResult>(json);
    }

    /// <summary>
    /// Get Media Assets based on Live Status by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <returns>List of Media Assets</returns>
    public static List<OoyalaData.Assets.Item> GetLiveAssets(string SecretKey, string APIKey)
    {
        var list = new List<OoyalaData.Assets.Item>();
        OoyalaData.Assets.OoyalaAssetDataResult odr = new OoyalaData.Assets.OoyalaAssetDataResult();

        System.Collections.Generic.Dictionary<string, string> parameters = new System.Collections.Generic.Dictionary<string, string>();
        string expireValue = DateTime.Now.AddHours(1).Ticks.ToString();
        parameters.Add("api_key", APIKey);
        parameters.Add("expires", expireValue);
        parameters.Add("where", string.Format(formatSearch, "status", "live"));
        parameters.Add("orderby", "name");
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, pathAsset, parameters);

        odr = JsonHelper.JsonDeserialize<OoyalaData.Assets.OoyalaAssetDataResult>(json);
        list.AddRange(odr.items);
        return list;
    }
    
    /// <summary>
    /// Used to create New Media Asset by using V2 API
    /// </summary>
    /// <param name="SecretKey"></param>
    /// <param name="APIKey"></param>
    /// <param name="hs"></param>
    /// <returns></returns>
    public static string NewMediaAsset(string SecretKey, string APIKey, Hashtable hs)
    {
        var res = OoyalaAPI.post(SecretKey, APIKey, pathAsset, hs);
        if (res != null)
            return res["embed_code"].ToString();
        else
            return null;
    }

    /// <summary>
    /// Get Stream of file for uploading
    /// </summary>
    /// <param name="fileURL"></param>
    /// <returns></returns>
    public static Stream GetFileStream(string fileURL)
    {
        using (WebClient client = new WebClient())
        {
            client.UseDefaultCredentials = true;
            return new MemoryStream(client.DownloadData(fileURL));
        }
    }

    /// <summary>
    /// Upload new Media File by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="hs">File's Standard Metadata in Hashtable Format</param>
    /// <param name="file">Upload File Object</param>
    public static void NewMediaUploadStream(string SecretKey, string APIKey, string id, Stream inStream)
    {
        ArrayList resHS = new ArrayList();
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, string.Format(formatUploadURLs, id), null);
        resHS = (ArrayList)JSON.JsonDecode(json);
        foreach (string uploadurl in resHS)
        {
            System.Net.Cache.RequestCachePolicy requestCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            HttpWebRequest request = HttpWebRequest.Create(uploadurl) as HttpWebRequest;
            request.CachePolicy = requestCachePolicy;
            request.Method = "PUT";
            request.AllowWriteStreamBuffering = false;
            request.Timeout = UPLOAD_TIMEOUT;
            request.SendChunked = false;
            request.ContentLength = inStream.Length;
            var outStream = request.GetRequestStream();
            byte[] b = new byte[1024];
            int r;
            while ((r = inStream.Read(b, 0, b.Length)) > 0)
                outStream.Write(b, 0, r);
            outStream.Flush();
            outStream.Close();
        }
    }

    /// <summary>
    /// Set Upload status for Media asset after uploading by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    /// <param name="status">Upload Status</param>
    public static void SetMediaFileStatus(string SecretKey, string APIKey, string id, string status)
    {
        string postStatus = string.Format(formatStatus, id);
        Hashtable hsStatus = new Hashtable();
        hsStatus.Add("status", status);
        OoyalaAPI.put(SecretKey, APIKey, postStatus, hsStatus);
    }

    /// <summary>
    /// Get Custom Metadata of given Asset ID by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    /// <returns>Custom Metadata of given Asset ID</returns>
    public static Hashtable GetAssetCustomMetadata(string SecretKey, string APIKey, String asset_id)
    {
        string postPath = postPath = string.Format(formatMetadata, asset_id);
        return OoyalaAPI.get(SecretKey, APIKey, postPath, null);
    }

    /// <summary>
    /// Get Labels of given Asset ID by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    /// <returns>List of Labels for given Asset ID</returns>
    public static List<OoyalaData.Labels.Item> GetAssetLabels(string SecretKey, string APIKey, String asset_id)
    {
        var list = new List<OoyalaData.Labels.Item>();
        string postPath = postPath = string.Format(formatLabels, asset_id);
        OoyalaData.Labels.OoyalaLabelDataResult odr = new OoyalaData.Labels.OoyalaLabelDataResult();
        string json = OoyalaAPI.getJSON(SecretKey, APIKey, postPath, null);
        odr = JsonHelper.JsonDeserialize<OoyalaData.Labels.OoyalaLabelDataResult>(json);
        list.AddRange(odr.items);
        return list;
    }

    /// <summary>
    /// Apply Standard Metadata to Media asset based on Asset ID by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    /// <param name="hs">Standard Metadata in Hashtable Format</param>
    public static string ApplyMetadataToAssets(string SecretKey, string APIKey, String id, Hashtable hs)
    {
        string postPath = postPath = string.Format(formatAsset, id);
        var res=OoyalaAPI.patch(SecretKey, APIKey, postPath, hs);
        if (res != null)
            return res["name"].ToString();
        else
            return null;
    }

    /// <summary>
    /// Apply Custom Metadata to Media asset based on Asset ID by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    /// <param name="hs">Custom Metadata in Hashtable Format</param>
    public static void ApplyCustomMetadataToAssets(string SecretKey, string APIKey, String id, Hashtable hs)
    {
        string postPath = postPath = string.Format(formatMetadata, id);
        OoyalaAPI.put(SecretKey, APIKey, postPath, hs);
    }

    /// <summary>
    /// Apply Labels to Media Assets by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    /// <param name="labelList">Selected Lables which are to be applied to given media asset</param>
    public static void ApplyLabelsToAssets(string SecretKey, string APIKey, String id, ArrayList labelList)
    {
        string postPath = string.Empty;
        string jsonbody = "[";
        bool isLabelAvail = false;
        foreach (string tnValue in labelList)
        {
            if (!string.IsNullOrEmpty(tnValue))
            {
                if (isLabelAvail)
                    jsonbody += ",\"" + tnValue + "\"";
                else
                    jsonbody += "\"" + tnValue + "\"";

                isLabelAvail = true;
            }
        }
        jsonbody += "]";
        if (isLabelAvail)
        {
            postPath = string.Format(formatLabels, id);
            OoyalaAPI.post(SecretKey, APIKey, postPath, jsonbody);
        }
    }

    /// <summary>
    /// Remove Labels from Media Assets by using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Media Asset ID</param>
    public static void DeleteLabelsFromAssets(string SecretKey, string APIKey, String id)
    {
        OoyalaAPI.delete(SecretKey, APIKey, string.Format(formatLabels, id));
    }

    /// <summary>
    /// Delete Media Asset based on Asset ID using V2 API
    /// </summary>
    /// <param name="SecretKey">Backlot Secret Key</param>
    /// <param name="APIKey">Backlot API Key</param>
    /// <param name="id">Player ID</param>
    public static bool DeleteMediaAsset(string SecretKey, string APIKey, string id)
    {
        var res=OoyalaAPI.delete(SecretKey, APIKey, pathAsset + "/" + id);
        return res;
    }
    #endregion

    #region ValidateCertificate

    private static bool ValidateFbCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
    {
        return true;
    }

    #endregion

    
}
