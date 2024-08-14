using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Classes.Editors;

public class ParameterCollectionEditor<ParameterT> : CollectionEditor
{
	private List<string> usedParameterNames;

	private List<ParameterT> resultList;

	private string GetNewParameterName()
	{
		int num = 0;
		string text = "parameter";
		string text2;
		do
		{
			int num2 = ++num;
			text2 = text + num2;
		}
		while (usedParameterNames.Contains(text2));
		return text2;
	}

	private void Parameter_NameChanging(object sender, NameChangingEventArgs e)
	{
		if (usedParameterNames.Contains(e.NewValue))
		{
			e.Cancel = true;
			MessageBox.Show(LocalizedResourceMgr.GetString("ParameterCollectionEditor.MessageBox.Error.ParameterNameAlreadyExists"), LocalizedResourceMgr.GetString("ParameterCollectionEditor.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Parameter_NameChanged(object sender, NameChangedEventArgs e)
	{
		usedParameterNames.Remove(e.OldValue);
		usedParameterNames.Add(e.NewValue);
	}

	public ParameterCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override CollectionForm CreateCollectionForm()
	{
		usedParameterNames = new List<string>();
		foreach (ParameterT item in base.Context.PropertyDescriptor.GetValue(base.Context.Instance) as List<ParameterT>)
		{
			usedParameterNames.Add((item as Parameter).Name);
		}
		CollectionForm collectionForm = base.CreateCollectionForm();
		collectionForm.FormClosed += CollectionForm_FormClosed;
		return collectionForm;
	}

	private void CollectionForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if ((sender as CollectionForm).DialogResult == DialogResult.OK)
		{
			base.Context.PropertyDescriptor.SetValue(base.Context.Instance, resultList);
		}
	}

	protected override object SetItems(object editValue, object[] value)
	{
		resultList = new List<ParameterT>();
		for (int i = 0; i < value.Length; i++)
		{
			ParameterT item = (ParameterT)value[i];
			resultList.Add(item);
		}
		return base.SetItems(editValue, value);
	}

	protected override string GetDisplayText(object value)
	{
		Parameter parameter = value as Parameter;
		if (!parameter.IsNameChangingHandled())
		{
			parameter.NameChanging += Parameter_NameChanging;
		}
		if (!parameter.IsNameChangedHandled())
		{
			parameter.NameChanged += Parameter_NameChanged;
		}
		if (parameter.GetContainerActivity() == null)
		{
			parameter.SetContainerActivity(base.Context.Instance as IVadActivity);
		}
		if (string.IsNullOrEmpty(parameter.Name))
		{
			parameter.Name = GetNewParameterName();
		}
		return parameter.Name;
	}
}
