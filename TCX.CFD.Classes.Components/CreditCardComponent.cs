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
[Designer(typeof(CreditCardComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(CreditCardComponentToolboxItem))]
[ToolboxBitmap(typeof(CreditCardComponent), "Resources.CreditCard.png")]
[ActivityValidator(typeof(CreditCardComponentValidator))]
public class CreditCardComponent : AbsVadCompositeActivity
{
	private uint maxRetryCount = 3u;

	[Browsable(false)]
	public UserInputComponent RequestNumber { get; set; } = new UserInputComponent();


	[Browsable(false)]
	public string RequestNumberInitialPromptList
	{
		get
		{
			return RequestNumber.InitialPromptList;
		}
		set
		{
			RequestNumber.InitialPromptList = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The list of prompts to play the first time the user input is executed.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestNumberInitialPrompts
	{
		get
		{
			return RequestNumber.InitialPrompts;
		}
		set
		{
			RequestNumber.InitialPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestNumberSubsequentPromptList
	{
		get
		{
			return RequestNumber.SubsequentPromptList;
		}
		set
		{
			RequestNumber.SubsequentPromptList = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The list of prompts to play subsequent times.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestNumberSubsequentPrompts
	{
		get
		{
			return RequestNumber.SubsequentPrompts;
		}
		set
		{
			RequestNumber.SubsequentPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestNumberTimeoutPromptList
	{
		get
		{
			return RequestNumber.TimeoutPromptList;
		}
		set
		{
			RequestNumber.TimeoutPromptList = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The list of prompts to play when no user input is detected.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestNumberTimeoutPrompts
	{
		get
		{
			return RequestNumber.TimeoutPrompts;
		}
		set
		{
			RequestNumber.TimeoutPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestNumberInvalidDigitPromptList
	{
		get
		{
			return RequestNumber.InvalidDigitPromptList;
		}
		set
		{
			RequestNumber.InvalidDigitPromptList = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The list of prompts to play when the user enters an invalid digit or less digits than MinDigits.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestNumberInvalidDigitPrompts
	{
		get
		{
			return RequestNumber.InvalidDigitPrompts;
		}
		set
		{
			RequestNumber.InvalidDigitPrompts = value;
		}
	}

	[Category("Request Card Number")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool RequestNumberAcceptDtmfInput
	{
		get
		{
			return RequestNumber.AcceptDtmfInput;
		}
		set
		{
			RequestNumber.AcceptDtmfInput = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The time to wait for the first digit before playing the specified timeout prompts, in seconds.")]
	public uint RequestNumberFirstDigitTimeout
	{
		get
		{
			return RequestNumber.FirstDigitTimeout;
		}
		set
		{
			RequestNumber.FirstDigitTimeout = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The time to wait for subsequent digits before playing the specified invalid digits prompts, in seconds.")]
	public uint RequestNumberInterDigitTimeout
	{
		get
		{
			return RequestNumber.InterDigitTimeout;
		}
		set
		{
			RequestNumber.InterDigitTimeout = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The time to wait for digits after MinDigits has been reached, before returning the entered data, in seconds.")]
	public uint RequestNumberFinalDigitTimeout
	{
		get
		{
			return RequestNumber.FinalDigitTimeout;
		}
		set
		{
			RequestNumber.FinalDigitTimeout = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Quantity of retry attempts for invalid input or timeout (single counter).")]
	public uint RequestNumberMaxRetryCount
	{
		get
		{
			return RequestNumber.MaxRetryCount;
		}
		set
		{
			RequestNumber.MaxRetryCount = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The minimum quantity of digits that must be entered by the user.")]
	public uint RequestNumberMinDigits
	{
		get
		{
			return RequestNumber.MinDigits;
		}
		set
		{
			RequestNumber.MinDigits = value;
		}
	}

	[Category("Request Card Number")]
	[Description("The maximum quantity of digits that can be entered by the user.")]
	public uint RequestNumberMaxDigits
	{
		get
		{
			return RequestNumber.MaxDigits;
		}
		set
		{
			RequestNumber.MaxDigits = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits RequestNumberStopDigit
	{
		get
		{
			return RequestNumber.StopDigit;
		}
		set
		{
			RequestNumber.StopDigit = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_0
	{
		get
		{
			return RequestNumber.IsValidDigit_0;
		}
		set
		{
			RequestNumber.IsValidDigit_0 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_1
	{
		get
		{
			return RequestNumber.IsValidDigit_1;
		}
		set
		{
			RequestNumber.IsValidDigit_1 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_2
	{
		get
		{
			return RequestNumber.IsValidDigit_2;
		}
		set
		{
			RequestNumber.IsValidDigit_2 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_3
	{
		get
		{
			return RequestNumber.IsValidDigit_3;
		}
		set
		{
			RequestNumber.IsValidDigit_3 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_4
	{
		get
		{
			return RequestNumber.IsValidDigit_4;
		}
		set
		{
			RequestNumber.IsValidDigit_4 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_5
	{
		get
		{
			return RequestNumber.IsValidDigit_5;
		}
		set
		{
			RequestNumber.IsValidDigit_5 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_6
	{
		get
		{
			return RequestNumber.IsValidDigit_6;
		}
		set
		{
			RequestNumber.IsValidDigit_6 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_7
	{
		get
		{
			return RequestNumber.IsValidDigit_7;
		}
		set
		{
			RequestNumber.IsValidDigit_7 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_8
	{
		get
		{
			return RequestNumber.IsValidDigit_8;
		}
		set
		{
			RequestNumber.IsValidDigit_8 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_9
	{
		get
		{
			return RequestNumber.IsValidDigit_9;
		}
		set
		{
			RequestNumber.IsValidDigit_9 = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_Star
	{
		get
		{
			return RequestNumber.IsValidDigit_Star;
		}
		set
		{
			RequestNumber.IsValidDigit_Star = value;
		}
	}

	[Category("Request Card Number")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool RequestNumberIsValidDigit_Pound
	{
		get
		{
			return RequestNumber.IsValidDigit_Pound;
		}
		set
		{
			RequestNumber.IsValidDigit_Pound = value;
		}
	}

	[Browsable(false)]
	public UserInputComponent RequestExpiration { get; set; } = new UserInputComponent();


	[Browsable(false)]
	public string RequestExpirationInitialPromptList
	{
		get
		{
			return RequestExpiration.InitialPromptList;
		}
		set
		{
			RequestExpiration.InitialPromptList = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The list of prompts to play the first time the user input is executed.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestExpirationInitialPrompts
	{
		get
		{
			return RequestExpiration.InitialPrompts;
		}
		set
		{
			RequestExpiration.InitialPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestExpirationSubsequentPromptList
	{
		get
		{
			return RequestExpiration.SubsequentPromptList;
		}
		set
		{
			RequestExpiration.SubsequentPromptList = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The list of prompts to play subsequent times.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestExpirationSubsequentPrompts
	{
		get
		{
			return RequestExpiration.SubsequentPrompts;
		}
		set
		{
			RequestExpiration.SubsequentPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestExpirationTimeoutPromptList
	{
		get
		{
			return RequestExpiration.TimeoutPromptList;
		}
		set
		{
			RequestExpiration.TimeoutPromptList = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The list of prompts to play when no user input is detected.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestExpirationTimeoutPrompts
	{
		get
		{
			return RequestExpiration.TimeoutPrompts;
		}
		set
		{
			RequestExpiration.TimeoutPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestExpirationInvalidDigitPromptList
	{
		get
		{
			return RequestExpiration.InvalidDigitPromptList;
		}
		set
		{
			RequestExpiration.InvalidDigitPromptList = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The list of prompts to play when the user enters an invalid digit or less digits than MinDigits.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestExpirationInvalidDigitPrompts
	{
		get
		{
			return RequestExpiration.InvalidDigitPrompts;
		}
		set
		{
			RequestExpiration.InvalidDigitPrompts = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool RequestExpirationAcceptDtmfInput
	{
		get
		{
			return RequestExpiration.AcceptDtmfInput;
		}
		set
		{
			RequestExpiration.AcceptDtmfInput = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The time to wait for the first digit before playing the specified timeout prompts, in seconds.")]
	public uint RequestExpirationFirstDigitTimeout
	{
		get
		{
			return RequestExpiration.FirstDigitTimeout;
		}
		set
		{
			RequestExpiration.FirstDigitTimeout = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The time to wait for subsequent digits before playing the specified invalid digits prompts, in seconds.")]
	public uint RequestExpirationInterDigitTimeout
	{
		get
		{
			return RequestExpiration.InterDigitTimeout;
		}
		set
		{
			RequestExpiration.InterDigitTimeout = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The time to wait for digits after MinDigits has been reached, before returning the entered data, in seconds.")]
	public uint RequestExpirationFinalDigitTimeout
	{
		get
		{
			return RequestExpiration.FinalDigitTimeout;
		}
		set
		{
			RequestExpiration.FinalDigitTimeout = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Quantity of retry attempts for invalid input or timeout (single counter).")]
	public uint RequestExpirationMaxRetryCount
	{
		get
		{
			return RequestExpiration.MaxRetryCount;
		}
		set
		{
			RequestExpiration.MaxRetryCount = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The minimum quantity of digits that must be entered by the user.")]
	public uint RequestExpirationMinDigits
	{
		get
		{
			return RequestExpiration.MinDigits;
		}
		set
		{
			RequestExpiration.MinDigits = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("The maximum quantity of digits that can be entered by the user.")]
	public uint RequestExpirationMaxDigits
	{
		get
		{
			return RequestExpiration.MaxDigits;
		}
		set
		{
			RequestExpiration.MaxDigits = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits RequestExpirationStopDigit
	{
		get
		{
			return RequestExpiration.StopDigit;
		}
		set
		{
			RequestExpiration.StopDigit = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_0
	{
		get
		{
			return RequestExpiration.IsValidDigit_0;
		}
		set
		{
			RequestExpiration.IsValidDigit_0 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_1
	{
		get
		{
			return RequestExpiration.IsValidDigit_1;
		}
		set
		{
			RequestExpiration.IsValidDigit_1 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_2
	{
		get
		{
			return RequestExpiration.IsValidDigit_2;
		}
		set
		{
			RequestExpiration.IsValidDigit_2 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_3
	{
		get
		{
			return RequestExpiration.IsValidDigit_3;
		}
		set
		{
			RequestExpiration.IsValidDigit_3 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_4
	{
		get
		{
			return RequestExpiration.IsValidDigit_4;
		}
		set
		{
			RequestExpiration.IsValidDigit_4 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_5
	{
		get
		{
			return RequestExpiration.IsValidDigit_5;
		}
		set
		{
			RequestExpiration.IsValidDigit_5 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_6
	{
		get
		{
			return RequestExpiration.IsValidDigit_6;
		}
		set
		{
			RequestExpiration.IsValidDigit_6 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_7
	{
		get
		{
			return RequestExpiration.IsValidDigit_7;
		}
		set
		{
			RequestExpiration.IsValidDigit_7 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_8
	{
		get
		{
			return RequestExpiration.IsValidDigit_8;
		}
		set
		{
			RequestExpiration.IsValidDigit_8 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_9
	{
		get
		{
			return RequestExpiration.IsValidDigit_9;
		}
		set
		{
			RequestExpiration.IsValidDigit_9 = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_Star
	{
		get
		{
			return RequestExpiration.IsValidDigit_Star;
		}
		set
		{
			RequestExpiration.IsValidDigit_Star = value;
		}
	}

	[Category("Request Expiration Date")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool RequestExpirationIsValidDigit_Pound
	{
		get
		{
			return RequestExpiration.IsValidDigit_Pound;
		}
		set
		{
			RequestExpiration.IsValidDigit_Pound = value;
		}
	}

	[Browsable(false)]
	public UserInputComponent RequestSecurityCode { get; set; } = new UserInputComponent();


	[Browsable(false)]
	public string RequestSecurityCodeInitialPromptList
	{
		get
		{
			return RequestSecurityCode.InitialPromptList;
		}
		set
		{
			RequestSecurityCode.InitialPromptList = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The list of prompts to play the first time the user input is executed.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestSecurityCodeInitialPrompts
	{
		get
		{
			return RequestSecurityCode.InitialPrompts;
		}
		set
		{
			RequestSecurityCode.InitialPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestSecurityCodeSubsequentPromptList
	{
		get
		{
			return RequestSecurityCode.SubsequentPromptList;
		}
		set
		{
			RequestSecurityCode.SubsequentPromptList = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The list of prompts to play subsequent times.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestSecurityCodeSubsequentPrompts
	{
		get
		{
			return RequestSecurityCode.SubsequentPrompts;
		}
		set
		{
			RequestSecurityCode.SubsequentPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestSecurityCodeTimeoutPromptList
	{
		get
		{
			return RequestSecurityCode.TimeoutPromptList;
		}
		set
		{
			RequestSecurityCode.TimeoutPromptList = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The list of prompts to play when no user input is detected.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestSecurityCodeTimeoutPrompts
	{
		get
		{
			return RequestSecurityCode.TimeoutPrompts;
		}
		set
		{
			RequestSecurityCode.TimeoutPrompts = value;
		}
	}

	[Browsable(false)]
	public string RequestSecurityCodeInvalidDigitPromptList
	{
		get
		{
			return RequestSecurityCode.InvalidDigitPromptList;
		}
		set
		{
			RequestSecurityCode.InvalidDigitPromptList = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The list of prompts to play when the user enters an invalid digit or less digits than MinDigits.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> RequestSecurityCodeInvalidDigitPrompts
	{
		get
		{
			return RequestSecurityCode.InvalidDigitPrompts;
		}
		set
		{
			RequestSecurityCode.InvalidDigitPrompts = value;
		}
	}

	[Category("Request Security Code")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool RequestSecurityCodeAcceptDtmfInput
	{
		get
		{
			return RequestSecurityCode.AcceptDtmfInput;
		}
		set
		{
			RequestSecurityCode.AcceptDtmfInput = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The time to wait for the first digit before playing the specified timeout prompts, in seconds.")]
	public uint RequestSecurityCodeFirstDigitTimeout
	{
		get
		{
			return RequestSecurityCode.FirstDigitTimeout;
		}
		set
		{
			RequestSecurityCode.FirstDigitTimeout = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The time to wait for subsequent digits before playing the specified invalid digits prompts, in seconds.")]
	public uint RequestSecurityCodeInterDigitTimeout
	{
		get
		{
			return RequestSecurityCode.InterDigitTimeout;
		}
		set
		{
			RequestSecurityCode.InterDigitTimeout = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The time to wait for digits after MinDigits has been reached, before returning the entered data, in seconds.")]
	public uint RequestSecurityCodeFinalDigitTimeout
	{
		get
		{
			return RequestSecurityCode.FinalDigitTimeout;
		}
		set
		{
			RequestSecurityCode.FinalDigitTimeout = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Quantity of retry attempts for invalid input or timeout (single counter).")]
	public uint RequestSecurityCodeMaxRetryCount
	{
		get
		{
			return RequestSecurityCode.MaxRetryCount;
		}
		set
		{
			RequestSecurityCode.MaxRetryCount = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The minimum quantity of digits that must be entered by the user.")]
	public uint RequestSecurityCodeMinDigits
	{
		get
		{
			return RequestSecurityCode.MinDigits;
		}
		set
		{
			RequestSecurityCode.MinDigits = value;
		}
	}

	[Category("Request Security Code")]
	[Description("The maximum quantity of digits that can be entered by the user.")]
	public uint RequestSecurityCodeMaxDigits
	{
		get
		{
			return RequestSecurityCode.MaxDigits;
		}
		set
		{
			RequestSecurityCode.MaxDigits = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits RequestSecurityCodeStopDigit
	{
		get
		{
			return RequestSecurityCode.StopDigit;
		}
		set
		{
			RequestSecurityCode.StopDigit = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_0
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_0;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_0 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_1
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_1;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_1 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_2
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_2;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_2 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_3
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_3;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_3 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_4
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_4;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_4 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_5
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_5;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_5 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_6
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_6;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_6 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_7
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_7;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_7 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_8
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_8;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_8 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_9
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_9;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_9 = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_Star
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_Star;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_Star = value;
		}
	}

	[Category("Request Security Code")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool RequestSecurityCodeIsValidDigit_Pound
	{
		get
		{
			return RequestSecurityCode.IsValidDigit_Pound;
		}
		set
		{
			RequestSecurityCode.IsValidDigit_Pound = value;
		}
	}

	[Category("Credit Card")]
	[Description("True to request the expiration date, otherwise False.")]
	public bool IsExpirationRequired { get; set; } = true;


	[Category("Credit Card")]
	[Description("True to request the security code, otherwise False.")]
	public bool IsSecurityCodeRequired { get; set; } = true;


	[Category("Credit Card")]
	[Description("Number of retry attempts to enter a valid credit card number, expiration date and security code combination (when required).")]
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

	public CreditCardComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Validated", VariableScopes.Public, VariableAccessibilities.ReadWrite)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.CreditCard.ValidatedHelpText")
		};
		Variable item2 = new Variable("Number", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.CreditCard.NumberHelpText")
		};
		Variable item3 = new Variable("Expiration", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.CreditCard.ExpirationHelpText")
		};
		Variable item4 = new Variable("SecurityCode", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.CreditCard.SecurityCodeHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
		properties.Add(item3);
		properties.Add(item4);
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
		return new CreditCardComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.7ktrrs8xo913");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "CreditCardComponent";
	}
}
