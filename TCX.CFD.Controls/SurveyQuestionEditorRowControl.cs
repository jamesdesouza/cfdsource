using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Controls;

public class SurveyQuestionEditorRowControl : UserControl
{
	private IVadActivity activity;

	private SurveyQuestion surveyQuestion;

	private YesNoSurveyQuestion yesNoSurveyQuestion;

	private RangeSurveyQuestion rangeSurveyQuestion;

	private RecordingSurveyQuestion recordingSurveyQuestion;

	private AbsSurveyQuestionEditorRowControl surveyQuestionEditorRowControl;

	private List<Prompt> promptList = new List<Prompt>();

	private IContainer components;

	private ComboBox comboQuestionType;

	private CheckBox chkSelection;

	private Label lblTag;

	private TextBox txtTag;

	private Button promptsButton;

	public bool IsChecked
	{
		get
		{
			return chkSelection.Checked;
		}
		set
		{
			chkSelection.Checked = value;
		}
	}

	public event EventHandler CheckedChanged;

	public event EventHandler HeightChanged;

	private void CreateSurveyQuestionEditorRowControl()
	{
		if (surveyQuestionEditorRowControl != null)
		{
			base.Controls.Remove(surveyQuestionEditorRowControl);
		}
		surveyQuestionEditorRowControl = surveyQuestion.CreateSurveyQuestionEditorRowControl();
		surveyQuestionEditorRowControl.Location = new Point(lblTag.Location.X - 2, lblTag.Location.Y + 18);
		surveyQuestionEditorRowControl.Size = new Size(base.Width - surveyQuestionEditorRowControl.Location.X, surveyQuestionEditorRowControl.Height);
		surveyQuestionEditorRowControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		base.Controls.Add(surveyQuestionEditorRowControl);
		base.Size = new Size(base.Size.Width, surveyQuestionEditorRowControl.Height + 30);
		this.HeightChanged?.Invoke(this, EventArgs.Empty);
	}

	public SurveyQuestionEditorRowControl(IVadActivity activity, SurveyQuestion surveyQuestion)
	{
		InitializeComponent();
		this.activity = activity;
		this.surveyQuestion = surveyQuestion;
		surveyQuestionEditorRowControl = null;
		promptList = surveyQuestion.Prompts;
		yesNoSurveyQuestion = new YesNoSurveyQuestion();
		yesNoSurveyQuestion.ContainerActivity = activity;
		rangeSurveyQuestion = new RangeSurveyQuestion();
		rangeSurveyQuestion.ContainerActivity = activity;
		recordingSurveyQuestion = new RecordingSurveyQuestion();
		recordingSurveyQuestion.ContainerActivity = activity;
		comboQuestionType.Items.AddRange(new object[3]
		{
			LocalizedResourceMgr.GetString("SurveyQuestionEditorRowControl.comboQuestionType.YesNoSurveyQuestion"),
			LocalizedResourceMgr.GetString("SurveyQuestionEditorRowControl.comboQuestionType.RangeSurveyQuestion"),
			LocalizedResourceMgr.GetString("SurveyQuestionEditorRowControl.comboQuestionType.RecordingSurveyQuestion")
		});
		if (surveyQuestion is YesNoSurveyQuestion)
		{
			yesNoSurveyQuestion = surveyQuestion as YesNoSurveyQuestion;
			comboQuestionType.SelectedIndex = 0;
		}
		else if (surveyQuestion is RangeSurveyQuestion)
		{
			rangeSurveyQuestion = surveyQuestion as RangeSurveyQuestion;
			comboQuestionType.SelectedIndex = 1;
		}
		else
		{
			recordingSurveyQuestion = surveyQuestion as RecordingSurveyQuestion;
			comboQuestionType.SelectedIndex = 2;
		}
		txtTag.Text = surveyQuestion.Tag;
	}

	private void ChkSelection_CheckedChanged(object sender, EventArgs e)
	{
		this.CheckedChanged?.Invoke(this, e);
	}

	private void Combo_MouseWheel(object sender, MouseEventArgs e)
	{
		((HandledMouseEventArgs)e).Handled = true;
	}

	private void ComboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (surveyQuestionEditorRowControl != null)
		{
			surveyQuestionEditorRowControl.Save();
		}
		switch (comboQuestionType.SelectedIndex)
		{
		case 0:
			surveyQuestion = yesNoSurveyQuestion;
			break;
		case 1:
			surveyQuestion = rangeSurveyQuestion;
			break;
		case 2:
			surveyQuestion = recordingSurveyQuestion;
			break;
		}
		CreateSurveyQuestionEditorRowControl();
	}

	private void TxtTag_Enter(object sender, EventArgs e)
	{
		txtTag.SelectAll();
	}

	private void PromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(activity);
		promptCollectionEditorForm.PromptList = promptList;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			promptList = promptCollectionEditorForm.PromptList;
		}
	}

	public SurveyQuestion Save()
	{
		surveyQuestion.Tag = txtTag.Text;
		surveyQuestion.Prompts = promptList;
		return surveyQuestionEditorRowControl.Save();
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
		this.comboQuestionType = new System.Windows.Forms.ComboBox();
		this.chkSelection = new System.Windows.Forms.CheckBox();
		this.lblTag = new System.Windows.Forms.Label();
		this.txtTag = new System.Windows.Forms.TextBox();
		this.promptsButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.comboQuestionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboQuestionType.FormattingEnabled = true;
		this.comboQuestionType.Location = new System.Drawing.Point(34, 6);
		this.comboQuestionType.Margin = new System.Windows.Forms.Padding(4);
		this.comboQuestionType.Name = "comboQuestionType";
		this.comboQuestionType.Size = new System.Drawing.Size(199, 24);
		this.comboQuestionType.TabIndex = 1;
		this.comboQuestionType.SelectedIndexChanged += new System.EventHandler(ComboQuestionType_SelectedIndexChanged);
		this.comboQuestionType.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.chkSelection.AutoSize = true;
		this.chkSelection.Location = new System.Drawing.Point(6, 10);
		this.chkSelection.Margin = new System.Windows.Forms.Padding(4);
		this.chkSelection.Name = "chkSelection";
		this.chkSelection.Size = new System.Drawing.Size(18, 17);
		this.chkSelection.TabIndex = 0;
		this.chkSelection.UseVisualStyleBackColor = true;
		this.chkSelection.CheckedChanged += new System.EventHandler(ChkSelection_CheckedChanged);
		this.lblTag.AutoSize = true;
		this.lblTag.Location = new System.Drawing.Point(249, 9);
		this.lblTag.Name = "lblTag";
		this.lblTag.Size = new System.Drawing.Size(33, 17);
		this.lblTag.TabIndex = 2;
		this.lblTag.Text = "Tag";
		this.txtTag.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTag.Location = new System.Drawing.Point(288, 6);
		this.txtTag.MaxLength = 150;
		this.txtTag.Name = "txtTag";
		this.txtTag.Size = new System.Drawing.Size(361, 22);
		this.txtTag.TabIndex = 3;
		this.txtTag.Enter += new System.EventHandler(TxtTag_Enter);
		this.promptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.promptsButton.Location = new System.Drawing.Point(655, 4);
		this.promptsButton.Name = "promptsButton";
		this.promptsButton.Size = new System.Drawing.Size(122, 28);
		this.promptsButton.TabIndex = 4;
		this.promptsButton.Text = "Prompts";
		this.promptsButton.UseVisualStyleBackColor = true;
		this.promptsButton.Click += new System.EventHandler(PromptsButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.promptsButton);
		base.Controls.Add(this.txtTag);
		base.Controls.Add(this.lblTag);
		base.Controls.Add(this.chkSelection);
		base.Controls.Add(this.comboQuestionType);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "SurveyQuestionEditorRowControl";
		base.Size = new System.Drawing.Size(780, 95);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
