using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class RecordComponentValidator : CompositeActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		RecordComponent recordComponent = obj as RecordComponent;
		if (recordComponent.Parent != null)
		{
			if (recordComponent.EnabledActivities.Count != 2)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Record.InvalidBranchCount"), 0, isWarning: false, ""));
			}
			List<string> validVariables = ExpressionHelper.GetValidVariables(recordComponent);
			if (recordComponent.Prompts.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Record.PromptsAreEmpty"), 0, isWarning: false, "Prompts"));
			}
			if (string.IsNullOrEmpty(recordComponent.SaveToFile))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Record.SaveToFileRequired"), 0, isWarning: false, "SaveToFile"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, recordComponent.SaveToFile).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Record.InvalidSaveToFile"), 0, isWarning: false, "SaveToFile"));
			}
			if (recordComponent.SaveToFile != "false")
			{
				if (string.IsNullOrEmpty(recordComponent.FileName))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Record.FileNameRequired"), 0, isWarning: false, "FileName"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, recordComponent.FileName).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Record.InvalidFileName"), 0, isWarning: false, "FileName"));
				}
			}
			bool flag = false;
			foreach (Prompt prompt in recordComponent.Prompts)
			{
				if (prompt is TextToSpeechAudioPrompt)
				{
					flag = true;
					break;
				}
			}
			if (flag && !recordComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}
