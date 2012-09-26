namespace SharePointInstaller
{
  partial class RepairControl
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepairControl));
        this.messageLabel = new System.Windows.Forms.Label();
        this.repairRadioButton = new System.Windows.Forms.RadioButton();
        this.removeRadioButton = new System.Windows.Forms.RadioButton();
        this.hintLabel = new System.Windows.Forms.Label();
        this.removeDescriptionLabel = new System.Windows.Forms.Label();
        this.repairDescriptionLabel = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // messageLabel
        // 
        this.messageLabel.AccessibleDescription = null;
        this.messageLabel.AccessibleName = null;
        resources.ApplyResources(this.messageLabel, "messageLabel");
        this.messageLabel.Font = null;
        this.messageLabel.Name = "messageLabel";
        // 
        // repairRadioButton
        // 
        this.repairRadioButton.AccessibleDescription = null;
        this.repairRadioButton.AccessibleName = null;
        resources.ApplyResources(this.repairRadioButton, "repairRadioButton");
        this.repairRadioButton.BackgroundImage = null;
        this.repairRadioButton.Checked = true;
        this.repairRadioButton.Name = "repairRadioButton";
        this.repairRadioButton.TabStop = true;
        this.repairRadioButton.UseVisualStyleBackColor = true;
        this.repairRadioButton.CheckedChanged += new System.EventHandler(this.repairRadioButton_CheckedChanged);
        // 
        // removeRadioButton
        // 
        this.removeRadioButton.AccessibleDescription = null;
        this.removeRadioButton.AccessibleName = null;
        resources.ApplyResources(this.removeRadioButton, "removeRadioButton");
        this.removeRadioButton.BackgroundImage = null;
        this.removeRadioButton.Name = "removeRadioButton";
        this.removeRadioButton.UseVisualStyleBackColor = true;
        this.removeRadioButton.CheckedChanged += new System.EventHandler(this.removeRadioButton_CheckedChanged);
        // 
        // hintLabel
        // 
        this.hintLabel.AccessibleDescription = null;
        this.hintLabel.AccessibleName = null;
        resources.ApplyResources(this.hintLabel, "hintLabel");
        this.hintLabel.Font = null;
        this.hintLabel.Name = "hintLabel";
        // 
        // removeDescriptionLabel
        // 
        this.removeDescriptionLabel.AccessibleDescription = null;
        this.removeDescriptionLabel.AccessibleName = null;
        resources.ApplyResources(this.removeDescriptionLabel, "removeDescriptionLabel");
        this.removeDescriptionLabel.Font = null;
        this.removeDescriptionLabel.Name = "removeDescriptionLabel";
        // 
        // repairDescriptionLabel
        // 
        this.repairDescriptionLabel.AccessibleDescription = null;
        this.repairDescriptionLabel.AccessibleName = null;
        resources.ApplyResources(this.repairDescriptionLabel, "repairDescriptionLabel");
        this.repairDescriptionLabel.Font = null;
        this.repairDescriptionLabel.Name = "repairDescriptionLabel";
        // 
        // RepairControl
        // 
        this.AccessibleDescription = null;
        this.AccessibleName = null;
        resources.ApplyResources(this, "$this");
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackgroundImage = null;
        this.Controls.Add(this.repairDescriptionLabel);
        this.Controls.Add(this.removeDescriptionLabel);
        this.Controls.Add(this.hintLabel);
        this.Controls.Add(this.removeRadioButton);
        this.Controls.Add(this.repairRadioButton);
        this.Controls.Add(this.messageLabel);
        this.Font = null;
        this.Name = "RepairControl";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label messageLabel;
    private System.Windows.Forms.RadioButton repairRadioButton;
    private System.Windows.Forms.RadioButton removeRadioButton;
    private System.Windows.Forms.Label hintLabel;
    private System.Windows.Forms.Label removeDescriptionLabel;
    private System.Windows.Forms.Label repairDescriptionLabel;
  }
}
