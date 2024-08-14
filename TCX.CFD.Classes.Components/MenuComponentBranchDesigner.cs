using System.Collections;

namespace TCX.CFD.Classes.Components;

public class MenuComponentBranchDesigner : ComponentBranchDesigner
{
	public override string Text
	{
		get
		{
			MenuComponentBranch menuComponentBranch = base.Activity as MenuComponentBranch;
			return menuComponentBranch.Option switch
			{
				MenuOptions.Option0 => "Option 0" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option1 => "Option 1" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option2 => "Option 2" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option3 => "Option 3" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option4 => "Option 4" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option5 => "Option 5" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option6 => "Option 6" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option7 => "Option 7" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option8 => "Option 8" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.Option9 => "Option 9" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.OptionStar => "Option Star" + GetFriendlyNameText(menuComponentBranch), 
				MenuOptions.OptionPound => "Option Pound" + GetFriendlyNameText(menuComponentBranch), 
				_ => "Timeout or Invalid Option", 
			};
		}
		protected set
		{
			base.Text = value;
		}
	}

	public MenuComponentBranchDesigner()
		: base(canBeMoved: false)
	{
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.HideProperty(properties, "Name");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General", isReadOnly: true);
		base.PostFilterProperties(properties);
	}

	private string GetFriendlyNameText(MenuComponentBranch branch)
	{
		if (string.IsNullOrEmpty(branch.FriendlyName))
		{
			if (string.IsNullOrEmpty(branch.Tag))
			{
				return "";
			}
			return " (" + branch.Tag + ")";
		}
		return " (" + branch.FriendlyName + ")";
	}
}
