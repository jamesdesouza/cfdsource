using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class VoiceInputHintBuilderRowControl : UserControl
{
	public delegate void RowInsertedHandler(int controlIndex, AbsVoiceInputHint insertedHint, bool isFirstControl);

	public delegate void RowUpdatedHandler(int controlIndex, AbsVoiceInputHint updatedHint);

	public delegate void RowDeletedHandler(int controlIndex);

	private const int InitialRowMargin = 5;

	private const int PanelMargin = 8;

	private const int DragHandleSpace = 38;

	private readonly string languageCode;

	private AbsVoiceInputHint voiceInputHint;

	private int controlIndex;

	private bool isLastControl;

	private Rectangle dragBoxFromMouseDown;

	private bool dropBefore = true;

	private IContainer components;

	private ToolTip hintTooltip;

	private Panel detailPanel;

	private Panel panelDragAndDropTop;

	private PictureBox dragHandle;

	private Panel panelDragAndDropBottom;

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

	public bool DragHandleVisible
	{
		set
		{
			if (value)
			{
				detailPanel.Location = new Point(38, detailPanel.Location.Y);
				detailPanel.Size = new Size(base.Width - 38 - 8, detailPanel.Height);
			}
			else
			{
				detailPanel.Location = new Point(5, detailPanel.Location.Y);
				detailPanel.Size = new Size(base.Width - 8, detailPanel.Height);
			}
			dragHandle.Visible = value;
		}
	}

	public event RowInsertedHandler Inserted;

	public event RowUpdatedHandler Updated;

	public event RowDeletedHandler Deleted;

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		ResizeChildren();
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
			string data = "MovingHint." + voiceInputHint?.GetText();
			if (DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move) == DragDropEffects.Move)
			{
				DeleteRow();
			}
		}
	}

	private void RowControl_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.Text))
		{
			bool flag = detailPanel.Controls[0] is VoiceInputHintBuilderEmptyControl;
			string obj = e.Data.GetData(DataFormats.Text).ToString();
			bool flag2 = obj.StartsWith("MovingHint.");
			bool flag3 = obj.StartsWith("Tokens.");
			bool flag4 = obj.StartsWith("Constant Values.");
			if (flag2 || flag3 || flag4)
			{
				e.Effect = DragDropEffects.Move;
				if (!flag)
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
		if (e.Effect == DragDropEffects.Move && !(detailPanel.Controls[0] is VoiceInputHintBuilderEmptyControl))
		{
			Point point = PointToClient(new Point(e.X, e.Y));
			dropBefore = !isLastControl || point.Y < base.Height / 2;
			panelDragAndDropTop.Visible = dropBefore;
			panelDragAndDropBottom.Visible = !dropBefore;
		}
	}

	private AbsVoiceInputHint GetDroppedHint(string data)
	{
		if (data == "Constant Values.String")
		{
			return new VoiceInputHintConstantValue("");
		}
		if (data.StartsWith("Tokens."))
		{
			string token = data.Split('.')[1];
			return VoiceInputHintsHelper.GetToken(languageCode, token).Clone();
		}
		if (data.StartsWith("MovingHint."))
		{
			string token2 = data.Substring(11);
			VoiceInputHintToken token3 = VoiceInputHintsHelper.GetToken(languageCode, token2);
			if (token3 != null)
			{
				return token3.Clone();
			}
			return new VoiceInputHintConstantValue(token2);
		}
		return null;
	}

	private void RowControl_DragDrop(object sender, DragEventArgs e)
	{
		string data = e.Data.GetData(DataFormats.Text).ToString();
		AbsVoiceInputHint droppedHint = GetDroppedHint(data);
		if (droppedHint != null)
		{
			InsertRow(droppedHint, detailPanel.Controls[0] is VoiceInputHintBuilderEmptyControl);
		}
		RowControl_DragLeave(sender, EventArgs.Empty);
	}

	private void SetHelpText(string helpText)
	{
		hintTooltip.SetToolTip(this, helpText);
		hintTooltip.SetToolTip(dragHandle, helpText);
		hintTooltip.SetToolTip(detailPanel, helpText);
		foreach (UserControl control in detailPanel.Controls)
		{
			hintTooltip.SetToolTip(control, helpText);
		}
	}

	private void SetChildControl(UserControl childControl)
	{
		detailPanel.Controls.Clear();
		detailPanel.Controls.Add(childControl);
		childControl.Width = detailPanel.Width - 5;
		childControl.DragEnter += RowControl_DragEnter;
		childControl.DragLeave += RowControl_DragLeave;
		childControl.DragOver += RowControl_DragOver;
		childControl.DragDrop += RowControl_DragDrop;
	}

	public VoiceInputHintBuilderRowControl(string languageCode)
	{
		InitializeComponent();
		this.languageCode = languageCode;
		voiceInputHint = null;
		SetChildControl(new VoiceInputHintBuilderEmptyControl());
	}

	public VoiceInputHintBuilderRowControl(string languageCode, AbsVoiceInputHint voiceInputHint)
	{
		InitializeComponent();
		this.languageCode = languageCode;
		this.voiceInputHint = voiceInputHint;
		SetChildControl(voiceInputHint.CreateEditorControl());
		SetHelpText(voiceInputHint.GetHelpText());
	}

	private void InsertRow(AbsVoiceInputHint insertedHint, bool isFirstControl)
	{
		this.Inserted?.Invoke(dropBefore ? controlIndex : (controlIndex + 1), insertedHint, isFirstControl);
	}

	public void UpdateRow(AbsVoiceInputHint updatedHint)
	{
		voiceInputHint = updatedHint;
		this.Updated?.Invoke(controlIndex, updatedHint);
	}

	public void DeleteRow()
	{
		this.Deleted?.Invoke(controlIndex);
	}

	public void ResizeChildren()
	{
		foreach (Control control in detailPanel.Controls)
		{
			control.Width = detailPanel.Width - 5;
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
		this.hintTooltip = new System.Windows.Forms.ToolTip(this.components);
		this.detailPanel = new System.Windows.Forms.Panel();
		this.panelDragAndDropTop = new System.Windows.Forms.Panel();
		this.dragHandle = new System.Windows.Forms.PictureBox();
		this.panelDragAndDropBottom = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)this.dragHandle).BeginInit();
		base.SuspendLayout();
		this.hintTooltip.AutoPopDelay = 20000;
		this.hintTooltip.InitialDelay = 500;
		this.hintTooltip.ReshowDelay = 100;
		this.hintTooltip.ShowAlways = true;
		this.hintTooltip.StripAmpersands = true;
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
		base.Name = "VoiceInputHintBuilderRowControl";
		base.Size = new System.Drawing.Size(468, 68);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(RowControl_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(RowControl_DragEnter);
		base.DragOver += new System.Windows.Forms.DragEventHandler(RowControl_DragOver);
		base.DragLeave += new System.EventHandler(RowControl_DragLeave);
		((System.ComponentModel.ISupportInitialize)this.dragHandle).EndInit();
		base.ResumeLayout(false);
	}
}
