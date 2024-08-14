using System;
using System.Text;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
[XmlInclude(typeof(AudioFilePrompt))]
[XmlInclude(typeof(RecordedAudioPrompt))]
[XmlInclude(typeof(DynamicAudioFilePrompt))]
[XmlInclude(typeof(NumberPrompt))]
[XmlInclude(typeof(TextToSpeechAudioPrompt))]
public abstract class Prompt
{
	protected IVadActivity containerActivity;

	[XmlIgnore]
	public IVadActivity ContainerActivity
	{
		get
		{
			return containerActivity;
		}
		set
		{
			containerActivity = value;
		}
	}

	public abstract void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, string promptCollectionName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector);

	public abstract void MigrateConstantStringExpressions();

	public abstract Prompt Clone();

	public abstract AbsPromptEditorRowControl CreatePromptEditorRowControl();

	public abstract void NotifyComponentRenamed(string oldValue, string newValue);
}
