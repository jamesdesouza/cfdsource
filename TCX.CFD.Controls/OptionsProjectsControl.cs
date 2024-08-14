using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsProjectsControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblProjectsDefaultFolder;

	private TextBox txtDefaultProjectsFolder;

	private Button browseDefaultProjectsFolderButton;

	private FolderBrowserDialog folderBrowserDialog;

	private ErrorProvider errorProvider;

	private ToolTip helpToolTip;

	private Label lblGeneral;

	private Label lblBuild;

	private Label lblMaxRecentProjects;

	private MaskedTextBox txtMaxRecentProjects;

	private Label lblRecentProjects;

	private Label lblMaxErrorsBuildingProject;

	private MaskedTextBox txtMaxErrorsBuildingProject;

	public OptionsProjectsControl()
	{
		InitializeComponent();
		txtDefaultProjectsFolder.Text = (string.IsNullOrEmpty(Settings.Default.DefaultProjectsFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.DefaultProjectsFolder);
		txtMaxErrorsBuildingProject.Text = Settings.Default.MaxErrorsBuildingProject.ToString();
		txtMaxRecentProjects.Text = Settings.Default.MaxRecentProjects.ToString();
		lblGeneral.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.lblGeneral.Text");
		lblProjectsDefaultFolder.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.lblProjectsDefaultFolder.Text");
		browseDefaultProjectsFolderButton.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.browseDefaultProjectsFolderButton.Text");
		lblBuild.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.lblBuild.Text");
		lblMaxErrorsBuildingProject.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.lblMaxErrorsBuildingProject.Text");
		lblRecentProjects.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.lblRecentProjects.Text");
		lblMaxRecentProjects.Text = LocalizedResourceMgr.GetString("OptionsProjectsControl.lblMaxRecentProjects.Text");
		helpToolTip.SetToolTip(txtDefaultProjectsFolder, LocalizedResourceMgr.GetString("OptionsProjectsControl.txtDefaultProjectsFolder.HelpText"));
		helpToolTip.SetToolTip(txtMaxErrorsBuildingProject, LocalizedResourceMgr.GetString("OptionsProjectsControl.txtMaxErrorsBuildingProject.HelpText"));
		helpToolTip.SetToolTip(txtMaxRecentProjects, LocalizedResourceMgr.GetString("OptionsProjectsControl.txtMaxRecentProjects.HelpText"));
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void BrowseDefaultProjectsFolderButton_Click(object sender, EventArgs e)
	{
		folderBrowserDialog.SelectedPath = txtDefaultProjectsFolder.Text;
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			txtDefaultProjectsFolder.Text = folderBrowserDialog.SelectedPath;
		}
	}

	private void ValidateDefaultProjectsFolder()
	{
		if (!string.IsNullOrEmpty(txtDefaultProjectsFolder.Text) && !Directory.Exists(txtDefaultProjectsFolder.Text))
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsProjectsControl.Error.ProjectsFolderDoesNotExist"), txtDefaultProjectsFolder.Text));
		}
	}

	private void ValidateMaxErrorsBuildingProject()
	{
		if (string.IsNullOrEmpty(txtMaxErrorsBuildingProject.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsProjectsControl.Error.MaxErrorsBuildingProjectIsMandatory"));
		}
		int num = Convert.ToInt32(txtMaxErrorsBuildingProject.Text);
		if (num < 1 || num > 999)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsProjectsControl.Error.InvalidMaxErrorsBuildingProjectValue"), 1, 999));
		}
	}

	private void ValidateMaxRecentProjects()
	{
		if (string.IsNullOrEmpty(txtMaxRecentProjects.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsProjectsControl.Error.MaxRecentProjectsIsMandatory"));
		}
		int num = Convert.ToInt32(txtMaxRecentProjects.Text);
		if (num < 1 || num > 99)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsProjectsControl.Error.InvalidMaxRecentProjectsValue"), 1, 99));
		}
	}

	private void ValidateFields()
	{
		ValidateDefaultProjectsFolder();
		ValidateMaxErrorsBuildingProject();
		ValidateMaxRecentProjects();
	}

	private void TxtDefaultProjectsFolder_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateDefaultProjectsFolder();
			errorProvider.SetError(txtDefaultProjectsFolder, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtDefaultProjectsFolder, ErrorHelper.GetErrorDescription(exc));
		}
	}

	private void TxtMaxErrorsBuildingProject_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMaxErrorsBuildingProject();
			errorProvider.SetError(txtMaxErrorsBuildingProject, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtMaxErrorsBuildingProject, ErrorHelper.GetErrorDescription(exc));
		}
	}

	private void TxtMaxRecentProjects_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMaxRecentProjects();
			errorProvider.SetError(txtMaxRecentProjects, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtMaxRecentProjects, ErrorHelper.GetErrorDescription(exc));
		}
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.DefaultProjectsFolder = txtDefaultProjectsFolder.Text;
		Settings.Default.MaxErrorsBuildingProject = Convert.ToUInt32(txtMaxErrorsBuildingProject.Text);
		Settings.Default.MaxRecentProjects = Convert.ToUInt32(txtMaxRecentProjects.Text);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.lblProjectsDefaultFolder = new System.Windows.Forms.Label();
		this.txtDefaultProjectsFolder = new System.Windows.Forms.TextBox();
		this.browseDefaultProjectsFolderButton = new System.Windows.Forms.Button();
		this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.helpToolTip = new System.Windows.Forms.ToolTip(this.components);
		this.lblGeneral = new System.Windows.Forms.Label();
		this.lblBuild = new System.Windows.Forms.Label();
		this.lblMaxErrorsBuildingProject = new System.Windows.Forms.Label();
		this.txtMaxErrorsBuildingProject = new System.Windows.Forms.MaskedTextBox();
		this.lblRecentProjects = new System.Windows.Forms.Label();
		this.lblMaxRecentProjects = new System.Windows.Forms.Label();
		this.txtMaxRecentProjects = new System.Windows.Forms.MaskedTextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblProjectsDefaultFolder.AutoSize = true;
		this.lblProjectsDefaultFolder.Location = new System.Drawing.Point(9, 38);
		this.lblProjectsDefaultFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblProjectsDefaultFolder.Name = "lblProjectsDefaultFolder";
		this.lblProjectsDefaultFolder.Size = new System.Drawing.Size(152, 17);
		this.lblProjectsDefaultFolder.TabIndex = 1;
		this.lblProjectsDefaultFolder.Text = "Default Projects Folder";
		this.txtDefaultProjectsFolder.Location = new System.Drawing.Point(12, 59);
		this.txtDefaultProjectsFolder.Margin = new System.Windows.Forms.Padding(4);
		this.txtDefaultProjectsFolder.MaxLength = 1024;
		this.txtDefaultProjectsFolder.Name = "txtDefaultProjectsFolder";
		this.txtDefaultProjectsFolder.Size = new System.Drawing.Size(742, 22);
		this.txtDefaultProjectsFolder.TabIndex = 2;
		this.txtDefaultProjectsFolder.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtDefaultProjectsFolder.Validating += new System.ComponentModel.CancelEventHandler(TxtDefaultProjectsFolder_Validating);
		this.browseDefaultProjectsFolderButton.Location = new System.Drawing.Point(12, 89);
		this.browseDefaultProjectsFolderButton.Margin = new System.Windows.Forms.Padding(4);
		this.browseDefaultProjectsFolderButton.Name = "browseDefaultProjectsFolderButton";
		this.browseDefaultProjectsFolderButton.Size = new System.Drawing.Size(100, 28);
		this.browseDefaultProjectsFolderButton.TabIndex = 3;
		this.browseDefaultProjectsFolderButton.Text = "&Browse";
		this.browseDefaultProjectsFolderButton.UseVisualStyleBackColor = true;
		this.browseDefaultProjectsFolderButton.Click += new System.EventHandler(BrowseDefaultProjectsFolderButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.helpToolTip.AutoPopDelay = 15000;
		this.helpToolTip.InitialDelay = 500;
		this.helpToolTip.ReshowDelay = 100;
		this.helpToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.lblGeneral.AutoSize = true;
		this.lblGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblGeneral.Location = new System.Drawing.Point(8, 8);
		this.lblGeneral.Name = "lblGeneral";
		this.lblGeneral.Size = new System.Drawing.Size(75, 20);
		this.lblGeneral.TabIndex = 0;
		this.lblGeneral.Text = "General";
		this.lblBuild.AutoSize = true;
		this.lblBuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBuild.Location = new System.Drawing.Point(8, 129);
		this.lblBuild.Name = "lblBuild";
		this.lblBuild.Size = new System.Drawing.Size(52, 20);
		this.lblBuild.TabIndex = 4;
		this.lblBuild.Text = "Build";
		this.lblMaxErrorsBuildingProject.AutoSize = true;
		this.lblMaxErrorsBuildingProject.Location = new System.Drawing.Point(9, 159);
		this.lblMaxErrorsBuildingProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxErrorsBuildingProject.Name = "lblMaxErrorsBuildingProject";
		this.lblMaxErrorsBuildingProject.Size = new System.Drawing.Size(178, 17);
		this.lblMaxErrorsBuildingProject.TabIndex = 5;
		this.lblMaxErrorsBuildingProject.Text = "Max Errors Building Project";
		this.txtMaxErrorsBuildingProject.HidePromptOnLeave = true;
		this.txtMaxErrorsBuildingProject.Location = new System.Drawing.Point(12, 180);
		this.txtMaxErrorsBuildingProject.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxErrorsBuildingProject.Mask = "999";
		this.txtMaxErrorsBuildingProject.Name = "txtMaxErrorsBuildingProject";
		this.txtMaxErrorsBuildingProject.Size = new System.Drawing.Size(742, 22);
		this.txtMaxErrorsBuildingProject.TabIndex = 6;
		this.txtMaxErrorsBuildingProject.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxErrorsBuildingProject.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxErrorsBuildingProject_Validating);
		this.lblRecentProjects.AutoSize = true;
		this.lblRecentProjects.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRecentProjects.Location = new System.Drawing.Point(8, 220);
		this.lblRecentProjects.Name = "lblRecentProjects";
		this.lblRecentProjects.Size = new System.Drawing.Size(144, 20);
		this.lblRecentProjects.TabIndex = 7;
		this.lblRecentProjects.Text = "Recent Projects";
		this.lblMaxRecentProjects.AutoSize = true;
		this.lblMaxRecentProjects.Location = new System.Drawing.Point(9, 250);
		this.lblMaxRecentProjects.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRecentProjects.Name = "lblMaxRecentProjects";
		this.lblMaxRecentProjects.Size = new System.Drawing.Size(207, 17);
		this.lblMaxRecentProjects.TabIndex = 8;
		this.lblMaxRecentProjects.Text = "Max Number of Recent Projects";
		this.txtMaxRecentProjects.HidePromptOnLeave = true;
		this.txtMaxRecentProjects.Location = new System.Drawing.Point(12, 271);
		this.txtMaxRecentProjects.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRecentProjects.Mask = "99";
		this.txtMaxRecentProjects.Name = "txtMaxRecentProjects";
		this.txtMaxRecentProjects.Size = new System.Drawing.Size(742, 22);
		this.txtMaxRecentProjects.TabIndex = 9;
		this.txtMaxRecentProjects.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRecentProjects.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRecentProjects_Validating);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblMaxRecentProjects);
		base.Controls.Add(this.txtMaxRecentProjects);
		base.Controls.Add(this.lblRecentProjects);
		base.Controls.Add(this.lblMaxErrorsBuildingProject);
		base.Controls.Add(this.txtMaxErrorsBuildingProject);
		base.Controls.Add(this.lblBuild);
		base.Controls.Add(this.lblGeneral);
		base.Controls.Add(this.browseDefaultProjectsFolderButton);
		base.Controls.Add(this.txtDefaultProjectsFolder);
		base.Controls.Add(this.lblProjectsDefaultFolder);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsProjectsControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
