using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class WebServicesInteractionConfigurationForm : Form
{
	private readonly WebServicesInteractionComponent webServicesInteractionComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblURI;

	private Label lblWebServiceName;

	private MaskedTextBox txtTimeout;

	private Label lblTimeout;

	private Button cancelButton;

	private Button okButton;

	private TextBox txtURI;

	private TextBox txtWebServiceName;

	private ErrorProvider errorProvider;

	private Button webServiceNameExpressionButton;

	private Button uriExpressionButton;

	private Label lblContent;

	private Button contentExpressionButton;

	private TextBox txtContent;

	private ComboBox comboContentType;

	private Label lblContentType;

	private DataGridView headersGrid;

	private Label lblHeaders;

	private BindingSource headerBindingSource;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

	private DataGridViewButtonColumn expressionBuilderColumn;

	public string URI => txtURI.Text;

	public string WebServiceName => txtWebServiceName.Text;

	public string ContentType => comboContentType.Text;

	public string Content => txtContent.Text;

	public uint Timeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtTimeout.Text))
			{
				return Convert.ToUInt32(txtTimeout.Text);
			}
			return Settings.Default.WebServicesInteractionTemplateTimeout;
		}
	}

	public List<Parameter> Headers
	{
		get
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter item in headerBindingSource.List)
			{
				list.Add(item);
			}
			return list;
		}
	}

	private void FillComboContentType()
	{
		comboContentType.Items.Clear();
		comboContentType.Items.Add("application/javascript");
		comboContentType.Items.Add("application/json");
		comboContentType.Items.Add("application/x-www-form-urlencoded");
		comboContentType.Items.Add("application/pdf");
		comboContentType.Items.Add("application/xml");
		comboContentType.Items.Add("application/zip");
		comboContentType.Items.Add("multipart/form-data");
		comboContentType.Items.Add("text/css");
		comboContentType.Items.Add("text/html");
		comboContentType.Items.Add("text/plain");
		comboContentType.Items.Add("image/png");
		comboContentType.Items.Add("image/jpeg");
		comboContentType.Items.Add("image/gif");
		if (!comboContentType.Items.Contains(webServicesInteractionComponent.ContentType))
		{
			comboContentType.Items.Add(webServicesInteractionComponent.ContentType);
		}
	}

	public WebServicesInteractionConfigurationForm(WebServicesInteractionComponent webServicesInteractionComponent)
	{
		InitializeComponent();
		this.webServicesInteractionComponent = webServicesInteractionComponent;
		validVariables = ExpressionHelper.GetValidVariables(webServicesInteractionComponent);
		FillComboContentType();
		txtURI.Text = webServicesInteractionComponent.URI;
		txtWebServiceName.Text = webServicesInteractionComponent.WebServiceName;
		comboContentType.SelectedItem = webServicesInteractionComponent.ContentType;
		txtContent.Text = webServicesInteractionComponent.Content;
		txtTimeout.Text = webServicesInteractionComponent.Timeout.ToString();
		TxtURI_Validating(txtURI, new CancelEventArgs());
		TxtWebServiceName_Validating(txtWebServiceName, new CancelEventArgs());
		TxtContent_Validating(txtContent, new CancelEventArgs());
		TxtTimeout_Validating(txtTimeout, new CancelEventArgs());
		foreach (Parameter header in webServicesInteractionComponent.Headers)
		{
			headerBindingSource.List.Add(header);
		}
		Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Title");
		lblURI.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.lblURI.Text");
		lblWebServiceName.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.lblWebServiceName.Text");
		lblContentType.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.lblContentType.Text");
		lblContent.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.lblContent.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.lblTimeout.Text");
		lblHeaders.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.lblHeaders.Text");
		okButton.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtURI_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtURI.Text))
		{
			errorProvider.SetError(uriExpressionButton, LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.UriIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtURI.Text);
		errorProvider.SetError(uriExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.UriIsInvalid"));
	}

	private void UriExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServicesInteractionComponent);
		expressionEditorForm.Expression = txtURI.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtURI.Text = expressionEditorForm.Expression;
			TxtURI_Validating(txtURI, new CancelEventArgs());
		}
	}

	private void TxtWebServiceName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtWebServiceName.Text))
		{
			errorProvider.SetError(webServiceNameExpressionButton, LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.WebServiceNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtWebServiceName.Text);
		errorProvider.SetError(webServiceNameExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.WebServiceNameIsInvalid"));
	}

	private void WebServiceNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServicesInteractionComponent);
		expressionEditorForm.Expression = txtWebServiceName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtWebServiceName.Text = expressionEditorForm.Expression;
			TxtWebServiceName_Validating(txtWebServiceName, new CancelEventArgs());
		}
	}

	private void ComboContentType_TextChanged(object sender, EventArgs e)
	{
		errorProvider.SetError(comboContentType, string.IsNullOrEmpty(comboContentType.Text) ? LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.ContentTypeIsMandatory") : string.Empty);
	}

	private void TxtContent_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtContent.Text))
		{
			errorProvider.SetError(contentExpressionButton, LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.ContentIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtContent.Text);
		errorProvider.SetError(contentExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.ContentIsInvalid"));
	}

	private void ContentExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServicesInteractionComponent);
		expressionEditorForm.Expression = txtContent.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtContent.Text = expressionEditorForm.Expression;
			TxtContent_Validating(txtContent, new CancelEventArgs());
		}
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.Error.TimeoutIsMandatory") : string.Empty);
	}

	private void HeadersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2)
		{
			ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServicesInteractionComponent);
			expressionEditorForm.Expression = ((headersGrid[1, e.RowIndex].Value == null) ? string.Empty : headersGrid[1, e.RowIndex].Value.ToString());
			if (expressionEditorForm.ShowDialog() == DialogResult.OK)
			{
				headersGrid.CurrentCell = headersGrid[1, e.RowIndex];
				SendKeys.SendWait(" ");
				headersGrid.EndEdit();
				headersGrid.CurrentCell = null;
				headersGrid[1, e.RowIndex].Value = expressionEditorForm.Expression;
			}
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		List<string> list = new List<string>();
		foreach (Parameter item in headerBindingSource.List)
		{
			if (!string.IsNullOrEmpty(item.Name) || !string.IsNullOrEmpty(item.Value))
			{
				if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
				{
					MessageBox.Show(LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					headerBindingSource.Position = headerBindingSource.IndexOf(item);
					return;
				}
				if (list.Contains(item.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("WebServicesInteractionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					headerBindingSource.Position = headerBindingSource.IndexOf(item);
					return;
				}
				list.Add(item.Name);
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void WebServicesInteractionConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		webServicesInteractionComponent.ShowHelp();
	}

	private void WebServicesInteractionConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		webServicesInteractionComponent.ShowHelp();
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
		this.components = new System.ComponentModel.Container();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		this.lblURI = new System.Windows.Forms.Label();
		this.lblWebServiceName = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.txtURI = new System.Windows.Forms.TextBox();
		this.txtWebServiceName = new System.Windows.Forms.TextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.uriExpressionButton = new System.Windows.Forms.Button();
		this.webServiceNameExpressionButton = new System.Windows.Forms.Button();
		this.lblContent = new System.Windows.Forms.Label();
		this.contentExpressionButton = new System.Windows.Forms.Button();
		this.txtContent = new System.Windows.Forms.TextBox();
		this.comboContentType = new System.Windows.Forms.ComboBox();
		this.lblContentType = new System.Windows.Forms.Label();
		this.headersGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionBuilderColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		this.headerBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.lblHeaders = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.headersGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.headerBindingSource).BeginInit();
		base.SuspendLayout();
		this.lblURI.AutoSize = true;
		this.lblURI.Location = new System.Drawing.Point(16, 11);
		this.lblURI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblURI.Name = "lblURI";
		this.lblURI.Size = new System.Drawing.Size(31, 17);
		this.lblURI.TabIndex = 0;
		this.lblURI.Text = "URI";
		this.lblWebServiceName.AutoSize = true;
		this.lblWebServiceName.Location = new System.Drawing.Point(16, 46);
		this.lblWebServiceName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblWebServiceName.Name = "lblWebServiceName";
		this.lblWebServiceName.Size = new System.Drawing.Size(129, 17);
		this.lblWebServiceName.TabIndex = 3;
		this.lblWebServiceName.Text = "Web Service Name";
		this.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(165, 144);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(431, 22);
		this.txtTimeout.TabIndex = 12;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(16, 148);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 11;
		this.lblTimeout.Text = "Timeout (secs)";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 352);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 16;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 352);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 15;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.txtURI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtURI.Location = new System.Drawing.Point(165, 7);
		this.txtURI.Margin = new System.Windows.Forms.Padding(4);
		this.txtURI.MaxLength = 8192;
		this.txtURI.Name = "txtURI";
		this.txtURI.Size = new System.Drawing.Size(384, 22);
		this.txtURI.TabIndex = 1;
		this.txtURI.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtURI.Validating += new System.ComponentModel.CancelEventHandler(TxtURI_Validating);
		this.txtWebServiceName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtWebServiceName.Location = new System.Drawing.Point(165, 42);
		this.txtWebServiceName.Margin = new System.Windows.Forms.Padding(4);
		this.txtWebServiceName.MaxLength = 1024;
		this.txtWebServiceName.Name = "txtWebServiceName";
		this.txtWebServiceName.Size = new System.Drawing.Size(384, 22);
		this.txtWebServiceName.TabIndex = 4;
		this.txtWebServiceName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtWebServiceName.Validating += new System.ComponentModel.CancelEventHandler(TxtWebServiceName_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.uriExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.uriExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.uriExpressionButton.Location = new System.Drawing.Point(559, 5);
		this.uriExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.uriExpressionButton.Name = "uriExpressionButton";
		this.uriExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.uriExpressionButton.TabIndex = 2;
		this.uriExpressionButton.UseVisualStyleBackColor = true;
		this.uriExpressionButton.Click += new System.EventHandler(UriExpressionButton_Click);
		this.webServiceNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.webServiceNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.webServiceNameExpressionButton.Location = new System.Drawing.Point(559, 39);
		this.webServiceNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.webServiceNameExpressionButton.Name = "webServiceNameExpressionButton";
		this.webServiceNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.webServiceNameExpressionButton.TabIndex = 5;
		this.webServiceNameExpressionButton.UseVisualStyleBackColor = true;
		this.webServiceNameExpressionButton.Click += new System.EventHandler(WebServiceNameExpressionButton_Click);
		this.lblContent.AutoSize = true;
		this.lblContent.Location = new System.Drawing.Point(16, 114);
		this.lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContent.Name = "lblContent";
		this.lblContent.Size = new System.Drawing.Size(57, 17);
		this.lblContent.TabIndex = 8;
		this.lblContent.Text = "Content";
		this.contentExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.contentExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.contentExpressionButton.Location = new System.Drawing.Point(559, 108);
		this.contentExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.contentExpressionButton.Name = "contentExpressionButton";
		this.contentExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.contentExpressionButton.TabIndex = 10;
		this.contentExpressionButton.UseVisualStyleBackColor = true;
		this.contentExpressionButton.Click += new System.EventHandler(ContentExpressionButton_Click);
		this.txtContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtContent.Location = new System.Drawing.Point(165, 108);
		this.txtContent.Margin = new System.Windows.Forms.Padding(4);
		this.txtContent.MaxLength = 8192;
		this.txtContent.Name = "txtContent";
		this.txtContent.Size = new System.Drawing.Size(384, 22);
		this.txtContent.TabIndex = 9;
		this.txtContent.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtContent.Validating += new System.ComponentModel.CancelEventHandler(TxtContent_Validating);
		this.comboContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboContentType.Location = new System.Drawing.Point(165, 75);
		this.comboContentType.Margin = new System.Windows.Forms.Padding(4);
		this.comboContentType.MaxLength = 1024;
		this.comboContentType.Name = "comboContentType";
		this.comboContentType.Size = new System.Drawing.Size(431, 24);
		this.comboContentType.TabIndex = 7;
		this.comboContentType.TextChanged += new System.EventHandler(ComboContentType_TextChanged);
		this.lblContentType.AutoSize = true;
		this.lblContentType.Location = new System.Drawing.Point(16, 79);
		this.lblContentType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContentType.Name = "lblContentType";
		this.lblContentType.Size = new System.Drawing.Size(93, 17);
		this.lblContentType.TabIndex = 6;
		this.lblContentType.Text = "Content Type";
		this.headersGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.headersGrid.AutoGenerateColumns = false;
		this.headersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.headersGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.headersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.headersGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.valueDataGridViewTextBoxColumn, this.expressionBuilderColumn);
		this.headersGrid.DataSource = this.headerBindingSource;
		this.headersGrid.Location = new System.Drawing.Point(20, 202);
		this.headersGrid.Margin = new System.Windows.Forms.Padding(4);
		this.headersGrid.Name = "headersGrid";
		this.headersGrid.RowHeadersWidth = 51;
		this.headersGrid.Size = new System.Drawing.Size(577, 143);
		this.headersGrid.TabIndex = 14;
		this.headersGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(HeadersGrid_CellClick);
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
		this.nameDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
		this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
		this.valueDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.NullValue = "...";
		this.expressionBuilderColumn.DefaultCellStyle = dataGridViewCellStyle;
		this.expressionBuilderColumn.FillWeight = 20f;
		this.expressionBuilderColumn.HeaderText = "";
		this.expressionBuilderColumn.MinimumWidth = 6;
		this.expressionBuilderColumn.Name = "expressionBuilderColumn";
		this.expressionBuilderColumn.Text = "";
		this.headerBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.Parameter);
		this.lblHeaders.AutoSize = true;
		this.lblHeaders.Location = new System.Drawing.Point(16, 182);
		this.lblHeaders.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHeaders.Name = "lblHeaders";
		this.lblHeaders.Size = new System.Drawing.Size(62, 17);
		this.lblHeaders.TabIndex = 13;
		this.lblHeaders.Text = "Headers";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 395);
		base.Controls.Add(this.headersGrid);
		base.Controls.Add(this.lblHeaders);
		base.Controls.Add(this.lblContent);
		base.Controls.Add(this.contentExpressionButton);
		base.Controls.Add(this.txtContent);
		base.Controls.Add(this.comboContentType);
		base.Controls.Add(this.lblContentType);
		base.Controls.Add(this.webServiceNameExpressionButton);
		base.Controls.Add(this.uriExpressionButton);
		base.Controls.Add(this.txtWebServiceName);
		base.Controls.Add(this.txtURI);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.lblWebServiceName);
		base.Controls.Add(this.lblURI);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 432);
		base.Name = "WebServicesInteractionConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Web Service (POST)";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(WebServicesInteractionConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(WebServicesInteractionConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.headersGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.headerBindingSource).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
