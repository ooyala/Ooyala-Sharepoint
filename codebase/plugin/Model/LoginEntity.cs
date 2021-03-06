﻿/*
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
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using OoyalaData;
using System.Configuration;
using System.Web;
using System.IO;

/// <summary>
/// Used to Load/Save/Validate API Configuration in DB by using V2 API
/// </summary>
public static class LoginEntity
{
    #region Constants

    const int DBRetriveError = -1, ValidationFailed = 0, ValidationSuccess = 1;

    #endregion

    #region API Validation/DB Transaction Methods

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

        string ConnectionString = ConfigurationManager.AppSettings["OoyalaConnectionString"];

        SqlConnection con = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand(queryString, con);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

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
    /// Get API Key and Secret Key info from Database based on Loggedin User
    /// </summary>
    /// <param name="UserID">Logged in User ID</param>
    /// <returns>API Validation Result</returns>
    public static int LoadConfiguration(string UserID, ref Hashtable hs)
    {
        //TextWriter tw = new StreamWriter(@"E:\logfile.txt",);
        string ConnectionString = ConfigurationManager.AppSettings["OoyalaConnectionString"];
        string queryString = "Select PartnerCode,SecretKey,APIKey From tblUser Where UserID=@UserID";
        SqlConnection con = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand(queryString, con);
        cmd.Parameters.AddWithValue("UserID", UserID);
        //tw.WriteLine("User ID : " + UserID);
        try
        {
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            //tw.WriteLine("Hash table count: " + hs.Count);
            if (rdr.Read())
            {
                //tw.WriteLine("Reader : " + rdr.Read());
                hs.Add("PartnerCode", rdr["PartnerCode"].ToString());
                hs.Add("SecretKey", rdr["SecretKey"].ToString());
                hs.Add("APIKey", rdr["APIKey"].ToString());

                if (OoyalaMediaUtils.ValidateAPIInfo(rdr["SecretKey"].ToString(), rdr["APIKey"].ToString()))
                    return ValidationSuccess;
                else
                    return ValidationFailed;
            }
            else
            {
                //tw.WriteLine("else : " );
                return DBRetriveError;
            }
        }
        catch (Exception e)
        {
            //tw.WriteLine("Exception Message " + e.Message);
            //tw.Close();
            //tw.Dispose();

            throw e;

        }
    }
   
    #endregion
}

