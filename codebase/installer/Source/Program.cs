using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.SharePoint;

namespace SharePointInstaller
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      InstallerForm form = new InstallerForm();
      form.Text = InstallConfiguration.FormatString("{SolutionTitle}");

      form.ContentControls.Add(CreateWelcomeControl());
      form.ContentControls.Add(CreateSystemCheckControl());

      Application.Run(form);
    }

    private static InstallerControl CreateWelcomeControl()
    {
      WelcomeControl control = new WelcomeControl();
      control.Title = InstallConfiguration.FormatString(Resources.CommonUIStrings.controlTitleWelcome);
      control.SubTitle = InstallConfiguration.FormatString(Resources.CommonUIStrings.controlSubTitleWelcome);
      return control;
    }

    private static InstallerControl CreateSystemCheckControl()
    {
      SystemCheckControl control = new SystemCheckControl();
      control.Title = Resources.CommonUIStrings.controlTitleSystemCheck;
      control.SubTitle = InstallConfiguration.FormatString(Resources.CommonUIStrings.controlSubTitleSystemCheck);

      control.RequireMOSS = InstallConfiguration.RequireMoss;
      control.RequireSearchSKU = false;

      return control;
    }

    internal static InstallerControl CreateUpgradeControl()
    {
      UpgradeControl control = new UpgradeControl();
      control.Title = Resources.CommonUIStrings.controlTitleUpgradeRemove;
      control.SubTitle = Resources.CommonUIStrings.controlSubTitleSelectOperation;
      return control;
    }

    internal static InstallerControl CreateRepairControl()
    {
      RepairControl control = new RepairControl();
      control.Title = Resources.CommonUIStrings.controlTitleRepairRemove;
      control.SubTitle = Resources.CommonUIStrings.controlSubTitleSelectOperation;
      return control;
    }

    internal static InstallerControl CreateDeploymentTargetsControl()
    {
      InstallerControl control = null;
      SPFeatureScope featureScope = InstallConfiguration.FeatureScope;
      if (featureScope == SPFeatureScope.Farm)
      {
          control = new DeploymentTargetsControl();
          control.Title = Resources.CommonUIStrings.controlTitleFarmDeployment;
          control.SubTitle = Resources.CommonUIStrings.controlSubTitleFarmDeployment;
      }
      else if (featureScope == SPFeatureScope.Site)
      {
          control = new SiteCollectionDeploymentTargetsControl();
          control.Title = Resources.CommonUIStrings.controlTitleSiteDeployment;
          control.SubTitle = Resources.CommonUIStrings.controlSubTitleSiteDeployment;
      }
      return control;
    }

    internal static InstallProcessControl CreateProcessControl()
    {
      InstallProcessControl control = new InstallProcessControl();
      control.Title = Resources.CommonUIStrings.controlTitleInstalling;
      control.SubTitle = Resources.CommonUIStrings.controlSubTitleInstalling;
      return control;
    }

    internal static InstallerControl CreateSqlServerSettingsControl()
    {
        SQLServerSettings control = new SQLServerSettings();
        control.Title = Resources.CommonUIStrings.controlTitleInstalling;
        control.SubTitle = Resources.CommonUIStrings.controlSubTitleInstalling;
        return control;
    }
    internal static InstallerControl CreateAPISettingsControl()
    {
        APISettings control = new APISettings();
        control.Title = Resources.CommonUIStrings.controlTitleInstalling;
        control.SubTitle = Resources.CommonUIStrings.controlSubTitleInstalling;
        return control;
    }
    public static ApplicationContext AppSettings { get; set; }
  }
}