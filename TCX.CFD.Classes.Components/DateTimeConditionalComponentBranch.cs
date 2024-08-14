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

[DesignerSerializer(typeof(CompositeActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(DateTimeConditionalComponentBranchDesigner), typeof(IDesigner))]
[ToolboxBitmap(typeof(DateTimeConditionalComponentBranch), "Resources.Branch.png")]
[ActivityValidator(typeof(DateTimeConditionalComponentBranchValidator))]
public class DateTimeConditionalComponentBranch : AbsVadSequenceActivity
{
	private DIDFilters didFilter;

	private string didFilterList = string.Empty;

	private List<DateTimeCondition> dateTimeConditions = new List<DateTimeCondition>();

	private readonly XmlSerializer dateTimeConditionSerializer = new XmlSerializer(typeof(List<DateTimeCondition>));

	[Browsable(false)]
	public string DateTimeConditionList
	{
		get
		{
			return SerializationHelper.Serialize(dateTimeConditionSerializer, dateTimeConditions);
		}
		set
		{
			dateTimeConditions = SerializationHelper.Deserialize(dateTimeConditionSerializer, value) as List<DateTimeCondition>;
		}
	}

	[Category("Date Time condition Branch")]
	[Description("Select AllDIDs to enable this branch for every DID. Select SpecificDIDs to enable this branch only for the specific DIDs listed in property DIDFilterList. Or select AllDIDsWithExceptions to enable this branch for every DIDs except the ones listed in property DIDFilterList.")]
	public DIDFilters DIDFilter
	{
		get
		{
			return didFilter;
		}
		set
		{
			didFilter = value;
		}
	}

	[Category("Date Time condition Branch")]
	[Description("The list of DIDs to accept or reject for this branch, depending on the value selected for DIDFilters. In order to specify more than one DID, use a comma to separate them.")]
	public string DIDFilterList
	{
		get
		{
			return didFilterList;
		}
		set
		{
			didFilterList = value;
		}
	}

	[Category("Date Time condition Branch")]
	[Description("The list of date time conditions. The branch will be executed if just one condition is met.")]
	[Editor(typeof(DateTimeConditionCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<DateTimeCondition> DateTimeConditions
	{
		get
		{
			List<DateTimeCondition> list = new List<DateTimeCondition>();
			foreach (DateTimeCondition dateTimeCondition in dateTimeConditions)
			{
				list.Add(dateTimeCondition.Clone());
			}
			return list;
		}
		set
		{
			dateTimeConditions = value;
		}
	}

	public DateTimeConditionalComponentBranch()
	{
		InitializeComponent();
		base.Description = LocalizedResourceMgr.GetString("ComponentDesigners.DateTimeConditionalComponentBranch.Tooltip");
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		base.NotifyComponentRenamed(oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new SequenceBranchCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.a8oq7aqinjc3");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "DateTimeConditionalComponentBranch";
	}
}
