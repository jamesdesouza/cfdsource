using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ErrorListControl : UserControl
{
	private readonly List<ErrorDescriptor> errorDescriptorList = new List<ErrorDescriptor>();

	private IContainer components;

	private ListView errorListView;

	private ColumnHeader numberColumnHeader;

	private ColumnHeader typeColumnHeader;

	private ColumnHeader descriptionColumnHeader;

	private ColumnHeader fileColumnHeader;

	private ImageList errorTypeImageList;

	private ToolStrip filterButtonsToolStrip;

	private ToolStripButton enableErrorsToolStripButton;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton enableWarningsToolStripButton;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton enableMessagesToolStripButton;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem toolStripMenuItemCopy;

	public uint ErrorCount { get; private set; }

	public uint WarningCount { get; private set; }

	public uint MessageCount { get; private set; }

	public uint TotalErrorCount => ErrorCount + WarningCount + MessageCount;

	public ErrorListControl()
	{
		InitializeComponent();
		enableErrorsToolStripButton.Text = string.Format("{0} {1}", ErrorCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Errors.Text"));
		enableErrorsToolStripButton.ToolTipText = LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Errors.Tooltip");
		enableWarningsToolStripButton.Text = string.Format("{0} {1}", WarningCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Warnings.Text"));
		enableWarningsToolStripButton.ToolTipText = LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Warnings.Tooltip");
		enableMessagesToolStripButton.Text = string.Format("{0} {1}", MessageCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Messages.Text"));
		enableMessagesToolStripButton.ToolTipText = LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Messages.Tooltip");
		toolStripMenuItemCopy.Text = LocalizedResourceMgr.GetString("ErrorListControl.toolStripMenuItemCopy.Text");
		numberColumnHeader.Text = LocalizedResourceMgr.GetString("ErrorListControl.ColumnHeaders.Number");
		typeColumnHeader.Text = LocalizedResourceMgr.GetString("ErrorListControl.ColumnHeaders.Type");
		descriptionColumnHeader.Text = LocalizedResourceMgr.GetString("ErrorListControl.ColumnHeaders.Description");
		fileColumnHeader.Text = LocalizedResourceMgr.GetString("ErrorListControl.ColumnHeaders.File");
	}

	public void ClearErrorList()
	{
		ErrorDescriptor.Reset();
		errorDescriptorList.Clear();
		errorListView.Items.Clear();
		ErrorCount = 0u;
		WarningCount = 0u;
		MessageCount = 0u;
		enableErrorsToolStripButton.Text = string.Format("{0} {1}", ErrorCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Errors.Text"));
		enableWarningsToolStripButton.Text = string.Format("{0} {1}", WarningCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Warnings.Text"));
		enableMessagesToolStripButton.Text = string.Format("{0} {1}", MessageCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Messages.Text"));
	}

	public void AddErrorDescriptor(ErrorDescriptor errorDescriptor)
	{
		errorDescriptorList.Add(errorDescriptor);
		switch (errorDescriptor.ErrorType)
		{
		case CompilerMessageTypes.Error:
		{
			uint messageCount = ErrorCount + 1;
			ErrorCount = messageCount;
			enableErrorsToolStripButton.Text = string.Format("{0} {1}", ErrorCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Errors.Text"));
			if (enableErrorsToolStripButton.Checked)
			{
				errorListView.Items.Add(errorDescriptor.ToListViewItem());
			}
			break;
		}
		case CompilerMessageTypes.Warning:
		{
			uint messageCount = WarningCount + 1;
			WarningCount = messageCount;
			enableWarningsToolStripButton.Text = string.Format("{0} {1}", WarningCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Warnings.Text"));
			if (enableWarningsToolStripButton.Checked)
			{
				errorListView.Items.Add(errorDescriptor.ToListViewItem());
			}
			break;
		}
		case CompilerMessageTypes.Message:
		{
			uint messageCount = MessageCount + 1;
			MessageCount = messageCount;
			enableMessagesToolStripButton.Text = string.Format("{0} {1}", MessageCount, LocalizedResourceMgr.GetString("ErrorListControl.EnableButtons.Messages.Text"));
			if (enableMessagesToolStripButton.Checked)
			{
				errorListView.Items.Add(errorDescriptor.ToListViewItem());
			}
			break;
		}
		default:
			throw new InvalidEnumArgumentException(string.Format(LocalizedResourceMgr.GetString("ErrorListControl.Error.InvalidErrorType"), errorDescriptor.ErrorType));
		}
		if (errorListView.Items.Count > 0 && errorListView.SelectedItems.Count == 0)
		{
			errorListView.Items[0].Selected = true;
		}
	}

	private void EnableErrorsToolStripButton_Click(object sender, EventArgs e)
	{
		if (enableErrorsToolStripButton.Checked)
		{
			foreach (ErrorDescriptor errorDescriptor in errorDescriptorList)
			{
				if (errorDescriptor.ErrorType == CompilerMessageTypes.Error)
				{
					errorListView.Items.Add(errorDescriptor.ToListViewItem());
				}
			}
			return;
		}
		for (int i = 0; i < errorListView.Items.Count; i++)
		{
			if (errorListView.Items[i].SubItems[1].Text == LocalizedResourceMgr.GetString("ErrorListControl.ErrorTypes.Error"))
			{
				errorListView.Items[i].Remove();
				i--;
			}
		}
	}

	private void EnableWarningsToolStripButton_Click(object sender, EventArgs e)
	{
		if (enableWarningsToolStripButton.Checked)
		{
			foreach (ErrorDescriptor errorDescriptor in errorDescriptorList)
			{
				if (errorDescriptor.ErrorType == CompilerMessageTypes.Warning)
				{
					errorListView.Items.Add(errorDescriptor.ToListViewItem());
				}
			}
			return;
		}
		for (int i = 0; i < errorListView.Items.Count; i++)
		{
			if (errorListView.Items[i].SubItems[1].Text == LocalizedResourceMgr.GetString("ErrorListControl.ErrorTypes.Warning"))
			{
				errorListView.Items[i].Remove();
				i--;
			}
		}
	}

	private void EnableMessagesToolStripButton_Click(object sender, EventArgs e)
	{
		if (enableMessagesToolStripButton.Checked)
		{
			foreach (ErrorDescriptor errorDescriptor in errorDescriptorList)
			{
				if (errorDescriptor.ErrorType == CompilerMessageTypes.Message)
				{
					errorListView.Items.Add(errorDescriptor.ToListViewItem());
				}
			}
			return;
		}
		for (int i = 0; i < errorListView.Items.Count; i++)
		{
			if (errorListView.Items[i].SubItems[1].Text == LocalizedResourceMgr.GetString("ErrorListControl.ErrorTypes.Message"))
			{
				errorListView.Items[i].Remove();
				i--;
			}
		}
	}

	private void OpenFileForSelectedError()
	{
		foreach (ListViewItem selectedItem in errorListView.SelectedItems)
		{
			if (!(selectedItem.Tag is ErrorDescriptor { FileObject: not null } errorDescriptor))
			{
				continue;
			}
			try
			{
				errorDescriptor.FileObject.Open();
				errorDescriptor.FileObject.ChangeFlowType(errorDescriptor.FlowType);
				if (errorDescriptor.ErrorActivity != null)
				{
					errorDescriptor.FileObject.SelectActivity(errorDescriptor.ErrorActivity.Name, errorDescriptor.FlowType);
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ErrorListControl.MessageBox.Error.OpeningFile"), errorDescriptor.FileObject.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ErrorListControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
	}

	private void ErrorListView_DoubleClick(object sender, EventArgs e)
	{
		OpenFileForSelectedError();
	}

	private void ErrorListView_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			OpenFileForSelectedError();
		}
		else if (e.Control && e.KeyCode == Keys.C)
		{
			toolStripMenuItemCopy.PerformClick();
		}
	}

	private void ErrorListView_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right && errorListView.SelectedItems.Count > 0)
		{
			contextMenu.Show(errorListView, e.Location);
		}
	}

	private void ToolStripMenuItemCopy_Click(object sender, EventArgs e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ListViewItem selectedItem in errorListView.SelectedItems)
		{
			if (selectedItem.Tag is ErrorDescriptor errorDescriptor)
			{
				stringBuilder.AppendLine(errorDescriptor.ToCopiedText());
			}
		}
		string value = stringBuilder.ToString().Trim();
		if (!string.IsNullOrEmpty(value))
		{
			Clipboard.SetText(value);
		}
	}

	private void ErrorListControl_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/#h.9aspecbb78pm");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Controls.ErrorListControl));
		this.errorTypeImageList = new System.Windows.Forms.ImageList(this.components);
		this.errorListView = new System.Windows.Forms.ListView();
		this.numberColumnHeader = new System.Windows.Forms.ColumnHeader();
		this.typeColumnHeader = new System.Windows.Forms.ColumnHeader();
		this.descriptionColumnHeader = new System.Windows.Forms.ColumnHeader();
		this.fileColumnHeader = new System.Windows.Forms.ColumnHeader();
		this.filterButtonsToolStrip = new System.Windows.Forms.ToolStrip();
		this.enableErrorsToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.enableWarningsToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.enableMessagesToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
		this.filterButtonsToolStrip.SuspendLayout();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.errorTypeImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("errorTypeImageList.ImageStream");
		this.errorTypeImageList.TransparentColor = System.Drawing.Color.Magenta;
		this.errorTypeImageList.Images.SetKeyName(0, "Errors.png");
		this.errorTypeImageList.Images.SetKeyName(1, "Warning.png");
		this.errorTypeImageList.Images.SetKeyName(2, "Messages.png");
		this.errorListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.errorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.numberColumnHeader, this.typeColumnHeader, this.descriptionColumnHeader, this.fileColumnHeader });
		this.errorListView.FullRowSelect = true;
		this.errorListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.errorListView.HideSelection = false;
		this.errorListView.Location = new System.Drawing.Point(5, 41);
		this.errorListView.Margin = new System.Windows.Forms.Padding(4);
		this.errorListView.Name = "errorListView";
		this.errorListView.Size = new System.Drawing.Size(1093, 111);
		this.errorListView.SmallImageList = this.errorTypeImageList;
		this.errorListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
		this.errorListView.TabIndex = 3;
		this.errorListView.UseCompatibleStateImageBehavior = false;
		this.errorListView.View = System.Windows.Forms.View.Details;
		this.errorListView.DoubleClick += new System.EventHandler(ErrorListView_DoubleClick);
		this.errorListView.KeyDown += new System.Windows.Forms.KeyEventHandler(ErrorListView_KeyDown);
		this.errorListView.MouseUp += new System.Windows.Forms.MouseEventHandler(ErrorListView_MouseUp);
		this.numberColumnHeader.Text = "Number";
		this.numberColumnHeader.Width = 55;
		this.typeColumnHeader.Text = "Type";
		this.typeColumnHeader.Width = 58;
		this.descriptionColumnHeader.Text = "Description";
		this.descriptionColumnHeader.Width = 744;
		this.fileColumnHeader.Text = "File";
		this.fileColumnHeader.Width = 221;
		this.filterButtonsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.filterButtonsToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.filterButtonsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.enableErrorsToolStripButton, this.toolStripSeparator1, this.enableWarningsToolStripButton, this.toolStripSeparator2, this.enableMessagesToolStripButton });
		this.filterButtonsToolStrip.Location = new System.Drawing.Point(0, 0);
		this.filterButtonsToolStrip.Name = "filterButtonsToolStrip";
		this.filterButtonsToolStrip.Size = new System.Drawing.Size(1103, 27);
		this.filterButtonsToolStrip.TabIndex = 4;
		this.enableErrorsToolStripButton.Checked = true;
		this.enableErrorsToolStripButton.CheckOnClick = true;
		this.enableErrorsToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
		this.enableErrorsToolStripButton.Image = TCX.CFD.Properties.Resources.Errors;
		this.enableErrorsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.enableErrorsToolStripButton.Name = "enableErrorsToolStripButton";
		this.enableErrorsToolStripButton.Size = new System.Drawing.Size(83, 24);
		this.enableErrorsToolStripButton.Text = "0 Errors";
		this.enableErrorsToolStripButton.ToolTipText = "Show or hide errors";
		this.enableErrorsToolStripButton.Click += new System.EventHandler(EnableErrorsToolStripButton_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.enableWarningsToolStripButton.Checked = true;
		this.enableWarningsToolStripButton.CheckOnClick = true;
		this.enableWarningsToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
		this.enableWarningsToolStripButton.Image = TCX.CFD.Properties.Resources.Warning;
		this.enableWarningsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.enableWarningsToolStripButton.Name = "enableWarningsToolStripButton";
		this.enableWarningsToolStripButton.Size = new System.Drawing.Size(106, 24);
		this.enableWarningsToolStripButton.Text = "0 Warnings";
		this.enableWarningsToolStripButton.ToolTipText = "Show or hide warnings";
		this.enableWarningsToolStripButton.Click += new System.EventHandler(EnableWarningsToolStripButton_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.enableMessagesToolStripButton.Checked = true;
		this.enableMessagesToolStripButton.CheckOnClick = true;
		this.enableMessagesToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
		this.enableMessagesToolStripButton.Image = TCX.CFD.Properties.Resources.Messages;
		this.enableMessagesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.enableMessagesToolStripButton.Name = "enableMessagesToolStripButton";
		this.enableMessagesToolStripButton.Size = new System.Drawing.Size(109, 24);
		this.enableMessagesToolStripButton.Text = "0 Messages";
		this.enableMessagesToolStripButton.ToolTipText = "Show or hide messages";
		this.enableMessagesToolStripButton.Click += new System.EventHandler(EnableMessagesToolStripButton_Click);
		this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripMenuItemCopy });
		this.contextMenu.Name = "contextMenu";
		this.contextMenu.Size = new System.Drawing.Size(215, 58);
		this.toolStripMenuItemCopy.Image = TCX.CFD.Properties.Resources.Edit_Copy;
		this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
		this.toolStripMenuItemCopy.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
		this.toolStripMenuItemCopy.Size = new System.Drawing.Size(214, 26);
		this.toolStripMenuItemCopy.Text = "Copy";
		this.toolStripMenuItemCopy.Click += new System.EventHandler(ToolStripMenuItemCopy_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.filterButtonsToolStrip);
		base.Controls.Add(this.errorListView);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "ErrorListControl";
		base.Size = new System.Drawing.Size(1103, 156);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ErrorListControl_HelpRequested);
		this.filterButtonsToolStrip.ResumeLayout(false);
		this.filterButtonsToolStrip.PerformLayout();
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
