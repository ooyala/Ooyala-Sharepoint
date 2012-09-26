using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharePointInstaller.Resources;

namespace SharePointInstaller
{
  public partial class CompletionControl : InstallerControl
  {
    public CompletionControl()
    {
      InitializeComponent();

      this.Load += new EventHandler(CompletionControl_Load);
    }

    void CompletionControl_Load(object sender, EventArgs e)
    {
      // Conditionally show the FinishedControl
      if (InstallConfiguration.ShowFinishedControl && Form.Operation == InstallOperation.Install)
      {
        FinishedControl finishedControl = new FinishedControl();
        finishedControl.Title = CommonUIStrings.finishedTitle;
        finishedControl.SubTitle = InstallConfiguration.FormatString(CommonUIStrings.finishedSubTitle);
        Form.ContentControls.Add(finishedControl);

        Form.NextButton.Enabled = true;
      }
    }

    public string Details
    {
      get { return detailsTextBox.Text; }
      set { detailsTextBox.Text = value; }
    }

    protected internal override void Open(InstallOptions options)
    {
      Form.PrevButton.Enabled = false;
      if (InstallConfiguration.ShowFinishedControl && Form.Operation == InstallOperation.Install)
      {
        Form.AbortButton.Enabled = false;
      }
    }
  }
}
