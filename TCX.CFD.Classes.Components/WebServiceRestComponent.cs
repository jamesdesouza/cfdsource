using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(WebServiceRestComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(WebServiceRestComponentToolboxItem))]
[ToolboxBitmap(typeof(WebServiceRestComponent), "Resources.WebServicesInteraction.png")]
[ActivityValidator(typeof(WebServiceRestComponentValidator))]
public class WebServiceRestComponent : AbsVadActivity
{
	private List<Parameter> headers = new List<Parameter>();

	private uint timeout;

	private readonly XmlSerializer headersSerializer = new XmlSerializer(typeof(List<Parameter>));

	[Category("Web Service REST")]
	[Description("The URI where the request must be sent.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string URI { get; set; } = string.Empty;


	[Category("Web Service REST")]
	[Description("The HTTP request method to use.")]
	public HttpRequestTypes HttpRequestType { get; set; } = HttpRequestTypes.POST;


	[Category("Web Service REST")]
	[Description("The Content-Type for the request to be sent.")]
	[TypeConverter(typeof(ContentTypesTypeConverter))]
	public string ContentType { get; set; } = "application/json";


	[Category("Web Service REST")]
	[Description("The Content to be sent in this HTTP request.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Content { get; set; } = string.Empty;


	[Category("Web Service REST")]
	[Description("The authentication type to use in this HTTP request.")]
	public WebServiceAuthenticationTypes AuthenticationType { get; set; }

	[Category("Web Service REST")]
	[Description("The authentication user name, only valid when AuthenticationType is set to BasicUserPassword.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string AuthenticationUserName { get; set; } = string.Empty;


	[Category("Web Service REST")]
	[Description("The authentication password, only valid when AuthenticationType is set to BasicUserPassword.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string AuthenticationPassword { get; set; } = string.Empty;


	[Category("Web Service REST")]
	[Description("The authentication API Key, only valid when AuthenticationType is set to BasicApiKey.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string AuthenticationApiKey { get; set; } = string.Empty;


	[Category("Web Service REST")]
	[Description("The authentication OAuth2 Access Token, only valid when AuthenticationType is set to OAuth2.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string AuthenticationOAuth2AccessToken { get; set; } = string.Empty;


	[Browsable(false)]
	public string HeaderList
	{
		get
		{
			return SerializationHelper.Serialize(headersSerializer, headers);
		}
		set
		{
			headers = SerializationHelper.Deserialize(headersSerializer, value) as List<Parameter>;
		}
	}

	[Category("Web Service REST")]
	[Description("The list of headers to send with the HTTP request.")]
	[Editor(typeof(ParameterCollectionEditor<Parameter>), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Parameter> Headers
	{
		get
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter header in headers)
			{
				list.Add(new Parameter(header.Name, header.Value));
			}
			return list;
		}
		set
		{
			headers = value;
		}
	}

	[Category("Web Service REST")]
	[Description("The time to wait while trying to connect to the Web Server before returning the Server Timeout condition, in seconds. Zero means to wait forever.")]
	public uint Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			if (value > 999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 0, 999));
			}
			timeout = value;
		}
	}

	public WebServiceRestComponent()
	{
		InitializeComponent();
		Variable item = new Variable("ResponseStatusCode", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.WebServiceRest.ResponseStatusCodeHelpText")
		};
		Variable item2 = new Variable("ResponseContent", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		{
			HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.WebServiceRest.ResponseContentHelpText")
		};
		properties.Add(item);
		properties.Add(item2);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		URI = ExpressionHelper.RenameComponent(this, URI, oldValue, newValue);
		Content = ExpressionHelper.RenameComponent(this, Content, oldValue, newValue);
		AuthenticationUserName = ExpressionHelper.RenameComponent(this, AuthenticationUserName, oldValue, newValue);
		AuthenticationPassword = ExpressionHelper.RenameComponent(this, AuthenticationPassword, oldValue, newValue);
		AuthenticationApiKey = ExpressionHelper.RenameComponent(this, AuthenticationApiKey, oldValue, newValue);
		AuthenticationOAuth2AccessToken = ExpressionHelper.RenameComponent(this, AuthenticationOAuth2AccessToken, oldValue, newValue);
		foreach (Parameter header in headers)
		{
			header.Value = ExpressionHelper.RenameComponent(this, header.Value, oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		URI = ExpressionHelper.MigrateConstantStringExpression(this, URI);
		Content = ExpressionHelper.MigrateConstantStringExpression(this, Content);
		AuthenticationUserName = ExpressionHelper.MigrateConstantStringExpression(this, AuthenticationUserName);
		AuthenticationPassword = ExpressionHelper.MigrateConstantStringExpression(this, AuthenticationPassword);
		AuthenticationApiKey = ExpressionHelper.MigrateConstantStringExpression(this, AuthenticationApiKey);
		AuthenticationOAuth2AccessToken = ExpressionHelper.MigrateConstantStringExpression(this, AuthenticationOAuth2AccessToken);
		foreach (Parameter header in headers)
		{
			header.Value = ExpressionHelper.MigrateConstantStringExpression(this, header.Value);
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new WebServiceRestComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.9y715p43wo2k");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "WebServiceRestComponent";
	}
}
