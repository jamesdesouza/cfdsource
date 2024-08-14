using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsCryptographyControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblKey;

	private TextBox txtKey;

	private ComboBox comboFormat;

	private Label lblFormat;

	private ComboBox comboAlgorithm;

	private Label lblAlgorithm;

	private ErrorProvider errorProvider;

	private Label lblEncryption;

	private CryptographyAlgorithms GetCryptographyAlgorithm(string str)
	{
		if (str == "3DES")
		{
			return CryptographyAlgorithms.TripleDES;
		}
		return CryptographyAlgorithms.HashMD5;
	}

	private string GetCryptographyAlgorithmAsStr(CryptographyAlgorithms algorithm)
	{
		if (algorithm == CryptographyAlgorithms.TripleDES)
		{
			return "3DES";
		}
		return "HashMD5";
	}

	public OptionsComponentsCryptographyControl()
	{
		InitializeComponent();
		comboAlgorithm.Items.AddRange(new object[2]
		{
			CryptographyAlgorithms.TripleDES,
			CryptographyAlgorithms.HashMD5
		});
		comboFormat.Items.AddRange(new object[2]
		{
			CodificationFormats.Hexadecimal,
			CodificationFormats.Base64
		});
		comboAlgorithm.SelectedItem = GetCryptographyAlgorithm(Settings.Default.CryptographyTemplateAlgorithm);
		comboFormat.SelectedItem = ((!(Settings.Default.CryptographyTemplateFormat == "Hexadecimal")) ? CodificationFormats.Base64 : CodificationFormats.Hexadecimal);
		txtKey.Text = Settings.Default.CryptographyTemplateKey;
		lblEncryption.Text = LocalizedResourceMgr.GetString("OptionsComponentsCryptographyControl.lblEncryption.Text");
		lblAlgorithm.Text = LocalizedResourceMgr.GetString("OptionsComponentsCryptographyControl.lblAlgorithm.Text");
		lblFormat.Text = LocalizedResourceMgr.GetString("OptionsComponentsCryptographyControl.lblFormat.Text");
		lblKey.Text = LocalizedResourceMgr.GetString("OptionsComponentsCryptographyControl.lblKey.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void ValidateKey()
	{
		if (txtKey.Enabled && !string.IsNullOrEmpty(txtKey.Text) && (CryptographyAlgorithms)comboAlgorithm.SelectedItem == CryptographyAlgorithms.TripleDES && txtKey.Text.Length != 24)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsCryptographyControl.Error.InvalidTripleDESKeyLength"));
		}
	}

	private void ValidateFields()
	{
		ValidateKey();
	}

	private void TxtKey_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateKey();
			errorProvider.SetError(txtKey, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtKey, ErrorHelper.GetErrorDescription(exc));
		}
	}

	private void ComboAlgorithm_SelectionChangeCommitted(object sender, EventArgs e)
	{
		txtKey.Enabled = (CryptographyAlgorithms)comboAlgorithm.SelectedItem != CryptographyAlgorithms.HashMD5;
		TxtKey_Validating(txtKey, new CancelEventArgs());
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.CryptographyTemplateAlgorithm = GetCryptographyAlgorithmAsStr((CryptographyAlgorithms)comboAlgorithm.SelectedItem);
		Settings.Default.CryptographyTemplateFormat = (((CodificationFormats)comboFormat.SelectedItem == CodificationFormats.Hexadecimal) ? "Hexadecimal" : "Base64");
		Settings.Default.CryptographyTemplateKey = txtKey.Text;
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
		this.lblKey = new System.Windows.Forms.Label();
		this.txtKey = new System.Windows.Forms.TextBox();
		this.comboFormat = new System.Windows.Forms.ComboBox();
		this.lblFormat = new System.Windows.Forms.Label();
		this.comboAlgorithm = new System.Windows.Forms.ComboBox();
		this.lblAlgorithm = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblEncryption = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblKey.AutoSize = true;
		this.lblKey.Location = new System.Drawing.Point(9, 136);
		this.lblKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblKey.Name = "lblKey";
		this.lblKey.Size = new System.Drawing.Size(32, 17);
		this.lblKey.TabIndex = 4;
		this.lblKey.Text = "Key";
		this.txtKey.Location = new System.Drawing.Point(12, 157);
		this.txtKey.Margin = new System.Windows.Forms.Padding(4);
		this.txtKey.MaxLength = 24;
		this.txtKey.Name = "txtKey";
		this.txtKey.Size = new System.Drawing.Size(300, 22);
		this.txtKey.TabIndex = 5;
		this.txtKey.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtKey.Validating += new System.ComponentModel.CancelEventHandler(TxtKey_Validating);
		this.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFormat.FormattingEnabled = true;
		this.comboFormat.Location = new System.Drawing.Point(12, 108);
		this.comboFormat.Margin = new System.Windows.Forms.Padding(4);
		this.comboFormat.Name = "comboFormat";
		this.comboFormat.Size = new System.Drawing.Size(300, 24);
		this.comboFormat.TabIndex = 3;
		this.lblFormat.AutoSize = true;
		this.lblFormat.Location = new System.Drawing.Point(9, 87);
		this.lblFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFormat.Name = "lblFormat";
		this.lblFormat.Size = new System.Drawing.Size(52, 17);
		this.lblFormat.TabIndex = 2;
		this.lblFormat.Text = "Format";
		this.comboAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboAlgorithm.FormattingEnabled = true;
		this.comboAlgorithm.Location = new System.Drawing.Point(12, 59);
		this.comboAlgorithm.Margin = new System.Windows.Forms.Padding(4);
		this.comboAlgorithm.Name = "comboAlgorithm";
		this.comboAlgorithm.Size = new System.Drawing.Size(300, 24);
		this.comboAlgorithm.TabIndex = 1;
		this.comboAlgorithm.SelectionChangeCommitted += new System.EventHandler(ComboAlgorithm_SelectionChangeCommitted);
		this.lblAlgorithm.AutoSize = true;
		this.lblAlgorithm.Location = new System.Drawing.Point(9, 38);
		this.lblAlgorithm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAlgorithm.Name = "lblAlgorithm";
		this.lblAlgorithm.Size = new System.Drawing.Size(67, 17);
		this.lblAlgorithm.TabIndex = 0;
		this.lblAlgorithm.Text = "Algorithm";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblEncryption.AutoSize = true;
		this.lblEncryption.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEncryption.Location = new System.Drawing.Point(8, 8);
		this.lblEncryption.Name = "lblEncryption";
		this.lblEncryption.Size = new System.Drawing.Size(98, 20);
		this.lblEncryption.TabIndex = 6;
		this.lblEncryption.Text = "Encryption";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblEncryption);
		base.Controls.Add(this.lblKey);
		base.Controls.Add(this.txtKey);
		base.Controls.Add(this.comboFormat);
		base.Controls.Add(this.lblFormat);
		base.Controls.Add(this.comboAlgorithm);
		base.Controls.Add(this.lblAlgorithm);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsCryptographyControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
