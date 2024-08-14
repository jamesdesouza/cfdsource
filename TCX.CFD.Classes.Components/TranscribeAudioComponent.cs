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
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(TranscribeAudioComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(TranscribeAudioComponentToolboxItem))]
[ToolboxBitmap(typeof(TranscribeAudioComponent), "Resources.TranscribeAudio.png")]
[ActivityValidator(typeof(TranscribeAudioComponentValidator))]
public class TranscribeAudioComponent : AbsVadCompositeActivity
{
	private string languageCode = "en-US";

	[Category("Transcribe Audio")]
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
				throw new ArgumentException(LocalizedResourceMgr.GetString("ComponentValidators.TranscribeAudio.LanguageCodeRequired"));
			}
			languageCode = value;
		}
	}

	[Category("Transcribe Audio")]
	[Description("The name of the file from which the audio will be transcribed.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string FileName { get; set; } = string.Empty;


	[Category("Transcribe Audio")]
	[Description("The list of hints to provide to the speech recognition engine, in order to improve accuracy")]
	[Editor(typeof(VoiceInputHintsEditor), typeof(UITypeEditor))]
	public List<string> Hints { get; set; } = new List<string>();


	public TranscribeAudioComponent()
	{
		InitializeComponent();
		properties.Add(new Variable("RecognizedText", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.TranscribeAudio.RecognizedTextHelpText")
		});
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		FileName = ExpressionHelper.RenameComponent(this, FileName, oldValue, newValue);
		base.NotifyComponentRenamed(oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		FileName = ExpressionHelper.MigrateConstantStringExpression(this, FileName);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new TranscribeAudioComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.y7zk96pg5gv8");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "TranscribeAudioComponent";
	}
}
