using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace TCX.CFD.Classes.Editors;

public class FolderSelectorEditor : UITypeEditor
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
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = context.PropertyDescriptor.GetValue(context.Instance) as string;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				return folderBrowserDialog.SelectedPath;
			}
		}
		return base.EditValue(context, provider, value);
	}
}
