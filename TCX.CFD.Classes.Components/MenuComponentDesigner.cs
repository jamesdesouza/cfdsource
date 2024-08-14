using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(MenuComponentTheme))]
public class MenuComponentDesigner : CompositeComponentDesigner
{
	private ActivityDesignerVerbCollection verbs;

	private ActivityDesignerVerb addOption0Verb;

	private ActivityDesignerVerb addOption1Verb;

	private ActivityDesignerVerb addOption2Verb;

	private ActivityDesignerVerb addOption3Verb;

	private ActivityDesignerVerb addOption4Verb;

	private ActivityDesignerVerb addOption5Verb;

	private ActivityDesignerVerb addOption6Verb;

	private ActivityDesignerVerb addOption7Verb;

	private ActivityDesignerVerb addOption8Verb;

	private ActivityDesignerVerb addOption9Verb;

	private ActivityDesignerVerb addOptionStarVerb;

	private ActivityDesignerVerb addOptionPoundVerb;

	protected override ActivityDesignerVerbCollection Verbs
	{
		get
		{
			if (verbs == null)
			{
				verbs = new ActivityDesignerVerbCollection();
				ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.ConfigureVerb"), OnConfigure);
				activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
				ActivityDesignerVerb activityDesignerVerb2 = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.OpenAudioFolderVerb"), OnOpenAudioFolder);
				activityDesignerVerb2.Properties.Add("Image", Resources.PromptPlayback);
				addOption0Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption0"), OnAddOption);
				addOption1Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption1"), OnAddOption);
				addOption2Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption2"), OnAddOption);
				addOption3Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption3"), OnAddOption);
				addOption4Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption4"), OnAddOption);
				addOption5Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption5"), OnAddOption);
				addOption6Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption6"), OnAddOption);
				addOption7Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption7"), OnAddOption);
				addOption8Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption8"), OnAddOption);
				addOption9Verb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOption9"), OnAddOption);
				addOptionStarVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOptionStar"), OnAddOption);
				addOptionPoundVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.Actions, LocalizedResourceMgr.GetString("ComponentDesigners.Menu.AddOptionPound"), OnAddOption);
				verbs.Add(activityDesignerVerb);
				verbs.Add(activityDesignerVerb2);
				verbs.Add(addOption0Verb);
				verbs.Add(addOption1Verb);
				verbs.Add(addOption2Verb);
				verbs.Add(addOption3Verb);
				verbs.Add(addOption4Verb);
				verbs.Add(addOption5Verb);
				verbs.Add(addOption6Verb);
				verbs.Add(addOption7Verb);
				verbs.Add(addOption8Verb);
				verbs.Add(addOption9Verb);
				verbs.Add(addOptionStarVerb);
				verbs.Add(addOptionPoundVerb);
			}
			return verbs;
		}
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is MenuComponent menuComponent))
		{
			return;
		}
		MenuConfigurationForm menuConfigurationForm = new MenuConfigurationForm(menuComponent);
		if (menuConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Activity);
				properties["AcceptDtmfInput"].SetValue(base.Activity, menuConfigurationForm.AcceptDtmfInput);
				properties["InitialPrompts"].SetValue(base.Activity, menuConfigurationForm.InitialPrompts);
				properties["SubsequentPrompts"].SetValue(base.Activity, menuConfigurationForm.SubsequentPrompts);
				properties["TimeoutPrompts"].SetValue(base.Activity, menuConfigurationForm.TimeoutPrompts);
				properties["InvalidDigitPrompts"].SetValue(base.Activity, menuConfigurationForm.InvalidDigitPrompts);
				properties["Timeout"].SetValue(base.Activity, menuConfigurationForm.Timeout);
				properties["MaxRetryCount"].SetValue(base.Activity, menuConfigurationForm.MaxRetryCount);
				properties["RepeatOption"].SetValue(base.Activity, menuConfigurationForm.RepeatOption);
				properties["IsValidOption_0"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_0);
				properties["IsValidOption_1"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_1);
				properties["IsValidOption_2"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_2);
				properties["IsValidOption_3"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_3);
				properties["IsValidOption_4"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_4);
				properties["IsValidOption_5"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_5);
				properties["IsValidOption_6"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_6);
				properties["IsValidOption_7"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_7);
				properties["IsValidOption_8"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_8);
				properties["IsValidOption_9"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_9);
				properties["IsValidOption_Star"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_Star);
				properties["IsValidOption_Pound"].SetValue(base.Activity, menuConfigurationForm.IsValidOption_Pound);
				designerTransaction.Commit();
			}
		}
	}

	private void OnOpenAudioFolder(object sender, EventArgs e)
	{
		RootFlow rootFlow = ((IVadActivity)base.Activity).GetRootFlow();
		if (rootFlow == null)
		{
			return;
		}
		FileObject fileObject = rootFlow.FileObject;
		if (fileObject != null)
		{
			ProjectObject projectObject = fileObject.GetProjectObject();
			if (projectObject != null)
			{
				Process.Start(Path.Combine(projectObject.GetFolderPath(), "Audio"));
			}
		}
	}

	private void OnAddOption(object sender, EventArgs e)
	{
		MenuComponent menuComponent = base.Activity as MenuComponent;
		if (sender == addOption0Verb)
		{
			menuComponent.IsValidOption_0 = true;
		}
		else if (sender == addOption1Verb)
		{
			menuComponent.IsValidOption_1 = true;
		}
		else if (sender == addOption2Verb)
		{
			menuComponent.IsValidOption_2 = true;
		}
		else if (sender == addOption3Verb)
		{
			menuComponent.IsValidOption_3 = true;
		}
		else if (sender == addOption4Verb)
		{
			menuComponent.IsValidOption_4 = true;
		}
		else if (sender == addOption5Verb)
		{
			menuComponent.IsValidOption_5 = true;
		}
		else if (sender == addOption6Verb)
		{
			menuComponent.IsValidOption_6 = true;
		}
		else if (sender == addOption7Verb)
		{
			menuComponent.IsValidOption_7 = true;
		}
		else if (sender == addOption8Verb)
		{
			menuComponent.IsValidOption_8 = true;
		}
		else if (sender == addOption9Verb)
		{
			menuComponent.IsValidOption_9 = true;
		}
		else if (sender == addOptionStarVerb)
		{
			menuComponent.IsValidOption_Star = true;
		}
		else if (sender == addOptionPoundVerb)
		{
			menuComponent.IsValidOption_Pound = true;
		}
	}

	private void ShowHideVerbs()
	{
		if (verbs == null)
		{
			verbs = Verbs;
		}
		addOption0Verb.Visible = true;
		addOption1Verb.Visible = true;
		addOption2Verb.Visible = true;
		addOption3Verb.Visible = true;
		addOption4Verb.Visible = true;
		addOption5Verb.Visible = true;
		addOption6Verb.Visible = true;
		addOption7Verb.Visible = true;
		addOption8Verb.Visible = true;
		addOption9Verb.Visible = true;
		addOptionStarVerb.Visible = true;
		addOptionPoundVerb.Visible = true;
		using IEnumerator<Activity> enumerator = (base.Activity as MenuComponent).Activities.GetEnumerator();
		while (enumerator.MoveNext())
		{
			switch (((MenuComponentBranch)enumerator.Current).Option)
			{
			case MenuOptions.Option0:
				addOption0Verb.Visible = false;
				break;
			case MenuOptions.Option1:
				addOption1Verb.Visible = false;
				break;
			case MenuOptions.Option2:
				addOption2Verb.Visible = false;
				break;
			case MenuOptions.Option3:
				addOption3Verb.Visible = false;
				break;
			case MenuOptions.Option4:
				addOption4Verb.Visible = false;
				break;
			case MenuOptions.Option5:
				addOption5Verb.Visible = false;
				break;
			case MenuOptions.Option6:
				addOption6Verb.Visible = false;
				break;
			case MenuOptions.Option7:
				addOption7Verb.Visible = false;
				break;
			case MenuOptions.Option8:
				addOption8Verb.Visible = false;
				break;
			case MenuOptions.Option9:
				addOption9Verb.Visible = false;
				break;
			case MenuOptions.OptionStar:
				addOptionStarVerb.Visible = false;
				break;
			case MenuOptions.OptionPound:
				addOptionPoundVerb.Visible = false;
				break;
			}
		}
	}

	private void ActivitiesChanged(object sender, ActivityCollectionChangeEventArgs e)
	{
		MenuComponent menuComponent = base.Activity as MenuComponent;
		if (e.Action == ActivityCollectionChangeAction.Remove)
		{
			using IEnumerator<Activity> enumerator = e.RemovedItems.GetEnumerator();
			while (enumerator.MoveNext())
			{
				switch (((MenuComponentBranch)enumerator.Current).Option)
				{
				case MenuOptions.Option0:
					menuComponent.IsValidOption_0 = false;
					break;
				case MenuOptions.Option1:
					menuComponent.IsValidOption_1 = false;
					break;
				case MenuOptions.Option2:
					menuComponent.IsValidOption_2 = false;
					break;
				case MenuOptions.Option3:
					menuComponent.IsValidOption_3 = false;
					break;
				case MenuOptions.Option4:
					menuComponent.IsValidOption_4 = false;
					break;
				case MenuOptions.Option5:
					menuComponent.IsValidOption_5 = false;
					break;
				case MenuOptions.Option6:
					menuComponent.IsValidOption_6 = false;
					break;
				case MenuOptions.Option7:
					menuComponent.IsValidOption_7 = false;
					break;
				case MenuOptions.Option8:
					menuComponent.IsValidOption_8 = false;
					break;
				case MenuOptions.Option9:
					menuComponent.IsValidOption_9 = false;
					break;
				case MenuOptions.OptionStar:
					menuComponent.IsValidOption_Star = false;
					break;
				case MenuOptions.OptionPound:
					menuComponent.IsValidOption_Pound = false;
					break;
				}
			}
		}
		else if (e.Action == ActivityCollectionChangeAction.Add)
		{
			menuComponent.Activities.Sort();
		}
		ShowHideVerbs();
	}

	protected override void Initialize(Activity activity)
	{
		base.Initialize(activity);
		(activity as MenuComponent).Activities.ListChanged += ActivitiesChanged;
		ShowHideVerbs();
	}

	public override bool CanRemoveActivities(ReadOnlyCollection<Activity> activitiesToRemove)
	{
		foreach (MenuComponentBranch item in activitiesToRemove)
		{
			if (item.Option == MenuOptions.TimeoutOrInvalidOption)
			{
				return false;
			}
		}
		return true;
	}

	public override bool CanInsertActivities(HitTestInfo insertLocation, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		MenuComponent menuComponent = base.Activity as MenuComponent;
		foreach (Activity item in activitiesToInsert)
		{
			if (!(item is MenuComponentBranch))
			{
				return false;
			}
			switch ((item as MenuComponentBranch).Option)
			{
			case MenuOptions.Option0:
				if (menuComponent.IsValidOption_0)
				{
					return false;
				}
				break;
			case MenuOptions.Option1:
				if (menuComponent.IsValidOption_1)
				{
					return false;
				}
				break;
			case MenuOptions.Option2:
				if (menuComponent.IsValidOption_2)
				{
					return false;
				}
				break;
			case MenuOptions.Option3:
				if (menuComponent.IsValidOption_3)
				{
					return false;
				}
				break;
			case MenuOptions.Option4:
				if (menuComponent.IsValidOption_4)
				{
					return false;
				}
				break;
			case MenuOptions.Option5:
				if (menuComponent.IsValidOption_5)
				{
					return false;
				}
				break;
			case MenuOptions.Option6:
				if (menuComponent.IsValidOption_6)
				{
					return false;
				}
				break;
			case MenuOptions.Option7:
				if (menuComponent.IsValidOption_7)
				{
					return false;
				}
				break;
			case MenuOptions.Option8:
				if (menuComponent.IsValidOption_8)
				{
					return false;
				}
				break;
			case MenuOptions.Option9:
				if (menuComponent.IsValidOption_9)
				{
					return false;
				}
				break;
			case MenuOptions.OptionStar:
				if (menuComponent.IsValidOption_Star)
				{
					return false;
				}
				break;
			case MenuOptions.OptionPound:
				if (menuComponent.IsValidOption_Pound)
				{
					return false;
				}
				break;
			case MenuOptions.TimeoutOrInvalidOption:
				return false;
			}
		}
		RootFlow rootFlow = menuComponent.GetRootFlow();
		if (rootFlow != null)
		{
			FileObject fileObject = rootFlow.FileObject;
			if (fileObject != null)
			{
				if (fileObject is DialerFileObject)
				{
					return ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(fileObject.GetProjectObject(), activitiesToInsert);
				}
				if (fileObject is ComponentFileObject)
				{
					return ComponentDesignerHelper.CanInsertActivitiesCheckingUserComponent(fileObject as ComponentFileObject, activitiesToInsert);
				}
			}
		}
		return true;
	}

	protected override void DoDefaultAction()
	{
		base.DoDefaultAction();
		OnConfigure(null, EventArgs.Empty);
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Menu", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}
}
