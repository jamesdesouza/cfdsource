using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Classes.Editors;

public class MailAttachmentCollectionEditor : CollectionEditor
{
	private List<string> usedAttachmentNames;

	private List<MailAttachment> resultList;

	private string getNewMailAttachmentName()
	{
		int num = 0;
		string text = "attachment";
		string text2;
		do
		{
			int num2 = ++num;
			text2 = text + num2;
		}
		while (usedAttachmentNames.Contains(text2));
		return text2;
	}

	private void attachment_NameChanging(object sender, NameChangingEventArgs e)
	{
		if (usedAttachmentNames.Contains(e.NewValue))
		{
			e.Cancel = true;
			MessageBox.Show(LocalizedResourceMgr.GetString("MailAttachmentCollectionEditor.MessageBox.Error.MailAttachmentNameAlreadyExists"), LocalizedResourceMgr.GetString("MailAttachmentCollectionEditor.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void attachment_NameChanged(object sender, NameChangedEventArgs e)
	{
		usedAttachmentNames.Remove(e.OldValue);
		usedAttachmentNames.Add(e.NewValue);
	}

	public MailAttachmentCollectionEditor(Type type)
		: base(type)
	{
	}

	protected override CollectionForm CreateCollectionForm()
	{
		usedAttachmentNames = new List<string>();
		foreach (MailAttachment item in base.Context.PropertyDescriptor.GetValue(base.Context.Instance) as List<MailAttachment>)
		{
			usedAttachmentNames.Add(item.Name);
		}
		CollectionForm collectionForm = base.CreateCollectionForm();
		collectionForm.FormClosed += collectionForm_FormClosed;
		return collectionForm;
	}

	private void collectionForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if ((sender as CollectionForm).DialogResult == DialogResult.OK)
		{
			base.Context.PropertyDescriptor.SetValue(base.Context.Instance, resultList);
		}
	}

	protected override object SetItems(object editValue, object[] value)
	{
		resultList = new List<MailAttachment>();
		for (int i = 0; i < value.Length; i++)
		{
			MailAttachment item = (MailAttachment)value[i];
			resultList.Add(item);
		}
		return base.SetItems(editValue, value);
	}

	protected override string GetDisplayText(object value)
	{
		MailAttachment mailAttachment = value as MailAttachment;
		if (!mailAttachment.IsNameChangingHandled())
		{
			mailAttachment.NameChanging += attachment_NameChanging;
		}
		if (!mailAttachment.IsNameChangedHandled())
		{
			mailAttachment.NameChanged += attachment_NameChanged;
		}
		if (mailAttachment.GetContainerActivity() == null)
		{
			mailAttachment.SetContainerActivity(base.Context.Instance as IVadActivity);
		}
		if (string.IsNullOrEmpty(mailAttachment.Name))
		{
			mailAttachment.Name = getNewMailAttachmentName();
		}
		return mailAttachment.Name;
	}
}
