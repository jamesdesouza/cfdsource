using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class AbsSurveyQuestionEditorRowControl : UserControl
{
	private IContainer components;

	protected int GetIndex(SurveyAnswers answer)
	{
		return answer switch
		{
			SurveyAnswers.Option0 => 0, 
			SurveyAnswers.Option1 => 1, 
			SurveyAnswers.Option2 => 2, 
			SurveyAnswers.Option3 => 3, 
			SurveyAnswers.Option4 => 4, 
			SurveyAnswers.Option5 => 5, 
			SurveyAnswers.Option6 => 6, 
			SurveyAnswers.Option7 => 7, 
			SurveyAnswers.Option8 => 8, 
			_ => 9, 
		};
	}

	protected SurveyAnswers GetAnswer(int index)
	{
		return index switch
		{
			0 => SurveyAnswers.Option0, 
			1 => SurveyAnswers.Option1, 
			2 => SurveyAnswers.Option2, 
			3 => SurveyAnswers.Option3, 
			4 => SurveyAnswers.Option4, 
			5 => SurveyAnswers.Option5, 
			6 => SurveyAnswers.Option6, 
			7 => SurveyAnswers.Option7, 
			8 => SurveyAnswers.Option8, 
			_ => SurveyAnswers.Option9, 
		};
	}

	public AbsSurveyQuestionEditorRowControl()
	{
		InitializeComponent();
	}

	public virtual SurveyQuestion Save()
	{
		throw new NotImplementedException("Save method not implemented");
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "AbsSurveyQuestionEditorRowControl";
		base.Size = new System.Drawing.Size(418, 27);
		base.ResumeLayout(false);
	}
}
