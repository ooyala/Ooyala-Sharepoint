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
using System.Text;
using System.Configuration;

    public static class WPHelper
    {
        public static string permissionErrorMsg
        { get; set; }
        
        public static string escapteChar(string searchText)
        {
          
        //p = p.Replace("  ", string.Empty);
        //p = p.Replace(Environment.NewLine, string.Empty);
        //p = p.Replace("\\t", string.Empty);
        //p = p.Replace(" {", "{");
        //p = p.Replace(" :", ":");
        //p = p.Replace(": ", ":");
        //p = p.Replace(", ", ",");
        //p = p.Replace("; ", ";");
        //p = p.Replace(";}", "}");
        //p = p.Replace("""", "");
        //p = p.Replace(", ", ",");
        //p = p.Replace("; ", ";");
        //p = p.Replace(";}", "}");
            searchText = searchText.Replace("\"", "");
            searchText = searchText.Replace("\\", "");
            searchText = searchText.Replace("'", "");
            return searchText;
   
        }

        public static bool fileType(string fileExtn)
        {
            bool isValid;
            string[] fileTypes = new string[] {};
            string fileType=ConfigurationManager.AppSettings["OoyalaFileType"];
            fileTypes = fileType.Split(',');
            if (fileTypes.Contains(fileExtn.Replace(".","")))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        public static double fileSize()
        {   
            string fileSize=ConfigurationManager.AppSettings["OoyalaFileUploadLimitInMB"];
            double dblFileSize=Convert.ToDouble(fileSize);
            return dblFileSize;
        }

    }

