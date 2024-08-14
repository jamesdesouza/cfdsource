using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class FileManagementComponentToolboxItem : ActivityToolboxItem
{
	private FileManagementOpenModes GetOpenMode(string str)
	{
		return str switch
		{
			"Append" => FileManagementOpenModes.Append, 
			"Create" => FileManagementOpenModes.Create, 
			"CreateNew" => FileManagementOpenModes.CreateNew, 
			"Open" => FileManagementOpenModes.Open, 
			"OpenOrCreate" => FileManagementOpenModes.OpenOrCreate, 
			"Truncate" => FileManagementOpenModes.Truncate, 
			_ => FileManagementOpenModes.Open, 
		};
	}

	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		FileManagementComponent fileManagementComponent = new FileManagementComponent
		{
			OpenMode = GetOpenMode(Settings.Default.FileManagementTemplateOpenMode),
			Action = ((!(Settings.Default.FileManagementTemplateAction == "Read")) ? FileManagementActions.Write : FileManagementActions.Read)
		};
		FlowDesignerNameCreator.CreateName("ReadWriteFile", host.Container, fileManagementComponent);
		return new IComponent[1] { fileManagementComponent };
	}
}
