/*
Copyright (c) 2012, Ooyala, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following
conditions are met:
     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer 
        in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
namespace SharePointInstaller
{
  partial class InstallerForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerForm));
            this.contentPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.topSeparatorPanel = new System.Windows.Forms.Panel();
            this.bottomSeparatorPanel = new System.Windows.Forms.Panel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.logoPicture = new System.Windows.Forms.PictureBox();
            this.subTitleLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.buttonPanel.SuspendLayout();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            resources.ApplyResources(this.contentPanel, "contentPanel");
            this.contentPanel.Name = "contentPanel";
            // 
            // buttonPanel
            // 
            resources.ApplyResources(this.buttonPanel, "buttonPanel");
            this.buttonPanel.Controls.Add(this.cancelButton, 3, 0);
            this.buttonPanel.Controls.Add(this.prevButton, 1, 0);
            this.buttonPanel.Controls.Add(this.nextButton, 2, 0);
            this.buttonPanel.Name = "buttonPanel";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // prevButton
            // 
            resources.ApplyResources(this.prevButton, "prevButton");
            this.prevButton.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.prevButton.Name = "prevButton";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // nextButton
            // 
            resources.ApplyResources(this.nextButton, "nextButton");
            this.nextButton.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.nextButton.Name = "nextButton";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // topSeparatorPanel
            // 
            resources.ApplyResources(this.topSeparatorPanel, "topSeparatorPanel");
            this.topSeparatorPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.topSeparatorPanel.Name = "topSeparatorPanel";
            // 
            // bottomSeparatorPanel
            // 
            resources.ApplyResources(this.bottomSeparatorPanel, "bottomSeparatorPanel");
            this.bottomSeparatorPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.bottomSeparatorPanel.Name = "bottomSeparatorPanel";
            // 
            // titlePanel
            // 
            resources.ApplyResources(this.titlePanel, "titlePanel");
            this.titlePanel.BackColor = System.Drawing.Color.White;
            this.titlePanel.Controls.Add(this.logoPicture);
            this.titlePanel.Controls.Add(this.subTitleLabel);
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Name = "titlePanel";
            // 
            // logoPicture
            // 
            resources.ApplyResources(this.logoPicture, "logoPicture");
            this.logoPicture.BackColor = System.Drawing.Color.Transparent;
            this.logoPicture.Name = "logoPicture";
            this.logoPicture.TabStop = false;
            // 
            // subTitleLabel
            // 
            resources.ApplyResources(this.subTitleLabel, "subTitleLabel");
            this.subTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.subTitleLabel.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.subTitleLabel.Name = "subTitleLabel";
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.ImageKey = global::SharePointInstaller.Resources.CommonUIStrings.controlSubTitleOptions;
            this.titleLabel.Name = "titleLabel";
            // 
            // InstallerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ControlBox = false;
            this.Controls.Add(this.topSeparatorPanel);
            this.Controls.Add(this.bottomSeparatorPanel);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.buttonPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallerForm";
            this.buttonPanel.ResumeLayout(false);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel titlePanel;
    private System.Windows.Forms.Panel contentPanel;
    private System.Windows.Forms.Panel topSeparatorPanel;
    private System.Windows.Forms.Panel bottomSeparatorPanel;
    private System.Windows.Forms.Button nextButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button prevButton;
    private System.Windows.Forms.Label titleLabel;
    private System.Windows.Forms.Label subTitleLabel;
    private System.Windows.Forms.TableLayoutPanel buttonPanel;
    private System.Windows.Forms.PictureBox logoPicture;
  }
}