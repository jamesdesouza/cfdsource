using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class SurveyQuestionCollectionEditorControl : UserControl
{
	private readonly IVadActivity activity;

	private IContainer components;

	private Button addButton;

	private Button removeButton;

	private Button moveUpButton;

	private Button moveDownButton;

	private Panel childrenPanel;

	private Button selectAllButton;

	private Button clearAllButton;

	public List<SurveyQuestion> SurveyQuestionList
	{
		get
		{
			List<SurveyQuestion> list = new List<SurveyQuestion>();
			foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
			{
				SurveyQuestion item = control.Save();
				list.Add(item);
			}
			return list;
		}
		set
		{
			foreach (SurveyQuestion item in value)
			{
				SurveyQuestion surveyQuestion = item.Clone();
				surveyQuestion.ContainerActivity = activity;
				AddSurveyQuestion(surveyQuestion);
			}
		}
	}

	private int GetNextControlVerticalLocation()
	{
		int num = 0;
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
		{
			num += control.Height;
		}
		return num;
	}

	private void AddSurveyQuestion(SurveyQuestion surveyQuestion)
	{
		SurveyQuestionEditorRowControl surveyQuestionEditorRowControl = new SurveyQuestionEditorRowControl(activity, surveyQuestion);
		surveyQuestionEditorRowControl.Location = new Point(0, GetNextControlVerticalLocation() - childrenPanel.VerticalScroll.Value);
		surveyQuestionEditorRowControl.CheckedChanged += RowControl_CheckedChanged;
		surveyQuestionEditorRowControl.HeightChanged += RowControl_HeightChanged;
		childrenPanel.Controls.Add(surveyQuestionEditorRowControl);
		ResizeRows();
		EnableDisableButtons();
	}

	private void ResizeRows()
	{
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
		{
			control.Size = new Size(childrenPanel.VerticalScroll.Visible ? (childrenPanel.Width - 20) : (childrenPanel.Width - 6), control.Height);
		}
	}

	private void RelocateRowControls()
	{
		int num = 0;
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
		{
			control.Location = new Point(0, num - childrenPanel.VerticalScroll.Value);
			num += control.Height;
		}
	}

	private void EnableDisableButtons()
	{
		bool flag = false;
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
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
			SurveyQuestionEditorRowControl surveyQuestionEditorRowControl = childrenPanel.Controls[0] as SurveyQuestionEditorRowControl;
			SurveyQuestionEditorRowControl surveyQuestionEditorRowControl2 = childrenPanel.Controls[childrenPanel.Controls.Count - 1] as SurveyQuestionEditorRowControl;
			moveUpButton.Enabled = !surveyQuestionEditorRowControl.IsChecked;
			moveDownButton.Enabled = !surveyQuestionEditorRowControl2.IsChecked;
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

	private void RowControl_HeightChanged(object sender, EventArgs e)
	{
		RelocateRowControls();
	}

	public SurveyQuestionCollectionEditorControl(IVadActivity activity)
	{
		InitializeComponent();
		this.activity = activity;
		addButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorControl.addButton.Text");
		removeButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorControl.removeButton.Text");
		moveUpButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorControl.moveUpButton.Text");
		moveDownButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorControl.moveDownButton.Text");
		selectAllButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorControl.selectAllButton.Text");
		clearAllButton.Text = LocalizedResourceMgr.GetString("SurveyQuestionCollectionEditorControl.clearAllButton.Text");
		EnableDisableButtons();
	}

	private void ChildrenPanel_Resize(object sender, EventArgs e)
	{
		ResizeRows();
	}

	private void AddButton_Click(object sender, EventArgs e)
	{
		AddSurveyQuestion(new YesNoSurveyQuestion
		{
			ContainerActivity = activity
		});
	}

	private void RemoveButton_Click(object sender, EventArgs e)
	{
		List<SurveyQuestionEditorRowControl> list = new List<SurveyQuestionEditorRowControl>();
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
		{
			if (control.IsChecked)
			{
				list.Add(control);
			}
		}
		foreach (SurveyQuestionEditorRowControl item in list)
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
			SurveyQuestionEditorRowControl surveyQuestionEditorRowControl = childrenPanel.Controls[i] as SurveyQuestionEditorRowControl;
			if (surveyQuestionEditorRowControl.IsChecked)
			{
				childrenPanel.Controls.SetChildIndex(surveyQuestionEditorRowControl, i - 1);
			}
		}
		RelocateRowControls();
		EnableDisableButtons();
	}

	private void MoveDownButton_Click(object sender, EventArgs e)
	{
		for (int num = childrenPanel.Controls.Count - 1; num >= 0; num--)
		{
			SurveyQuestionEditorRowControl surveyQuestionEditorRowControl = childrenPanel.Controls[num] as SurveyQuestionEditorRowControl;
			if (surveyQuestionEditorRowControl.IsChecked)
			{
				childrenPanel.Controls.SetChildIndex(surveyQuestionEditorRowControl, num + 1);
			}
		}
		RelocateRowControls();
		EnableDisableButtons();
	}

	private void SelectAllButton_Click(object sender, EventArgs e)
	{
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
		{
			control.IsChecked = true;
		}
	}

	private void ClearAllButton_Click(object sender, EventArgs e)
	{
		foreach (SurveyQuestionEditorRowControl control in childrenPanel.Controls)
		{
			control.IsChecked = false;
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
		this.addButton = new System.Windows.Forms.Button();
		this.removeButton = new System.Windows.Forms.Button();
		this.moveUpButton = new System.Windows.Forms.Button();
		this.moveDownButton = new System.Windows.Forms.Button();
		this.childrenPanel = new System.Windows.Forms.Panel();
		this.selectAllButton = new System.Windows.Forms.Button();
		this.clearAllButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
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
		this.childrenPanel.Size = new System.Drawing.Size(815, 320);
		this.childrenPanel.TabIndex = 6;
		this.childrenPanel.Resize += new System.EventHandler(ChildrenPanel_Resize);
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
		base.Controls.Add(this.clearAllButton);
		base.Controls.Add(this.selectAllButton);
		base.Controls.Add(this.childrenPanel);
		base.Controls.Add(this.moveDownButton);
		base.Controls.Add(this.moveUpButton);
		base.Controls.Add(this.removeButton);
		base.Controls.Add(this.addButton);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "SurveyQuestionCollectionEditorControl";
		base.Size = new System.Drawing.Size(836, 377);
		base.ResumeLayout(false);
	}
}
