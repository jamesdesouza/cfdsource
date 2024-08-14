using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class PromptPlaybackComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		PromptPlaybackComponent promptPlaybackComponent = obj as PromptPlaybackComponent;
		if (promptPlaybackComponent.Parent != null)
		{
			if (promptPlaybackComponent.Prompts.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.PromptPlayback.PromptsAreEmpty"), 0, isWarning: false, "Prompts"));
			}
			bool flag = false;
			foreach (Prompt prompt in promptPlaybackComponent.Prompts)
			{
				if (prompt is TextToSpeechAudioPrompt)
				{
					flag = true;
					break;
				}
			}
			if (flag && !promptPlaybackComponent.GetRootFlow().FileObject.GetProjectObject().OnlineServices.IsReadyForTTS())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.General.TTSRequired"), 0, isWarning: false, ""));
			}
		}
		return validationErrorCollection;
	}
}
