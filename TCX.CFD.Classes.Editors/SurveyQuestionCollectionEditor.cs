using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class SurveyQuestionCollectionEditor : UITypeEditor
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
				SurveyQuestionCollectionEditorForm surveyQuestionCollectionEditorForm = new SurveyQuestionCollectionEditorForm(vadActivity);
				surveyQuestionCollectionEditorForm.SurveyQuestionList = value as List<SurveyQuestion>;
				if (windowsFormsEditorService.ShowDialog(surveyQuestionCollectionEditorForm) == DialogResult.OK)
				{
					return surveyQuestionCollectionEditorForm.SurveyQuestionList;
				}
			}
		}
		return base.EditValue(context, provider, value);
	}
}
