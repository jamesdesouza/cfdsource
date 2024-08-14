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

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(PromptPlaybackComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(PromptPlaybackComponentToolboxItem))]
[ToolboxBitmap(typeof(PromptPlaybackComponent), "Resources.PromptPlayback.png")]
[ActivityValidator(typeof(PromptPlaybackComponentValidator))]
public class PromptPlaybackComponent : AbsVadActivity
{
	private List<Prompt> prompts = new List<Prompt>();

	private readonly XmlSerializer promptSerializer = new XmlSerializer(typeof(List<Prompt>));

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

	[Category("Prompt Playback")]
	[Description("True to accept DTMF input during prompt playback. False otherwise.")]
	public bool AcceptDtmfInput { get; set; } = true;


	[Category("Prompt Playback")]
	[Description("The list of prompts to play.")]
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

	public PromptPlaybackComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = this;
			prompt.NotifyComponentRenamed(oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = this;
			prompt.MigrateConstantStringExpressions();
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new PromptPlaybackComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.p7szrmrdl6vg");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "PromptPlaybackComponent";
	}
}
