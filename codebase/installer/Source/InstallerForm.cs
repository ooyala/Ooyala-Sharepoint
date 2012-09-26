
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