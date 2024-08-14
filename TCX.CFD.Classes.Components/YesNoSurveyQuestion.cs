using System;
using System.Collections.Generic;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class YesNoSurveyQuestion : SurveyQuestion
{
	public SurveyAnswers YesAnswer { get; set; }

	public SurveyAnswers NoAnswer { get; set; }

	public YesNoSurveyQuestion()
	{
		YesAnswer = SurveyAnswers.Option1;
		NoAnswer = SurveyAnswers.Option2;
	}

	public YesNoSurveyQuestion(string tag, List<Prompt> prompts, SurveyAnswers yesAnswer, SurveyAnswers noAnswer)
		: base(tag, prompts)
	{
		YesAnswer = yesAnswer;
		NoAnswer = noAnswer;
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, componentsInitializationScriptSb, audioFileCollector);
	}

	public override SurveyQuestion Clone()
	{
		return new YesNoSurveyQuestion(base.Tag, base.Prompts, YesAnswer, NoAnswer)
		{
			ContainerActivity = base.ContainerActivity
		};
	}

	public override AbsSurveyQuestionEditorRowControl CreateSurveyQuestionEditorRowControl()
	{
		return new YesNoSurveyQuestionEditorRowControl(this);
	}

	public override List<Prompt> GetAllPrompts()
	{
		return prompts;
	}

	public override bool IsValid()
	{
		if (base.IsValid())
		{
			return YesAnswer != NoAnswer;
		}
		return false;
	}
}
