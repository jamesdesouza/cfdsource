using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class ImportProjectFromBuildOutputWizardForm : Form
{
	private int selectedPage;

	private readonly List<UserControl> wizardPages = new List<UserControl>();

	private readonly ImportProjectFromBuildOutputWizardPage1 page1;

	private readonly ImportProjectFromBuildOutputWizardPage2 page2;

	private IContainer components;

	private Panel controlsPanel;

	private Button backButton;

	private Button nextButton;

	private Button cancelButton;

	public string ProjectFilePath { get; private set; } = "";


	private void ChangeVisibility()
	{
		for (int i = 0; i < wizardPages.Count; i++)
		{
			UserControl userControl = wizardPages[i];
			if (i == selectedPage)
			{
				userControl.Visible = true;
				(userControl as IWizardPage).FocusFirstControl();
			}
			else
			{
				userControl.Visible = false;
			}
		}
	}

	private void CheckButtons()
	{
		backButton.Visible = selectedPage > 0;
		nextButton.Text = ((selectedPage < wizardPages.Count - 1) ? LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.nextButton.Text") : LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.finishButton.Text"));
	}

	public ImportProjectFromBuildOutputWizardForm()
	{
		InitializeComponent();
		Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.Title");
		backButton.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.backButton.Text");
		nextButton.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.nextButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.cancelButton.Text");
		page1 = new ImportProjectFromBuildOutputWizardPage1
		{
			Dock = DockStyle.Fill
		};
		wizardPages.Add(page1);
		page2 = new ImportProjectFromBuildOutputWizardPage2
		{
			Dock = DockStyle.Fill
		};
		wizardPages.Add(page2);
		Control.ControlCollection controls = controlsPanel.Controls;
		Control[] controls2 = wizardPages.ToArray();
		controls.AddRange(controls2);
		ChangeVisibility();
		CheckButtons();
	}

	private void BackButton_Click(object sender, EventArgs e)
	{
		try
		{
			selectedPage--;
			ChangeVisibility();
			CheckButtons();
		}
		catch (Exception ex)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.MessageBox.Error.Back") + ex.ToString(), LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void NextButton_Click(object sender, EventArgs e)
	{
		try
		{
			nextButton.Focus();
			if ((wizardPages[selectedPage] as IWizardPage).ValidateBeforeMovingToNext())
			{
				if (selectedPage == wizardPages.Count - 1)
				{
					ProjectGenerator projectGenerator = new ProjectGenerator(page1.ScriptCode, page2.OutputFolder);
					ProjectFilePath = projectGenerator.Generate();
					base.DialogResult = DialogResult.OK;
					Close();
				}
				else
				{
					selectedPage++;
					ChangeVisibility();
					CheckButtons();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.MessageBox.Error.Next") + ex.ToString(), LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-building-projects/#h.9suzlhr4nikv");
	}

	private void ImportProjectFromBuildOutputWizardForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void ImportProjectFromBuildOutputWizardForm_HelpRequested(object sender, HelpEventArgs hlpevent)
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
		this.controlsPanel = new System.Windows.Forms.Panel();
		this.backButton = new System.Windows.Forms.Button();
		this.nextButton = new System.Windows.Forms.Button();
		this.cancelButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.controlsPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.controlsPanel.BackColor = System.Drawing.SystemColors.Control;
		this.controlsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.controlsPanel.Location = new System.Drawing.Point(-1, -1);
		this.controlsPanel.Margin = new System.Windows.Forms.Padding(4);
		this.controlsPanel.Name = "controlsPanel";
		this.controlsPanel.Size = new System.Drawing.Size(955, 603);
		this.controlsPanel.TabIndex = 0;
		this.backButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.backButton.Location = new System.Drawing.Point(596, 610);
		this.backButton.Margin = new System.Windows.Forms.Padding(4);
		this.backButton.Name = "backButton";
		this.backButton.Size = new System.Drawing.Size(100, 28);
		this.backButton.TabIndex = 1;
		this.backButton.Text = "< &Back";
		this.backButton.UseVisualStyleBackColor = true;
		this.backButton.Click += new System.EventHandler(BackButton_Click);
		this.nextButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.nextButton.Location = new System.Drawing.Point(704, 610);
		this.nextButton.Margin = new System.Windows.Forms.Padding(4);
		this.nextButton.Name = "nextButton";
		this.nextButton.Size = new System.Drawing.Size(100, 28);
		this.nextButton.TabIndex = 2;
		this.nextButton.Text = "&Next >";
		this.nextButton.UseVisualStyleBackColor = true;
		this.nextButton.Click += new System.EventHandler(NextButton_Click);
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(836, 610);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 3;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		base.AcceptButton = this.nextButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(952, 653);
		base.Controls.Add(this.controlsPanel);
		base.Controls.Add(this.backButton);
		base.Controls.Add(this.nextButton);
		base.Controls.Add(this.cancelButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(970, 700);
		base.Name = "ImportProjectFromBuildOutputWizardForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Import Project from Build Output";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(ImportProjectFromBuildOutputWizardForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ImportProjectFromBuildOutputWizardForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
