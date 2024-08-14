using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class RangeSurveyQuestionEditorRowControl : AbsSurveyQuestionEditorRowControl
{
	private RangeSurveyQuestion surveyQuestion;

	private IContainer components;

	private ComboBox comboEnd;

	private Label lblEnd;

	private ComboBox comboStart;

	private Label lblStart;

	public RangeSurveyQuestionEditorRowControl(RangeSurveyQuestion surveyQuestion)
	{
		InitializeComponent();
		this.surveyQuestion = surveyQuestion;
		lblStart.Text = LocalizedResourceMgr.GetString("RangeSurveyQuestionEditorRowControl.lblStart.Text");
		lblEnd.Text = LocalizedResourceMgr.GetString("RangeSurveyQuestionEditorRowControl.lblEnd.Text");
		comboStart.SelectedIndex = GetIndex(surveyQuestion.RangeStartAnswer);
		comboEnd.SelectedIndex = GetIndex(surveyQuestion.RangeEndAnswer);
	}

	private void Combo_MouseWheel(object sender, MouseEventArgs e)
	{
		((HandledMouseEventArgs)e).Handled = true;
	}

	public override SurveyQuestion Save()
	{
		surveyQuestion.RangeStartAnswer = GetAnswer(comboStart.SelectedIndex);
		surveyQuestion.RangeEndAnswer = GetAnswer(comboEnd.SelectedIndex);
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
		this.comboEnd = new System.Windows.Forms.ComboBox();
		this.lblEnd = new System.Windows.Forms.Label();
		this.comboStart = new System.Windows.Forms.ComboBox();
		this.lblStart = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.comboEnd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboEnd.FormattingEnabled = true;
		this.comboEnd.Items.AddRange(new object[10] { "Option 0", "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9" });
		this.comboEnd.Location = new System.Drawing.Point(277, 29);
		this.comboEnd.Margin = new System.Windows.Forms.Padding(4);
		this.comboEnd.Name = "comboEnd";
		this.comboEnd.Size = new System.Drawing.Size(249, 24);
		this.comboEnd.TabIndex = 7;
		this.comboEnd.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.lblEnd.AutoSize = true;
		this.lblEnd.Location = new System.Drawing.Point(274, 8);
		this.lblEnd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblEnd.Name = "lblEnd";
		this.lblEnd.Size = new System.Drawing.Size(134, 17);
		this.lblEnd.TabIndex = 6;
		this.lblEnd.Text = "Range Final Answer";
		this.comboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStart.FormattingEnabled = true;
		this.comboStart.Items.AddRange(new object[10] { "Option 0", "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8", "Option 9" });
		this.comboStart.Location = new System.Drawing.Point(7, 29);
		this.comboStart.Margin = new System.Windows.Forms.Padding(4);
		this.comboStart.Name = "comboStart";
		this.comboStart.Size = new System.Drawing.Size(249, 24);
		this.comboStart.TabIndex = 5;
		this.comboStart.MouseWheel += new System.Windows.Forms.MouseEventHandler(Combo_MouseWheel);
		this.lblStart.AutoSize = true;
		this.lblStart.Location = new System.Drawing.Point(4, 8);
		this.lblStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStart.Name = "lblStart";
		this.lblStart.Size = new System.Drawing.Size(136, 17);
		this.lblStart.TabIndex = 4;
		this.lblStart.Text = "Range Initial Answer";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.comboEnd);
		base.Controls.Add(this.lblEnd);
		base.Controls.Add(this.comboStart);
		base.Controls.Add(this.lblStart);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "RangeSurveyQuestionEditorRowControl";
		base.Size = new System.Drawing.Size(530, 60);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
