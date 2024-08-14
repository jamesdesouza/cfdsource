using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class UserComponentCompiler : AbsComponentCompiler
{
	private readonly UserComponent userComponent;

	public UserComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, UserComponent userComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(userComponent), userComponent.GetRootFlow().FlowType, userComponent)
	{
		this.userComponent = userComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		ComponentFileObject componentFileObject = userComponent.FileObject;
		if (componentFileObject == null)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.Component.UnknownFileObject"), userComponent.Name), fileObject, flowType, userComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("{0} {1} = new {0}(onlineServices, \"{1}\", callflow, myCall, logHeader);", NameHelper.SanitizeName(componentFileObject.Name.Substring(0, componentFileObject.Name.Length - 5)), userComponent.Name).AppendLine().Append("            ");
			foreach (UserComponent.UserProperty publicProperty in userComponent.GetPublicProperties())
			{
				if (publicProperty.Value == null)
				{
					continue;
				}
				string text = publicProperty.Value.ToString().Trim();
				if (!string.IsNullOrEmpty(text))
				{
					AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, text);
					if (!absArgument.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), publicProperty.Name, userComponent.Name, text), fileObject, flowType, userComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.{1}Setter = () => {{ return {2}; }};", userComponent.Name, publicProperty.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, userComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
