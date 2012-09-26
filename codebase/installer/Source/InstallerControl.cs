using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace SharePointInstaller
{
  public class InstallerControl : UserControl
  {
    private string title;
    private string subTitle;

    protected InstallerControl()
    {
    }

    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    public string SubTitle
    {
      get { return subTitle; }
      set { subTitle = value; }
    }

    protected InstallerForm Form
    {
      get
      {
        return (InstallerForm)this.ParentForm;
      }
    }

    protected internal virtual void Open(InstallOptions options) {}

    protected internal virtual void Close(InstallOptions options) {}

    protected internal virtual void RequestCancel() 
    {
      Application.Exit();
    }

      private void InitializeComponent()
      {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerControl));
            this.SuspendLayout();
            // 
            // InstallerControl
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "InstallerControl";
            this.Load += new System.EventHandler(this.InstallerControl_Load);
            this.ResumeLayout(false);

      }

      private void InstallerControl_Load(object sender, EventArgs e)
      {

      }

  }

  public class InstallerControlList : List<InstallerControl>
  {
  }
}
