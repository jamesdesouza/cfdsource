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
[Designer(typeof(RecordComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(RecordComponentToolboxItem))]
[ToolboxBitmap(typeof(RecordComponent), "Resources.Record.png")]
[ActivityValidator(typeof(RecordComponentValidator))]
public class RecordComponent : AbsVadCompositeActivity
{
	private uint maxTime = 60u;

	private List<Prompt> prompts = new List<Prompt>();

	private readonly XmlSerializer promptSerializer = new XmlSerializer(typeof(List<Prompt>));

	[Category("Record")]
	[Description("If true, a beep will be played just before recording starts.")]
	public bool Beep { get; set; } = true;


	[Category("Record")]
	[Description("The maximum duration to record, in seconds.")]
	public uint MaxTime
	{
		get
		{
			return maxTime;
		}
		set
		{
			if (value < 1 || value > 99999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99999));
			}
			maxTime = value;
		}
	}

	[Category("Record")]
	[Description("If true, any DTMF keypress will stop the recording.")]
	public bool TerminateByDtmf { get; set; }

	[Category("Record")]
	[Description("If true, the recorded audio will be saved to the file specified by FileName.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string SaveToFile { get; set; } = "true";


	[Category("Record")]
	[Description("The name of the file where the recording must be saved. Only valid when SaveToFile is true.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string FileName { get; set; } = string.Empty;


	[Browsable(false)]
	public string PromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, prompts);
		}
		set
		{
			prompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Record")]
	[Description("The list of prompts to play before recording.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> Prompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt prompt in prompts)
			{
				list.Add(prompt.Clone());
			}
			return list;
		}
		set
		{
			prompts = value;
		}
	}

	public RecordComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Record.ResultHelpText")
		};
		Variable item2 = new Variable("Duration", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Record.DurationHelpText")
		};
		Variable item3 = new Variable("StopDigit", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Record.StopDigitHelpText")
		};
		Variable variable = new Variable("AudioId", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.Record.AudioIdHelpText")
		};
		variable.DebuggerVisible = false;
		properties.Add(item);
		properties.Add(item2);
		properties.Add(item3);
		properties.Add(variable);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		SaveToFile = ExpressionHelper.RenameComponent(this, SaveToFile, oldValue, newValue);
		FileName = ExpressionHelper.RenameComponent(this, FileName, oldValue, newValue);
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = this;
			prompt.NotifyComponentRenamed(oldValue, newValue);
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
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = this;
			prompt.MigrateConstantStringExpressions();
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new RecordComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.fzb4ldahvlii");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "RecordComponent";
	}
}
