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
[Designer(typeof(JsonXmlParserComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(JsonXmlParserComponentToolboxItem))]
[ToolboxBitmap(typeof(JsonXmlParserComponent), "Resources.JsonXmlParser.png")]
[ActivityValidator(typeof(JsonXmlParserComponentValidator))]
public class JsonXmlParserComponent : AbsVadActivity
{
	private List<ResponseMapping> responseMappings = new List<ResponseMapping>();

	private readonly XmlSerializer responseMappingsSerializer = new XmlSerializer(typeof(List<ResponseMapping>));

	[Category("JSON / XML Parser")]
	[Description("The type of response to analyze.")]
	public TextTypes TextType { get; set; }

	[Category("JSON / XML Parser")]
	[Description("The input to parse. The selected variable or component output should return valid JSON or XML.")]
	[TypeConverter(typeof(JsonXmlInputTypeConverter))]
	public string Input { get; set; } = string.Empty;


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

	[Category("JSON / XML Parser")]
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

	public JsonXmlParserComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Input = ExpressionHelper.RenameComponent(this, Input, oldValue, newValue);
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
		Input = ExpressionHelper.MigrateConstantStringExpression(this, Input);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new JsonXmlParserComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.3p1doiiaxyfq");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "JsonXmlParserComponent";
	}
}
