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
[Designer(typeof(CRMLookupComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(CRMLookupComponentToolboxItem))]
[ToolboxBitmap(typeof(CRMLookupComponent), "Resources.CRMLookup.png")]
[ActivityValidator(typeof(CRMLookupComponentValidator))]
public class CRMLookupComponent : AbsVadActivity
{
	private List<ResponseMapping> responseMappings = new List<ResponseMapping>();

	private readonly XmlSerializer responseMappingsSerializer = new XmlSerializer(typeof(List<ResponseMapping>));

	[Category("CRM Lookup")]
	[Description("The entity on which the lookup will be executed.")]
	public CRMEntities Entity { get; set; }

	[Category("CRM Lookup")]
	[Description("The lookup process will be executed based on the selected option.")]
	public CRMLookupBy LookupBy { get; set; }

	[Category("CRM Lookup")]
	[Description("The input parameter to use for CRM lookup. When LookupBy is LookupNumber, this should be the number to search. When LookupBy is LookupID, this should be the entity ID to search. When LookupBy is LookupFreeQuery, this should be the custom query to execute on the CRM.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string LookupInputParameter { get; set; } = string.Empty;


	[Browsable(false)]
	public string ResponseMappingsList
	{
		get
		{
			return SerializationHelper.Serialize(responseMappingsSerializer, responseMappings);
		}
		set
		{
			responseMappings = SerializationHelper.Deserialize(responseMappingsSerializer, value) as List<ResponseMapping>;
		}
	}

	[Category("CRM Lookup")]
	[Description("How the response should be mapped to variables.")]
	[Editor(typeof(ResponseMappingCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<ResponseMapping> ResponseMappings
	{
		get
		{
			List<ResponseMapping> list = new List<ResponseMapping>();
			foreach (ResponseMapping responseMapping in responseMappings)
			{
				list.Add(new ResponseMapping(responseMapping.Path, responseMapping.Variable));
			}
			return list;
		}
		set
		{
			responseMappings = value;
		}
	}

	public CRMLookupComponent()
	{
		InitializeComponent();
		Variable item = new Variable("Result", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.CRMLookup.ResultHelpText")
		};
		properties.Add(item);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		LookupInputParameter = ExpressionHelper.RenameComponent(this, LookupInputParameter, oldValue, newValue);
		foreach (ResponseMapping responseMapping in responseMappings)
		{
			if (responseMapping.Variable.StartsWith(oldValue + "."))
			{
				responseMapping.Variable = newValue + responseMapping.Variable.Substring(oldValue.Length);
			}
			else if (responseMapping.Variable == oldValue)
			{
				responseMapping.Variable = newValue;
			}
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		LookupInputParameter = ExpressionHelper.MigrateConstantStringExpression(this, LookupInputParameter);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new CRMLookupComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.c3zw6gg1c506");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "CRMLookupComponent";
	}
}
