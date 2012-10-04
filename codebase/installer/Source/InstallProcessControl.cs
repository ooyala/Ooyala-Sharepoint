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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.SharePoint.Administration;
using System.Web.Configuration;
using Microsoft.SharePoint;
using System.IO;
using SharePointInstaller.Resources;
using System.Security;
using System.Reflection;
using System.Xml;


namespace SharePointInstaller
{
    public partial class InstallProcessControl : InstallerControl
    {
        private static readonly MessageCollector log = new MessageCollector(LogManager.GetLogger());

        private static readonly TimeSpan JobTimeout = TimeSpan.FromMinutes(15);

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private CommandList executeCommands;
        private CommandList rollbackCommands;
        private int nextCommand;
        private bool completed;
        private bool requestCancel;
        private int errors;
        private int rollbackErrors;

        public InstallProcessControl()
        {
            InitializeComponent();

            errorPictureBox.Visible = false;
            errorDetailsTextBox.Visible = false;

            this.Load += new EventHandler(InstallProcessControl_Load);
        }

        #region Event Handlers

        private void InstallProcessControl_Load(object sender, EventArgs e)
        {
            switch (Form.Operation)
            {
                case InstallOperation.Install:
                    Form.SetTitle(CommonUIStrings.installTitle);
                    Form.SetSubTitle(InstallConfiguration.FormatString(CommonUIStrings.installSubTitle));
                    break;

                case InstallOperation.Upgrade:
                    Form.SetTitle(CommonUIStrings.upgradeTitle);
                    Form.SetSubTitle(InstallConfiguration.FormatString(CommonUIStrings.upgradeSubTitle));
                    break;

                case InstallOperation.Repair:
                    Form.SetTitle(CommonUIStrings.repairTitle);
                    Form.SetSubTitle(InstallConfiguration.FormatString(CommonUIStrings.repairSubTitle));
                    break;

                case InstallOperation.Uninstall:
                    Form.SetTitle(CommonUIStrings.uninstallTitle);
                    Form.SetSubTitle(InstallConfiguration.FormatString(CommonUIStrings.uninstallSubTitle));
                    removeTableEntries();
                    break;
            }

            Form.PrevButton.Enabled = false;
            Form.NextButton.Enabled = false;
        }

        private void removeTableEntries()
        {
            try
            {
                //Configuration config = WebConfigurationManager.OpenWebConfiguration("/", webApp.Name);

                string ConnectionString = GetConnetionString(); ;
                string queryString = "DELETE FROM tblUser";
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(queryString, con);
                if (con != null)
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TimerEventInstall(Object myObject, EventArgs myEventArgs)
        {
            timer.Stop();

            if (requestCancel)
            {
                descriptionLabel.Text = Resources.CommonUIStrings.descriptionLabelTextOperationCanceled;
                InitiateRollback();
            }

            else if (nextCommand < executeCommands.Count)
            {
                try
                {
                    Command command = executeCommands[nextCommand];
                    if (command.Execute())
                    {
                        nextCommand++;
                        progressBar.PerformStep();

                        if (nextCommand < executeCommands.Count)
                        {
                            descriptionLabel.Text = executeCommands[nextCommand].Description;
                        }
                    }
                    timer.Start();
                }

                catch (Exception ex)
                {
                    log.Error(CommonUIStrings.logError);
                    log.Error(ex.Message, ex);

                    errors++;
                    errorPictureBox.Visible = true;
                    errorDetailsTextBox.Visible = true;
                    errorDetailsTextBox.Text = ex.Message;

                    descriptionLabel.Text = Resources.CommonUIStrings.descriptionLabelTextErrorsDetected;
                    InitiateRollback();
                }
            }

            else
            {
                descriptionLabel.Text = Resources.CommonUIStrings.descriptionLabelTextSuccess;
                HandleCompletion();
            }
        }

        private void TimerEventRollback(Object myObject, EventArgs myEventArgs)
        {
            timer.Stop();

            if (nextCommand < rollbackCommands.Count)
            {
                try
                {
                    Command command = rollbackCommands[nextCommand];
                    if (command.Rollback())
                    {
                        nextCommand++;
                        progressBar.PerformStep();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(CommonUIStrings.logError);
                    log.Error(ex.Message, ex);

                    rollbackErrors++;
                    nextCommand++;
                    progressBar.PerformStep();
                }

                timer.Start();
            }

            else
            {
                if (rollbackErrors == 0)
                {
                    descriptionLabel.Text = Resources.CommonUIStrings.descriptionLabelTextRollbackSuccess;
                }
                else
                {
                    descriptionLabel.Text = string.Format(Resources.CommonUIStrings.descriptionLabelTextRollbackError, rollbackErrors);
                }

                HandleCompletion();
            }
        }

        #endregion

        #region Protected Methods

        protected internal override void RequestCancel()
        {
            if (completed)
            {
                base.RequestCancel();
            }
            else
            {
                requestCancel = true;
                Form.AbortButton.Enabled = false;

            }
        }

        protected internal override void Open(InstallOptions options)
        {
            executeCommands = new CommandList();
            rollbackCommands = new CommandList();
            nextCommand = 0;
            SPFeatureScope featureScope = InstallConfiguration.FeatureScope;
            DeactivateSiteCollectionFeatureCommand deactivateSiteCollectionFeatureCommand = null;

            switch (Form.Operation)
            {
                case InstallOperation.Install:
                    executeCommands.Add(new ConfigureConnectionStringCommand(this, options.WebApplicationTargets));
                    executeCommands.Add(new AddSolutionCommand(this));
                    executeCommands.Add(new CreateDeploymentJobCommand(this, options.WebApplicationTargets));
                    executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionDeployment));
                    if (featureScope == SPFeatureScope.Farm)
                    {
                        executeCommands.Add(new ActivateFarmFeatureCommand(this));
                    }
                    else if (featureScope == SPFeatureScope.Site)
                    {
                        executeCommands.Add(new ActivateSiteCollectionFeatureCommand(this, options.SiteCollectionTargets));
                    }
                    executeCommands.Add(new RegisterVersionNumberCommand(this));

                    for (int i = executeCommands.Count - 1; i <= 0; i--)
                    {
                        rollbackCommands.Add(executeCommands[i]);
                    }
                    break;

                case InstallOperation.Upgrade:
                    if (featureScope == SPFeatureScope.Farm)
                    {
                        executeCommands.Add(new DeactivateFarmFeatureCommand(this));
                    }
                    else if (featureScope == SPFeatureScope.Site)
                    {
                        deactivateSiteCollectionFeatureCommand = new DeactivateSiteCollectionFeatureCommand(this);
                        executeCommands.Add(deactivateSiteCollectionFeatureCommand);
                    }
                    if (!IsSolutionRenamed())
                    {
                        executeCommands.Add(new CreateUpgradeJobCommand(this));
                        executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionUpgrade));
                    }
                    else
                    {
                        executeCommands.Add(new CreateRetractionJobCommand(this));
                        executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionRetraction));
                        executeCommands.Add(new RemoveSolutionCommand(this));
                        executeCommands.Add(new AddSolutionCommand(this));
                        executeCommands.Add(new CreateDeploymentJobCommand(this, GetDeployedApplications()));
                        executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionDeployment));
                    }
                    if (featureScope == SPFeatureScope.Farm)
                    {
                        executeCommands.Add(new ActivateFarmFeatureCommand(this));
                    }
                    if (featureScope == SPFeatureScope.Site)
                    {
                        executeCommands.Add(new ActivateSiteCollectionFeatureCommand(this, deactivateSiteCollectionFeatureCommand.DeactivatedSiteCollections));
                    }
                    executeCommands.Add(new RegisterVersionNumberCommand(this));
                    break;

                case InstallOperation.Repair:
                    if (featureScope == SPFeatureScope.Farm)
                    {
                        executeCommands.Add(new DeactivateFarmFeatureCommand(this));
                    }
                    if (featureScope == SPFeatureScope.Site)
                    {
                        deactivateSiteCollectionFeatureCommand = new DeactivateSiteCollectionFeatureCommand(this);
                        executeCommands.Add(deactivateSiteCollectionFeatureCommand);
                    }
                    executeCommands.Add(new CreateRetractionJobCommand(this));
                    executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionRetraction));
                    executeCommands.Add(new RemoveSolutionCommand(this));
                    executeCommands.Add(new AddSolutionCommand(this));
                    executeCommands.Add(new CreateDeploymentJobCommand(this, GetDeployedApplications()));
                    executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionDeployment));
                    if (featureScope == SPFeatureScope.Farm)
                    {
                        executeCommands.Add(new ActivateFarmFeatureCommand(this));
                    }
                    if (featureScope == SPFeatureScope.Site)
                    {
                        executeCommands.Add(new ActivateSiteCollectionFeatureCommand(this, deactivateSiteCollectionFeatureCommand.DeactivatedSiteCollections));
                    }
                    executeCommands.Add(new RegisterVersionNumberCommand(this));
                    break;

                case InstallOperation.Uninstall:
                    if (featureScope == SPFeatureScope.Farm)
                    {
                        executeCommands.Add(new DeactivateFarmFeatureCommand(this));
                    }
                    if (featureScope == SPFeatureScope.Site)
                    {
                        executeCommands.Add(new DeactivateSiteCollectionFeatureCommand(this));
                    }
                    executeCommands.Add(new CreateRetractionJobCommand(this));
                    executeCommands.Add(new WaitForJobCompletionCommand(this, CommonUIStrings.waitForSolutionRetraction));
                    executeCommands.Add(new RemoveSolutionCommand(this));
                    executeCommands.Add(new UnregisterVersionNumberCommand(this));
                    break;
            }

            progressBar.Maximum = executeCommands.Count;

            descriptionLabel.Text = executeCommands[0].Description;

            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerEventInstall);
            timer.Start();
        }

        #endregion

        #region Private Methods

        private void HandleCompletion()
        {
            completed = true;

            Form.NextButton.Enabled = true;
            Form.AbortButton.Text = CommonUIStrings.abortButtonText;
            Form.AbortButton.Enabled = true;

            CompletionControl nextControl = new CompletionControl();

            foreach (string message in log.Messages)
            {
                nextControl.Details += message + "\r\n";
            }

            switch (Form.Operation)
            {
                case InstallOperation.Install:
                    nextControl.Title = errors == 0 ? CommonUIStrings.installSuccess : CommonUIStrings.installError;
                    break;

                case InstallOperation.Upgrade:
                    nextControl.Title = errors == 0 ? CommonUIStrings.upgradeSuccess : CommonUIStrings.upgradeError;
                    break;

                case InstallOperation.Repair:
                    nextControl.Title = errors == 0 ? CommonUIStrings.repairSuccess : CommonUIStrings.repairError;
                    break;

                case InstallOperation.Uninstall:
                    nextControl.Title = errors == 0 ? CommonUIStrings.uninstallSuccess : CommonUIStrings.uninstallError;
                    break;
            }

            Form.ContentControls.Add(nextControl);
        }

        private void InitiateRollback()
        {
            Form.AbortButton.Enabled = false;

            progressBar.Maximum = rollbackCommands.Count;
            progressBar.Value = rollbackCommands.Count;
            nextCommand = 0;
            rollbackErrors = 0;
            progressBar.Step = -1;

            //
            // Create and start new timer.
            //
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerEventRollback);
            timer.Start();
        }

        private bool IsSolutionRenamed()
        {
            SPFarm farm = SPFarm.Local;
            SPSolution solution = farm.Solutions[InstallConfiguration.SolutionId];
            if (solution == null) return false;

            FileInfo solutionFileInfo = new FileInfo(InstallConfiguration.SolutionFile);

            return !solution.Name.Equals(solutionFileInfo.Name, StringComparison.OrdinalIgnoreCase);
        }

        private Collection<SPWebApplication> GetDeployedApplications()
        {
            SPFarm farm = SPFarm.Local;
            SPSolution solution = farm.Solutions[InstallConfiguration.SolutionId];
            if (solution.ContainsWebApplicationResource)
            {
                return solution.DeployedWebApplications;
            }
            return null;
        }

        #endregion

        #region Command Classes

        /// <summary>
        /// The base class of all installation commands.
        /// </summary>
        private abstract class Command
        {
            private readonly InstallProcessControl parent;

            protected Command(InstallProcessControl parent)
            {
                this.parent = parent;
            }

            internal InstallProcessControl Parent
            {
                get { return parent; }
            }

            internal abstract string Description { get; }

            protected internal virtual bool Execute() { return true; }

            protected internal virtual bool Rollback() { return true; }
        }

        private class CommandList : List<Command>
        {
        }

        /// <summary>
        /// The base class of all SharePoint solution related commands.
        /// </summary>
        private abstract class SolutionCommand : Command
        {
            protected SolutionCommand(InstallProcessControl parent) : base(parent) { }

            protected void RemoveSolution()
            {
                try
                {
                    SPFarm farm = SPFarm.Local;
                    SPSolution solution = farm.Solutions[InstallConfiguration.SolutionId];
                    if (solution != null)
                    {
                        if (!solution.Deployed)
                        {
                            solution.Delete();
                        }
                    }
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Command for adding the SharePoint solution.
        /// </summary>
        private class AddSolutionCommand : SolutionCommand
        {
            internal AddSolutionCommand(InstallProcessControl parent)
                : base(parent)
            {
            }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.addSolutionCommand;
                }
            }

            protected internal override bool Execute()
            {
                string filename = InstallConfiguration.SolutionFile;
                if (String.IsNullOrEmpty(filename))
                {
                    throw new InstallException(CommonUIStrings.installExceptionConfigurationNoWsp);
                }

                try
                {
                    SPFarm farm = SPFarm.Local;
                    SPSolution solution = farm.Solutions.Add(filename);
                    return true;
                }

                catch (SecurityException ex)
                {
                    string message = CommonUIStrings.addSolutionAccessError;
                    if (Environment.OSVersion.Version >= new Version("6.0"))
                        message += " " + CommonUIStrings.addSolutionAccessErrorWinServer2008Solution;
                    else
                        message += " " + CommonUIStrings.addSolutionAccessErrorWinServer2003Solution;
                    throw new InstallException(message, ex);
                }

                catch (IOException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }

                catch (ArgumentException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }

            protected internal override bool Rollback()
            {
                RemoveSolution();
                return true;
            }
        }

        /// <summary>
        /// Command for removing the SharePoint solution.
        /// </summary>
        private class RemoveSolutionCommand : SolutionCommand
        {
            internal RemoveSolutionCommand(InstallProcessControl parent) : base(parent) { }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.removeSolutionCommand;
                }
            }

            protected internal override bool Execute()
            {
                RemoveSolution();
                return true;
            }
        }

        private abstract class JobCommand : Command
        {
            protected JobCommand(InstallProcessControl parent) : base(parent) { }

            protected static void RemoveExistingJob(SPSolution solution)
            {
                if (solution.JobStatus == SPRunningJobStatus.Initialized)
                {
                    throw new InstallException(CommonUIStrings.installExceptionDuplicateJob);
                }

                SPJobDefinition jobDefinition = GetSolutionJob(solution);
                if (jobDefinition != null)
                {
                    jobDefinition.Delete();
                    Thread.Sleep(500);
                }
            }

            private static SPJobDefinition GetSolutionJob(SPSolution solution)
            {
                SPFarm localFarm = SPFarm.Local;
                SPTimerService service = localFarm.TimerService;
                foreach (SPJobDefinition definition in service.JobDefinitions)
                {
                    if (definition.Title != null && definition.Title.Contains(solution.Name))
                    {
                        return definition;
                    }
                }
                return null;
            }

            protected static DateTime GetImmediateJobTime()
            {
                return DateTime.Now - TimeSpan.FromDays(1);
            }
        }

        /// <summary>
        /// Command for creating a deployment job.
        /// </summary>
        private class CreateDeploymentJobCommand : JobCommand
        {
            private readonly Collection<SPWebApplication> applications;

            internal CreateDeploymentJobCommand(InstallProcessControl parent, IList<SPWebApplication> applications)
                : base(parent)
            {
                if (applications != null)
                {
                    this.applications = new Collection<SPWebApplication>();
                    foreach (SPWebApplication application in applications)
                    {
                        this.applications.Add(application);
                    }
                }
                else
                {
                    this.applications = null;
                }
            }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.createDeploymentJobCommand;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    SPSolution installedSolution = SPFarm.Local.Solutions[InstallConfiguration.SolutionId];

                    //
                    // Remove existing job, if any. 
                    //
                    if (installedSolution.JobExists)
                    {
                        RemoveExistingJob(installedSolution);
                    }

                    log.Info("***** SOLUTION DEPLOYMENT *****");
                    if (installedSolution.ContainsWebApplicationResource && applications != null && applications.Count > 0)
                    {
                        installedSolution.Deploy(GetImmediateJobTime(), true, applications, true);
                    }
                    else
                    {
                        installedSolution.Deploy(GetImmediateJobTime(), true, true);
                    }

                    return true;
                }

                catch (SPException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }

            protected internal override bool Rollback()
            {
                SPSolution installedSolution = SPFarm.Local.Solutions[InstallConfiguration.SolutionId];

                if (installedSolution != null)
                {
                    //
                    // Remove existing job, if any. 
                    //
                    if (installedSolution.JobExists)
                    {
                        RemoveExistingJob(installedSolution);
                    }

                    log.Info("***** SOLUTION RETRACTION *****");
                    if (installedSolution.ContainsWebApplicationResource)
                    {
                        installedSolution.Retract(GetImmediateJobTime(), applications);
                    }
                    else
                    {
                        installedSolution.Retract(GetImmediateJobTime());
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Command for creating an upgrade job.
        /// </summary>
        private class CreateUpgradeJobCommand : JobCommand
        {
            internal CreateUpgradeJobCommand(InstallProcessControl parent)
                : base(parent)
            {
            }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.createUpgradeJobCommand;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    string filename = InstallConfiguration.SolutionFile;
                    if (String.IsNullOrEmpty(filename))
                    {
                        throw new InstallException(CommonUIStrings.installExceptionConfigurationNoWsp);
                    }

                    SPSolution installedSolution = SPFarm.Local.Solutions[InstallConfiguration.SolutionId];

                    //
                    // Remove existing job, if any. 
                    //
                    if (installedSolution.JobExists)
                    {
                        RemoveExistingJob(installedSolution);
                    }

                    log.Info(CommonUIStrings.logUpgrade);
                    installedSolution.Upgrade(filename, GetImmediateJobTime());
                    return true;
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Command for creating a retraction job.
        /// </summary>
        private class CreateRetractionJobCommand : JobCommand
        {
            internal CreateRetractionJobCommand(InstallProcessControl parent)
                : base(parent)
            {
            }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.createRetractionJobCommand;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    SPSolution installedSolution = SPFarm.Local.Solutions[InstallConfiguration.SolutionId];

                    //
                    // Remove existing job, if any. 
                    //
                    if (installedSolution.JobExists)
                    {
                        RemoveExistingJob(installedSolution);
                    }

                    if (installedSolution.Deployed)
                    {
                        log.Info(CommonUIStrings.logRetract);
                        if (installedSolution.ContainsWebApplicationResource)
                        {
                            Collection<SPWebApplication> applications = installedSolution.DeployedWebApplications;
                            installedSolution.Retract(GetImmediateJobTime(), applications);
                        }
                        else
                        {
                            installedSolution.Retract(GetImmediateJobTime());
                        }
                    }
                    return true;
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        private class WaitForJobCompletionCommand : Command
        {
            private readonly string description;
            private DateTime startTime;
            private bool first = true;

            internal WaitForJobCompletionCommand(InstallProcessControl parent, string description)
                : base(parent)
            {
                this.description = description;
            }

            internal override string Description
            {
                get
                {
                    return description;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    SPSolution installedSolution = SPFarm.Local.Solutions[InstallConfiguration.SolutionId];

                    if (first)
                    {
                        if (!installedSolution.JobExists) return true;
                        startTime = DateTime.Now;
                        first = false;
                    }

                    //
                    // Wait for job to end
                    //
                    if (installedSolution.JobExists)
                    {
                        if (DateTime.Now > startTime.Add(JobTimeout))
                        {
                            throw new InstallException(CommonUIStrings.installExceptionTimeout);
                        }

                        return false;
                    }
                    else
                    {
                        log.Info(installedSolution.LastOperationDetails);

                        SPSolutionOperationResult result = installedSolution.LastOperationResult;
                        if (result != SPSolutionOperationResult.DeploymentSucceeded && result != SPSolutionOperationResult.RetractionSucceeded)
                        {
                            throw new InstallException(installedSolution.LastOperationDetails);
                        }

                        return true;
                    }
                }

                catch (Exception ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }

            protected internal override bool Rollback()
            {
                SPSolution installedSolution = SPFarm.Local.Solutions[InstallConfiguration.SolutionId];

                //
                // Wait for job to end
                //
                if (installedSolution != null)
                {
                    if (installedSolution.JobExists)
                    {
                        if (DateTime.Now > startTime.Add(JobTimeout))
                        {
                            throw new InstallException(CommonUIStrings.installExceptionTimeout);
                        }
                        return false;
                    }
                    else
                    {
                        log.Info(installedSolution.LastOperationDetails);
                    }
                }

                return true;
            }
        }

        private abstract class FeatureCommand : Command
        {
            protected FeatureCommand(InstallProcessControl parent) : base(parent) { }

            protected static void DeactivateFeature(List<Guid?> featureIds)
            {
                try
                {
                    if (featureIds != null && featureIds.Count > 0)
                    {
                        foreach (Guid? featureId in featureIds)
                        {
                            if (featureId != null)
                            {
                                SPFeature feature = SPWebService.AdministrationService.Features[featureId.Value];
                                if (feature != null)
                                {
                                    SPWebService.AdministrationService.Features.Remove(featureId.Value);
                                }
                            }
                        }
                    }
                }

                catch (ArgumentException ex)  // Missing assembly in GAC
                {
                    log.Warn(ex.Message, ex);
                }

                catch (InvalidOperationException ex)  // Missing receiver class
                {
                    log.Warn(ex.Message, ex);
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        private class ActivateFarmFeatureCommand : FeatureCommand
        {
            internal ActivateFarmFeatureCommand(InstallProcessControl parent) : base(parent) { }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.activateFarmFeatureCommand;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    List<Guid?> featureIds = InstallConfiguration.FeatureId;
                    if (featureIds != null && featureIds.Count > 0)
                    {
                        foreach (Guid? featureId in featureIds)
                        {
                            if (featureId != null)
                            {
                                SPFeature feature = SPWebService.AdministrationService.Features.Add(featureId.Value, true);
                            }
                        }
                    }
                    return true;
                }

                catch (Exception ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }

            protected internal override bool Rollback()
            {
                DeactivateFeature(InstallConfiguration.FeatureId);
                return true;
            }
        }

        private class DeactivateFarmFeatureCommand : FeatureCommand
        {
            internal DeactivateFarmFeatureCommand(InstallProcessControl parent) : base(parent) { }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.deactivateFarmFeatureCommand;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    List<Guid?> featureIds = InstallConfiguration.FeatureId;
                    if (featureIds != null && featureIds.Count > 0)
                    {
                        foreach (Guid? featureId in featureIds)
                        {
                            if (featureId != null && SPWebService.AdministrationService.Features[featureId.Value] != null)
                            {
                                SPWebService.AdministrationService.Features.Remove(featureId.Value);
                            }
                        }
                    }

                    return true;
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }

                return true;
            }
        }

        private abstract class SiteCollectionFeatureCommand : Command
        {
            internal SiteCollectionFeatureCommand(InstallProcessControl parent) : base(parent) { }

            protected static void DeactivateFeature(IList<SPSite> siteCollections, List<Guid?> featureIds)
            {
                try
                {
                    if (siteCollections != null && featureIds != null && featureIds.Count > 0)
                    {
                        log.Info(CommonUIStrings.logFeatureDeactivate);
                        foreach (SPSite siteCollection in siteCollections)
                        {
                            foreach (Guid? featureId in featureIds)
                            {
                                if (featureId == null) continue;

                                SPFeature feature = siteCollection.Features[featureId.Value];
                                if (feature == null) continue;

                                siteCollection.Features.Remove(featureId.Value);
                                log.Info(siteCollection.Url + " : " + featureId.Value.ToString());
                            }
                        }
                    }
                }

                catch (ArgumentException ex)
                {
                    log.Warn(ex.Message, ex);
                }

                catch (InvalidOperationException ex)
                {
                    log.Warn(ex.Message, ex);
                }

                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        private class ActivateSiteCollectionFeatureCommand : SiteCollectionFeatureCommand
        {
            private readonly IList<SPSite> siteCollections;

            internal ActivateSiteCollectionFeatureCommand(InstallProcessControl parent, IList<SPSite> siteCollections)
                : base(parent)
            {
                this.siteCollections = siteCollections;
            }

            internal override string Description
            {
                get
                {
                    return String.Format(CommonUIStrings.activateSiteCollectionFeatureCommand, siteCollections.Count, siteCollections.Count == 1 ? String.Empty : "s");
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    List<Guid?> featureIds = InstallConfiguration.FeatureId;
                    if (siteCollections != null && featureIds != null && featureIds.Count > 0)
                    {
                        log.Info(CommonUIStrings.logFeatureActivate);
                        foreach (SPSite siteCollection in siteCollections)
                        {
                            foreach (Guid? featureId in featureIds)
                            {
                                if (featureId == null) continue;

                                SPFeature feature = siteCollection.Features.Add(featureId.Value, true);
                                log.Info(siteCollection.Url + " : " + featureId.Value.ToString());
                            }
                        }
                    }

                    return true;
                }

                catch (Exception ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }

            protected internal override bool Rollback()
            {
                DeactivateFeature(siteCollections, InstallConfiguration.FeatureId);
                return true;
            }
        }

        private class DeactivateSiteCollectionFeatureCommand : SiteCollectionFeatureCommand
        {
            private List<SPSite> deactivatedSiteCollections;

            internal DeactivateSiteCollectionFeatureCommand(InstallProcessControl parent)
                : base(parent)
            {
                deactivatedSiteCollections = new List<SPSite>();
            }

            public List<SPSite> DeactivatedSiteCollections
            {
                get { return deactivatedSiteCollections; }
            }

            internal override string Description
            {
                get { return CommonUIStrings.deactivateSiteCollectionFeatureCommand; }
            }

            protected internal override bool Execute()
            {
                try
                {
                    List<Guid?> featureIds = InstallConfiguration.FeatureId;

                    SPFarm farm = SPFarm.Local;
                    SPSolution solution = farm.Solutions[InstallConfiguration.SolutionId];
                    if (solution != null && solution.Deployed && featureIds != null && featureIds.Count > 0)
                    {
                        log.Info(CommonUIStrings.logFeatureDeactivate);

                        foreach (SPWebApplication webApp in SPWebService.AdministrationService.WebApplications)
                        {
                            DeactivateFeatures(webApp);
                        }

                        foreach (SPWebApplication webApp in SPWebService.ContentService.WebApplications)
                        {
                            DeactivateFeatures(webApp);
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                }

                return true;
            }

            private void DeactivateFeatures(SPWebApplication webApp)
            {
                List<Guid?> featureIds = InstallConfiguration.FeatureId;

                foreach (SPSite siteCollection in webApp.Sites)
                {
                    foreach (Guid? featureId in featureIds)
                    {
                        if (featureId == null) continue;
                        if (siteCollection.Features[featureId.Value] == null) continue;

                        log.Info(siteCollection.Url + " : " + featureId.Value.ToString());

                        siteCollection.Features.Remove(featureId.Value);
                    }
                    siteCollection.Dispose();
                }
            }
        }

        /// <summary>
        /// Command that registers the version number of a solution.
        /// </summary>
        private class RegisterVersionNumberCommand : Command
        {
            private Version oldVersion;

            internal RegisterVersionNumberCommand(InstallProcessControl parent) : base(parent) { }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.registerVersionNumberCommand;
                }
            }

            protected internal override bool Execute()
            {
                oldVersion = InstallConfiguration.InstalledVersion;
                InstallConfiguration.InstalledVersion = InstallConfiguration.SolutionVersion;
                return true;
            }

            protected internal override bool Rollback()
            {
                InstallConfiguration.InstalledVersion = oldVersion;
                return true;
            }
        }

        /// <summary>
        /// Command that unregisters the version number of a solution.
        /// </summary>
        private class UnregisterVersionNumberCommand : Command
        {
            internal UnregisterVersionNumberCommand(InstallProcessControl parent) : base(parent) { }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.unregisterVersionNumberCommand;
                }
            }

            protected internal override bool Execute()
            {
                InstallConfiguration.InstalledVersion = null;
                return true;
            }
        }

        /// <summary>
        /// Command that configuring Connection String in Web Application Config File.
        /// </summary>
        private class ConfigureConnectionStringCommand : Command
        {

            private Collection<SPWebApplication> applications;

            internal ConfigureConnectionStringCommand(InstallProcessControl parent, IList<SPWebApplication> applications)
                : base(parent)
            {
                if (applications != null)
                {
                    this.applications = new Collection<SPWebApplication>();
                    foreach (SPWebApplication application in applications)
                    {
                        this.applications.Add(application);
                    }
                }
                else
                {
                    this.applications = null;
                }
            }

            internal override string Description
            {
                get
                {
                    return CommonUIStrings.configureConnectionStringCommand;
                }
            }

            protected internal override bool Execute()
            {
                try
                {
                    foreach (SPWebApplication application in applications)
                    {
                        if (!application.IsAdministrationWebApplication)
                        {
                            WebConfigModifier.EnsureChildNode(application);
                        }
                    }
                    return true;
                }
                catch (SPException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
                catch (SqlException ex)
                {
                    throw new InstallException(ex.Message, ex);
                }
            }
        }

        protected string  GetConnetionString()
        {
            try
            {

                string conString = string.Empty;

                foreach (SPWebApplication webApp in SPWebService.ContentService.WebApplications)
                {
                    if (!webApp.IsAdministrationWebApplication)
                    {
                        conString=WebConfigModifier.getConnectionString(webApp);
                    }
                }
                return conString;
            }
            catch (SPException ex)
            {
                throw new InstallException(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                throw new InstallException(ex.Message, ex);
            }
        }

        #endregion

        #region ILog Wrapper

        private class MessageList : List<string>
        {
        }

        private class MessageCollector : ILog
        {
            private readonly ILog wrappee;
            private readonly MessageList messages = new MessageList();

            internal MessageCollector(ILog wrappee)
            {
                this.wrappee = wrappee;
            }

            public MessageList Messages
            {
                get { return messages; }
            }

            public void Info(object message)
            {
                messages.Add(message.ToString());
                wrappee.Info(message);
            }

            public void Info(object message, Exception t)
            {
                messages.Add(message.ToString());
                messages.Add(t.ToString());
                wrappee.Info(message, t);
            }

            public void Warn(object message)
            {
                wrappee.Warn(message);
            }

            public void Warn(object message, Exception t)
            {
                wrappee.Warn(message, t);
            }

            public void Error(object message)
            {
                messages.Add(message.ToString());
                wrappee.Error(message);
            }

            public void Error(object message, Exception t)
            {
                messages.Add(message.ToString());
                messages.Add(t.ToString());
                wrappee.Error(message, t);
            }

            public void Fatal(object message)
            {
                wrappee.Fatal(message);
            }

            public void Fatal(object message, Exception t)
            {
                wrappee.Fatal(message, t);
            }
        }

        #endregion
    }
}
