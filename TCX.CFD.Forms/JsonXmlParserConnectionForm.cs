using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class JsonXmlParserConnectionForm : Form
{
	private readonly IVadActivity referencedComponent;

	private readonly List<string> validVariables;

	private readonly IList variableValues;

	private IContainer components;

	private ProgressBar connectionProgressBar;

	private BackgroundWorker backgroundRequestWorker;

	public string Result { get; private set; } = "";


	public JsonXmlParserConnectionForm(IVadActivity referencedComponent, IList variableValues)
	{
		InitializeComponent();
		this.referencedComponent = referencedComponent;
		validVariables = ExpressionHelper.GetValidVariables(referencedComponent);
		this.variableValues = variableValues;
		Text = LocalizedResourceMgr.GetString("JsonXmlParserConnectionForm.Title");
	}

	private void JsonXmlParserConnectionForm_Load(object sender, EventArgs e)
	{
		backgroundRequestWorker.RunWorkerAsync();
	}

	private HttpMethod GetHttpMethod(HttpRequestTypes requestType)
	{
		return requestType switch
		{
			HttpRequestTypes.DELETE => HttpMethod.Delete, 
			HttpRequestTypes.GET => HttpMethod.Get, 
			HttpRequestTypes.HEAD => HttpMethod.Head, 
			HttpRequestTypes.OPTIONS => HttpMethod.Options, 
			HttpRequestTypes.POST => HttpMethod.Post, 
			HttpRequestTypes.PUT => HttpMethod.Put, 
			HttpRequestTypes.TRACE => HttpMethod.Trace, 
			_ => throw new Exception("Invalid request type: " + requestType), 
		};
	}

	private async Task<string> ExecuteRequest(string uri, HttpRequestTypes requestType, string contentType, string content, List<Parameter> headers, int timeout)
	{
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, uri);
		using HttpRequestMessage httpRequest = new HttpRequestMessage(GetHttpMethod(requestType), Convert.ToString(absArgument.Evaluate(variableValues)));
		AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, content);
		if (!string.IsNullOrEmpty(contentType) && !string.IsNullOrEmpty(content))
		{
			httpRequest.Content = new StringContent(Convert.ToString(absArgument2.Evaluate(variableValues)), Encoding.UTF8, contentType);
		}
		for (int i = 0; i < headers.Count; i++)
		{
			Parameter parameter = headers[i];
			AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, parameter.Value);
			httpRequest.Headers.TryAddWithoutValidation(parameter.Name, Convert.ToString(absArgument3.Evaluate(variableValues)));
		}
		using HttpClient httpClient = new HttpClient();
		if (timeout > 0)
		{
			httpClient.Timeout = new TimeSpan(0, 0, 0, timeout);
		}
		using HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
		return await httpResponse.Content.ReadAsStringAsync();
	}

	private async Task<string> ExecuteRequest(WebInteractionComponent component)
	{
		return await ExecuteRequest(component.URI, component.HttpRequestType, component.ContentType, component.Content, component.Headers, (int)component.Timeout);
	}

	private async Task<string> ExecuteRequest(WebServiceRestComponent component)
	{
		List<Parameter> list = new List<Parameter>();
		switch (component.AuthenticationType)
		{
		case WebServiceAuthenticationTypes.BasicUserPassword:
		{
			AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, component.AuthenticationUserName);
			AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, component.AuthenticationPassword);
			list.Add(new Parameter("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(absArgument3.Evaluate(variableValues)?.ToString() + ":" + absArgument4.Evaluate(variableValues)))));
			break;
		}
		case WebServiceAuthenticationTypes.BasicApiKey:
		{
			AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, component.AuthenticationApiKey);
			list.Add(new Parameter("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(absArgument2.Evaluate(variableValues)?.ToString() + ":X"))));
			break;
		}
		case WebServiceAuthenticationTypes.OAuth2:
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, component.AuthenticationOAuth2AccessToken);
			list.Add(new Parameter("Authorization", "Bearer " + absArgument.Evaluate(variableValues)));
			break;
		}
		}
		foreach (Parameter header in component.Headers)
		{
			list.Add(header);
		}
		return await ExecuteRequest(component.URI, component.HttpRequestType, component.ContentType, component.Content, list, (int)component.Timeout);
	}

	private async Task<string> ExecuteRequest(WebServicesInteractionComponent component)
	{
		return await ExecuteRequest(component.URI + "/" + component.WebServiceName, HttpRequestTypes.POST, component.ContentType, component.Content, component.Headers, (int)component.Timeout);
	}

	private void BackgroundRequestWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		if (referencedComponent is WebInteractionComponent)
		{
			e.Result = ExecuteRequest(referencedComponent as WebInteractionComponent).Result;
		}
		else if (referencedComponent is WebServiceRestComponent)
		{
			e.Result = ExecuteRequest(referencedComponent as WebServiceRestComponent).Result;
		}
		else if (referencedComponent is WebServicesInteractionComponent)
		{
			e.Result = ExecuteRequest(referencedComponent as WebServicesInteractionComponent).Result;
		}
	}

	private void BackgroundRequestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserConnectionForm.MessageBox.Error.ExecutingRequest") + ErrorHelper.GetErrorDescription(e.Error), LocalizedResourceMgr.GetString("JsonXmlParserConnectionForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			base.DialogResult = DialogResult.Cancel;
		}
		else if (string.IsNullOrEmpty(e.Result as string))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserConnectionForm.MessageBox.Error.EmptyResponse"), LocalizedResourceMgr.GetString("JsonXmlParserConnectionForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			base.DialogResult = DialogResult.Cancel;
		}
		else
		{
			Result = e.Result as string;
			base.DialogResult = DialogResult.OK;
		}
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.connectionProgressBar = new System.Windows.Forms.ProgressBar();
		this.backgroundRequestWorker = new System.ComponentModel.BackgroundWorker();
		base.SuspendLayout();
		this.connectionProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.connectionProgressBar.Location = new System.Drawing.Point(12, 33);
		this.connectionProgressBar.Name = "connectionProgressBar";
		this.connectionProgressBar.Size = new System.Drawing.Size(558, 23);
		this.connectionProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
		this.connectionProgressBar.TabIndex = 0;
		this.backgroundRequestWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundRequestWorker_DoWork);
		this.backgroundRequestWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BackgroundRequestWorker_RunWorkerCompleted);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(582, 95);
		base.Controls.Add(this.connectionProgressBar);
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "JsonXmlParserConnectionForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Connecting...";
		base.Load += new System.EventHandler(JsonXmlParserConnectionForm_Load);
		base.ResumeLayout(false);
	}
}
