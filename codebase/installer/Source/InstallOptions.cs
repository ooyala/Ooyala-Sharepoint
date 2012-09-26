using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;


namespace SharePointInstaller
{
  public sealed class InstallOptions
  {
    private readonly IList<SPWebApplication> webApplicationTargets;
    private readonly IList<SPSite> siteCollectionTargets;

    public InstallOptions()
    {
      this.webApplicationTargets = new List<SPWebApplication>();
      this.siteCollectionTargets = new List<SPSite>();
    }

    public IList<SPWebApplication> WebApplicationTargets
    {
      get { return webApplicationTargets; }
    }

    public IList<SPSite> SiteCollectionTargets
    {
      get { return siteCollectionTargets; }
    }
  }
}
