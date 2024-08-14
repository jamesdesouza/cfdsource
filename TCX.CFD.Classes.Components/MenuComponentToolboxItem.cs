using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class MenuComponentToolboxItem : ActivityToolboxItem
{
	private void AddValidOption(MenuComponent activity, MenuOptions menuOption)
	{
		MenuComponentBranch menuComponentBranch = new MenuComponentBranch();
		menuComponentBranch.Option = menuOption;
		activity.Activities.Add(menuComponentBranch);
	}

	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		MenuComponent menuComponent = new MenuComponent();
		menuComponent.AcceptDtmfInput = Settings.Default.MenuTemplateAcceptDtmfInput;
		menuComponent.Timeout = Settings.Default.MenuTemplateTimeout;
		menuComponent.MaxRetryCount = Settings.Default.MenuTemplateMaxRetryCount;
		if (Settings.Default.MenuTemplateIsValidOption0)
		{
			AddValidOption(menuComponent, MenuOptions.Option0);
		}
		if (Settings.Default.MenuTemplateIsValidOption1)
		{
			AddValidOption(menuComponent, MenuOptions.Option1);
		}
		if (Settings.Default.MenuTemplateIsValidOption2)
		{
			AddValidOption(menuComponent, MenuOptions.Option2);
		}
		if (Settings.Default.MenuTemplateIsValidOption3)
		{
			AddValidOption(menuComponent, MenuOptions.Option3);
		}
		if (Settings.Default.MenuTemplateIsValidOption4)
		{
			AddValidOption(menuComponent, MenuOptions.Option4);
		}
		if (Settings.Default.MenuTemplateIsValidOption5)
		{
			AddValidOption(menuComponent, MenuOptions.Option5);
		}
		if (Settings.Default.MenuTemplateIsValidOption6)
		{
			AddValidOption(menuComponent, MenuOptions.Option6);
		}
		if (Settings.Default.MenuTemplateIsValidOption7)
		{
			AddValidOption(menuComponent, MenuOptions.Option7);
		}
		if (Settings.Default.MenuTemplateIsValidOption8)
		{
			AddValidOption(menuComponent, MenuOptions.Option8);
		}
		if (Settings.Default.MenuTemplateIsValidOption9)
		{
			AddValidOption(menuComponent, MenuOptions.Option9);
		}
		if (Settings.Default.MenuTemplateIsValidOptionStar)
		{
			AddValidOption(menuComponent, MenuOptions.OptionStar);
		}
		if (Settings.Default.MenuTemplateIsValidOptionPound)
		{
			AddValidOption(menuComponent, MenuOptions.OptionPound);
		}
		MenuComponentBranch menuComponentBranch = new MenuComponentBranch();
		menuComponentBranch.Option = MenuOptions.TimeoutOrInvalidOption;
		menuComponent.Activities.Add(menuComponentBranch);
		FlowDesignerNameCreator.CreateName("Menu", host.Container, menuComponent);
		return new IComponent[1] { menuComponent };
	}
}
