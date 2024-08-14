using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TCX.CFD.Classes.Editors;

public class RuntimeServiceProvider : IServiceProvider, ITypeDescriptorContext
{
	private class WindowsFormsEditorService : IWindowsFormsEditorService
	{
		public void DropDownControl(Control control)
		{
		}

		public void CloseDropDown()
		{
		}

		public DialogResult ShowDialog(Form dialog)
		{
			return dialog.ShowDialog();
		}
	}

	private readonly PropertyDescriptor pd;

	private readonly object instance;

	public IContainer Container => null;

	public object Instance => instance;

	public PropertyDescriptor PropertyDescriptor => pd;

	public RuntimeServiceProvider(PropertyDescriptor pd, object instance)
	{
		this.pd = pd;
		this.instance = instance;
	}

	object IServiceProvider.GetService(Type serviceType)
	{
		if (serviceType == typeof(IWindowsFormsEditorService))
		{
			return new WindowsFormsEditorService();
		}
		return null;
	}

	public void OnComponentChanged()
	{
	}

	public bool OnComponentChanging()
	{
		return true;
	}
}
