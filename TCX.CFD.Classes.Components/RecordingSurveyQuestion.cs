using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class RecordingSurveyQuestion : SurveyQuestion
{
	private List<Prompt> offerPlaybackPreRecordingPrompts = new List<Prompt>();

	private List<Prompt> offerPlaybackPostRecordingPrompts = new List<Prompt>();

	public uint MaxRecordingTime { get; set; }

	public bool OfferPlayback { get; set; }

	[Browsable(false)]
	public string OfferPlaybackPreRecordingPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, offerPlaybackPreRecordingPrompts);
		}
		set
		{
			offerPlaybackPreRecordingPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> OfferPlaybackPreRecordingPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt offerPlaybackPreRecordingPrompt in offerPlaybackPreRecordingPrompts)
			{
				list.Add(offerPlaybackPreRecordingPrompt.Clone());
			}
			return list;
		}
		set
		{
			offerPlaybackPreRecordingPrompts = value;
		}
	}

	[Browsable(false)]
	public string OfferPlaybackPostRecordingPromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, offerPlaybackPostRecordingPrompts);
		}
		set
		{
			offerPlaybackPostRecordingPrompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> OfferPlaybackPostRecordingPrompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt offerPlaybackPostRecordingPrompt in offerPlaybackPostRecordingPrompts)
			{
				list.Add(offerPlaybackPostRecordingPrompt.Clone());
			}
			return list;
		}
		set
		{
			offerPlaybackPostRecordingPrompts = value;
		}
	}

	public SurveyAnswers KeepAnswer { get; set; }

	public SurveyAnswers RerecordAnswer { get; set; }

	public RecordingSurveyQuestion()
	{
		OfferPlayback = false;
		MaxRecordingTime = 60u;
		KeepAnswer = SurveyAnswers.Option1;
		RerecordAnswer = SurveyAnswers.Option2;
	}

	public RecordingSurveyQuestion(string tag, List<Prompt> prompts, uint maxRecordingTime, bool offerPlayback, List<Prompt> offerPlaybackPreRecordingPrompts, List<Prompt> offerPlaybackPostRecordingPrompts, SurveyAnswers keepAnswer, SurveyAnswers rerecordAnswer)
		: base(tag, prompts)
	{
		MaxRecordingTime = maxRecordingTime;
		OfferPlayback = offerPlayback;
		OfferPlaybackPreRecordingPrompts = offerPlaybackPreRecordingPrompts;
		OfferPlaybackPostRecordingPrompts = offerPlaybackPostRecordingPrompts;
		KeepAnswer = keepAnswer;
		RerecordAnswer = rerecordAnswer;
	}

	public override void Accept(AbsComponentCompiler compiler, bool isDebugBuild, string componentName, StringBuilder componentsInitializationScriptSb, AudioFileCollector audioFileCollector)
	{
		compiler.Visit(this, isDebugBuild, componentName, componentsInitializationScriptSb, audioFileCollector);
	}

	public override SurveyQuestion Clone()
	{
		return new RecordingSurveyQuestion(base.Tag, base.Prompts, MaxRecordingTime, OfferPlayback, OfferPlaybackPreRecordingPrompts, OfferPlaybackPostRecordingPrompts, KeepAnswer, RerecordAnswer)
		{
			ContainerActivity = base.ContainerActivity
		};
	}

	public override AbsSurveyQuestionEditorRowControl CreateSurveyQuestionEditorRowControl()
	{
		return new RecordingSurveyQuestionEditorRowControl(base.ContainerActivity, this);
	}

	public override List<Prompt> GetAllPrompts()
	{
		List<Prompt> list = new List<Prompt>();
		list.AddRange(prompts);
		list.AddRange(offerPlaybackPreRecordingPrompts);
		list.AddRange(offerPlaybackPostRecordingPrompts);
		return list;
	}

	public override bool IsValid()
	{
		if (base.IsValid())
		{
			if (OfferPlayback)
			{
				if (KeepAnswer != RerecordAnswer && offerPlaybackPreRecordingPrompts.Count > 0)
				{
					return offerPlaybackPostRecordingPrompts.Count > 0;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (Prompt offerPlaybackPreRecordingPrompt in offerPlaybackPreRecordingPrompts)
		{
			offerPlaybackPreRecordingPrompt.ContainerActivity = base.ContainerActivity;
			offerPlaybackPreRecordingPrompt.MigrateConstantStringExpressions();
		}
		foreach (Prompt offerPlaybackPostRecordingPrompt in offerPlaybackPostRecordingPrompts)
		{
			offerPlaybackPostRecordingPrompt.ContainerActivity = base.ContainerActivity;
			offerPlaybackPostRecordingPrompt.MigrateConstantStringExpressions();
		}
		base.MigrateConstantStringExpressions();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (Prompt offerPlaybackPreRecordingPrompt in offerPlaybackPreRecordingPrompts)
		{
			offerPlaybackPreRecordingPrompt.ContainerActivity = base.ContainerActivity;
			offerPlaybackPreRecordingPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		foreach (Prompt offerPlaybackPostRecordingPrompt in offerPlaybackPostRecordingPrompts)
		{
			offerPlaybackPostRecordingPrompt.ContainerActivity = base.ContainerActivity;
			offerPlaybackPostRecordingPrompt.NotifyComponentRenamed(oldValue, newValue);
		}
		base.NotifyComponentRenamed(oldValue, newValue);
	}
}
