using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Net.Mail;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class EMailSenderComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		EMailSenderComponent eMailSenderComponent = new EMailSenderComponent();
		eMailSenderComponent.UseServerSettings = Settings.Default.EMailSenderTemplateUseServerSettings;
		eMailSenderComponent.Server = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateServer) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateServer) + "\""));
		eMailSenderComponent.Port = Convert.ToString(Settings.Default.EMailSenderTemplateServerPort);
		eMailSenderComponent.EnableSSL = (Settings.Default.EMailSenderTemplateServerEnableSSL ? "true" : "false");
		eMailSenderComponent.UserName = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateUserName) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateUserName) + "\""));
		eMailSenderComponent.Password = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplatePassword) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplatePassword) + "\""));
		eMailSenderComponent.From = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateFrom) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateFrom) + "\""));
		eMailSenderComponent.To = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateTo) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateTo) + "\""));
		eMailSenderComponent.CC = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateCC) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateCC) + "\""));
		eMailSenderComponent.BCC = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateBCC) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateBCC) + "\""));
		eMailSenderComponent.Subject = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateSubject) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateSubject) + "\""));
		eMailSenderComponent.Body = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateBody) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateBody) + "\""));
		eMailSenderComponent.Priority = ((Settings.Default.EMailSenderTemplatePriority == "High") ? MailPriority.High : ((Settings.Default.EMailSenderTemplatePriority == "Low") ? MailPriority.Low : MailPriority.Normal));
		eMailSenderComponent.IgnoreMissingAttachments = Settings.Default.EMailSenderTemplateIgnoreMissingAttachments;
		eMailSenderComponent.IsBodyHtml = Settings.Default.EMailSenderTemplateIsBodyHtml;
		FlowDesignerNameCreator.CreateName("EmailSender", host.Container, eMailSenderComponent);
		return new IComponent[1] { eMailSenderComponent };
	}
}
