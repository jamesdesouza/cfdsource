using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class ExpressionEditorForm : Form
{
	private const int ExpressionMargin = 25;

	private readonly IVadActivity vadActivity;

	private readonly List<string> validVariables;

	private readonly List<string> validVariablesWithConstants;

	private AbsArgument argument;

	private bool hasChanges;

	private bool savingChanges;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private GroupBox grpBoxExpressionElements;

	private TreeView variablesTreeView;

	private TreeView inbuiltFunctionsTreeView;

	private GroupBox grpBoxExpression;

	private FlowLayoutPanel expressionPanel;

	private TreeView constantValuesTreeView;

	private TabControl expressionElementsTabControl;

	private TabPage tabPageInbuiltFunctions;

	private TabPage tabPageVariables;

	private TabPage tabPageConstantValues;

	private RichTextBox txtFunctionHelp;

	private RichTextBox txtVariablesHelp;

	private Panel functionHelpPanel;

	private Panel variableHelpPanel;

	public string Expression
	{
		get
		{
			return argument.GetString();
		}
		set
		{
			argument = AbsArgument.BuildArgument(validVariablesWithConstants, value);
			SetArgumentControl(updateConstantValues: true);
			hasChanges = false;
		}
	}

	private void FillInbuiltFunctions()
	{
		TreeNode treeNode = new TreeNode("Boolean");
		string[] booleanFunctionList = ExpressionHelper.BooleanFunctionList;
		foreach (string text in booleanFunctionList)
		{
			treeNode.Nodes.Add(new TreeNode(text.Replace("_", " "))
			{
				Tag = AbsFunction.BuildFunction(null, text, new List<string>())
			});
		}
		TreeNode treeNode2 = new TreeNode("String");
		booleanFunctionList = ExpressionHelper.StringFunctionList;
		foreach (string text2 in booleanFunctionList)
		{
			treeNode2.Nodes.Add(new TreeNode(text2.Replace("_", " "))
			{
				Tag = AbsFunction.BuildFunction(null, text2, new List<string>())
			});
		}
		TreeNode treeNode3 = new TreeNode("Date Time");
		booleanFunctionList = ExpressionHelper.DateTimeFunctionList;
		foreach (string text3 in booleanFunctionList)
		{
			treeNode3.Nodes.Add(new TreeNode(text3.Replace("_", " "))
			{
				Tag = AbsFunction.BuildFunction(null, text3, new List<string>())
			});
		}
		TreeNode treeNode4 = new TreeNode("Number");
		booleanFunctionList = ExpressionHelper.NumberFunctionList;
		foreach (string text4 in booleanFunctionList)
		{
			treeNode4.Nodes.Add(new TreeNode(text4.Replace("_", " "))
			{
				Tag = AbsFunction.BuildFunction(null, text4, new List<string>())
			});
		}
		TreeNode treeNode5 = new TreeNode("Object");
		booleanFunctionList = ExpressionHelper.AnyFunctionList;
		foreach (string text5 in booleanFunctionList)
		{
			treeNode5.Nodes.Add(new TreeNode(text5.Replace("_", " "))
			{
				Tag = AbsFunction.BuildFunction(null, text5, new List<string>())
			});
		}
		inbuiltFunctionsTreeView.Nodes.Add(treeNode);
		inbuiltFunctionsTreeView.Nodes.Add(treeNode2);
		inbuiltFunctionsTreeView.Nodes.Add(treeNode3);
		inbuiltFunctionsTreeView.Nodes.Add(treeNode4);
		inbuiltFunctionsTreeView.Nodes.Add(treeNode5);
	}

	private TreeNode FindTreeNode(string objectName)
	{
		foreach (TreeNode node in variablesTreeView.Nodes)
		{
			if (node.Text == objectName)
			{
				return node;
			}
		}
		return null;
	}

	private void FillVariables()
	{
		foreach (string validVariable in validVariables)
		{
			string[] array = validVariable.Split('.');
			if (array.Length == 2)
			{
				string objectName = array[0];
				string text = array[1];
				TreeNode treeNode = FindTreeNode(objectName);
				if (treeNode == null)
				{
					treeNode = new TreeNode(objectName);
					variablesTreeView.Nodes.Add(treeNode);
				}
				treeNode.Nodes.Add(new TreeNode(text));
			}
		}
		variablesTreeView.Sort();
		variablesTreeView.ExpandAll();
	}

	private void LoadVariables(TreeView treeview, string name, List<Variable> properties, bool onlyPublicVariables)
	{
		TreeNode treeNode = new TreeNode(name);
		foreach (Variable property in properties)
		{
			if (property.Scope == VariableScopes.Public || !onlyPublicVariables)
			{
				treeNode.Nodes.Add(new TreeNode(property.Name));
			}
		}
		if (treeNode.Nodes.Count > 0)
		{
			treeview.Nodes.Add(treeNode);
		}
	}

	private bool IsComponentUsed(ActivityCollection activityCollection, Type componentType)
	{
		foreach (Activity item in activityCollection)
		{
			if (item.Enabled && item.GetType() == componentType)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsComponentUsed(Type componentType)
	{
		ActivityCollection activities = (vadActivity as Activity).Parent.Activities;
		Activity activity = activities[0];
		while (activity.Parent != null && activity.Parent.Parent != null)
		{
			if (IsComponentUsed(activity.Parent.Parent.Activities, componentType))
			{
				return true;
			}
			activity = activity.Parent.Parent.Activities[0];
		}
		return IsComponentUsed(activities, componentType);
	}

	private void FillConstantValues(FileObject fileObject)
	{
		TreeNode treeNode = new TreeNode("Constant Values");
		treeNode.Nodes.Add(new TreeNode("String"));
		treeNode.Nodes.Add(new TreeNode("Multiline String"));
		treeNode.Nodes.Add(new TreeNode("Single Character"));
		treeNode.Nodes.Add(new TreeNode("Boolean"));
		treeNode.Nodes.Add(new TreeNode("Integer Number"));
		treeNode.Nodes.Add(new TreeNode("Floating Point Number"));
		constantValuesTreeView.Nodes.Add(treeNode);
		ProjectObject projectObject = fileObject.GetProjectObject();
		if (projectObject != null)
		{
			if (IsComponentUsed(typeof(RecordComponent)))
			{
				LoadVariables(constantValuesTreeView, "RecordResult", projectObject.RecordResultConstantList, onlyPublicVariables: false);
			}
			if (IsComponentUsed(typeof(MenuComponent)))
			{
				LoadVariables(constantValuesTreeView, "MenuResult", projectObject.MenuResultConstantList, onlyPublicVariables: false);
			}
			if (IsComponentUsed(typeof(UserInputComponent)) || IsComponentUsed(typeof(AuthenticationComponent)) || IsComponentUsed(typeof(CreditCardComponent)))
			{
				LoadVariables(constantValuesTreeView, "UserInputResult", projectObject.UserInputResultConstantList, onlyPublicVariables: false);
			}
			if (IsComponentUsed(typeof(VoiceInputComponent)))
			{
				LoadVariables(constantValuesTreeView, "VoiceInputResult", projectObject.VoiceInputResultConstantList, onlyPublicVariables: false);
			}
		}
		constantValuesTreeView.ExpandAll();
	}

	private void ExpressionElementsTabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (expressionElementsTabControl.SelectedIndex == 0)
		{
			inbuiltFunctionsTreeView.Focus();
		}
		else if (expressionElementsTabControl.SelectedIndex == 1)
		{
			variablesTreeView.Focus();
		}
	}

	private void InbuiltFunctionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		txtFunctionHelp.Clear();
		object obj = inbuiltFunctionsTreeView.SelectedNode?.Tag;
		if (obj is AbsFunction)
		{
			AbsFunction absFunction = obj as AbsFunction;
			txtFunctionHelp.SelectionFont = new Font(txtFunctionHelp.Font, FontStyle.Bold);
			txtFunctionHelp.AppendText(absFunction.Name + Environment.NewLine + Environment.NewLine);
			txtFunctionHelp.SelectionFont = new Font(txtFunctionHelp.Font, FontStyle.Regular);
			txtFunctionHelp.AppendText(absFunction.GetHelpText());
		}
	}

	private void VariablesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		txtVariablesHelp.Clear();
		string text = variablesTreeView.SelectedNode?.FullPath;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (text.StartsWith("project$.") || text.StartsWith("callflow$.") || text.StartsWith("session."))
		{
			txtVariablesHelp.SelectionFont = new Font(txtVariablesHelp.Font, FontStyle.Bold);
			txtVariablesHelp.AppendText(string.Format(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Title"), text) + Environment.NewLine + Environment.NewLine);
			txtVariablesHelp.SelectionFont = new Font(txtVariablesHelp.Font, FontStyle.Regular);
			if (text.StartsWith("project$."))
			{
				Variable variable = ExpressionHelper.GetVariable(vadActivity, text);
				if (string.IsNullOrEmpty(variable?.HelpText))
				{
					txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.UserDefinedProjectVariable"));
				}
				else
				{
					txtVariablesHelp.AppendText(variable?.HelpText);
				}
				return;
			}
			if (text.StartsWith("callflow$."))
			{
				Variable variable2 = ExpressionHelper.GetVariable(vadActivity, text);
				if (string.IsNullOrEmpty(variable2?.HelpText))
				{
					txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.UserDefinedCallflowVariable"));
				}
				else
				{
					txtVariablesHelp.AppendText(variable2?.HelpText);
				}
				return;
			}
			switch (text)
			{
			case "session.ani":
				txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Session.Ani"));
				break;
			case "session.callid":
				txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Session.Callid"));
				break;
			case "session.dnis":
				txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Session.Dnis"));
				break;
			case "session.did":
				txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Session.Did"));
				break;
			case "session.audioFolder":
				txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Session.AudioFolder"));
				break;
			case "session.transferingExtension":
				txtVariablesHelp.AppendText(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Session.TransferingExtension"));
				break;
			}
		}
		else
		{
			Variable variable3 = ExpressionHelper.GetVariable(vadActivity, text);
			if (!string.IsNullOrEmpty(variable3?.HelpText))
			{
				txtVariablesHelp.SelectionFont = new Font(txtVariablesHelp.Font, FontStyle.Bold);
				txtVariablesHelp.AppendText(string.Format(LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableHelp.Title"), text) + Environment.NewLine + Environment.NewLine);
				txtVariablesHelp.SelectionFont = new Font(txtVariablesHelp.Font, FontStyle.Regular);
				txtVariablesHelp.AppendText(variable3.HelpText);
			}
		}
	}

	private void InbuiltFunctionsTreeView_ItemDrag(object sender, ItemDragEventArgs e)
	{
		if (e.Button == MouseButtons.Left && e.Item is TreeNode { Level: 1 } treeNode)
		{
			inbuiltFunctionsTreeView.SelectedNode = treeNode;
			DoDragDrop(treeNode.FullPath, DragDropEffects.Copy | DragDropEffects.Move);
		}
	}

	private void VariableSelectionTreeView_ItemDrag(object sender, ItemDragEventArgs e)
	{
		if (e.Button == MouseButtons.Left && e.Item is TreeNode { Level: 1 } treeNode)
		{
			variablesTreeView.SelectedNode = treeNode;
			DoDragDrop("VariableName." + treeNode.FullPath, DragDropEffects.Copy | DragDropEffects.Move);
		}
	}

	private void ConstantValuesTreeView_ItemDrag(object sender, ItemDragEventArgs e)
	{
		if (e.Button == MouseButtons.Left && e.Item is TreeNode { Level: 1 } treeNode)
		{
			constantValuesTreeView.SelectedNode = treeNode;
			string data = (treeNode.FullPath.StartsWith("Constant Values") ? treeNode.FullPath : ("VariableName." + treeNode.FullPath));
			DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move);
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (argument.IsSafeExpression() || string.IsNullOrWhiteSpace(argument.GetString()) || MessageBox.Show(LocalizedResourceMgr.GetString("ExpressionEditorForm.MessageBox.Error.InvalidExpression"), LocalizedResourceMgr.GetString("ExpressionEditorForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
		{
			savingChanges = true;
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	public ExpressionEditorForm(IVadActivity vadActivity)
	{
		InitializeComponent();
		this.vadActivity = vadActivity;
		validVariables = ExpressionHelper.GetValidVariables(vadActivity, onlyWritableVariables: false, includeConstants: false);
		validVariablesWithConstants = ExpressionHelper.GetValidVariables(vadActivity);
		FileObject fileObject = vadActivity.GetRootFlow()?.FileObject;
		FillInbuiltFunctions();
		FillVariables();
		FillConstantValues(fileObject);
		Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.Title");
		grpBoxExpression.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.grpBoxExpression.Text");
		grpBoxExpressionElements.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.grpBoxExpressionElements.Text");
		tabPageInbuiltFunctions.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.tabPageInbuiltFunctions.Text");
		tabPageVariables.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.tabPageVariables.Text");
		tabPageConstantValues.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.tabPageConstantValues.Text");
		okButton.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("ExpressionEditorForm.cancelButton.Text");
	}

	private void ExpressionEditorForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (!savingChanges && hasChanges && MessageBox.Show(LocalizedResourceMgr.GetString("ExpressionEditorForm.MessageBox.Question.ConfirmCancel"), LocalizedResourceMgr.GetString("ExpressionEditorForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			e.Cancel = true;
		}
	}

	private void ExpressionEditorForm_Resize(object sender, EventArgs e)
	{
		foreach (Control control in expressionPanel.Controls)
		{
			control.Width = expressionPanel.Width - 25;
		}
	}

	private void SetArgumentControl(bool updateConstantValues)
	{
		ExpressionEditorRowControl expressionEditorRowControl = argument.GetExpressionEditorRowControl(validVariablesWithConstants);
		expressionEditorRowControl.Updated += RowControl_Updated;
		expressionEditorRowControl.Deleted += RowControl_Deleted;
		expressionEditorRowControl.HeightUpdated += RowControl_HeightUpdated;
		expressionEditorRowControl.HideDragHandle();
		if (updateConstantValues)
		{
			expressionEditorRowControl.UpdateConstantValues();
		}
		expressionEditorRowControl.Width = expressionPanel.Width - 25;
		expressionPanel.Controls.Clear();
		expressionPanel.Controls.Add(expressionEditorRowControl);
		ResetScrollbar();
	}

	private void ResetScrollbar()
	{
		expressionPanel.AutoScroll = false;
		expressionPanel.AutoScroll = true;
	}

	private void RowControl_Updated(int controlIndex, AbsArgument updatedArgument, bool redraw)
	{
		hasChanges = true;
		argument = updatedArgument;
		if (redraw)
		{
			SetArgumentControl(updateConstantValues: false);
		}
	}

	private void RowControl_Deleted(int controlIndex)
	{
		RowControl_Updated(controlIndex, new DotNetExpressionArgument(""), redraw: true);
	}

	private void RowControl_HeightUpdated(int controlIndex, int deltaHeight)
	{
		ResetScrollbar();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-prompts-expressions/#h.v17ic2wxant9");
	}

	private void ExpressionEditorForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void ExpressionEditorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		ShowHelp();
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.grpBoxExpressionElements = new System.Windows.Forms.GroupBox();
		this.expressionElementsTabControl = new System.Windows.Forms.TabControl();
		this.tabPageInbuiltFunctions = new System.Windows.Forms.TabPage();
		this.inbuiltFunctionsTreeView = new System.Windows.Forms.TreeView();
		this.functionHelpPanel = new System.Windows.Forms.Panel();
		this.txtFunctionHelp = new System.Windows.Forms.RichTextBox();
		this.tabPageVariables = new System.Windows.Forms.TabPage();
		this.variablesTreeView = new System.Windows.Forms.TreeView();
		this.variableHelpPanel = new System.Windows.Forms.Panel();
		this.txtVariablesHelp = new System.Windows.Forms.RichTextBox();
		this.tabPageConstantValues = new System.Windows.Forms.TabPage();
		this.constantValuesTreeView = new System.Windows.Forms.TreeView();
		this.grpBoxExpression = new System.Windows.Forms.GroupBox();
		this.expressionPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.grpBoxExpressionElements.SuspendLayout();
		this.expressionElementsTabControl.SuspendLayout();
		this.tabPageInbuiltFunctions.SuspendLayout();
		this.functionHelpPanel.SuspendLayout();
		this.tabPageVariables.SuspendLayout();
		this.variableHelpPanel.SuspendLayout();
		this.tabPageConstantValues.SuspendLayout();
		this.grpBoxExpression.SuspendLayout();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(1126, 796);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 7;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(1018, 796);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 6;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.grpBoxExpressionElements.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxExpressionElements.Controls.Add(this.expressionElementsTabControl);
		this.grpBoxExpressionElements.Location = new System.Drawing.Point(642, 13);
		this.grpBoxExpressionElements.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxExpressionElements.Name = "grpBoxExpressionElements";
		this.grpBoxExpressionElements.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxExpressionElements.Size = new System.Drawing.Size(584, 775);
		this.grpBoxExpressionElements.TabIndex = 4;
		this.grpBoxExpressionElements.TabStop = false;
		this.grpBoxExpressionElements.Text = "Expression Elements";
		this.expressionElementsTabControl.Controls.Add(this.tabPageInbuiltFunctions);
		this.expressionElementsTabControl.Controls.Add(this.tabPageVariables);
		this.expressionElementsTabControl.Controls.Add(this.tabPageConstantValues);
		this.expressionElementsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.expressionElementsTabControl.Location = new System.Drawing.Point(4, 19);
		this.expressionElementsTabControl.Name = "expressionElementsTabControl";
		this.expressionElementsTabControl.SelectedIndex = 0;
		this.expressionElementsTabControl.Size = new System.Drawing.Size(576, 752);
		this.expressionElementsTabControl.TabIndex = 1;
		this.expressionElementsTabControl.SelectedIndexChanged += new System.EventHandler(ExpressionElementsTabControl_SelectedIndexChanged);
		this.tabPageInbuiltFunctions.Controls.Add(this.inbuiltFunctionsTreeView);
		this.tabPageInbuiltFunctions.Controls.Add(this.functionHelpPanel);
		this.tabPageInbuiltFunctions.Location = new System.Drawing.Point(4, 25);
		this.tabPageInbuiltFunctions.Name = "tabPageInbuiltFunctions";
		this.tabPageInbuiltFunctions.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageInbuiltFunctions.Size = new System.Drawing.Size(568, 723);
		this.tabPageInbuiltFunctions.TabIndex = 0;
		this.tabPageInbuiltFunctions.Text = "Inbuilt Functions";
		this.tabPageInbuiltFunctions.UseVisualStyleBackColor = true;
		this.inbuiltFunctionsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.inbuiltFunctionsTreeView.Location = new System.Drawing.Point(3, 3);
		this.inbuiltFunctionsTreeView.Name = "inbuiltFunctionsTreeView";
		this.inbuiltFunctionsTreeView.PathSeparator = ".";
		this.inbuiltFunctionsTreeView.Size = new System.Drawing.Size(562, 589);
		this.inbuiltFunctionsTreeView.TabIndex = 0;
		this.inbuiltFunctionsTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(InbuiltFunctionsTreeView_ItemDrag);
		this.inbuiltFunctionsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(InbuiltFunctionsTreeView_AfterSelect);
		this.functionHelpPanel.BackColor = System.Drawing.SystemColors.Control;
		this.functionHelpPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.functionHelpPanel.Controls.Add(this.txtFunctionHelp);
		this.functionHelpPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.functionHelpPanel.Location = new System.Drawing.Point(3, 592);
		this.functionHelpPanel.Name = "functionHelpPanel";
		this.functionHelpPanel.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
		this.functionHelpPanel.Size = new System.Drawing.Size(562, 128);
		this.functionHelpPanel.TabIndex = 2;
		this.txtFunctionHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtFunctionHelp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtFunctionHelp.Location = new System.Drawing.Point(5, 3);
		this.txtFunctionHelp.Name = "txtFunctionHelp";
		this.txtFunctionHelp.ReadOnly = true;
		this.txtFunctionHelp.Size = new System.Drawing.Size(550, 120);
		this.txtFunctionHelp.TabIndex = 1;
		this.txtFunctionHelp.Text = "";
		this.tabPageVariables.Controls.Add(this.variablesTreeView);
		this.tabPageVariables.Controls.Add(this.variableHelpPanel);
		this.tabPageVariables.Location = new System.Drawing.Point(4, 25);
		this.tabPageVariables.Name = "tabPageVariables";
		this.tabPageVariables.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageVariables.Size = new System.Drawing.Size(568, 723);
		this.tabPageVariables.TabIndex = 1;
		this.tabPageVariables.Text = "Variables";
		this.tabPageVariables.UseVisualStyleBackColor = true;
		this.variablesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.variablesTreeView.Location = new System.Drawing.Point(3, 3);
		this.variablesTreeView.Margin = new System.Windows.Forms.Padding(4);
		this.variablesTreeView.Name = "variablesTreeView";
		this.variablesTreeView.PathSeparator = ".";
		this.variablesTreeView.Size = new System.Drawing.Size(562, 589);
		this.variablesTreeView.TabIndex = 0;
		this.variablesTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(VariableSelectionTreeView_ItemDrag);
		this.variablesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(VariablesTreeView_AfterSelect);
		this.variableHelpPanel.BackColor = System.Drawing.SystemColors.Control;
		this.variableHelpPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.variableHelpPanel.Controls.Add(this.txtVariablesHelp);
		this.variableHelpPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.variableHelpPanel.Location = new System.Drawing.Point(3, 592);
		this.variableHelpPanel.Name = "variableHelpPanel";
		this.variableHelpPanel.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
		this.variableHelpPanel.Size = new System.Drawing.Size(562, 128);
		this.variableHelpPanel.TabIndex = 3;
		this.txtVariablesHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtVariablesHelp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtVariablesHelp.Location = new System.Drawing.Point(5, 3);
		this.txtVariablesHelp.Name = "txtVariablesHelp";
		this.txtVariablesHelp.ReadOnly = true;
		this.txtVariablesHelp.Size = new System.Drawing.Size(550, 120);
		this.txtVariablesHelp.TabIndex = 2;
		this.txtVariablesHelp.Text = "";
		this.tabPageConstantValues.Controls.Add(this.constantValuesTreeView);
		this.tabPageConstantValues.Location = new System.Drawing.Point(4, 25);
		this.tabPageConstantValues.Name = "tabPageConstantValues";
		this.tabPageConstantValues.Size = new System.Drawing.Size(568, 723);
		this.tabPageConstantValues.TabIndex = 2;
		this.tabPageConstantValues.Text = "Constant Values";
		this.tabPageConstantValues.UseVisualStyleBackColor = true;
		this.constantValuesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.constantValuesTreeView.Location = new System.Drawing.Point(0, 0);
		this.constantValuesTreeView.Name = "constantValuesTreeView";
		this.constantValuesTreeView.PathSeparator = ".";
		this.constantValuesTreeView.Size = new System.Drawing.Size(568, 723);
		this.constantValuesTreeView.TabIndex = 0;
		this.constantValuesTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(ConstantValuesTreeView_ItemDrag);
		this.grpBoxExpression.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxExpression.Controls.Add(this.expressionPanel);
		this.grpBoxExpression.Location = new System.Drawing.Point(13, 13);
		this.grpBoxExpression.Name = "grpBoxExpression";
		this.grpBoxExpression.Size = new System.Drawing.Size(622, 775);
		this.grpBoxExpression.TabIndex = 8;
		this.grpBoxExpression.TabStop = false;
		this.grpBoxExpression.Text = "Expression";
		this.expressionPanel.AutoScroll = true;
		this.expressionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.expressionPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.expressionPanel.Location = new System.Drawing.Point(3, 18);
		this.expressionPanel.Name = "expressionPanel";
		this.expressionPanel.Size = new System.Drawing.Size(616, 754);
		this.expressionPanel.TabIndex = 0;
		this.expressionPanel.WrapContents = false;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(1232, 835);
		base.Controls.Add(this.grpBoxExpression);
		base.Controls.Add(this.grpBoxExpressionElements);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(1250, 882);
		base.Name = "ExpressionEditorForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Expression Editor";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(ExpressionEditorForm_HelpButtonClicked);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExpressionEditorForm_FormClosing);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ExpressionEditorForm_HelpRequested);
		base.Resize += new System.EventHandler(ExpressionEditorForm_Resize);
		this.grpBoxExpressionElements.ResumeLayout(false);
		this.expressionElementsTabControl.ResumeLayout(false);
		this.tabPageInbuiltFunctions.ResumeLayout(false);
		this.functionHelpPanel.ResumeLayout(false);
		this.tabPageVariables.ResumeLayout(false);
		this.variableHelpPanel.ResumeLayout(false);
		this.tabPageConstantValues.ResumeLayout(false);
		this.grpBoxExpression.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
