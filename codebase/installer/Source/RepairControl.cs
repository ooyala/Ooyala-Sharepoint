using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SharePointInstaller
{
  public partial class RepairControl : InstallerControl
  {
    private readonly InstallProcessControl processControl;

    public RepairControl()
    {
      this.processControl = Program.CreateProcessControl();
      InitializeComponent();

      messageLabel.Text = InstallConfiguration.FormatString(messageLabel.Text);
    }

    protected internal override void Open(InstallOptions options)
    {
      bool enable = repairRadioButton.Checked || removeRadioButton.Checked;
      Form.Operation = InstallOperation.Repair;
      Form.NextButton.Enabled = enable;
    }

    protected internal override void Close(InstallOptions options)
    {
      Form.ContentControls.Add(processControl);
    }

    private void repairRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (repairRadioButton.Checked)
      {
        Form.Operation = InstallOperation.Repair;
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
