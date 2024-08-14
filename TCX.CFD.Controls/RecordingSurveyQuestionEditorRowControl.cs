using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Controls;

public class RecordingSurveyQuestionEditorRowControl : AbsSurveyQuestionEditorRowControl
{
	private IVadActivity activity;

	private RecordingSurveyQuestion surveyQuestion;

	private IContainer components;

	private CheckBox chkOfferPlayback;

	private Button preRecordingPromptsButton;

	private Button postRecordingPromptsButton;

	private ComboBox comboKeepRecording;

	private Label lblKeepRecording;

	private ComboBox comboRerecord;

	private Label lblReRecord;

	private MaskedTextBox txtMaxTime;

	private Label lblMaxTime;

	public RecordingSurveyQuestionEditorRowControl(IVadActivity activity, RecordingSurveyQuestion surveyQuestion)
	{
		InitializeComponent();
		this.activity = activity;
		this.surveyQuestion = surveyQuestion;
		chkOfferPlayback.Text = LocalizedResourceMgr.GetString("RecordingSurveyQuestionEditorRowControl.chkOfferPlayback.Text");
		preRecordingPromptsButton.Text = LocalizedResourceMgr.GetString("RecordingSurveyQuestionEditorRowControl.preRecordingPromptsButton.Text");
		postRecordingPromptsButton.Text = LocalizedResourceMgr.GetString("RecordingSurveyQuestionEditorRowControl.postRecordingPromptsButton.Text");
		lblKeepRecording.Text = LocalizedResourceMgr.GetString("RecordingSurveyQuestionEditorRowControl.lblKeepRecording.Text");
		lblReRecord.Text = LocalizedResourceMgr.GetString("RecordingSurveyQuestionEditorRowControl.lblReRecord.Text");
		chkOfferPlayback.Checked = surveyQuestion.OfferPlayback;
		ChkOfferPlayback_CheckedChanged(chkOfferPlayback, EventArgs.Empty);
		txtMaxTime.Text = surveyQuestion.MaxRecordingTime.ToString();
		comboKeepRecording.SelectedIndex = GetIndex(surveyQuestion.KeepAnswer);
		comboRerecord.SelectedIndex = GetIndex(surveyQuestion.RerecordAnswer);
	}

	private void ChkOfferPlayback_CheckedChanged(object sender, EventArgs e)
	{
		preRecordingPromptsButton.Enabled = chkOfferPlayback.Checked;
		postRecordingPromptsButton.Enabled = chkOfferPlayback.Checked;
		comboKeepRecording.Enabled = chkOfferPlayback.Checked;
		comboRerecord.Enabled = chkOfferPlayback.Checked;
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void PreRecordingPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(activity);
		promptCollectionEditorForm.PromptList = surveyQuestion.OfferPlaybackPreRecordingPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			surveyQuestion.OfferPlaybackPreRecordingPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void PostRecordingPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(activity);
		promptCollectionEditorForm.PromptList = surveyQuestion.OfferPlaybackPostRecordingPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			surveyQuestion.OfferPlaybackPostRecordingPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void Combo_MouseWheel(object sender, MouseEventArgs e)
	{
		((HandledMouseEventArgs)e).Handled = true;
	}

	public override SurveyQuestion Save()
	{
		surveyQuestion.OfferPlayback = chkOfferPlayback.Checked;
		surveyQuestion.MaxRecordingTime = (string.IsNullOrEmpty(txtMaxTime.Text) ? 60u : Convert.ToUInt32(txtMaxTime.Text));
		surveyQuestion.KeepAnswer = GetAnswer(comboKeepRecording.SelectedIndex);
		surveyQuestion.RerecordAnswer = GetAnswer(comboRerecord.SelectedIndex);
		return surveyQuestion;
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
		this.chkOfferPlayback = new System.Windows.Forms.CheckBox();
		this.preRecordingPromptsButton = new System.Windows.Forms.Button();
		this.postRecordingPromptsButton = new System.Windows.Forms.Button();
		this.comboKeepRecording = new System.Windows.Forms.ComboBox();
		this.lblKeepRecording = new System.Windows.Forms.Label();
		this.comboRerecord = new System.Windows.Forms.ComboBox();
		this.lblReRecord = new System.Windows.Forms.Label();
		this.txtMaxTime = new System.Windows.Forms.MaskedTextBox();
		this.lblMaxTime = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.chkOfferPlayback.AutoSize = true;
		this.chkOfferPlayback.Location = new System.Drawing.Point(4, 8);
		this.chkOfferPlayback.Margin = new System.Windows.Forms.Padding(4);
		this.chkOfferPlayback.Name = "chkOfferPlayback";
		this.chkOfferPlayback.Size = new System.Drawing.Size(123, 21);
		this.chkOfferPlayback.TabIndex = 0;
		this.chkOfferPlayback.Text = "Offer Playback";
		this.chkOfferPlayback.UseVisualStyleBackColor = true;
		this.chkOfferPlayback.CheckedChanged += new System.EventHandler(ChkOfferPlayback_CheckedChanged);
		this.preRecordingPromptsButton.Location = new System.Drawing.Point(4, 32);
		this.preRecordingPromptsButton.Name = "preRecordingPromptsButton";
		this.preRecordingPromptsButton.Size = new System.Drawing.Size(194, 28);
		this.preRecordingPromptsButton.TabIndex = 3;
		this.preRecordingPromptsButton.Text = "Pre Recording Prompts";
		this.preRecordingPromptsButton.UseVisualStyleBackColor = true;
		this.preRecordingPromptsButton.Click += new System.EventHandler(PreRecordingPromptsButton_Click);
		this.postRecordingPromptsButton.Location = new System.Drawing.Point(4, 66);
		this.postRecordingPromptsButton.Name = "postRecordingPromptsButton";
		this.postRecordingPromptsButton.Size = new System.Drawing.Size(194, 28);
		this.postRecordingPromptsButton.TabIndex = 4;
		this.postRecordingPromptsButton.Text = "Post Recording Prompts";
		this.postRecordingPromptsButton.UseVisualStyleBackColor = true;
		this.postRecordingPromptsButton.Click += new System.EventHandler(PostRecordingPromptsButton_Click);
		this.comboKeepRecording.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboKeepRecording.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboKeepRecording.FormattingEnabled = true;
		this.comboKeepRecording.Items.AddRange(new object[10] { "Option 0", "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9" });
		this.comboKeepRecording.Location = new System.Drawing.Point(373, 36);
		this.comboKeepRecording.Margin = new System.Windows.Forms.Padding(4);
		this.comboKeepRecording.Name = "comboKeepRecording";
		this.comboKeepRecording.Size = new System.Drawing.Size(153, 24);
		this.comboKeepRecording.TabIndex = 6;
		this.comboKeepRecording.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.lblKeepRecording.AutoSize = true;
		this.lblKeepRecording.Location = new System.Drawing.Point(205, 39);
		this.lblKeepRecording.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblKeepRecording.Name = "lblKeepRecording";
		this.lblKeepRecording.Size = new System.Drawing.Size(160, 17);
		this.lblKeepRecording.TabIndex = 5;
		this.lblKeepRecording.Text = "Keep Recording Answer";
		this.comboRerecord.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboRerecord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboRerecord.FormattingEnabled = true;
		this.comboRerecord.Items.AddRange(new object[10] { "Option 0", "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9" });
		this.comboRerecord.Location = new System.Drawing.Point(373, 68);
		this.comboRerecord.Margin = new System.Windows.Forms.Padding(4);
		this.comboRerecord.Name = "comboRerecord";
		this.comboRerecord.Size = new System.Drawing.Size(153, 24);
		this.comboRerecord.TabIndex = 8;
		this.comboRerecord.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.lblReRecord.AutoSize = true;
		this.lblReRecord.Location = new System.Drawing.Point(205, 71);
		this.lblReRecord.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblReRecord.Name = "lblReRecord";
		this.lblReRecord.Size = new System.Drawing.Size(144, 17);
		this.lblReRecord.TabIndex = 7;
		this.lblReRecord.Text = "Record Again Answer";
		this.txtMaxTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMaxTime.HidePromptOnLeave = true;
		this.txtMaxTime.Location = new System.Drawing.Point(373, 6);
		this.txtMaxTime.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxTime.Mask = "99999";
		this.txtMaxTime.Name = "txtMaxTime";
		this.txtMaxTime.Size = new System.Drawing.Size(153, 22);
		this.txtMaxTime.TabIndex = 2;
		this.txtMaxTime.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.lblMaxTime.AutoSize = true;
		this.lblMaxTime.Location = new System.Drawing.Point(205, 9);
		this.lblMaxTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxTime.Name = "lblMaxTime";
		this.lblMaxTime.Size = new System.Drawing.Size(158, 17);
		this.lblMaxTime.TabIndex = 1;
		this.lblMaxTime.Text = "Max Duration (seconds)";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.txtMaxTime);
		base.Controls.Add(this.lblMaxTime);
		base.Controls.Add(this.comboRerecord);
		base.Controls.Add(this.lblReRecord);
		base.Controls.Add(this.comboKeepRecording);
		base.Controls.Add(this.lblKeepRecording);
		base.Controls.Add(this.postRecordingPromptsButton);
		base.Controls.Add(this.preRecordingPromptsButton);
		base.Controls.Add(this.chkOfferPlayback);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "RecordingSurveyQuestionEditorRowControl";
		base.Size = new System.Drawing.Size(530, 100);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
