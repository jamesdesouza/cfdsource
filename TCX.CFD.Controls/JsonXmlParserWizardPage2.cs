using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json.Linq;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Forms;

namespace TCX.CFD.Controls;

public class JsonXmlParserWizardPage2 : UserControl, IWizardPage
{
	private readonly JsonXmlParserComponent jsonXmlParserComponent;

	private readonly JsonXmlParserWizardPage1 page1;

	private List<string> variables;

	private const int RightMargin = 22;

	private JsonXmlParserEmptyMappingControl emptyMappingControl;

	private IContainer components;

	private Label lblTitle;

	private BindingSource valuesBindingSource;

	private TreeView responseTreeView;

	private FlowLayoutPanel responseMappingsPanel;

	private Label lblMappingValidation;

	private GroupBox grpBoxResponseMappings;

	private Label lblPasteSample;

	private TextBox txtPasteSample;

	private Button generateResetTreeViewButton;

	private Button addButton;

	private Button startNowButton;

	public List<ResponseMapping> ResponseMappings
	{
		get
		{
			List<ResponseMapping> list = new List<ResponseMapping>();
			for (int i = 0; i < responseMappingsPanel.Controls.Count; i++)
			{
				if (responseMappingsPanel.Controls[i] is JsonXmlParserMappingControl jsonXmlParserMappingControl)
				{
					list.Add(new ResponseMapping(jsonXmlParserMappingControl.Path, jsonXmlParserMappingControl.Variable));
				}
			}
			return list;
		}
	}

	public JsonXmlParserWizardPage2(JsonXmlParserComponent jsonXmlParserComponent, JsonXmlParserWizardPage1 page1)
	{
		InitializeComponent();
		this.jsonXmlParserComponent = jsonXmlParserComponent;
		this.page1 = page1;
		variables = ExpressionHelper.GetValidVariables(jsonXmlParserComponent, onlyWritableVariables: true);
		startNowButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.startNowButton.Text");
		addButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.addButton.Text");
		grpBoxResponseMappings.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.grpBoxResponseMappings.Text");
		foreach (ResponseMapping responseMapping in jsonXmlParserComponent.ResponseMappings)
		{
			AddResponseMapping(responseMapping);
		}
	}

	public void DeleteResponseMapping(UserControl childControl)
	{
		responseMappingsPanel.Controls.Remove(childControl);
	}

	public void AddResponseMapping(ResponseMapping responseMapping)
	{
		JsonXmlParserMappingControl jsonXmlParserMappingControl = new JsonXmlParserMappingControl(responseMapping, variables);
		jsonXmlParserMappingControl.Width = responseMappingsPanel.Width - 22;
		responseMappingsPanel.Controls.Add(jsonXmlParserMappingControl);
		responseMappingsPanel.Controls.SetChildIndex(jsonXmlParserMappingControl, (emptyMappingControl == null) ? (responseMappingsPanel.Controls.Count - 1) : (responseMappingsPanel.Controls.Count - 2));
	}

	public string CreateVariable()
	{
		CreateVariableForm createVariableForm = new CreateVariableForm(jsonXmlParserComponent);
		if (createVariableForm.ShowDialog() == DialogResult.OK)
		{
			variables = ExpressionHelper.GetValidVariables(jsonXmlParserComponent, onlyWritableVariables: true);
			for (int i = 0; i < responseMappingsPanel.Controls.Count; i++)
			{
				if (responseMappingsPanel.Controls[i] is JsonXmlParserMappingControl jsonXmlParserMappingControl)
				{
					jsonXmlParserMappingControl.AddCreatedVariable(createVariableForm.CreatedVariableName);
				}
			}
			return createVariableForm.CreatedVariableName;
		}
		return "";
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		foreach (UserControl control in responseMappingsPanel.Controls)
		{
			control.Width = responseMappingsPanel.Width - 22;
		}
	}

	private void ResponseTreeView_ItemDrag(object sender, ItemDragEventArgs e)
	{
		if (e.Button == MouseButtons.Left && e.Item is TreeNode treeNode)
		{
			responseTreeView.SelectedNode = treeNode;
			string text = treeNode.FullPath.Replace(".[", "[");
			int num = text.IndexOf('=');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			if (page1.TextType == TextTypes.XML)
			{
				text = "string(/" + text + ")";
			}
			DoDragDrop(text, DragDropEffects.Copy | DragDropEffects.Move);
		}
	}

	private void StartNowButton_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void TxtPasteSample_Enter(object sender, EventArgs e)
	{
		txtPasteSample.SelectAll();
	}

	private void GenerateResetTreeViewButton_Click(object sender, EventArgs e)
	{
		if (responseTreeView.Visible)
		{
			Reset();
		}
		else if (page1.TextType == TextTypes.JSON)
		{
			if (ExpressionHelper.IsJSON(txtPasteSample.Text))
			{
				Generate(txtPasteSample.Text, showResetButton: true);
				return;
			}
			MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.MessageBox.Error.InvalidJSON"), LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtPasteSample.Focus();
		}
		else if (ExpressionHelper.IsXML(txtPasteSample.Text))
		{
			Generate(txtPasteSample.Text, showResetButton: true);
		}
		else
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.MessageBox.Error.InvalidXML"), LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtPasteSample.Focus();
		}
	}

	private void FirstUse()
	{
		lblPasteSample.Visible = true;
		lblPasteSample.TextAlign = ContentAlignment.MiddleCenter;
		txtPasteSample.Visible = false;
		responseTreeView.Visible = false;
		startNowButton.Visible = true;
		generateResetTreeViewButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.generateResetTreeViewButton.Generate.Text");
		generateResetTreeViewButton.Visible = false;
		addButton.Visible = true;
		if (emptyMappingControl != null)
		{
			responseMappingsPanel.Controls.Remove(emptyMappingControl);
		}
		if (responseMappingsPanel.Controls.Count == 0)
		{
			AddResponseMapping(new ResponseMapping());
		}
	}

	private void Reset()
	{
		startNowButton.Visible = false;
		addButton.Visible = true;
		lblPasteSample.TextAlign = ContentAlignment.TopLeft;
		lblPasteSample.Visible = true;
		txtPasteSample.Visible = true;
		txtPasteSample.BringToFront();
		txtPasteSample.Focus();
		generateResetTreeViewButton.Visible = true;
		generateResetTreeViewButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.generateResetTreeViewButton.Generate.Text");
		responseTreeView.Visible = false;
		if (emptyMappingControl != null)
		{
			responseMappingsPanel.Controls.Remove(emptyMappingControl);
		}
	}

	private void Generate(string input, bool showResetButton)
	{
		startNowButton.Visible = false;
		addButton.Visible = false;
		lblPasteSample.Visible = false;
		txtPasteSample.Visible = false;
		generateResetTreeViewButton.Visible = showResetButton;
		generateResetTreeViewButton.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.generateResetTreeViewButton.Reset.Text");
		responseTreeView.Visible = true;
		if (emptyMappingControl == null)
		{
			emptyMappingControl = new JsonXmlParserEmptyMappingControl();
		}
		emptyMappingControl.Width = responseMappingsPanel.Width - 22;
		responseMappingsPanel.Controls.Add(emptyMappingControl);
		LoadTreeView(input);
	}

	private void AddButton_Click(object sender, EventArgs e)
	{
		AddResponseMapping(new ResponseMapping());
	}

	private void AddNode(JToken token, TreeNode inTreeNode)
	{
		if (token == null)
		{
			return;
		}
		if (token is JValue)
		{
			inTreeNode.Text = inTreeNode.Text + "=" + token.ToString();
			return;
		}
		if (token is JObject jObject)
		{
			{
				foreach (JProperty item in jObject.Properties())
				{
					TreeNode treeNode = new TreeNode(item.Name);
					inTreeNode.Nodes.Add(treeNode);
					AddNode(item.Value, treeNode);
				}
				return;
			}
		}
		if (token is JArray jArray)
		{
			for (int i = 0; i < jArray.Count; i++)
			{
				TreeNode treeNode2 = new TreeNode("[" + i + "]");
				inTreeNode.Nodes.Add(treeNode2);
				AddNode(jArray[i], treeNode2);
			}
		}
	}

	private void AddNode(XmlNode xmlNode, TreeNode inTreeNode)
	{
		if (xmlNode.Attributes != null)
		{
			foreach (XmlAttribute attribute in xmlNode.Attributes)
			{
				inTreeNode.Nodes.Add(new TreeNode("@" + attribute.Name + "=" + attribute.Value));
			}
		}
		if (xmlNode.HasChildNodes)
		{
			XmlNodeList childNodes = xmlNode.ChildNodes;
			for (int i = 0; i < childNodes.Count; i++)
			{
				XmlNode xmlNode2 = xmlNode.ChildNodes[i];
				if (xmlNode2.Name == "#text")
				{
					inTreeNode.Text = inTreeNode.Text + "=" + xmlNode2.Value;
					continue;
				}
				TreeNode treeNode = new TreeNode(xmlNode2.Name);
				inTreeNode.Nodes.Add(treeNode);
				AddNode(xmlNode2, treeNode);
			}
		}
		else if (!string.IsNullOrEmpty(xmlNode.Value))
		{
			inTreeNode.Text = inTreeNode.Text + "=" + xmlNode.Value;
		}
	}

	private void LoadTreeView(string input)
	{
		responseTreeView.BeginUpdate();
		try
		{
			responseTreeView.Nodes.Clear();
			if (page1.TextType == TextTypes.JSON)
			{
				responseTreeView.PathSeparator = ".";
				JToken token = JToken.Parse(input);
				TreeNode treeNode = new TreeNode();
				AddNode(token, treeNode);
				foreach (TreeNode node in treeNode.Nodes)
				{
					responseTreeView.Nodes.Add(node);
				}
			}
			else
			{
				responseTreeView.PathSeparator = "/";
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(input);
				TreeNode treeNode2 = new TreeNode(xmlDocument.DocumentElement.Name);
				responseTreeView.Nodes.Add(treeNode2);
				AddNode(xmlDocument.DocumentElement, treeNode2);
			}
			responseTreeView.ExpandAll();
			responseTreeView.Focus();
		}
		finally
		{
			responseTreeView.EndUpdate();
		}
	}

	public void FocusFirstControl()
	{
		if (page1.TextType == TextTypes.JSON)
		{
			lblTitle.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblTitle.JSON.Text");
			lblPasteSample.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblPasteSample.JSON.Text");
		}
		else
		{
			lblTitle.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblTitle.XML.Text");
			lblPasteSample.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblPasteSample.XML.Text");
		}
		txtPasteSample.Text = page1.ParserInput;
		if (string.IsNullOrEmpty(page1.ParserInput))
		{
			FirstUse();
		}
		else
		{
			Generate(page1.ParserInput, showResetButton: false);
		}
	}

	public bool ValidateBeforeMovingToNext()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		lblMappingValidation.Visible = false;
		int num = 0;
		for (int i = 0; i < responseMappingsPanel.Controls.Count; i++)
		{
			if (responseMappingsPanel.Controls[i] is JsonXmlParserMappingControl jsonXmlParserMappingControl)
			{
				if (string.IsNullOrEmpty(jsonXmlParserMappingControl.Variable))
				{
					lblMappingValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblMappingValidation.VariableMissing.Text");
					lblMappingValidation.Visible = true;
					return false;
				}
				if (string.IsNullOrEmpty(jsonXmlParserMappingControl.Path))
				{
					lblMappingValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblMappingValidation.PathMissing.Text");
					lblMappingValidation.Visible = true;
					return false;
				}
				if (list.Contains(jsonXmlParserMappingControl.Path))
				{
					lblMappingValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblMappingValidation.PathDuplicated.Text");
					lblMappingValidation.Visible = true;
					return false;
				}
				if (list2.Contains(jsonXmlParserMappingControl.Variable))
				{
					lblMappingValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblMappingValidation.VariableDuplicated.Text");
					lblMappingValidation.Visible = true;
					return false;
				}
				list.Add(jsonXmlParserMappingControl.Path);
				list2.Add(jsonXmlParserMappingControl.Variable);
				num++;
			}
		}
		if (num == 0)
		{
			lblMappingValidation.Text = LocalizedResourceMgr.GetString("JsonXmlParserWizardPage2.lblMappingValidation.NoMappings.Text");
			lblMappingValidation.Visible = true;
			return false;
		}
		return true;
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
		this.responseTreeView = new System.Windows.Forms.TreeView();
		this.responseMappingsPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.lblMappingValidation = new System.Windows.Forms.Label();
		this.grpBoxResponseMappings = new System.Windows.Forms.GroupBox();
		this.valuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.lblPasteSample = new System.Windows.Forms.Label();
		this.txtPasteSample = new System.Windows.Forms.TextBox();
		this.generateResetTreeViewButton = new System.Windows.Forms.Button();
		this.addButton = new System.Windows.Forms.Button();
		this.startNowButton = new System.Windows.Forms.Button();
		this.grpBoxResponseMappings.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.valuesBindingSource).BeginInit();
		base.SuspendLayout();
		this.lblTitle.AutoSize = true;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(58, 58);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(372, 26);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Map JSON / XML Nodes to Variables";
		this.responseTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.responseTreeView.Location = new System.Drawing.Point(63, 128);
		this.responseTreeView.Name = "responseTreeView";
		this.responseTreeView.PathSeparator = ".";
		this.responseTreeView.Size = new System.Drawing.Size(400, 401);
		this.responseTreeView.TabIndex = 4;
		this.responseTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(ResponseTreeView_ItemDrag);
		this.responseMappingsPanel.AutoScroll = true;
		this.responseMappingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.responseMappingsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.responseMappingsPanel.Location = new System.Drawing.Point(3, 18);
		this.responseMappingsPanel.Name = "responseMappingsPanel";
		this.responseMappingsPanel.Size = new System.Drawing.Size(374, 391);
		this.responseMappingsPanel.TabIndex = 0;
		this.responseMappingsPanel.WrapContents = false;
		this.lblMappingValidation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblMappingValidation.ForeColor = System.Drawing.Color.Red;
		this.lblMappingValidation.Location = new System.Drawing.Point(503, 538);
		this.lblMappingValidation.Name = "lblMappingValidation";
		this.lblMappingValidation.Size = new System.Drawing.Size(271, 50);
		this.lblMappingValidation.TabIndex = 8;
		this.lblMappingValidation.Text = "You must select a variable for every response mapping";
		this.lblMappingValidation.Visible = false;
		this.grpBoxResponseMappings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxResponseMappings.Controls.Add(this.responseMappingsPanel);
		this.grpBoxResponseMappings.Location = new System.Drawing.Point(503, 120);
		this.grpBoxResponseMappings.Name = "grpBoxResponseMappings";
		this.grpBoxResponseMappings.Size = new System.Drawing.Size(380, 412);
		this.grpBoxResponseMappings.TabIndex = 6;
		this.grpBoxResponseMappings.TabStop = false;
		this.grpBoxResponseMappings.Text = "Response Mappings";
		this.valuesBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.Parameter);
		this.lblPasteSample.Location = new System.Drawing.Point(60, 120);
		this.lblPasteSample.Name = "lblPasteSample";
		this.lblPasteSample.Size = new System.Drawing.Size(411, 409);
		this.lblPasteSample.TabIndex = 1;
		this.lblPasteSample.Text = "Simplify response mapping by pasting a sample JSON response";
		this.lblPasteSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPasteSample.AcceptsReturn = true;
		this.txtPasteSample.AcceptsTab = true;
		this.txtPasteSample.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.txtPasteSample.Location = new System.Drawing.Point(63, 141);
		this.txtPasteSample.Multiline = true;
		this.txtPasteSample.Name = "txtPasteSample";
		this.txtPasteSample.Size = new System.Drawing.Size(400, 388);
		this.txtPasteSample.TabIndex = 3;
		this.txtPasteSample.Enter += new System.EventHandler(TxtPasteSample_Enter);
		this.generateResetTreeViewButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.generateResetTreeViewButton.Location = new System.Drawing.Point(363, 535);
		this.generateResetTreeViewButton.Name = "generateResetTreeViewButton";
		this.generateResetTreeViewButton.Size = new System.Drawing.Size(100, 28);
		this.generateResetTreeViewButton.TabIndex = 5;
		this.generateResetTreeViewButton.Text = "Generate";
		this.generateResetTreeViewButton.UseVisualStyleBackColor = true;
		this.generateResetTreeViewButton.Click += new System.EventHandler(GenerateResetTreeViewButton_Click);
		this.addButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.addButton.Location = new System.Drawing.Point(780, 535);
		this.addButton.Name = "addButton";
		this.addButton.Size = new System.Drawing.Size(100, 28);
		this.addButton.TabIndex = 7;
		this.addButton.Text = "Add";
		this.addButton.UseVisualStyleBackColor = true;
		this.addButton.Click += new System.EventHandler(AddButton_Click);
		this.startNowButton.Location = new System.Drawing.Point(215, 347);
		this.startNowButton.Name = "startNowButton";
		this.startNowButton.Size = new System.Drawing.Size(100, 28);
		this.startNowButton.TabIndex = 2;
		this.startNowButton.Text = "Start Now";
		this.startNowButton.UseVisualStyleBackColor = true;
		this.startNowButton.Click += new System.EventHandler(StartNowButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.startNowButton);
		base.Controls.Add(this.addButton);
		base.Controls.Add(this.generateResetTreeViewButton);
		base.Controls.Add(this.grpBoxResponseMappings);
		base.Controls.Add(this.lblMappingValidation);
		base.Controls.Add(this.responseTreeView);
		base.Controls.Add(this.lblTitle);
		base.Controls.Add(this.lblPasteSample);
		base.Controls.Add(this.txtPasteSample);
		base.Name = "JsonXmlParserWizardPage2";
		base.Size = new System.Drawing.Size(952, 601);
		this.grpBoxResponseMappings.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.valuesBindingSource).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
