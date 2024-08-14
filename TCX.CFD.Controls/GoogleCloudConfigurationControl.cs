using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Controls;

public class GoogleCloudConfigurationControl : UserControl
{
	public delegate void ConfigurationChangedHandler();

	private string jsonContent;

	private IContainer components;

	private Button browseButton;

	private TextBox txtServiceAccountKey;

	private Label lblServiceAccountKey;

	private OpenFileDialog openGoogleFileDialog;

	private ErrorProvider errorProvider;

	public GoogleCloudSettings GoogleCloudSettings
	{
		get
		{
			return new GoogleCloudSettings
			{
				ServiceAccountKeyFileName = txtServiceAccountKey.Text,
				ServiceAccountKeyJSON = jsonContent
			};
		}
		set
		{
			txtServiceAccountKey.Text = value.ServiceAccountKeyFileName;
			jsonContent = value.ServiceAccountKeyJSON;
		}
	}

	public event ConfigurationChangedHandler ConfigurationChanged;

	public GoogleCloudConfigurationControl()
	{
		InitializeComponent();
		lblServiceAccountKey.Text = LocalizedResourceMgr.GetString("GoogleCloudConfigurationControl.lblServiceAccountKey.Text");
		browseButton.Text = LocalizedResourceMgr.GetString("GoogleCloudConfigurationControl.browseButton.Text");
		openGoogleFileDialog.Title = LocalizedResourceMgr.GetString("GoogleCloudConfigurationControl.openGoogleFileDialog.Title");
	}

	private void Txt_Enter(object sender, EventArgs e)
	{
		((TextBox)sender).SelectAll();
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		if (openGoogleFileDialog.ShowDialog() == DialogResult.OK)
		{
			FileInfo fileInfo = new FileInfo(openGoogleFileDialog.FileName);
			txtServiceAccountKey.Text = fileInfo.Name;
			using (StreamReader streamReader = fileInfo.OpenText())
			{
				jsonContent = streamReader.ReadToEnd();
			}
			this.ConfigurationChanged?.Invoke();
		}
	}

	public bool ValidateSettings()
	{
		if (string.IsNullOrEmpty(txtServiceAccountKey.Text))
		{
			errorProvider.SetError(browseButton, LocalizedResourceMgr.GetString("GoogleCloudConfigurationControl.Error.ServiceAccountKeyFileMandatory"));
			return false;
		}
		return true;
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
		this.browseButton = new System.Windows.Forms.Button();
		this.txtServiceAccountKey = new System.Windows.Forms.TextBox();
		this.lblServiceAccountKey = new System.Windows.Forms.Label();
		this.openGoogleFileDialog = new System.Windows.Forms.OpenFileDialog();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.browseButton.Location = new System.Drawing.Point(536, 23);
		this.browseButton.Margin = new System.Windows.Forms.Padding(4);
		this.browseButton.Name = "browseButton";
		this.browseButton.Size = new System.Drawing.Size(100, 28);
		this.browseButton.TabIndex = 2;
		this.browseButton.Text = "&Browse";
		this.browseButton.UseVisualStyleBackColor = true;
		this.browseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.txtServiceAccountKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtServiceAccountKey.Location = new System.Drawing.Point(12, 26);
		this.txtServiceAccountKey.Name = "txtServiceAccountKey";
		this.txtServiceAccountKey.ReadOnly = true;
		this.txtServiceAccountKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtServiceAccountKey.Size = new System.Drawing.Size(517, 22);
		this.txtServiceAccountKey.TabIndex = 1;
		this.txtServiceAccountKey.Enter += new System.EventHandler(Txt_Enter);
		this.lblServiceAccountKey.AutoSize = true;
		this.lblServiceAccountKey.Location = new System.Drawing.Point(9, 6);
		this.lblServiceAccountKey.Name = "lblServiceAccountKey";
		this.lblServiceAccountKey.Size = new System.Drawing.Size(205, 17);
		this.lblServiceAccountKey.TabIndex = 0;
		this.lblServiceAccountKey.Text = "Service Account Key JSON File";
		this.openGoogleFileDialog.DefaultExt = "json";
		this.openGoogleFileDialog.Filter = "JSON files|*.json";
		this.openGoogleFileDialog.Title = "Select Service Account Key JSON File";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.browseButton);
		base.Controls.Add(this.txtServiceAccountKey);
		base.Controls.Add(this.lblServiceAccountKey);
		base.Name = "GoogleCloudConfigurationControl";
		base.Size = new System.Drawing.Size(660, 55);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
