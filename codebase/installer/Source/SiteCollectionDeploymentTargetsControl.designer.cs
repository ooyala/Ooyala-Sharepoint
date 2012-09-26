namespace SharePointInstaller
{
  partial class SiteCollectionDeploymentTargetsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteCollectionDeploymentTargetsControl));
            this.webApplicationsLabel = new System.Windows.Forms.Label();
            this.hintLabel = new System.Windows.Forms.Label();
            this.siteCollectionsTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
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
            // siteCollectionsTreeView
            // 
            resources.ApplyResources(this.siteCollectionsTreeView, "siteCollectionsTreeView");
            this.siteCollectionsTreeView.CheckBoxes = true;
            this.siteCollectionsTreeView.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.siteCollectionsTreeView.Name = "siteCollectionsTreeView";
            this.siteCollectionsTreeView.SelectedImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            // 
            // SiteCollectionDeploymentTargetsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.siteCollectionsTreeView);
            this.Controls.Add(this.webApplicationsLabel);
            this.Controls.Add(this.hintLabel);
            this.Name = "SiteCollectionDeploymentTargetsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label webApplicationsLabel;
    private System.Windows.Forms.Label hintLabel;
      private System.Windows.Forms.TreeView siteCollectionsTreeView;
  }
}
