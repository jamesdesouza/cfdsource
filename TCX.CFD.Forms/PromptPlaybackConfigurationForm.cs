using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class PromptPlaybackConfigurationForm : Form
{
	private PromptPlaybackComponent promptPlaybackComponent;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private CheckBox chkAcceptDtmfInput;

	private Button editPromptsButton;

	public bool AcceptDtmfInput => chkAcceptDtmfInput.Checked;

	public List<Prompt> Prompts { get; private set; }

	private void EditPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(promptPlaybackComponent);
		promptCollectionEditorForm.PromptList = Prompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			Prompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	public PromptPlaybackConfigurationForm(PromptPlaybackComponent promptPlaybackComponent)
	{
		InitializeComponent();
		this.promptPlaybackComponent = promptPlaybackComponent;
		Prompts = new List<Prompt>(promptPlaybackComponent.Prompts);
		chkAcceptDtmfInput.Checked = promptPlaybackComponent.AcceptDtmfInput;
		Text = LocalizedResourceMgr.GetString("PromptPlaybackConfigurationForm.Title");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("PromptPlaybackConfigurationForm.chkAcceptDtmfInput.Text");
		editPromptsButton.Text = LocalizedResourceMgr.GetString("PromptPlaybackConfigurationForm.editPromptsButton.Text");
		okButton.Text = LocalizedResourceMgr.GetString("PromptPlaybackConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("PromptPlaybackConfigurationForm.cancelButton.Text");
	}

	private void PromptPlaybackConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		promptPlaybackComponent.ShowHelp();
	}

	private void PromptPlaybackConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		promptPlaybackComponent.ShowHelp();
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.editPromptsButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(219, 94);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 3;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(111, 94);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 2;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(16, 15);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(245, 21);
		this.chkAcceptDtmfInput.TabIndex = 0;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.editPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.editPromptsButton.Location = new System.Drawing.Point(16, 43);
		this.editPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPromptsButton.Name = "editPromptsButton";
		this.editPromptsButton.Size = new System.Drawing.Size(303, 28);
		this.editPromptsButton.TabIndex = 1;
		this.editPromptsButton.Text = "Edit Prompts";
		this.editPromptsButton.UseVisualStyleBackColor = true;
		this.editPromptsButton.Click += new System.EventHandler(EditPromptsButton_Click);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(335, 127);
		base.Controls.Add(this.editPromptsButton);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(634, 174);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(350, 174);
		base.Name = "PromptPlaybackConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Prompt Playback";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(PromptPlaybackConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(PromptPlaybackConfigurationForm_HelpRequested);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
