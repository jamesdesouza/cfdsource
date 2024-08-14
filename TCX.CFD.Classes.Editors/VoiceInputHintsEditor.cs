using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class VoiceInputHintsEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		if (context != null)
		{
			return UITypeEditorEditStyle.Modal;
		}
		return base.GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		if (context != null && provider != null)
		{
			IVadActivity vadActivity = null;
			string languageCode = "";
			if (context.Instance is VoiceInputComponent voiceInputComponent)
			{
				vadActivity = voiceInputComponent;
				languageCode = voiceInputComponent.LanguageCode;
			}
			else if (context.Instance is TranscribeAudioComponent transcribeAudioComponent)
			{
				vadActivity = transcribeAudioComponent;
				languageCode = transcribeAudioComponent.LanguageCode;
			}
			if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService windowsFormsEditorService && vadActivity != null)
			{
				SpeechToTextHintsEditorForm speechToTextHintsEditorForm = new SpeechToTextHintsEditorForm(vadActivity, languageCode);
				speechToTextHintsEditorForm.Hints = value as List<string>;
				if (windowsFormsEditorService.ShowDialog(speechToTextHintsEditorForm) == DialogResult.OK)
				{
					return speechToTextHintsEditorForm.Hints;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
