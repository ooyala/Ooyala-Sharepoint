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
