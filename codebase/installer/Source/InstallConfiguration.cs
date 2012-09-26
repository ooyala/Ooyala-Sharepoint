using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Data.SqlClient;
using SharePointInstaller.Resources;


namespace SharePointInstaller
{
    internal class InstallConfiguration
    {
        #region Constants

        public class BackwardCompatibilityConfigProps
        {
            // "Apllication" mispelled on purpose to match original mispelling released
            public const string RequireDeploymentToCentralAdminWebApllication = "RequireDeploymentToCentralAdminWebApllication";
            // Require="MOSS" = RequireMoss="true" 
            public const string Require = "Require";
            // FarmFeatureId = FeatureId with FeatureScope = Farm
            public const string FarmFeatureId = "FarmFeatureId";
        }

        public class ConfigProps
        {
            public const string BannerImage = "BannerImage";
            public const string LogoImage = "LogoImage";
            public const string RequireMoss = "RequireMoss";
            public const string UpgradeDescription = "UpgradeDescription";
            public const string RequireDeploymentToCentralAdminWebApplication = "RequireDeploymentToCentralAdminWebApplication";
            public const string RequireDeploymentToAllContentWebApplications = "RequireDeploymentToAllContentWebApplications";
            public const string DefaultDeployToSRP = "DefaultDeployToSRP";
            public const string DBScriptFile = "DBScriptFile";
            public const string SolutionId = "SolutionId";
            public const string SolutionFile = "SolutionFile";
            public const string SolutionTitle = "SolutionTitle";
            public const string SolutionVersion = "SolutionVersion";
            public const string FeatureScope = "FeatureScope";
            public const string FeatureId = "FeatureId";
            public const string SiteCollectionRelativeConfigLink = "SiteCollectionRelativeConfigLink";
            public const string SSPRelativeConfigLink = "SSPRelativeConfigLink";
            public const string DocumentationUrl = "DocumentationUrl";
            public const string OoyalaFileType = "OoyalaFileType";
            public const string OoyalaFileUploadLimitInMB = "OoyalaFileUploadLimitInMB";            
        }

        #endregion

        #region Internal Static Properties


        internal static string BannerImage
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.BannerImage]; }
        }

        internal static string LogoImage
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.LogoImage]; }
        }

        internal static bool RequireMoss
        {
            get
            {
                bool rtnValue = false;
                string valueStr = ConfigurationManager.AppSettings[ConfigProps.RequireMoss];
                if (String.IsNullOrEmpty(valueStr))
                {
                    valueStr = ConfigurationManager.AppSettings[BackwardCompatibilityConfigProps.Require];
                    rtnValue = valueStr != null && valueStr.Equals("MOSS", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    rtnValue = Boolean.Parse(valueStr);
                }
                return rtnValue;
            }
        }

        internal static string DBScriptFile
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.DBScriptFile]; }
        }

        internal static Guid SolutionId
        {
            get { return new Guid(ConfigurationManager.AppSettings[ConfigProps.SolutionId]); }
        }

        internal static string SolutionFile
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.SolutionFile]; }
        }

        internal static string SolutionTitle
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.SolutionTitle]; }
        }

        internal static string OoyalaFileType
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.OoyalaFileType]; }
        }

        internal static string OoyalaFileUploadLimitInMB
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.OoyalaFileUploadLimitInMB]; }
        }
                
        internal static Version SolutionVersion
        {
            get { return new Version(ConfigurationManager.AppSettings[ConfigProps.SolutionVersion]); }
        }


        internal static string UpgradeDescription
        {
            get
            {
                string str = ConfigurationManager.AppSettings[ConfigProps.UpgradeDescription];
                if (str != null)
                {
                    str = FormatString(str);
                }
                return str;
            }
        }

        internal static SPFeatureScope FeatureScope
        {
            get
            {
                // Default to farm features as this is what the installer only supported initially
                SPFeatureScope featureScope = SPFeatureScope.Farm;
                string valueStr = ConfigurationManager.AppSettings[ConfigProps.FeatureScope];
                if (!String.IsNullOrEmpty(valueStr))
                {
                    featureScope = (SPFeatureScope)Enum.Parse(typeof(SPFeatureScope), valueStr, true);
                }
                return featureScope;
            }
        }

        // Modif JPI - Début
        internal static List<Guid?> FeatureId
        {
            get
            {
                string valueStr = ConfigurationManager.AppSettings[ConfigProps.FeatureId];

                //
                // Backwards compatibility with old configuration files before site collection features allowed
                //
                if (String.IsNullOrEmpty(valueStr))
                {
                    valueStr = ConfigurationManager.AppSettings[BackwardCompatibilityConfigProps.FarmFeatureId];
                }

                if (!String.IsNullOrEmpty(valueStr))
                {
                    string[] _strGuidArray = valueStr.Split(";".ToCharArray());
                    if (_strGuidArray.Length >= 0)
                    {
                        List<Guid?> _guidArray = new List<Guid?>();
                        foreach (string _strGuid in _strGuidArray)
                        {
                            _guidArray.Add(new Guid(_strGuid));
                        }
                        return _guidArray;
                    }
                }

                return null;
            }
        }
        // Modif JPI - Fin

        internal static bool RequireDeploymentToCentralAdminWebApplication
        {
            get
            {
                string valueStr = ConfigurationManager.AppSettings[ConfigProps.RequireDeploymentToCentralAdminWebApplication];

                //
                // Backwards compatability with old configuration files containing spelling error in the 
                // application setting key (Bug 990).
                //
                if (String.IsNullOrEmpty(valueStr))
                {
                    valueStr = ConfigurationManager.AppSettings[BackwardCompatibilityConfigProps.RequireDeploymentToCentralAdminWebApllication];
                }

                if (!String.IsNullOrEmpty(valueStr))
                {
                    return valueStr.Equals("true", StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }
        }

        internal static bool RequireDeploymentToAllContentWebApplications
        {
            get
            {
                string valueStr = ConfigurationManager.AppSettings[ConfigProps.RequireDeploymentToAllContentWebApplications];
                if (!String.IsNullOrEmpty(valueStr))
                {
                    return valueStr.Equals("true", StringComparison.OrdinalIgnoreCase);
                }
                return false;
            }
        }

        internal static bool DefaultDeployToSRP
        {
            get
            {
                string valueStr = ConfigurationManager.AppSettings[ConfigProps.DefaultDeployToSRP];
                if (!String.IsNullOrEmpty(valueStr))
                {
                    return valueStr.Equals("true", StringComparison.OrdinalIgnoreCase);
                }
                return false;
            }
        }

        internal static Version InstalledVersion
        {
            get
            {
                try
                {
                    SPFarm farm = SPFarm.Local;
                    string key = "Solution_" + SolutionId.ToString() + "_Version";
                    return farm.Properties[key] as Version;
                }

                catch (NullReferenceException ex)
                {
                    throw new InstallException(CommonUIStrings.installExceptionDatabase, ex);
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }

            set
            {
                try
                {
                    SPFarm farm = SPFarm.Local;
                    string key = "Solution_" + SolutionId.ToString() + "_Version";
                    farm.Properties[key] = value;
                    farm.Update();
                }

                catch (NullReferenceException ex)
                {
                    throw new InstallException(CommonUIStrings.installExceptionDatabase, ex);
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        public static bool ShowFinishedControl
        {
            get
            {
                return !String.IsNullOrEmpty(ConfigurationManager.AppSettings[ConfigProps.SiteCollectionRelativeConfigLink]) ||
                  !String.IsNullOrEmpty(ConfigurationManager.AppSettings[ConfigProps.SSPRelativeConfigLink]);
            }
        }

        public static string SiteCollectionRelativeConfigLink
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.SiteCollectionRelativeConfigLink]; }
        }

        public static string SSPRelativeConfigLink
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.SSPRelativeConfigLink]; }
        }

        public static string DocumentationUrl
        {
            get { return ConfigurationManager.AppSettings[ConfigProps.DocumentationUrl]; }
        }

        #endregion

        #region Internal Static Methods

        internal static string FormatString(string str)
        {
            return FormatString(str, null);
        }

        internal static string FormatString(string str, params object[] args)
        {
            string formattedStr = str;
            string solutionTitle = SolutionTitle;
            if (!String.IsNullOrEmpty(solutionTitle))
            {
                formattedStr = formattedStr.Replace("{SolutionTitle}", solutionTitle);
            }
            if (args != null)
            {
                formattedStr = String.Format(formattedStr, args);
            }
            return formattedStr;
        }

        #endregion
    }

    public class OoyalaConnectStringBuilder
    {
        public static string DatabaseName { get; set; }
        public static string DBServer { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static bool IntegratedSecurity { get; set; }

        public static string BuildConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = DBServer;
            builder.InitialCatalog = DatabaseName;
            if (!IntegratedSecurity)
            {
                builder.UserID = User;
                builder.Password = Password;
            }
            else
            {
                builder.IntegratedSecurity = IntegratedSecurity;
            }
            return builder.ConnectionString;
        }
    }

    public enum InstallOperation
    {
        Install,
        Upgrade,
        Repair,
        Uninstall
    }
}
