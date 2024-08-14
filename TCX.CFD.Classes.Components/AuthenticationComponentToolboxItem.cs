using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class AuthenticationComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		AuthenticationComponent authenticationComponent = new AuthenticationComponent
		{
			RequestID = (UserInputComponentToolboxItem.CreateDefaultComponent(host) as UserInputComponent),
			RequestPIN = (UserInputComponentToolboxItem.CreateDefaultComponent(host) as UserInputComponent),
			IsPinRequired = Settings.Default.AuthenticationTemplateIsPinRequired,
			MaxRetryCount = Settings.Default.AuthenticationTemplateMaxRetryCount
		};
		ComponentBranch componentBranch = new ComponentBranch();
		ComponentBranch componentBranch2 = new ComponentBranch();
		componentBranch.DisplayedText = "Valid Input";
		componentBranch2.DisplayedText = "Invalid Input";
		authenticationComponent.Activities.Add(componentBranch);
		authenticationComponent.Activities.Add(componentBranch2);
		FlowDesignerNameCreator.CreateName("Authentication", host.Container, authenticationComponent);
		return new IComponent[1] { authenticationComponent };
	}
}
