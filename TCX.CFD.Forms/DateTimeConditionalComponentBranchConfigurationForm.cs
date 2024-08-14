using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class DateTimeConditionalComponentBranchConfigurationForm : Form
{
	private readonly DateTimeConditionalComponentBranch dateTimeConditionalComponentBranch;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Button editDateTimeConditionsButton;

	private Label lblDidFilter;

	private ComboBox comboDidFilter;

	private Label lblDidFilterList;

	private TextBox txtDidFilterList;

	public DIDFilters DIDFilter => (DIDFilters)comboDidFilter.SelectedItem;

	public string DIDFilterList => txtDidFilterList.Text;

	public List<DateTimeCondition> DateTimeConditions { get; private set; }

	private void ComboDidFilter_SelectedIndexChanged(object sender, EventArgs e)
	{
		DIDFilters dIDFilters = (DIDFilters)comboDidFilter.SelectedItem;
		txtDidFilterList.Enabled = dIDFilters != DIDFilters.AllDIDs;
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void EditDateTimeConditionsButton_Click(object sender, EventArgs e)
	{
		DateTimeConditionCollectionEditorForm dateTimeConditionCollectionEditorForm = new DateTimeConditionCollectionEditorForm(dateTimeConditionalComponentBranch);
		dateTimeConditionCollectionEditorForm.DateTimeConditionList = DateTimeConditions;
		if (dateTimeConditionCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			DateTimeConditions = dateTimeConditionCollectionEditorForm.DateTimeConditionList;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	public DateTimeConditionalComponentBranchConfigurationForm(DateTimeConditionalComponentBranch dateTimeConditionalComponentBranch)
	{
		InitializeComponent();
		this.dateTimeConditionalComponentBranch = dateTimeConditionalComponentBranch;
		DateTimeConditions = new List<DateTimeCondition>(dateTimeConditionalComponentBranch.DateTimeConditions);
		comboDidFilter.Items.AddRange(new object[3]
		{
			DIDFilters.AllDIDs,
			DIDFilters.AllDIDsWithExceptions,
			DIDFilters.SpecificDIDs
		});
		comboDidFilter.SelectedItem = dateTimeConditionalComponentBranch.DIDFilter;
		txtDidFilterList.Text = dateTimeConditionalComponentBranch.DIDFilterList;
		Text = LocalizedResourceMgr.GetString("DateTimeConditionalComponentBranchConfigurationForm.Title");
		lblDidFilter.Text = LocalizedResourceMgr.GetString("DateTimeConditionalComponentBranchConfigurationForm.lblDidFilter.Text");
		lblDidFilterList.Text = LocalizedResourceMgr.GetString("DateTimeConditionalComponentBranchConfigurationForm.lblDidFilterList.Text");
		editDateTimeConditionsButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionalComponentBranchConfigurationForm.editDateTimeConditionsButton.Text");
		okButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionalComponentBranchConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("DateTimeConditionalComponentBranchConfigurationForm.cancelButton.Text");
	}

	private void DateTimeConditionalComponentBranchConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		dateTimeConditionalComponentBranch.ShowHelp();
	}

	private void DateTimeConditionalComponentBranchConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		dateTimeConditionalComponentBranch.ShowHelp();
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
		this.editDateTimeConditionsButton = new System.Windows.Forms.Button();
		this.lblDidFilter = new System.Windows.Forms.Label();
		this.comboDidFilter = new System.Windows.Forms.ComboBox();
		this.lblDidFilterList = new System.Windows.Forms.Label();
		this.txtDidFilterList = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(503, 124);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 6;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(395, 124);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 5;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.editDateTimeConditionsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.editDateTimeConditionsButton.Location = new System.Drawing.Point(20, 73);
		this.editDateTimeConditionsButton.Margin = new System.Windows.Forms.Padding(4);
		this.editDateTimeConditionsButton.Name = "editDateTimeConditionsButton";
		this.editDateTimeConditionsButton.Size = new System.Drawing.Size(587, 28);
		this.editDateTimeConditionsButton.TabIndex = 4;
		this.editDateTimeConditionsButton.Text = "Edit Date Time Conditions";
		this.editDateTimeConditionsButton.UseVisualStyleBackColor = true;
		this.editDateTimeConditionsButton.Click += new System.EventHandler(EditDateTimeConditionsButton_Click);
		this.lblDidFilter.AutoSize = true;
		this.lblDidFilter.Location = new System.Drawing.Point(16, 11);
		this.lblDidFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDidFilter.Name = "lblDidFilter";
		this.lblDidFilter.Size = new System.Drawing.Size(66, 17);
		this.lblDidFilter.TabIndex = 0;
		this.lblDidFilter.Text = "DID Filter";
		this.comboDidFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboDidFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboDidFilter.FormattingEnabled = true;
		this.comboDidFilter.Location = new System.Drawing.Point(117, 7);
		this.comboDidFilter.Margin = new System.Windows.Forms.Padding(4);
		this.comboDidFilter.Name = "comboDidFilter";
		this.comboDidFilter.Size = new System.Drawing.Size(484, 24);
		this.comboDidFilter.TabIndex = 1;
		this.comboDidFilter.SelectedIndexChanged += new System.EventHandler(ComboDidFilter_SelectedIndexChanged);
		this.lblDidFilterList.AutoSize = true;
		this.lblDidFilterList.Location = new System.Drawing.Point(16, 44);
		this.lblDidFilterList.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDidFilterList.Name = "lblDidFilterList";
		this.lblDidFilterList.Size = new System.Drawing.Size(92, 17);
		this.lblDidFilterList.TabIndex = 2;
		this.lblDidFilterList.Text = "DID Filter List";
		this.txtDidFilterList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDidFilterList.Location = new System.Drawing.Point(117, 41);
		this.txtDidFilterList.Margin = new System.Windows.Forms.Padding(4);
		this.txtDidFilterList.Name = "txtDidFilterList";
		this.txtDidFilterList.Size = new System.Drawing.Size(484, 22);
		this.txtDidFilterList.TabIndex = 3;
		this.txtDidFilterList.Enter += new System.EventHandler(TxtBox_Enter);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 158);
		base.Controls.Add(this.txtDidFilterList);
		base.Controls.Add(this.lblDidFilterList);
		base.Controls.Add(this.comboDidFilter);
		base.Controls.Add(this.lblDidFilter);
		base.Controls.Add(this.editDateTimeConditionsButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(794, 205);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 205);
		base.Name = "DateTimeConditionalComponentBranchConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Date Time Conditional Branch";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(DateTimeConditionalComponentBranchConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(DateTimeConditionalComponentBranchConfigurationForm_HelpRequested);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
