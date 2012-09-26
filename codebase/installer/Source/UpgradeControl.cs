using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SharePointInstaller
{
  public partial class UpgradeControl : InstallerControl
  {
    private readonly InstallProcessControl processControl;

    public UpgradeControl()
    {
      this.processControl = Program.CreateProcessControl();
      InitializeComponent();

      messageLabel.Text = InstallConfiguration.FormatString(messageLabel.Text);

      string upgradeDescription = InstallConfiguration.UpgradeDescription;
      if (upgradeDescription != null)
      {
        upgradeDescriptionLabel.Text = upgradeDescription;
      }
    }

    protected internal override void Open(InstallOptions options)
    {
      bool enable = upgradeRadioButton.Checked || removeRadioButton.Checked;
      Form.Operation = InstallOperation.Upgrade;
      Form.NextButton.Enabled = enable;
    }

    protected internal override void Close(InstallOptions options)
    {
      Form.ContentControls.Add(processControl);
    }

    private void upgradeRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (upgradeRadioButton.Checked)
      {
        Form.Operation = InstallOperation.Upgrade;
        Form.NextButton.Enabled = true;
      }
    }

    private void removeRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (removeRadioButton.Checked)
      {
        Form.Operation = InstallOperation.Uninstall;
        Form.NextButton.Enabled = true;
      }
    }
  }
}
