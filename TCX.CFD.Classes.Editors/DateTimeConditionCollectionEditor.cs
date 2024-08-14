using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class DateTimeConditionCollectionEditor : UITypeEditor
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
			IVadActivity vadActivity = context.Instance as IVadActivity;
			if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService windowsFormsEditorService && vadActivity != null)
			{
				DateTimeConditionCollectionEditorForm dateTimeConditionCollectionEditorForm = new DateTimeConditionCollectionEditorForm(vadActivity);
				dateTimeConditionCollectionEditorForm.DateTimeConditionList = value as List<DateTimeCondition>;
				if (windowsFormsEditorService.ShowDialog(dateTimeConditionCollectionEditorForm) == DialogResult.OK)
				{
					return dateTimeConditionCollectionEditorForm.DateTimeConditionList;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
