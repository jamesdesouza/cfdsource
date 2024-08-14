using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class DateTimeConditionCollectionEditorForm : Form
{
	private readonly IVadActivity activity;

	private List<DateTimeCondition> dateTimeConditionList;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Button addButton;

	private Button removeButton;

	private Button moveUpButton;

	private Button moveDownButton;

	private Panel childrenPanel;

	private Button selectAllButton;

	private Button clearAllButton;

	public List<DateTimeCondition> DateTimeConditionList
	{
		get
		{
			return dateTimeConditionList;
		}
		set
		{
			dateTimeConditionList = new List<DateTimeCondition>(value);
			foreach (DateTimeCondition dateTimeCondition in dateTimeConditionList)
			{
				AddDateTimeCondition(dateTimeCondition);
			}
		}
	}

	private void AddDateTimeCondition(DateTimeCondition dateTimeCondition)
	{
		DateTimeConditionEditorRowControl dateTimeConditionEditorRowControl = new DateTimeConditionEditorRowControl(dateTimeCondition);
		dateTimeConditionEditorRowControl.Location = new Point(0, childrenPanel.AutoScrollPosition.Y + childrenPanel.Controls.Count * dateTimeConditionEditorRowControl.Height);
		dateTimeConditionEditorRowControl.CheckedChanged += RowControl_CheckedChanged;
		childrenPanel.Controls.Add(dateTimeConditionEditorRowControl);
		ResizeRows();
	}

	private void ResizeRows()
	{
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			control.Size = new Size(childrenPanel.VerticalScroll.Visible ? (childrenPanel.Width - 20) : (childrenPanel.Width - 6), control.Height);
		}
	}

	private void RelocateRowControls()
	{
		int num = 0;
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			control.Location = new Point(0, childrenPanel.AutoScrollPosition.Y + num++ * control.Height);
		}
	}

	private void EnableDisableButtons()
	{
		bool flag = false;
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			if (control.IsChecked)
			{
				flag = true;
				break;
			}
		}
		removeButton.Enabled = flag;
		if (flag && childrenPanel.Controls.Count > 1)
		{
			DateTimeConditionEditorRowControl dateTimeConditionEditorRowControl = childrenPanel.Controls[0] as DateTimeConditionEditorRowControl;
			DateTimeConditionEditorRowControl dateTimeConditionEditorRowControl2 = childrenPanel.Controls[childrenPanel.Controls.Count - 1] as DateTimeConditionEditorRowControl;
			moveUpButton.Enabled = !dateTimeConditionEditorRowControl.IsChecked;
			moveDownButton.Enabled = !dateTimeConditionEditorRowControl2.IsChecked;
		}
		else
		{
			moveUpButton.Enabled = false;
			moveDownButton.Enabled = false;
		}
		selectAllButton.Enabled = childrenPanel.Controls.Count > 0;
		clearAllButton.Enabled = childrenPanel.Controls.Count > 0;
	}

	private void RowControl_CheckedChanged(object sender, EventArgs e)
	{
		EnableDisableButtons();
	}

	public DateTimeConditionCollectionEditorForm(IVadActivity activity)
	{
		InitializeComponent();
		this.activity = activity;
		Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.Text");
		addButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.addButton.Text");
		removeButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.removeButton.Text");
		moveUpButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.moveUpButton.Text");
		moveDownButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.moveDownButton.Text");
		selectAllButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.selectAllButton.Text");
		clearAllButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.clearAllButton.Text");
		okButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionCollectionEditorForm.cancelButton.Text");
	}

	private void AddButton_Click(object sender, EventArgs e)
	{
		DateTimeCondition dateTimeCondition = new DayOfWeekDateTimeCondition();
		dateTimeConditionList.Add(dateTimeCondition);
		AddDateTimeCondition(dateTimeCondition);
	}

	private void RemoveButton_Click(object sender, EventArgs e)
	{
		List<DateTimeConditionEditorRowControl> list = new List<DateTimeConditionEditorRowControl>();
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			if (control.IsChecked)
			{
				list.Add(control);
			}
		}
		foreach (DateTimeConditionEditorRowControl item in list)
		{
			childrenPanel.Controls.Remove(item);
		}
		RelocateRowControls();
		EnableDisableButtons();
		ResizeRows();
	}

	private void MoveUpButton_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < childrenPanel.Controls.Count; i++)
		{
			DateTimeConditionEditorRowControl dateTimeConditionEditorRowControl = childrenPanel.Controls[i] as DateTimeConditionEditorRowControl;
			if (dateTimeConditionEditorRowControl.IsChecked)
			{
				childrenPanel.Controls.SetChildIndex(dateTimeConditionEditorRowControl, i - 1);
			}
		}
		RelocateRowControls();
		EnableDisableButtons();
	}

	private void MoveDownButton_Click(object sender, EventArgs e)
	{
		for (int num = childrenPanel.Controls.Count - 1; num >= 0; num--)
		{
			DateTimeConditionEditorRowControl dateTimeConditionEditorRowControl = childrenPanel.Controls[num] as DateTimeConditionEditorRowControl;
			if (dateTimeConditionEditorRowControl.IsChecked)
			{
				childrenPanel.Controls.SetChildIndex(dateTimeConditionEditorRowControl, num + 1);
			}
		}
		RelocateRowControls();
		EnableDisableButtons();
	}

	private void SelectAllButton_Click(object sender, EventArgs e)
	{
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			control.IsChecked = true;
		}
	}

	private void ClearAllButton_Click(object sender, EventArgs e)
	{
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			control.IsChecked = false;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		dateTimeConditionList.Clear();
		foreach (DateTimeConditionEditorRowControl control in childrenPanel.Controls)
		{
			DateTimeCondition item = control.Save();
			dateTimeConditionList.Add(item);
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void DateTimeConditionCollectionEditorForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		activity.ShowHelp();
	}

	private void DateTimeConditionCollectionEditorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		activity.ShowHelp();
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
		this.addButton = new System.Windows.Forms.Button();
		this.removeButton = new System.Windows.Forms.Button();
		this.moveUpButton = new System.Windows.Forms.Button();
		this.moveDownButton = new System.Windows.Forms.Button();
		this.childrenPanel = new System.Windows.Forms.Panel();
		this.selectAllButton = new System.Windows.Forms.Button();
		this.clearAllButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.Location = new System.Drawing.Point(720, 334);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 8;
		this.cancelButton.Text = "Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(612, 334);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 7;
		this.okButton.Text = "OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.addButton.Location = new System.Drawing.Point(17, 16);
		this.addButton.Margin = new System.Windows.Forms.Padding(4);
		this.addButton.Name = "addButton";
		this.addButton.Size = new System.Drawing.Size(100, 28);
		this.addButton.TabIndex = 0;
		this.addButton.Text = "Add";
		this.addButton.UseVisualStyleBackColor = true;
		this.addButton.Click += new System.EventHandler(AddButton_Click);
		this.removeButton.Enabled = false;
		this.removeButton.Location = new System.Drawing.Point(125, 16);
		this.removeButton.Margin = new System.Windows.Forms.Padding(4);
		this.removeButton.Name = "removeButton";
		this.removeButton.Size = new System.Drawing.Size(100, 28);
		this.removeButton.TabIndex = 1;
		this.removeButton.Text = "Remove";
		this.removeButton.UseVisualStyleBackColor = true;
		this.removeButton.Click += new System.EventHandler(RemoveButton_Click);
		this.moveUpButton.Enabled = false;
		this.moveUpButton.Location = new System.Drawing.Point(233, 16);
		this.moveUpButton.Margin = new System.Windows.Forms.Padding(4);
		this.moveUpButton.Name = "moveUpButton";
		this.moveUpButton.Size = new System.Drawing.Size(100, 28);
		this.moveUpButton.TabIndex = 2;
		this.moveUpButton.Text = "Move Up";
		this.moveUpButton.UseVisualStyleBackColor = true;
		this.moveUpButton.Click += new System.EventHandler(MoveUpButton_Click);
		this.moveDownButton.Enabled = false;
		this.moveDownButton.Location = new System.Drawing.Point(341, 16);
		this.moveDownButton.Margin = new System.Windows.Forms.Padding(4);
		this.moveDownButton.Name = "moveDownButton";
		this.moveDownButton.Size = new System.Drawing.Size(100, 28);
		this.moveDownButton.TabIndex = 3;
		this.moveDownButton.Text = "Move Down";
		this.moveDownButton.UseVisualStyleBackColor = true;
		this.moveDownButton.Click += new System.EventHandler(MoveDownButton_Click);
		this.childrenPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.childrenPanel.AutoScroll = true;
		this.childrenPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.childrenPanel.Location = new System.Drawing.Point(17, 53);
		this.childrenPanel.Margin = new System.Windows.Forms.Padding(4);
		this.childrenPanel.Name = "childrenPanel";
		this.childrenPanel.Size = new System.Drawing.Size(802, 273);
		this.childrenPanel.TabIndex = 6;
		this.selectAllButton.Location = new System.Drawing.Point(449, 17);
		this.selectAllButton.Margin = new System.Windows.Forms.Padding(4);
		this.selectAllButton.Name = "selectAllButton";
		this.selectAllButton.Size = new System.Drawing.Size(100, 28);
		this.selectAllButton.TabIndex = 4;
		this.selectAllButton.Text = "Select All";
		this.selectAllButton.UseVisualStyleBackColor = true;
		this.selectAllButton.Click += new System.EventHandler(SelectAllButton_Click);
		this.clearAllButton.Location = new System.Drawing.Point(557, 17);
		this.clearAllButton.Margin = new System.Windows.Forms.Padding(4);
		this.clearAllButton.Name = "clearAllButton";
		this.clearAllButton.Size = new System.Drawing.Size(100, 28);
		this.clearAllButton.TabIndex = 5;
		this.clearAllButton.Text = "Clear All";
		this.clearAllButton.UseVisualStyleBackColor = true;
		this.clearAllButton.Click += new System.EventHandler(ClearAllButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(836, 377);
		base.Controls.Add(this.clearAllButton);
		base.Controls.Add(this.selectAllButton);
		base.Controls.Add(this.childrenPanel);
		base.Controls.Add(this.moveDownButton);
		base.Controls.Add(this.moveUpButton);
		base.Controls.Add(this.removeButton);
		base.Controls.Add(this.addButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.cancelButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "DateTimeConditionCollectionEditorForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Date Time Condition Collection Editor";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(DateTimeConditionCollectionEditorForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(DateTimeConditionCollectionEditorForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
