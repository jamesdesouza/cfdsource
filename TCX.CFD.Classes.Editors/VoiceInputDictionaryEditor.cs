using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class VoiceInputDictionaryEditor : UITypeEditor
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
			VoiceInputComponent voiceInputComponent = context.Instance as VoiceInputComponent;
			if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService windowsFormsEditorService && voiceInputComponent != null)
			{
				VoiceInputDictionaryEditorForm voiceInputDictionaryEditorForm = new VoiceInputDictionaryEditorForm(voiceInputComponent);
				voiceInputDictionaryEditorForm.Dictionary = value as List<string>;
				if (windowsFormsEditorService.ShowDialog(voiceInputDictionaryEditorForm) == DialogResult.OK)
				{
					return voiceInputDictionaryEditorForm.Dictionary;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
