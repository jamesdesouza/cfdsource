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

public class WebInteractionConfigurationForm : Form
{
	private readonly WebInteractionComponent webInteractionComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblURI;

	private Label lblRequestType;

	private ComboBox comboRequestType;

	private MaskedTextBox txtTimeout;

	private Label lblTimeout;

	private Button cancelButton;

	private Button okButton;

	private TextBox txtURI;

	private ErrorProvider errorProvider;

	private Button uriExpressionButton;

	private Label lblContentType;

	private Label lblContent;

	private Button contentExpressionButton;

	private TextBox txtContent;

	private ComboBox comboContentType;

	private DataGridView headersGrid;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

	private DataGridViewButtonColumn expressionBuilderColumn;

	private BindingSource headerBindingSource;

	private Label lblHeaders;

	public string URI => txtURI.Text;

	public HttpRequestTypes RequestType => (HttpRequestTypes)comboRequestType.SelectedItem;

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
			return Settings.Default.WebInteractionTemplateTimeout;
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
		if (!comboContentType.Items.Contains(webInteractionComponent.ContentType))
		{
			comboContentType.Items.Add(webInteractionComponent.ContentType);
		}
	}

	public WebInteractionConfigurationForm(WebInteractionComponent webInteractionComponent)
	{
		InitializeComponent();
		this.webInteractionComponent = webInteractionComponent;
		validVariables = ExpressionHelper.GetValidVariables(webInteractionComponent);
		comboRequestType.Items.AddRange(new object[7]
		{
			HttpRequestTypes.DELETE,
			HttpRequestTypes.GET,
			HttpRequestTypes.HEAD,
			HttpRequestTypes.OPTIONS,
			HttpRequestTypes.POST,
			HttpRequestTypes.PUT,
			HttpRequestTypes.TRACE
		});
		FillComboContentType();
		txtURI.Text = webInteractionComponent.URI;
		comboRequestType.SelectedItem = webInteractionComponent.HttpRequestType;
		comboContentType.SelectedItem = webInteractionComponent.ContentType;
		txtContent.Text = webInteractionComponent.Content;
		txtTimeout.Text = webInteractionComponent.Timeout.ToString();
		TxtURI_Validating(txtURI, new CancelEventArgs());
		TxtContent_Validating(txtContent, new CancelEventArgs());
		TxtTimeout_Validating(txtTimeout, new CancelEventArgs());
		foreach (Parameter header in webInteractionComponent.Headers)
		{
			headerBindingSource.List.Add(header);
		}
		Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.Title");
		lblURI.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.lblURI.Text");
		lblRequestType.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.lblRequestType.Text");
		lblContentType.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.lblContentType.Text");
		lblContent.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.lblContent.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.lblTimeout.Text");
		lblHeaders.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.lblHeaders.Text");
		okButton.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.cancelButton.Text");
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
			errorProvider.SetError(uriExpressionButton, LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.Error.UriIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtURI.Text);
		errorProvider.SetError(uriExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.Error.UriIsInvalid"));
	}

	private void UriExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webInteractionComponent);
		expressionEditorForm.Expression = txtURI.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtURI.Text = expressionEditorForm.Expression;
			TxtURI_Validating(txtURI, new CancelEventArgs());
		}
	}

	private void ComboContentType_TextChanged(object sender, EventArgs e)
	{
		TxtContent_Validating(txtContent, new CancelEventArgs());
	}

	private void TxtContent_Validating(object sender, CancelEventArgs e)
	{
		if ((string.IsNullOrEmpty(txtContent.Text) && !string.IsNullOrEmpty(comboContentType.Text)) || (!string.IsNullOrEmpty(txtContent.Text) && string.IsNullOrEmpty(comboContentType.Text)))
		{
			errorProvider.SetError(contentExpressionButton, LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.Error.ContentAndContentTypeRequired"));
			return;
		}
		if (string.IsNullOrEmpty(txtContent.Text))
		{
			errorProvider.SetError(contentExpressionButton, string.Empty);
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtContent.Text);
		errorProvider.SetError(contentExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.Error.ContentIsInvalid"));
	}

	private void ContentExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webInteractionComponent);
		expressionEditorForm.Expression = txtContent.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtContent.Text = expressionEditorForm.Expression;
			TxtContent_Validating(txtContent, new CancelEventArgs());
		}
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.Error.TimeoutIsMandatory") : string.Empty);
	}

	private void HeadersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2 && e.RowIndex >= 0)
		{
			ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webInteractionComponent);
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
					MessageBox.Show(LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					headerBindingSource.Position = headerBindingSource.IndexOf(item);
					return;
				}
				if (list.Contains(item.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("WebInteractionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					headerBindingSource.Position = headerBindingSource.IndexOf(item);
					return;
				}
				list.Add(item.Name);
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void WebInteractionConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		webInteractionComponent.ShowHelp();
	}

	private void WebInteractionConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		webInteractionComponent.ShowHelp();
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
		this.lblRequestType = new System.Windows.Forms.Label();
		this.comboRequestType = new System.Windows.Forms.ComboBox();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.txtURI = new System.Windows.Forms.TextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.uriExpressionButton = new System.Windows.Forms.Button();
		this.lblContentType = new System.Windows.Forms.Label();
		this.comboContentType = new System.Windows.Forms.ComboBox();
		this.contentExpressionButton = new System.Windows.Forms.Button();
		this.txtContent = new System.Windows.Forms.TextBox();
		this.lblContent = new System.Windows.Forms.Label();
		this.headerBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.headersGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionBuilderColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		this.lblHeaders = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.headerBindingSource).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.headersGrid).BeginInit();
		base.SuspendLayout();
		this.lblURI.AutoSize = true;
		this.lblURI.Location = new System.Drawing.Point(16, 11);
		this.lblURI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblURI.Name = "lblURI";
		this.lblURI.Size = new System.Drawing.Size(31, 17);
		this.lblURI.TabIndex = 0;
		this.lblURI.Text = "URI";
		this.lblRequestType.AutoSize = true;
		this.lblRequestType.Location = new System.Drawing.Point(16, 46);
		this.lblRequestType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblRequestType.Name = "lblRequestType";
		this.lblRequestType.Size = new System.Drawing.Size(97, 17);
		this.lblRequestType.TabIndex = 3;
		this.lblRequestType.Text = "Request Type";
		this.comboRequestType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboRequestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboRequestType.FormattingEnabled = true;
		this.comboRequestType.Location = new System.Drawing.Point(125, 42);
		this.comboRequestType.Margin = new System.Windows.Forms.Padding(4);
		this.comboRequestType.Name = "comboRequestType";
		this.comboRequestType.Size = new System.Drawing.Size(471, 24);
		this.comboRequestType.TabIndex = 4;
		this.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(125, 142);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(471, 22);
		this.txtTimeout.TabIndex = 11;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(16, 145);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 10;
		this.lblTimeout.Text = "Timeout (secs)";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 348);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 15;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 348);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 14;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.txtURI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtURI.Location = new System.Drawing.Point(125, 7);
		this.txtURI.Margin = new System.Windows.Forms.Padding(4);
		this.txtURI.MaxLength = 8192;
		this.txtURI.Name = "txtURI";
		this.txtURI.Size = new System.Drawing.Size(424, 22);
		this.txtURI.TabIndex = 1;
		this.txtURI.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtURI.Validating += new System.ComponentModel.CancelEventHandler(TxtURI_Validating);
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
		this.lblContentType.AutoSize = true;
		this.lblContentType.Location = new System.Drawing.Point(16, 79);
		this.lblContentType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContentType.Name = "lblContentType";
		this.lblContentType.Size = new System.Drawing.Size(93, 17);
		this.lblContentType.TabIndex = 5;
		this.lblContentType.Text = "Content Type";
		this.comboContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboContentType.Location = new System.Drawing.Point(125, 75);
		this.comboContentType.Margin = new System.Windows.Forms.Padding(4);
		this.comboContentType.MaxLength = 1024;
		this.comboContentType.Name = "comboContentType";
		this.comboContentType.Size = new System.Drawing.Size(471, 24);
		this.comboContentType.TabIndex = 6;
		this.comboContentType.TextChanged += new System.EventHandler(ComboContentType_TextChanged);
		this.contentExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.contentExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.contentExpressionButton.Location = new System.Drawing.Point(559, 106);
		this.contentExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.contentExpressionButton.Name = "contentExpressionButton";
		this.contentExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.contentExpressionButton.TabIndex = 9;
		this.contentExpressionButton.UseVisualStyleBackColor = true;
		this.contentExpressionButton.Click += new System.EventHandler(ContentExpressionButton_Click);
		this.txtContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtContent.Location = new System.Drawing.Point(125, 108);
		this.txtContent.Margin = new System.Windows.Forms.Padding(4);
		this.txtContent.MaxLength = 8192;
		this.txtContent.Name = "txtContent";
		this.txtContent.Size = new System.Drawing.Size(424, 22);
		this.txtContent.TabIndex = 8;
		this.txtContent.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtContent.Validating += new System.ComponentModel.CancelEventHandler(TxtContent_Validating);
		this.lblContent.AutoSize = true;
		this.lblContent.Location = new System.Drawing.Point(16, 112);
		this.lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContent.Name = "lblContent";
		this.lblContent.Size = new System.Drawing.Size(57, 17);
		this.lblContent.TabIndex = 7;
		this.lblContent.Text = "Content";
		this.headerBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.Parameter);
		this.headersGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.headersGrid.AutoGenerateColumns = false;
		this.headersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.headersGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.headersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.headersGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.valueDataGridViewTextBoxColumn, this.expressionBuilderColumn);
		this.headersGrid.DataSource = this.headerBindingSource;
		this.headersGrid.Location = new System.Drawing.Point(20, 206);
		this.headersGrid.Margin = new System.Windows.Forms.Padding(4);
		this.headersGrid.Name = "headersGrid";
		this.headersGrid.RowHeadersWidth = 51;
		this.headersGrid.Size = new System.Drawing.Size(577, 135);
		this.headersGrid.TabIndex = 13;
		this.headersGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(HeadersGrid_CellClick);
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
		this.nameDataGridViewTextBoxColumn.MaxInputLength = 256;
		this.nameDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
		this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
		this.valueDataGridViewTextBoxColumn.MaxInputLength = 1024;
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
		this.lblHeaders.AutoSize = true;
		this.lblHeaders.Location = new System.Drawing.Point(16, 186);
		this.lblHeaders.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHeaders.Name = "lblHeaders";
		this.lblHeaders.Size = new System.Drawing.Size(62, 17);
		this.lblHeaders.TabIndex = 12;
		this.lblHeaders.Text = "Headers";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 391);
		base.Controls.Add(this.headersGrid);
		base.Controls.Add(this.lblHeaders);
		base.Controls.Add(this.lblContent);
		base.Controls.Add(this.contentExpressionButton);
		base.Controls.Add(this.txtContent);
		base.Controls.Add(this.comboContentType);
		base.Controls.Add(this.lblContentType);
		base.Controls.Add(this.uriExpressionButton);
		base.Controls.Add(this.txtURI);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.comboRequestType);
		base.Controls.Add(this.lblRequestType);
		base.Controls.Add(this.lblURI);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 429);
		base.Name = "WebInteractionConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "HTTP Requests";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(WebInteractionConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(WebInteractionConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.headerBindingSource).EndInit();
		((System.ComponentModel.ISupportInitialize)this.headersGrid).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
