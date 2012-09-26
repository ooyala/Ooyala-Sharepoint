namespace SharePointInstaller
{
    partial class SQLServerSettings
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
            this.pnlConnectionInfo = new System.Windows.Forms.Panel();
            this.pnlDatabaseConnection = new System.Windows.Forms.GroupBox();
            this.chkNewDB = new System.Windows.Forms.CheckBox();
            this.btnRunScr = new System.Windows.Forms.Button();
            this.txtNewDBName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDbName = new System.Windows.Forms.ComboBox();
            this.lblSelectDbName = new System.Windows.Forms.Label();
            this.pnlLogOn = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblServerName = new System.Windows.Forms.Label();
            this.btnConnection = new System.Windows.Forms.Button();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.rdbServerAuthentication = new System.Windows.Forms.RadioButton();
            this.rdbWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.pnlConnectionInfo.SuspendLayout();
            this.pnlDatabaseConnection.SuspendLayout();
            this.pnlLogOn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConnectionInfo
            // 
            this.pnlConnectionInfo.Controls.Add(this.pnlDatabaseConnection);
            this.pnlConnectionInfo.Controls.Add(this.pnlLogOn);
            this.pnlConnectionInfo.Location = new System.Drawing.Point(0, -17);
            this.pnlConnectionInfo.Name = "pnlConnectionInfo";
            this.pnlConnectionInfo.Size = new System.Drawing.Size(516, 298);
            this.pnlConnectionInfo.TabIndex = 17;
            // 
            // pnlDatabaseConnection
            // 
            this.pnlDatabaseConnection.Controls.Add(this.chkNewDB);
            this.pnlDatabaseConnection.Controls.Add(this.btnRunScr);
            this.pnlDatabaseConnection.Controls.Add(this.txtNewDBName);
            this.pnlDatabaseConnection.Controls.Add(this.label1);
            this.pnlDatabaseConnection.Controls.Add(this.cmbDbName);
            this.pnlDatabaseConnection.Controls.Add(this.lblSelectDbName);
            this.pnlDatabaseConnection.Location = new System.Drawing.Point(23, 162);
            this.pnlDatabaseConnection.Name = "pnlDatabaseConnection";
            this.pnlDatabaseConnection.Size = new System.Drawing.Size(481, 125);
            this.pnlDatabaseConnection.TabIndex = 5;
            this.pnlDatabaseConnection.TabStop = false;
            this.pnlDatabaseConnection.Text = "Connect to a database";
            // 
            // chkNewDB
            // 
            this.chkNewDB.AutoSize = true;
            this.chkNewDB.Location = new System.Drawing.Point(101, 18);
            this.chkNewDB.Name = "chkNewDB";
            this.chkNewDB.Size = new System.Drawing.Size(97, 17);
            this.chkNewDB.TabIndex = 10;
            this.chkNewDB.Text = "New Database";
            this.chkNewDB.UseVisualStyleBackColor = true;
            this.chkNewDB.CheckedChanged += new System.EventHandler(this.chkNewDB_CheckedChanged);
            // 
            // btnRunScr
            // 
            this.btnRunScr.Location = new System.Drawing.Point(385, 74);
            this.btnRunScr.Name = "btnRunScr";
            this.btnRunScr.Size = new System.Drawing.Size(75, 26);
            this.btnRunScr.TabIndex = 9;
            this.btnRunScr.Text = "Create";
            this.btnRunScr.UseVisualStyleBackColor = true;
            this.btnRunScr.Click += new System.EventHandler(this.btnRunScr_Click);
            // 
            // txtNewDBName
            // 
            this.txtNewDBName.Enabled = false;
            this.txtNewDBName.Location = new System.Drawing.Point(100, 80);
            this.txtNewDBName.Name = "txtNewDBName";
            this.txtNewDBName.Size = new System.Drawing.Size(268, 20);
            this.txtNewDBName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(19, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "New DB Name";
            // 
            // cmbDbName
            // 
            this.cmbDbName.AllowDrop = true;
            this.cmbDbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDbName.FormattingEnabled = true;
            this.cmbDbName.Location = new System.Drawing.Point(100, 47);
            this.cmbDbName.Name = "cmbDbName";
            this.cmbDbName.Size = new System.Drawing.Size(268, 21);
            this.cmbDbName.TabIndex = 1;
            // 
            // lblSelectDbName
            // 
            this.lblSelectDbName.AutoSize = true;
            this.lblSelectDbName.Location = new System.Drawing.Point(4, 52);
            this.lblSelectDbName.Name = "lblSelectDbName";
            this.lblSelectDbName.Size = new System.Drawing.Size(93, 13);
            this.lblSelectDbName.TabIndex = 0;
            this.lblSelectDbName.Text = "Select existing DB";
            // 
            // pnlLogOn
            // 
            this.pnlLogOn.Controls.Add(this.txtPassword);
            this.pnlLogOn.Controls.Add(this.lblServerName);
            this.pnlLogOn.Controls.Add(this.btnConnection);
            this.pnlLogOn.Controls.Add(this.cmbServerName);
            this.pnlLogOn.Controls.Add(this.btnRefresh);
            this.pnlLogOn.Controls.Add(this.txtUserName);
            this.pnlLogOn.Controls.Add(this.lblPassword);
            this.pnlLogOn.Controls.Add(this.lblUserName);
            this.pnlLogOn.Controls.Add(this.rdbServerAuthentication);
            this.pnlLogOn.Controls.Add(this.rdbWindowsAuthentication);
            this.pnlLogOn.Location = new System.Drawing.Point(24, 26);
            this.pnlLogOn.Name = "pnlLogOn";
            this.pnlLogOn.Size = new System.Drawing.Size(481, 128);
            this.pnlLogOn.TabIndex = 4;
            this.pnlLogOn.TabStop = false;
            this.pnlLogOn.Text = "Log on to the Server";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(100, 95);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(268, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(19, 26);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(75, 13);
            this.lblServerName.TabIndex = 1;
            this.lblServerName.Text = "Server Name :";
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(384, 89);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(75, 26);
            this.btnConnection.TabIndex = 6;
            this.btnConnection.Text = "Connect";
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // cmbServerName
            // 
            this.cmbServerName.FormattingEnabled = true;
            this.cmbServerName.Location = new System.Drawing.Point(99, 22);
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.Size = new System.Drawing.Size(268, 21);
            this.cmbServerName.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(384, 19);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(100, 72);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(268, 20);
            this.txtUserName.TabIndex = 4;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(44, 98);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(39, 75);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(58, 13);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "User name";
            // 
            // rdbServerAuthentication
            // 
            this.rdbServerAuthentication.AutoSize = true;
            this.rdbServerAuthentication.Location = new System.Drawing.Point(265, 49);
            this.rdbServerAuthentication.Name = "rdbServerAuthentication";
            this.rdbServerAuthentication.Size = new System.Drawing.Size(149, 17);
            this.rdbServerAuthentication.TabIndex = 1;
            this.rdbServerAuthentication.Text = "Use Server Authentication";
            this.rdbServerAuthentication.UseVisualStyleBackColor = true;
            this.rdbServerAuthentication.CheckedChanged += new System.EventHandler(this.rdbServerAuthentication_Click);
            // 
            // rdbWindowsAuthentication
            // 
            this.rdbWindowsAuthentication.AutoSize = true;
            this.rdbWindowsAuthentication.Checked = true;
            this.rdbWindowsAuthentication.Location = new System.Drawing.Point(97, 49);
            this.rdbWindowsAuthentication.Name = "rdbWindowsAuthentication";
            this.rdbWindowsAuthentication.Size = new System.Drawing.Size(162, 17);
            this.rdbWindowsAuthentication.TabIndex = 0;
            this.rdbWindowsAuthentication.TabStop = true;
            this.rdbWindowsAuthentication.Text = "Use Windows Authentication";
            this.rdbWindowsAuthentication.UseVisualStyleBackColor = true;
            this.rdbWindowsAuthentication.CheckedChanged += new System.EventHandler(this.rdbWindowsAuthentication_Click);
            // 
            // SQLServerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlConnectionInfo);
            this.Name = "SQLServerSettings";
            this.Size = new System.Drawing.Size(516, 298);
            this.Load += new System.EventHandler(this.SQLServerSettings_Load);
            this.pnlConnectionInfo.ResumeLayout(false);
            this.pnlDatabaseConnection.ResumeLayout(false);
            this.pnlDatabaseConnection.PerformLayout();
            this.pnlLogOn.ResumeLayout(false);
            this.pnlLogOn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pnlLogOn;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.ComboBox cmbServerName;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.RadioButton rdbServerAuthentication;
        private System.Windows.Forms.RadioButton rdbWindowsAuthentication;
        private System.Windows.Forms.GroupBox pnlDatabaseConnection;
        private System.Windows.Forms.CheckBox chkNewDB;
        private System.Windows.Forms.Button btnRunScr;
        private System.Windows.Forms.TextBox txtNewDBName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDbName;
        private System.Windows.Forms.Label lblSelectDbName;
        private System.Windows.Forms.Panel pnlConnectionInfo;

    }
}
