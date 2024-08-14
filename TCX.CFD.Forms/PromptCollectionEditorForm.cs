using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class PromptCollectionEditorForm : Form
{
	private readonly IVadActivity activity;

	private List<Prompt> promptList;

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

	public List<Prompt> PromptList
	{
		get
		{
			return promptList;
		}
		set
		{
			promptList = new List<Prompt>(value);
			foreach (Prompt prompt in promptList)
			{
				AddPrompt(prompt);
			}
		}
	}

	private void AddPrompt(Prompt prompt)
	{
		PromptEditorRowControl promptEditorRowControl = new PromptEditorRowControl(activity, prompt);
		promptEditorRowControl.Location = new Point(0, childrenPanel.AutoScrollPosition.Y + childrenPanel.Controls.Count * promptEditorRowControl.Height);
		promptEditorRowControl.CheckedChanged += RowControl_CheckedChanged;
		promptEditorRowControl.OnPlaybackStarted += RowControl_OnPlaybackStarted;
		promptEditorRowControl.OnPlaybackFinished += RowControl_OnPlaybackFinished;
		childrenPanel.Controls.Add(promptEditorRowControl);
		ResizeRows();
	}

	private void ResizeRows()
	{
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.Size = new Size(childrenPanel.VerticalScroll.Visible ? (childrenPanel.Width - 20) : (childrenPanel.Width - 6), control.Height);
		}
	}

	private void RelocateRowControls()
	{
		int num = 0;
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.Location = new Point(0, childrenPanel.AutoScrollPosition.Y + num++ * control.Height);
		}
	}

	private void EnableDisableButtons()
	{
		bool flag = false;
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
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
			PromptEditorRowControl promptEditorRowControl = childrenPanel.Controls[0] as PromptEditorRowControl;
			PromptEditorRowControl promptEditorRowControl2 = childrenPanel.Controls[childrenPanel.Controls.Count - 1] as PromptEditorRowControl;
			moveUpButton.Enabled = !promptEditorRowControl.IsChecked;
			moveDownButton.Enabled = !promptEditorRowControl2.IsChecked;
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

	private void RowControl_OnPlaybackStarted(object sender, EventArgs e)
	{
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.DisablePlayback(sender);
		}
	}

	private void RowControl_OnPlaybackFinished(object sender, EventArgs e)
	{
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.EnablePlayback();
		}
	}

	public PromptCollectionEditorForm(IVadActivity activity)
	{
		InitializeComponent();
		this.activity = activity;
		Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.Text");
		addButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.addButton.Text");
		removeButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.removeButton.Text");
		moveUpButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.moveUpButton.Text");
		moveDownButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.moveDownButton.Text");
		selectAllButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.selectAllButton.Text");
		clearAllButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.clearAllButton.Text");
		okButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("PromptCollectionEditorForm.cancelButton.Text");
	}

	private void AddButton_Click(object sender, EventArgs e)
	{
		AudioFilePrompt audioFilePrompt = new AudioFilePrompt();
		audioFilePrompt.ContainerActivity = activity;
		promptList.Add(audioFilePrompt);
		AddPrompt(audioFilePrompt);
	}

	private void RemoveButton_Click(object sender, EventArgs e)
	{
		List<PromptEditorRowControl> list = new List<PromptEditorRowControl>();
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			if (control.IsChecked)
			{
				list.Add(control);
			}
		}
		foreach (PromptEditorRowControl item in list)
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
			PromptEditorRowControl promptEditorRowControl = childrenPanel.Controls[i] as PromptEditorRowControl;
			if (promptEditorRowControl.IsChecked)
			{
				childrenPanel.Controls.SetChildIndex(promptEditorRowControl, i - 1);
			}
		}
		RelocateRowControls();
		EnableDisableButtons();
	}

	private void MoveDownButton_Click(object sender, EventArgs e)
	{
		for (int num = childrenPanel.Controls.Count - 1; num >= 0; num--)
		{
			PromptEditorRowControl promptEditorRowControl = childrenPanel.Controls[num] as PromptEditorRowControl;
			if (promptEditorRowControl.IsChecked)
			{
				childrenPanel.Controls.SetChildIndex(promptEditorRowControl, num + 1);
			}
		}
		RelocateRowControls();
		EnableDisableButtons();
	}

	private void SelectAllButton_Click(object sender, EventArgs e)
	{
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.IsChecked = true;
		}
	}

	private void ClearAllButton_Click(object sender, EventArgs e)
	{
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.IsChecked = false;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		promptList.Clear();
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.StopPlayback();
			Prompt prompt = control.Save();
			prompt.ContainerActivity = activity;
			promptList.Add(prompt);
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		foreach (PromptEditorRowControl control in childrenPanel.Controls)
		{
			control.StopPlayback();
		}
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-prompts-expressions/#h.3uoybbu5a4h");
	}

	private void PromptCollectionEditorForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void PromptCollectionEditorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
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
		this.addButton = new System.Windows.Forms.Button();
		this.removeButton = new System.Windows.Forms.Button();
		this.moveUpButton = new System.Windows.Forms.Button();
		this.moveDownButton = new System.Windows.Forms.Button();
		this.childrenPanel = new System.Windows.Forms.Panel();
		this.selectAllButton = new System.Windows.Forms.Button();
		this.clearAllButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.Location = new System.Drawing.Point(800, 334);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 8;
		this.cancelButton.Text = "Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(692, 334);
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
		this.childrenPanel.Size = new System.Drawing.Size(882, 273);
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
		base.ClientSize = new System.Drawing.Size(916, 377);
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
		base.Name = "PromptCollectionEditorForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Prompt Collection Editor";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(PromptCollectionEditorForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(PromptCollectionEditorForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
