using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Controls;

public class AmazonPollyConfigurationControl : UserControl
{
	private IContainer components;

	private ComboBox comboRegion;

	private Label lblRegion;

	private Label lblClientSecret;

	private TextBox txtClientSecret;

	private TextBox txtClientID;

	private Label lblClientID;

	private ErrorProvider errorProvider;

	private TextBox txtLexicons;

	private Label lblLexicons;

	public AmazonPollySettings AmazonPollySettings
	{
		get
		{
			AmazonPollySettings amazonPollySettings = new AmazonPollySettings();
			amazonPollySettings.ClientID = txtClientID.Text;
			amazonPollySettings.ClientSecret = txtClientSecret.Text;
			amazonPollySettings.Region = (TextToSpeechAmazonRegions)comboRegion.SelectedItem;
			amazonPollySettings.Lexicons = new List<string>(txtLexicons.Text.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
			return amazonPollySettings;
		}
		set
		{
			txtClientID.Text = value.ClientID;
			txtClientSecret.Text = value.ClientSecret;
			comboRegion.SelectedItem = value.Region;
			txtLexicons.Text = string.Join(Environment.NewLine, value.Lexicons);
		}
	}

	public AmazonPollyConfigurationControl()
	{
		InitializeComponent();
		lblClientID.Text = LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.lblClientID.Text");
		lblClientSecret.Text = LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.lblClientSecret.Text");
		lblRegion.Text = LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.lblRegion.Text");
		lblLexicons.Text = LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.lblLexicons.Text");
		comboRegion.Items.AddRange(new object[25]
		{
			TextToSpeechAmazonRegions.AfricaCapeTown,
			TextToSpeechAmazonRegions.AsiaPacificHongKong,
			TextToSpeechAmazonRegions.AsiaPacificTokyo,
			TextToSpeechAmazonRegions.AsiaPacificSeoul,
			TextToSpeechAmazonRegions.AsiaPacificOsaka,
			TextToSpeechAmazonRegions.AsiaPacificMumbai,
			TextToSpeechAmazonRegions.AsiaPacificSingapore,
			TextToSpeechAmazonRegions.AsiaPacificSydney,
			TextToSpeechAmazonRegions.CanadaCentral,
			TextToSpeechAmazonRegions.ChinaBeijing,
			TextToSpeechAmazonRegions.ChinaNingxia,
			TextToSpeechAmazonRegions.EUFrankfurt,
			TextToSpeechAmazonRegions.EUStockholm,
			TextToSpeechAmazonRegions.EUIreland,
			TextToSpeechAmazonRegions.EULondon,
			TextToSpeechAmazonRegions.EUMilan,
			TextToSpeechAmazonRegions.EUParis,
			TextToSpeechAmazonRegions.MiddleEastBahrain,
			TextToSpeechAmazonRegions.SouthAmericaSaoPaulo,
			TextToSpeechAmazonRegions.USEastVirginia,
			TextToSpeechAmazonRegions.USEastOhio,
			TextToSpeechAmazonRegions.USEastGovCloudVirginia,
			TextToSpeechAmazonRegions.USWestGovCloudOregon,
			TextToSpeechAmazonRegions.USWestCalifornia,
			TextToSpeechAmazonRegions.USWestOregon
		});
	}

	private void Txt_Enter(object sender, EventArgs e)
	{
		((TextBox)sender).SelectAll();
	}

	private void TxtClientID_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtClientID, string.IsNullOrEmpty(txtClientID.Text) ? LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.Error.ClientIdMandatory") : string.Empty);
	}

	private void TxtClientSecret_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtClientSecret, string.IsNullOrEmpty(txtClientSecret.Text) ? LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.Error.ClientSecretMandatory") : string.Empty);
	}

	public bool ValidateSettings()
	{
		bool result = true;
		if (string.IsNullOrEmpty(txtClientID.Text))
		{
			errorProvider.SetError(txtClientID, LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.Error.ClientIdMandatory"));
			result = false;
		}
		if (string.IsNullOrEmpty(txtClientSecret.Text))
		{
			errorProvider.SetError(txtClientSecret, LocalizedResourceMgr.GetString("AmazonPollyConfigurationControl.Error.ClientSecretMandatory"));
			result = false;
		}
		return result;
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
		this.comboRegion = new System.Windows.Forms.ComboBox();
		this.lblRegion = new System.Windows.Forms.Label();
		this.lblClientSecret = new System.Windows.Forms.Label();
		this.txtClientSecret = new System.Windows.Forms.TextBox();
		this.txtClientID = new System.Windows.Forms.TextBox();
		this.lblClientID = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblLexicons = new System.Windows.Forms.Label();
		this.txtLexicons = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.comboRegion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboRegion.FormattingEnabled = true;
		this.comboRegion.Location = new System.Drawing.Point(116, 60);
		this.comboRegion.Margin = new System.Windows.Forms.Padding(4);
		this.comboRegion.Name = "comboRegion";
		this.comboRegion.Size = new System.Drawing.Size(496, 24);
		this.comboRegion.TabIndex = 5;
		this.lblRegion.AutoSize = true;
		this.lblRegion.Location = new System.Drawing.Point(9, 63);
		this.lblRegion.Name = "lblRegion";
		this.lblRegion.Size = new System.Drawing.Size(53, 17);
		this.lblRegion.TabIndex = 4;
		this.lblRegion.Text = "Region";
		this.lblClientSecret.AutoSize = true;
		this.lblClientSecret.Location = new System.Drawing.Point(9, 34);
		this.lblClientSecret.Name = "lblClientSecret";
		this.lblClientSecret.Size = new System.Drawing.Size(88, 17);
		this.lblClientSecret.TabIndex = 2;
		this.lblClientSecret.Text = "Client Secret";
		this.txtClientSecret.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtClientSecret.Location = new System.Drawing.Point(116, 31);
		this.txtClientSecret.Name = "txtClientSecret";
		this.txtClientSecret.Size = new System.Drawing.Size(496, 22);
		this.txtClientSecret.TabIndex = 3;
		this.txtClientSecret.Enter += new System.EventHandler(Txt_Enter);
		this.txtClientSecret.Validating += new System.ComponentModel.CancelEventHandler(TxtClientSecret_Validating);
		this.txtClientID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtClientID.Location = new System.Drawing.Point(116, 3);
		this.txtClientID.Name = "txtClientID";
		this.txtClientID.Size = new System.Drawing.Size(496, 22);
		this.txtClientID.TabIndex = 1;
		this.txtClientID.Enter += new System.EventHandler(Txt_Enter);
		this.txtClientID.Validating += new System.ComponentModel.CancelEventHandler(TxtClientID_Validating);
		this.lblClientID.AutoSize = true;
		this.lblClientID.Location = new System.Drawing.Point(9, 6);
		this.lblClientID.Name = "lblClientID";
		this.lblClientID.Size = new System.Drawing.Size(60, 17);
		this.lblClientID.TabIndex = 0;
		this.lblClientID.Text = "Client ID";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblLexicons.AutoSize = true;
		this.lblLexicons.Location = new System.Drawing.Point(9, 94);
		this.lblLexicons.Name = "lblLexicons";
		this.lblLexicons.Size = new System.Drawing.Size(63, 17);
		this.lblLexicons.TabIndex = 6;
		this.lblLexicons.Text = "Lexicons";
		this.txtLexicons.AcceptsReturn = true;
		this.txtLexicons.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtLexicons.Location = new System.Drawing.Point(116, 91);
		this.txtLexicons.Multiline = true;
		this.txtLexicons.Name = "txtLexicons";
		this.txtLexicons.Size = new System.Drawing.Size(496, 75);
		this.txtLexicons.TabIndex = 7;
		this.txtLexicons.Enter += new System.EventHandler(Txt_Enter);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.txtLexicons);
		base.Controls.Add(this.lblLexicons);
		base.Controls.Add(this.comboRegion);
		base.Controls.Add(this.lblRegion);
		base.Controls.Add(this.lblClientSecret);
		base.Controls.Add(this.txtClientSecret);
		base.Controls.Add(this.txtClientID);
		base.Controls.Add(this.lblClientID);
		base.Name = "AmazonPollyConfigurationControl";
		base.Size = new System.Drawing.Size(636, 169);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
