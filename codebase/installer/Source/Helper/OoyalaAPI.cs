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
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Collections;
using System.Text;
using System.Security.Cryptography;


/// <summary>
/// Used to Access V2 API Methods
/// </summary>
public static class OoyalaAPI
{
    private const string BASE_URL = "https://api.ooyala.com/v2/";
    private static HttpWebResponse response;
       
    /// <summary>
    /// Makes an HTTP GET request to the Ooyala API with the specified path and using the given parameters
    /// </summary>
    /// <param name="SecretKey">Ooyala Backlot Secret Key</param>
    /// <param name="APIKey">Ooyala Backlot API Key</param>
    /// <param name="path">
    /// The path to the resource to use for the GET request, i.e "players"
    /// </param>
    /// <param name="parameters">
    /// A Dictionary containing the parameters to be send along with the GET request.     
    /// </param>
    /// <returns>
    /// Returns result as JSON string
    /// </returns>
    public static string getJSON(string SecretKey, string APIKey, string path, Dictionary<string, string> parameters)
    {
        if (parameters == null)
        {
            parameters = new System.Collections.Generic.Dictionary<string, string>();
            string expireValue = DateTime.Now.AddHours(1).Ticks.ToString();
            parameters.Add("api_key", APIKey);
            parameters.Add("expires", expireValue);
        }

        var url = generateURL(SecretKey, APIKey, "GET", path, "", parameters);

        HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
        request.Method = "GET";

        if (getResponse(request))
        {
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        return null;
    }
        
   
    /// <summary>
    /// Takes in the necessary parameters to build a V2 signature for the Ooyala API
    /// </summary>
    /// <param name="SecretKey">Ooyala Backlot Secret Key</param>
    /// <param name="APIKey">Ooyala Backlot API Key</param>
    /// <param name="HTTPMethod">
    /// The method to be used for the request. Possible values are: GET, POST, PUT, PATCH or DELETE
    /// </param>
    /// <param name="path">
    /// The path to use for the request
    /// </param>
    /// <param name="body">
    /// A string containing the JSON representation of the data to be sent on the request. If its a GET request, the body parameter will not be used to generate the signature.
    /// </param>
    /// <param name="parameters">
    /// A hash containing the list of parameters that will be included in the request.
    /// </param>
    /// <returns>
    /// The URL to be used in the HTTP request.
    /// </returns>
    private static string generateURL(string SecretKey, string APIKey, string HTTPMethod, string path, String body, Dictionary<string, string> parameters )
    {
        if (parameters == null)
        {
            parameters = new System.Collections.Generic.Dictionary<string, string>();
            string expireValue = DateTime.Now.AddHours(1).Ticks.ToString();
            parameters.Add("api_key", APIKey);
            parameters.Add("expires", expireValue);
        }

        var url = BASE_URL + path;

        path = "/v2/" + path;

        if (!parameters.ContainsKey("api_key"))
        {
            parameters.Add("api_key", APIKey);
        }

        if (!parameters.ContainsKey("expires"))
        {
            DateTime now = DateTime.UtcNow;
            //Round up to the expiration to the next hour for higher caching performance
            DateTime expiresWindow = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
            int expires = (int)(expiresWindow - new DateTime(1970, 1, 1)).TotalSeconds;
            parameters.Add("expires", expires.ToString());
        }

        //Sorting the keys
        var sortedKeys = new String[parameters.Keys.Count];
        parameters.Keys.CopyTo(sortedKeys, 0);
        Array.Sort(sortedKeys);

        for (int i = 0; i < sortedKeys.Length; i++)
        {
            url += (i == 0 && !url.Contains("?") ? "?" : "&") + sortedKeys[i] + "=" + HttpUtility.UrlEncode(parameters[sortedKeys[i]]);
        }

        url += "&signature=" + generateRequestSignature(SecretKey, HTTPMethod, path, sortedKeys, parameters, body);

        return url;
    }
    
    /// <summary>
    /// Generates the signature for the V2 API request based on the given parameters. <seealso cref="http://api.ooyala.com/v2/docs"/>
    /// </summary>
    /// <param name="SecretKey">Ooyala Backlot Secret Key</param>
    /// <param name="APIKey">Ooyala Backlot API Key</param>
    /// <param name="HTTPMethod">
    /// The method to be used for the request. Possible values are: GET, POST, PUT, PATCH or DELETE
    /// </param>
    /// <param name="path">
    /// The path to use for the request
    /// </param>
    /// <param name="sortedParameterKeys">
    /// A sorted array containing the keys of the parameters hash. This is to improve efficiency and not sort them twice since generateURL already does it.
    /// </param>
    /// <param name="parameters">
    /// A hash containing the list of parameters that will be included in the request.
    /// </param>
    /// <param name="body">
    /// A string containing the JSON representation of the data to be sent on the request. If its a GET request, the body parameter will not be used to generate the signature.
    /// </param>
    /// <returns>
    /// A string containing the signature to be used in the V2 API request.
    /// </returns>
    private static string generateRequestSignature(string SecretKey, string HTTPMethod, String path, String[] sortedParameterKeys, Dictionary<String, String> parameters, String body)
    {
        var stringToSign = SecretKey + HTTPMethod + path;

        for (int i = 0; i < sortedParameterKeys.Length; i++)
        {
            stringToSign += sortedParameterKeys[i] + "=" + parameters[sortedParameterKeys[i]];
        }

        stringToSign += body;

        var sha256 = new SHA256Managed();
        byte[] digest = sha256.ComputeHash(Encoding.Default.GetBytes(stringToSign));
        string signedInput = Convert.ToBase64String(digest);

        var lastEqualsSignindex = signedInput.Length - 1;
        while (signedInput[lastEqualsSignindex] == '=')
        {
            lastEqualsSignindex--;
        }

        signedInput = signedInput.Substring(0, lastEqualsSignindex + 1);

        return HttpUtility.UrlEncode(signedInput.Substring(0, 43));
    }
    
    private static Boolean getResponse(HttpWebRequest request)
    {
        try
        {
            response = request.GetResponse() as HttpWebResponse;
            return true;
        }
        catch (WebException e)
        {
            Console.WriteLine("Exception Message :" + e.Message);
            if (e.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ((HttpWebResponse)e.Response);
                Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);

                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                var text = reader.ReadToEnd();
                Console.WriteLine("Response Description : {0}", text);

            }
            return false;
        }
    }
       
}
