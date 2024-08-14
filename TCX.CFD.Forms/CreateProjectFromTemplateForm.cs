using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;

namespace TCX.CFD.Forms;

public class CreateProjectFromTemplateForm : Form
{
	private bool isCallflowSelected = true;

	private readonly Font tabSelectedFont = new Font("Verdana", 10.2f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);

	private readonly Font tabUnselectedFont = new Font("Verdana", 10.2f, FontStyle.Regular, GraphicsUnit.Point, 0);

	private readonly List<ProjectTemplate> callflowTemplates = new List<ProjectTemplate>();

	private readonly List<ProjectTemplate> dialerTemplates = new List<ProjectTemplate>();

	private IContainer components;

	private Label lblChooseYourProject;

	private FlowLayoutPanel projectTypesFlowLayoutPanel;

	private Label lblDescription;

	private Label lblTitle;

	private Label lblCallflows;

	private Label lblDialers;

	private Button createButton;

	private Button cancelButton;

	private Panel mainPanel;

	public ProjectTemplate SelectedProjectTemplate { get; private set; }

	private void AddProjectType(ProjectTemplate template)
	{
		bool flag = projectTypesFlowLayoutPanel.Controls.Count == 0;
		ProjectTypeControl projectTypeControl = new ProjectTypeControl
		{
			Title = template.Title,
			Description = template.Description,
			ProjectImage = template.GetImage(),
			IsSelected = flag,
			Tag = template
		};
		if (flag)
		{
			SelectedProjectTemplate = template;
			lblTitle.Text = template.Title;
			lblDescription.Text = template.Description;
		}
		projectTypeControl.OnProjectSelected += ProjectTypeControl_OnProjectSelected;
		projectTypesFlowLayoutPanel.Controls.Add(projectTypeControl);
	}

	private void ProjectTypeControl_OnProjectSelected(object sender, EventArgs e)
	{
		ProjectTypeControl projectTypeControl = sender as ProjectTypeControl;
		foreach (ProjectTypeControl control in projectTypesFlowLayoutPanel.Controls)
		{
			control.IsSelected = control == projectTypeControl;
		}
		SelectedProjectTemplate = projectTypeControl.Tag as ProjectTemplate;
		lblTitle.Text = projectTypeControl.Title;
		lblDescription.Text = projectTypeControl.Description;
	}

	private void AddCallflowProjectTypes()
	{
		foreach (ProjectTemplate callflowTemplate in callflowTemplates)
		{
			AddProjectType(callflowTemplate);
		}
	}

	private void AddDialerProjectTypes()
	{
		foreach (ProjectTemplate dialerTemplate in dialerTemplates)
		{
			AddProjectType(dialerTemplate);
		}
	}

	private void LoadTemplates()
	{
		string xml = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Templates", "templates.xml"));
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XmlHelper.SanitizeXmlString(xml));
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().SelectSingleNode("/templates");
		if (xPathNavigator == null)
		{
			return;
		}
		XPathNodeIterator xPathNodeIterator = xPathNavigator.SelectChildren(XPathNodeType.Element);
		while (xPathNodeIterator.MoveNext())
		{
			ProjectTemplate item = new ProjectTemplate
			{
				ID = xPathNodeIterator.Current.GetAttribute("id", ""),
				Title = xPathNodeIterator.Current.GetAttribute("title", ""),
				Description = xPathNodeIterator.Current.GetAttribute("description", ""),
				Folder = xPathNodeIterator.Current.GetAttribute("folder", "")
			};
			if (xPathNodeIterator.Current.GetAttribute("type", "") == "callflow")
			{
				callflowTemplates.Add(item);
			}
			else
			{
				dialerTemplates.Add(item);
			}
		}
	}

	public CreateProjectFromTemplateForm()
	{
		InitializeComponent();
		Text = LocalizedResourceMgr.GetString("CreateProjectFromTemplateForm.Text");
		lblChooseYourProject.Text = LocalizedResourceMgr.GetString("CreateProjectFromTemplateForm.lblChooseYourProject.Text");
		lblCallflows.Text = LocalizedResourceMgr.GetString("CreateProjectFromTemplateForm.lblCallflows.Text");
		lblDialers.Text = LocalizedResourceMgr.GetString("CreateProjectFromTemplateForm.lblDialers.Text");
		createButton.Text = LocalizedResourceMgr.GetString("CreateProjectFromTemplateForm.createButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("CreateProjectFromTemplateForm.cancelButton.Text");
		LoadTemplates();
		AddCallflowProjectTypes();
	}

	private void LblCallflows_Click(object sender, EventArgs e)
	{
		if (!isCallflowSelected)
		{
			lblCallflows.Font = tabSelectedFont;
			lblDialers.Font = tabUnselectedFont;
			isCallflowSelected = true;
			projectTypesFlowLayoutPanel.Controls.Clear();
			AddCallflowProjectTypes();
		}
	}

	private void LblDialers_Click(object sender, EventArgs e)
	{
		if (isCallflowSelected)
		{
			lblCallflows.Font = tabUnselectedFont;
			lblDialers.Font = tabSelectedFont;
			isCallflowSelected = false;
			projectTypesFlowLayoutPanel.Controls.Clear();
			AddDialerProjectTypes();
		}
	}

	private void CreateButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-building-projects/#h.9suzlhr4nikv");
	}

	private void CreateProjectFromTemplateForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		ShowHelp();
	}

	private void CreateProjectFromTemplateForm_HelpButtonClicked(object sender, CancelEventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Forms.CreateProjectFromTemplateForm));
		this.lblChooseYourProject = new System.Windows.Forms.Label();
		this.projectTypesFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.lblCallflows = new System.Windows.Forms.Label();
		this.lblDialers = new System.Windows.Forms.Label();
		this.createButton = new System.Windows.Forms.Button();
		this.cancelButton = new System.Windows.Forms.Button();
		this.mainPanel = new System.Windows.Forms.Panel();
		this.mainPanel.SuspendLayout();
		base.SuspendLayout();
		this.lblChooseYourProject.AutoSize = true;
		this.lblChooseYourProject.Font = new System.Drawing.Font("Verdana", 14f);
		this.lblChooseYourProject.ForeColor = System.Drawing.Color.FromArgb(5, 151, 212);
		this.lblChooseYourProject.Location = new System.Drawing.Point(16, 19);
		this.lblChooseYourProject.Name = "lblChooseYourProject";
		this.lblChooseYourProject.Size = new System.Drawing.Size(256, 29);
		this.lblChooseYourProject.TabIndex = 1;
		this.lblChooseYourProject.Text = "Choose your project";
		this.projectTypesFlowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.projectTypesFlowLayoutPanel.AutoScroll = true;
		this.projectTypesFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.projectTypesFlowLayoutPanel.Location = new System.Drawing.Point(21, 99);
		this.projectTypesFlowLayoutPanel.Name = "projectTypesFlowLayoutPanel";
		this.projectTypesFlowLayoutPanel.Size = new System.Drawing.Size(1158, 380);
		this.projectTypesFlowLayoutPanel.TabIndex = 2;
		this.lblDescription.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblDescription.Font = new System.Drawing.Font("Verdana", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.lblDescription.Location = new System.Drawing.Point(18, 516);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(1161, 61);
		this.lblDescription.TabIndex = 12;
		this.lblDescription.Text = "Process inbound calls according to your business needs";
		this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblTitle.AutoSize = true;
		this.lblTitle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.Black;
		this.lblTitle.Location = new System.Drawing.Point(18, 491);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(71, 18);
		this.lblTitle.TabIndex = 11;
		this.lblTitle.Text = "Callflow";
		this.lblCallflows.AutoSize = true;
		this.lblCallflows.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCallflows.ForeColor = System.Drawing.Color.Black;
		this.lblCallflows.Location = new System.Drawing.Point(17, 71);
		this.lblCallflows.Name = "lblCallflows";
		this.lblCallflows.Size = new System.Drawing.Size(96, 20);
		this.lblCallflows.TabIndex = 13;
		this.lblCallflows.Text = "Callflows";
		this.lblCallflows.Click += new System.EventHandler(LblCallflows_Click);
		this.lblDialers.AutoSize = true;
		this.lblDialers.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDialers.ForeColor = System.Drawing.Color.Black;
		this.lblDialers.Location = new System.Drawing.Point(143, 71);
		this.lblDialers.Name = "lblDialers";
		this.lblDialers.Size = new System.Drawing.Size(68, 20);
		this.lblDialers.TabIndex = 14;
		this.lblDialers.Text = "Dialers";
		this.lblDialers.Click += new System.EventHandler(LblDialers_Click);
		this.createButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.createButton.Location = new System.Drawing.Point(971, 598);
		this.createButton.Margin = new System.Windows.Forms.Padding(4);
		this.createButton.Name = "createButton";
		this.createButton.Size = new System.Drawing.Size(100, 28);
		this.createButton.TabIndex = 16;
		this.createButton.Text = "C&reate";
		this.createButton.UseVisualStyleBackColor = true;
		this.createButton.Click += new System.EventHandler(CreateButton_Click);
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(1079, 598);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 17;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.mainPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mainPanel.Controls.Add(this.lblChooseYourProject);
		this.mainPanel.Controls.Add(this.projectTypesFlowLayoutPanel);
		this.mainPanel.Controls.Add(this.lblTitle);
		this.mainPanel.Controls.Add(this.lblDescription);
		this.mainPanel.Controls.Add(this.lblDialers);
		this.mainPanel.Controls.Add(this.lblCallflows);
		this.mainPanel.Location = new System.Drawing.Point(-1, -1);
		this.mainPanel.Name = "mainPanel";
		this.mainPanel.Size = new System.Drawing.Size(1198, 578);
		this.mainPanel.TabIndex = 18;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(1192, 639);
		base.Controls.Add(this.mainPanel);
		base.Controls.Add(this.createButton);
		base.Controls.Add(this.cancelButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "CreateProjectFromTemplateForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		this.Text = "Create Project from Template";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(CreateProjectFromTemplateForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(CreateProjectFromTemplateForm_HelpRequested);
		this.mainPanel.ResumeLayout(false);
		this.mainPanel.PerformLayout();
		base.ResumeLayout(false);
	}
}
