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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SharePointInstaller
{
  public partial class InstallerForm : Form
  {
    private readonly InstallerControlList contentControls;
    private readonly InstallOptions options;
    private InstallOperation operation = InstallOperation.Install;
    private InstallerControl currentContentControl;
    private int currentContentControlIndex = 0;

    public InstallerForm()
    {
      this.contentControls = new InstallerControlList();
      this.options = new InstallOptions();
      InitializeComponent();

      this.Load += new EventHandler(InstallerForm_Load);
    }

    #region Event Handlers

    private void InstallerForm_Load(object sender, EventArgs e)
    {
      string bannerImageFile = InstallConfiguration.BannerImage;
      if (!String.IsNullOrEmpty(bannerImageFile))
      {
        this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        if (bannerImageFile != "Default")
        {
          this.titlePanel.BackgroundImage = LoadImage(bannerImageFile);
        } else
        {
          this.titlePanel.BackgroundImage = global::SharePointInstaller.Properties.Resources.Banner;
        }
      }

      string logoImageFile = InstallConfiguration.LogoImage;
      if (!String.IsNullOrEmpty(logoImageFile))
      {
        if (logoImageFile != "Default")
        {
          this.logoPicture.BackgroundImage = LoadImage(logoImageFile);
        }
      }

      ReplaceContentControl(0);
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
      currentContentControlIndex++;
      ReplaceContentControl(currentContentControlIndex);
    }

    private void prevButton_Click(object sender, EventArgs e)
    {
      currentContentControlIndex--;
      ReplaceContentControl(currentContentControlIndex);
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      if (currentContentControl != null)
      {
        currentContentControl.RequestCancel();
      }
      else
      {
        Application.Exit();
      }
    }

    #endregion

    #region Public Properties

    public InstallerControlList ContentControls
    {
      get { return contentControls; }
    }

    public Button AbortButton
    {
      get
      {
        return cancelButton;
      }
    }

    public Button PrevButton
    {
      get
      {
        return prevButton;
      }
    }

    public Button NextButton
    {
      get
      {
        return nextButton;
      }
    }

    public InstallOperation Operation
    {
      get { return operation; }
      set { operation = value; }
    }

    #endregion

    #region Public Methods

    public void SetTitle(string title)
    {
      titleLabel.Text = title;
    }

    public void SetSubTitle(string title)
    {
      subTitleLabel.Text = title;
    }

    #endregion

    #region Private Methods

    private void ReplaceContentControl(int index)
    {
      if (currentContentControl != null)
      {
        currentContentControl.Close(options);
      }

      if (index == 0)
      {
        prevButton.Enabled = false;
        nextButton.Enabled = true;
      } else if (index == (contentControls.Count - 1))
      {
        prevButton.Enabled = true;
        nextButton.Enabled = false;
      } else if(index ==2)
      {
        prevButton.Enabled = true;
        nextButton.Enabled = false;
      }
      else
      {
          prevButton.Enabled = true;
          nextButton.Enabled = true;
      }

      InstallerControl newContentControl = contentControls[index];
      newContentControl.Dock = DockStyle.Fill;

      titleLabel.Text = newContentControl.Title;
      subTitleLabel.Text = newContentControl.SubTitle;

      contentPanel.Controls.Clear();
      contentPanel.Controls.Add(newContentControl);

      newContentControl.Open(options);

      currentContentControl = newContentControl;
    }

    private Image LoadImage(string filename)
    {
      try
      {
        return Image.FromFile(filename);
      }

      catch (IOException)
      {
        return null;
      }
    }

    #endregion
  }
}