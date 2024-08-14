using System.ComponentModel;
using System.Workflow.ComponentModel;

namespace TCX.CFD.Classes.FlowDesigner;

internal class FlowDesignerNameCreator
{
	public static void CreateName(string baseName, IContainer container, Activity activity)
	{
		int num = 0;
		string name;
		do
		{
			int num2 = ++num;
			name = baseName + num2;
		}
		while (container.Components[name] != null);
		activity.Name = name;
	}
}
