using System.Net.Mail;
using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class RecordAndEmailComponentCompiler : AbsComponentCompiler
{
	private readonly RecordAndEmailComponent recordAndEmailComponent;

	public RecordAndEmailComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, RecordAndEmailComponent recordAndEmailComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(recordAndEmailComponent), recordAndEmailComponent.GetRootFlow().FlowType, recordAndEmailComponent)
	{
		this.recordAndEmailComponent = recordAndEmailComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (!recordAndEmailComponent.UseServerSettings && string.IsNullOrEmpty(recordAndEmailComponent.Server))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.ServerIsEmpty"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (!recordAndEmailComponent.UseServerSettings && string.IsNullOrEmpty(recordAndEmailComponent.From))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.FromIsEmpty"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (string.IsNullOrEmpty(recordAndEmailComponent.To) && string.IsNullOrEmpty(recordAndEmailComponent.CC) && string.IsNullOrEmpty(recordAndEmailComponent.BCC))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.DestinationIsEmpty"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (string.IsNullOrEmpty(recordAndEmailComponent.Subject) && string.IsNullOrEmpty(recordAndEmailComponent.Body))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.MessageIsEmpty"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (recordAndEmailComponent.Prompts.Count == 0)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.NoPrompts"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (!recordAndEmailComponent.UseServerSettings && recordAndEmailComponent.From.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.From) && !EMailValidator.IsEmail(recordAndEmailComponent.From.Substring(1, recordAndEmailComponent.From.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.FromIsInvalid"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (recordAndEmailComponent.To.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.To) && !EMailValidator.IsEmailList(recordAndEmailComponent.To.Substring(1, recordAndEmailComponent.To.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.ToIsInvalid"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (recordAndEmailComponent.CC.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.CC) && !EMailValidator.IsEmailList(recordAndEmailComponent.CC.Substring(1, recordAndEmailComponent.CC.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.CCIsInvalid"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else if (recordAndEmailComponent.BCC.Length > 2 && ExpressionHelper.IsStringLiteral(recordAndEmailComponent.BCC) && !EMailValidator.IsEmailList(recordAndEmailComponent.BCC.Substring(1, recordAndEmailComponent.BCC.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.RecordAndEmailComponent.BCCIsInvalid"), recordAndEmailComponent.Name), fileObject, flowType, recordAndEmailComponent);
		}
		else
		{
			if (recordAndEmailComponent.Beep)
			{
				audioFileCollector.IncludeBeepFile = true;
			}
			componentsInitializationScriptSb.AppendFormat("RecordAndEmailComponent {0} = new RecordAndEmailComponent(\"{0}\", callflow, myCall, logHeader);", recordAndEmailComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Beep = {1};", recordAndEmailComponent.Name, recordAndEmailComponent.Beep ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxTime = {1}000;", recordAndEmailComponent.Name, recordAndEmailComponent.MaxTime).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TerminateByDtmf = {1};", recordAndEmailComponent.Name, recordAndEmailComponent.TerminateByDtmf ? "true" : "false").AppendLine().Append("            ");
			foreach (Prompt prompt in recordAndEmailComponent.Prompts)
			{
				prompt.Accept(this, isDebugBuild, recordAndEmailComponent.Name, "Prompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.UseServerSettings = {1};", recordAndEmailComponent.Name, recordAndEmailComponent.UseServerSettings ? "true" : "false").AppendLine().Append("            ");
			if (!recordAndEmailComponent.UseServerSettings)
			{
				AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Server);
				if (!absArgument.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Server", recordAndEmailComponent.Name, recordAndEmailComponent.Server), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ServerHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
				if (!string.IsNullOrEmpty(recordAndEmailComponent.Port))
				{
					AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Port);
					if (!absArgument2.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Port", recordAndEmailComponent.Name, recordAndEmailComponent.Port), fileObject, flowType, recordAndEmailComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.PortHandler = () => {{ return Convert.ToInt32({1}); }};", recordAndEmailComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.EnableSSL))
				{
					AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.EnableSSL);
					if (!absArgument3.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "EnableSSL", recordAndEmailComponent.Name, recordAndEmailComponent.EnableSSL), fileObject, flowType, recordAndEmailComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.EnableSSLHandler = () => {{ return Convert.ToBoolean({1}); }};", recordAndEmailComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.UserName))
				{
					AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.UserName);
					if (!absArgument4.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "UserName", recordAndEmailComponent.Name, recordAndEmailComponent.UserName), fileObject, flowType, recordAndEmailComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.UserNameHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument4.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrEmpty(recordAndEmailComponent.Password))
				{
					AbsArgument absArgument5 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Password);
					if (!absArgument5.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Password", recordAndEmailComponent.Name, recordAndEmailComponent.Password), fileObject, flowType, recordAndEmailComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.PasswordHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument5.GetCompilerString()).AppendLine().Append("            ");
				}
				AbsArgument absArgument6 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.From);
				if (!absArgument6.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "From", recordAndEmailComponent.Name, recordAndEmailComponent.From), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.FromHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument6.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(recordAndEmailComponent.To))
			{
				AbsArgument absArgument7 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.To);
				if (!absArgument7.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "To", recordAndEmailComponent.Name, recordAndEmailComponent.To), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ToHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument7.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(recordAndEmailComponent.CC))
			{
				AbsArgument absArgument8 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.CC);
				if (!absArgument8.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "CC", recordAndEmailComponent.Name, recordAndEmailComponent.CC), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.CCHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument8.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(recordAndEmailComponent.BCC))
			{
				AbsArgument absArgument9 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.BCC);
				if (!absArgument9.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "BCC", recordAndEmailComponent.Name, recordAndEmailComponent.BCC), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.BCCHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument9.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(recordAndEmailComponent.Subject))
			{
				AbsArgument absArgument10 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Subject);
				if (!absArgument10.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Subject", recordAndEmailComponent.Name, recordAndEmailComponent.Subject), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.SubjectHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument10.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(recordAndEmailComponent.Body))
			{
				AbsArgument absArgument11 = AbsArgument.BuildArgument(validVariables, recordAndEmailComponent.Body);
				if (!absArgument11.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Body", recordAndEmailComponent.Name, recordAndEmailComponent.Body), fileObject, flowType, recordAndEmailComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.BodyHandler = () => {{ return Convert.ToString({1}); }};", recordAndEmailComponent.Name, absArgument11.GetCompilerString()).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.IsBodyHtml = {1};", recordAndEmailComponent.Name, recordAndEmailComponent.IsBodyHtml ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Priority = {1};", recordAndEmailComponent.Name, (recordAndEmailComponent.Priority == MailPriority.Normal) ? "MessagePriority.Normal" : ((recordAndEmailComponent.Priority == MailPriority.High) ? "MessagePriority.Urgent" : "MessagePriority.NonUrgent")).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, recordAndEmailComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
