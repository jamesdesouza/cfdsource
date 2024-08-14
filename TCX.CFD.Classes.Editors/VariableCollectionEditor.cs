using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Editors;

public class VariableCollectionEditor : CollectionEditor
{
	private readonly List<string> forbiddenVariableNames = new List<string> { "Name" };

	private List<string> usedVariableNames;

	private List<Variable> resultList;

	private List<KeyValuePair<string, string>> renamedVariablesList;

	private string GetNewVariableName(string baseName)
	{
		int num = 0;
		string text;
		do
		{
			int num2 = ++num;
			text = baseName + num2;
		}
		while (usedVariableNames.Contains(text) || forbiddenVariableNames.Contains(text));
		return text;
	}

	private void Variable_NameChanging(object sender, NameChangingEventArgs e)
	{
		if (usedVariableNames.Contains(e.NewValue))
		{
			e.Cancel = true;
			MessageBox.Show(LocalizedResourceMgr.GetString("VariableCollectionEditor.MessageBox.Error.VariableNameAlreadyExists"), LocalizedResourceMgr.GetString("VariableCollectionEditor.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (forbiddenVariableNames.Contains(e.NewValue))
		{
			e.Cancel = true;
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("VariableCollectionEditor.MessageBox.Error.VariableNameNowAllowed"), e.NewValue), LocalizedResourceMgr.GetString("VariableCollectionEditor.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Variable_NameChanged(object sender, NameChangedEventArgs e)
	{
		usedVariableNames.Remove(e.OldValue);
		usedVariableNames.Add(e.NewValue);
		renamedVariablesList.Add(new KeyValuePair<string, string>(e.OldValue, e.NewValue));
	}

	public VariableCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override CollectionForm CreateCollectionForm()
	{
		renamedVariablesList = new List<KeyValuePair<string, string>>();
		usedVariableNames = new List<string>();
		foreach (Variable item in base.Context.PropertyDescriptor.GetValue(base.Context.Instance) as List<Variable>)
		{
			usedVariableNames.Add(item.Name);
		}
		CollectionForm collectionForm = base.CreateCollectionForm();
		collectionForm.FormClosed += CollectionForm_FormClosed;
		return collectionForm;
	}

	private void CollectionForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if ((sender as CollectionForm).DialogResult != DialogResult.OK)
		{
			return;
		}
		base.Context.PropertyDescriptor.SetValue(base.Context.Instance, resultList);
		try
		{
			if (!(base.Context.Instance is FileObject fileObject))
			{
				if (base.Context.Instance is ProjectObject projectObject)
				{
					projectObject.NotifyProjectVariableChanges(renamedVariablesList);
				}
			}
			else
			{
				fileObject.NotifyVariableChanges(renamedVariablesList);
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("VariableCollectionEditor.MessageBox.Error.SavingVariables"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("VariableCollectionEditor.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	protected override object SetItems(object editValue, object[] value)
	{
		resultList = new List<Variable>();
		for (int i = 0; i < value.Length; i++)
		{
			Variable item = (Variable)value[i];
			resultList.Add(item);
		}
		return base.SetItems(editValue, value);
	}

	protected override object CreateInstance(Type itemType)
	{
		return TypeDescriptor.CreateInstance(null, itemType, new Type[1] { typeof(bool) }, new object[1] { base.Context.Instance is ComponentFileObject });
	}

	protected override string GetDisplayText(object value)
	{
		Variable variable = value as Variable;
		if (!variable.IsNameChangingHandled())
		{
			variable.NameChanging += Variable_NameChanging;
		}
		if (!variable.IsNameChangedHandled())
		{
			variable.NameChanged += Variable_NameChanged;
		}
		if (string.IsNullOrEmpty(variable.Name))
		{
			variable.Name = GetNewVariableName(variable.GetType().Name);
		}
		return variable.Name;
	}
}
