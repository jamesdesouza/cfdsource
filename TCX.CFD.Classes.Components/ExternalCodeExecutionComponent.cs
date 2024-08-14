using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(ExternalCodeExecutionComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(ExternalCodeExecutionComponentToolboxItem))]
[ToolboxBitmap(typeof(ExternalCodeExecutionComponent), "Resources.ExternalCodeExecution.png")]
[ActivityValidator(typeof(ExternalCodeExecutionComponentValidator))]
public class ExternalCodeExecutionComponent : AbsVadActivity
{
	private string libraryFileName = string.Empty;

	private bool returnsValue = true;

	private List<ScriptParameter> parameters = new List<ScriptParameter>();

	private readonly XmlSerializer parameterSerializer = new XmlSerializer(typeof(List<Parameter>));

	private readonly XmlSerializer scriptParameterSerializer = new XmlSerializer(typeof(List<ScriptParameter>));

	[Category("Execute C# File")]
	[Description("The C# file to execute.")]
	[TypeConverter(typeof(LibraryFileTypeConverter))]
	public string LibraryFileName
	{
		get
		{
			return libraryFileName;
		}
		set
		{
			if (value == LocalizedResourceMgr.GetString("FileTypeConverters.Browse.Text"))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog
				{
					DefaultExt = "cs",
					Filter = "CS Files (*.cs)|*.cs",
					RestoreDirectory = true,
					SupportMultiDottedExtensions = true,
					Title = "Open CS File",
					FileName = string.Empty
				};
				if (openFileDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				try
				{
					FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
					string text = Path.Combine(GetRootFlow().FileObject.GetProjectObject().GetFolderPath(), "Libraries", fileInfo.Name);
					if (Path.GetFullPath(text).ToLower() != Path.GetFullPath(openFileDialog.FileName).ToLower())
					{
						File.Copy(openFileDialog.FileName, text, overwrite: true);
					}
					libraryFileName = fileInfo.Name;
					return;
				}
				catch (Exception exc)
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Error.CopyingFile"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			libraryFileName = value;
		}
	}

	[Category("Execute C# File")]
	[Description("The class name of the object to create, not including any namespace declaration.")]
	public string ClassName { get; set; } = string.Empty;


	[Category("Execute C# File")]
	[Description("The name of the method to execute. An object of type ClassName will be created and this method will be executed on that object.")]
	public string MethodName { get; set; } = string.Empty;


	[Category("Execute C# File")]
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
			try
			{
				parameters = SerializationHelper.Deserialize(scriptParameterSerializer, value) as List<ScriptParameter>;
			}
			catch (InvalidOperationException)
			{
				if (!(SerializationHelper.Deserialize(parameterSerializer, value) is List<Parameter> list))
				{
					return;
				}
				parameters = new List<ScriptParameter>();
				foreach (Parameter item in list)
				{
					parameters.Add(new ScriptParameter(item.Name, item.Value, ScriptParameterTypes.String));
				}
			}
		}
	}

	[Category("Execute C# File")]
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
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.ExternalCodeExecution.ReturnValueHelpText")
		};
		properties.Add(item);
	}

	public ExternalCodeExecutionComponent()
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
		return new ExternalCodeExecutionComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.ut2dfoojpyq6");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "ExternalCodeExecutionComponent";
	}
}
