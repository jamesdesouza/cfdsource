using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class PromptCollectionEditor : UITypeEditor
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
			IWindowsFormsEditorService windowsFormsEditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			IVadActivity vadActivity = context.Instance as IVadActivity;
			if (windowsFormsEditorService != null && vadActivity != null)
			{
				PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(vadActivity);
				promptCollectionEditorForm.PromptList = value as List<Prompt>;
				if (windowsFormsEditorService.ShowDialog(promptCollectionEditorForm) == DialogResult.OK)
				{
					return promptCollectionEditorForm.PromptList;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
