using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ExpressionEditorInbuiltFunctionControl : UserControl, IExpressionEditorControl
{
	private const int SeparationHeight = 2;

	private const int CollapsingSeparationHeight = 5;

	private List<string> validVariables;

	private AbsFunction function;

	private List<AbsArgument> largestArgumentList = new List<AbsArgument>();

	private bool isReady;

	private bool isFunctionChangePending;

	private bool isCollapsed;

	private int collapsedHeight;

	private int expandedHeight;

	private IContainer components;

	private Label lblInbuiltFunction;

	private ComboBox comboFunction;

	private FlowLayoutPanel argumentsPanel;

	private PictureBox expandCollapseIcon;

	private PictureBox menuPicture;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem addMenuOption;

	private ToolStripMenuItem deleteMenuOption;

	public List<string> ValidVariables
	{
		set
		{
			validVariables = value;
		}
	}

	public AbsFunction Function
	{
		set
		{
			function = value;
			comboFunction.SelectedItem = function.Name.Replace("_", " ");
			collapsedHeight = base.Height + 5;
			addMenuOption.Enabled = !function.HasFixedArguments();
			while (function.ArgumentList.Count < function.GetMinArgumentCount())
			{
				function.ArgumentList.Add(new DotNetExpressionArgument(""));
			}
			for (int i = 0; i < function.ArgumentList.Count; i++)
			{
				AbsArgument absArgument = function.ArgumentList[i];
				largestArgumentList.Add(absArgument);
				AddRow(absArgument.GetExpressionEditorRowControl(validVariables));
			}
			ResetIndexes();
			isReady = true;
		}
	}

	private void FillFunctions()
	{
		string[] booleanFunctionList = ExpressionHelper.BooleanFunctionList;
		foreach (string text in booleanFunctionList)
		{
			comboFunction.Items.Add(text.Replace("_", " "));
		}
		booleanFunctionList = ExpressionHelper.StringFunctionList;
		foreach (string text2 in booleanFunctionList)
		{
			comboFunction.Items.Add(text2.Replace("_", " "));
		}
		booleanFunctionList = ExpressionHelper.DateTimeFunctionList;
		foreach (string text3 in booleanFunctionList)
		{
			comboFunction.Items.Add(text3.Replace("_", " "));
		}
		booleanFunctionList = ExpressionHelper.NumberFunctionList;
		foreach (string text4 in booleanFunctionList)
		{
			comboFunction.Items.Add(text4.Replace("_", " "));
		}
		booleanFunctionList = ExpressionHelper.AnyFunctionList;
		foreach (string text5 in booleanFunctionList)
		{
			comboFunction.Items.Add(text5.Replace("_", " "));
		}
	}

	private void RowControl_Inserted(int controlIndex, AbsArgument insertedArgument)
	{
		function.ArgumentList.Insert(controlIndex, insertedArgument);
		largestArgumentList.Insert(controlIndex, insertedArgument);
		AddRow(controlIndex, insertedArgument);
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRow(function, redraw: false);
	}

	private void RowControl_Updated(int controlIndex, AbsArgument updatedArgument, bool redraw)
	{
		function.ArgumentList[controlIndex] = updatedArgument;
		largestArgumentList[controlIndex] = updatedArgument;
		if (redraw)
		{
			RemoveRow(controlIndex);
			AddRow(controlIndex, updatedArgument);
		}
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRow(function, redraw: false);
	}

	private void RowControl_Deleted(int controlIndex)
	{
		if ((function.HasFixedArguments() && function.ArgumentList.Count == function.GetFixedArgumentCount()) || function.ArgumentList.Count <= function.GetMinArgumentCount())
		{
			RowControl_Updated(controlIndex, new DotNetExpressionArgument(""), redraw: true);
			return;
		}
		function.ArgumentList.RemoveAt(controlIndex);
		largestArgumentList.RemoveAt(controlIndex);
		RemoveRow(controlIndex);
		ResetIndexes();
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRow(function, redraw: false);
	}

	private void RowControl_HeightUpdated(int controlIndex, int deltaHeight)
	{
		base.Height += deltaHeight;
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRowHeight(base.Height);
	}

	private void ChangeFunction()
	{
		AbsFunction absFunction = AbsFunction.BuildFunction(new List<string>(), comboFunction.SelectedItem.ToString().Replace(" ", "_"), new List<string>());
		int num = (absFunction.HasFixedArguments() ? absFunction.GetFixedArgumentCount() : Math.Max(function.ArgumentList.Count, absFunction.GetMinArgumentCount()));
		for (int i = 0; i < num; i++)
		{
			if (i < largestArgumentList.Count)
			{
				absFunction.ArgumentList.Add(largestArgumentList[i]);
				continue;
			}
			AbsArgument item = new DotNetExpressionArgument("");
			absFunction.ArgumentList.Add(item);
			largestArgumentList.Add(item);
		}
		function = absFunction;
		addMenuOption.Enabled = !function.HasFixedArguments();
		while (argumentsPanel.Controls.Count < num)
		{
			AddRow(function.ArgumentList[argumentsPanel.Controls.Count].GetExpressionEditorRowControl(validVariables));
		}
		while (argumentsPanel.Controls.Count > num)
		{
			RemoveRow(argumentsPanel.Controls.Count - 1);
		}
		ResetIndexes();
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRow(function, redraw: false);
	}

	private void ComboFunction_DropDownClosed(object sender, EventArgs e)
	{
		if (isFunctionChangePending)
		{
			ChangeFunction();
			isFunctionChangePending = false;
		}
	}

	private void ComboFunction_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (isReady)
		{
			if (comboFunction.DroppedDown)
			{
				isFunctionChangePending = true;
			}
			else
			{
				ChangeFunction();
			}
		}
	}

	private void ComboFunction_MouseWheel(object sender, MouseEventArgs e)
	{
		((HandledMouseEventArgs)e).Handled = true;
	}

	private void ExpandCollapseIcon_Click(object sender, EventArgs e)
	{
		isCollapsed = !isCollapsed;
		expandCollapseIcon.Image = (isCollapsed ? Resources.Expression_Expand : Resources.Expression_Collapse);
		if (isCollapsed)
		{
			base.Height = collapsedHeight;
		}
		else
		{
			base.Height = expandedHeight;
		}
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRowHeight(base.Height);
	}

	private void AddButton_Click(object sender, EventArgs e)
	{
		AbsArgument absArgument = new DotNetExpressionArgument("");
		function.ArgumentList.Add(absArgument);
		if (largestArgumentList.Count < function.ArgumentList.Count)
		{
			largestArgumentList.Add(absArgument);
		}
		else
		{
			largestArgumentList[function.ArgumentList.Count - 1] = absArgument;
		}
		AddRow(absArgument.GetExpressionEditorRowControl(validVariables));
		ResetIndexes();
		(base.Parent.Parent as ExpressionEditorRowControl).UpdateRow(function, redraw: false);
	}

	private void DeleteButton_Click(object sender, EventArgs e)
	{
		(base.Parent?.Parent as ExpressionEditorRowControl)?.DeleteRow();
	}

	private void MenuPicture_Click(object sender, EventArgs e)
	{
		contextMenu.Show(menuPicture, 10, 10);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);
		foreach (Control control in argumentsPanel.Controls)
		{
			control.Width = argumentsPanel.Width;
		}
	}

	public AbsArgument GetArgument()
	{
		return function;
	}

	public void UpdateConstantValues()
	{
		foreach (ExpressionEditorRowControl control in argumentsPanel.Controls)
		{
			control.UpdateConstantValues();
		}
	}

	public bool HasFixedArguments()
	{
		return function.HasFixedArguments();
	}

	public ExpressionEditorInbuiltFunctionControl()
	{
		InitializeComponent();
		FillFunctions();
		addMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorInbuiltFunctionControl.addMenuOption.Text");
		deleteMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorInbuiltFunctionControl.deleteMenuOption.Text");
	}

	private void ResetIndexes()
	{
		bool ısDeleteEnabled = !function.HasFixedArguments() && function.ArgumentList.Count > function.GetMinArgumentCount();
		for (int i = 0; i < argumentsPanel.Controls.Count; i++)
		{
			ExpressionEditorRowControl obj = argumentsPanel.Controls[i] as ExpressionEditorRowControl;
			obj.ControlIndex = i;
			obj.IsLastControl = i == argumentsPanel.Controls.Count - 1;
			obj.IsDeleteEnabled = ısDeleteEnabled;
		}
	}

	private void RemoveRow(int controlIndex)
	{
		int num = argumentsPanel.Controls[controlIndex].Height;
		base.Height -= num + 2;
		argumentsPanel.Controls.RemoveAt(controlIndex);
		expandedHeight = base.Height;
		(base.Parent?.Parent as ExpressionEditorRowControl)?.UpdateRowHeight(base.Height);
	}

	private void AddRow(int controlIndex, AbsArgument argument)
	{
		ExpressionEditorRowControl expressionEditorRowControl = argument.GetExpressionEditorRowControl(validVariables);
		AddRow(expressionEditorRowControl);
		argumentsPanel.Controls.SetChildIndex(expressionEditorRowControl, controlIndex);
		ResetIndexes();
	}

	private void AddRow(ExpressionEditorRowControl rowControl)
	{
		rowControl.Inserted += RowControl_Inserted;
		rowControl.Updated += RowControl_Updated;
		rowControl.Deleted += RowControl_Deleted;
		rowControl.HeightUpdated += RowControl_HeightUpdated;
		rowControl.Width = argumentsPanel.Width;
		base.Height += rowControl.Height + 2;
		argumentsPanel.Controls.Add(rowControl);
		expandedHeight = base.Height;
		(base.Parent?.Parent as ExpressionEditorRowControl)?.UpdateRowHeight(base.Height);
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
		this.lblInbuiltFunction = new System.Windows.Forms.Label();
		this.comboFunction = new System.Windows.Forms.ComboBox();
		this.argumentsPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.expandCollapseIcon = new System.Windows.Forms.PictureBox();
		this.menuPicture = new System.Windows.Forms.PictureBox();
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.addMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		this.deleteMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		((System.ComponentModel.ISupportInitialize)this.expandCollapseIcon).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.menuPicture).BeginInit();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.lblInbuiltFunction.AutoSize = true;
		this.lblInbuiltFunction.Location = new System.Drawing.Point(0, 0);
		this.lblInbuiltFunction.Name = "lblInbuiltFunction";
		this.lblInbuiltFunction.Size = new System.Drawing.Size(103, 17);
		this.lblInbuiltFunction.TabIndex = 0;
		this.lblInbuiltFunction.Text = "Inbuilt Function";
		this.comboFunction.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFunction.FormattingEnabled = true;
		this.comboFunction.Location = new System.Drawing.Point(31, 20);
		this.comboFunction.Name = "comboFunction";
		this.comboFunction.Size = new System.Drawing.Size(391, 24);
		this.comboFunction.Sorted = true;
		this.comboFunction.TabIndex = 1;
		this.comboFunction.SelectedIndexChanged += new System.EventHandler(ComboFunction_SelectedIndexChanged);
		this.comboFunction.DropDownClosed += new System.EventHandler(ComboFunction_DropDownClosed);
		this.comboFunction.MouseWheel += new System.Windows.Forms.MouseEventHandler(ComboFunction_MouseWheel);
		this.argumentsPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.argumentsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.argumentsPanel.Location = new System.Drawing.Point(27, 51);
		this.argumentsPanel.Name = "argumentsPanel";
		this.argumentsPanel.Size = new System.Drawing.Size(394, 0);
		this.argumentsPanel.TabIndex = 5;
		this.argumentsPanel.WrapContents = false;
		this.expandCollapseIcon.Image = TCX.CFD.Properties.Resources.Expression_Collapse;
		this.expandCollapseIcon.Location = new System.Drawing.Point(7, 16);
		this.expandCollapseIcon.Name = "expandCollapseIcon";
		this.expandCollapseIcon.Size = new System.Drawing.Size(18, 28);
		this.expandCollapseIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.expandCollapseIcon.TabIndex = 9;
		this.expandCollapseIcon.TabStop = false;
		this.expandCollapseIcon.Click += new System.EventHandler(ExpandCollapseIcon_Click);
		this.menuPicture.Image = TCX.CFD.Properties.Resources.Expression_Menu;
		this.menuPicture.Location = new System.Drawing.Point(109, -1);
		this.menuPicture.Name = "menuPicture";
		this.menuPicture.Size = new System.Drawing.Size(18, 18);
		this.menuPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.menuPicture.TabIndex = 11;
		this.menuPicture.TabStop = false;
		this.menuPicture.Click += new System.EventHandler(MenuPicture_Click);
		this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.addMenuOption, this.deleteMenuOption });
		this.contextMenu.Name = "contextMenu";
		this.contextMenu.Size = new System.Drawing.Size(177, 52);
		this.addMenuOption.Image = TCX.CFD.Properties.Resources.Edit_Add;
		this.addMenuOption.Name = "addMenuOption";
		this.addMenuOption.Size = new System.Drawing.Size(176, 24);
		this.addMenuOption.Text = "Add Argument";
		this.addMenuOption.Click += new System.EventHandler(AddButton_Click);
		this.deleteMenuOption.Image = TCX.CFD.Properties.Resources.Edit_Remove;
		this.deleteMenuOption.Name = "deleteMenuOption";
		this.deleteMenuOption.Size = new System.Drawing.Size(176, 24);
		this.deleteMenuOption.Text = "Delete";
		this.deleteMenuOption.Click += new System.EventHandler(DeleteButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.menuPicture);
		base.Controls.Add(this.expandCollapseIcon);
		base.Controls.Add(this.argumentsPanel);
		base.Controls.Add(this.comboFunction);
		base.Controls.Add(this.lblInbuiltFunction);
		base.Name = "ExpressionEditorInbuiltFunctionControl";
		base.Size = new System.Drawing.Size(425, 50);
		((System.ComponentModel.ISupportInitialize)this.expandCollapseIcon).EndInit();
		((System.ComponentModel.ISupportInitialize)this.menuPicture).EndInit();
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
