using System;
using System.Collections.Generic;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class RangeSurveyQuestion : SurveyQuestion
{
	public SurveyAnswers RangeStartAnswer { get; set; }

	public SurveyAnswers RangeEndAnswer { get; set; }

	public RangeSurveyQuestion()
	{
		RangeStartAnswer = SurveyAnswers.Option1;
		RangeEndAnswer = SurveyAnswers.Option5;
	}

	public RangeSurveyQuestion(string tag, List<Prompt> prompts, SurveyAnswers rangeStartAnswer, SurveyAnswers rangeEndAnswer)
		: base(tag, prompts)
	{
		RangeStartAnswer = rangeStartAnswer;
		RangeEndAnswer = rangeEndAnswer;
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, componentsInitializationScriptSb, audioFileCollector);
	}

	public override SurveyQuestion Clone()
	{
		return new RangeSurveyQuestion(base.Tag, base.Prompts, RangeStartAnswer, RangeEndAnswer)
		{
			ContainerActivity = base.ContainerActivity
		};
	}

	public override AbsSurveyQuestionEditorRowControl CreateSurveyQuestionEditorRowControl()
	{
		return new RangeSurveyQuestionEditorRowControl(this);
	}

	public override List<Prompt> GetAllPrompts()
	{
		return prompts;
	}

	public override bool IsValid()
	{
		if (base.IsValid())
		{
			return RangeStartAnswer != RangeEndAnswer;
		}
		return false;
	}
}
