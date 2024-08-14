using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(MenuComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(MenuComponentToolboxItem))]
[ToolboxBitmap(typeof(MenuComponent), "Resources.Menu.png")]
[ActivityValidator(typeof(MenuComponentValidator))]
public class MenuComponent : AbsVadCompositeActivity
{
	private List<Prompt> initialPrompts = new List<Prompt>();

	private List<Prompt> subsequentPrompts = new List<Prompt>();

	private List<Prompt> timeoutPrompts = new List<Prompt>();

	private List<Prompt> invalidDigitPrompts = new List<Prompt>();

	private bool acceptDtmfInput = true;

	private bool isValidOption_0;

	private bool isValidOption_1;

	private bool isValidOption_2;

	private bool isValidOption_3;

	private bool isValidOption_4;

	private bool isValidOption_5;

	private bool isValidOption_6;

	private bool isValidOption_7;

	private bool isValidOption_8;

	private bool isValidOption_9;

	private bool isValidOption_Star;

	private bool isValidOption_Pound;

	private uint timeout = 5u;

	private uint maxRetryCount = 3u;

	private readonly XmlSerializer promptSerializer = new XmlSerializer(typeof(List<Prompt>));

	[Browsable(false)]
	public string InitialPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, initialPrompts);
		}
		set
		{
			initialPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Prompts")]
	[Description("The list of prompts to play the first time the menu is executed.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> InitialPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt initialPrompt in initialPrompts)
			{
				list.Add(initialPrompt.Clone());
			}
			return list;
		}
		set
		{
			initialPrompts = value;
		}
	}

	[Browsable(false)]
	public string SubsequentPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, subsequentPrompts);
		}
		set
		{
			subsequentPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Prompts")]
	[Description("The list of prompts to play subsequent times.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> SubsequentPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt subsequentPrompt in subsequentPrompts)
			{
				list.Add(subsequentPrompt.Clone());
			}
			return list;
		}
		set
		{
			subsequentPrompts = value;
		}
	}

	[Browsable(false)]
	public string TimeoutPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, timeoutPrompts);
		}
		set
		{
			timeoutPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Prompts")]
	[Description("The list of prompts to play when no user input is detected.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> TimeoutPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt timeoutPrompt in timeoutPrompts)
			{
				list.Add(timeoutPrompt.Clone());
			}
			return list;
		}
		set
		{
			timeoutPrompts = value;
		}
	}

	[Browsable(false)]
	public string InvalidDigitPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, invalidDigitPrompts);
		}
		set
		{
			invalidDigitPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Prompts")]
	[Description("The list of prompts to play when the user enters an invalid digit.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> InvalidDigitPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt invalidDigitPrompt in invalidDigitPrompts)
			{
				list.Add(invalidDigitPrompt.Clone());
			}
			return list;
		}
		set
		{
			invalidDigitPrompts = value;
		}
	}

	[Category("Menu")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool AcceptDtmfInput
	{
		get
		{
			return acceptDtmfInput;
		}
		set
		{
			acceptDtmfInput = value;
		}
	}

	[Category("Options")]
	[Description("Specify the digit that the user must press in order to repeat the menu prompt.")]
	public DtmfDigits RepeatOption { get; set; } = DtmfDigits.None;


	[Category("Options")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid option.")]
	public bool IsValidOption_0
	{
		get
		{
			return isValidOption_0;
		}
		set
		{
			if (isValidOption_0 != value)
			{
				isValidOption_0 = value;
				if (isValidOption_0)
				{
					AddOption(MenuOptions.Option0);
				}
				else
				{
					RemoveOption(MenuOptions.Option0);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid option.")]
	public bool IsValidOption_1
	{
		get
		{
			return isValidOption_1;
		}
		set
		{
			if (isValidOption_1 != value)
			{
				isValidOption_1 = value;
				if (isValidOption_1)
				{
					AddOption(MenuOptions.Option1);
				}
				else
				{
					RemoveOption(MenuOptions.Option1);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid option.")]
	public bool IsValidOption_2
	{
		get
		{
			return isValidOption_2;
		}
		set
		{
			if (isValidOption_2 != value)
			{
				isValidOption_2 = value;
				if (isValidOption_2)
				{
					AddOption(MenuOptions.Option2);
				}
				else
				{
					RemoveOption(MenuOptions.Option2);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid option.")]
	public bool IsValidOption_3
	{
		get
		{
			return isValidOption_3;
		}
		set
		{
			if (isValidOption_3 != value)
			{
				isValidOption_3 = value;
				if (isValidOption_3)
				{
					AddOption(MenuOptions.Option3);
				}
				else
				{
					RemoveOption(MenuOptions.Option3);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid option.")]
	public bool IsValidOption_4
	{
		get
		{
			return isValidOption_4;
		}
		set
		{
			if (isValidOption_4 != value)
			{
				isValidOption_4 = value;
				if (isValidOption_4)
				{
					AddOption(MenuOptions.Option4);
				}
				else
				{
					RemoveOption(MenuOptions.Option4);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid option.")]
	public bool IsValidOption_5
	{
		get
		{
			return isValidOption_5;
		}
		set
		{
			if (isValidOption_5 != value)
			{
				isValidOption_5 = value;
				if (isValidOption_5)
				{
					AddOption(MenuOptions.Option5);
				}
				else
				{
					RemoveOption(MenuOptions.Option5);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid option.")]
	public bool IsValidOption_6
	{
		get
		{
			return isValidOption_6;
		}
		set
		{
			if (isValidOption_6 != value)
			{
				isValidOption_6 = value;
				if (isValidOption_6)
				{
					AddOption(MenuOptions.Option6);
				}
				else
				{
					RemoveOption(MenuOptions.Option6);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid option.")]
	public bool IsValidOption_7
	{
		get
		{
			return isValidOption_7;
		}
		set
		{
			if (isValidOption_7 != value)
			{
				isValidOption_7 = value;
				if (isValidOption_7)
				{
					AddOption(MenuOptions.Option7);
				}
				else
				{
					RemoveOption(MenuOptions.Option7);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid option.")]
	public bool IsValidOption_8
	{
		get
		{
			return isValidOption_8;
		}
		set
		{
			if (isValidOption_8 != value)
			{
				isValidOption_8 = value;
				if (isValidOption_8)
				{
					AddOption(MenuOptions.Option8);
				}
				else
				{
					RemoveOption(MenuOptions.Option8);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid option.")]
	public bool IsValidOption_9
	{
		get
		{
			return isValidOption_9;
		}
		set
		{
			if (isValidOption_9 != value)
			{
				isValidOption_9 = value;
				if (isValidOption_9)
				{
					AddOption(MenuOptions.Option9);
				}
				else
				{
					RemoveOption(MenuOptions.Option9);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid option.")]
	public bool IsValidOption_Star
	{
		get
		{
			return isValidOption_Star;
		}
		set
		{
			if (isValidOption_Star != value)
			{
				isValidOption_Star = value;
				if (isValidOption_Star)
				{
					AddOption(MenuOptions.OptionStar);
				}
				else
				{
					RemoveOption(MenuOptions.OptionStar);
				}
			}
		}
	}

	[Category("Options")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid option.")]
	public bool IsValidOption_Pound
	{
		get
		{
			return isValidOption_Pound;
		}
		set
		{
			if (isValidOption_Pound != value)
			{
				isValidOption_Pound = value;
				if (isValidOption_Pound)
				{
					AddOption(MenuOptions.OptionPound);
				}
				else
				{
					RemoveOption(MenuOptions.OptionPound);
				}
			}
		}
	}

	[Category("Menu")]
	[Description("The time to wait for user input before playing the specified timeout prompts, in seconds.")]
	public uint Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			timeout = value;
		}
	}

	[Category("Menu")]
	[Description("Quantity of retry attempts for invalid input or timeout (single counter).")]
	public uint MaxRetryCount
	{
		get
		{
			return maxRetryCount;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			maxRetryCount = value;
		}
	}

	private void AddOption(MenuOptions menuOption)
	{
		MenuComponentBranch menuComponentBranch = new MenuComponentBranch();
		menuComponentBranch.Option = menuOption;
		int num = 0;
		using (IEnumerator<Activity> enumerator = base.Activities.GetEnumerator())
		{
			while (enumerator.MoveNext() && ((MenuComponentBranch)enumerator.Current).Option <= menuOption)
			{
				num++;
			}
		}
		List<Activity> list = new List<Activity> { menuComponentBranch };
		if (base.Site != null && base.Site.Container is IDesignerHost designerHost && designerHost.GetDesigner(this) is MenuComponentDesigner compositeActivityDesigner)
		{
			CompositeActivityDesigner.InsertActivities(compositeActivityDesigner, new ConnectorHitTestInfo(compositeActivityDesigner, HitTestLocations.Connector, num), new ReadOnlyCollection<Activity>(list), "Adding option");
		}
	}

	private void RemoveOption(MenuOptions menuOption)
	{
		foreach (MenuComponentBranch activity in base.Activities)
		{
			if (activity.Option == menuOption)
			{
				List<Activity> list = new List<Activity> { activity };
				CompositeActivityDesigner.RemoveActivities(base.Site.Container as IServiceProvider, new ReadOnlyCollection<Activity>(list), "Removing option");
				break;
			}
		}
	}

	private void ActivitiesChanged(object sender, ActivityCollectionChangeEventArgs e)
	{
		if (e.Action != 0)
		{
			return;
		}
		using IEnumerator<Activity> enumerator = e.AddedItems.GetEnumerator();
		while (enumerator.MoveNext())
		{
			switch (((MenuComponentBranch)enumerator.Current).Option)
			{
			case MenuOptions.Option0:
				isValidOption_0 = true;
				break;
			case MenuOptions.Option1:
				isValidOption_1 = true;
				break;
			case MenuOptions.Option2:
				isValidOption_2 = true;
				break;
			case MenuOptions.Option3:
				isValidOption_3 = true;
				break;
			case MenuOptions.Option4:
				isValidOption_4 = true;
				break;
			case MenuOptions.Option5:
				isValidOption_5 = true;
				break;
			case MenuOptions.Option6:
				isValidOption_6 = true;
				break;
			case MenuOptions.Option7:
				isValidOption_7 = true;
				break;
			case MenuOptions.Option8:
				isValidOption_8 = true;
				break;
			case MenuOptions.Option9:
				isValidOption_9 = true;
				break;
			case MenuOptions.OptionStar:
				isValidOption_Star = true;
				break;
			case MenuOptions.OptionPound:
				isValidOption_Pound = true;
				break;
			}
		}
	}

	public MenuComponent()
	{
		InitializeComponent();
		base.Activities.ListChanged += ActivitiesChanged;
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Menu.ResultHelpText")
		};
		Variable item2 = new Variable("SelectedOption", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Menu.SelectedOptionHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (Prompt initialPrompt in initialPrompts)
		{
			initialPrompt.ContainerActivity = this;
			initialPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		foreach (Prompt subsequentPrompt in subsequentPrompts)
		{
			subsequentPrompt.ContainerActivity = this;
			subsequentPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		foreach (Prompt timeoutPrompt in timeoutPrompts)
		{
			timeoutPrompt.ContainerActivity = this;
			timeoutPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		foreach (Prompt invalidDigitPrompt in invalidDigitPrompts)
		{
			invalidDigitPrompt.ContainerActivity = this;
			invalidDigitPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		base.NotifyComponentRenamed(oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (Prompt initialPrompt in initialPrompts)
		{
			initialPrompt.ContainerActivity = this;
			initialPrompt.MigrateConstantStringExpressions();
		}
		foreach (Prompt subsequentPrompt in subsequentPrompts)
		{
			subsequentPrompt.ContainerActivity = this;
			subsequentPrompt.MigrateConstantStringExpressions();
		}
		foreach (Prompt timeoutPrompt in timeoutPrompts)
		{
			timeoutPrompt.ContainerActivity = this;
			timeoutPrompt.MigrateConstantStringExpressions();
		}
		foreach (Prompt invalidDigitPrompt in invalidDigitPrompts)
		{
			invalidDigitPrompt.ContainerActivity = this;
			invalidDigitPrompt.MigrateConstantStringExpressions();
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new MenuComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.tne2kc81ot15");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "MenuComponent";
	}
}
