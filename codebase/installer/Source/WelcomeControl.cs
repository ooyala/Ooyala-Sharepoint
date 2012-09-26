using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SharePointInstaller
{
  public partial class WelcomeControl : InstallerControl
  {
    public WelcomeControl()
    {
      InitializeComponent();

      messageLabel.Text = InstallConfiguration.FormatString(messageLabel.Text);
    }
  }
}
