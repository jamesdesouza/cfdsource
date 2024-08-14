using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ExpressionEditorRowControl : UserControl
{
	public delegate void RowInsertedHandler(int controlIndex, AbsArgument insertedArgument);

	public delegate void RowUpdatedHandler(int controlIndex, AbsArgument updatedArgument, bool redraw);

	public delegate void RowDeletedHandler(int controlIndex);

	public delegate void RowHeightUpdatedHandler(int controlIndex, int deltaHeight);

	private const int ExtraRowHeight = 18;

	private const int InitialRowMargin = 5;

	private List<string> validVariables;

	private int controlIndex;

	private bool isLastControl;

	private Rectangle dragBoxFromMouseDown;

	private bool dropBefore = true;

	private IContainer components;

	private ToolTip expressionTooltip;

	private Panel detailPanel;

	private Panel panelDragAndDropTop;

	private PictureBox dragHandle;

	private Panel panelDragAndDropBottom;

	public List<string> ValidVariables
	{
		set
		{
			validVariables = value;
		}
	}

	public int ControlIndex
	{
		set
		{
			controlIndex = value;
		}
	}

	public bool IsLastControl
	{
		set
		{
			isLastControl = value;
		}
	}

	public bool IsDeleteEnabled
	{
		set
		{
			if (detailPanel.Controls[0] is ExpressionEditorEmptyControl expressionEditorEmptyControl)
			{
				expressionEditorEmptyControl.DeleteEnabled = value;
			}
		}
	}

	public UserControl ChildControl
	{
		set
		{
			detailPanel.Controls.Clear();
			detailPanel.Controls.Add(value);
			value.Width = detailPanel.Width - 5;
			int num = value.Height - detailPanel.Height;
			base.Height += num;
			this.HeightUpdated?.Invoke(controlIndex, num);
			value.DragEnter += RowControl_DragEnter;
			value.DragLeave += RowControl_DragLeave;
			value.DragOver += RowControl_DragOver;
			value.DragDrop += RowControl_DragDrop;
		}
	}

	public string HelpText
	{
		set
		{
			expressionTooltip.SetToolTip(this, value);
			expressionTooltip.SetToolTip(dragHandle, value);
			expressionTooltip.SetToolTip(detailPanel, value);
			foreach (UserControl control in detailPanel.Controls)
			{
				expressionTooltip.SetToolTip(control, value);
			}
		}
	}

	public event RowInsertedHandler Inserted;

	public event RowUpdatedHandler Updated;

	public event RowDeletedHandler Deleted;

	public event RowHeightUpdatedHandler HeightUpdated;

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (detailPanel.Controls.Count > 0)
		{
			detailPanel.Controls[0].Width = detailPanel.Width - 5;
		}
	}

	private void DragHandle_MouseDown(object sender, MouseEventArgs e)
	{
		Size dragSize = SystemInformation.DragSize;
		dragBoxFromMouseDown = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
	}

	private void DragHandle_MouseUp(object sender, MouseEventArgs e)
	{
		dragBoxFromMouseDown = Rectangle.Empty;
	}

	private void DragHandle_MouseMove(object sender, MouseEventArgs e)
	{
		if ((e.Button & MouseButtons.Left) == MouseButtons.Left && dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
		{
			AbsArgument argument = (detailPanel.Controls[0] as IExpressionEditorControl).GetArgument();
			string data = "MovingArgument." + base.Parent?.Parent.GetHashCode() + "|" + argument.GetString();
			if (DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move) == DragDropEffects.Move)
			{
				DeleteRow();
			}
		}
	}

	private ConstantValueTypes GetConstantValueType(string type)
	{
		return type switch
		{
			"String" => ConstantValueTypes.String, 
			"Multiline String" => ConstantValueTypes.MultilineString, 
			"Single Character" => ConstantValueTypes.Char, 
			"Boolean" => ConstantValueTypes.Boolean, 
			"Integer Number" => ConstantValueTypes.Integer, 
			"Floating Point Number" => ConstantValueTypes.Double, 
			_ => ConstantValueTypes.None, 
		};
	}

	private string GetDefaultValue(ConstantValueTypes forcedType)
	{
		return forcedType switch
		{
			ConstantValueTypes.Boolean => "true", 
			ConstantValueTypes.Char => "' '", 
			ConstantValueTypes.Double => "0.0", 
			ConstantValueTypes.Integer => "0", 
			_ => "\"\"", 
		};
	}

	private void RowControl_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.Text))
		{
			bool flag = detailPanel.Controls[0] is ExpressionEditorEmptyControl;
			bool flag2 = base.Parent?.Parent is ExpressionEditorInbuiltFunctionControl expressionEditorInbuiltFunctionControl && expressionEditorInbuiltFunctionControl.HasFixedArguments();
			string text = e.Data.GetData(DataFormats.Text).ToString();
			int num = text.IndexOf('|');
			bool flag3 = ((text.StartsWith("MovingArgument.") && num != -1) ? Convert.ToInt32(text.Substring(15, num - 15)) : (-1)) == base.Parent?.Parent.GetHashCode();
			if (flag || ((!flag2 || flag3) && this.Inserted != null))
			{
				e.Effect = DragDropEffects.Move;
				if (!flag && (!flag2 || flag3) && this.Inserted != null)
				{
					Point point = PointToClient(new Point(e.X, e.Y));
					dropBefore = !isLastControl || point.Y < base.Height / 2;
					panelDragAndDropTop.Visible = dropBefore;
					panelDragAndDropBottom.Visible = !dropBefore;
				}
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void RowControl_DragLeave(object sender, EventArgs e)
	{
		panelDragAndDropTop.Visible = false;
		panelDragAndDropBottom.Visible = false;
	}

	private void RowControl_DragOver(object sender, DragEventArgs e)
	{
		if (e.Effect == DragDropEffects.Move)
		{
			bool num = detailPanel.Controls[0] is ExpressionEditorEmptyControl;
			bool flag = base.Parent?.Parent is ExpressionEditorInbuiltFunctionControl expressionEditorInbuiltFunctionControl && expressionEditorInbuiltFunctionControl.HasFixedArguments();
			string text = e.Data.GetData(DataFormats.Text).ToString();
			int num2 = text.IndexOf('|');
			bool flag2 = ((text.StartsWith("MovingArgument.") && num2 != -1) ? Convert.ToInt32(text.Substring(15, num2 - 15)) : (-1)) == base.Parent?.Parent.GetHashCode();
			if (!num && (!flag || flag2) && this.Inserted != null)
			{
				Point point = PointToClient(new Point(e.X, e.Y));
				dropBefore = !isLastControl || point.Y < base.Height / 2;
				panelDragAndDropTop.Visible = dropBefore;
				panelDragAndDropBottom.Visible = !dropBefore;
			}
		}
	}

	private AbsArgument GetDroppedArgument(string data)
	{
		if (data.StartsWith("Constant Values."))
		{
			ConstantValueTypes constantValueType = GetConstantValueType(data.Substring(16));
			return new DotNetExpressionArgument(GetDefaultValue(constantValueType), constantValueType);
		}
		if (data.StartsWith("Boolean.") || data.StartsWith("String.") || data.StartsWith("Date Time.") || data.StartsWith("Number.") || data.StartsWith("Object."))
		{
			string name = data.Split('.')[1].Replace(" ", "_");
			AbsFunction absFunction = AbsFunction.BuildFunction(new List<string>(), name, new List<string>());
			int num = (absFunction.HasFixedArguments() ? absFunction.GetFixedArgumentCount() : absFunction.GetMinArgumentCount());
			for (int i = 0; i < num; i++)
			{
				absFunction.ArgumentList.Add(new DotNetExpressionArgument(""));
			}
			return absFunction;
		}
		if (data.StartsWith("VariableName."))
		{
			string expression = data.Substring(13);
			return AbsArgument.BuildArgument(validVariables, expression);
		}
		if (data.StartsWith("MovingArgument."))
		{
			string expression2 = data.Substring(1 + data.IndexOf('|'));
			return AbsArgument.BuildArgument(validVariables, expression2);
		}
		return null;
	}

	private void RowControl_DragDrop(object sender, DragEventArgs e)
	{
		string data = e.Data.GetData(DataFormats.Text).ToString();
		AbsArgument droppedArgument = GetDroppedArgument(data);
		if (droppedArgument != null)
		{
			if (detailPanel.Controls[0] is ExpressionEditorEmptyControl)
			{
				ChildControl = droppedArgument.GetExpressionEditorChildControl(validVariables);
				OnResize(EventArgs.Empty);
				UpdateRow(droppedArgument, redraw: true);
			}
			else
			{
				InsertRow(droppedArgument);
			}
		}
		RowControl_DragLeave(sender, EventArgs.Empty);
	}

	public ExpressionEditorRowControl()
	{
		InitializeComponent();
	}

	public void InsertRow(AbsArgument insertedArgument)
	{
		this.Inserted?.Invoke(dropBefore ? controlIndex : (controlIndex + 1), insertedArgument);
	}

	public void UpdateRow(AbsArgument updatedArgument, bool redraw)
	{
		this.Updated?.Invoke(controlIndex, updatedArgument, redraw);
	}

	public void DeleteRow()
	{
		this.Deleted?.Invoke(controlIndex);
	}

	public void UpdateRowHeight(int height)
	{
		int deltaHeight = height + 18 - base.Height;
		base.Height = height + 18;
		this.HeightUpdated?.Invoke(controlIndex, deltaHeight);
	}

	public void HideDragHandle()
	{
		int num = detailPanel.Location.X - dragHandle.Location.X - 5;
		detailPanel.Location = new Point(dragHandle.Location.X + 5, detailPanel.Location.Y);
		detailPanel.Size = new Size(detailPanel.Width + num, detailPanel.Height);
		dragHandle.Visible = false;
	}

	public void UpdateConstantValues()
	{
		(detailPanel.Controls[0] as IExpressionEditorControl)?.UpdateConstantValues();
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
		this.expressionTooltip = new System.Windows.Forms.ToolTip(this.components);
		this.detailPanel = new System.Windows.Forms.Panel();
		this.panelDragAndDropTop = new System.Windows.Forms.Panel();
		this.dragHandle = new System.Windows.Forms.PictureBox();
		this.panelDragAndDropBottom = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.dragHandle).BeginInit();
		base.SuspendLayout();
		this.expressionTooltip.AutoPopDelay = 20000;
		this.expressionTooltip.InitialDelay = 500;
		this.expressionTooltip.ReshowDelay = 100;
		this.expressionTooltip.ShowAlways = true;
		this.expressionTooltip.StripAmpersands = true;
		this.detailPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.detailPanel.Location = new System.Drawing.Point(38, 12);
		this.detailPanel.Name = "detailPanel";
		this.detailPanel.Size = new System.Drawing.Size(427, 50);
		this.detailPanel.TabIndex = 6;
		this.panelDragAndDropTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelDragAndDropTop.AutoScroll = true;
		this.panelDragAndDropTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelDragAndDropTop.Location = new System.Drawing.Point(6, 3);
		this.panelDragAndDropTop.Name = "panelDragAndDropTop";
		this.panelDragAndDropTop.Size = new System.Drawing.Size(456, 2);
		this.panelDragAndDropTop.TabIndex = 7;
		this.panelDragAndDropTop.Visible = false;
		this.dragHandle.Image = TCX.CFD.Properties.Resources.DragHandle;
		this.dragHandle.Location = new System.Drawing.Point(0, 19);
		this.dragHandle.Name = "dragHandle";
		this.dragHandle.Size = new System.Drawing.Size(36, 36);
		this.dragHandle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.dragHandle.TabIndex = 8;
		this.dragHandle.TabStop = false;
		this.dragHandle.MouseDown += new System.Windows.Forms.MouseEventHandler(DragHandle_MouseDown);
		this.dragHandle.MouseMove += new System.Windows.Forms.MouseEventHandler(DragHandle_MouseMove);
		this.dragHandle.MouseUp += new System.Windows.Forms.MouseEventHandler(DragHandle_MouseUp);
		this.panelDragAndDropBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelDragAndDropBottom.AutoScroll = true;
		this.panelDragAndDropBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelDragAndDropBottom.Location = new System.Drawing.Point(6, 65);
		this.panelDragAndDropBottom.Name = "panelDragAndDropBottom";
		this.panelDragAndDropBottom.Size = new System.Drawing.Size(456, 2);
		this.panelDragAndDropBottom.TabIndex = 9;
		this.panelDragAndDropBottom.Visible = false;
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.panelDragAndDropBottom);
		base.Controls.Add(this.dragHandle);
		base.Controls.Add(this.detailPanel);
		base.Controls.Add(this.panelDragAndDropTop);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "ExpressionEditorRowControl";
		base.Size = new System.Drawing.Size(468, 68);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(RowControl_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(RowControl_DragEnter);
		base.DragOver += new System.Windows.Forms.DragEventHandler(RowControl_DragOver);
		base.DragLeave += new System.EventHandler(RowControl_DragLeave);
		((System.ComponentModel.ISupportInitialize)this.dragHandle).EndInit();
		base.ResumeLayout(false);
	}
}
