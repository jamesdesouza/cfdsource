using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Debug;
using TCX.CFD.Classes.FileSystem;
using TCX.TreeGridView;

namespace TCX.CFD.Controls;

public class DebugWatcherControl : UserControl
{
	private readonly Dictionary<string, string> variableValuesDictionary = new Dictionary<string, string>();

	private readonly List<string> removableNodes = new List<string>();

	private IContainer components;

	private TCX.TreeGridView.TreeGridView watcherTreeGridView;

	private ImageList treeViewImageList;

	private TreeGridColumn variableNameColumn;

	private DataGridViewTextBoxColumn variableValueColumn;

	private string GetVariableValue(DebugVariableInfo debugVariableInfo)
	{
		if (variableValuesDictionary.ContainsKey(debugVariableInfo.VariableName))
		{
			return variableValuesDictionary[debugVariableInfo.VariableName];
		}
		return "null";
	}

	private TreeGridNode AddChildNode(TreeGridNode parentNode, string name, string variableName, int imageIndex)
	{
		TreeGridNode treeGridNode = FindNode(name, parentNode.Nodes);
		if (treeGridNode == null)
		{
			DebugVariableInfo debugVariableInfo = new DebugVariableInfo(variableName);
			treeGridNode = parentNode.Nodes.Add(name, GetVariableValue(debugVariableInfo));
			treeGridNode.ImageIndex = imageIndex;
			treeGridNode.Tag = debugVariableInfo;
		}
		else
		{
			removableNodes.Remove(parentNode.Cells[0].Value.ToString() + "." + name);
		}
		return treeGridNode;
	}

	private void LoadSessionVariables()
	{
		List<Variable> properties = new List<Variable>
		{
			new Variable("ani", VariableScopes.Public, VariableAccessibilities.ReadOnly),
			new Variable("callid", VariableScopes.Public, VariableAccessibilities.ReadOnly),
			new Variable("dnis", VariableScopes.Public, VariableAccessibilities.ReadOnly),
			new Variable("did", VariableScopes.Public, VariableAccessibilities.ReadOnly),
			new Variable("audioFolder", VariableScopes.Public, VariableAccessibilities.ReadOnly),
			new Variable("transferingExtension", VariableScopes.Public, VariableAccessibilities.ReadOnly)
		};
		LoadVariables("session", properties, 2, onlyPublicVariables: false);
	}

	private TreeGridNode FindNode(string name, TreeGridNodeCollection nodeCollection)
	{
		foreach (TreeGridNode item in nodeCollection)
		{
			if (item.Cells[0].Value.ToString() == name)
			{
				return item;
			}
		}
		return null;
	}

	private TreeGridNode FindNode(string fullName)
	{
		foreach (TreeGridNode node in watcherTreeGridView.Nodes)
		{
			string text = node.Cells[0].Value.ToString();
			if (!fullName.StartsWith(text))
			{
				continue;
			}
			foreach (TreeGridNode node2 in node.Nodes)
			{
				if (text + "." + node2.Cells[0].Value.ToString() == fullName)
				{
					return node2;
				}
			}
		}
		return null;
	}

	private void LoadVariables(string name, List<Variable> properties, int variableImageIndex, bool onlyPublicVariables)
	{
		TreeGridNode treeGridNode = FindNode(name, watcherTreeGridView.Nodes);
		if (treeGridNode == null)
		{
			treeGridNode = watcherTreeGridView.Nodes.Add(name);
			treeGridNode.ImageIndex = 0;
		}
		foreach (Variable property in properties)
		{
			if (property.DebuggerVisible && (property.Scope == VariableScopes.Public || !onlyPublicVariables))
			{
				AddChildNode(treeGridNode, property.Name, name + "." + property.Name, variableImageIndex);
			}
		}
		if (treeGridNode.Nodes.Count == 0)
		{
			watcherTreeGridView.Nodes.Remove(treeGridNode);
		}
	}

	private void LoadActivitiesVariables(ReadOnlyCollection<Activity> activityCollection)
	{
		foreach (Activity item in activityCollection)
		{
			LoadVariables(item.Name, (item as IVadActivity).Properties, 2, onlyPublicVariables: true);
		}
	}

	private void LoadChildrenActivityVariables(Activity activity)
	{
		if (!(activity is CompositeActivity))
		{
			return;
		}
		CompositeActivity compositeActivity = activity as CompositeActivity;
		LoadActivitiesVariables(compositeActivity.EnabledActivities);
		foreach (Activity enabledActivity in compositeActivity.EnabledActivities)
		{
			LoadChildrenActivityVariables(enabledActivity);
		}
	}

	private void AddVariables(IVadActivity vadActivity)
	{
		RootFlow rootFlow = vadActivity.GetRootFlow();
		FileObject fileObject = rootFlow.FileObject;
		if (fileObject != null)
		{
			LoadVariables("callflow$", rootFlow.Properties, 1, onlyPublicVariables: false);
			ProjectObject projectObject = fileObject.GetProjectObject();
			if (projectObject != null)
			{
				if (!(fileObject is DialerFileObject))
				{
					LoadSessionVariables();
					LoadVariables("RecordResult", projectObject.RecordResultConstantList, 3, onlyPublicVariables: false);
					LoadVariables("MenuResult", projectObject.MenuResultConstantList, 3, onlyPublicVariables: false);
					LoadVariables("UserInputResult", projectObject.UserInputResultConstantList, 3, onlyPublicVariables: false);
					LoadVariables("VoiceInputResult", projectObject.VoiceInputResultConstantList, 3, onlyPublicVariables: false);
				}
				LoadVariables("project$", projectObject.Variables, 1, onlyPublicVariables: false);
			}
		}
		LoadChildrenActivityVariables(rootFlow);
	}

	public DebugWatcherControl()
	{
		InitializeComponent();
	}

	public void StartDebugging()
	{
		variableValuesDictionary.Clear();
	}

	public void StopDebugging()
	{
		watcherTreeGridView.Nodes.Clear();
	}

	public void SetActivity(IVadActivity vadActivity)
	{
		vadActivity.DebugModeActive = true;
		removableNodes.Clear();
		foreach (TreeGridNode node in watcherTreeGridView.Nodes)
		{
			string text = node.Cells[0].Value.ToString();
			foreach (TreeGridNode node2 in node.Nodes)
			{
				removableNodes.Add(text + "." + node2.Cells[0].Value.ToString());
			}
		}
		AddVariables(vadActivity);
		foreach (TreeGridNode node3 in watcherTreeGridView.Nodes)
		{
			node3.DefaultCellStyle.ForeColor = Color.Black;
			foreach (TreeGridNode node4 in node3.Nodes)
			{
				node4.DefaultCellStyle.ForeColor = Color.Black;
			}
		}
		while (removableNodes.Count > 0)
		{
			string fullName = removableNodes[0];
			TreeGridNode treeGridNode = FindNode(fullName);
			if (treeGridNode != null)
			{
				TreeGridNode treeGridNode2 = treeGridNode.Parent;
				treeGridNode2.Nodes.Remove(treeGridNode);
				if (treeGridNode2.Nodes.Count == 0)
				{
					watcherTreeGridView.Nodes.Remove(treeGridNode2);
				}
			}
			removableNodes.RemoveAt(0);
		}
	}

	public void SetVariableChange(VariableChangeDebugInfo variableChangeDebugInfo)
	{
		variableValuesDictionary[variableChangeDebugInfo.Name] = variableChangeDebugInfo.Value;
		foreach (TreeGridNode node in watcherTreeGridView.Nodes)
		{
			foreach (TreeGridNode node2 in node.Nodes)
			{
				DebugVariableInfo debugVariableInfo = node2.Tag as DebugVariableInfo;
				if (debugVariableInfo.VariableName == variableChangeDebugInfo.Name)
				{
					node2.SetValues(node2.Cells[0].Value, GetVariableValue(debugVariableInfo));
					node2.DefaultCellStyle.ForeColor = Color.Red;
					node.DefaultCellStyle.ForeColor = Color.Red;
					return;
				}
			}
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Controls.DebugWatcherControl));
		this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
		this.watcherTreeGridView = new TCX.TreeGridView.TreeGridView();
		this.variableNameColumn = new TCX.TreeGridView.TreeGridColumn();
		this.variableValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)this.watcherTreeGridView).BeginInit();
		base.SuspendLayout();
		this.treeViewImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("treeViewImageList.ImageStream");
		this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
		this.treeViewImageList.Images.SetKeyName(0, "ObjectInstance.png");
		this.treeViewImageList.Images.SetKeyName(1, "ObjectField.png");
		this.treeViewImageList.Images.SetKeyName(2, "ObjectProperty.png");
		this.treeViewImageList.Images.SetKeyName(3, "ObjectEnum.png");
		this.watcherTreeGridView.AllowUserToAddRows = false;
		this.watcherTreeGridView.AllowUserToDeleteRows = false;
		this.watcherTreeGridView.Columns.AddRange(this.variableNameColumn, this.variableValueColumn);
		this.watcherTreeGridView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.watcherTreeGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
		this.watcherTreeGridView.ImageList = this.treeViewImageList;
		this.watcherTreeGridView.Location = new System.Drawing.Point(0, 0);
		this.watcherTreeGridView.MultiSelect = false;
		this.watcherTreeGridView.Name = "watcherTreeGridView";
		this.watcherTreeGridView.Size = new System.Drawing.Size(455, 137);
		this.watcherTreeGridView.TabIndex = 0;
		this.variableNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		this.variableNameColumn.DefaultNodeImage = null;
		this.variableNameColumn.HeaderText = "Name";
		this.variableNameColumn.Name = "variableNameColumn";
		this.variableNameColumn.ReadOnly = true;
		this.variableNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.variableNameColumn.Width = 41;
		this.variableValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.variableValueColumn.HeaderText = "Value";
		this.variableValueColumn.Name = "variableValueColumn";
		this.variableValueColumn.ReadOnly = true;
		this.variableValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.variableValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.watcherTreeGridView);
		base.Name = "DebugWatcherControl";
		base.Size = new System.Drawing.Size(455, 137);
		((System.ComponentModel.ISupportInitialize)this.watcherTreeGridView).EndInit();
		base.ResumeLayout(false);
	}
}
