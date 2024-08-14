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
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(VoiceInputComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(VoiceInputComponentToolboxItem))]
[ToolboxBitmap(typeof(VoiceInputComponent), "Resources.VoiceInput.png")]
[ActivityValidator(typeof(VoiceInputComponentValidator))]
public class VoiceInputComponent : AbsVadCompositeActivity
{
	private List<Prompt> initialPrompts = new List<Prompt>();

	private List<Prompt> subsequentPrompts = new List<Prompt>();

	private List<Prompt> timeoutPrompts = new List<Prompt>();

	private List<Prompt> invalidInputPrompts = new List<Prompt>();

	private uint inputTimeout = 3u;

	private uint maxRetryCount = 3u;

	private string languageCode = "en-US";

	private uint dtmfTimeout = 3u;

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
	[Description("The list of prompts to play the first time the voice input is executed.")]
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
	[Description("The list of prompts to play when no voice input is detected once the timeout expires.")]
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
	public string InvalidInputPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, invalidInputPrompts);
		}
		set
		{
			invalidInputPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Prompts")]
	[Description("The list of prompts to play when the voice input can't be matched to an entry from the valid responses list.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> InvalidInputPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt invalidInputPrompt in invalidInputPrompts)
			{
				list.Add(invalidInputPrompt.Clone());
			}
			return list;
		}
		set
		{
			invalidInputPrompts = value;
		}
	}

	[Category("Voice Input")]
	[Description("The time to wait for voice input before playing the specified timeout prompts, in seconds.")]
	public uint InputTimeout
	{
		get
		{
			return inputTimeout;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			inputTimeout = value;
		}
	}

	[Category("Voice Input")]
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

	[Category("Voice Input")]
	[Description("The language code to recognize")]
	[TypeConverter(typeof(SpeechRecognitionLanguageTypesTypeConverter))]
	public string LanguageCode
	{
		get
		{
			return languageCode;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException(LocalizedResourceMgr.GetString("ComponentValidators.VoiceInput.LanguageCodeRequired"));
			}
			languageCode = value;
		}
	}

	[Category("Voice Input")]
	[Description("If true, the recognized audio will be saved to the file specified by FileName.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string SaveToFile { get; set; } = "false";


	[Category("Voice Input")]
	[Description("The name of the file where the recognized audio must be saved. Only valid when SaveToFile is true.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string FileName { get; set; } = string.Empty;


	[Category("DTMF Input")]
	[Description("True to accept DTMF input in addition to voice input. False otherwise.")]
	public bool AcceptDtmfInput { get; set; }

	[Category("DTMF Input")]
	[Description("The time to wait for DTMF digits once the first digit has been received, before playing the specified timeout prompts, in seconds.")]
	public uint DtmfTimeout
	{
		get
		{
			return dtmfTimeout;
		}
		set
		{
			if (value == 0 || value > 99)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99));
			}
			dtmfTimeout = value;
		}
	}

	[Category("DTMF Input")]
	[Description("The minimum quantity of digits that must be entered by the user, when DTMF input is allowed.")]
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

	[Category("DTMF Input")]
	[Description("The maximum quantity of digits that can be entered by the user, when DTMF input is allowed.")]
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

	[Category("DTMF Input")]
	[Description("Specify the digit that the user must press in order to finalize the data entry.")]
	public DtmfDigits StopDigit { get; set; } = DtmfDigits.None;


	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 0. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_0 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 1. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_1 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 2. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_2 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 3. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_3 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 4. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_4 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 5. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_5 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 6. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_6 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 7. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_7 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 8. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_8 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses 9. It could be a valid or an invalid digit.")]
	public bool IsValidDigit_9 { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses the star key (*). It could be a valid or an invalid digit.")]
	public bool IsValidDigit_Star { get; set; }

	[Category("DTMF Input")]
	[Description("Specify what happens when the user presses the pound key (#). It could be a valid or an invalid digit.")]
	public bool IsValidDigit_Pound { get; set; }

	[Category("Voice Input")]
	[Description("The list of valid words to recognize")]
	[Editor(typeof(VoiceInputDictionaryEditor), typeof(UITypeEditor))]
	public List<string> Dictionary { get; set; } = new List<string>();


	[Category("Voice Input")]
	[Description("The list of hints to provide to the speech recognition engine, in order to improve accuracy")]
	[Editor(typeof(VoiceInputHintsEditor), typeof(UITypeEditor))]
	public List<string> Hints { get; set; } = new List<string>();


	public VoiceInputComponent()
	{
		InitializeComponent();
		properties.Add(new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.VoiceInput.ResultHelpText")
		});
		properties.Add(new Variable("RecognizedText", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.VoiceInput.RecognizedTextHelpText")
		});
		properties.Add(new Variable("DictionaryMatch", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.VoiceInput.DictionaryMatchHelpText")
		});
		properties.Add(new Variable("DtmfInput", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.VoiceInput.DtmfInputHelpText")
		});
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		SaveToFile = ExpressionHelper.RenameComponent(this, SaveToFile, oldValue, newValue);
		FileName = ExpressionHelper.RenameComponent(this, FileName, oldValue, newValue);
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
		foreach (Prompt invalidInputPrompt in invalidInputPrompts)
		{
			invalidInputPrompt.ContainerActivity = this;
			invalidInputPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		for (int i = 0; i < Dictionary.Count; i++)
		{
			Dictionary[i] = ExpressionHelper.RenameComponent(this, Dictionary[i], oldValue, newValue);
		}
		base.NotifyComponentRenamed(oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		SaveToFile = ExpressionHelper.MigrateConstantStringExpression(this, SaveToFile);
		FileName = ExpressionHelper.MigrateConstantStringExpression(this, FileName);
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
		foreach (Prompt invalidInputPrompt in invalidInputPrompts)
		{
			invalidInputPrompt.ContainerActivity = this;
			invalidInputPrompt.MigrateConstantStringExpressions();
		}
		for (int i = 0; i < Dictionary.Count; i++)
		{
			Dictionary[i] = ExpressionHelper.MigrateConstantStringExpression(this, Dictionary[i]);
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new VoiceInputComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.lgzfai7rclby");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "VoiceInputComponent";
	}
}
