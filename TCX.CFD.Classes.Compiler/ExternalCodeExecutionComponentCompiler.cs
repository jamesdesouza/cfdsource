using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ExternalCodeExecutionComponentCompiler : AbsComponentCompiler
{
	private readonly ExternalCodeExecutionComponent externalCodeExecutionComponent;

	private bool ValidateMethodInvocation(string scriptContent, string className, string methodName, List<ScriptParameter> parameterList)
	{
		int num = scriptContent.IndexOf("class");
		if (num == -1)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.ClassNameNotFound"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName, className), fileObject, flowType, externalCodeExecutionComponent);
			return false;
		}
		int i;
		for (i = num + 5; i < scriptContent.Length && char.IsWhiteSpace(scriptContent, i); i++)
		{
		}
		if (scriptContent.Substring(i, className.Length) != className)
		{
			return ValidateMethodInvocation(scriptContent.Substring(i), className, methodName, parameterList);
		}
		i += className.Length;
		int num2 = scriptContent.IndexOf('{', i);
		if (num2 == -1)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.ClassCodeNotFound"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName, className), fileObject, flowType, externalCodeExecutionComponent);
			return false;
		}
		int num3 = scriptContent.IndexOf(methodName, 1 + num2);
		if (num3 == -1)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.MethodNotFound"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName, className, methodName), fileObject, flowType, externalCodeExecutionComponent);
			return false;
		}
		int num4 = scriptContent.IndexOf('(', num3 + methodName.Length);
		if (num4 == -1)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.MethodParametersNotFound"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName, className, methodName), fileObject, flowType, externalCodeExecutionComponent);
			return false;
		}
		int num5 = scriptContent.IndexOf(')', 1 + num4);
		if (num5 == -1)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.MethodParametersNotFound"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName, className, methodName), fileObject, flowType, externalCodeExecutionComponent);
			return false;
		}
		if (scriptContent.Substring(1 + num4, num5 - num4 - 1).Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length != parameterList.Count)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.MethodParametersDontMatch"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName, className, methodName), fileObject, flowType, externalCodeExecutionComponent);
			return false;
		}
		return true;
	}

	public ExternalCodeExecutionComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, ExternalCodeExecutionComponent externalCodeExecutionComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(externalCodeExecutionComponent), externalCodeExecutionComponent.GetRootFlow().FlowType, externalCodeExecutionComponent)
	{
		this.externalCodeExecutionComponent = externalCodeExecutionComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(externalCodeExecutionComponent.LibraryFileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.FilePathIsEmpty"), externalCodeExecutionComponent.Name), fileObject, flowType, externalCodeExecutionComponent);
		}
		else
		{
			FileInfo fileInfo = new FileInfo(Path.Combine(fileObject.GetProjectObject().GetFolderPath(), Path.Combine("Libraries", externalCodeExecutionComponent.LibraryFileName)));
			if (!fileInfo.Exists)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.FilePathNotFound"), externalCodeExecutionComponent.Name, externalCodeExecutionComponent.LibraryFileName), fileObject, flowType, externalCodeExecutionComponent);
			}
			else if (string.IsNullOrEmpty(externalCodeExecutionComponent.MethodName))
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.MethodNameIsEmpty"), externalCodeExecutionComponent.Name), fileObject, flowType, externalCodeExecutionComponent);
			}
			else if (string.IsNullOrEmpty(externalCodeExecutionComponent.ClassName))
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.ClassNameIsEmpty"), externalCodeExecutionComponent.Name), fileObject, flowType, externalCodeExecutionComponent);
			}
			else
			{
				string scriptContent = File.ReadAllText(fileInfo.FullName);
				if (ValidateMethodInvocation(scriptContent, externalCodeExecutionComponent.ClassName, externalCodeExecutionComponent.MethodName, externalCodeExecutionComponent.Parameters))
				{
					string name = externalCodeExecutionComponent.Name;
					string arg = name + CompilerHelper.GetRandomId() + "ECFComponent";
					componentsInitializationScriptSb.AppendFormat("{0} {1} = new {0}(\"{1}\", callflow, myCall, logHeader);", arg, name).AppendLine().Append("            ");
					foreach (ScriptParameter parameter in externalCodeExecutionComponent.Parameters)
					{
						if (!string.IsNullOrEmpty(parameter.Name) || !string.IsNullOrEmpty(parameter.Value))
						{
							if (string.IsNullOrEmpty(parameter.Name))
							{
								CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.ParameterNameIsEmpty"), externalCodeExecutionComponent.Name), fileObject, flowType, externalCodeExecutionComponent);
							}
							if (string.IsNullOrEmpty(parameter.Value))
							{
								CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExternalCodeExecutionComponent.ParameterValueIsEmpty"), externalCodeExecutionComponent.Name), fileObject, flowType, externalCodeExecutionComponent);
							}
							AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, parameter.Value);
							if (!absArgument.IsSafeExpression())
							{
								CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Parameter - " + parameter.Name, externalCodeExecutionComponent.Name, parameter.Value), fileObject, flowType, externalCodeExecutionComponent);
							}
							componentsInitializationScriptSb.AppendFormat("{0}.Parameters.Add(new CallFlow.CFD.Parameter(\"{1}\", () => {{ return {2}; }}));", name, parameter.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
						}
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, name).AppendLine().Append("            ");
					externalCodeLauncherSb.AppendFormat("public class {0} : ExternalCodeExecutionComponent", arg).AppendLine().Append("            ");
					externalCodeLauncherSb.Append("{").AppendLine().Append("            ");
					externalCodeLauncherSb.Append("    public List<CallFlow.CFD.Parameter> Parameters { get; } = new List<CallFlow.CFD.Parameter>();").AppendLine().Append("            ");
					externalCodeLauncherSb.AppendFormat("    public {0}(string name, ICallflow callflow, ICall myCall, string projectName) : base(name, callflow, myCall, projectName) {{}}", arg).AppendLine().Append("            ");
					externalCodeLauncherSb.Append("    protected override object ExecuteCode()").AppendLine().Append("            ");
					externalCodeLauncherSb.Append("    {").AppendLine().Append("            ");
					externalCodeLauncherSb.AppendFormat("        var instance = new {0}();", externalCodeExecutionComponent.ClassName).AppendLine().Append("            ");
					externalCodeLauncherSb.AppendFormat("        {0}instance.{1}(", externalCodeExecutionComponent.ReturnsValue ? "return " : "", externalCodeExecutionComponent.MethodName);
					for (int i = 0; i < externalCodeExecutionComponent.Parameters.Count; i++)
					{
						ScriptParameter scriptParameter = externalCodeExecutionComponent.Parameters[i];
						if (i > 0)
						{
							externalCodeLauncherSb.Append(", ");
						}
						if (scriptParameter.Type == ScriptParameterTypes.Object)
						{
							externalCodeLauncherSb.AppendFormat("Parameters[{0}].Value", i);
						}
						else
						{
							externalCodeLauncherSb.AppendFormat("Convert.To{0}(Parameters[{1}].Value)", scriptParameter.Type, i);
						}
					}
					externalCodeLauncherSb.Append(");").AppendLine().Append("            ");
					if (!externalCodeExecutionComponent.ReturnsValue)
					{
						externalCodeLauncherSb.Append("        return null;").AppendLine().Append("            ");
					}
					externalCodeLauncherSb.Append("    }").AppendLine().Append("            ");
					externalCodeLauncherSb.Append("}").AppendLine().Append("            ");
				}
			}
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
