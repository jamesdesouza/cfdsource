using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class WebInteractionComponentCompiler : AbsComponentCompiler
{
	private readonly WebInteractionComponent webInteractionComponent;

	private string GetHttpMethodText(HttpRequestTypes httpRequestType)
	{
		return httpRequestType switch
		{
			HttpRequestTypes.GET => "System.Net.Http.HttpMethod.Get", 
			HttpRequestTypes.HEAD => "System.Net.Http.HttpMethod.Head", 
			HttpRequestTypes.POST => "System.Net.Http.HttpMethod.Post", 
			HttpRequestTypes.PUT => "System.Net.Http.HttpMethod.Put", 
			HttpRequestTypes.DELETE => "System.Net.Http.HttpMethod.Delete", 
			HttpRequestTypes.TRACE => "System.Net.Http.HttpMethod.Trace", 
			HttpRequestTypes.OPTIONS => "System.Net.Http.HttpMethod.Options", 
			_ => "System.Net.Http.HttpMethod.Get", 
		};
	}

	public WebInteractionComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, WebInteractionComponent webInteractionComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(webInteractionComponent), webInteractionComponent.GetRootFlow().FlowType, webInteractionComponent)
	{
		this.webInteractionComponent = webInteractionComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(webInteractionComponent.URI))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebInteractionComponent.UriIsEmpty"), webInteractionComponent.Name), fileObject, flowType, webInteractionComponent);
		}
		else if ((string.IsNullOrEmpty(webInteractionComponent.Content) && !string.IsNullOrEmpty(webInteractionComponent.ContentType)) || (string.IsNullOrEmpty(webInteractionComponent.ContentType) && !string.IsNullOrEmpty(webInteractionComponent.Content)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebInteractionComponent.ContentAndContentTypeRequired"), webInteractionComponent.Name), fileObject, flowType, webInteractionComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, webInteractionComponent.URI);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "URI", webInteractionComponent.Name, webInteractionComponent.URI), fileObject, flowType, webInteractionComponent);
			}
			componentsInitializationScriptSb.AppendFormat("WebInteractionComponent {0} = new WebInteractionComponent(\"{0}\", callflow, myCall, logHeader);", webInteractionComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.HttpMethod = {1};", webInteractionComponent.Name, GetHttpMethodText(webInteractionComponent.HttpRequestType)).AppendLine().Append("            ");
			if (!string.IsNullOrEmpty(webInteractionComponent.ContentType))
			{
				componentsInitializationScriptSb.AppendFormat("{0}.ContentType = \"{1}\";", webInteractionComponent.Name, webInteractionComponent.ContentType).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Timeout = {1};", webInteractionComponent.Name, 1000 * webInteractionComponent.Timeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.UriHandler = () => {{ return Convert.ToString({1}); }};", webInteractionComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			if (!string.IsNullOrEmpty(webInteractionComponent.Content))
			{
				AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, webInteractionComponent.Content);
				if (!absArgument2.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Content", webInteractionComponent.Name, webInteractionComponent.Content), fileObject, flowType, webInteractionComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ContentHandler = () => {{ return Convert.ToString({1}); }};", webInteractionComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			}
			foreach (Parameter header in webInteractionComponent.Headers)
			{
				if (!string.IsNullOrEmpty(header.Name) || !string.IsNullOrEmpty(header.Value))
				{
					if (string.IsNullOrEmpty(header.Name))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebInteractionComponent.HeaderNameIsEmpty"), webInteractionComponent.Name), fileObject, flowType, webInteractionComponent);
					}
					if (string.IsNullOrEmpty(header.Value))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebInteractionComponent.HeaderValueIsEmpty"), webInteractionComponent.Name), fileObject, flowType, webInteractionComponent);
					}
					AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, header.Value);
					if (!absArgument3.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Header - " + header.Name, webInteractionComponent.Name, header.Value), fileObject, flowType, webInteractionComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Headers.Add(new CallFlow.CFD.Parameter(\"{1}\", () => {{ return {2}; }}));", webInteractionComponent.Name, header.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, webInteractionComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
