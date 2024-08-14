using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

public class UserComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		if (host.GetService(typeof(IToolboxService)) is ToolBox { SelectedNode: not null } toolBox)
		{
			UserComponent userComponent = new UserComponent();
			TreeNode selectedNode = toolBox.SelectedNode;
			userComponent.FileObject = selectedNode.Tag as ComponentFileObject;
			FlowDesignerNameCreator.CreateName(selectedNode.Text, host.Container, userComponent);
			return new IComponent[1] { userComponent };
		}
		return Array.Empty<IComponent>();
	}
}
