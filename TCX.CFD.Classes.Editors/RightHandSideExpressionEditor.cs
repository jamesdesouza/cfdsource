using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class RightHandSideExpressionEditor : UITypeEditor
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
			else if (context.Instance is Parameter)
			{
				vadActivity = (context.Instance as Parameter).GetContainerActivity();
			}
			else if (context.Instance is MailAttachment)
			{
				vadActivity = (context.Instance as MailAttachment).GetContainerActivity();
			}
			if (windowsFormsEditorService != null && vadActivity != null)
			{
				ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(vadActivity);
				expressionEditorForm.Expression = value as string;
				if (windowsFormsEditorService.ShowDialog(expressionEditorForm) == DialogResult.OK)
				{
					return expressionEditorForm.Expression;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
