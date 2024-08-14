using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace TCX.CFD.Classes.FlowDesigner;

internal class FlowDesignerNameCreationService : INameCreationService
{
	public string CreateName(IContainer container, Type dataType)
	{
		int num = 0;
		string name = dataType.Name;
		string text;
		do
		{
			int num2 = ++num;
			text = name + num2;
		}
		while (container.Components[text] != null);
		return text;
	}

	public bool IsValidName(string name)
	{
		return true;
	}

	public void ValidateName(string name)
	{
		if (!IsValidName(name))
		{
			throw new ArgumentException("Invalid name: " + name);
		}
	}
}
