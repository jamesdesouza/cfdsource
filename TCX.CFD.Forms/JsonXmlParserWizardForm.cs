using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class JsonXmlParserWizardForm : Form
{
	private readonly JsonXmlParserComponent jsonXmlParserComponent;

	private bool wizardFinished;

	private int selectedPage;

	private readonly List<UserControl> wizardPages = new List<UserControl>();

	private IContainer components;

	private Panel controlsPanel;

	private Button backButton;

	private Button nextButton;

	private Button cancelButton;

	public TextTypes TextType => (wizardPages[0] as JsonXmlParserWizardPage1).TextType;

	public string Input => (wizardPages[0] as JsonXmlParserWizardPage1).Input;

	public List<ResponseMapping> ResponseMappings => (wizardPages[1] as JsonXmlParserWizardPage2).ResponseMappings;

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
		nextButton.Text = ((selectedPage < wizardPages.Count - 1) ? LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.nextButton.Text") : LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.finishButton.Text"));
	}

	public JsonXmlParserWizardForm(JsonXmlParserComponent jsonXmlParserComponent)
	{
		InitializeComponent();
		this.jsonXmlParserComponent = jsonXmlParserComponent;
		Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.Title");
		backButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.backButton.Text");
		nextButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.nextButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.cancelButton.Text");
		JsonXmlParserWizardPage1 jsonXmlParserWizardPage = new JsonXmlParserWizardPage1(jsonXmlParserComponent)
		{
			Dock = DockStyle.Fill
		};
		wizardPages.Add(jsonXmlParserWizardPage);
		JsonXmlParserWizardPage2 item = new JsonXmlParserWizardPage2(jsonXmlParserComponent, jsonXmlParserWizardPage)
		{
			Dock = DockStyle.Fill
		};
		wizardPages.Add(item);
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
			MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.MessageBox.Error.Back") + ex.ToString(), LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
					wizardFinished = true;
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
			MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.MessageBox.Error.Next") + ex.ToString(), LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void JsonXmlParserWizardForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (!wizardFinished && MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.MessageBox.Question.ExitWizard"), LocalizedResourceMgr.GetString("JsonXmlParserWizardForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			e.Cancel = true;
		}
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void JsonXmlParserWizardForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		jsonXmlParserComponent.ShowHelp();
	}

	private void JsonXmlParserWizardForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		jsonXmlParserComponent.ShowHelp();
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
		base.Name = "JsonXmlParserWizardForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "JSON / XML Parser Wizard";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(JsonXmlParserWizardForm_HelpButtonClicked);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(JsonXmlParserWizardForm_FormClosing);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(JsonXmlParserWizardForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
