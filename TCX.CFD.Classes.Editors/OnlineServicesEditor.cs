using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;

namespace TCX.CFD.Classes.Editors;

public class OnlineServicesEditor : UITypeEditor
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
		if (context != null && provider != null && provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService windowsFormsEditorService)
		{
			OnlineServicesConfigurationForm onlineServicesConfigurationForm = new OnlineServicesConfigurationForm
			{
				OnlineServices = (value as OnlineServices)
			};
			if (windowsFormsEditorService.ShowDialog(onlineServicesConfigurationForm) == DialogResult.OK)
			{
				return onlineServicesConfigurationForm.OnlineServices;
			}
		}
		return base.EditValue(context, provider, value);
	}
}
