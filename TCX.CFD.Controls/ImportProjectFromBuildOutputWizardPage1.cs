using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ImportProjectFromBuildOutputWizardPage1 : UserControl, IWizardPage
{
	private string scriptCode = "";

	private IContainer components;

	private Label lblTitle;

	private RadioButton rbZipFile;

	private Label lblValidation;

	private GroupBox grpBoxSource;

	private RadioButton rbText;

	private Label lblInputSourceHelp;

	private TextBox txtZipFilePath;

	private Button browseButton;

	private TextBox txtScript;

	private OpenFileDialog openZipFileDialog;

	public string ScriptCode => scriptCode;

	public ImportProjectFromBuildOutputWizardPage1()
	{
		InitializeComponent();
		lblTitle.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.lblTitle.Text");
		grpBoxSource.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.grpBoxSource.Text");
		rbZipFile.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.rbZipFile.Text");
		rbText.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.rbText.Text");
		browseButton.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.browseButton.Text");
		lblInputSourceHelp.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.lblInputSourceHelp.Text");
	}

	public void FocusFirstControl()
	{
		rbZipFile.Focus();
	}

	public bool ValidateBeforeMovingToNext()
	{
		lblValidation.Visible = false;
		if (rbZipFile.Checked)
		{
			if (new FileInfo(txtZipFilePath.Text).Exists)
			{
				return ValidateScriptFromZip();
			}
			lblValidation.Text = string.Format(LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.ValidationError.ZipFileNotExists"), txtZipFilePath.Text);
			lblValidation.Visible = true;
			return false;
		}
		scriptCode = txtScript.Text;
		return ValidateScriptData();
	}

	private bool ValidateScriptFromZip()
	{
		string text = txtZipFilePath.Text + "_files";
		try
		{
			if (Directory.Exists(text))
			{
				Directory.Delete(text, recursive: true);
			}
			ZipFile.ExtractToDirectory(txtZipFilePath.Text, text);
			string[] files = Directory.GetFiles(Path.Combine(text, "Sources"));
			if (files.Length == 1)
			{
				scriptCode = File.ReadAllText(files[0]);
				return ValidateScriptData();
			}
			lblValidation.Text = string.Format(LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.ValidationError.ZipFileIsInvalid"), txtZipFilePath.Text);
			lblValidation.Visible = true;
			return false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.MessageBox.Error.ProcessingZipFile"), ex.ToString()), LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		finally
		{
			Directory.Delete(text, recursive: true);
		}
	}

	private bool ValidateScriptData()
	{
		if (scriptCode.Contains("// ---Project File: "))
		{
			return true;
		}
		lblValidation.Text = LocalizedResourceMgr.GetString("ImportProjectFromBuildOutputWizardPage1.ValidationError.OldScriptVersion");
		lblValidation.Visible = true;
		return false;
	}

	private void RadioButtonMode_CheckedChanged(object sender, EventArgs e)
	{
		txtZipFilePath.Enabled = rbZipFile.Checked;
		browseButton.Enabled = rbZipFile.Checked;
		txtScript.Enabled = rbText.Checked;
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		openZipFileDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.LastOpenExistingProjectFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.LastOpenExistingProjectFolder);
		openZipFileDialog.FileName = txtZipFilePath.Text;
		if (openZipFileDialog.ShowDialog() == DialogResult.OK)
		{
			txtZipFilePath.Text = openZipFileDialog.FileName;
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
		this.rbZipFile = new System.Windows.Forms.RadioButton();
		this.lblValidation = new System.Windows.Forms.Label();
		this.grpBoxSource = new System.Windows.Forms.GroupBox();
		this.txtScript = new System.Windows.Forms.TextBox();
		this.browseButton = new System.Windows.Forms.Button();
		this.txtZipFilePath = new System.Windows.Forms.TextBox();
		this.lblInputSourceHelp = new System.Windows.Forms.Label();
		this.rbText = new System.Windows.Forms.RadioButton();
		this.openZipFileDialog = new System.Windows.Forms.OpenFileDialog();
		this.grpBoxSource.SuspendLayout();
		base.SuspendLayout();
		this.lblTitle.AutoSize = true;
		this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.Location = new System.Drawing.Point(58, 58);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(160, 26);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Configure Input";
		this.rbZipFile.AutoSize = true;
		this.rbZipFile.Checked = true;
		this.rbZipFile.Location = new System.Drawing.Point(20, 28);
		this.rbZipFile.Name = "rbZipFile";
		this.rbZipFile.Size = new System.Drawing.Size(463, 21);
		this.rbZipFile.TabIndex = 0;
		this.rbZipFile.TabStop = true;
		this.rbZipFile.Text = "Import the project from the ZIP file generated by the CFD during build";
		this.rbZipFile.UseVisualStyleBackColor = true;
		this.rbZipFile.CheckedChanged += new System.EventHandler(RadioButtonMode_CheckedChanged);
		this.lblValidation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblValidation.ForeColor = System.Drawing.Color.Red;
		this.lblValidation.Location = new System.Drawing.Point(42, 368);
		this.lblValidation.Name = "lblValidation";
		this.lblValidation.Size = new System.Drawing.Size(671, 45);
		this.lblValidation.TabIndex = 6;
		this.lblValidation.Text = "You must select a valid file";
		this.lblValidation.Visible = false;
		this.grpBoxSource.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxSource.Controls.Add(this.txtScript);
		this.grpBoxSource.Controls.Add(this.browseButton);
		this.grpBoxSource.Controls.Add(this.txtZipFilePath);
		this.grpBoxSource.Controls.Add(this.lblInputSourceHelp);
		this.grpBoxSource.Controls.Add(this.rbText);
		this.grpBoxSource.Controls.Add(this.rbZipFile);
		this.grpBoxSource.Controls.Add(this.lblValidation);
		this.grpBoxSource.Location = new System.Drawing.Point(58, 125);
		this.grpBoxSource.Name = "grpBoxSource";
		this.grpBoxSource.Size = new System.Drawing.Size(821, 421);
		this.grpBoxSource.TabIndex = 1;
		this.grpBoxSource.TabStop = false;
		this.grpBoxSource.Text = "Source";
		this.txtScript.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtScript.Enabled = false;
		this.txtScript.Location = new System.Drawing.Point(42, 123);
		this.txtScript.MaxLength = int.MaxValue;
		this.txtScript.Multiline = true;
		this.txtScript.Name = "txtScript";
		this.txtScript.Size = new System.Drawing.Size(671, 199);
		this.txtScript.TabIndex = 4;
		this.txtScript.Enter += new System.EventHandler(TxtBox_Enter);
		this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.browseButton.Location = new System.Drawing.Point(613, 53);
		this.browseButton.Margin = new System.Windows.Forms.Padding(4);
		this.browseButton.Name = "browseButton";
		this.browseButton.Size = new System.Drawing.Size(100, 28);
		this.browseButton.TabIndex = 2;
		this.browseButton.Text = "&Browse";
		this.browseButton.UseVisualStyleBackColor = true;
		this.browseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.txtZipFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtZipFilePath.Location = new System.Drawing.Point(42, 56);
		this.txtZipFilePath.Name = "txtZipFilePath";
		this.txtZipFilePath.Size = new System.Drawing.Size(564, 22);
		this.txtZipFilePath.TabIndex = 1;
		this.txtZipFilePath.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblInputSourceHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblInputSourceHelp.Location = new System.Drawing.Point(42, 343);
		this.lblInputSourceHelp.Name = "lblInputSourceHelp";
		this.lblInputSourceHelp.Size = new System.Drawing.Size(671, 24);
		this.lblInputSourceHelp.TabIndex = 5;
		this.lblInputSourceHelp.Text = "* The source must have been generated using the 3CX Call Flow Designer for 3CX v18 or later.";
		this.rbText.AutoSize = true;
		this.rbText.Location = new System.Drawing.Point(20, 96);
		this.rbText.Name = "rbText";
		this.rbText.Size = new System.Drawing.Size(435, 21);
		this.rbText.TabIndex = 3;
		this.rbText.Text = "Import the project from the C# code available in the 3CX Console";
		this.rbText.UseVisualStyleBackColor = true;
		this.rbText.CheckedChanged += new System.EventHandler(RadioButtonMode_CheckedChanged);
		this.openZipFileDialog.DefaultExt = "zip";
		this.openZipFileDialog.FileName = "ProjectOutput.zip";
		this.openZipFileDialog.Filter = "ZIP Files|*.zip";
		this.openZipFileDialog.RestoreDirectory = true;
		this.openZipFileDialog.SupportMultiDottedExtensions = true;
		this.openZipFileDialog.Title = "Select CFD Build Output File";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.grpBoxSource);
		base.Controls.Add(this.lblTitle);
		base.Name = "ImportProjectFromBuildOutputWizardPage1";
		base.Size = new System.Drawing.Size(952, 601);
		this.grpBoxSource.ResumeLayout(false);
		this.grpBoxSource.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
