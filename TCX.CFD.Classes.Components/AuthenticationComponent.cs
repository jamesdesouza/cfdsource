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
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(AuthenticationComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(AuthenticationComponentToolboxItem))]
[ToolboxBitmap(typeof(AuthenticationComponent), "Resources.Authentication.png")]
[ActivityValidator(typeof(AuthenticationComponentValidator))]
public class AuthenticationComponent : AbsVadCompositeActivity
{
	private uint maxRetryCount = 3u;

	[Browsable(false)]
	public UserInputComponent RequestID { get; set; } = new UserInputComponent();


	[Browsable(false)]
	public string RequestIdInitialPromptList
	{
		get
		{
			return RequestID.InitialPromptList;
		}
		set
		{
			RequestID.InitialPromptList = value;
		}
	}

	[Category("Request ID")]
	[Description("The list of prompts to play the first time the user input is executed.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestIdInitialPrompts
	{
		get
		{
			return RequestID.InitialPrompts;
		}
		set
		{
			RequestID.InitialPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestIdSubsequentPromptList
	{
		get
		{
			return RequestID.SubsequentPromptList;
		}
		set
		{
			RequestID.SubsequentPromptList = value;
		}
	}

	[Category("Request ID")]
	[Description("The list of prompts to play subsequent times.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestIdSubsequentPrompts
	{
		get
		{
			return RequestID.SubsequentPrompts;
		}
		set
		{
			RequestID.SubsequentPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestIdTimeoutPromptList
	{
		get
		{
			return RequestID.TimeoutPromptList;
		}
		set
		{
			RequestID.TimeoutPromptList = value;
		}
	}

	[Category("Request ID")]
	[Description("The list of prompts to play when no user input is detected.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestIdTimeoutPrompts
	{
		get
		{
			return RequestID.TimeoutPrompts;
		}
		set
		{
			RequestID.TimeoutPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestIdInvalidDigitPromptList
	{
		get
		{
			return RequestID.InvalidDigitPromptList;
		}
		set
		{
			RequestID.InvalidDigitPromptList = value;
		}
	}

	[Category("Request ID")]
	[Description("The list of prompts to play when the user enters an invalid digit or less digits than MinDigits.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestIdInvalidDigitPrompts
	{
		get
		{
			return RequestID.InvalidDigitPrompts;
		}
		set
		{
			RequestID.InvalidDigitPrompts = value;
		}
	}

	[Category("Request ID")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool RequestIdAcceptDtmfInput
	{
		get
		{
			return RequestID.AcceptDtmfInput;
		}
		set
		{
			RequestID.AcceptDtmfInput = value;
		}
	}

	[Category("Request ID")]
	[Description("The time to wait for the first digit before playing the specified timeout prompts, in seconds.")]
	public uint RequestIdFirstDigitTimeout
	{
		get
		{
			return RequestID.FirstDigitTimeout;
		}
		set
		{
			RequestID.FirstDigitTimeout = value;
		}
	}

	[Category("Request ID")]
	[Description("The time to wait for subsequent digits before playing the specified invalid digits prompts, in seconds.")]
	public uint RequestIdInterDigitTimeout
	{
		get
		{
			return RequestID.InterDigitTimeout;
		}
		set
		{
			RequestID.InterDigitTimeout = value;
		}
	}

	[Category("Request ID")]
	[Description("The time to wait for digits after MinDigits has been reached, before returning the entered data, in seconds.")]
	public uint RequestIdFinalDigitTimeout
	{
		get
		{
			return RequestID.FinalDigitTimeout;
		}
		set
		{
			RequestID.FinalDigitTimeout = value;
		}
	}

	[Category("Request ID")]
	[Description("Quantity of retry attempts for invalid input or timeout (single counter).")]
	public uint RequestIdMaxRetryCount
	{
		get
		{
			return RequestID.MaxRetryCount;
		}
		set
		{
			RequestID.MaxRetryCount = value;
		}
	}

	[Category("Request ID")]
	[Description("The minimum quantity of digits that must be entered by the user.")]
	public uint RequestIdMinDigits
	{
		get
		{
			return RequestID.MinDigits;
		}
		set
		{
			RequestID.MinDigits = value;
		}
	}

	[Category("Request ID")]
	[Description("The maximum quantity of digits that can be entered by the user.")]
	public uint RequestIdMaxDigits
	{
		get
		{
			return RequestID.MaxDigits;
		}
		set
		{
			RequestID.MaxDigits = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits RequestIdStopDigit
	{
		get
		{
			return RequestID.StopDigit;
		}
		set
		{
			RequestID.StopDigit = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_0
	{
		get
		{
			return RequestID.IsValidDigit_0;
		}
		set
		{
			RequestID.IsValidDigit_0 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_1
	{
		get
		{
			return RequestID.IsValidDigit_1;
		}
		set
		{
			RequestID.IsValidDigit_1 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_2
	{
		get
		{
			return RequestID.IsValidDigit_2;
		}
		set
		{
			RequestID.IsValidDigit_2 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_3
	{
		get
		{
			return RequestID.IsValidDigit_3;
		}
		set
		{
			RequestID.IsValidDigit_3 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_4
	{
		get
		{
			return RequestID.IsValidDigit_4;
		}
		set
		{
			RequestID.IsValidDigit_4 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_5
	{
		get
		{
			return RequestID.IsValidDigit_5;
		}
		set
		{
			RequestID.IsValidDigit_5 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_6
	{
		get
		{
			return RequestID.IsValidDigit_6;
		}
		set
		{
			RequestID.IsValidDigit_6 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_7
	{
		get
		{
			return RequestID.IsValidDigit_7;
		}
		set
		{
			RequestID.IsValidDigit_7 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_8
	{
		get
		{
			return RequestID.IsValidDigit_8;
		}
		set
		{
			RequestID.IsValidDigit_8 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_9
	{
		get
		{
			return RequestID.IsValidDigit_9;
		}
		set
		{
			RequestID.IsValidDigit_9 = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_Star
	{
		get
		{
			return RequestID.IsValidDigit_Star;
		}
		set
		{
			RequestID.IsValidDigit_Star = value;
		}
	}

	[Category("Request ID")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool RequestIdIsValidDigit_Pound
	{
		get
		{
			return RequestID.IsValidDigit_Pound;
		}
		set
		{
			RequestID.IsValidDigit_Pound = value;
		}
	}

	[Browsable(false)]
	public UserInputComponent RequestPIN { get; set; } = new UserInputComponent();


	[Browsable(false)]
	public string RequestPinInitialPromptList
	{
		get
		{
			return RequestPIN.InitialPromptList;
		}
		set
		{
			RequestPIN.InitialPromptList = value;
		}
	}

	[Category("Request PIN")]
	[Description("The list of prompts to play the first time the user input is executed.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestPinInitialPrompts
	{
		get
		{
			return RequestPIN.InitialPrompts;
		}
		set
		{
			RequestPIN.InitialPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestPinSubsequentPromptList
	{
		get
		{
			return RequestPIN.SubsequentPromptList;
		}
		set
		{
			RequestPIN.SubsequentPromptList = value;
		}
	}

	[Category("Request PIN")]
	[Description("The list of prompts to play subsequent times.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestPinSubsequentPrompts
	{
		get
		{
			return RequestPIN.SubsequentPrompts;
		}
		set
		{
			RequestPIN.SubsequentPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestPinTimeoutPromptList
	{
		get
		{
			return RequestPIN.TimeoutPromptList;
		}
		set
		{
			RequestPIN.TimeoutPromptList = value;
		}
	}

	[Category("Request PIN")]
	[Description("The list of prompts to play when no user input is detected.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestPinTimeoutPrompts
	{
		get
		{
			return RequestPIN.TimeoutPrompts;
		}
		set
		{
			RequestPIN.TimeoutPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestPinInvalidDigitPromptList
	{
		get
		{
			return RequestPIN.InvalidDigitPromptList;
		}
		set
		{
			RequestPIN.InvalidDigitPromptList = value;
		}
	}

	[Category("Request PIN")]
	[Description("The list of prompts to play when the user enters an invalid digit or less digits than MinDigits.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestPinInvalidDigitPrompts
	{
		get
		{
			return RequestPIN.InvalidDigitPrompts;
		}
		set
		{
			RequestPIN.InvalidDigitPrompts = value;
		}
	}

	[Category("Request PIN")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool RequestPinAcceptDtmfInput
	{
		get
		{
			return RequestPIN.AcceptDtmfInput;
		}
		set
		{
			RequestPIN.AcceptDtmfInput = value;
		}
	}

	[Category("Request PIN")]
	[Description("The time to wait for the first digit before playing the specified timeout prompts, in seconds.")]
	public uint RequestPinFirstDigitTimeout
	{
		get
		{
			return RequestPIN.FirstDigitTimeout;
		}
		set
		{
			RequestPIN.FirstDigitTimeout = value;
		}
	}

	[Category("Request PIN")]
	[Description("The time to wait for subsequent digits before playing the specified invalid digits prompts, in seconds.")]
	public uint RequestPinInterDigitTimeout
	{
		get
		{
			return RequestPIN.InterDigitTimeout;
		}
		set
		{
			RequestPIN.InterDigitTimeout = value;
		}
	}

	[Category("Request PIN")]
	[Description("The time to wait for digits after MinDigits has been reached, before returning the entered data, in seconds.")]
	public uint RequestPinFinalDigitTimeout
	{
		get
		{
			return RequestPIN.FinalDigitTimeout;
		}
		set
		{
			RequestPIN.FinalDigitTimeout = value;
		}
	}

	[Category("Request PIN")]
	[Description("Quantity of retry attempts for invalid input or timeout (single counter).")]
	public uint RequestPinMaxRetryCount
	{
		get
		{
			return RequestPIN.MaxRetryCount;
		}
		set
		{
			RequestPIN.MaxRetryCount = value;
		}
	}

	[Category("Request PIN")]
	[Description("The minimum quantity of digits that must be entered by the user.")]
	public uint RequestPinMinDigits
	{
		get
		{
			return RequestPIN.MinDigits;
		}
		set
		{
			RequestPIN.MinDigits = value;
		}
	}

	[Category("Request PIN")]
	[Description("The maximum quantity of digits that can be entered by the user.")]
	public uint RequestPinMaxDigits
	{
		get
		{
			return RequestPIN.MaxDigits;
		}
		set
		{
			RequestPIN.MaxDigits = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits RequestPinStopDigit
	{
		get
		{
			return RequestPIN.StopDigit;
		}
		set
		{
			RequestPIN.StopDigit = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_0
	{
		get
		{
			return RequestPIN.IsValidDigit_0;
		}
		set
		{
			RequestPIN.IsValidDigit_0 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_1
	{
		get
		{
			return RequestPIN.IsValidDigit_1;
		}
		set
		{
			RequestPIN.IsValidDigit_1 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_2
	{
		get
		{
			return RequestPIN.IsValidDigit_2;
		}
		set
		{
			RequestPIN.IsValidDigit_2 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_3
	{
		get
		{
			return RequestPIN.IsValidDigit_3;
		}
		set
		{
			RequestPIN.IsValidDigit_3 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_4
	{
		get
		{
			return RequestPIN.IsValidDigit_4;
		}
		set
		{
			RequestPIN.IsValidDigit_4 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_5
	{
		get
		{
			return RequestPIN.IsValidDigit_5;
		}
		set
		{
			RequestPIN.IsValidDigit_5 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_6
	{
		get
		{
			return RequestPIN.IsValidDigit_6;
		}
		set
		{
			RequestPIN.IsValidDigit_6 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_7
	{
		get
		{
			return RequestPIN.IsValidDigit_7;
		}
		set
		{
			RequestPIN.IsValidDigit_7 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_8
	{
		get
		{
			return RequestPIN.IsValidDigit_8;
		}
		set
		{
			RequestPIN.IsValidDigit_8 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_9
	{
		get
		{
			return RequestPIN.IsValidDigit_9;
		}
		set
		{
			RequestPIN.IsValidDigit_9 = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_Star
	{
		get
		{
			return RequestPIN.IsValidDigit_Star;
		}
		set
		{
			RequestPIN.IsValidDigit_Star = value;
		}
	}

	[Category("Request PIN")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool RequestPinIsValidDigit_Pound
	{
		get
		{
			return RequestPIN.IsValidDigit_Pound;
		}
		set
		{
			RequestPIN.IsValidDigit_Pound = value;
		}
	}

	[Category("Authentication")]
	[Description("True to request ID and PIN. False to request only the ID.")]
	public bool IsPinRequired { get; set; } = true;


	[Category("Authentication")]
	[Description("Number of retry attempts to enter a valid ID and PIN combination.")]
	public uint MaxRetryCount
	{
		get
		{
			return maxRetryCount;
		}
		set
		{
			if (value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 0, 99));
			}
			maxRetryCount = value;
		}
	}

	public AuthenticationComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Validated", VariableScopes.Public, VariableAccessibilities.ReadWrite)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Authentication.ValidatedHelpText")
		};
		Variable item2 = new Variable("ID", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Authentication.IDHelpText")
		};
		Variable item3 = new Variable("PIN", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Authentication.PINHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
		properties.Add(item3);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		base.NotifyComponentRenamed(oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new AuthenticationComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.rn05m5j1kxb2");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "AuthenticationComponent";
	}
}
