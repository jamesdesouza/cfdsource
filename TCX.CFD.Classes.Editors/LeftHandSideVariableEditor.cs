using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class LeftHandSideVariableEditor : UITypeEditor
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
			IVadActivity vadActivity = null;
			if (context.Instance is IVadActivity)
			{
				vadActivity = context.Instance as IVadActivity;
			}
			else if (context.Instance is ResponseMapping)
			{
				vadActivity = (context.Instance as ResponseMapping).GetContainerActivity();
			}
			if (windowsFormsEditorService != null && vadActivity != null)
			{
				LeftHandSideVariableSelectorForm leftHandSideVariableSelectorForm = new LeftHandSideVariableSelectorForm();
				leftHandSideVariableSelectorForm.Component = vadActivity;
				leftHandSideVariableSelectorForm.VariableName = value as string;
				if (windowsFormsEditorService.ShowDialog(leftHandSideVariableSelectorForm) == DialogResult.OK)
				{
					return leftHandSideVariableSelectorForm.VariableName;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
