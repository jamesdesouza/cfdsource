using System.Net.Mail;
using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class EMailSenderComponentCompiler : AbsComponentCompiler
{
	private readonly EMailSenderComponent eMailSenderComponent;

	public EMailSenderComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, EMailSenderComponent eMailSenderComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(eMailSenderComponent), eMailSenderComponent.GetRootFlow().FlowType, eMailSenderComponent)
	{
		this.eMailSenderComponent = eMailSenderComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (!eMailSenderComponent.UseServerSettings && string.IsNullOrEmpty(eMailSenderComponent.Server))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.ServerIsEmpty"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (!eMailSenderComponent.UseServerSettings && string.IsNullOrEmpty(eMailSenderComponent.From))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.FromIsEmpty"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (string.IsNullOrEmpty(eMailSenderComponent.To) && string.IsNullOrEmpty(eMailSenderComponent.CC) && string.IsNullOrEmpty(eMailSenderComponent.BCC))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.DestinationIsEmpty"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (string.IsNullOrEmpty(eMailSenderComponent.Subject) && string.IsNullOrEmpty(eMailSenderComponent.Body))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.MessageIsEmpty"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (!eMailSenderComponent.UseServerSettings && eMailSenderComponent.From.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.From) && !EMailValidator.IsEmail(eMailSenderComponent.From.Substring(1, eMailSenderComponent.From.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.FromIsInvalid"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (eMailSenderComponent.To.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.To) && !EMailValidator.IsEmailList(eMailSenderComponent.To.Substring(1, eMailSenderComponent.To.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.ToIsInvalid"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (eMailSenderComponent.CC.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.CC) && !EMailValidator.IsEmailList(eMailSenderComponent.CC.Substring(1, eMailSenderComponent.CC.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.CCIsInvalid"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else if (eMailSenderComponent.BCC.Length > 2 && ExpressionHelper.IsStringLiteral(eMailSenderComponent.BCC) && !EMailValidator.IsEmailList(eMailSenderComponent.BCC.Substring(1, eMailSenderComponent.BCC.Length - 2)))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.BCCIsInvalid"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
		}
		else
		{
			componentsInitializationScriptSb.AppendFormat("EMailSenderComponent {0} = new EMailSenderComponent(\"{0}\", callflow, myCall, logHeader);", eMailSenderComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.UseServerSettings = {1};", eMailSenderComponent.Name, eMailSenderComponent.UseServerSettings ? "true" : "false").AppendLine().Append("            ");
			if (!eMailSenderComponent.UseServerSettings)
			{
				AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Server);
				if (!absArgument.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Server", eMailSenderComponent.Name, eMailSenderComponent.Server), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ServerHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
				if (!string.IsNullOrEmpty(eMailSenderComponent.Port))
				{
					AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Port);
					if (!absArgument2.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Port", eMailSenderComponent.Name, eMailSenderComponent.Port), fileObject, flowType, eMailSenderComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.PortHandler = () => {{ return Convert.ToInt32({1}); }};", eMailSenderComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.EnableSSL))
				{
					AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.EnableSSL);
					if (!absArgument3.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "EnableSSL", eMailSenderComponent.Name, eMailSenderComponent.EnableSSL), fileObject, flowType, eMailSenderComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.EnableSSLHandler = () => {{ return Convert.ToBoolean({1}); }};", eMailSenderComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.UserName))
				{
					AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.UserName);
					if (!absArgument4.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "UserName", eMailSenderComponent.Name, eMailSenderComponent.UserName), fileObject, flowType, eMailSenderComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.UserNameHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument4.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrEmpty(eMailSenderComponent.Password))
				{
					AbsArgument absArgument5 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Password);
					if (!absArgument5.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Password", eMailSenderComponent.Name, eMailSenderComponent.Password), fileObject, flowType, eMailSenderComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.PasswordHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument5.GetCompilerString()).AppendLine().Append("            ");
				}
				AbsArgument absArgument6 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.From);
				if (!absArgument6.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "From", eMailSenderComponent.Name, eMailSenderComponent.From), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.FromHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument6.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(eMailSenderComponent.To))
			{
				AbsArgument absArgument7 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.To);
				if (!absArgument7.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "To", eMailSenderComponent.Name, eMailSenderComponent.To), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ToHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument7.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(eMailSenderComponent.CC))
			{
				AbsArgument absArgument8 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.CC);
				if (!absArgument8.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "CC", eMailSenderComponent.Name, eMailSenderComponent.CC), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.CCHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument8.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(eMailSenderComponent.BCC))
			{
				AbsArgument absArgument9 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.BCC);
				if (!absArgument9.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "BCC", eMailSenderComponent.Name, eMailSenderComponent.BCC), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.BCCHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument9.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(eMailSenderComponent.Subject))
			{
				AbsArgument absArgument10 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Subject);
				if (!absArgument10.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Subject", eMailSenderComponent.Name, eMailSenderComponent.Subject), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.SubjectHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument10.GetCompilerString()).AppendLine().Append("            ");
			}
			if (!string.IsNullOrWhiteSpace(eMailSenderComponent.Body))
			{
				AbsArgument absArgument11 = AbsArgument.BuildArgument(validVariables, eMailSenderComponent.Body);
				if (!absArgument11.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Body", eMailSenderComponent.Name, eMailSenderComponent.Body), fileObject, flowType, eMailSenderComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.BodyHandler = () => {{ return Convert.ToString({1}); }};", eMailSenderComponent.Name, absArgument11.GetCompilerString()).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.IsBodyHtml = {1};", eMailSenderComponent.Name, eMailSenderComponent.IsBodyHtml ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Priority = {1};", eMailSenderComponent.Name, (eMailSenderComponent.Priority == MailPriority.Normal) ? "MessagePriority.Normal" : ((eMailSenderComponent.Priority == MailPriority.High) ? "MessagePriority.Urgent" : "MessagePriority.NonUrgent")).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.IgnoreMissingAttachments = {1};", eMailSenderComponent.Name, eMailSenderComponent.IgnoreMissingAttachments ? "true" : "false").AppendLine().Append("            ");
			foreach (MailAttachment attachment in eMailSenderComponent.Attachments)
			{
				if (!string.IsNullOrEmpty(attachment.Name) || !string.IsNullOrEmpty(attachment.File))
				{
					if (string.IsNullOrEmpty(attachment.Name))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.AttachmentNameIsEmpty"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
					}
					if (string.IsNullOrEmpty(attachment.File))
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.EMailSenderComponent.AttachmentFileIsEmpty"), eMailSenderComponent.Name), fileObject, flowType, eMailSenderComponent);
					}
					AbsArgument absArgument12 = AbsArgument.BuildArgument(validVariables, attachment.File);
					if (!absArgument12.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "AttachmentFile - " + attachment.Name, eMailSenderComponent.Name, attachment.File), fileObject, flowType, eMailSenderComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.Attachments.Add(new MailAttachment(\"{1}\", () => {{ return Convert.ToString({2}); }}));", eMailSenderComponent.Name, attachment.Name, absArgument12.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, eMailSenderComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
