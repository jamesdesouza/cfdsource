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
[Designer(typeof(ExecuteCSharpCodeComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(ExecuteCSharpCodeComponentToolboxItem))]
[ToolboxBitmap(typeof(ExecuteCSharpCodeComponent), "Resources.ExternalCodeExecution.png")]
[ActivityValidator(typeof(ExecuteCSharpCodeComponentValidator))]
public class ExecuteCSharpCodeComponent : AbsVadActivity
{
	private bool returnsValue = true;

	private List<ScriptParameter> parameters = new List<ScriptParameter>();

	private readonly XmlSerializer scriptParameterSerializer = new XmlSerializer(typeof(List<ScriptParameter>));

	[Category("Execute C# Code")]
	[Description("The C# code to execute. This is the content of the method, not including the method name or parameters.")]
	public string Code { get; set; } = string.Empty;


	[Category("Execute C# Code")]
	[Description("The name of the method to execute.")]
	public string MethodName { get; set; } = string.Empty;


	[Category("Execute C# Code")]
	[Description("True if the method to invoke returns a value. False if it returns void.")]
	public bool ReturnsValue
	{
		get
		{
			return returnsValue;
		}
		set
		{
			returnsValue = value;
			if (value)
			{
				AddReturnValueVariable();
			}
			else
			{
				properties.Clear();
			}
		}
	}

	[Browsable(false)]
	public string ParameterList
	{
		get
		{
			return SerializationHelper.Serialize(scriptParameterSerializer, parameters);
		}
		set
		{
			parameters = SerializationHelper.Deserialize(scriptParameterSerializer, value) as List<ScriptParameter>;
		}
	}

	[Category("Execute C# Code")]
	[Description("The list of parameters to use when invoking the specified method.")]
	[Editor(typeof(ParameterCollectionEditor<ScriptParameter>), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<ScriptParameter> Parameters
	{
		get
		{
			List<ScriptParameter> list = new List<ScriptParameter>();
			foreach (ScriptParameter parameter in parameters)
			{
				list.Add(new ScriptParameter(parameter.Name, parameter.Value, parameter.Type));
			}
			return list;
		}
		set
		{
			parameters = value;
		}
	}

	private void AddReturnValueVariable()
	{
		properties.Clear();
		Variable item = new Variable("ReturnValue", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.ExecuteCSharpCode.ReturnValueHelpText")
		};
		properties.Add(item);
	}

	public ExecuteCSharpCodeComponent()
	{
		InitializeComponent();
		AddReturnValueVariable();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (ScriptParameter parameter in parameters)
		{
			parameter.Value = ExpressionHelper.RenameComponent(this, parameter.Value, oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (ScriptParameter parameter in parameters)
		{
			parameter.Value = ExpressionHelper.MigrateConstantStringExpression(this, parameter.Value);
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new ExecuteCSharpCodeComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.tatp7bos28vo");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ExecuteCSharpCodeComponent";
	}
}
