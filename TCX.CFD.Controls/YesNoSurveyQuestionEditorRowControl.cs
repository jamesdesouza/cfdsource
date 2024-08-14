using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class YesNoSurveyQuestionEditorRowControl : AbsSurveyQuestionEditorRowControl
{
	private YesNoSurveyQuestion surveyQuestion;

	private IContainer components;

	private Label lblYes;

	private ComboBox comboYes;

	private ComboBox comboNo;

	private Label lblNo;

	public YesNoSurveyQuestionEditorRowControl(YesNoSurveyQuestion surveyQuestion)
	{
		InitializeComponent();
		this.surveyQuestion = surveyQuestion;
		lblYes.Text = LocalizedResourceMgr.GetString("YesNoSurveyQuestionEditorRowControl.lblYes.Text");
		lblNo.Text = LocalizedResourceMgr.GetString("YesNoSurveyQuestionEditorRowControl.lblNo.Text");
		comboYes.SelectedIndex = GetIndex(surveyQuestion.YesAnswer);
		comboNo.SelectedIndex = GetIndex(surveyQuestion.NoAnswer);
	}

	private void Combo_MouseWheel(object sender, MouseEventArgs e)
	{
		((HandledMouseEventArgs)e).Handled = true;
	}

	public override SurveyQuestion Save()
	{
		surveyQuestion.YesAnswer = GetAnswer(comboYes.SelectedIndex);
		surveyQuestion.NoAnswer = GetAnswer(comboNo.SelectedIndex);
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
		this.lblYes = new System.Windows.Forms.Label();
		this.comboYes = new System.Windows.Forms.ComboBox();
		this.comboNo = new System.Windows.Forms.ComboBox();
		this.lblNo = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblYes.AutoSize = true;
		this.lblYes.Location = new System.Drawing.Point(4, 7);
		this.lblYes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblYes.Name = "lblYes";
		this.lblYes.Size = new System.Drawing.Size(82, 17);
		this.lblYes.TabIndex = 0;
		this.lblYes.Text = "Yes Answer";
		this.comboYes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboYes.FormattingEnabled = true;
		this.comboYes.Items.AddRange(new object[10] { "Option 0", "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9" });
		this.comboYes.Location = new System.Drawing.Point(7, 28);
		this.comboYes.Margin = new System.Windows.Forms.Padding(4);
		this.comboYes.Name = "comboYes";
		this.comboYes.Size = new System.Drawing.Size(249, 24);
		this.comboYes.TabIndex = 1;
		this.comboYes.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.comboNo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboNo.FormattingEnabled = true;
		this.comboNo.Items.AddRange(new object[10] { "Option 0", "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9" });
		this.comboNo.Location = new System.Drawing.Point(277, 28);
		this.comboNo.Margin = new System.Windows.Forms.Padding(4);
		this.comboNo.Name = "comboNo";
		this.comboNo.Size = new System.Drawing.Size(249, 24);
		this.comboNo.TabIndex = 3;
		this.comboNo.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.lblNo.AutoSize = true;
		this.lblNo.Location = new System.Drawing.Point(274, 7);
		this.lblNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblNo.Name = "lblNo";
		this.lblNo.Size = new System.Drawing.Size(76, 17);
		this.lblNo.TabIndex = 2;
		this.lblNo.Text = "No Answer";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.comboNo);
		base.Controls.Add(this.lblNo);
		base.Controls.Add(this.comboYes);
		base.Controls.Add(this.lblYes);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "YesNoSurveyQuestionEditorRowControl";
		base.Size = new System.Drawing.Size(530, 60);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
