using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Classes.Editors;

public class ResponseMappingCollectionEditor : CollectionEditor
{
	private List<ResponseMapping> resultList;

	public ResponseMappingCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override CollectionForm CreateCollectionForm()
	{
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
		resultList = new List<ResponseMapping>();
		for (int i = 0; i < value.Length; i++)
		{
			ResponseMapping item = (ResponseMapping)value[i];
			resultList.Add(item);
		}
		return base.SetItems(editValue, value);
	}

	protected override string GetDisplayText(object value)
	{
		ResponseMapping responseMapping = value as ResponseMapping;
		if (responseMapping.GetContainerActivity() == null)
		{
			responseMapping.SetContainerActivity(base.Context.Instance as IVadActivity);
		}
		if (string.IsNullOrEmpty(responseMapping.Path))
		{
			responseMapping.Path = "path";
		}
		return responseMapping.Path;
	}
}
