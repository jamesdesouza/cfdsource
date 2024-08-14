using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TCX.CFD.Classes;

namespace TCX.CFD.Controls;

public class ImportProjectFromBuildOutputWizardPage2 : UserControl, IWizardPage
{
	private IContainer components;

	private Label lblTitle;

	private Label lblValidation;

	private GroupBox grpBoxDestination;

	private Label lblOutputSourceHelp;

	private TextBox txtOutputFolder;

	private Button browseButton;

	private FolderBrowserDialog selectOutputFolderDialog;

	public string OutputFolder => txtOutputFolder.Text;

	public ImportProjectFromBuildOutputWizardPage2()
	{
		InitializeComponent();
		lblTitle.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage2.lblTitle.Text");
		grpBoxDestination.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage2.grpBoxDestination.Text");
		lblOutputSourceHelp.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage2.lblOutputSourceHelp.Text");
		browseButton.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage2.browseButton.Text");
	}

	public void FocusFirstControl()
	{
		browseButton.Focus();
	}

	public bool ValidateBeforeMovingToNext()
	{
		lblValidation.Visible = false;
		if (Directory.Exists(txtOutputFolder.Text))
		{
			return true;
		}
		lblValidation.Text = string.Format(LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage2.ValidationError.InvalidOutputFolder"), txtOutputFolder.Text);
		lblValidation.Visible = true;
		return false;
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		selectOutputFolderDialog.SelectedPath = txtOutputFolder.Text;
		if (selectOutputFolderDialog.ShowDialog() == DialogResult.OK)
		{
			txtOutputFolder.Text = selectOutputFolderDialog.SelectedPath;
		}
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		((TextBox)sender).SelectAll();
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
		this.lblTitle = new System.Windows.Forms.Label();
		this.lblValidation = new System.Windows.Forms.Label();
		this.grpBoxDestination = new System.Windows.Forms.GroupBox();
		this.browseButton = new System.Windows.Forms.Button();
		this.txtOutputFolder = new System.Windows.Forms.TextBox();
		this.lblOutputSourceHelp = new System.Windows.Forms.Label();
		this.selectOutputFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
		this.grpBoxDestination.SuspendLayout();
		base.SuspendLayout();
		this.lblTitle.AutoSize = true;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(58, 58);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(177, 26);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Configure Output";
		this.lblValidation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblValidation.ForeColor = System.Drawing.Color.Red;
		this.lblValidation.Location = new System.Drawing.Point(42, 368);
		this.lblValidation.Name = "lblValidation";
		this.lblValidation.Size = new System.Drawing.Size(671, 45);
		this.lblValidation.TabIndex = 3;
		this.lblValidation.Text = "You must select a valid folder";
		this.lblValidation.Visible = false;
		this.grpBoxDestination.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxDestination.Controls.Add(this.browseButton);
		this.grpBoxDestination.Controls.Add(this.txtOutputFolder);
		this.grpBoxDestination.Controls.Add(this.lblOutputSourceHelp);
		this.grpBoxDestination.Controls.Add(this.lblValidation);
		this.grpBoxDestination.Location = new System.Drawing.Point(58, 125);
		this.grpBoxDestination.Name = "grpBoxDestination";
		this.grpBoxDestination.Size = new System.Drawing.Size(821, 421);
		this.grpBoxDestination.TabIndex = 1;
		this.grpBoxDestination.TabStop = false;
		this.grpBoxDestination.Text = "Destination";
		this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.browseButton.Location = new System.Drawing.Point(613, 53);
		this.browseButton.Margin = new System.Windows.Forms.Padding(4);
		this.browseButton.Name = "browseButton";
		this.browseButton.Size = new System.Drawing.Size(100, 28);
		this.browseButton.TabIndex = 2;
		this.browseButton.Text = "&Browse";
		this.browseButton.UseVisualStyleBackColor = true;
		this.browseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.txtOutputFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtOutputFolder.Location = new System.Drawing.Point(42, 56);
		this.txtOutputFolder.Name = "txtOutputFolder";
		this.txtOutputFolder.Size = new System.Drawing.Size(564, 22);
		this.txtOutputFolder.TabIndex = 1;
		this.txtOutputFolder.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblOutputSourceHelp.AutoSize = true;
		this.lblOutputSourceHelp.Location = new System.Drawing.Point(39, 32);
		this.lblOutputSourceHelp.Name = "lblOutputSourceHelp";
		this.lblOutputSourceHelp.Size = new System.Drawing.Size(416, 17);
		this.lblOutputSourceHelp.TabIndex = 0;
		this.lblOutputSourceHelp.Text = "Select the folder in which you want to store your new CFD project";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.grpBoxDestination);
		base.Controls.Add(this.lblTitle);
		base.Name = "ImportProjectFromBuildOutputWizardPage2";
		base.Size = new System.Drawing.Size(952, 601);
		this.grpBoxDestination.ResumeLayout(false);
		this.grpBoxDestination.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
