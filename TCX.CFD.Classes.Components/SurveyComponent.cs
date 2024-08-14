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

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(SurveyComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(SurveyComponentToolboxItem))]
[ToolboxBitmap(typeof(SurveyComponent), "Resources.Survey.png")]
[ActivityValidator(typeof(SurveyComponentValidator))]
public class SurveyComponent : AbsVadActivity
{
	private List<Prompt> introductoryPrompts = new List<Prompt>();

	private List<Prompt> goodbyePrompts = new List<Prompt>();

	private List<Prompt> timeoutPrompts = new List<Prompt>();

	private List<Prompt> invalidDigitPrompts = new List<Prompt>();

	private List<Parameter> outputFields = new List<Parameter>();

	private List<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>();

	private uint timeout = 5u;

	private uint maxRetryCount = 3u;

	private readonly XmlSerializer promptSerializer = new XmlSerializer(typeof(List<Prompt>));

	private readonly XmlSerializer outputFieldSerializer = new XmlSerializer(typeof(List<Parameter>));

	private readonly XmlSerializer questionSerializer = new XmlSerializer(typeof(List<SurveyQuestion>));

	[Browsable(false)]
	public string IntroductoryPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, introductoryPrompts);
		}
		set
		{
			introductoryPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Survey")]
	[Description("The list of prompts to play when the survey starts.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> IntroductoryPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt introductoryPrompt in introductoryPrompts)
			{
				list.Add(introductoryPrompt.Clone());
			}
			return list;
		}
		set
		{
			introductoryPrompts = value;
		}
	}

	[Browsable(false)]
	public string GoodbyePromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, goodbyePrompts);
		}
		set
		{
			goodbyePrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Survey")]
	[Description("The list of prompts to play when the survey ends.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> GoodbyePrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt goodbyePrompt in goodbyePrompts)
			{
				list.Add(goodbyePrompt.Clone());
			}
			return list;
		}
		set
		{
			goodbyePrompts = value;
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

	[Category("Survey")]
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

	[Category("Survey")]
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

	[Browsable(false)]
	public string OutputFieldList
	{
		get
		{
			return SerializationHelper.Serialize(outputFieldSerializer, outputFields);
		}
		set
		{
			outputFields = SerializationHelper.Deserialize(outputFieldSerializer, value) as List<Parameter>;
		}
	}

	[Category("Survey")]
	[Description("The list of output fields to include as part of the survey, collected previously during the callflow.")]
	[Editor(typeof(ParameterCollectionEditor<Parameter>), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Parameter> OutputFields
	{
		get
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter outputField in outputFields)
			{
				list.Add(new Parameter(outputField.Name, outputField.Value));
			}
			return list;
		}
		set
		{
			outputFields = value;
		}
	}

	[Browsable(false)]
	public string SurveyQuestionList
	{
		get
		{
			return SerializationHelper.Serialize(questionSerializer, surveyQuestions);
		}
		set
		{
			surveyQuestions = SerializationHelper.Deserialize(questionSerializer, value) as List<SurveyQuestion>;
		}
	}

	[Category("Survey")]
	[Description("The questions to ask to the caller.")]
	[Editor(typeof(SurveyQuestionCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<SurveyQuestion> SurveyQuestions
	{
		get
		{
			List<SurveyQuestion> list = new List<SurveyQuestion>();
			foreach (SurveyQuestion surveyQuestion in surveyQuestions)
			{
				list.Add(surveyQuestion.Clone());
			}
			return list;
		}
		set
		{
			surveyQuestions = value;
		}
	}

	[Category("Survey")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool AcceptDtmfInput { get; set; } = true;


	[Category("Survey")]
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

	[Category("Survey")]
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

	[Category("Survey")]
	[Description("When the value is set to true, the component will save the results to the CSV file even when the user hangs up before answering all the questions. As long as there is 1 question answered, results will be saved.")]
	public bool AllowPartialAnswers { get; set; }

	[Category("Survey")]
	[Description("The path where the recording must be saved. Only required when there are recording questions.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string RecordingsPath { get; set; } = string.Empty;


	[Category("Survey")]
	[Description("The path to the CSV file where the result of the survey will be exported.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string ExportToCSVFile { get; set; } = string.Empty;


	public SurveyComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Survey.ResultHelpText")
		};
		Variable item2 = new Variable("ResultHeaders", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Survey.ResultHeadersHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (Prompt introductoryPrompt in introductoryPrompts)
		{
			introductoryPrompt.ContainerActivity = this;
			introductoryPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		foreach (Prompt goodbyePrompt in goodbyePrompts)
		{
			goodbyePrompt.ContainerActivity = this;
			goodbyePrompt.NotifyComponentRenamed(oldValue, newValue);
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
		foreach (Parameter outputField in outputFields)
		{
			outputField.Value = ExpressionHelper.RenameComponent(this, outputField.Value, oldValue, newValue);
		}
		foreach (SurveyQuestion surveyQuestion in surveyQuestions)
		{
			surveyQuestion.ContainerActivity = this;
			surveyQuestion.NotifyComponentRenamed(oldValue, newValue);
		}
		RecordingsPath = ExpressionHelper.RenameComponent(this, RecordingsPath, oldValue, newValue);
		ExportToCSVFile = ExpressionHelper.RenameComponent(this, ExportToCSVFile, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (Prompt introductoryPrompt in introductoryPrompts)
		{
			introductoryPrompt.ContainerActivity = this;
			introductoryPrompt.MigrateConstantStringExpressions();
		}
		foreach (Prompt goodbyePrompt in goodbyePrompts)
		{
			goodbyePrompt.ContainerActivity = this;
			goodbyePrompt.MigrateConstantStringExpressions();
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
		foreach (Parameter outputField in outputFields)
		{
			outputField.Value = ExpressionHelper.MigrateConstantStringExpression(this, outputField.Value);
		}
		foreach (SurveyQuestion surveyQuestion in surveyQuestions)
		{
			surveyQuestion.ContainerActivity = this;
			surveyQuestion.MigrateConstantStringExpressions();
		}
		RecordingsPath = ExpressionHelper.MigrateConstantStringExpression(this, RecordingsPath);
		ExportToCSVFile = ExpressionHelper.MigrateConstantStringExpression(this, ExportToCSVFile);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new SurveyComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.h1v1u1welto");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "SurveyComponent";
	}
}
