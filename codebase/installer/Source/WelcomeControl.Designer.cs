namespace SharePointInstaller
{
  partial class WelcomeControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeControl));
            this.messageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            resources.ApplyResources(this.messageLabel, "messageLabel");
            this.messageLabel.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.messageLabel.Name = "messageLabel";
            // 
            // WelcomeControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.messageLabel);
            this.Name = "WelcomeControl";
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label messageLabel;
  }
}
