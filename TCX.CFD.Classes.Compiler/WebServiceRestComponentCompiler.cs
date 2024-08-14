using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class WebServiceRestComponentCompiler : AbsComponentCompiler
{
	private readonly WebServiceRestComponent webServiceRestComponent;

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

	public WebServiceRestComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, WebServiceRestComponent webServiceRestComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(webServiceRestComponent), webServiceRestComponent.GetRootFlow().FlowType, webServiceRestComponent)
	{
		this.webServiceRestComponent = webServiceRestComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(webServiceRestComponent.URI))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.UriIsEmpty"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
		}
		else if ((string.IsNullOrEmpty(webServiceRestComponent.Content) && !string.IsNullOrEmpty(webServiceRestComponent.ContentType)) || (string.IsNullOrEmpty(webServiceRestComponent.ContentType) && !string.IsNullOrEmpty(webServiceRestComponent.Content)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.ContentAndContentTypeRequired"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
		}
		else if (webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.BasicUserPassword && (string.IsNullOrEmpty(webServiceRestComponent.AuthenticationUserName) || string.IsNullOrEmpty(webServiceRestComponent.AuthenticationPassword)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.UserNamePasswordRequired"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
		}
		else if (webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.BasicApiKey && string.IsNullOrEmpty(webServiceRestComponent.AuthenticationApiKey))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.ApiKeyRequired"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
		}
		else if (webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.OAuth2 && string.IsNullOrEmpty(webServiceRestComponent.AuthenticationOAuth2AccessToken))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.AccessTokenRequired"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, webServiceRestComponent.URI);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "URI", webServiceRestComponent.Name, webServiceRestComponent.URI), fileObject, flowType, webServiceRestComponent);
			}
			componentsInitializationScriptSb.AppendFormat("WebInteractionComponent {0} = new WebInteractionComponent(\"{0}\", callflow, myCall, logHeader);", webServiceRestComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.HttpMethod = {1};", webServiceRestComponent.Name, GetHttpMethodText(webServiceRestComponent.HttpRequestType)).AppendLine().Append("            ");
			if (!string.IsNullOrEmpty(webServiceRestComponent.ContentType))
			{
				componentsInitializationScriptSb.AppendFormat("{0}.ContentType = \"{1}\";", webServiceRestComponent.Name, webServiceRestComponent.ContentType).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Timeout = {1};", webServiceRestComponent.Name, 1000 * webServiceRestComponent.Timeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.UriHandler = () => {{ return Convert.ToString({1}); }};", webServiceRestComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			if (!string.IsNullOrEmpty(webServiceRestComponent.Content))
			{
				AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, webServiceRestComponent.Content);
				if (!absArgument2.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Content", webServiceRestComponent.Name, webServiceRestComponent.Content), fileObject, flowType, webServiceRestComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ContentHandler = () => {{ return Convert.ToString({1}); }};", webServiceRestComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
			}
			switch (webServiceRestComponent.AuthenticationType)
			{
			case WebServiceAuthenticationTypes.BasicUserPassword:
			{
				AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationUserName);
				if (!absArgument4.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "AuthenticationUserName", webServiceRestComponent.Name, webServiceRestComponent.AuthenticationUserName), fileObject, flowType, webServiceRestComponent);
				}
				AbsArgument absArgument5 = AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationPassword);
				if (!absArgument5.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "AuthenticationPassword", webServiceRestComponent.Name, webServiceRestComponent.AuthenticationPassword), fileObject, flowType, webServiceRestComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.Headers.Add(new CallFlow.CFD.Parameter(\"Authorization\", () => {{ return \"Basic \" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(({1}) + \":\" + ({2}))); }}));", webServiceRestComponent.Name, absArgument4.GetCompilerString(), absArgument5.GetCompilerString()).AppendLine().Append("            ");
				break;
			}
			case WebServiceAuthenticationTypes.BasicApiKey:
			{
				AbsArgument absArgument6 = AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationApiKey);
				if (!absArgument6.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "AuthenticationApiKey", webServiceRestComponent.Name, webServiceRestComponent.AuthenticationApiKey), fileObject, flowType, webServiceRestComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.Headers.Add(new CallFlow.CFD.Parameter(\"Authorization\", () => {{ return \"Basic \" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(({1}) + \":X\")); }}));", webServiceRestComponent.Name, absArgument6.GetCompilerString()).AppendLine().Append("            ");
				break;
			}
			case WebServiceAuthenticationTypes.OAuth2:
			{
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, webServiceRestComponent.AuthenticationOAuth2AccessToken);
				if (!absArgument3.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "AuthenticationOAuth2AccessToken", webServiceRestComponent.Name, webServiceRestComponent.AuthenticationOAuth2AccessToken), fileObject, flowType, webServiceRestComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.Headers.Add(new CallFlow.CFD.Parameter(\"Authorization\", () => {{ return \"Bearer \" + ({1}); }}));", webServiceRestComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				break;
			}
			}
			foreach (Parameter header in webServiceRestComponent.Headers)
			{
				if (!string.IsNullOrEmpty(header.Name) || !string.IsNullOrEmpty(header.Value))
				{
					if (string.IsNullOrEmpty(header.Name))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.HeaderNameIsEmpty"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
					}
					if (string.IsNullOrEmpty(header.Value))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.WebServiceRestComponent.HeaderValueIsEmpty"), webServiceRestComponent.Name), fileObject, flowType, webServiceRestComponent);
					}
					AbsArgument absArgument7 = AbsArgument.BuildArgument(validVariables, header.Value);
					if (!absArgument7.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Header - " + header.Name, webServiceRestComponent.Name, header.Value), fileObject, flowType, webServiceRestComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Headers.Add(new CallFlow.CFD.Parameter(\"{1}\", () => {{ return {2}; }}));", webServiceRestComponent.Name, header.Name, absArgument7.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, webServiceRestComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
