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

public class SpeechToTextHintsEditorForm : Form
{
	private readonly IVadActivity component;

	private readonly VoiceInputHintsEditorControl editorControl;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Panel editorPanel;

	public List<string> Hints
	{
		get
		{
			return editorControl.Hints;
		}
		set
		{
			editorControl.Hints = value;
		}
	}

	public SpeechToTextHintsEditorForm(IVadActivity component, string languageCode)
	{
		InitializeComponent();
		this.component = component;
		editorControl = new VoiceInputHintsEditorControl(languageCode);
		editorControl.Dock = DockStyle.Fill;
		editorPanel.Controls.Add(editorControl);
		Text = LocalizedResourceMgr.GetString("SpeechToTextHintsEditorForm.Text");
		okButton.Text = LocalizedResourceMgr.GetString("SpeechToTextHintsEditorForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("SpeechToTextHintsEditorForm.cancelButton.Text");
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

	private void SpeechToTextHintsEditorForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		component.ShowHelp();
	}

	private void SpeechToTextHintsEditorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		component.ShowHelp();
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
		this.editorPanel = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.Location = new System.Drawing.Point(720, 334);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 8;
		this.cancelButton.Text = "Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(612, 334);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 7;
		this.okButton.Text = "OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.editorPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.editorPanel.Location = new System.Drawing.Point(0, 0);
		this.editorPanel.Name = "editorPanel";
		this.editorPanel.Size = new System.Drawing.Size(820, 327);
		this.editorPanel.TabIndex = 9;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(836, 377);
		base.Controls.Add(this.editorPanel);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.cancelButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(854, 424);
		base.Name = "SpeechToTextHintsEditorForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Speech to Text Hints Editor";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(SpeechToTextHintsEditorForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(SpeechToTextHintsEditorForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
