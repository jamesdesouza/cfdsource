using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ExecuteCSharpCodeComponentCompiler : AbsComponentCompiler
{
	private readonly ExecuteCSharpCodeComponent executeCSharpCodeComponent;

	public ExecuteCSharpCodeComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, ExecuteCSharpCodeComponent executeCSharpCodeComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(executeCSharpCodeComponent), executeCSharpCodeComponent.GetRootFlow().FlowType, executeCSharpCodeComponent)
	{
		this.executeCSharpCodeComponent = executeCSharpCodeComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(executeCSharpCodeComponent.MethodName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExecuteCSharpCodeComponent.MethodNameIsEmpty"), executeCSharpCodeComponent.Name), fileObject, flowType, executeCSharpCodeComponent);
		}
		else if (string.IsNullOrEmpty(executeCSharpCodeComponent.Code))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExecuteCSharpCodeComponent.CodeIsEmpty"), executeCSharpCodeComponent.Name), fileObject, flowType, executeCSharpCodeComponent);
		}
		else
		{
			string name = executeCSharpCodeComponent.Name;
			string arg = name + CompilerHelper.GetRandomId() + "ECCComponent";
			componentsInitializationScriptSb.AppendFormat("{0} {1} = new {0}(\"{1}\", callflow, myCall, logHeader);", arg, name).AppendLine().Append("            ");
			foreach (ScriptParameter parameter in executeCSharpCodeComponent.Parameters)
			{
				if (!string.IsNullOrEmpty(parameter.Name) || !string.IsNullOrEmpty(parameter.Value))
				{
					if (string.IsNullOrEmpty(parameter.Name))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExecuteCSharpCodeComponent.ParameterNameIsEmpty"), executeCSharpCodeComponent.Name), fileObject, flowType, executeCSharpCodeComponent);
					}
					if (string.IsNullOrEmpty(parameter.Value))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.ExecuteCSharpCodeComponent.ParameterValueIsEmpty"), executeCSharpCodeComponent.Name), fileObject, flowType, executeCSharpCodeComponent);
					}
					AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, parameter.Value);
					if (!absArgument.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Parameter - " + parameter.Name, executeCSharpCodeComponent.Name, parameter.Value), fileObject, flowType, executeCSharpCodeComponent);
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
			externalCodeLauncherSb.AppendFormat("        {0}{1}(", executeCSharpCodeComponent.ReturnsValue ? "return " : "", executeCSharpCodeComponent.MethodName);
			for (int i = 0; i < executeCSharpCodeComponent.Parameters.Count; i++)
			{
				ScriptParameter scriptParameter = executeCSharpCodeComponent.Parameters[i];
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
			if (!executeCSharpCodeComponent.ReturnsValue)
			{
				externalCodeLauncherSb.Append("        return null;").AppendLine().Append("            ");
			}
			externalCodeLauncherSb.Append("    }").AppendLine().Append("            ");
			externalCodeLauncherSb.AppendLine().Append("            ");
			externalCodeLauncherSb.AppendFormat("private {0} {1}(", executeCSharpCodeComponent.ReturnsValue ? "object" : "void", executeCSharpCodeComponent.MethodName);
			for (int j = 0; j < executeCSharpCodeComponent.Parameters.Count; j++)
			{
				ScriptParameter scriptParameter2 = executeCSharpCodeComponent.Parameters[j];
				if (j > 0)
				{
					externalCodeLauncherSb.Append(", ");
				}
				externalCodeLauncherSb.AppendFormat("{0} {1}", EnumHelper.ScriptParameterTypeToCSharpString(scriptParameter2.Type), scriptParameter2.Name);
			}
			externalCodeLauncherSb.Append(")").AppendLine().Append("            ");
			externalCodeLauncherSb.Append("    {").AppendLine().Append("            ");
			externalCodeLauncherSb.Append(executeCSharpCodeComponent.Code);
			externalCodeLauncherSb.Append("    }").AppendLine().Append("            ");
			externalCodeLauncherSb.Append("}").AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
