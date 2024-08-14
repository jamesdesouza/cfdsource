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

public class OptionsForm : Form
{
	private readonly Font regularFont;

	private readonly Font boldFont;

	private IContainer components;

	private TreeView categoryTreeView;

	private Button cancelButton;

	private Button okButton;

	private Panel controlContainerPanel;

	private OptionsProjectsControl optionsProjectsControl;

	private OptionsComponentsMenuControl optionsComponentsMenuControl;

	private OptionsComponentsSurveyControl optionsComponentsSurveyControl;

	private OptionsComponentsTextToSpeechControl optionsComponentsTextToSpeechControl;

	private OptionsComponentsSpeechToTextControl optionsComponentsSpeechToTextControl;

	private OptionsComponentsPromptPlaybackControl optionsComponentsPromptPlaybackControl;

	private OptionsComponentsUserInputControl optionsComponentsUserInputControl;

	private OptionsComponentsVoiceInputControl optionsComponentsVoiceInputControl;

	private OptionsComponentsTranscribeAudioControl optionsComponentsTranscribeAudioControl;

	private OptionsComponentsAuthenticationControl optionsComponentsAuthenticationControl;

	private OptionsComponentsCreditCardControl optionsComponentsCreditCardControl;

	private OptionsComponentsRecordControl optionsComponentsRecordControl;

	private OptionsComponentsWebInteractionControl optionsComponentsWebInteractionControl;

	private OptionsComponentsWebServiceRestControl optionsComponentsWebServiceRestControl;

	private OptionsComponentsCryptographyControl optionsComponentsCryptographyControl;

	private OptionsComponentsFileManagementControl optionsComponentsFileManagementControl;

	private OptionsComponentsSocketClientControl optionsComponentsSocketClientControl;

	private OptionsComponentsEMailSenderControl optionsComponentsEMailSenderControl;

	private OptionsComponentsDatabaseAccessControl optionsComponentsDatabaseAccessControl;

	private OptionsUpdatesControl optionsUpdatesControl;

	private void HideOptionControls()
	{
		optionsProjectsControl.Visible = false;
		optionsComponentsTextToSpeechControl.Visible = false;
		optionsComponentsSpeechToTextControl.Visible = false;
		optionsComponentsMenuControl.Visible = false;
		optionsComponentsSurveyControl.Visible = false;
		optionsComponentsPromptPlaybackControl.Visible = false;
		optionsComponentsRecordControl.Visible = false;
		optionsComponentsUserInputControl.Visible = false;
		optionsComponentsVoiceInputControl.Visible = false;
		optionsComponentsAuthenticationControl.Visible = false;
		optionsComponentsCreditCardControl.Visible = false;
		optionsComponentsTranscribeAudioControl.Visible = false;
		optionsComponentsWebInteractionControl.Visible = false;
		optionsComponentsWebServiceRestControl.Visible = false;
		optionsComponentsCryptographyControl.Visible = false;
		optionsComponentsFileManagementControl.Visible = false;
		optionsComponentsSocketClientControl.Visible = false;
		optionsComponentsEMailSenderControl.Visible = false;
		optionsComponentsDatabaseAccessControl.Visible = false;
		optionsUpdatesControl.Visible = false;
	}

	public OptionsForm()
	{
		InitializeComponent();
		regularFont = new Font(categoryTreeView.Font, FontStyle.Regular);
		boldFont = new Font(categoryTreeView.Font, FontStyle.Bold);
		TreeNode treeNode = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.Category.Projects"))
		{
			Tag = optionsProjectsControl
		};
		TreeNode treeNode2 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.Category.Updates"))
		{
			Tag = optionsUpdatesControl
		};
		TreeNode treeNode3 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.Category.ComponentsTemplates"));
		TreeNode treeNode4 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsTextToSpeech"))
		{
			Tag = optionsComponentsTextToSpeechControl
		};
		TreeNode treeNode5 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsSpeechToText"))
		{
			Tag = optionsComponentsSpeechToTextControl
		};
		TreeNode treeNode6 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsMenu"))
		{
			Tag = optionsComponentsMenuControl
		};
		TreeNode treeNode7 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsUserInput"))
		{
			Tag = optionsComponentsUserInputControl
		};
		TreeNode treeNode8 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsVoiceInput"))
		{
			Tag = optionsComponentsVoiceInputControl
		};
		TreeNode treeNode9 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsAuthentication"))
		{
			Tag = optionsComponentsAuthenticationControl
		};
		TreeNode treeNode10 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsCreditCard"))
		{
			Tag = optionsComponentsCreditCardControl
		};
		TreeNode treeNode11 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsTranscribeAudio"))
		{
			Tag = optionsComponentsTranscribeAudioControl
		};
		TreeNode treeNode12 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsSurvey"))
		{
			Tag = optionsComponentsSurveyControl
		};
		TreeNode treeNode13 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsPromptPlayback"))
		{
			Tag = optionsComponentsPromptPlaybackControl
		};
		TreeNode treeNode14 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsRecord"))
		{
			Tag = optionsComponentsRecordControl
		};
		TreeNode treeNode15 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsCryptography"))
		{
			Tag = optionsComponentsCryptographyControl
		};
		TreeNode treeNode16 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsDatabaseAccess"))
		{
			Tag = optionsComponentsDatabaseAccessControl
		};
		TreeNode treeNode17 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsEMailSender"))
		{
			Tag = optionsComponentsEMailSenderControl
		};
		TreeNode treeNode18 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsFileManagement"))
		{
			Tag = optionsComponentsFileManagementControl
		};
		TreeNode treeNode19 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsSocketClient"))
		{
			Tag = optionsComponentsSocketClientControl
		};
		TreeNode treeNode20 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsWebInteraction"))
		{
			Tag = optionsComponentsWebInteractionControl
		};
		TreeNode treeNode21 = new TreeNode(LocalizedResourceMgr.GetString("OptionsForm.SubCategory.ComponentsWebServiceRest"))
		{
			Tag = optionsComponentsWebServiceRestControl
		};
		treeNode3.Nodes.AddRange(new TreeNode[18]
		{
			treeNode4, treeNode5, treeNode6, treeNode7, treeNode8, treeNode9, treeNode10, treeNode11, treeNode12, treeNode13,
			treeNode14, treeNode15, treeNode16, treeNode17, treeNode18, treeNode19, treeNode20, treeNode21
		});
		categoryTreeView.Nodes.AddRange(new TreeNode[3] { treeNode, treeNode2, treeNode3 });
		categoryTreeView.ExpandAll();
		categoryTreeView.SelectedNode = categoryTreeView.Nodes[0];
		Text = LocalizedResourceMgr.GetString("OptionsForm.Title");
		okButton.Text = LocalizedResourceMgr.GetString("OptionsForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("OptionsForm.cancelButton.Text");
	}

	private void CategoryTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
	{
		Color backColor = categoryTreeView.BackColor;
		Rectangle rectangle = new Rectangle(e.Bounds.Left, e.Bounds.Top, categoryTreeView.ClientSize.Width - e.Bounds.Left, e.Bounds.Height);
		e.Graphics.FillRectangle(new SolidBrush(backColor), rectangle);
		ControlPaint.DrawBorder(e.Graphics, rectangle, backColor, ButtonBorderStyle.None);
		e.Graphics.DrawString(e.Node.Text, ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected) ? boldFont : regularFont, new SolidBrush(categoryTreeView.ForeColor), e.Bounds.Left, e.Bounds.Top);
	}

	private void CategoryTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		HideOptionControls();
		if (categoryTreeView.SelectedNode.Tag is Control control)
		{
			control.Visible = true;
		}
	}

	private void SelectNodeForControl(IOptionsControl optionsControl)
	{
		foreach (TreeNode node in categoryTreeView.Nodes)
		{
			if (node.Tag == optionsControl)
			{
				categoryTreeView.SelectedNode = node;
				break;
			}
			foreach (TreeNode node2 in node.Nodes)
			{
				if (node2.Tag == optionsControl)
				{
					categoryTreeView.SelectedNode = node2;
					return;
				}
			}
		}
	}

	private void SpeechToText_GoogleCloudConfigurationChanged(GoogleCloudSettings settings)
	{
		optionsComponentsTextToSpeechControl.UpdateGoogleCloudConfiguration(settings);
	}

	private void TextToSpeech_GoogleCloudConfigurationChanged(GoogleCloudSettings settings)
	{
		optionsComponentsSpeechToTextControl.UpdateGoogleCloudConfiguration(settings);
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		IOptionsControl optionsControl = null;
		try
		{
			foreach (TreeNode node in categoryTreeView.Nodes)
			{
				optionsControl = node.Tag as IOptionsControl;
				optionsControl?.Save();
				foreach (TreeNode node2 in node.Nodes)
				{
					optionsControl = node2.Tag as IOptionsControl;
					optionsControl?.Save();
				}
			}
			Settings.Default.Save();
			base.DialogResult = DialogResult.OK;
			Close();
		}
		catch (Exception exc)
		{
			SelectNodeForControl(optionsControl);
			Settings.Default.Reload();
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("OptionsForm.MessageBox.Error.SavingSettings"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("OptionsForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.vdomow42y5db");
	}

	private void OptionsForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		ShowHelp();
	}

	private void OptionsForm_HelpButtonClicked(object sender, CancelEventArgs e)
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
		this.categoryTreeView = new System.Windows.Forms.TreeView();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.controlContainerPanel = new System.Windows.Forms.Panel();
		this.optionsComponentsWebInteractionControl = new TCX.CFD.Controls.OptionsComponentsWebInteractionControl();
		this.optionsComponentsWebServiceRestControl = new TCX.CFD.Controls.OptionsComponentsWebServiceRestControl();
		this.optionsComponentsUserInputControl = new TCX.CFD.Controls.OptionsComponentsUserInputControl();
		this.optionsComponentsVoiceInputControl = new TCX.CFD.Controls.OptionsComponentsVoiceInputControl();
		this.optionsComponentsTranscribeAudioControl = new TCX.CFD.Controls.OptionsComponentsTranscribeAudioControl();
		this.optionsComponentsAuthenticationControl = new TCX.CFD.Controls.OptionsComponentsAuthenticationControl();
		this.optionsComponentsCreditCardControl = new TCX.CFD.Controls.OptionsComponentsCreditCardControl();
		this.optionsComponentsRecordControl = new TCX.CFD.Controls.OptionsComponentsRecordControl();
		this.optionsComponentsMenuControl = new TCX.CFD.Controls.OptionsComponentsMenuControl();
		this.optionsComponentsSurveyControl = new TCX.CFD.Controls.OptionsComponentsSurveyControl();
		this.optionsComponentsTextToSpeechControl = new TCX.CFD.Controls.OptionsComponentsTextToSpeechControl();
		this.optionsComponentsSpeechToTextControl = new TCX.CFD.Controls.OptionsComponentsSpeechToTextControl();
		this.optionsComponentsPromptPlaybackControl = new TCX.CFD.Controls.OptionsComponentsPromptPlaybackControl();
		this.optionsComponentsCryptographyControl = new TCX.CFD.Controls.OptionsComponentsCryptographyControl();
		this.optionsComponentsFileManagementControl = new TCX.CFD.Controls.OptionsComponentsFileManagementControl();
		this.optionsComponentsSocketClientControl = new TCX.CFD.Controls.OptionsComponentsSocketClientControl();
		this.optionsComponentsEMailSenderControl = new TCX.CFD.Controls.OptionsComponentsEMailSenderControl();
		this.optionsComponentsDatabaseAccessControl = new TCX.CFD.Controls.OptionsComponentsDatabaseAccessControl();
		this.optionsProjectsControl = new TCX.CFD.Controls.OptionsProjectsControl();
		this.optionsUpdatesControl = new TCX.CFD.Controls.OptionsUpdatesControl();
		this.controlContainerPanel.SuspendLayout();
		base.SuspendLayout();
		this.categoryTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.categoryTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
		this.categoryTreeView.HideSelection = false;
		this.categoryTreeView.Location = new System.Drawing.Point(16, 15);
		this.categoryTreeView.Margin = new System.Windows.Forms.Padding(4);
		this.categoryTreeView.Name = "categoryTreeView";
		this.categoryTreeView.Size = new System.Drawing.Size(317, 666);
		this.categoryTreeView.TabIndex = 0;
		this.categoryTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(CategoryTreeView_DrawNode);
		this.categoryTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(CategoryTreeView_AfterSelect);
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(1024, 689);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 3;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(916, 689);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 2;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.controlContainerPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.controlContainerPanel.Controls.Add(this.optionsComponentsWebInteractionControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsWebServiceRestControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsUserInputControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsVoiceInputControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsTranscribeAudioControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsAuthenticationControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsCreditCardControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsRecordControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsMenuControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsSurveyControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsTextToSpeechControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsSpeechToTextControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsPromptPlaybackControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsCryptographyControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsFileManagementControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsSocketClientControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsEMailSenderControl);
		this.controlContainerPanel.Controls.Add(this.optionsComponentsDatabaseAccessControl);
		this.controlContainerPanel.Controls.Add(this.optionsProjectsControl);
		this.controlContainerPanel.Controls.Add(this.optionsUpdatesControl);
		this.controlContainerPanel.Location = new System.Drawing.Point(343, 16);
		this.controlContainerPanel.Margin = new System.Windows.Forms.Padding(4);
		this.controlContainerPanel.Name = "controlContainerPanel";
		this.controlContainerPanel.Size = new System.Drawing.Size(780, 665);
		this.controlContainerPanel.TabIndex = 1;
		this.optionsComponentsWebInteractionControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsWebInteractionControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsWebInteractionControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsWebInteractionControl.Name = "optionsComponentsWebInteractionControl";
		this.optionsComponentsWebInteractionControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsWebInteractionControl.TabIndex = 7;
		this.optionsComponentsWebServiceRestControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsWebServiceRestControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsWebServiceRestControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsWebServiceRestControl.Name = "optionsComponentsWebServiceRestControl";
		this.optionsComponentsWebServiceRestControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsWebServiceRestControl.TabIndex = 7;
		this.optionsComponentsUserInputControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsUserInputControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsUserInputControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsUserInputControl.Name = "optionsComponentsUserInputControl";
		this.optionsComponentsUserInputControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsUserInputControl.TabIndex = 6;
		this.optionsComponentsVoiceInputControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsVoiceInputControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsVoiceInputControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsVoiceInputControl.Name = "optionsComponentsVoiceInputControl";
		this.optionsComponentsVoiceInputControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsVoiceInputControl.TabIndex = 6;
		this.optionsComponentsTranscribeAudioControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsTranscribeAudioControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsTranscribeAudioControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsTranscribeAudioControl.Name = "optionsComponentsTranscribeAudioControl";
		this.optionsComponentsTranscribeAudioControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsTranscribeAudioControl.TabIndex = 6;
		this.optionsComponentsAuthenticationControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsAuthenticationControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsAuthenticationControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsAuthenticationControl.Name = "optionsComponentsAuthenticationControl";
		this.optionsComponentsAuthenticationControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsAuthenticationControl.TabIndex = 6;
		this.optionsComponentsCreditCardControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsCreditCardControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsCreditCardControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsCreditCardControl.Name = "optionsComponentsCreditCardControl";
		this.optionsComponentsCreditCardControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsCreditCardControl.TabIndex = 6;
		this.optionsComponentsRecordControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsRecordControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsRecordControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsRecordControl.Name = "optionsComponentsRecordControl";
		this.optionsComponentsRecordControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsRecordControl.TabIndex = 4;
		this.optionsComponentsMenuControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsMenuControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsMenuControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsMenuControl.Name = "optionsComponentsMenuControl";
		this.optionsComponentsMenuControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsMenuControl.TabIndex = 3;
		this.optionsComponentsSurveyControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsSurveyControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsSurveyControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsSurveyControl.Name = "optionsComponentsSurveyControl";
		this.optionsComponentsSurveyControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsSurveyControl.TabIndex = 3;
		this.optionsComponentsTextToSpeechControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsTextToSpeechControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsTextToSpeechControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsTextToSpeechControl.Name = "optionsComponentsTextToSpeechControl";
		this.optionsComponentsTextToSpeechControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsTextToSpeechControl.TabIndex = 3;
		this.optionsComponentsTextToSpeechControl.GoogleCloudConfigurationChanged += new TCX.CFD.Controls.OptionsComponentsTextToSpeechControl.GoogleCloudConfigurationChangedHandler(TextToSpeech_GoogleCloudConfigurationChanged);
		this.optionsComponentsSpeechToTextControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsSpeechToTextControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsSpeechToTextControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsSpeechToTextControl.Name = "optionsComponentsSpeechToTextControl";
		this.optionsComponentsSpeechToTextControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsSpeechToTextControl.TabIndex = 3;
		this.optionsComponentsSpeechToTextControl.GoogleCloudConfigurationChanged += new TCX.CFD.Controls.OptionsComponentsSpeechToTextControl.GoogleCloudConfigurationChangedHandler(SpeechToText_GoogleCloudConfigurationChanged);
		this.optionsComponentsPromptPlaybackControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsPromptPlaybackControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsPromptPlaybackControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsPromptPlaybackControl.Name = "optionsComponentsPromptPlaybackControl";
		this.optionsComponentsPromptPlaybackControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsPromptPlaybackControl.TabIndex = 3;
		this.optionsComponentsCryptographyControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsCryptographyControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsCryptographyControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsCryptographyControl.Name = "optionsComponentsCryptographyControl";
		this.optionsComponentsCryptographyControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsCryptographyControl.TabIndex = 2;
		this.optionsComponentsFileManagementControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsFileManagementControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsFileManagementControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsFileManagementControl.Name = "optionsComponentsFileManagementControl";
		this.optionsComponentsFileManagementControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsFileManagementControl.TabIndex = 2;
		this.optionsComponentsSocketClientControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsSocketClientControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsSocketClientControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsSocketClientControl.Name = "optionsComponentsSocketClientControl";
		this.optionsComponentsSocketClientControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsSocketClientControl.TabIndex = 2;
		this.optionsComponentsEMailSenderControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsEMailSenderControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsEMailSenderControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsEMailSenderControl.Name = "optionsComponentsEMailSenderControl";
		this.optionsComponentsEMailSenderControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsEMailSenderControl.TabIndex = 2;
		this.optionsComponentsDatabaseAccessControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsComponentsDatabaseAccessControl.Location = new System.Drawing.Point(0, 0);
		this.optionsComponentsDatabaseAccessControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsComponentsDatabaseAccessControl.Name = "optionsComponentsDatabaseAccessControl";
		this.optionsComponentsDatabaseAccessControl.Size = new System.Drawing.Size(780, 665);
		this.optionsComponentsDatabaseAccessControl.TabIndex = 2;
		this.optionsProjectsControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsProjectsControl.Location = new System.Drawing.Point(0, 0);
		this.optionsProjectsControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsProjectsControl.Name = "optionsProjectsControl";
		this.optionsProjectsControl.Size = new System.Drawing.Size(780, 665);
		this.optionsProjectsControl.TabIndex = 0;
		this.optionsUpdatesControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.optionsUpdatesControl.Location = new System.Drawing.Point(0, 0);
		this.optionsUpdatesControl.Margin = new System.Windows.Forms.Padding(5);
		this.optionsUpdatesControl.Name = "optionsUpdatesControl";
		this.optionsUpdatesControl.Size = new System.Drawing.Size(780, 665);
		this.optionsUpdatesControl.TabIndex = 0;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(1140, 732);
		base.Controls.Add(this.controlContainerPanel);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.categoryTreeView);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(1061, 666);
		base.Name = "OptionsForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Options";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(OptionsForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(OptionsForm_HelpRequested);
		this.controlContainerPanel.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
