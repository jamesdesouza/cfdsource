using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class SurveyComponentValidator : ActivityValidator
{
	private bool PromptsUseTTS(List<Prompt> prompts)
	{
		foreach (Prompt prompt in prompts)
		{
			if (prompt is TextToSpeechAudioPrompt)
			{
				return true;
			}
		}
		return false;
	}

	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		SurveyComponent surveyComponent = obj as SurveyComponent;
		if (surveyComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(surveyComponent);
			if (string.IsNullOrEmpty(surveyComponent.ExportToCSVFile))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.ExportToCSVFileRequired"), 0, isWarning: false, "ExportToCSVFile"));
			}
			else
			{
				AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, surveyComponent.ExportToCSVFile);
				if (!absArgument.IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.InvalidExportToCSVFile"), 0, isWarning: false, "ExportToCSVFile"));
				}
				foreach (DotNetExpressionArgument literalExpression in absArgument.GetLiteralExpressionList())
				{
					if (literalExpression.IsStringLiteral() && ExpressionHelper.UnescapeConstantString(literalExpression.GetString()).IndexOfAny(new char[6] { '<', '>', '"', '|', '?', '*' }) >= 0)
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.ExportToCSVFileInvalidCharacters"), 0, isWarning: false, "ExportToCSVFile"));
						break;
					}
				}
			}
			if (surveyComponent.SurveyQuestions.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.OneQuestionRequired"), 0, isWarning: false, "SurveyQuestions"));
			}
			bool flag = false;
			int num = 0;
			foreach (SurveyQuestion surveyQuestion in surveyComponent.SurveyQuestions)
			{
				if (!surveyQuestion.IsValid())
				{
					validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.InvalidSurveyQuestion"), num), 0, isWarning: false, "SurveyQuestions"));
				}
				if (surveyQuestion is RecordingSurveyQuestion)
				{
					flag = true;
				}
				num++;
			}
			if (flag && !string.IsNullOrEmpty(surveyComponent.RecordingsPath))
			{
				AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, surveyComponent.RecordingsPath);
				if (!absArgument2.IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.InvalidRecordingsPath"), 0, isWarning: false, "RecordingsPath"));
				}
				foreach (DotNetExpressionArgument literalExpression2 in absArgument2.GetLiteralExpressionList())
				{
					if (literalExpression2.IsStringLiteral() && ExpressionHelper.UnescapeConstantString(literalExpression2.GetString()).IndexOfAny(new char[6] { '<', '>', '"', '|', '?', '*' }) >= 0)
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.RecordingsPathInvalidCharacters"), 0, isWarning: false, "RecordingsPath"));
						break;
					}
				}
			}
			num = 0;
			foreach (Parameter outputField in surveyComponent.OutputFields)
			{
				if (!string.IsNullOrEmpty(outputField.Name) || !string.IsNullOrEmpty(outputField.Value))
				{
					if (string.IsNullOrEmpty(outputField.Name))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.OutputFieldNameRequired"), num), 0, isWarning: false, "OutputFields"));
					}
					else if (string.IsNullOrEmpty(outputField.Value))
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.OutputFieldValueRequired"), num), 0, isWarning: false, "OutputFields"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, outputField.Value).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.SurveyComponent.InvalidOutputFieldExpression"), num), 0, isWarning: false, "OutputFields"));
					}
					num++;
				}
			}
			bool flag2 = PromptsUseTTS(surveyComponent.IntroductoryPrompts);
			if (!flag2)
			{
				flag2 = PromptsUseTTS(surveyComponent.GoodbyePrompts);
			}
			if (!flag2)
			{
				flag2 = PromptsUseTTS(surveyComponent.TimeoutPrompts);
			}
			if (!flag2)
			{
				flag2 = PromptsUseTTS(surveyComponent.InvalidDigitPrompts);
			}
			if (!flag2)
			{
				foreach (SurveyQuestion surveyQuestion2 in surveyComponent.SurveyQuestions)
				{
					flag2 = PromptsUseTTS(surveyQuestion2.GetAllPrompts());
					if (flag2)
					{
						break;
					}
				}
			}
			if (flag2 && !surveyComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}
