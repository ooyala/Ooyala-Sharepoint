namespace SharePointInstaller
{
  partial class DeploymentTargetsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeploymentTargetsControl));
            this.webApplicationsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.webApplicationsLabel = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // webApplicationsCheckedListBox
            // 
            resources.ApplyResources(this.webApplicationsCheckedListBox, "webApplicationsCheckedListBox");
            this.webApplicationsCheckedListBox.CheckOnClick = true;
            this.webApplicationsCheckedListBox.FormattingEnabled = true;
            this.webApplicationsCheckedListBox.Name = "webApplicationsCheckedListBox";
            this.webApplicationsCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.webApplicationsCheckedListBox_SelectedIndexChanged);
            // 
            // webApplicationsLabel
            // 
            resources.ApplyResources(this.webApplicationsLabel, "webApplicationsLabel");
            this.webApplicationsLabel.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.webApplicationsLabel.Name = "webApplicationsLabel";
            // 
            // hintLabel
            // 
            resources.ApplyResources(this.hintLabel, "hintLabel");
            this.hintLabel.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.hintLabel.Name = "hintLabel";
            // 
            // DeploymentTargetsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webApplicationsCheckedListBox);
            this.Controls.Add(this.webApplicationsLabel);
            this.Controls.Add(this.hintLabel);
            this.Name = "DeploymentTargetsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckedListBox webApplicationsCheckedListBox;
    private System.Windows.Forms.Label webApplicationsLabel;
    private System.Windows.Forms.Label hintLabel;
  }
}
