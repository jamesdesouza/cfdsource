using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Forms;

namespace TCX.CFD.Controls;

public class JsonXmlParserWizardPage1 : UserControl, IWizardPage
{
	private JsonXmlParserComponent jsonXmlParserComponent;

	private List<string> usedVariableNames = new List<string>();

	private IContainer components;

	private Label lblTitle;

	private RadioButton rbWebRequest;

	private BindingSource valuesBindingSource;

	private DataGridView valuesGrid;

	private Label lblValues;

	private Label lblValidation;

	private ComboBox comboWebRequestComponents;

	private ComboBox comboTextType;

	private Label lblType;

	private Label lblComponentVariable;

	private GroupBox grpBoxSource;

	private RadioButton rbComponentVariable;

	private Label lblComponent;

	private Label lblInputSourceHelp;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

	private ComboBox comboComponentVariable;

	public string ParserInput { get; private set; } = "";


	public TextTypes TextType => (TextTypes)comboTextType.SelectedItem;

	public string Input
	{
		get
		{
			if (!rbComponentVariable.Checked)
			{
				return (comboWebRequestComponents.Items[comboWebRequestComponents.SelectedIndex] as string) + ".ResponseContent";
			}
			if (comboComponentVariable.SelectedIndex != -1)
			{
				return comboComponentVariable.Items[comboComponentVariable.SelectedIndex] as string;
			}
			return "";
		}
	}

	public JsonXmlParserWizardPage1(JsonXmlParserComponent jsonXmlParserComponent)
	{
		InitializeComponent();
		this.jsonXmlParserComponent = jsonXmlParserComponent;
		comboTextType.Items.AddRange(new object[2]
		{
			TextTypes.JSON,
			TextTypes.XML
		});
		comboTextType.SelectedItem = jsonXmlParserComponent.TextType;
		lblTitle.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblTitle.Text");
		lblType.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblType.Text");
		grpBoxSource.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.grpBoxSource.Text");
		rbWebRequest.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.rbWebRequest.Text");
		rbComponentVariable.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.rbComponentVariable.Text");
		lblComponent.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblComponent.Text");
		lblComponentVariable.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblComponentVariable.Text");
		lblValues.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblValues.Text");
		foreach (string validVariable in ExpressionHelper.GetValidVariables(jsonXmlParserComponent))
		{
			string[] array = validVariable.Split('.');
			string text = array[0];
			IVadActivity componentWithName = GetComponentWithName(text);
			if (componentWithName != null)
			{
				string text2 = array[1];
				if ((!(componentWithName is CRMLookupComponent) || !(text2 == "Result")) && (!(componentWithName is DatabaseAccessComponent) || !(text2 == "ScalarResult")) && (!(componentWithName is ExecuteCSharpCodeComponent) || !(text2 == "ReturnValue")) && (!(componentWithName is ExternalCodeExecutionComponent) || !(text2 == "ReturnValue")) && (!(componentWithName is FileManagementComponent) || !(text2 == "Result")) && (!(componentWithName is SocketClientComponent) || !(text2 == "Response")) && (!(componentWithName is WebInteractionComponent) || !(text2 == "ResponseContent")) && (!(componentWithName is WebServiceRestComponent) || !(text2 == "ResponseContent")) && (!(componentWithName is WebServicesInteractionComponent) || !(text2 == "ResponseContent")) && (!(componentWithName is TcxGetDnPropertyComponent) || !(text2 == "PropertyValue")) && (!(componentWithName is TcxGetGlobalPropertyComponent) || !(text2 == "PropertyValue")))
				{
					continue;
				}
				comboComponentVariable.Items.Add(validVariable);
				if (jsonXmlParserComponent.Input == validVariable)
				{
					comboComponentVariable.SelectedIndex = comboComponentVariable.Items.Count - 1;
				}
				if (text2 == "ResponseContent")
				{
					comboWebRequestComponents.Items.Add(text);
					if (jsonXmlParserComponent.Input == validVariable)
					{
						comboWebRequestComponents.SelectedIndex = comboWebRequestComponents.Items.Count - 1;
					}
				}
			}
			else if (text == "callflow$" || text == "project$")
			{
				comboComponentVariable.Items.Add(validVariable);
				if (jsonXmlParserComponent.Input == validVariable)
				{
					comboComponentVariable.SelectedIndex = comboComponentVariable.Items.Count - 1;
				}
			}
		}
		if (comboWebRequestComponents.Items.Count == 0)
		{
			rbWebRequest.Enabled = false;
			rbComponentVariable.Checked = true;
			return;
		}
		rbWebRequest.Enabled = true;
		if (comboWebRequestComponents.SelectedIndex == -1)
		{
			comboWebRequestComponents.SelectedIndex = 0;
			if (string.IsNullOrEmpty(jsonXmlParserComponent.Input))
			{
				rbWebRequest.Checked = true;
			}
			else
			{
				rbComponentVariable.Checked = true;
			}
		}
		else
		{
			rbWebRequest.Checked = true;
		}
	}

	private IVadActivity GetComponentWithName(string name)
	{
		return jsonXmlParserComponent.GetRootFlow().GetActivityByName(name) as IVadActivity;
	}

	private void FillVariablesForExpression(AbsArgument argument)
	{
		foreach (VariableNameArgument variableName in argument.GetVariableNameList())
		{
			string @string = variableName.GetString();
			if (!usedVariableNames.Contains(@string))
			{
				usedVariableNames.Add(@string);
				valuesBindingSource.List.Add(new Parameter(@string, ""));
			}
		}
	}

	private void FillVariables(WebInteractionComponent referencedComponent)
	{
		List<string> validVariables = ExpressionHelper.GetValidVariables(referencedComponent);
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.URI));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.Content));
		foreach (Parameter header in referencedComponent.Headers)
		{
			FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, header.Value));
		}
	}

	private void FillVariables(WebServiceRestComponent referencedComponent)
	{
		List<string> validVariables = ExpressionHelper.GetValidVariables(referencedComponent);
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.URI));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.Content));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.AuthenticationUserName));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.AuthenticationPassword));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.AuthenticationApiKey));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.AuthenticationOAuth2AccessToken));
		foreach (Parameter header in referencedComponent.Headers)
		{
			FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, header.Value));
		}
	}

	private void FillVariables(WebServicesInteractionComponent referencedComponent)
	{
		List<string> validVariables = ExpressionHelper.GetValidVariables(referencedComponent);
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.URI));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.WebServiceName));
		FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, referencedComponent.Content));
		foreach (Parameter header in referencedComponent.Headers)
		{
			FillVariablesForExpression(AbsArgument.BuildArgument(validVariables, header.Value));
		}
	}

	public void FocusFirstControl()
	{
		comboTextType.Focus();
	}

	private bool ValidateRealResponse(string errorTextJson, string errorTextXml)
	{
		if (TextType == TextTypes.JSON && !ExpressionHelper.IsJSON(ParserInput))
		{
			lblValidation.Text = errorTextJson;
			lblValidation.Visible = true;
			return false;
		}
		if (TextType == TextTypes.XML && !ExpressionHelper.IsXML(ParserInput))
		{
			lblValidation.Text = errorTextXml;
			lblValidation.Visible = true;
			return false;
		}
		return true;
	}

	private bool ValidateActivity(IVadActivity referencedComponent)
	{
		ActivityValidator activityValidator = ((referencedComponent is WebInteractionComponent) ? new WebInteractionComponentValidator() : ((!(referencedComponent is WebServiceRestComponent)) ? ((ActivityValidator)new WebServicesInteractionComponentValidator()) : ((ActivityValidator)new WebServiceRestComponentValidator())));
		if (activityValidator.Validate(null, referencedComponent).HasErrors)
		{
			lblValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.ValidationError.ReferencedComponentHasErrors.Text");
			lblValidation.Visible = true;
			return false;
		}
		return true;
	}

	public bool ValidateBeforeMovingToNext()
	{
		lblValidation.Visible = false;
		if (rbComponentVariable.Checked)
		{
			if (comboComponentVariable.SelectedIndex >= 0)
			{
				ParserInput = string.Empty;
				return true;
			}
			lblValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.ValidationError.ComponentVariableIsMandatory.Text");
			lblValidation.Visible = true;
			return false;
		}
		foreach (Parameter item in valuesBindingSource.List)
		{
			if (string.IsNullOrEmpty(item.Value))
			{
				lblValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.ValidationError.ProvideValues.Text");
				lblValidation.Visible = true;
				return false;
			}
		}
		string name = comboWebRequestComponents.Items[comboWebRequestComponents.SelectedIndex] as string;
		IVadActivity componentWithName = GetComponentWithName(name);
		if (ValidateActivity(componentWithName))
		{
			JsonXmlParserConnectionForm jsonXmlParserConnectionForm = new JsonXmlParserConnectionForm(componentWithName, valuesBindingSource.List);
			if (jsonXmlParserConnectionForm.ShowDialog() == DialogResult.OK)
			{
				ParserInput = jsonXmlParserConnectionForm.Result;
				return ValidateRealResponse(LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.ValidationError.InvalidJSONFromWebResponse.Text"), LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.ValidationError.InvalidXMLFromWebResponse.Text"));
			}
			return false;
		}
		return false;
	}

	private void ComboTextType_SelectedIndexChanged(object sender, EventArgs e)
	{
		lblValidation.Visible = false;
	}

	private void RadioButtonMode_CheckedChanged(object sender, EventArgs e)
	{
		lblComponent.Visible = rbWebRequest.Checked;
		comboWebRequestComponents.Visible = rbWebRequest.Checked;
		lblComponentVariable.Visible = rbComponentVariable.Checked;
		comboComponentVariable.Visible = rbComponentVariable.Checked;
		lblValues.Visible = rbWebRequest.Checked;
		valuesGrid.Visible = rbWebRequest.Checked;
		lblValidation.Visible = false;
		if (rbWebRequest.Checked)
		{
			lblInputSourceHelp.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblInputSourceHelp.WebRequest.Text");
			ComboWebRequestComponents_SelectedIndexChanged(comboWebRequestComponents, EventArgs.Empty);
		}
		else if (rbComponentVariable.Checked)
		{
			lblInputSourceHelp.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage1.lblInputSourceHelp.ComponentVariable.Text");
			ComboComponentVariable_SelectedIndexChanged(comboComponentVariable, EventArgs.Empty);
		}
	}

	private void ComboWebRequestComponents_SelectedIndexChanged(object sender, EventArgs e)
	{
		valuesBindingSource.List.Clear();
		usedVariableNames.Clear();
		string name = comboWebRequestComponents.Items[comboWebRequestComponents.SelectedIndex] as string;
		IVadActivity componentWithName = GetComponentWithName(name);
		if (componentWithName is WebInteractionComponent)
		{
			FillVariables(componentWithName as WebInteractionComponent);
		}
		else if (componentWithName is WebServiceRestComponent)
		{
			FillVariables(componentWithName as WebServiceRestComponent);
		}
		else if (componentWithName is WebServicesInteractionComponent)
		{
			FillVariables(componentWithName as WebServicesInteractionComponent);
		}
		lblValidation.Visible = false;
	}

	private void ComboComponentVariable_SelectedIndexChanged(object sender, EventArgs e)
	{
		lblValidation.Visible = false;
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
		this.lblTitle = new System.Windows.Forms.Label();
		this.rbWebRequest = new System.Windows.Forms.RadioButton();
		this.valuesGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.valuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.lblValues = new System.Windows.Forms.Label();
		this.lblValidation = new System.Windows.Forms.Label();
		this.comboWebRequestComponents = new System.Windows.Forms.ComboBox();
		this.comboTextType = new System.Windows.Forms.ComboBox();
		this.lblType = new System.Windows.Forms.Label();
		this.lblComponentVariable = new System.Windows.Forms.Label();
		this.grpBoxSource = new System.Windows.Forms.GroupBox();
		this.comboComponentVariable = new System.Windows.Forms.ComboBox();
		this.lblInputSourceHelp = new System.Windows.Forms.Label();
		this.rbComponentVariable = new System.Windows.Forms.RadioButton();
		this.lblComponent = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.valuesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.valuesBindingSource).BeginInit();
		this.grpBoxSource.SuspendLayout();
		base.SuspendLayout();
		this.lblTitle.AutoSize = true;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(58, 58);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(160, 26);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Configure Input";
		this.rbWebRequest.AutoSize = true;
		this.rbWebRequest.Location = new System.Drawing.Point(20, 28);
		this.rbWebRequest.Name = "rbWebRequest";
		this.rbWebRequest.Size = new System.Drawing.Size(304, 21);
		this.rbWebRequest.TabIndex = 0;
		this.rbWebRequest.Text = "Invoke web request component from project";
		this.rbWebRequest.UseVisualStyleBackColor = true;
		this.rbWebRequest.CheckedChanged += new System.EventHandler(RadioButtonMode_CheckedChanged);
		this.valuesGrid.AllowUserToAddRows = false;
		this.valuesGrid.AllowUserToDeleteRows = false;
		this.valuesGrid.AllowUserToOrderColumns = true;
		this.valuesGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.valuesGrid.AutoGenerateColumns = false;
		this.valuesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.valuesGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.valuesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.valuesGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.valueDataGridViewTextBoxColumn);
		this.valuesGrid.DataSource = this.valuesBindingSource;
		this.valuesGrid.Location = new System.Drawing.Point(42, 169);
		this.valuesGrid.Margin = new System.Windows.Forms.Padding(4);
		this.valuesGrid.Name = "valuesGrid";
		this.valuesGrid.Size = new System.Drawing.Size(735, 168);
		this.valuesGrid.TabIndex = 8;
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Variable";
		this.nameDataGridViewTextBoxColumn.MaxInputLength = 256;
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.nameDataGridViewTextBoxColumn.ReadOnly = true;
		this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
		this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
		this.valueDataGridViewTextBoxColumn.MaxInputLength = 32768;
		this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
		this.valuesBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.Parameter);
		this.lblValues.AutoSize = true;
		this.lblValues.Location = new System.Drawing.Point(39, 148);
		this.lblValues.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblValues.Name = "lblValues";
		this.lblValues.Size = new System.Drawing.Size(434, 17);
		this.lblValues.TabIndex = 7;
		this.lblValues.Text = "Set the required parameters to execute the web request component";
		this.lblValidation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblValidation.AutoSize = true;
		this.lblValidation.ForeColor = System.Drawing.Color.Red;
		this.lblValidation.Location = new System.Drawing.Point(39, 341);
		this.lblValidation.Name = "lblValidation";
		this.lblValidation.Size = new System.Drawing.Size(239, 17);
		this.lblValidation.TabIndex = 9;
		this.lblValidation.Text = "You must enter a valid JSON or XML";
		this.lblValidation.Visible = false;
		this.comboWebRequestComponents.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboWebRequestComponents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboWebRequestComponents.FormattingEnabled = true;
		this.comboWebRequestComponents.Location = new System.Drawing.Point(124, 112);
		this.comboWebRequestComponents.Name = "comboWebRequestComponents";
		this.comboWebRequestComponents.Size = new System.Drawing.Size(325, 24);
		this.comboWebRequestComponents.TabIndex = 4;
		this.comboWebRequestComponents.SelectedIndexChanged += new System.EventHandler(ComboWebRequestComponents_SelectedIndexChanged);
		this.comboTextType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboTextType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboTextType.FormattingEnabled = true;
		this.comboTextType.Location = new System.Drawing.Point(111, 125);
		this.comboTextType.Margin = new System.Windows.Forms.Padding(4);
		this.comboTextType.Name = "comboTextType";
		this.comboTextType.Size = new System.Drawing.Size(238, 24);
		this.comboTextType.TabIndex = 2;
		this.comboTextType.SelectedIndexChanged += new System.EventHandler(ComboTextType_SelectedIndexChanged);
		this.lblType.AutoSize = true;
		this.lblType.Location = new System.Drawing.Point(63, 128);
		this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblType.Name = "lblType";
		this.lblType.Size = new System.Drawing.Size(40, 17);
		this.lblType.TabIndex = 1;
		this.lblType.Text = "Type";
		this.lblComponentVariable.AutoSize = true;
		this.lblComponentVariable.Location = new System.Drawing.Point(38, 115);
		this.lblComponentVariable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblComponentVariable.Name = "lblComponentVariable";
		this.lblComponentVariable.Size = new System.Drawing.Size(153, 17);
		this.lblComponentVariable.TabIndex = 5;
		this.lblComponentVariable.Text = "Component or Variable";
		this.grpBoxSource.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxSource.Controls.Add(this.comboComponentVariable);
		this.grpBoxSource.Controls.Add(this.lblComponentVariable);
		this.grpBoxSource.Controls.Add(this.lblInputSourceHelp);
		this.grpBoxSource.Controls.Add(this.rbComponentVariable);
		this.grpBoxSource.Controls.Add(this.rbWebRequest);
		this.grpBoxSource.Controls.Add(this.lblValues);
		this.grpBoxSource.Controls.Add(this.valuesGrid);
		this.grpBoxSource.Controls.Add(this.lblValidation);
		this.grpBoxSource.Controls.Add(this.lblComponent);
		this.grpBoxSource.Controls.Add(this.comboWebRequestComponents);
		this.grpBoxSource.Location = new System.Drawing.Point(58, 171);
		this.grpBoxSource.Name = "grpBoxSource";
		this.grpBoxSource.Size = new System.Drawing.Size(821, 375);
		this.grpBoxSource.TabIndex = 3;
		this.grpBoxSource.TabStop = false;
		this.grpBoxSource.Text = "Source";
		this.comboComponentVariable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboComponentVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboComponentVariable.FormattingEnabled = true;
		this.comboComponentVariable.Location = new System.Drawing.Point(198, 112);
		this.comboComponentVariable.Name = "comboComponentVariable";
		this.comboComponentVariable.Size = new System.Drawing.Size(400, 24);
		this.comboComponentVariable.TabIndex = 6;
		this.comboComponentVariable.SelectedIndexChanged += new System.EventHandler(ComboComponentVariable_SelectedIndexChanged);
		this.lblInputSourceHelp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblInputSourceHelp.Location = new System.Drawing.Point(38, 59);
		this.lblInputSourceHelp.Name = "lblInputSourceHelp";
		this.lblInputSourceHelp.Size = new System.Drawing.Size(739, 50);
		this.lblInputSourceHelp.TabIndex = 2;
		this.lblInputSourceHelp.Text = "Call selected component on run-time and map output to variables. Enabled for HTTP Request, Web Service REST, Web Service (POST) used in project.";
		this.rbComponentVariable.AutoSize = true;
		this.rbComponentVariable.Location = new System.Drawing.Point(330, 28);
		this.rbComponentVariable.Name = "rbComponentVariable";
		this.rbComponentVariable.Size = new System.Drawing.Size(243, 21);
		this.rbComponentVariable.TabIndex = 1;
		this.rbComponentVariable.Text = "Use component output or variable";
		this.rbComponentVariable.UseVisualStyleBackColor = true;
		this.rbComponentVariable.CheckedChanged += new System.EventHandler(RadioButtonMode_CheckedChanged);
		this.lblComponent.AutoSize = true;
		this.lblComponent.Location = new System.Drawing.Point(38, 115);
		this.lblComponent.Name = "lblComponent";
		this.lblComponent.Size = new System.Drawing.Size(80, 17);
		this.lblComponent.TabIndex = 3;
		this.lblComponent.Text = "Component";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.grpBoxSource);
		base.Controls.Add(this.comboTextType);
		base.Controls.Add(this.lblType);
		base.Controls.Add(this.lblTitle);
		base.Name = "JsonXmlParserWizardPage1";
		base.Size = new System.Drawing.Size(952, 601);
		((System.ComponentModel.ISupportInitialize)this.valuesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.valuesBindingSource).EndInit();
		this.grpBoxSource.ResumeLayout(false);
		this.grpBoxSource.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
