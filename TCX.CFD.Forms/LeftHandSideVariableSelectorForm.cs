using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class LeftHandSideVariableSelectorForm : Form
{
	private string variableName = string.Empty;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private TreeView variablesTreeView;

	private Label lblNoVariables;

	public IVadActivity Component
	{
		set
		{
			foreach (string validVariable in ExpressionHelper.GetValidVariables(value, onlyWritableVariables: true, includeConstants: false))
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
			if (variablesTreeView.Nodes.Count > 0)
			{
				lblNoVariables.Visible = false;
			}
			variablesTreeView.Sort();
			variablesTreeView.ExpandAll();
		}
	}

	public string VariableName
	{
		get
		{
			return variableName;
		}
		set
		{
			TreeNodeCollection nodes = variablesTreeView.Nodes;
			TreeNode treeNode = null;
			string[] array = value.Split(new string[1] { variablesTreeView.PathSeparator }, StringSplitOptions.None);
			foreach (string key in array)
			{
				treeNode = FindChild(key, nodes);
				if (treeNode == null)
				{
					break;
				}
				nodes = treeNode.Nodes;
			}
			variablesTreeView.SelectedNode = treeNode;
		}
	}

	private TreeNode FindChild(string key, TreeNodeCollection nodeCollection)
	{
		foreach (TreeNode item in nodeCollection)
		{
			if (item.Text == key)
			{
				return item;
			}
		}
		return null;
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = variablesTreeView.SelectedNode;
		if (selectedNode == null)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.MessageBox.Error.NoSelection"), LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		if (selectedNode.Nodes.Count > 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.MessageBox.Error.InvalidSelection"), LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		variableName = selectedNode.FullPath;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void VariablesTreeView_DoubleClick(object sender, EventArgs e)
	{
		TreeNode selectedNode = variablesTreeView.SelectedNode;
		if (selectedNode != null && selectedNode.Nodes.Count == 0)
		{
			okButton.PerformClick();
		}
	}

	private void VariablesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		TreeNode selectedNode = variablesTreeView.SelectedNode;
		okButton.Enabled = selectedNode != null && selectedNode.Nodes.Count == 0;
	}

	public LeftHandSideVariableSelectorForm()
	{
		InitializeComponent();
		Text = LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.Title");
		lblNoVariables.Text = LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.lblNoVariables.Text");
		okButton.Text = LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("LeftHandSideVariableSelectorForm.cancelButton.Text");
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

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-conditions-variables/#h.7wya1opzarwq");
	}

	private void LeftHandSideVariableSelectorForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void LeftHandSideVariableSelectorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Forms.LeftHandSideVariableSelectorForm));
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.variablesTreeView = new System.Windows.Forms.TreeView();
		this.lblNoVariables = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 320);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 2;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Enabled = false;
		this.okButton.Location = new System.Drawing.Point(389, 320);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 1;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.variablesTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.variablesTreeView.HideSelection = false;
		this.variablesTreeView.Location = new System.Drawing.Point(16, 15);
		this.variablesTreeView.Margin = new System.Windows.Forms.Padding(4);
		this.variablesTreeView.Name = "variablesTreeView";
		this.variablesTreeView.PathSeparator = ".";
		this.variablesTreeView.Size = new System.Drawing.Size(580, 297);
		this.variablesTreeView.TabIndex = 0;
		this.variablesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(VariablesTreeView_AfterSelect);
		this.variablesTreeView.DoubleClick += new System.EventHandler(VariablesTreeView_DoubleClick);
		this.lblNoVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblNoVariables.BackColor = System.Drawing.SystemColors.Window;
		this.lblNoVariables.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNoVariables.ForeColor = System.Drawing.SystemColors.ControlDark;
		this.lblNoVariables.Location = new System.Drawing.Point(32, 33);
		this.lblNoVariables.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblNoVariables.Name = "lblNoVariables";
		this.lblNoVariables.Size = new System.Drawing.Size(551, 261);
		this.lblNoVariables.TabIndex = 3;
		this.lblNoVariables.Text = resources.GetString("lblNoVariables.Text");
		this.lblNoVariables.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(613, 363);
		base.Controls.Add(this.lblNoVariables);
		base.Controls.Add(this.variablesTreeView);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(618, 394);
		base.Name = "LeftHandSideVariableSelectorForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Variable Selector";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(LeftHandSideVariableSelectorForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(LeftHandSideVariableSelectorForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
