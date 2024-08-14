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

public class SurveyQuestionCollectionEditorForm : Form
{
	private IVadActivity activity;

	private SurveyQuestionCollectionEditorControl editorControl;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Panel editorPanel;

	public List<SurveyQuestion> SurveyQuestionList
	{
		get
		{
			return editorControl.SurveyQuestionList;
		}
		set
		{
			editorControl.SurveyQuestionList = value;
		}
	}

	public SurveyQuestionCollectionEditorForm(IVadActivity activity)
	{
		InitializeComponent();
		this.activity = activity;
		editorControl = new SurveyQuestionCollectionEditorControl(activity);
		editorControl.Dock = DockStyle.Fill;
		editorPanel.Controls.Add(editorControl);
		Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorForm.Text");
		okButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorForm.cancelButton.Text");
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

	private void SurveyQuestionCollectionEditorForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		activity.ShowHelp();
	}

	private void SurveyQuestionCollectionEditorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		activity.ShowHelp();
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
		base.Name = "SurveyQuestionCollectionEditorForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Survey Question Collection Editor";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(SurveyQuestionCollectionEditorForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(SurveyQuestionCollectionEditorForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
