using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Microsoft.SharePoint.Administration;


namespace SharePointInstaller
{
  public partial class DeploymentTargetsControl : InstallerControl
  {
    public DeploymentTargetsControl()
    {
      InitializeComponent();

      webApplicationsCheckedListBox.ItemCheck += new ItemCheckEventHandler(webApplicationsCheckedListBox_ItemCheck);

      this.Load += new EventHandler(Control_Load);
    }

    private void Control_Load(object sender, EventArgs e)
    {
      ConfigureWebApplicationList();
    }

    private void webApplicationsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      WebApplicationInfo info = (WebApplicationInfo)webApplicationsCheckedListBox.Items[e.Index];
      if (info.Required)
      {
        e.NewValue = CheckState.Indeterminate;
      }
    }

    protected internal override void Close(InstallOptions options)
    {
      foreach (WebApplicationInfo info in webApplicationsCheckedListBox.CheckedItems)
      {
        options.WebApplicationTargets.Add(info.Application);
      }
    }

    private void webApplicationsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool enable = webApplicationsCheckedListBox.CheckedItems.Count > 0;
      Form.NextButton.Enabled = enable;
    }

    private void ConfigureWebApplicationList()
    {
      bool defaultDeployToSRP = InstallConfiguration.DefaultDeployToSRP;

      bool required = InstallConfiguration.RequireDeploymentToCentralAdminWebApplication;
      CheckState defaultCheckState = defaultDeployToSRP ? CheckState.Unchecked : CheckState.Checked;
      Collection<SPWebApplication> applications = new Collection<SPWebApplication>();
      foreach (SPWebApplication application in SPWebService.AdministrationService.WebApplications)
      {
        WebApplicationInfo webAppInfo = new WebApplicationInfo(application, required);
        if (required)
        {
          webApplicationsCheckedListBox.Items.Add(webAppInfo, CheckState.Indeterminate);
        }
        else
        {
          webApplicationsCheckedListBox.Items.Add(webAppInfo, defaultCheckState);
        }
      }

      required = InstallConfiguration.RequireDeploymentToAllContentWebApplications;
      foreach (SPWebApplication application in SPWebService.ContentService.WebApplications)
      {
        WebApplicationInfo webAppInfo = new WebApplicationInfo(application, required);
        if (required)
        {
          webApplicationsCheckedListBox.Items.Add(webAppInfo, CheckState.Indeterminate);
        }
        else
        {
          if (defaultDeployToSRP && !webAppInfo.IsSRP)
          {
            defaultCheckState = CheckState.Unchecked;
          }
          else
          {
            defaultCheckState = CheckState.Checked;
          }
          webApplicationsCheckedListBox.Items.Add(webAppInfo, defaultCheckState);
        }
      }
    }

    private class WebApplicationInfo
    {
      private readonly SPWebApplication application;
      private readonly bool required;

      internal WebApplicationInfo(SPWebApplication application, bool required)
      {
        this.application = application;
        this.required = required;
      }

      internal SPWebApplication Application
      {
        get { return application; }
      }

      public bool Required
      {
        get { return required; }
      }

      public bool IsSRP
      {
        get { return application.Properties.ContainsKey("Microsoft.Office.Server.SharedResourceProvider"); }
      }

      public override string ToString()
      {
        string str = application.GetResponseUri(SPUrlZone.Default).ToString();

        if (application.IsAdministrationWebApplication)
        {
          str += "     (Central Administration)";
        } else if (IsSRP)
        {
          str += "     (Shared Resource Provider)";
        } else if (!String.IsNullOrEmpty(application.DisplayName))
        {
          str += "     (" + application.DisplayName + ")";
        } else if (!String.IsNullOrEmpty(application.Name))
        {
          str += "     (" + application.Name + ")";
        }

        return str;
      }
    }
  }
}
