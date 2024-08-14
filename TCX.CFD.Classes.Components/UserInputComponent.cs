using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(UserInputComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(UserInputComponentToolboxItem))]
[ToolboxBitmap(typeof(UserInputComponent), "Resources.UserInput.png")]
[ActivityValidator(typeof(UserInputComponentValidator))]
public class UserInputComponent : AbsVadCompositeActivity
{
	private List<Prompt> initialPrompts = new List<Prompt>();

	private List<Prompt> subsequentPrompts = new List<Prompt>();

	private List<Prompt> timeoutPrompts = new List<Prompt>();

	private List<Prompt> invalidDigitPrompts = new List<Prompt>();

	private uint firstDigitTimeout = 5u;

	private uint interDigitTimeout = 3u;

	private uint finalDigitTimeout = 2u;

	private uint maxRetryCount = 3u;

	private uint minDigits = 3u;

	private uint maxDigits = 6u;

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
	[Description("The list of prompts to play the first time the user input is executed.")]
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
	[Description("The list of prompts to play when the user enters an invalid digit or less digits than MinDigits.")]
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

	[Category("User Input")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool AcceptDtmfInput { get; set; } = true;


	[Category("Timeout")]
	[Description("The time to wait for the first digit before playing the specified timeout prompts, in seconds.")]
	public uint FirstDigitTimeout
	{
		get
		{
			return firstDigitTimeout;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			firstDigitTimeout = value;
		}
	}

	[Category("Timeout")]
	[Description("The time to wait for subsequent digits before playing the specified invalid digits prompts, in seconds.")]
	public uint InterDigitTimeout
	{
		get
		{
			return interDigitTimeout;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			interDigitTimeout = value;
		}
	}

	[Category("Timeout")]
	[Description("The time to wait for digits after MinDigits has been reached, before returning the entered data, in seconds.")]
	public uint FinalDigitTimeout
	{
		get
		{
			return finalDigitTimeout;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			finalDigitTimeout = value;
		}
	}

	[Category("User Input")]
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

	[Category("Digits")]
	[Description("The minimum quantity of digits that must be entered by the user.")]
	public uint MinDigits
	{
		get
		{
			return minDigits;
		}
		set
		{
			if (value == 0 || value > 999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 999));
			}
			minDigits = value;
		}
	}

	[Category("Digits")]
	[Description("The maximum quantity of digits that can be entered by the user.")]
	public uint MaxDigits
	{
		get
		{
			return maxDigits;
		}
		set
		{
			if (value == 0 || value > 999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 999));
			}
			maxDigits = value;
		}
	}

	[Category("Digits")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits StopDigit { get; set; } = DtmfDigits.None;


	[Category("Digits")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_0 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_1 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_2 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_3 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_4 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_5 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_6 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_7 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_8 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_9 { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool IsValidDigit_Star { get; set; }

	[Category("Digits")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool IsValidDigit_Pound { get; set; }

	public UserInputComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.UserInput.ResultHelpText")
		};
		Variable item2 = new Variable("Buffer", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.UserInput.BufferHelpText")
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
		return new UserInputComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.uj9rte6to4je");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "UserInputComponent";
	}
}
