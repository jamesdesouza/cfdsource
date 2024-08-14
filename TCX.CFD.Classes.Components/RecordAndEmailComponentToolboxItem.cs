using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Net.Mail;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class RecordAndEmailComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		RecordAndEmailComponent recordAndEmailComponent = new RecordAndEmailComponent();
		recordAndEmailComponent.Beep = Settings.Default.RecordTemplateBeep;
		recordAndEmailComponent.MaxTime = Settings.Default.RecordTemplateMaxTime;
		recordAndEmailComponent.TerminateByDtmf = Settings.Default.RecordTemplateTerminateByDtmf;
		recordAndEmailComponent.UseServerSettings = Settings.Default.EMailSenderTemplateUseServerSettings;
		recordAndEmailComponent.Server = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateServer) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateServer) + "\""));
		recordAndEmailComponent.Port = Convert.ToString(Settings.Default.EMailSenderTemplateServerPort);
		recordAndEmailComponent.EnableSSL = (Settings.Default.EMailSenderTemplateServerEnableSSL ? "true" : "false");
		recordAndEmailComponent.UserName = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateUserName) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateUserName) + "\""));
		recordAndEmailComponent.Password = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplatePassword) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplatePassword) + "\""));
		recordAndEmailComponent.From = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateFrom) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateFrom) + "\""));
		recordAndEmailComponent.To = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateTo) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateTo) + "\""));
		recordAndEmailComponent.CC = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateCC) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateCC) + "\""));
		recordAndEmailComponent.BCC = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateBCC) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateBCC) + "\""));
		recordAndEmailComponent.Subject = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateSubject) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateSubject) + "\""));
		recordAndEmailComponent.Body = (string.IsNullOrEmpty(Settings.Default.EMailSenderTemplateBody) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.EMailSenderTemplateBody) + "\""));
		recordAndEmailComponent.Priority = ((Settings.Default.EMailSenderTemplatePriority == "High") ? MailPriority.High : ((Settings.Default.EMailSenderTemplatePriority == "Low") ? MailPriority.Low : MailPriority.Normal));
		recordAndEmailComponent.IsBodyHtml = Settings.Default.EMailSenderTemplateIsBodyHtml;
		FlowDesignerNameCreator.CreateName("RecordAndEmail", host.Container, recordAndEmailComponent);
		return new IComponent[1] { recordAndEmailComponent };
	}
}
