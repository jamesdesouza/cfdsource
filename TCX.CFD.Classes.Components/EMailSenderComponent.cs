using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Net.Mail;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(EMailSenderComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(EMailSenderComponentToolboxItem))]
[ToolboxBitmap(typeof(EMailSenderComponent), "Resources.EMailSender.png")]
[ActivityValidator(typeof(EMailSenderComponentValidator))]
public class EMailSenderComponent : AbsVadActivity
{
	private List<MailAttachment> attachments = new List<MailAttachment>();

	private readonly XmlSerializer attachmentSerializer = new XmlSerializer(typeof(List<MailAttachment>));

	[Category("E-Mail Sender")]
	[Description("True to use 3CX Server email settings, False to specify SMTP server settings here.")]
	public bool UseServerSettings { get; set; } = true;


	[Category("E-Mail Sender")]
	[Description("The SMTP Server name or IP address.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Server { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The port number where the server is listening for incoming connections.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Port { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("If true, the connection with the SMTP server will be established over SSL.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string EnableSSL { get; set; } = "false";


	[Category("E-Mail Sender")]
	[Description("The username to use when connecting to the SMTP Server.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string UserName { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The password to use when connecting to the SMTP Server.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Password { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The e-mail address to use in the 'from' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string From { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The e-mail address to use in the 'to' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string To { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The e-mail address to use in the 'cc' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string CC { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The e-mail address to use in the 'bcc' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string BCC { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The subject of the e-mail message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Subject { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The body of the e-mail message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Body { get; set; } = string.Empty;


	[Category("E-Mail Sender")]
	[Description("The priority of the e-mail message.")]
	public MailPriority Priority { get; set; }

	[Category("E-Mail Sender")]
	[Description("True to silently ignore missing attachment files on runtime, False to cause a runtime error when that condition occurs.")]
	public bool IgnoreMissingAttachments { get; set; }

	[Category("E-Mail Sender")]
	[Description("True to send the mail body as HTML, False to send it as plain text.")]
	public bool IsBodyHtml { get; set; }

	[Browsable(false)]
	public string AttachmentList
	{
		get
		{
			return SerializationHelper.Serialize(attachmentSerializer, attachments);
		}
		set
		{
			attachments = SerializationHelper.Deserialize(attachmentSerializer, value) as List<MailAttachment>;
		}
	}

	[Category("E-Mail Sender")]
	[Description("The list of attachments to send with the e-mail message.")]
	[Editor(typeof(MailAttachmentCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<MailAttachment> Attachments
	{
		get
		{
			List<MailAttachment> list = new List<MailAttachment>();
			foreach (MailAttachment attachment in attachments)
			{
				list.Add(new MailAttachment(attachment.Name, attachment.File));
			}
			return list;
		}
		set
		{
			attachments = value;
		}
	}

	public EMailSenderComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		Server = ExpressionHelper.RenameComponent(this, Server, oldValue, newValue);
		Port = ExpressionHelper.RenameComponent(this, Port, oldValue, newValue);
		EnableSSL = ExpressionHelper.RenameComponent(this, EnableSSL, oldValue, newValue);
		UserName = ExpressionHelper.RenameComponent(this, UserName, oldValue, newValue);
		Password = ExpressionHelper.RenameComponent(this, Password, oldValue, newValue);
		From = ExpressionHelper.RenameComponent(this, From, oldValue, newValue);
		To = ExpressionHelper.RenameComponent(this, To, oldValue, newValue);
		CC = ExpressionHelper.RenameComponent(this, CC, oldValue, newValue);
		BCC = ExpressionHelper.RenameComponent(this, BCC, oldValue, newValue);
		Subject = ExpressionHelper.RenameComponent(this, Subject, oldValue, newValue);
		Body = ExpressionHelper.RenameComponent(this, Body, oldValue, newValue);
		foreach (MailAttachment attachment in attachments)
		{
			attachment.File = ExpressionHelper.RenameComponent(this, attachment.File, oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		Server = ExpressionHelper.MigrateConstantStringExpression(this, Server);
		Port = ExpressionHelper.MigrateConstantStringExpression(this, Port);
		EnableSSL = ExpressionHelper.MigrateConstantStringExpression(this, EnableSSL);
		UserName = ExpressionHelper.MigrateConstantStringExpression(this, UserName);
		Password = ExpressionHelper.MigrateConstantStringExpression(this, Password);
		To = ExpressionHelper.MigrateConstantStringExpression(this, To);
		CC = ExpressionHelper.MigrateConstantStringExpression(this, CC);
		BCC = ExpressionHelper.MigrateConstantStringExpression(this, BCC);
		Subject = ExpressionHelper.MigrateConstantStringExpression(this, Subject);
		Body = ExpressionHelper.MigrateConstantStringExpression(this, Body);
		foreach (MailAttachment attachment in attachments)
		{
			attachment.File = ExpressionHelper.MigrateConstantStringExpression(this, attachment.File);
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new EMailSenderComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.myg5euxb5km9");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "EMailSenderComponent";
	}
}
