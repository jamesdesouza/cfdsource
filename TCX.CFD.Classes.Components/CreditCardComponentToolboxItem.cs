using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class CreditCardComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CreditCardComponent creditCardComponent = new CreditCardComponent
		{
			RequestNumber = (UserInputComponentToolboxItem.CreateDefaultComponent(host) as UserInputComponent),
			RequestExpiration = (UserInputComponentToolboxItem.CreateDefaultComponent(host) as UserInputComponent),
			RequestSecurityCode = (UserInputComponentToolboxItem.CreateDefaultComponent(host) as UserInputComponent),
			IsExpirationRequired = Settings.Default.CreditCardTemplateIsExpirationRequired,
			IsSecurityCodeRequired = Settings.Default.CreditCardTemplateIsSecurityCodeRequired,
			MaxRetryCount = Settings.Default.CreditCardTemplateMaxRetryCount
		};
		creditCardComponent.RequestNumber.MinDigits = 8u;
		creditCardComponent.RequestNumber.MaxDigits = 19u;
		creditCardComponent.RequestExpiration.MinDigits = 4u;
		creditCardComponent.RequestExpiration.MaxDigits = 4u;
		creditCardComponent.RequestSecurityCode.MinDigits = 3u;
		creditCardComponent.RequestSecurityCode.MaxDigits = 4u;
		ComponentBranch componentBranch = new ComponentBranch();
		ComponentBranch componentBranch2 = new ComponentBranch();
		componentBranch.DisplayedText = "Valid Input";
		componentBranch2.DisplayedText = "Invalid Input";
		creditCardComponent.Activities.Add(componentBranch);
		creditCardComponent.Activities.Add(componentBranch2);
		FlowDesignerNameCreator.CreateName("CreditCard", host.Container, creditCardComponent);
		return new IComponent[1] { creditCardComponent };
	}
}
