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
[Designer(typeof(WebInteractionComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(WebInteractionComponentToolboxItem))]
[ToolboxBitmap(typeof(WebInteractionComponent), "Resources.WebInteraction.png")]
[ActivityValidator(typeof(WebInteractionComponentValidator))]
public class WebInteractionComponent : AbsVadActivity
{
	private List<Parameter> headers = new List<Parameter>();

	private uint timeout;

	private readonly XmlSerializer headersSerializer = new XmlSerializer(typeof(List<Parameter>));

	[Category("HTTP Requests")]
	[Description("The URI where the request must be sent.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string URI { get; set; } = string.Empty;


	[Category("HTTP Requests")]
	[Description("The type of the HTTP request.")]
	public HttpRequestTypes HttpRequestType { get; set; }

	[Category("HTTP Requests")]
	[Description("The Content-Type for the request to be sent.")]
	[TypeConverter(typeof(ContentTypesTypeConverter))]
	public string ContentType { get; set; } = "application/x-www-form-urlencoded";


	[Category("HTTP Requests")]
	[Description("The Content to be sent in this HTTP request.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Content { get; set; } = string.Empty;


	[Browsable(false)]
	public string HeaderList
	{
		get
		{
			return SerializationHelper.Serialize(headersSerializer, headers);
		}
		set
		{
			headers = SerializationHelper.Deserialize(headersSerializer, value) as List<Parameter>;
		}
	}

	[Category("HTTP Requests")]
	[Description("The list of headers to send with the HTTP request.")]
	[Editor(typeof(ParameterCollectionEditor<Parameter>), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Parameter> Headers
	{
		get
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter header in headers)
			{
				list.Add(new Parameter(header.Name, header.Value));
			}
			return list;
		}
		set
		{
			headers = value;
		}
	}

	[Category("HTTP Requests")]
	[Description("The time to wait while trying to connect to the Web Server before returning the Server Timeout condition, in seconds. Zero means to wait forever.")]
	public uint Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			if (value > 999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 0, 999));
			}
			timeout = value;
		}
	}

	public WebInteractionComponent()
	{
		InitializeComponent();
		Variable item = new Variable("ResponseStatusCode", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.WebInteraction.ResponseStatusCodeHelpText")
		};
		Variable item2 = new Variable("ResponseContent", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.WebInteraction.ResponseContentHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		URI = ExpressionHelper.RenameComponent(this, URI, oldValue, newValue);
		Content = ExpressionHelper.RenameComponent(this, Content, oldValue, newValue);
		foreach (Parameter header in headers)
		{
			header.Value = ExpressionHelper.RenameComponent(this, header.Value, oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		URI = ExpressionHelper.MigrateConstantStringExpression(this, URI);
		Content = ExpressionHelper.MigrateConstantStringExpression(this, Content);
		foreach (Parameter header in headers)
		{
			header.Value = ExpressionHelper.MigrateConstantStringExpression(this, header.Value);
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new WebInteractionComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.fobpzzo3v2tx");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "WebInteractionComponent";
	}
}
