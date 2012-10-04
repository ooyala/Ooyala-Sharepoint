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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SharePointInstaller.Resources;


namespace SharePointInstaller
{
  public partial class FinishedControl : SharePointInstaller.InstallerControl
  {
    #region Constants

    public const int DefaultLinkHeight = 28;

    #endregion

    #region Member Variables

    private bool initialized = false;

    #endregion

    #region Constructor

    public FinishedControl()
    {
      InitializeComponent();
    }

    #endregion

    #region Overrides

    protected internal override void Open(InstallOptions options)
    {
      if (!initialized)
      {
        initialized = true;
        if (Form.Operation == InstallOperation.Install)
        {
          AddLinks(options);
        }
      }
      Form.AbortButton.Enabled = true;
    }

    #endregion

    #region Private Methods

    private void AddLinks(InstallOptions options)
    {
      // Show a documentation Url if one is configured
      if (!String.IsNullOrEmpty(InstallConfiguration.DocumentationUrl))
      {
        string linkText = InstallConfiguration.FormatString(CommonUIStrings.finishedLinkText);
        int linkStart = linkText.Length - 5;
        AddLink(linkText, linkStart, 4, InstallConfiguration.DocumentationUrl);
      }

      // Add the for each target
      if (InstallConfiguration.FeatureScope == SPFeatureScope.Site &&
        !String.IsNullOrEmpty(InstallConfiguration.SiteCollectionRelativeConfigLink))
      {
        // Add site collection links
        AddSiteCollectionLinks(options.SiteCollectionTargets, FormatRelativeLink(InstallConfiguration.SiteCollectionRelativeConfigLink));
      }
      else if (InstallConfiguration.FeatureScope == SPFeatureScope.Farm &&
        !String.IsNullOrEmpty(InstallConfiguration.SSPRelativeConfigLink))
      {
        // Add Shared Service Provider links
        // Note that thes are really Shared Resource Provider links - we just wish we knew how to only show links for a SSP and not SRPs
        AddSspLinks(options.WebApplicationTargets, FormatRelativeLink(InstallConfiguration.SSPRelativeConfigLink));
      }
    }

    private string FormatRelativeLink(string relativeLink)
    {
      if (!relativeLink.StartsWith("/"))
      {
        relativeLink = "/" + relativeLink;
      }
      return relativeLink;
    }

    private void AddLink(string linkText, int linkStart, int linkLength, string url)
    {
      LinkLabel linkLabel = new LinkLabel();
      linkLabel.Text = linkText;
      linkLabel.LinkArea = new LinkArea(linkStart, linkLength);
      linkLabel.Links[0].LinkData = url;
      linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel_LinkClicked);
      linkLabel.Width = tableLayoutPanel.Width - 10;
      linkLabel.Height = DefaultLinkHeight;
      tableLayoutPanel.Controls.Add(linkLabel);
    }

    private void AddSspLinks(IList<SPWebApplication> webApplicationTargets, string relativeLink)
    {
      foreach (SPWebApplication webApp in webApplicationTargets)
      {
        Hashtable properties = webApp.Properties;
        if (properties.ContainsKey("Microsoft.Office.Server.SharedResourceProvider"))
        {
          string linkText = InstallConfiguration.FormatString(CommonUIStrings.finishedLinkTextSsp, webApp.Sites[0].Url);
          AddLink(linkText, 6, 4, webApp.Sites[0].Url + relativeLink);
        }
      }
    }

    private void AddSiteCollectionLinks(IList<SPSite> siteCollectionTargets, string relativeLink)
    {
      foreach (SPSite siteCollection in siteCollectionTargets)
      {
        string linkText = InstallConfiguration.FormatString(CommonUIStrings.finishedLinkTextSiteCollection, siteCollection.Url);
        AddLink(linkText, 6, 4, siteCollection.Url + relativeLink);
      }
    }

    #endregion

    #region Event Handlers

    void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      string target = e.Link.LinkData as string;
      if (target != null && (target.StartsWith("http") || target.StartsWith("www")))
      {
        System.Diagnostics.Process.Start(target);
      }
      else
      {
        MessageBox.Show(string.Format(CommonUIStrings.linkLabelDialogText, target));
      }
    }

    #endregion
  }
}

