using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Diagnostics;
using System.IO;
using SharePointInstaller.Resources;
using Microsoft.SharePoint.Administration;


namespace SharePointInstaller
{
    public partial class SQLServerSettings : InstallerControl
    {
        #region Private Variables
        ServerConnection m_ServerConnection;
        Server m_Server;
        #endregion

        #region Events

        public SQLServerSettings()
        {
            InitializeComponent();
            this.Load += new EventHandler(SQLServerSettings_Load);
        }
        
        private void SQLServerSettings_Load(object sender, EventArgs e)
        {
            InitLoad();
            pnlDatabaseConnection.Enabled = false;
            Form.NextButton.Enabled = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable dtServers = SmoApplication.EnumAvailableSqlServers(false);
            this.cmbServerName.DataSource = dtServers;
            this.cmbServerName.DisplayMember = "Name";
            this.cmbServerName.ValueMember = "Name";
            this.Cursor = Cursors.Default;
        }

        private void rdbServerAuthentication_Click(object sender, EventArgs e)
        {
            this.txtUserName.Enabled = true;
            this.txtPassword.Enabled = true;
        }

        private void rdbWindowsAuthentication_Click(object sender, EventArgs e)
        {
            this.txtUserName.Enabled = false;
            this.txtPassword.Enabled = false;
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // If a server is selected
                string message = this.ConnectDatabase();
                if (string.IsNullOrEmpty(message))
                {
                    //This will populate list of databases on selected server
                    this.ListDatabasesInServer();
                    btnRunScr.Visible = true;
                    pnlDatabaseConnection.Enabled = true;
                }
                else
                {
                    MessageBox.Show(message, "SQL Authetication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "SQL Authetication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.Cursor = Cursors.Default;
        }

        private void cmbServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbDbName.Items.Clear();
            
        }

        private void btnRunScr_Click(object sender, EventArgs e)
        {
            try
            {

                string ServerName = cmbServerName.Text;
                string UserName = txtUserName.Text.Trim();
                string Password = txtPassword.Text.Trim();

                string argument = string.Empty;

                if (chkNewDB.Checked && string.IsNullOrEmpty(txtNewDBName.Text))
                {
                    MessageBox.Show("DB Name should not be empty","Information", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                string filename = InstallConfiguration.DBScriptFile;
                FileInfo dbScriptFileInfo = new FileInfo(filename);
                if (!dbScriptFileInfo.Exists)
                {
                    throw new InstallException(string.Format(CommonUIStrings.installExceptionFileNotFound, filename));
                }
                string filepath = '"' + dbScriptFileInfo.FullName + '"';
                string dbName = string.Empty;string formatedDBName = string.Empty;

                if (txtNewDBName.Text.Trim().Length == 0)
                {
                    dbName = cmbDbName.Text.ToString();
                    formatedDBName = string.Format(@"{0}{1}{2}{3}{4}", '"', '[', cmbDbName.Text.ToString(), ']', '"');
                }
                else
                {
                    dbName = txtNewDBName.Text.Trim();
                    formatedDBName = string.Format(@"{0}{1}{2}{3}{4}", '"', '[', txtNewDBName.Text.Trim(), ']', '"');
                }


                if (rdbWindowsAuthentication.Checked)
                    argument = string.Format(@"-E -S {0} -i {1} -v dbname={2}", ServerName, filepath, formatedDBName);
                else if (rdbServerAuthentication.Checked)
                    argument = string.Format(@"-U {0} -P {1} -S {2} -i {3} -v dbname={4}", UserName, Password, ServerName, filepath, formatedDBName);

                var process = Process.Start("sqlcmd.exe", argument);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                OoyalaConnectStringBuilder.DatabaseName = dbName;
                OoyalaConnectStringBuilder.DBServer = ServerName;
                if (rdbWindowsAuthentication.Checked == true) //Windows Authentication
                {
                    OoyalaConnectStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    OoyalaConnectStringBuilder.IntegratedSecurity = false;
                    OoyalaConnectStringBuilder.User = UserName;
                    OoyalaConnectStringBuilder.Password = Password;
                }                
                Form.NextButton.Enabled = true;
            }
            catch (Exception f)
            {
                MessageBox.Show("An error occurred:"+ f.ToString());
                Form.NextButton.Enabled = false;
            }
        }

        private void chkNewDB_CheckedChanged(object sender, EventArgs e)
        {
            txtNewDBName.Enabled = chkNewDB.Checked;
            cmbDbName.Enabled = !(chkNewDB.Checked);
            Form.NextButton.Enabled = false;
            if(chkNewDB.Checked) 
                btnRunScr.Visible = true;
        }
        #endregion

        #region Private Methods

        private void InitLoad()
        {
            btnRunScr.Visible = false;
            this.pnlLogOn.Enabled = false;
            this.pnlDatabaseConnection.Enabled = false;
            
            this.cmbDbName.Enabled = false;
            this.cmbServerName.Enabled = false;

            Form.NextButton.Enabled = false;            
            //Check for all the available servers
            DataTable dtServers = SmoApplication.EnumAvailableSqlServers(false);

            this.cmbServerName.DataSource = dtServers;
            this.cmbServerName.DisplayMember = "Name";
            this.cmbServerName.ValueMember = "Name";

            this.cmbServerName.Enabled = true;
            this.cmbDbName.Enabled = true;
            this.rdbServerAuthentication.Checked = true;
            this.pnlLogOn.Enabled = true;
            this.pnlDatabaseConnection.Enabled = true;
        }

        public Server DatabaseServer
        {
            get { return this.m_Server; }
        }
        
        private string ConnectDatabase()
        {
            if (!string.IsNullOrEmpty(this.cmbServerName.Text))
            {
                try
                {
                    this.m_ServerConnection = new ServerConnection(this.cmbServerName.Text.ToString());

                    //First Check type of Authentication
                    if (this.rdbWindowsAuthentication.Checked == true)   //Windows Authentication
                    {
                        this.m_ServerConnection.LoginSecure = true;
                        this.m_Server = new Server(this.m_ServerConnection);
                    }
                    else
                    {
                        // Create a new connection to the selected server name
                        this.m_ServerConnection.LoginSecure = false;
                        this.m_ServerConnection.Login = this.txtUserName.Text;       //Login User
                        this.m_ServerConnection.Password = this.txtPassword.Text;    //Login Password
                        this.m_ServerConnection.DatabaseName = this.cmbDbName.Text;  //Database Name
                        // Create a new SQL Server object using the connection we created
                        this.m_Server = new Server(this.m_ServerConnection);
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "No server selected";
        }

        private void ListDatabasesInServer()
        {
            this.cmbDbName.Items.Clear();
            // Loop through the databases list
            foreach (Database db in this.m_Server.Databases)
            {
                //We don't want to be adding the System databases to our list
                //Check if database is system database
                if (!db.IsSystemObject)
                {
                    this.cmbDbName.Items.Add(db.Name); // Add database to combobox
                }
            }

            if (cmbDbName.Items.Count > 0)
                this.cmbDbName.SelectedIndex = 0;
        }

        #endregion      

    }
}
