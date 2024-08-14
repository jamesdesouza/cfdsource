using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class VoiceInputHintBuilderForm : Form
{
	private const int HintMargin = 25;

	private readonly string languageCode;

	private List<AbsVoiceInputHint> voiceInputHints;

	private bool hasChanges;

	private bool savingChanges;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private GroupBox grpBoxElements;

	private GroupBox grpBoxHint;

	private FlowLayoutPanel hintPanel;

	private RichTextBox txtHintHelp;

	private TreeView hintElementsTreeView;

	public string Hint
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (AbsVoiceInputHint voiceInputHint in voiceInputHints)
			{
				string text = voiceInputHint.GetText().Trim();
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(text + " ");
				}
			}
			return stringBuilder.ToString().Trim();
		}
		set
		{
			hintPanel.Controls.Clear();
			if (string.IsNullOrEmpty(value))
			{
				voiceInputHints = new List<AbsVoiceInputHint>();
				AddHintControl(new VoiceInputHintBuilderRowControl(languageCode));
			}
			else
			{
				voiceInputHints = AbsVoiceInputHint.BuildHints(languageCode, value);
				foreach (AbsVoiceInputHint voiceInputHint in voiceInputHints)
				{
					AddHintControl(new VoiceInputHintBuilderRowControl(languageCode, voiceInputHint));
				}
			}
			hasChanges = false;
		}
	}

	private void FillTreeView()
	{
		TreeNode treeNode = new TreeNode("Constant Values");
		treeNode.Nodes.Add(new TreeNode("String")
		{
			Tag = new VoiceInputHintConstantValue("")
		});
		hintElementsTreeView.Nodes.Add(treeNode);
		List<VoiceInputHintToken> tokensForLanguage = VoiceInputHintsHelper.GetTokensForLanguage(languageCode);
		if (tokensForLanguage.Count > 0)
		{
			TreeNode treeNode2 = new TreeNode("Tokens");
			foreach (VoiceInputHintToken item in tokensForLanguage)
			{
				treeNode2.Nodes.Add(new TreeNode(item.GetText())
				{
					Tag = item
				});
			}
			hintElementsTreeView.Nodes.Add(treeNode2);
		}
		hintElementsTreeView.ExpandAll();
	}

	private void HintElementsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		txtHintHelp.Clear();
		object obj = hintElementsTreeView.SelectedNode?.Tag;
		if (obj is AbsVoiceInputHint)
		{
			(obj as AbsVoiceInputHint).SetHelpText(txtHintHelp);
		}
	}

	private void HintElementsTreeView_ItemDrag(object sender, ItemDragEventArgs e)
	{
		if (e.Button == MouseButtons.Left && e.Item is TreeNode { Level: 1 } treeNode)
		{
			hintElementsTreeView.SelectedNode = treeNode;
			DoDragDrop(treeNode.FullPath, DragDropEffects.Copy | DragDropEffects.Move);
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		savingChanges = true;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	public VoiceInputHintBuilderForm(string languageCode)
	{
		InitializeComponent();
		this.languageCode = languageCode;
		FillTreeView();
		Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.Title");
		grpBoxHint.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.grpBoxHint.Text");
		grpBoxElements.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.grpBoxElements.Text");
		okButton.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.cancelButton.Text");
	}

	private void VoiceInputHintBuilderForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (!savingChanges && hasChanges && MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.MessageBox.Question.ConfirmCancel"), LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			e.Cancel = true;
		}
	}

	private void VoiceInputHintBuilderForm_Resize(object sender, EventArgs e)
	{
		foreach (Control control in hintPanel.Controls)
		{
			control.Width = hintPanel.Width - 25;
		}
	}

	private void AddHintControl(VoiceInputHintBuilderRowControl rowControl)
	{
		AddHintControl(rowControl, hintPanel.Controls.Count);
	}

	private void AddHintControl(VoiceInputHintBuilderRowControl rowControl, int controlIndex)
	{
		rowControl.Inserted += RowControl_Inserted;
		rowControl.Updated += RowControl_Updated;
		rowControl.Deleted += RowControl_Deleted;
		rowControl.Width = hintPanel.Width - 25;
		hintPanel.Controls.Add(rowControl);
		hintPanel.Controls.SetChildIndex(rowControl, controlIndex);
		ResetScrollbar();
		UpdateChildrenIndex();
	}

	private void UpdateChildrenIndex()
	{
		bool dragHandleVisible = hintPanel.Controls.Count > 1;
		for (int i = 0; i < hintPanel.Controls.Count; i++)
		{
			VoiceInputHintBuilderRowControl obj = hintPanel.Controls[i] as VoiceInputHintBuilderRowControl;
			obj.ControlIndex = i;
			obj.IsLastControl = i == hintPanel.Controls.Count - 1;
			obj.DragHandleVisible = dragHandleVisible;
			obj.ResizeChildren();
		}
	}

	private void ResetScrollbar()
	{
		hintPanel.AutoScroll = false;
		hintPanel.AutoScroll = true;
	}

	private void RowControl_Inserted(int controlIndex, AbsVoiceInputHint insertedHint, bool isFirstControl)
	{
		hasChanges = true;
		if (isFirstControl)
		{
			hintPanel.Controls.Clear();
		}
		voiceInputHints.Insert(controlIndex, insertedHint);
		AddHintControl(new VoiceInputHintBuilderRowControl(languageCode, insertedHint), controlIndex);
	}

	private void RowControl_Updated(int controlIndex, AbsVoiceInputHint updatedHint)
	{
		hasChanges = true;
		voiceInputHints[controlIndex] = updatedHint;
	}

	private void RowControl_Deleted(int controlIndex)
	{
		hasChanges = true;
		voiceInputHints.RemoveAt(controlIndex);
		hintPanel.Controls.RemoveAt(controlIndex);
		if (hintPanel.Controls.Count == 0)
		{
			AddHintControl(new VoiceInputHintBuilderRowControl(languageCode));
		}
		UpdateChildrenIndex();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.lgzfai7rclby");
	}

	private void VoiceInputHintBuilderForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void VoiceInputHintBuilderForm_HelpRequested(object sender, HelpEventArgs hlpevent)
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
		this.grpBoxElements = new System.Windows.Forms.GroupBox();
		this.hintElementsTreeView = new System.Windows.Forms.TreeView();
		this.txtHintHelp = new System.Windows.Forms.RichTextBox();
		this.grpBoxHint = new System.Windows.Forms.GroupBox();
		this.hintPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.grpBoxElements.SuspendLayout();
		this.grpBoxHint.SuspendLayout();
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
		this.grpBoxElements.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxElements.Controls.Add(this.hintElementsTreeView);
		this.grpBoxElements.Controls.Add(this.txtHintHelp);
		this.grpBoxElements.Location = new System.Drawing.Point(642, 13);
		this.grpBoxElements.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxElements.Name = "grpBoxElements";
		this.grpBoxElements.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxElements.Size = new System.Drawing.Size(584, 775);
		this.grpBoxElements.TabIndex = 4;
		this.grpBoxElements.TabStop = false;
		this.grpBoxElements.Text = "Elements";
		this.hintElementsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.hintElementsTreeView.Location = new System.Drawing.Point(4, 19);
		this.hintElementsTreeView.Name = "hintElementsTreeView";
		this.hintElementsTreeView.PathSeparator = ".";
		this.hintElementsTreeView.Size = new System.Drawing.Size(576, 597);
		this.hintElementsTreeView.TabIndex = 2;
		this.hintElementsTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(HintElementsTreeView_ItemDrag);
		this.hintElementsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(HintElementsTreeView_AfterSelect);
		this.txtHintHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHintHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.txtHintHelp.Location = new System.Drawing.Point(4, 616);
		this.txtHintHelp.Name = "txtHintHelp";
		this.txtHintHelp.ReadOnly = true;
		this.txtHintHelp.Size = new System.Drawing.Size(576, 155);
		this.txtHintHelp.TabIndex = 3;
		this.txtHintHelp.Text = "";
		this.grpBoxHint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxHint.Controls.Add(this.hintPanel);
		this.grpBoxHint.Location = new System.Drawing.Point(13, 13);
		this.grpBoxHint.Name = "grpBoxHint";
		this.grpBoxHint.Size = new System.Drawing.Size(622, 775);
		this.grpBoxHint.TabIndex = 8;
		this.grpBoxHint.TabStop = false;
		this.grpBoxHint.Text = "Hint";
		this.hintPanel.AutoScroll = true;
		this.hintPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.hintPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.hintPanel.Location = new System.Drawing.Point(3, 18);
		this.hintPanel.Name = "hintPanel";
		this.hintPanel.Size = new System.Drawing.Size(616, 754);
		this.hintPanel.TabIndex = 0;
		this.hintPanel.WrapContents = false;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(1232, 835);
		base.Controls.Add(this.grpBoxHint);
		base.Controls.Add(this.grpBoxElements);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(1250, 882);
		base.Name = "VoiceInputHintBuilderForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Hint Builder";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(VoiceInputHintBuilderForm_HelpButtonClicked);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(VoiceInputHintBuilderForm_FormClosing);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(VoiceInputHintBuilderForm_HelpRequested);
		base.Resize += new System.EventHandler(VoiceInputHintBuilderForm_Resize);
		this.grpBoxElements.ResumeLayout(false);
		this.grpBoxHint.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
