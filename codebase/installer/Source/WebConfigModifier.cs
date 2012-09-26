using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using System.Collections.ObjectModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Configuration;
using System.Web.Configuration;
using System.Xml;
namespace SharePointInstaller
{
    class WebConfigModifier
    {
        public static void EnsureChildNode(SPWebApplication webApp)
        {
            string connectionStringKey = "OoyalaConnectionString";
            string connectionStringvalue = OoyalaConnectStringBuilder.BuildConnectionString();

            string fileTypeKey = "OoyalaFileType";
            string fileTypeValue = InstallConfiguration.OoyalaFileType;

            string fileUploadLimitKey = "OoyalaFileUploadLimitInMB";
            string fileUploadLimitValue = InstallConfiguration.OoyalaFileUploadLimitInMB;
            
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/", webApp.Name);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(config.FilePath);

            xmlDoc = AddOrModifyAppSettings(xmlDoc, connectionStringKey, connectionStringvalue);
            xmlDoc = AddOrModifyAppSettings(xmlDoc, fileTypeKey, fileTypeValue);
            xmlDoc = AddOrModifyAppSettings(xmlDoc, fileUploadLimitKey, fileUploadLimitValue);

            xmlDoc.Save(config.FilePath);
        }

        static XmlDocument AddOrModifyAppSettings(XmlDocument xmlDoc, string key, string value)
        {
            bool isNew = false;

            XmlNodeList list = xmlDoc.DocumentElement.SelectNodes(string.Format("appSettings/add[@key='{0}']", key));
            XmlNode node;
            isNew = list.Count == 0;
            if (isNew)
            {
                node = xmlDoc.CreateNode(XmlNodeType.Element, "add", null);
                XmlAttribute attribute = xmlDoc.CreateAttribute("key");
                attribute.Value = key;
                node.Attributes.Append(attribute);

                attribute = xmlDoc.CreateAttribute("value");
                attribute.Value = value;
                node.Attributes.Append(attribute);

                xmlDoc.DocumentElement.SelectNodes("appSettings")[0].AppendChild(node);
            }            
            else
            {
                node = list[0];
                node.Attributes["value"].Value = value;
            }
            return xmlDoc;
        }

        public static string getConnectionString(SPWebApplication webApp)
        {
            string conString; string connectionStringKey = "OoyalaConnectionString";
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/",webApp.Name);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(config.FilePath);

            XmlNodeList list = xmlDoc.DocumentElement.SelectNodes(string.Format("appSettings/add[@key='{0}']", connectionStringKey));
            XmlNode node;

            node = list[0];
            conString=node.Attributes["value"].Value;

            return conString;
        }
    }
}
