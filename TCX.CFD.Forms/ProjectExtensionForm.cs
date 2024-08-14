using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class ProjectExtensionForm : Form
{
	private readonly ProjectObject projectObject;

	private IContainer components;

	private Label lblExtension;

	private MaskedTextBox txtExtension;

	private Button cancelButton;

	private Button okButton;

	private Label lblInitialDescription;

	private CheckBox chkDoNotShowAgain;

	public ProjectExtensionForm(ProjectObject projectObject)
	{
		InitializeComponent();
		this.projectObject = projectObject;
		lblInitialDescription.Text = LocalizedResourceMgr.GetString("ProjectExtensionForm.lblInitialDescription.Text");
		chkDoNotShowAgain.Text = LocalizedResourceMgr.GetString("ProjectExtensionForm.chkDoNotShowAgain.Text");
		lblExtension.Text = LocalizedResourceMgr.GetString("ProjectExtensionForm.lblExtension.Text");
		okButton.Text = LocalizedResourceMgr.GetString("ProjectExtensionForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("ProjectExtensionForm.cancelButton.Text");
		txtExtension.Text = projectObject.Extension;
		chkDoNotShowAgain.Checked = projectObject.DoNotAskForExtension;
	}

	private bool ValidateExtension()
	{
		string text = txtExtension.Text.Trim();
		if (string.IsNullOrEmpty(text))
		{
			return true;
		}
		if (text.Length < 2)
		{
			return false;
		}
		uint result;
		return uint.TryParse(text, out result);
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (!ValidateExtension())
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("ProjectExtensionForm.MessageBox.Error.InvalidExtension"), LocalizedResourceMgr.GetString("ProjectExtensionForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtExtension.Focus();
			return;
		}
		projectObject.Extension = txtExtension.Text.Trim();
		projectObject.DoNotAskForExtension = chkDoNotShowAgain.Checked;
		projectObject.Save();
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-building-projects/#h.arsjbl1xz6q5");
	}

	private void ProjectExtensionForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void ProjectExtensionForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		ShowHelp();
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
		this.lblExtension = new System.Windows.Forms.Label();
		this.txtExtension = new System.Windows.Forms.MaskedTextBox();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.lblInitialDescription = new System.Windows.Forms.Label();
		this.chkDoNotShowAgain = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.lblExtension.AutoSize = true;
		this.lblExtension.Location = new System.Drawing.Point(18, 82);
		this.lblExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExtension.Name = "lblExtension";
		this.lblExtension.Size = new System.Drawing.Size(69, 17);
		this.lblExtension.TabIndex = 1;
		this.lblExtension.Text = "Extension";
		this.txtExtension.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExtension.Location = new System.Drawing.Point(95, 79);
		this.txtExtension.Margin = new System.Windows.Forms.Padding(4);
		this.txtExtension.Mask = "99999";
		this.txtExtension.MaximumSize = new System.Drawing.Size(216, 22);
		this.txtExtension.MinimumSize = new System.Drawing.Size(216, 22);
		this.txtExtension.Name = "txtExtension";
		this.txtExtension.Size = new System.Drawing.Size(216, 22);
		this.txtExtension.TabIndex = 2;
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(461, 127);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 5;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(353, 127);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 4;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblInitialDescription.Location = new System.Drawing.Point(19, 13);
		this.lblInitialDescription.Name = "lblInitialDescription";
		this.lblInitialDescription.Size = new System.Drawing.Size(542, 62);
		this.lblInitialDescription.TabIndex = 0;
		this.lblInitialDescription.Text = "You can assign this CFD project to a PBX extension to easily call it from any internal number. To change the assigned extension use the menu \"Build\" > \"Project Extension\".";
		this.chkDoNotShowAgain.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.chkDoNotShowAgain.AutoSize = true;
		this.chkDoNotShowAgain.Location = new System.Drawing.Point(22, 132);
		this.chkDoNotShowAgain.Name = "chkDoNotShowAgain";
		this.chkDoNotShowAgain.Size = new System.Drawing.Size(301, 21);
		this.chkDoNotShowAgain.TabIndex = 3;
		this.chkDoNotShowAgain.Text = "Do not show this dialog during project build";
		this.chkDoNotShowAgain.UseVisualStyleBackColor = true;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(582, 168);
		base.Controls.Add(this.chkDoNotShowAgain);
		base.Controls.Add(this.lblInitialDescription);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.txtExtension);
		base.Controls.Add(this.lblExtension);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(600, 215);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(600, 215);
		base.Name = "ProjectExtensionForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Project Extension";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(ProjectExtensionForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ProjectExtensionForm_HelpRequested);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
