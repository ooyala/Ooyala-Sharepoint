using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

using Microsoft.Win32;

using Microsoft.SharePoint.Administration;
using System.Configuration;
using System.IO;
using SharePointInstaller.Resources;


namespace SharePointInstaller
{
  public partial class SystemCheckControl : InstallerControl
  {
    private static readonly ILog log = LogManager.GetLogger();
    private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

    private bool requireMOSS;
    private bool requireSearchSKU;
    private SystemCheckList checks;
    private int nextCheckIndex;
    private int errors;

    #region Constructors

    public SystemCheckControl()
    {
      InitializeComponent();

      this.Load += new EventHandler(SystemCheckControl_Load);
    }

    #endregion

    #region Public Properties

    public bool RequireMOSS
    {
      get { return requireMOSS; }
      set { requireMOSS = value; }
    }

    public bool RequireSearchSKU
    {
      get { return requireSearchSKU; }
      set { requireSearchSKU = value; }
    }

    #endregion

    #region Event Handlers

    private void SystemCheckControl_Load(object sender, EventArgs e)
    {
    }

    private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
    {
      timer.Stop();

      if (nextCheckIndex < checks.Count)
      {
        if (ExecuteCheck(nextCheckIndex))
        {
          nextCheckIndex++;
          timer.Start();
          return;
        } 
      }

      FinalizeChecks();
    }

    #endregion

    #region Protected Methods

    protected internal override void Open(InstallOptions options)
    {
      if (checks == null)
      {
        Form.NextButton.Enabled = false;
        Form.PrevButton.Enabled = false;

        checks = new SystemCheckList();
        InitializeChecks();

        timer.Interval = 100;
        timer.Tick += new EventHandler(TimerEventProcessor);
        timer.Start();
      }
    }

    protected internal override void Close(InstallOptions options)
    {
    }

    #endregion

    #region Private Methods

    private void InitializeChecks()
    {
      this.tableLayoutPanel.SuspendLayout();

      //
      // WSS Installed Check
      //
      WSSInstalledCheck wssCheck = new WSSInstalledCheck();
      wssCheck.QuestionText = CommonUIStrings.wssCheckQuestionText;
      wssCheck.OkText = CommonUIStrings.wssCheckOkText;
      wssCheck.ErrorText = CommonUIStrings.wssCheckErrorText;
      AddCheck(wssCheck);

      //
      // MOSS Installed Check
      //
      if (requireMOSS)
      {
        MOSSInstalledCheck mossCheck = new MOSSInstalledCheck();
        mossCheck.QuestionText = CommonUIStrings.mossCheckQuestionText;
        mossCheck.OkText = CommonUIStrings.mossCheckOkText;
        mossCheck.ErrorText = CommonUIStrings.mossCheckErrorText;
        AddCheck(mossCheck);
      }

      //
      // Admin Rights Check
      //
      AdminRightsCheck adminRightsCheck = new AdminRightsCheck();
      adminRightsCheck.QuestionText = CommonUIStrings.adminRightsCheckQuestionText;
      adminRightsCheck.OkText = CommonUIStrings.adminRightsCheckOkText;
      adminRightsCheck.ErrorText = CommonUIStrings.adminRightsCheckErrorText;
      AddCheck(adminRightsCheck);

      //
      // Admin Service Check
      //
      AdminServiceCheck adminServiceCheck = new AdminServiceCheck();
      adminServiceCheck.QuestionText = CommonUIStrings.adminServiceCheckQuestionText;
      adminServiceCheck.OkText = CommonUIStrings.adminServiceCheckOkText;
      adminServiceCheck.ErrorText = CommonUIStrings.adminServiceCheckErrorText;
      AddCheck(adminServiceCheck);

      //
      // Timer Service Check
      //
      TimerServiceCheck timerServiceCheck = new TimerServiceCheck();
      timerServiceCheck.QuestionText = CommonUIStrings.timerServiceCheckQuestionText;
      timerServiceCheck.OkText = CommonUIStrings.timerServiceCheckOkText;
      timerServiceCheck.ErrorText = CommonUIStrings.timerServiceCheckErrorText;
      AddCheck(timerServiceCheck);

      //
      // Solution File Check
      //
      SolutionFileCheck solutionFileCheck = new SolutionFileCheck();
      solutionFileCheck.QuestionText = CommonUIStrings.solutionFileCheckQuestionText;
      solutionFileCheck.OkText = CommonUIStrings.solutionFileCheckOkText;
      AddCheck(solutionFileCheck);

      //
      // Solution Check
      //
      SolutionCheck solutionCheck = new SolutionCheck();
      solutionCheck.QuestionText = InstallConfiguration.FormatString(CommonUIStrings.solutionCheckQuestionText);
      solutionCheck.OkText = InstallConfiguration.FormatString(CommonUIStrings.solutionFileCheckOkText);
      solutionCheck.ErrorText = InstallConfiguration.FormatString(CommonUIStrings.solutionCheckErrorText);
      AddCheck(solutionCheck);

      //
      // Add empty row that will eat up the rest of the 
      // row space in the layout table.
      //
      this.tableLayoutPanel.RowCount++;
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

      this.tableLayoutPanel.ResumeLayout(false);
      this.tableLayoutPanel.PerformLayout();
    }

    private bool ExecuteCheck(int index)
    {
      SystemCheck check = checks[index];
      string imageLabelName = "imageLabel" + index;
      string textLabelName = "textLabel" + index;
      Label imageLabel = (Label)tableLayoutPanel.Controls[imageLabelName];
      Label textLabel = (Label)tableLayoutPanel.Controls[textLabelName];

      try
      {
        SystemCheckResult result = check.Execute();
        if (result == SystemCheckResult.Success)
        {
          imageLabel.Image = global::SharePointInstaller.Properties.Resources.CheckOk;
          textLabel.Text = check.OkText;
        } else if (result == SystemCheckResult.Error)
        {
          errors++;
          imageLabel.Image = global::SharePointInstaller.Properties.Resources.CheckFail;
          textLabel.Text = check.ErrorText;
        }

        int nextIndex = index + 1;
        string nextImageLabelName = "imageLabel" + nextIndex;
        Label nextImageLabel = (Label)tableLayoutPanel.Controls[nextImageLabelName];
        if (nextImageLabel != null)
        {
          nextImageLabel.Image = global::SharePointInstaller.Properties.Resources.CheckPlay;
        }

        return true;
      }

      catch (InstallException ex)
      {
        errors++;
        imageLabel.Image = global::SharePointInstaller.Properties.Resources.CheckFail;
        textLabel.Text = ex.Message;
      }

      return false;
    }

    private void FinalizeChecks()
    {
      if (errors == 0)
      {
        ConfigureControls();
        Form.NextButton.Enabled = true;
        messageLabel.Text = CommonUIStrings.messageLabelTextSuccess;
      } else
      {
        messageLabel.Text = InstallConfiguration.FormatString(CommonUIStrings.messageLabelTextError);
      }

      Form.PrevButton.Enabled = true;
    }

    private void AddCheck(SystemCheck check)
    {
      int row = tableLayoutPanel.RowCount;

      Label imageLabel = new Label();
      imageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      imageLabel.Image = global::SharePointInstaller.Properties.Resources.CheckWait;
      imageLabel.Location = new System.Drawing.Point(3, 0);
      imageLabel.Name = "imageLabel" + row;
      imageLabel.Size = new System.Drawing.Size(24, 20);

      Label textLabel = new Label();
      textLabel.AutoSize = true;
      textLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      textLabel.Location = new System.Drawing.Point(33, 0);
      textLabel.Name = "textLabel" + row;
      textLabel.Size = new System.Drawing.Size(390, 20);
      textLabel.Text = check.QuestionText;
      textLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

      this.tableLayoutPanel.Controls.Add(imageLabel, 0, row);
      this.tableLayoutPanel.Controls.Add(textLabel, 1, row);
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel.RowCount++;

      checks.Add(check);
    }

    private void ConfigureControls()
    {
      SolutionCheck check = (SolutionCheck)checks["SolutionCheck"];
      SPSolution solution = check.Solution;

      if (solution == null)
      {
        AddInstallControls();
      } 
      
      else
      {
        Version installedVersion = InstallConfiguration.InstalledVersion;
        Version newVersion = InstallConfiguration.SolutionVersion;

        if (newVersion != installedVersion)
        {
          Form.ContentControls.Add(Program.CreateUpgradeControl());
        } else
        {
          Form.ContentControls.Add(Program.CreateRepairControl());
        }
      }
    }

    private void AddInstallControls()
    {
      Form.ContentControls.Add(Program.CreateSqlServerSettingsControl());
      Form.ContentControls.Add(Program.CreateAPISettingsControl());
      Form.ContentControls.Add(Program.CreateDeploymentTargetsControl());
      Form.ContentControls.Add(Program.CreateProcessControl());
    }

    #endregion

    #region Check Classes

    private enum SystemCheckResult
    {
      Inconclusive,
      Success,
      Error
    }

    /// <summary>
    /// Base class for all system checks.
    /// </summary>
    private abstract class SystemCheck
    {
      private readonly string id;
      private string questionText;
      private string okText;
      private string errorText;

      protected SystemCheck(string id)
      {
        this.id = id;
      }

      public string Id
      {
        get { return id; }
      } 

      public string QuestionText
      {
        get { return questionText; }
        set { questionText = value; }
      }

      public string OkText
      {
        get { return okText; }
        set { okText = value; }
      }

      public string ErrorText
      {
        get { return errorText; }
        set { errorText = value; }
      }

      internal SystemCheckResult Execute()
      {
        if (CanRun)
        {
          return DoExecute();
        }
        return SystemCheckResult.Inconclusive;
      }

      protected abstract SystemCheckResult DoExecute();

      protected virtual bool CanRun
      {
        get { return true; }
      }

      protected static bool IsWSSInstalled
      {
        get
        {
          string name = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\14.0";

          try
          {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(name);
            if (key != null)
            {
              object val = key.GetValue("SharePoint");
              if (val != null && val.Equals("Installed"))
              {
                return true;
              }
            }
            return false;
          }

          catch (SecurityException ex)
          {
            throw new InstallException(string.Format(Resources.CommonUIStrings.installExceptionAccessDenied, name), ex);
          }
        }
      }

      protected static bool IsMOSSInstalled
      {
        get
        {
          string name = @"SOFTWARE\Microsoft\Office Server\14.0";

          try
          {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(name);
            if (key != null)
            {
              string versionStr = key.GetValue("BuildVersion") as string;
              if (versionStr != null)
              {
                Version buildVersion = new Version(versionStr);
                if (buildVersion.Major == 14)
                {
                  return true;
                }
              }
            }
            return false;
          }

          catch (SecurityException ex)
          {
            throw new InstallException(string.Format(CommonUIStrings.installExceptionAccessDenied, name), ex);
          }
        }
      }
    }
    
    private class SystemCheckList : List<SystemCheck>
    {
      internal SystemCheck this[string id]
      {
        get
        {
          foreach (SystemCheck check in this)
          {
            if (check.Id == id) return check;
          }
          return null;
        }
      }
    }

    /// <summary>
    /// Checks if WSS 3.0 is installed.
    /// </summary>
    private class WSSInstalledCheck : SystemCheck
    {
      internal WSSInstalledCheck() : base("WSSInstalledCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        if (IsWSSInstalled) return SystemCheckResult.Success;
        return SystemCheckResult.Error;
      }
    }

    /// <summary>
    /// Checks if Microsoft Office Server 2007 is installed.
    /// </summary>
    private class MOSSInstalledCheck : SystemCheck
    {
      internal MOSSInstalledCheck() : base("MOSSInstalledCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        if (IsMOSSInstalled) return SystemCheckResult.Success;
        return SystemCheckResult.Error;
      }
    }

    /// <summary>
    /// Checks if the Windows SharePoint Services Administration service is started.
    /// </summary>
    private class AdminServiceCheck : SystemCheck
    {
      internal AdminServiceCheck() : base("AdminServiceCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        try
        {
          ServiceController sc = new ServiceController("SPAdminV4");
          if (sc.Status == ServiceControllerStatus.Running)
          {
            return SystemCheckResult.Success;
          }
          return SystemCheckResult.Error;
        }

        catch (Win32Exception ex)
        {
          log.Error(ex.Message, ex);
        }

        catch (InvalidOperationException ex)
        {
          log.Error(ex.Message, ex);
        }

        return SystemCheckResult.Inconclusive;
      }

      protected override bool CanRun
      {
        get { return IsWSSInstalled; }
      }
    }

    /// <summary>
    /// Checks if the Windows SharePoint Services Timer service is started.
    /// </summary>
    private class TimerServiceCheck : SystemCheck
    {
      internal TimerServiceCheck() : base("TimerServiceCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        try
        {
          ServiceController sc = new ServiceController("SPTimerV4");
          if (sc.Status == ServiceControllerStatus.Running)
          {
            return SystemCheckResult.Success;
          }
          return SystemCheckResult.Error;          
        }

        catch (System.ServiceProcess.TimeoutException ex)
        {
          log.Error(ex.Message, ex);
        }

        catch (Win32Exception ex)
        {
          log.Error(ex.Message, ex);
        }

        catch (InvalidOperationException ex)
        {
          log.Error(ex.Message, ex);
        }

        return SystemCheckResult.Inconclusive;
      }

      protected override bool CanRun
      {
        get { return IsWSSInstalled; }
      }
    }

    /// <summary>
    /// Checks if the current user is an administrator.
    /// </summary>
    private class AdminRightsCheck : SystemCheck
    {
      internal AdminRightsCheck() : base("AdminRightsCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        try
        {
          if (SPFarm.Local.CurrentUserIsAdministrator())
          {
            return SystemCheckResult.Success;
          }
          return SystemCheckResult.Error;
        }

        catch (NullReferenceException)
        {
          throw new InstallException(CommonUIStrings.installExceptionDatabase);
        }

        catch (Exception ex)
        {
          throw new InstallException(ex.Message, ex);
        }
      }

      protected override bool CanRun
      {
        get { return IsWSSInstalled; }
      }
    }

    private class SolutionFileCheck : SystemCheck
    {
      internal SolutionFileCheck() : base("SolutionFileCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        string filename = InstallConfiguration.SolutionFile;
        if (!String.IsNullOrEmpty(filename))
        {
          FileInfo solutionFileInfo = new FileInfo(filename);
          if (!solutionFileInfo.Exists)
          {
            throw new InstallException(string.Format(CommonUIStrings.installExceptionFileNotFound, filename));
          }
        } else
        {
          throw new InstallException(CommonUIStrings.installExceptionConfigurationNoWsp);
        }

        return SystemCheckResult.Success;
      }
    }

    private class SolutionCheck : SystemCheck
    {
      private SPSolution solution;

      internal SolutionCheck() : base("SolutionCheck") { }

      protected override SystemCheckResult DoExecute()
      {
        Guid solutionId = Guid.Empty;
        try
        {
          solutionId = InstallConfiguration.SolutionId;
        }
        catch (ArgumentNullException)
        {
          throw new InstallException(CommonUIStrings.installExceptionConfigurationNoId);
        }
        catch (FormatException)
        {
          throw new InstallException(CommonUIStrings.installExceptionConfigurationInvalidId);
        }

        try
        {
          solution = SPFarm.Local.Solutions[solutionId];
          if (solution != null)
          {
            this.OkText = InstallConfiguration.FormatString(CommonUIStrings.solutionCheckOkTextInstalled);
          }
          else
          {
            this.OkText = InstallConfiguration.FormatString(CommonUIStrings.solutionCheckOkTextNotInstalled);
          }
        }

        catch (NullReferenceException)
        {
            throw new InstallException(CommonUIStrings.installExceptionDatabase);
        }

        catch (Exception ex)
        {
          throw new InstallException(ex.Message, ex);
        }

        return SystemCheckResult.Success;
      }

      protected override bool CanRun
      {
        get { return IsWSSInstalled; }
      }

      internal SPSolution Solution
      {
        get { return solution; }
      }
    }

    #endregion
  }
}