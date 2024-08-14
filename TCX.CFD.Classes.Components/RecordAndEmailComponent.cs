using System;
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
[Designer(typeof(RecordAndEmailComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(RecordAndEmailComponentToolboxItem))]
[ToolboxBitmap(typeof(RecordAndEmailComponent), "Resources.RecordEmail.png")]
[ActivityValidator(typeof(RecordAndEmailComponentValidator))]
public class RecordAndEmailComponent : AbsVadActivity
{
	private bool beep = true;

	private uint maxTime = 60u;

	private bool terminateByDtmf;

	private List<Prompt> prompts = new List<Prompt>();

	private readonly XmlSerializer promptSerializer = new XmlSerializer(typeof(List<Prompt>));

	private bool useServerSettings = true;

	private string server = string.Empty;

	private string port = string.Empty;

	private string enableSSL = "false";

	private string userName = string.Empty;

	private string password = string.Empty;

	private string from = string.Empty;

	private string to = string.Empty;

	private string cc = string.Empty;

	private string bcc = string.Empty;

	private string subject = string.Empty;

	private string body = string.Empty;

	private MailPriority priority;

	private bool isBodyHtml;

	[Category("Record and Email")]
	[Description("If true, a beep will be played just before recording starts.")]
	public bool Beep
	{
		get
		{
			return beep;
		}
		set
		{
			beep = value;
		}
	}

	[Category("Record and Email")]
	[Description("The maximum duration to record, in seconds.")]
	public uint MaxTime
	{
		get
		{
			return maxTime;
		}
		set
		{
			if (value < 1 || value > 99999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 1, 99999));
			}
			maxTime = value;
		}
	}

	[Category("Record and Email")]
	[Description("If true, any DTMF keypress will stop the recording.")]
	public bool TerminateByDtmf
	{
		get
		{
			return terminateByDtmf;
		}
		set
		{
			terminateByDtmf = value;
		}
	}

	[Browsable(false)]
	public string PromptList
	{
		get
		{
			return SerializationHelper.Serialize(promptSerializer, prompts);
		}
		set
		{
			prompts = SerializationHelper.Deserialize(promptSerializer, value) as List<Prompt>;
		}
	}

	[Category("Record and Email")]
	[Description("The list of prompts to play before recording.")]
	[Editor(typeof(PromptCollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Prompt> Prompts
	{
		get
		{
			List<Prompt> list = new List<Prompt>();
			foreach (Prompt prompt in prompts)
			{
				list.Add(prompt.Clone());
			}
			return list;
		}
		set
		{
			prompts = value;
		}
	}

	[Category("Record and Email")]
	[Description("True to use 3CX Server email settings, False to specify SMTP server settings here.")]
	public bool UseServerSettings
	{
		get
		{
			return useServerSettings;
		}
		set
		{
			useServerSettings = value;
		}
	}

	[Category("Record and Email")]
	[Description("The SMTP Server name or IP address.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Server
	{
		get
		{
			return server;
		}
		set
		{
			server = value;
		}
	}

	[Category("Record and Email")]
	[Description("The port number where the server is listening for incoming connections.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Port
	{
		get
		{
			return port;
		}
		set
		{
			port = value;
		}
	}

	[Category("Record and Email")]
	[Description("If true, the connection with the SMTP server will be established over SSL.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string EnableSSL
	{
		get
		{
			return enableSSL;
		}
		set
		{
			enableSSL = value;
		}
	}

	[Category("Record and Email")]
	[Description("The username to use when connecting to the SMTP Server.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string UserName
	{
		get
		{
			return userName;
		}
		set
		{
			userName = value;
		}
	}

	[Category("Record and Email")]
	[Description("The password to use when connecting to the SMTP Server.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Password
	{
		get
		{
			return password;
		}
		set
		{
			password = value;
		}
	}

	[Category("Record and Email")]
	[Description("The e-mail address to use in the 'from' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string From
	{
		get
		{
			return from;
		}
		set
		{
			from = value;
		}
	}

	[Category("Record and Email")]
	[Description("The e-mail address to use in the 'to' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string To
	{
		get
		{
			return to;
		}
		set
		{
			to = value;
		}
	}

	[Category("Record and Email")]
	[Description("The e-mail address to use in the 'cc' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string CC
	{
		get
		{
			return cc;
		}
		set
		{
			cc = value;
		}
	}

	[Category("Record and Email")]
	[Description("The e-mail address to use in the 'bcc' field of the message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string BCC
	{
		get
		{
			return bcc;
		}
		set
		{
			bcc = value;
		}
	}

	[Category("Record and Email")]
	[Description("The subject of the e-mail message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Subject
	{
		get
		{
			return subject;
		}
		set
		{
			subject = value;
		}
	}

	[Category("Record and Email")]
	[Description("The body of the e-mail message.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Body
	{
		get
		{
			return body;
		}
		set
		{
			body = value;
		}
	}

	[Category("Record and Email")]
	[Description("The priority of the e-mail message.")]
	public MailPriority Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	[Category("Record and Email")]
	[Description("True to send the mail body as HTML, False to send it as plain text.")]
	public bool IsBodyHtml
	{
		get
		{
			return isBodyHtml;
		}
		set
		{
			isBodyHtml = value;
		}
	}

	public RecordAndEmailComponent()
	{
		InitializeComponent();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = this;
			prompt.NotifyComponentRenamed(oldValue, newValue);
		}
		server = ExpressionHelper.RenameComponent(this, server, oldValue, newValue);
		port = ExpressionHelper.RenameComponent(this, port, oldValue, newValue);
		enableSSL = ExpressionHelper.RenameComponent(this, enableSSL, oldValue, newValue);
		userName = ExpressionHelper.RenameComponent(this, userName, oldValue, newValue);
		password = ExpressionHelper.RenameComponent(this, password, oldValue, newValue);
		from = ExpressionHelper.RenameComponent(this, from, oldValue, newValue);
		to = ExpressionHelper.RenameComponent(this, to, oldValue, newValue);
		cc = ExpressionHelper.RenameComponent(this, cc, oldValue, newValue);
		bcc = ExpressionHelper.RenameComponent(this, bcc, oldValue, newValue);
		subject = ExpressionHelper.RenameComponent(this, subject, oldValue, newValue);
		body = ExpressionHelper.RenameComponent(this, body, oldValue, newValue);
	}

	public override bool IsCallRelated()
	{
		return true;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (Prompt prompt in prompts)
		{
			prompt.ContainerActivity = this;
			prompt.MigrateConstantStringExpressions();
		}
		server = ExpressionHelper.MigrateConstantStringExpression(this, server);
		port = ExpressionHelper.MigrateConstantStringExpression(this, port);
		enableSSL = ExpressionHelper.MigrateConstantStringExpression(this, enableSSL);
		userName = ExpressionHelper.MigrateConstantStringExpression(this, userName);
		password = ExpressionHelper.MigrateConstantStringExpression(this, password);
		to = ExpressionHelper.MigrateConstantStringExpression(this, to);
		cc = ExpressionHelper.MigrateConstantStringExpression(this, cc);
		bcc = ExpressionHelper.MigrateConstantStringExpression(this, bcc);
		subject = ExpressionHelper.MigrateConstantStringExpression(this, subject);
		body = ExpressionHelper.MigrateConstantStringExpression(this, body);
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new RecordAndEmailComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.ql40cxor0qax");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "RecordAndEmailComponent";
	}
}
