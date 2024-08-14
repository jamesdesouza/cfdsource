using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class OnlineServicesConfigurationForm : Form
{
	private readonly OnlineServicesTextToSpeechControl textToSpeechControl;

	private readonly OnlineServicesSpeechToTextControl speechToTextControl;

	private readonly AmazonPollyConfigurationControl amazonPollyConfigurationControl;

	private readonly GoogleCloudConfigurationControl googleCloudConfigurationControl;

	private readonly TreeNode textToSpeechTreeNode;

	private readonly TreeNode speechToTextTreeNode;

	private readonly Font regularFont;

	private readonly Font boldFont;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private TreeView servicesTreeView;

	private Panel engineSelectionPanel;

	private Panel engineSettingsPanel;

	public OnlineServices OnlineServices
	{
		get
		{
			return new OnlineServices
			{
				TextToSpeechEngine = textToSpeechControl.TextToSpeechEngine,
				SpeechToTextEngine = speechToTextControl.SpeechToTextEngine,
				AmazonPollySettings = amazonPollyConfigurationControl.AmazonPollySettings,
				GoogleCloudSettings = googleCloudConfigurationControl.GoogleCloudSettings
			};
		}
		set
		{
			textToSpeechControl.TextToSpeechEngine = value.TextToSpeechEngine;
			speechToTextControl.SpeechToTextEngine = value.SpeechToTextEngine;
			amazonPollyConfigurationControl.AmazonPollySettings = value.AmazonPollySettings;
			googleCloudConfigurationControl.GoogleCloudSettings = value.GoogleCloudSettings;
		}
	}

	public OnlineServicesConfigurationForm()
	{
		InitializeComponent();
		regularFont = new Font(servicesTreeView.Font, FontStyle.Regular);
		boldFont = new Font(servicesTreeView.Font, FontStyle.Bold);
		Text = LocalizedResourceMgr.GetString("OnlineServicesConfigurationForm.Title");
		okButton.Text = LocalizedResourceMgr.GetString("OnlineServicesConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("OnlineServicesConfigurationForm.cancelButton.Text");
		textToSpeechControl = new OnlineServicesTextToSpeechControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		textToSpeechControl.EngineChanged += TextToSpeechControl_EngineChanged;
		engineSelectionPanel.Controls.Add(textToSpeechControl);
		speechToTextControl = new OnlineServicesSpeechToTextControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		speechToTextControl.EngineChanged += SpeechToTextControl_EngineChanged;
		engineSelectionPanel.Controls.Add(speechToTextControl);
		amazonPollyConfigurationControl = new AmazonPollyConfigurationControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		engineSettingsPanel.Controls.Add(amazonPollyConfigurationControl);
		googleCloudConfigurationControl = new GoogleCloudConfigurationControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		engineSettingsPanel.Controls.Add(googleCloudConfigurationControl);
		TreeNode treeNode = new TreeNode(LocalizedResourceMgr.GetString("OnlineServicesConfigurationForm.Node.OnlineServices"));
		textToSpeechTreeNode = new TreeNode(LocalizedResourceMgr.GetString("OnlineServicesConfigurationForm.Node.TextToSpeech"));
		textToSpeechTreeNode.Tag = textToSpeechControl;
		speechToTextTreeNode = new TreeNode(LocalizedResourceMgr.GetString("OnlineServicesConfigurationForm.Node.SpeechToText"));
		speechToTextTreeNode.Tag = speechToTextControl;
		treeNode.Nodes.AddRange(new TreeNode[2] { textToSpeechTreeNode, speechToTextTreeNode });
		servicesTreeView.Nodes.AddRange(new TreeNode[1] { treeNode });
		servicesTreeView.SelectedNode = textToSpeechTreeNode;
		servicesTreeView.ExpandAll();
	}

	private void TextToSpeechControl_EngineChanged()
	{
		ShowEngineSettings(textToSpeechControl.TextToSpeechEngine);
	}

	private void SpeechToTextControl_EngineChanged()
	{
		ShowEngineSettings(speechToTextControl.SpeechToTextEngine);
	}

	private void ServicesTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
	{
		Color backColor = servicesTreeView.BackColor;
		Rectangle rectangle = new Rectangle(e.Bounds.Left, e.Bounds.Top, servicesTreeView.ClientSize.Width - e.Bounds.Left, e.Bounds.Height);
		e.Graphics.FillRectangle(new SolidBrush(backColor), rectangle);
		ControlPaint.DrawBorder(e.Graphics, rectangle, backColor, ButtonBorderStyle.None);
		e.Graphics.DrawString(e.Node.Text, ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected) ? boldFont : regularFont, new SolidBrush(servicesTreeView.ForeColor), e.Bounds.Left, e.Bounds.Top);
	}

	private void ServicesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		TreeNode selectedNode = servicesTreeView.SelectedNode;
		if (selectedNode.Tag == null)
		{
			foreach (Control control2 in engineSelectionPanel.Controls)
			{
				control2.Visible = false;
			}
			ShowEngineSettings(TextToSpeechEngines.None);
			return;
		}
		foreach (Control control3 in engineSelectionPanel.Controls)
		{
			if (control3 == selectedNode.Tag)
			{
				control3.Visible = true;
				if (control3 == textToSpeechControl)
				{
					ShowEngineSettings(textToSpeechControl.TextToSpeechEngine);
				}
				else if (control3 == speechToTextControl)
				{
					ShowEngineSettings(speechToTextControl.SpeechToTextEngine);
				}
				else
				{
					ShowEngineSettings(TextToSpeechEngines.None);
				}
			}
			else
			{
				control3.Visible = false;
			}
		}
	}

	private void ShowEngineSettings(TextToSpeechEngines engine)
	{
		amazonPollyConfigurationControl.Visible = engine == TextToSpeechEngines.AmazonPolly;
		googleCloudConfigurationControl.Visible = engine == TextToSpeechEngines.GoogleCloud;
	}

	private void ShowEngineSettings(SpeechToTextEngines engine)
	{
		amazonPollyConfigurationControl.Visible = false;
		googleCloudConfigurationControl.Visible = engine == SpeechToTextEngines.GoogleCloud;
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (textToSpeechControl.TextToSpeechEngine == TextToSpeechEngines.AmazonPolly && !amazonPollyConfigurationControl.ValidateSettings())
		{
			servicesTreeView.SelectedNode = textToSpeechTreeNode;
			return;
		}
		if (textToSpeechControl.TextToSpeechEngine == TextToSpeechEngines.GoogleCloud && !googleCloudConfigurationControl.ValidateSettings())
		{
			servicesTreeView.SelectedNode = textToSpeechTreeNode;
			return;
		}
		if (speechToTextControl.SpeechToTextEngine == SpeechToTextEngines.GoogleCloud && !googleCloudConfigurationControl.ValidateSettings())
		{
			servicesTreeView.SelectedNode = speechToTextTreeNode;
			return;
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.3lrgtl2kyif5");
	}

	private void OnlineServicesConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		ShowHelp();
	}

	private void OnlineServicesConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.servicesTreeView = new System.Windows.Forms.TreeView();
		this.engineSelectionPanel = new System.Windows.Forms.Panel();
		this.engineSettingsPanel = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(570, 272);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 4;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(462, 272);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 3;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.servicesTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.servicesTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
		this.servicesTreeView.HideSelection = false;
		this.servicesTreeView.Location = new System.Drawing.Point(13, 13);
		this.servicesTreeView.Margin = new System.Windows.Forms.Padding(4);
		this.servicesTreeView.Name = "servicesTreeView";
		this.servicesTreeView.Size = new System.Drawing.Size(196, 238);
		this.servicesTreeView.TabIndex = 0;
		this.servicesTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(ServicesTreeView_DrawNode);
		this.servicesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(ServicesTreeView_AfterSelect);
		this.engineSelectionPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.engineSelectionPanel.Location = new System.Drawing.Point(217, 13);
		this.engineSelectionPanel.Name = "engineSelectionPanel";
		this.engineSelectionPanel.Size = new System.Drawing.Size(461, 68);
		this.engineSelectionPanel.TabIndex = 1;
		this.engineSettingsPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.engineSettingsPanel.Location = new System.Drawing.Point(217, 85);
		this.engineSettingsPanel.Name = "engineSettingsPanel";
		this.engineSettingsPanel.Size = new System.Drawing.Size(461, 167);
		this.engineSettingsPanel.TabIndex = 2;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(690, 313);
		base.Controls.Add(this.engineSettingsPanel);
		base.Controls.Add(this.engineSelectionPanel);
		base.Controls.Add(this.servicesTreeView);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "OnlineServicesConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Online Services";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(OnlineServicesConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(OnlineServicesConfigurationForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
