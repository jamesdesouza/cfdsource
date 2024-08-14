using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class WebServicesInteractionComponentCompiler : AbsComponentCompiler
{
	private readonly WebServicesInteractionComponent webServicesInteractionComponent;

	public WebServicesInteractionComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, WebServicesInteractionComponent webServicesInteractionComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(webServicesInteractionComponent), webServicesInteractionComponent.GetRootFlow().FlowType, webServicesInteractionComponent)
	{
		this.webServicesInteractionComponent = webServicesInteractionComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(webServicesInteractionComponent.URI))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServicesInteractionComponent.UriIsEmpty"), webServicesInteractionComponent.Name), fileObject, flowType, webServicesInteractionComponent);
		}
		else if (string.IsNullOrEmpty(webServicesInteractionComponent.WebServiceName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServicesInteractionComponent.WebServiceNameIsEmpty"), webServicesInteractionComponent.Name), fileObject, flowType, webServicesInteractionComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, webServicesInteractionComponent.URI);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "URI", webServicesInteractionComponent.Name, webServicesInteractionComponent.URI), fileObject, flowType, webServicesInteractionComponent);
			}
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, webServicesInteractionComponent.WebServiceName);
			if (!absArgument2.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "WebServiceName", webServicesInteractionComponent.Name, webServicesInteractionComponent.WebServiceName), fileObject, flowType, webServicesInteractionComponent);
			}
			componentsInitializationScriptSb.AppendFormat("WebServicesInteractionComponent {0} = new WebServicesInteractionComponent(\"{0}\", callflow, myCall, logHeader);", webServicesInteractionComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Timeout = {1};", webServicesInteractionComponent.Name, 1000 * webServicesInteractionComponent.Timeout).AppendLine().Append("            ");
			if (!string.IsNullOrEmpty(webServicesInteractionComponent.ContentType))
			{
				componentsInitializationScriptSb.AppendFormat("{0}.ContentType = \"{1}\";", webServicesInteractionComponent.Name, webServicesInteractionComponent.ContentType).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.UriHandler = () => {{ return Convert.ToString({1}); }};", webServicesInteractionComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.WebServiceNameHandler = () => {{ return Convert.ToString({1}); }};", webServicesInteractionComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			if (!string.IsNullOrEmpty(webServicesInteractionComponent.Content))
			{
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, webServicesInteractionComponent.Content);
				if (!absArgument3.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Content", webServicesInteractionComponent.Name, webServicesInteractionComponent.Content), fileObject, flowType, webServicesInteractionComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ContentHandler = () => {{ return Convert.ToString({1}); }};", webServicesInteractionComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
			}
			foreach (Parameter header in webServicesInteractionComponent.Headers)
			{
				if (!string.IsNullOrEmpty(header.Name) || !string.IsNullOrEmpty(header.Value))
				{
					if (string.IsNullOrEmpty(header.Name))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServicesInteractionComponent.HeaderNameIsEmpty"), webServicesInteractionComponent.Name), fileObject, flowType, webServicesInteractionComponent);
					}
					if (string.IsNullOrEmpty(header.Value))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServicesInteractionComponent.HeaderValueIsEmpty"), webServicesInteractionComponent.Name), fileObject, flowType, webServicesInteractionComponent);
					}
					AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, header.Value);
					if (!absArgument4.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Header - " + header.Name, webServicesInteractionComponent.Name, header.Value), fileObject, flowType, webServicesInteractionComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Headers.Add(new CallFlow.CFD.Parameter(\"{1}\", () => {{ return {2}; }}));", webServicesInteractionComponent.Name, header.Name, absArgument4.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, webServicesInteractionComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
