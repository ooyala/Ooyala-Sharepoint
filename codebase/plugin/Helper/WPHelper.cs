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

