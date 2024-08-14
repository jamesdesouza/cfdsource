using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class SplashForm : Form
{
	private IContainer components;

	private PictureBox splashPictureBox;

	private Label lblTitle;

	private Label lblVersion;

	private Label lblCopyright;

	private Label lblCompany;

	private Label lblDescription;

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

	public SplashForm()
	{
		InitializeComponent();
		lblTitle.Parent = splashPictureBox;
		lblVersion.Parent = splashPictureBox;
		lblCopyright.Parent = splashPictureBox;
		lblCompany.Parent = splashPictureBox;
		lblDescription.Parent = splashPictureBox;
		lblTitle.Text = AssemblyProduct;
		lblVersion.Text = string.Format("{0} {1}", LocalizedResourceMgr.GetString("AboutForm.VersionBegin"), AssemblyVersion);
		lblCopyright.Text = AssemblyCopyright;
		lblCompany.Text = AssemblyCompany;
		lblDescription.Text = AssemblyDescription;
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
		new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Forms.SplashForm));
		this.splashPictureBox = new System.Windows.Forms.PictureBox();
		this.lblTitle = new System.Windows.Forms.Label();
		this.lblVersion = new System.Windows.Forms.Label();
		this.lblCopyright = new System.Windows.Forms.Label();
		this.lblCompany = new System.Windows.Forms.Label();
		this.lblDescription = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.splashPictureBox).BeginInit();
		base.SuspendLayout();
		this.splashPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.splashPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splashPictureBox.Image = TCX.CFD.Properties.Resources._3CX_Splash;
		this.splashPictureBox.InitialImage = TCX.CFD.Properties.Resources._3CX_Splash;
		this.splashPictureBox.Location = new System.Drawing.Point(0, 0);
		this.splashPictureBox.Name = "splashPictureBox";
		this.splashPictureBox.Size = new System.Drawing.Size(503, 313);
		this.splashPictureBox.TabIndex = 0;
		this.splashPictureBox.TabStop = false;
		this.lblTitle.BackColor = System.Drawing.Color.Transparent;
		this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.Black;
		this.lblTitle.Location = new System.Drawing.Point(168, 9);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(325, 47);
		this.lblTitle.TabIndex = 1;
		this.lblTitle.Text = "3CX Call Flow Designer";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblVersion.AutoSize = true;
		this.lblVersion.BackColor = System.Drawing.Color.Transparent;
		this.lblVersion.Location = new System.Drawing.Point(169, 143);
		this.lblVersion.Name = "lblVersion";
		this.lblVersion.Size = new System.Drawing.Size(42, 13);
		this.lblVersion.TabIndex = 2;
		this.lblVersion.Text = "Version";
		this.lblCopyright.AutoSize = true;
		this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
		this.lblCopyright.Location = new System.Drawing.Point(169, 163);
		this.lblCopyright.Name = "lblCopyright";
		this.lblCopyright.Size = new System.Drawing.Size(51, 13);
		this.lblCopyright.TabIndex = 3;
		this.lblCopyright.Text = "Copyright";
		this.lblCompany.AutoSize = true;
		this.lblCompany.BackColor = System.Drawing.Color.Transparent;
		this.lblCompany.Location = new System.Drawing.Point(169, 183);
		this.lblCompany.Name = "lblCompany";
		this.lblCompany.Size = new System.Drawing.Size(51, 13);
		this.lblCompany.TabIndex = 4;
		this.lblCompany.Text = "Company";
		this.lblDescription.AutoSize = true;
		this.lblDescription.BackColor = System.Drawing.Color.Transparent;
		this.lblDescription.Location = new System.Drawing.Point(169, 203);
		this.lblDescription.MaximumSize = new System.Drawing.Size(300, 200);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(60, 13);
		this.lblDescription.TabIndex = 5;
		this.lblDescription.Text = "Description";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(503, 313);
		base.Controls.Add(this.lblDescription);
		base.Controls.Add(this.lblCompany);
		base.Controls.Add(this.lblCopyright);
		base.Controls.Add(this.lblVersion);
		base.Controls.Add(this.lblTitle);
		base.Controls.Add(this.splashPictureBox);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Name = "SplashForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		((System.ComponentModel.ISupportInitialize)this.splashPictureBox).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
