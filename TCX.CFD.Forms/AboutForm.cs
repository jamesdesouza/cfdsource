using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

internal class AboutForm : Form
{
	private IContainer components;

	private TableLayoutPanel tableLayoutPanel;

	private PictureBox logoPictureBox;

	private Label labelProductName;

	private Label labelVersion;

	private Label labelCopyright;

	private Label labelCompanyName;

	private TextBox textBoxDescription;

	private Button okButton;

	public string AssemblyTitle
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), inherit: false);
			if (customAttributes.Length != 0)
			{
				AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
				if (assemblyTitleAttribute.Title != string.Empty)
				{
					return assemblyTitleAttribute.Title;
				}
			}
			return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
		}
	}

	public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

	public string AssemblyDescription
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return string.Empty;
			}
			return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
		}
	}

	public string AssemblyProduct
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return string.Empty;
			}
			return ((AssemblyProductAttribute)customAttributes[0]).Product;
		}
	}

	public string AssemblyCopyright
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return string.Empty;
			}
			return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
		}
	}

	public string AssemblyCompany
	{
		get
		{
			object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), inherit: false);
			if (customAttributes.Length == 0)
			{
				return string.Empty;
			}
			return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
		}
	}

	public AboutForm()
	{
		InitializeComponent();
		okButton.Text = LocalizedResourceMgr.GetString("AboutForm.OkButton.Text");
		Text = string.Format("{0} {1}", LocalizedResourceMgr.GetString("AboutForm.TitleBegin"), AssemblyTitle);
		labelProductName.Text = AssemblyProduct;
		labelVersion.Text = string.Format("{0} {1}", LocalizedResourceMgr.GetString("AboutForm.VersionBegin"), AssemblyVersion);
		labelCopyright.Text = AssemblyCopyright;
		labelCompanyName.Text = AssemblyCompany;
		textBoxDescription.Text = AssemblyDescription;
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
		this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
		this.logoPictureBox = new System.Windows.Forms.PictureBox();
		this.labelProductName = new System.Windows.Forms.Label();
		this.labelVersion = new System.Windows.Forms.Label();
		this.labelCopyright = new System.Windows.Forms.Label();
		this.labelCompanyName = new System.Windows.Forms.Label();
		this.textBoxDescription = new System.Windows.Forms.TextBox();
		this.okButton = new System.Windows.Forms.Button();
		this.tableLayoutPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.logoPictureBox).BeginInit();
		base.SuspendLayout();
		this.tableLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel.ColumnCount = 2;
		this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33f));
		this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67f));
		this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
		this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
		this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
		this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
		this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
		this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
		this.tableLayoutPanel.Location = new System.Drawing.Point(12, 11);
		this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
		this.tableLayoutPanel.Name = "tableLayoutPanel";
		this.tableLayoutPanel.RowCount = 5;
		this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111f));
		this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111f));
		this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111f));
		this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111f));
		this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556f));
		this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
		this.tableLayoutPanel.Size = new System.Drawing.Size(688, 395);
		this.tableLayoutPanel.TabIndex = 0;
		this.logoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.logoPictureBox.Image = TCX.CFD.Properties.Resources._3CX_About;
		this.logoPictureBox.Location = new System.Drawing.Point(4, 4);
		this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4);
		this.logoPictureBox.Name = "logoPictureBox";
		this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 5);
		this.logoPictureBox.Size = new System.Drawing.Size(216, 384);
		this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.logoPictureBox.TabIndex = 12;
		this.logoPictureBox.TabStop = false;
		this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.labelProductName.Location = new System.Drawing.Point(235, 0);
		this.labelProductName.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
		this.labelProductName.MaximumSize = new System.Drawing.Size(0, 21);
		this.labelProductName.Name = "labelProductName";
		this.labelProductName.Size = new System.Drawing.Size(449, 21);
		this.labelProductName.TabIndex = 19;
		this.labelProductName.Text = "Product Name";
		this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
		this.labelVersion.Location = new System.Drawing.Point(235, 43);
		this.labelVersion.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
		this.labelVersion.MaximumSize = new System.Drawing.Size(0, 21);
		this.labelVersion.Name = "labelVersion";
		this.labelVersion.Size = new System.Drawing.Size(449, 21);
		this.labelVersion.TabIndex = 0;
		this.labelVersion.Text = "Version";
		this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
		this.labelCopyright.Location = new System.Drawing.Point(235, 86);
		this.labelCopyright.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
		this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 21);
		this.labelCopyright.Name = "labelCopyright";
		this.labelCopyright.Size = new System.Drawing.Size(449, 21);
		this.labelCopyright.TabIndex = 21;
		this.labelCopyright.Text = "Copyright";
		this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.labelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
		this.labelCompanyName.Location = new System.Drawing.Point(235, 129);
		this.labelCompanyName.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
		this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 21);
		this.labelCompanyName.Name = "labelCompanyName";
		this.labelCompanyName.Size = new System.Drawing.Size(449, 21);
		this.labelCompanyName.TabIndex = 22;
		this.labelCompanyName.Text = "Company Name";
		this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBoxDescription.Location = new System.Drawing.Point(235, 176);
		this.textBoxDescription.Margin = new System.Windows.Forms.Padding(8, 4, 4, 4);
		this.textBoxDescription.Multiline = true;
		this.textBoxDescription.Name = "textBoxDescription";
		this.textBoxDescription.ReadOnly = true;
		this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBoxDescription.Size = new System.Drawing.Size(449, 215);
		this.textBoxDescription.TabIndex = 23;
		this.textBoxDescription.TabStop = false;
		this.textBoxDescription.Text = "Description";
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.okButton.Location = new System.Drawing.Point(600, 414);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 24;
		this.okButton.Text = "&OK";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(712, 457);
		base.Controls.Add(this.tableLayoutPanel);
		base.Controls.Add(this.okButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "AboutForm";
		base.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "About 3CX Call Flow Designer";
		this.tableLayoutPanel.ResumeLayout(false);
		this.tableLayoutPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.logoPictureBox).EndInit();
		base.ResumeLayout(false);
	}
}
