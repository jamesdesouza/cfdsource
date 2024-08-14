using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class TranscribeAudioComponentValidator : CompositeActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		TranscribeAudioComponent transcribeAudioComponent = obj as TranscribeAudioComponent;
		if (transcribeAudioComponent.Parent != null)
		{
			if (string.IsNullOrEmpty(transcribeAudioComponent.LanguageCode))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TranscribeAudio.LanguageCodeRequired"), 0, isWarning: false, "LanguageCode"));
			}
			List<string> validVariables = ExpressionHelper.GetValidVariables(transcribeAudioComponent);
			if (string.IsNullOrEmpty(transcribeAudioComponent.FileName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TranscribeAudio.FileNameRequired"), 0, isWarning: false, "FileName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, transcribeAudioComponent.FileName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TranscribeAudio.InvalidFileName"), 0, isWarning: false, "FileName"));
			}
			bool flag = false;
			for (int i = 0; i < transcribeAudioComponent.Hints.Count; i++)
			{
				if (string.IsNullOrEmpty(transcribeAudioComponent.Hints[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.TranscribeAudio.EmptyHints"), 0, isWarning: false, "Hints"));
			}
			if (!transcribeAudioComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForSTT())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.STTRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}
