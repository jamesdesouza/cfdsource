using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
[XmlInclude(typeof(YesNoSurveyQuestion))]
[XmlInclude(typeof(RangeSurveyQuestion))]
[XmlInclude(typeof(RecordingSurveyQuestion))]
public abstract class SurveyQuestion
{
	protected List<Prompt> prompts;

	[NonSerialized]
	protected readonly XmlSerializer promptSerializer = new XmlSerializer(typeof(List<Prompt>));

	[XmlIgnore]
	public IVadActivity ContainerActivity { get; set; }

	public string Tag { get; set; }

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

	protected SurveyQuestion()
	{
		Tag = string.Empty;
		prompts = new List<Prompt>();
	}

	protected SurveyQuestion(string tag, List<Prompt> prompts)
	{
		Tag = tag;
		this.prompts = prompts;
	}

	public abstract void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector);

	public abstract SurveyQuestion Clone();

	public abstract AbsSurveyQuestionEditorRowControl CreateSurveyQuestionEditorRowControl();

	public abstract List<Prompt> GetAllPrompts();

	public virtual bool IsValid()
	{
		if (!string.IsNullOrEmpty(Tag))
		{
			return prompts.Count > 0;
		}
		return false;
	}

	public virtual void MigrateConstantStringExpressions()
	{
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = ContainerActivity;
			prompt.MigrateConstantStringExpressions();
		}
	}

	public virtual void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = ContainerActivity;
			prompt.NotifyComponentRenamed(oldValue, newValue);
		}
	}
}
