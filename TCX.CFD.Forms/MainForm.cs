using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Controls;
using TCX.CFD.Properties;
using TCX.Windows.Forms.Controls.Docking.Serialization;

namespace TCX.CFD.Forms;

public class MainForm : Form
{
	private readonly SplashForm splashForm;

	private ProjectObject projectObject;

	private StartPageControl startPageControl;

	private IContainer components;

	private ToolStripContainer toolStripContainer1;

	private StatusStrip statusStrip;

	private MenuStrip menuStrip;

	private ToolStripMenuItem fileToolStripMenuItem;

	private ToolStripMenuItem openToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripMenuItem saveToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem exitToolStripMenuItem;

	private ToolStripMenuItem editToolStripMenuItem;

	private ToolStripMenuItem undoToolStripMenuItem;

	private ToolStripMenuItem redoToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem cutToolStripMenuItem;

	private ToolStripMenuItem copyToolStripMenuItem;

	private ToolStripMenuItem pasteToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripMenuItem selectAllToolStripMenuItem;

	private ToolStripMenuItem toolsToolStripMenuItem;

	private ToolStripMenuItem onlineServicesToolStripMenuItem;

	private ToolStripMenuItem optionsToolStripMenuItem;

	private ToolStripMenuItem helpToolStripMenuItem;

	private ToolStripMenuItem checkForUpdatesToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator23;

	private ToolStripSeparator toolStripSeparator24;

	private ToolStripMenuItem aboutToolStripMenuItem;

	private ToolStripMenuItem documentationToolStripMenuItem;

	private ToolStrip toolStrip;

	private ToolStripButton saveToolStripButton;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton cutToolStripButton;

	private ToolStripButton copyToolStripButton;

	private ToolStripButton pasteToolStripButton;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripMenuItem exportProjectToolStripMenuItem;

	private ToolStripMenuItem saveProjectAsToolStripMenuItem;

	private ToolStripMenuItem closeProjectToolStripMenuItem;

	private ToolStripMenuItem saveAllToolStripMenuItem;

	private ToolStripMenuItem deleteToolStripMenuItem;

	private ToolStripMenuItem buildToolStripMenuItem;

	private ToolStripMenuItem debugBuildToolStripMenuItem;

	private ToolStripMenuItem releaseBuildToolStripMenuItem;

	private ToolStripMenuItem buildAllToolStripMenuItem;

	private ToolStripStatusLabel toolStripStatusLabel;

	private ToolStripButton saveAllToolStripButton;

	private ProjectManagerControl projectManagerControl;

	private ToolStripMenuItem newToolStripMenuItem;

	private ToolStripMenuItem newProjectCallflowToolStripMenuItem;

	private ToolStripMenuItem newProjectDialerToolStripMenuItem;

	private ToolStripMenuItem newProjectTemplateToolStripMenuItem;

	private ToolStripMenuItem importProjectFromBuildOutputToolStripMenuItem;

	private ToolStripMenuItem newDialerToolStripMenuItem;

	private ToolStripMenuItem newCallflowToolStripMenuItem;

	private ToolStripMenuItem newFolderToolStripMenuItem;

	private ToolStripMenuItem openProjectToolStripMenuItem;

	private ToolStripMenuItem openDialerFileToolStripMenuItem;

	private ToolStripMenuItem openCallflowFileToolStripMenuItem;

	private ToolStripMenuItem closeComponentToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator9;

	private ToolStripMenuItem removeComponentToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator10;

	private ToolStripMenuItem renameToolStripMenuItem;

	private ToolStripMenuItem viewToolStripMenuItem;

	private ToolStripMenuItem viewComponentsToolbarToolStripMenuItem;

	private ToolStripMenuItem viewProjectExplorerWindowToolStripMenuItem;

	private ToolStripMenuItem viewPropertiesWindowToolStripMenuItem;

	private ToolStripMenuItem viewOutputWindowToolStripMenuItem;

	private ToolStripMenuItem viewErrorListWindowToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator11;

	private ToolStripMenuItem toolbarToolStripMenuItem;

	private ToolStripMenuItem statusbarToolStripMenuItem;

	private ToolStripMenuItem expandComponentToolStripMenuItem;

	private ToolStripMenuItem collapseComponentToolStripMenuItem;

	private ToolStripMenuItem zoomLevelsToolStripMenuItem;

	private ToolStripMenuItem zoom400ToolStripMenuItem;

	private ToolStripMenuItem zoom300ToolStripMenuItem;

	private ToolStripMenuItem zoom200ToolStripMenuItem;

	private ToolStripMenuItem zoom150ToolStripMenuItem;

	private ToolStripMenuItem zoom100ToolStripMenuItem;

	private ToolStripMenuItem zoom75ToolStripMenuItem;

	private ToolStripMenuItem zoom50ToolStripMenuItem;

	private ToolStripMenuItem zoomShowAllToolStripMenuItem;

	private ToolStripMenuItem navigationToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator12;

	private ToolStripMenuItem zoomInToolStripMenuItem;

	private ToolStripMenuItem zoomOutToolStripMenuItem;

	private ToolStripMenuItem navigationPanToolStripMenuItem;

	private ToolStripMenuItem navigationDefaultToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator13;

	private ToolStripDropDownButton newToolStripDropDownButton;

	private ToolStripMenuItem toolbarNewProjectDialerToolStripMenuItem;

	private ToolStripMenuItem toolbarNewProjectCallflowToolStripMenuItem;

	private ToolStripMenuItem toolbarNewProjectTemplateToolStripMenuItem;

	private ToolStripMenuItem toolbarImportProjectFromBuildOutputToolStripMenuItem;

	private ToolStripMenuItem toolbarNewDialerToolStripMenuItem;

	private ToolStripMenuItem toolbarNewCallflowToolStripMenuItem;

	private ToolStripMenuItem toolbarNewFolderToolStripMenuItem;

	private ToolStripDropDownButton openToolStripDropDownButton;

	private ToolStripMenuItem toolbarOpenProjectToolStripMenuItem;

	private ToolStripMenuItem toolbarOpenDialerFileToolStripMenuItem;

	private ToolStripMenuItem toolbarOpenCallflowFileToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator14;

	private ToolStripButton undoToolStripButton;

	private ToolStripButton redoToolStripButton;

	private ToolStripSeparator toolStripSeparator16;

	private ToolStripSeparator debugToolStripSeparator;

	private ToolStripButton expandToolStripButton;

	private ToolStripButton collapseToolStripButton;

	private ToolStripComboBox zoomLevelToolStripComboBox;

	private ToolStripButton zoomInToolStripButton;

	private ToolStripButton zoomOutToolStripButton;

	private ToolStripButton navigationPanToolStripButton;

	private ToolStripButton navigationDefaultToolStripButton;

	private ToolStripSeparator toolsToolStripSeparator;

	private ToolStripButton onlineServicesToolStripButton;

	private ToolStripButton optionsToolStripButton;

	private SaveFileDialog saveProjectAsDialog;

	private SaveFileDialog saveNewProjectDialog;

	private ToolStripMenuItem toolbarNewComponentToolStripMenuItem;

	private ToolStripMenuItem newComponentToolStripMenuItem;

	private ToolStripButton deleteToolStripButton;

	private OpenFileDialog openProjectDialog;

	private ToolStripMenuItem enableComponentToolStripMenuItem;

	private ToolStripMenuItem disableComponentToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator19;

	private ToolStripButton buildAllToolStripButton;

	private ToolStripButton enableToolStripButton;

	private ToolStripButton disableToolStripButton;

	private ToolStripMenuItem recentProjectsToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator17;

	private ToolStripMenuItem cancelBuildToolStripMenuItem;

	private ToolStripButton cancelBuildToolStripButton;

	private ToolStripMenuItem buildProjectExtensionToolStripMenuItem;

	private ToolStripButton buildProjectExtensionToolStripButton;

	private ToolStripMenuItem addToolStripMenuItem;

	private ToolStripMenuItem addExistingDialerToolStripMenuItem;

	private ToolStripMenuItem addExistingCallflowToolStripMenuItem;

	private ToolStripMenuItem addExistingComponentToolStripMenuItem;

	private ToolStripProgressBar toolStripStatusProgressBar;

	private ToolStripMenuItem debugToolStripMenuItem;

	private ToolStripMenuItem debugStartToolStripMenuItem;

	private ToolStripMenuItem debugStopToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator20;

	private ToolStripMenuItem debugStepOverToolStripMenuItem;

	private ToolStripMenuItem debugStepIntoToolStripMenuItem;

	private ToolStripMenuItem debugStepOutToolStripMenuItem;

	private ToolStripSeparator buildToolStripSeparator;

	private ToolStripButton debugStartToolStripButton;

	private ToolStripButton debugStopToolStripButton;

	private ToolStripButton debugStepOverToolStripButton;

	private ToolStripButton debugStepIntoToolStripButton;

	private ToolStripButton debugStepOutToolStripButton;

	private ToolStripMenuItem viewDebugWindowToolStripMenuItem;

	private ToolStripMenuItem openComponentFileToolStripMenuItem;

	private ToolStripMenuItem toolbarOpenComponentFileToolStripMenuItem;

	private ToolStripMenuItem removeFolderToolStripMenuItem;

	private ToolStripMenuItem removeDialerToolStripMenuItem;

	private ToolStripMenuItem removeCallflowToolStripMenuItem;

	private ToolStripMenuItem closeDialerToolStripMenuItem;

	private ToolStripMenuItem closeCallflowToolStripMenuItem;

	private ToolStripButton debugBuildToolStripButton;

	private ToolStripButton releaseBuildToolStripButton;

	private ToolStripSeparator toolStripSeparator22;

	private ToolStripMenuItem viewStartPageToolStripMenuItem;

	private void AddRecentProjectMenuItem(string projectPath)
	{
		if (!string.IsNullOrEmpty(projectPath))
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(projectPath);
			toolStripMenuItem.Click += RecentProjectMenuItem_Click;
			recentProjectsToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
		}
	}

	private void AddRecentProject(string projectPath)
	{
		RecentProjectsHelper.AddRecentProject(projectPath);
		ReloadRecentProjects();
	}

	private void ReloadRecentProjects()
	{
		recentProjectsToolStripMenuItem.DropDownItems.Clear();
		foreach (string recentProject in RecentProjectsHelper.GetRecentProjects())
		{
			AddRecentProjectMenuItem(recentProject);
		}
		startPageControl?.ReloadRecentProjects();
	}

	private void RecentProjectMenuItem_Click(object sender, EventArgs e)
	{
		LoadProject((sender as ToolStripMenuItem).Text);
	}

	private void LoadProject(string projectPath)
	{
		if (projectObject != null)
		{
			if (MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Question.ProjectStillLoaded"), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			projectObject.Close();
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.LoadingProject");
			statusStrip.Refresh();
			FileInfo fileInfo = new FileInfo(projectPath);
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ProjectFileNotFound"));
			}
			projectObject = new ProjectObject(fileInfo.DirectoryName, fileInfo.Name, projectManagerControl);
			BindProjectObjectAndOpen();
		}
		catch (Exception exc)
		{
			projectObject = null;
			projectManagerControl.SetProjectClosed();
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.OpeningProject") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
			EnableDisableControls();
		}
	}

	private void BindProjectObjectAndOpen()
	{
		projectObject.Loading += ProjectObject_Loading;
		projectObject.Loaded += ProjectObject_Loaded;
		projectObject.NotLoaded += ProjectObject_NotLoaded;
		projectObject.LoadProgress += ProjectObject_LoadProgress;
		projectObject.NameChanged += ProjectObject_NameChanged;
		projectObject.Closed += ProjectObject_Closed;
		projectObject.Open();
		AddRecentProject(projectObject.Path);
		List<CallflowFileObject> callflowFileObjectList = projectObject.GetCallflowFileObjectList();
		if (callflowFileObjectList.Count > 0)
		{
			callflowFileObjectList[0].Open();
			return;
		}
		List<DialerFileObject> dialerFileObjectList = projectObject.GetDialerFileObjectList();
		if (dialerFileObjectList.Count > 0)
		{
			dialerFileObjectList[0].Open();
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);
		if (projectManagerControl.IsBuilding)
		{
			e.Cancel = true;
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.BuildIsActive"), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Closing");
			statusStrip.Refresh();
			if (projectObject != null && !projectObject.Close())
			{
				e.Cancel = true;
			}
			try
			{
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3CX Call Flow Designer");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				DockingControlsPersister.Serialize(projectManagerControl, Path.Combine(text, "3CX Call Flow Designer.layout"));
			}
			catch (Exception)
			{
			}
		}
		catch (Exception exc)
		{
			e.Cancel = MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Closing"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1) == DialogResult.No;
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
		}
	}

	public MainForm(params string[] args)
	{
		splashForm = new SplashForm();
		splashForm.Show();
		splashForm.Refresh();
		InitializeComponent();
		try
		{
			if (Settings.Default.HasToUpgradeSettings)
			{
				Settings.Default.Upgrade();
				Settings.Default.HasToUpgradeSettings = false;
				Settings.Default.Save();
			}
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "3CX Call Flow Designer");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			DockingControlsPersister.Deserialize(projectManagerControl, Path.Combine(text, "3CX Call Flow Designer.layout"));
		}
		catch (Exception)
		{
		}
		LoadLabelsFromResources();
		projectManagerControl.SetStatusStrip(statusStrip);
		ReloadRecentProjects();
		projectManagerControl.ProjectExplorer.OnSaveProjectAsRequested += ProjectExplorer_OnSaveProjectAsRequested;
		projectManagerControl.ShowStartPage(CreateStartPage);
		if (args.Length != 0)
		{
			LoadProject(args[0]);
		}
		EnableDisableControls();
	}

	private StartPageControl CreateStartPage()
	{
		startPageControl = new StartPageControl();
		startPageControl.OnNewProject += CreateNewProject;
		startPageControl.OnOpenProject += StartPageControl_OnOpenProject;
		startPageControl.OnOpenRecentProject += LoadProject;
		return startPageControl;
	}

	private void StartPageControl_OnOpenProject()
	{
		openProjectToolStripMenuItem.PerformClick();
	}

	private void MainForm_Load(object sender, EventArgs e)
	{
		splashForm.Close();
		if (Settings.Default.AutoUpdatesEnabled)
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			try
			{
				Process.Start(Path.Combine(directoryName, "updater.exe"));
			}
			catch (Exception)
			{
			}
		}
	}

	private void LoadLabelsFromResources()
	{
		Text = LocalizedResourceMgr.GetString("MainForm.Title");
		fileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.File");
		newToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNew");
		newProjectDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewProjectDialer");
		newProjectCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewProjectCallflow");
		newProjectTemplateToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewProjectTemplate");
		importProjectFromBuildOutputToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileImportProjectFromBuildOutput");
		newDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewDialer");
		newCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewCallflow");
		newComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewComponent");
		newFolderToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileNewFolder");
		openToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpen");
		openProjectToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenProject");
		openCallflowFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenCallflowFile");
		openComponentFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenComponentFile");
		addToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileAdd");
		addExistingDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileAddExistingDialer");
		addExistingCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileAddExistingCallflow");
		addExistingComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileAddExistingComponent");
		renameToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRename");
		removeFolderToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveFolder");
		removeCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveCallflow");
		removeComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveComponent");
		exportProjectToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileExportProject");
		closeProjectToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileCloseProject");
		closeCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileCloseCallflow");
		closeComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileCloseComponent");
		saveProjectAsToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSaveProjectAs");
		saveToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSave");
		saveAllToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSaveAll");
		recentProjectsToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRecentProjects");
		exitToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileExit");
		editToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.Edit");
		undoToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditUndo");
		redoToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditRedo");
		cutToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditCut");
		copyToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditCopy");
		pasteToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditPaste");
		deleteToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditDelete");
		selectAllToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.EditSelectAll");
		viewToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.View");
		viewComponentsToolbarToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewComponentsToolbar");
		viewProjectExplorerWindowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewProjectExplorerWindow");
		viewPropertiesWindowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewPropertiesWindow");
		viewOutputWindowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewOutputWindow");
		viewErrorListWindowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewErrorListWindow");
		viewDebugWindowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewDebugWindow");
		viewStartPageToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewStartPage");
		toolbarToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewToolbar");
		statusbarToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ViewStatusbar");
		buildToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.Build");
		debugBuildToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.BuildDebug");
		releaseBuildToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.BuildRelease");
		buildAllToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.BuildAll");
		cancelBuildToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.CancelBuild");
		buildProjectExtensionToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ProjectExtension");
		debugToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.Debug");
		debugStartToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.DebugStart");
		debugStopToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.DebugStop");
		debugStepOverToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.DebugStepOver");
		debugStepIntoToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.DebugStepInto");
		debugStepOutToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.DebugStepOut");
		toolsToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.Tools");
		enableComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsEnable");
		disableComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsDisable");
		expandComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsExpand");
		collapseComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsCollapse");
		zoomLevelsToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevels");
		zoom400ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel400");
		zoom300ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel300");
		zoom200ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel200");
		zoom150ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel150");
		zoom100ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel100");
		zoom75ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel75");
		zoom50ToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevel50");
		zoomShowAllToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevelShowAll");
		navigationToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsNavigationTools");
		zoomInToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomIn");
		zoomOutToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomOut");
		navigationPanToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsNavigationToolPan");
		navigationDefaultToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsNavigationToolDefault");
		onlineServicesToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsOnlineServices");
		optionsToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsOptions");
		helpToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.Help");
		documentationToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.HelpDocumentation");
		checkForUpdatesToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.HelpCheckForUpdates");
		aboutToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.HelpAbout");
		newToolStripDropDownButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.New");
		toolbarNewProjectDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.NewProjectDialer");
		toolbarNewProjectCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.NewProjectCallflow");
		toolbarNewProjectTemplateToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.NewProjectTemplate");
		toolbarImportProjectFromBuildOutputToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.ImportProjectFromBuildOutput");
		toolbarNewCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.NewCallflow");
		toolbarNewComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.NewComponent");
		toolbarNewFolderToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.NewNewFolder");
		openToolStripDropDownButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Open");
		toolbarOpenProjectToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenProject");
		toolbarOpenCallflowFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenCallflowFile");
		toolbarOpenComponentFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenComponentFile");
		saveToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Save");
		saveAllToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.SaveAll");
		undoToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Undo");
		redoToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Redo");
		cutToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Cut");
		copyToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Copy");
		pasteToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Paste");
		deleteToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Delete");
		debugBuildToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DebugBuild");
		releaseBuildToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.ReleaseBuild");
		buildAllToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.BuildAll");
		cancelBuildToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.CancelBuild");
		buildProjectExtensionToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.ProjectExtension");
		debugStartToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DebugStart");
		debugStopToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DebugStop");
		debugStepOverToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DebugStepOver");
		debugStepIntoToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DebugStepInto");
		debugStepOutToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DebugStepOut");
		enableToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Enable");
		disableToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Disable");
		expandToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Expand");
		collapseToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.Collapse");
		zoomLevelToolStripComboBox.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.ZoomLevel");
		zoomLevelToolStripComboBox.Items.AddRange(new object[8]
		{
			"400%",
			"300%",
			"200%",
			"150%",
			"100%",
			"75%",
			"50%",
			LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevelShowAll")
		});
		zoomInToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.ZoomIn");
		zoomOutToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.ZoomOut");
		navigationPanToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.PanNavigationTool");
		navigationDefaultToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.DefaultNavigationTool");
		onlineServicesToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OnlineServices");
		toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
		saveNewProjectDialog.Filter = LocalizedResourceMgr.GetString("FileDialogs.ProjectFiles.Filter");
		saveNewProjectDialog.FileName = LocalizedResourceMgr.GetString("MainForm.ProjectDialogs.DefaultCreationName") + ".cfdproj";
		saveNewProjectDialog.Title = LocalizedResourceMgr.GetString("MainForm.ProjectDialogs.CreateTitle");
		openProjectDialog.Filter = LocalizedResourceMgr.GetString("FileDialogs.ProjectFiles.Filter");
		openProjectDialog.Title = LocalizedResourceMgr.GetString("MainForm.ProjectDialogs.OpenTitle");
	}

	private void EnableDisableControls()
	{
		if (projectObject == null)
		{
			Text = LocalizedResourceMgr.GetString("MainForm.Title");
			newDialerToolStripMenuItem.Visible = false;
			newCallflowToolStripMenuItem.Visible = false;
			newComponentToolStripMenuItem.Visible = false;
			newFolderToolStripMenuItem.Visible = false;
			importProjectFromBuildOutputToolStripMenuItem.Visible = false;
			openDialerFileToolStripMenuItem.Visible = false;
			openDialerFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenDialerFile");
			openCallflowFileToolStripMenuItem.Visible = false;
			openCallflowFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenCallflowFile");
			openComponentFileToolStripMenuItem.Visible = false;
			openComponentFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenComponentFile");
			addToolStripMenuItem.Visible = false;
			addExistingDialerToolStripMenuItem.Visible = false;
			addExistingCallflowToolStripMenuItem.Visible = false;
			addExistingComponentToolStripMenuItem.Visible = false;
			renameToolStripMenuItem.Enabled = false;
			renameToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRename");
			removeFolderToolStripMenuItem.Visible = false;
			removeDialerToolStripMenuItem.Visible = false;
			removeCallflowToolStripMenuItem.Visible = false;
			removeComponentToolStripMenuItem.Visible = false;
			exportProjectToolStripMenuItem.Enabled = false;
			exportProjectToolStripMenuItem.Visible = false;
			closeProjectToolStripMenuItem.Enabled = false;
			closeDialerToolStripMenuItem.Visible = false;
			closeCallflowToolStripMenuItem.Visible = false;
			closeComponentToolStripMenuItem.Visible = false;
			saveProjectAsToolStripMenuItem.Enabled = false;
			saveToolStripMenuItem.Enabled = false;
			saveToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSave");
			saveAllToolStripMenuItem.Enabled = false;
			undoToolStripMenuItem.Enabled = false;
			redoToolStripMenuItem.Enabled = false;
			cutToolStripMenuItem.Enabled = false;
			copyToolStripMenuItem.Enabled = false;
			pasteToolStripMenuItem.Enabled = false;
			deleteToolStripMenuItem.Enabled = false;
			selectAllToolStripMenuItem.Enabled = false;
			viewStartPageToolStripMenuItem.Enabled = true;
			viewDebugWindowToolStripMenuItem.Visible = false;
			buildToolStripMenuItem.Visible = false;
			debugToolStripMenuItem.Visible = false;
			enableComponentToolStripMenuItem.Enabled = false;
			disableComponentToolStripMenuItem.Enabled = false;
			expandComponentToolStripMenuItem.Enabled = false;
			collapseComponentToolStripMenuItem.Enabled = false;
			zoomLevelsToolStripMenuItem.Enabled = false;
			navigationToolStripMenuItem.Enabled = false;
			onlineServicesToolStripMenuItem.Visible = false;
			toolbarNewDialerToolStripMenuItem.Visible = false;
			toolbarNewCallflowToolStripMenuItem.Visible = false;
			toolbarNewComponentToolStripMenuItem.Visible = false;
			toolbarNewFolderToolStripMenuItem.Visible = false;
			toolbarImportProjectFromBuildOutputToolStripMenuItem.Visible = false;
			toolbarOpenDialerFileToolStripMenuItem.Visible = false;
			toolbarOpenDialerFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenDialerFile");
			toolbarOpenCallflowFileToolStripMenuItem.Visible = false;
			toolbarOpenCallflowFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenCallflowFile");
			toolbarOpenComponentFileToolStripMenuItem.Visible = false;
			toolbarOpenComponentFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenComponentFile");
			saveToolStripButton.Enabled = false;
			saveToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSave");
			saveAllToolStripButton.Enabled = false;
			undoToolStripButton.Enabled = false;
			redoToolStripButton.Enabled = false;
			cutToolStripButton.Enabled = false;
			copyToolStripButton.Enabled = false;
			pasteToolStripButton.Enabled = false;
			deleteToolStripButton.Enabled = false;
			debugBuildToolStripButton.Visible = false;
			releaseBuildToolStripButton.Visible = false;
			buildAllToolStripButton.Visible = false;
			cancelBuildToolStripButton.Visible = false;
			buildProjectExtensionToolStripButton.Visible = false;
			buildToolStripSeparator.Visible = false;
			debugStartToolStripButton.Visible = false;
			debugStopToolStripButton.Visible = false;
			debugStepOverToolStripButton.Visible = false;
			debugStepIntoToolStripButton.Visible = false;
			debugStepOutToolStripButton.Visible = false;
			debugToolStripSeparator.Visible = false;
			enableToolStripButton.Enabled = false;
			disableToolStripButton.Enabled = false;
			expandToolStripButton.Enabled = false;
			collapseToolStripButton.Enabled = false;
			zoomLevelToolStripComboBox.Enabled = false;
			zoomInToolStripButton.Enabled = false;
			zoomOutToolStripButton.Enabled = false;
			navigationDefaultToolStripButton.Enabled = false;
			navigationPanToolStripButton.Enabled = false;
			onlineServicesToolStripButton.Visible = false;
			toolStripStatusProgressBar.Visible = false;
			return;
		}
		bool ısBuilding = projectManagerControl.IsBuilding;
		bool ısDebugging = projectManagerControl.IsDebugging;
		bool ısProjectSelected = projectManagerControl.ProjectExplorer.IsProjectSelected;
		bool ısFolderSelected = projectManagerControl.ProjectExplorer.IsFolderSelected;
		bool ısDialerFileSelected = projectManagerControl.ProjectExplorer.IsDialerFileSelected;
		bool ısCallflowFileSelected = projectManagerControl.ProjectExplorer.IsCallflowFileSelected;
		bool ısComponentFileSelected = projectManagerControl.ProjectExplorer.IsComponentFileSelected;
		bool flag = ısDialerFileSelected || ısCallflowFileSelected || ısComponentFileSelected;
		string selectedFileName = projectManagerControl.ProjectExplorer.GetSelectedFileName();
		bool flag2 = projectObject.HasDialer();
		bool flag3 = projectObject.HasCallflow();
		bool ısFlowDesignerControlActive = projectManagerControl.IsFlowDesignerControlActive;
		bool ısEditing = projectManagerControl.IsEditing;
		bool ısCurrentFileModified = projectManagerControl.IsCurrentFileModified;
		string editingFileName = projectManagerControl.GetEditingFileName();
		FlowDesignerControl flowDesigner = projectManagerControl.FlowDesigner;
		WorkflowView workflowView = flowDesigner?.CurrentView;
		Text = LocalizedResourceMgr.GetString("MainForm.Title") + " - " + projectObject.Name;
		newProjectDialerToolStripMenuItem.Visible = true;
		newProjectDialerToolStripMenuItem.Enabled = !ısBuilding;
		newProjectCallflowToolStripMenuItem.Visible = true;
		newProjectCallflowToolStripMenuItem.Enabled = !ısBuilding;
		newProjectTemplateToolStripMenuItem.Enabled = !ısBuilding;
		importProjectFromBuildOutputToolStripMenuItem.Visible = false;
		importProjectFromBuildOutputToolStripMenuItem.Enabled = !ısBuilding;
		newDialerToolStripMenuItem.Visible = true;
		newDialerToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected) && !flag2;
		newCallflowToolStripMenuItem.Visible = true;
		newCallflowToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected) && !flag3;
		newComponentToolStripMenuItem.Visible = true;
		newComponentToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected);
		newFolderToolStripMenuItem.Visible = true;
		newFolderToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected);
		openProjectToolStripMenuItem.Visible = true;
		openProjectToolStripMenuItem.Enabled = !ısBuilding;
		openDialerFileToolStripMenuItem.Visible = true;
		openDialerFileToolStripMenuItem.Enabled = !ısBuilding && ısDialerFileSelected;
		openDialerFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenDialerFile") + (openDialerFileToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		openCallflowFileToolStripMenuItem.Visible = true;
		openCallflowFileToolStripMenuItem.Enabled = !ısBuilding && ısCallflowFileSelected;
		openCallflowFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenCallflowFile") + (openCallflowFileToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		openComponentFileToolStripMenuItem.Visible = true;
		openComponentFileToolStripMenuItem.Enabled = !ısBuilding && ısComponentFileSelected;
		openComponentFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileOpenComponentFile") + (openComponentFileToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		addToolStripMenuItem.Visible = true;
		addExistingDialerToolStripMenuItem.Visible = true;
		addExistingDialerToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected) && !flag2;
		addExistingCallflowToolStripMenuItem.Visible = true;
		addExistingCallflowToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected) && !flag3;
		addExistingComponentToolStripMenuItem.Visible = true;
		addExistingComponentToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected);
		renameToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected || flag);
		renameToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRename") + (renameToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		removeFolderToolStripMenuItem.Visible = !ısBuilding && ısFolderSelected;
		removeFolderToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveFolder") + (removeFolderToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		removeDialerToolStripMenuItem.Visible = !ısBuilding && ısDialerFileSelected;
		removeDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveDialer") + (removeDialerToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		removeCallflowToolStripMenuItem.Visible = !ısBuilding && ısCallflowFileSelected;
		removeCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveCallflow") + (removeCallflowToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		removeComponentToolStripMenuItem.Visible = !ısBuilding && ısComponentFileSelected;
		removeComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileRemoveComponent") + (removeComponentToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		exportProjectToolStripMenuItem.Enabled = !ısBuilding;
		exportProjectToolStripMenuItem.Visible = false;
		closeProjectToolStripMenuItem.Enabled = !ısBuilding;
		closeDialerToolStripMenuItem.Visible = !ısBuilding && ısEditing && ısDialerFileSelected;
		closeDialerToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileCloseDialer") + (closeDialerToolStripMenuItem.Enabled ? (" " + editingFileName) : string.Empty);
		closeCallflowToolStripMenuItem.Visible = !ısBuilding && ısEditing && ısCallflowFileSelected;
		closeCallflowToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileCloseCallflow") + (closeCallflowToolStripMenuItem.Enabled ? (" " + editingFileName) : string.Empty);
		closeComponentToolStripMenuItem.Visible = !ısBuilding && ısEditing && ısComponentFileSelected;
		closeComponentToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileCloseComponent") + (closeComponentToolStripMenuItem.Enabled ? (" " + editingFileName) : string.Empty);
		saveProjectAsToolStripMenuItem.Enabled = !ısBuilding;
		saveToolStripMenuItem.Enabled = !ısBuilding && ısCurrentFileModified;
		saveToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSave") + (saveToolStripMenuItem.Enabled ? (" " + editingFileName) : string.Empty);
		saveAllToolStripMenuItem.Enabled = !ısBuilding;
		recentProjectsToolStripMenuItem.Enabled = !ısBuilding;
		undoToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Undo);
		if (undoToolStripMenuItem.Enabled)
		{
			undoToolStripMenuItem.ToolTipText = LocalizedResourceMgr.GetString("MainForm.Menu.EditUndo") + " " + flowDesigner.GetUndoDescription();
		}
		redoToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Redo);
		if (redoToolStripMenuItem.Enabled)
		{
			redoToolStripMenuItem.ToolTipText = LocalizedResourceMgr.GetString("MainForm.Menu.EditRedo") + " " + flowDesigner.GetRedoDescription();
		}
		cutToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Cut);
		copyToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Copy);
		pasteToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Paste);
		deleteToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Delete);
		selectAllToolStripMenuItem.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing;
		viewStartPageToolStripMenuItem.Enabled = true;
		viewDebugWindowToolStripMenuItem.Visible = false;
		buildToolStripMenuItem.Visible = true;
		debugBuildToolStripMenuItem.Visible = false;
		debugBuildToolStripMenuItem.Enabled = !ısBuilding;
		releaseBuildToolStripMenuItem.Enabled = !ısBuilding;
		releaseBuildToolStripMenuItem.Visible = false;
		buildAllToolStripMenuItem.Enabled = !ısBuilding;
		cancelBuildToolStripMenuItem.Enabled = ısBuilding;
		buildProjectExtensionToolStripMenuItem.Enabled = !ısBuilding;
		debugToolStripMenuItem.Visible = false;
		debugStartToolStripMenuItem.Enabled = !ısDebugging && projectObject.DebugBuildSuccessful && !projectObject.ChangedSinceLastDebugBuild;
		debugStopToolStripMenuItem.Enabled = ısDebugging;
		debugStepOverToolStripMenuItem.Enabled = ısDebugging && projectManagerControl.CanStepOver;
		debugStepIntoToolStripMenuItem.Enabled = ısDebugging && projectManagerControl.CanStepInto;
		debugStepOutToolStripMenuItem.Enabled = ısDebugging && projectManagerControl.CanStepOut;
		enableComponentToolStripMenuItem.Enabled = !ısBuilding && ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Enable);
		disableComponentToolStripMenuItem.Enabled = !ısBuilding && ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Disable);
		expandComponentToolStripMenuItem.Enabled = ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Expand);
		collapseComponentToolStripMenuItem.Enabled = ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Collapse);
		zoomLevelsToolStripMenuItem.Enabled = !ısBuilding && ısEditing;
		navigationToolStripMenuItem.Enabled = !ısBuilding && ısEditing;
		onlineServicesToolStripMenuItem.Visible = true;
		onlineServicesToolStripMenuItem.Enabled = !ısBuilding;
		if (ısEditing && workflowView != null)
		{
			int zoom = workflowView.Zoom;
			zoom400ToolStripMenuItem.Checked = zoom == 400;
			zoom300ToolStripMenuItem.Checked = zoom == 300;
			zoom200ToolStripMenuItem.Checked = zoom == 200;
			zoom150ToolStripMenuItem.Checked = zoom == 150;
			zoom100ToolStripMenuItem.Checked = zoom == 100;
			zoom75ToolStripMenuItem.Checked = zoom == 75;
			zoom50ToolStripMenuItem.Checked = zoom == 50;
			zoomInToolStripMenuItem.Checked = flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.ZoomIn);
			zoomOutToolStripMenuItem.Checked = flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.ZoomOut);
			navigationPanToolStripMenuItem.Checked = flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.Pan);
			navigationDefaultToolStripMenuItem.Checked = flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.DefaultFilter);
		}
		toolbarNewProjectDialerToolStripMenuItem.Enabled = !ısBuilding;
		toolbarNewProjectCallflowToolStripMenuItem.Enabled = !ısBuilding;
		toolbarNewProjectTemplateToolStripMenuItem.Enabled = !ısBuilding;
		toolbarImportProjectFromBuildOutputToolStripMenuItem.Visible = false;
		toolbarImportProjectFromBuildOutputToolStripMenuItem.Enabled = !ısBuilding;
		toolbarNewDialerToolStripMenuItem.Visible = true;
		toolbarNewDialerToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected) && !flag2;
		toolbarNewCallflowToolStripMenuItem.Visible = true;
		toolbarNewCallflowToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected) && !flag3;
		toolbarNewComponentToolStripMenuItem.Visible = true;
		toolbarNewComponentToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected);
		toolbarNewFolderToolStripMenuItem.Visible = true;
		toolbarNewFolderToolStripMenuItem.Enabled = !ısBuilding && (ısProjectSelected || ısFolderSelected);
		toolbarOpenProjectToolStripMenuItem.Enabled = !ısBuilding;
		toolbarOpenDialerFileToolStripMenuItem.Visible = true;
		toolbarOpenDialerFileToolStripMenuItem.Enabled = !ısBuilding && ısDialerFileSelected;
		toolbarOpenDialerFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenDialerFile") + (toolbarOpenDialerFileToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		toolbarOpenCallflowFileToolStripMenuItem.Visible = true;
		toolbarOpenCallflowFileToolStripMenuItem.Enabled = !ısBuilding && ısCallflowFileSelected;
		toolbarOpenCallflowFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenCallflowFile") + (toolbarOpenCallflowFileToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		toolbarOpenComponentFileToolStripMenuItem.Visible = true;
		toolbarOpenComponentFileToolStripMenuItem.Enabled = !ısBuilding && ısComponentFileSelected;
		toolbarOpenComponentFileToolStripMenuItem.Text = LocalizedResourceMgr.GetString("MainForm.Toolbar.OpenComponentFile") + (toolbarOpenComponentFileToolStripMenuItem.Enabled ? (" " + selectedFileName) : string.Empty);
		saveToolStripButton.Enabled = !ısBuilding && ısCurrentFileModified;
		saveToolStripButton.Text = LocalizedResourceMgr.GetString("MainForm.Menu.FileSave") + (saveToolStripButton.Enabled ? (" " + editingFileName) : string.Empty);
		saveAllToolStripButton.Enabled = !ısBuilding;
		undoToolStripButton.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Undo);
		if (undoToolStripButton.Enabled)
		{
			undoToolStripButton.ToolTipText = LocalizedResourceMgr.GetString("MainForm.Toolbar.Undo") + " " + flowDesigner.GetUndoDescription();
		}
		redoToolStripButton.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Redo);
		if (redoToolStripButton.Enabled)
		{
			redoToolStripButton.ToolTipText = LocalizedResourceMgr.GetString("MainForm.Toolbar.Redo") + " " + flowDesigner.GetRedoDescription();
		}
		cutToolStripButton.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Cut);
		copyToolStripButton.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Copy);
		pasteToolStripButton.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Paste);
		deleteToolStripButton.Enabled = !ısBuilding && ısFlowDesignerControlActive && ısEditing && flowDesigner.IsStandardCommandEnabled(StandardCommands.Delete);
		debugBuildToolStripButton.Visible = false;
		debugBuildToolStripButton.Enabled = !ısBuilding;
		releaseBuildToolStripButton.Visible = false;
		releaseBuildToolStripButton.Enabled = !ısBuilding;
		buildAllToolStripButton.Visible = true;
		buildAllToolStripButton.Enabled = !ısBuilding;
		cancelBuildToolStripButton.Visible = true;
		cancelBuildToolStripButton.Enabled = ısBuilding;
		buildProjectExtensionToolStripButton.Visible = true;
		buildProjectExtensionToolStripButton.Enabled = !ısBuilding;
		buildToolStripSeparator.Visible = true;
		debugStartToolStripButton.Visible = false;
		debugStartToolStripButton.Enabled = !ısDebugging && projectObject.DebugBuildSuccessful && !projectObject.ChangedSinceLastDebugBuild;
		debugStopToolStripButton.Visible = false;
		debugStopToolStripButton.Enabled = ısDebugging;
		debugStepOverToolStripButton.Visible = false;
		debugStepOverToolStripButton.Enabled = ısDebugging && projectManagerControl.CanStepOver;
		debugStepIntoToolStripButton.Visible = false;
		debugStepIntoToolStripButton.Enabled = ısDebugging && projectManagerControl.CanStepInto;
		debugStepOutToolStripButton.Visible = false;
		debugStepOutToolStripButton.Enabled = ısDebugging && projectManagerControl.CanStepOut;
		debugToolStripSeparator.Visible = false;
		enableToolStripButton.Enabled = !ısBuilding && ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Enable);
		disableToolStripButton.Enabled = !ısBuilding && ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Disable);
		expandToolStripButton.Enabled = !ısBuilding && ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Expand);
		collapseToolStripButton.Enabled = !ısBuilding && ısEditing && flowDesigner.IsStandardCommandEnabled(WorkflowMenuCommands.Collapse);
		zoomLevelToolStripComboBox.Enabled = !ısBuilding && ısEditing;
		zoomLevelToolStripComboBox.Text = ((ısEditing && workflowView != null) ? $"{workflowView.Zoom}%" : LocalizedResourceMgr.GetString("MainForm.Toolbar.ZoomLevel"));
		zoomInToolStripButton.Enabled = !ısBuilding && ısEditing;
		zoomInToolStripButton.Checked = ısEditing && flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.ZoomIn);
		zoomOutToolStripButton.Enabled = !ısBuilding && ısEditing;
		zoomOutToolStripButton.Checked = ısEditing && flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.ZoomOut);
		navigationPanToolStripButton.Enabled = !ısBuilding && ısEditing;
		navigationPanToolStripButton.Checked = ısEditing && flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.Pan);
		navigationDefaultToolStripButton.Enabled = !ısBuilding && ısEditing;
		navigationDefaultToolStripButton.Checked = ısEditing && flowDesigner.IsStandardCommandChecked(WorkflowMenuCommands.DefaultFilter);
		onlineServicesToolStripButton.Visible = true;
		onlineServicesToolStripButton.Enabled = !ısBuilding;
		toolStripStatusProgressBar.Visible = ısBuilding;
	}

	private void ProjectObject_Loading(object sender, EventArgs e)
	{
		toolStripStatusProgressBar.Visible = true;
	}

	private void ProjectObject_Loaded(object sender, EventArgs e)
	{
		toolStripStatusProgressBar.Visible = false;
	}

	private void ProjectObject_NotLoaded(object sender, EventArgs e)
	{
		toolStripStatusProgressBar.Visible = false;
	}

	private void ProjectObject_LoadProgress(int progress)
	{
		toolStripStatusProgressBar.Value = progress;
		statusStrip.Refresh();
	}

	private void ProjectObject_NameChanged(object sender, EventArgs e)
	{
		Text = LocalizedResourceMgr.GetString("MainForm.Title") + " - " + projectObject.Name;
		AddRecentProject(projectObject.Path);
	}

	private void ProjectObject_Closed(object sender, EventArgs e)
	{
		projectObject = null;
		EnableDisableControls();
	}

	private void ProjectManagerControl_AvailableMenuCommandsChange(object sender, EventArgs e)
	{
		EnableDisableControls();
	}

	private void NewProjectDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CreateNewProject(NewProjectTypes.Dialer);
	}

	private void NewProjectCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CreateNewProject(NewProjectTypes.Callflow);
	}

	private void NewProjectTemplateToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CreateNewProject(NewProjectTypes.Template);
	}

	private void ImportProjectFromBuildOutputToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CreateNewProject(NewProjectTypes.ImportFromBuildOutput);
	}

	private void CreateNewProject(NewProjectTypes projectType)
	{
		if (projectObject != null)
		{
			if (MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Question.ProjectStillLoaded"), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			projectObject.Close();
		}
		try
		{
			switch (projectType)
			{
			case NewProjectTypes.Template:
			{
				CreateProjectFromTemplateForm createProjectFromTemplateForm = new CreateProjectFromTemplateForm();
				if (createProjectFromTemplateForm.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				ProjectTemplate selectedProjectTemplate = createProjectFromTemplateForm.SelectedProjectTemplate;
				saveNewProjectDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.DefaultProjectsFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.DefaultProjectsFolder);
				saveNewProjectDialog.FileName = selectedProjectTemplate.Folder + ".cfdproj";
				if (saveNewProjectDialog.ShowDialog() == DialogResult.OK)
				{
					Cursor = Cursors.WaitCursor;
					toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.CreatingProject");
					statusStrip.Refresh();
					FileInfo fileInfo = new FileInfo(saveNewProjectDialog.FileName);
					string directoryName = fileInfo.DirectoryName;
					string text = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
					if (!NameHelper.IsValidName(text))
					{
						throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectName"), text));
					}
					string text2 = Path.Combine(directoryName, text);
					Directory.CreateDirectory(text2);
					string fullPathToTemplate = selectedProjectTemplate.GetFullPathToTemplate();
					string[] directories = Directory.GetDirectories(fullPathToTemplate, "*", SearchOption.AllDirectories);
					for (int i = 0; i < directories.Length; i++)
					{
						Directory.CreateDirectory(directories[i].Replace(fullPathToTemplate, text2));
					}
					directories = Directory.GetFiles(fullPathToTemplate, "*.*", SearchOption.AllDirectories);
					foreach (string obj in directories)
					{
						File.Copy(obj, obj.Replace(fullPathToTemplate, text2), overwrite: true);
					}
					File.Move(Path.Combine(text2, selectedProjectTemplate.Folder + ".cfdproj"), Path.Combine(text2, fileInfo.Name));
					Settings.Default.LastOpenExistingProjectFolder = fileInfo.DirectoryName;
					projectObject = new ProjectObject(text2, fileInfo.Name, projectManagerControl);
					BindProjectObjectAndOpen();
				}
				return;
			}
			case NewProjectTypes.ImportFromBuildOutput:
			{
				ImportProjectFromBuildOutputWizardForm ımportProjectFromBuildOutputWizardForm = new ImportProjectFromBuildOutputWizardForm();
				if (ımportProjectFromBuildOutputWizardForm.ShowDialog() == DialogResult.OK)
				{
					LoadProject(ımportProjectFromBuildOutputWizardForm.ProjectFilePath);
				}
				return;
			}
			}
			saveNewProjectDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.DefaultProjectsFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.DefaultProjectsFolder);
			saveNewProjectDialog.FileName = string.Empty;
			if (saveNewProjectDialog.ShowDialog() == DialogResult.OK)
			{
				Cursor = Cursors.WaitCursor;
				toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.CreatingProject");
				statusStrip.Refresh();
				FileInfo fileInfo2 = new FileInfo(saveNewProjectDialog.FileName);
				string directoryName2 = fileInfo2.DirectoryName;
				string text3 = fileInfo2.Name.Substring(0, fileInfo2.Name.Length - fileInfo2.Extension.Length);
				if (!NameHelper.IsValidName(text3))
				{
					throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectName"), text3));
				}
				string text4 = Path.Combine(directoryName2, text3);
				Directory.CreateDirectory(text4);
				Settings.Default.LastOpenExistingProjectFolder = directoryName2;
				projectObject = new ProjectObject(text4, fileInfo2.Name, projectManagerControl, projectType);
				BindProjectObjectAndOpen();
			}
		}
		catch (Exception exc)
		{
			projectObject = null;
			projectManagerControl.SetProjectClosed();
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.CreatingProject") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
			EnableDisableControls();
		}
	}

	private void ToolbarNewProjectDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newProjectDialerToolStripMenuItem.PerformClick();
	}

	private void ToolbarNewProjectCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newProjectCallflowToolStripMenuItem.PerformClick();
	}

	private void ToolbarNewProjectTemplateToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newProjectTemplateToolStripMenuItem.PerformClick();
	}

	private void ToolbarImportProjectFromBuildOutputToolStripMenuItem_Click(object sender, EventArgs e)
	{
		importProjectFromBuildOutputToolStripMenuItem.PerformClick();
	}

	private void NewDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.CreateNewDialer();
	}

	private void ToolbarNewDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newDialerToolStripMenuItem.PerformClick();
	}

	private void NewCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.CreateNewCallflow();
	}

	private void ToolbarNewCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newCallflowToolStripMenuItem.PerformClick();
	}

	private void NewComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.CreateNewComponent();
	}

	private void ToolbarNewComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newComponentToolStripMenuItem.PerformClick();
	}

	private void NewFolderToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.CreateNewFolder();
	}

	private void ToolbarNewFolderToolStripMenuItem_Click(object sender, EventArgs e)
	{
		newFolderToolStripMenuItem.PerformClick();
	}

	private void OpenProjectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (projectObject != null)
		{
			if (MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Question.ProjectStillLoaded"), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			projectObject.Close();
		}
		try
		{
			openProjectDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.LastOpenExistingProjectFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.LastOpenExistingProjectFolder);
			openProjectDialog.FileName = string.Empty;
			if (openProjectDialog.ShowDialog() == DialogResult.OK)
			{
				Cursor = Cursors.WaitCursor;
				toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.LoadingProject");
				statusStrip.Refresh();
				FileInfo fileInfo = new FileInfo(openProjectDialog.FileName);
				if (!fileInfo.Exists)
				{
					throw new FileNotFoundException(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ProjectFileNotFound"));
				}
				Settings.Default.LastOpenExistingProjectFolder = fileInfo.DirectoryName;
				projectObject = new ProjectObject(fileInfo.DirectoryName, fileInfo.Name, projectManagerControl);
				BindProjectObjectAndOpen();
			}
		}
		catch (Exception exc)
		{
			projectObject = null;
			projectManagerControl.SetProjectClosed();
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.OpeningProject") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
			EnableDisableControls();
		}
	}

	private void ToolbarOpenProjectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		openProjectToolStripMenuItem.PerformClick();
	}

	private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.OpenFile();
	}

	private void ToolbarOpenFileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.OpenFile();
	}

	private void AddExistingDialerToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.AddExistingDialer();
	}

	private void AddExistingCallflowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.AddExistingCallflow();
	}

	private void AddExistingComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.AddExistingComponent();
	}

	private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.Rename();
	}

	private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ProjectExplorer.Remove();
	}

	private void ExportProjectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.ExportingProject");
			statusStrip.Refresh();
			projectObject.Export();
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ExportingProject") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
		}
	}

	private void CloseProjectToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectObject.Close();
	}

	private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.ClosingFile");
			statusStrip.Refresh();
			projectManagerControl.CloseFile();
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ClosingFile") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
		}
	}

	private void ProjectExplorer_OnSaveProjectAsRequested(object sender, EventArgs e)
	{
		saveProjectAsToolStripMenuItem.PerformClick();
	}

	private void SaveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			saveProjectAsDialog.InitialDirectory = (string.IsNullOrEmpty(Settings.Default.DefaultProjectsFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Personal) : Settings.Default.DefaultProjectsFolder);
			saveProjectAsDialog.FileName = projectObject.Name;
			if (saveProjectAsDialog.ShowDialog() == DialogResult.OK)
			{
				Cursor = Cursors.WaitCursor;
				toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.SavingProjectAs");
				statusStrip.Refresh();
				FileInfo fileInfo = new FileInfo(saveProjectAsDialog.FileName);
				string directoryName = fileInfo.DirectoryName;
				string text = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
				if (!NameHelper.IsValidName(text))
				{
					throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectName"), text));
				}
				string text2 = Path.Combine(directoryName, text);
				Directory.CreateDirectory(text2);
				projectObject.SaveTo(text2);
				projectObject.Name = text;
				AddRecentProject(projectObject.Path);
				saveAllToolStripMenuItem.PerformClick();
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.SavingProjectAs") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
			EnableDisableControls();
		}
	}

	private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.SavingFile");
			statusStrip.Refresh();
			projectManagerControl.SaveFile();
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.SavingFile") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
		}
	}

	private void SaveToolStripButton_Click(object sender, EventArgs e)
	{
		saveToolStripMenuItem.PerformClick();
	}

	private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.SavingAll");
			statusStrip.Refresh();
			projectManagerControl.SaveAll();
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.SavingAll") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Ready");
		}
	}

	private void SaveAllToolStripButton_Click(object sender, EventArgs e)
	{
		saveAllToolStripMenuItem.PerformClick();
	}

	private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.Undo);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Undo") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UndoToolStripButton_Click(object sender, EventArgs e)
	{
		undoToolStripMenuItem.PerformClick();
	}

	private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.Redo);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Redo") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RedoToolStripButton_Click(object sender, EventArgs e)
	{
		redoToolStripMenuItem.PerformClick();
	}

	private void CutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.Cut);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Cut") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CutToolStripButton_Click(object sender, EventArgs e)
	{
		cutToolStripMenuItem.PerformClick();
	}

	private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.Copy);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Copy") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CopyToolStripButton_Click(object sender, EventArgs e)
	{
		copyToolStripMenuItem.PerformClick();
	}

	private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.Paste);
		}
		catch (Exception)
		{
		}
	}

	private void PasteToolStripButton_Click(object sender, EventArgs e)
	{
		pasteToolStripMenuItem.PerformClick();
	}

	private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.Delete);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Delete") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeleteToolStripButton_Click(object sender, EventArgs e)
	{
		deleteToolStripMenuItem.PerformClick();
	}

	private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(StandardCommands.SelectAll);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.SelectAll") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ViewComponentsToolbarToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowComponentsToolbar();
	}

	private void ViewProjectExplorerWindowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowProjectExplorerWindow();
	}

	private void ViewPropertiesWindowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowPropertiesWindow();
	}

	private void ViewOutputWindowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowOutputWindow();
	}

	private void ViewErrorListWindowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowErrorListWindow(pin: false);
	}

	private void ViewDebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowDebugWindow();
	}

	private void ViewStartPageToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.ShowStartPage(CreateStartPage);
	}

	private void ToolbarToolStripMenuItem_Click(object sender, EventArgs e)
	{
		toolStrip.Visible = toolbarToolStripMenuItem.Checked;
	}

	private void StatusbarToolStripMenuItem_Click(object sender, EventArgs e)
	{
		statusStrip.Visible = statusbarToolStripMenuItem.Checked;
	}

	private void DebugBuildToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Building");
			statusStrip.Refresh();
			projectManagerControl.BuildProject(BuildTypes.Debug);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Building") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
		}
	}

	private void DebugBuildToolStripButton_Click(object sender, EventArgs e)
	{
		debugBuildToolStripMenuItem.PerformClick();
	}

	private void ReleaseBuildToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Building");
			statusStrip.Refresh();
			projectManagerControl.BuildProject(BuildTypes.Release);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Building") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
		}
	}

	private void ReleaseBuildToolStripButton_Click(object sender, EventArgs e)
	{
		releaseBuildToolStripMenuItem.PerformClick();
	}

	private void BuildAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			toolStripStatusLabel.Text = LocalizedResourceMgr.GetString("MainForm.Statusbar.Building");
			statusStrip.Refresh();
			projectManagerControl.BuildProject(BuildTypes.Release);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Building") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
		}
	}

	private void BuildAllToolStripButton_Click(object sender, EventArgs e)
	{
		buildAllToolStripMenuItem.PerformClick();
	}

	private void CancelBuildToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.CancelBuild();
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.CancellingBuild") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CancelBuildToolStripButton_Click(object sender, EventArgs e)
	{
		cancelBuildToolStripMenuItem.PerformClick();
	}

	private void BuildProjectExtensionToolStripMenuItem_Click(object sender, EventArgs e)
	{
		new ProjectExtensionForm(projectObject).ShowDialog();
	}

	private void BuildProjectExtensionToolStripButton_Click(object sender, EventArgs e)
	{
		buildProjectExtensionToolStripMenuItem.PerformClick();
	}

	private void DebugStartToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void DebugStartToolStripButton_Click(object sender, EventArgs e)
	{
		debugStartToolStripMenuItem.PerformClick();
	}

	private void DebugStopToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.StopDebugging();
	}

	private void DebugStopToolStripButton_Click(object sender, EventArgs e)
	{
		debugStopToolStripMenuItem.PerformClick();
	}

	private void DebugStepOverToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.StepOver();
	}

	private void DebugStepOverToolStripButton_Click(object sender, EventArgs e)
	{
		debugStepOverToolStripMenuItem.PerformClick();
	}

	private void DebugStepIntoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.StepInto();
	}

	private void DebugStepIntoToolStripButton_Click(object sender, EventArgs e)
	{
		debugStepIntoToolStripMenuItem.PerformClick();
	}

	private void DebugStepOutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		projectManagerControl.StepOut();
	}

	private void DebugStepOutToolStripButton_Click(object sender, EventArgs e)
	{
		debugStepOutToolStripMenuItem.PerformClick();
	}

	private void EnableComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.Enable);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Enable") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void EnableToolStripButton_Click(object sender, EventArgs e)
	{
		enableComponentToolStripMenuItem.PerformClick();
	}

	private void DisableComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.Disable);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Disable") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DisableToolStripButton_Click(object sender, EventArgs e)
	{
		disableComponentToolStripMenuItem.PerformClick();
	}

	private void ExpandComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.Expand);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Expand") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ExpandToolStripButton_Click(object sender, EventArgs e)
	{
		expandComponentToolStripMenuItem.PerformClick();
	}

	private void CollapseComponentToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.Collapse);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.Collapse") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CollapseToolStripButton_Click(object sender, EventArgs e)
	{
		collapseComponentToolStripMenuItem.PerformClick();
	}

	private void Zoom400ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "400%";
		ChangeZoomLevel();
	}

	private void Zoom300ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "300%";
		ChangeZoomLevel();
	}

	private void Zoom200ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "200%";
		ChangeZoomLevel();
	}

	private void Zoom150ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "150%";
		ChangeZoomLevel();
	}

	private void Zoom100ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "100%";
		ChangeZoomLevel();
	}

	private void Zoom75ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "75%";
		ChangeZoomLevel();
	}

	private void Zoom50ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = "50%";
		ChangeZoomLevel();
	}

	private void ZoomShowAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		zoomLevelToolStripComboBox.Text = LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevelShowAll");
		ChangeZoomLevel();
	}

	private void ZoomLevelToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		ChangeZoomLevel();
	}

	private void ZoomLevelToolStripComboBox_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			ChangeZoomLevel();
		}
	}

	private void ZoomLevelToolStripComboBox_Leave(object sender, EventArgs e)
	{
		ChangeZoomLevel();
	}

	private void ChangeZoomLevel()
	{
		if (!projectManagerControl.IsEditing)
		{
			return;
		}
		try
		{
			if (zoomLevelToolStripComboBox.Text == LocalizedResourceMgr.GetString("MainForm.Menu.ToolsZoomLevelShowAll"))
			{
				projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.ShowAll);
			}
			else
			{
				string text = zoomLevelToolStripComboBox.Text;
				if (text.EndsWith("%"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				projectManagerControl.FlowDesigner.CurrentView.Zoom = Convert.ToInt32(text);
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ChangingZoomLevel") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		zoomLevelToolStripComboBox.Text = $"{projectManagerControl.FlowDesigner.CurrentView.Zoom}%";
	}

	private void ZoomInToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.ZoomIn);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ZoomIn") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ZoomInToolStripButton_Click(object sender, EventArgs e)
	{
		zoomInToolStripMenuItem.PerformClick();
	}

	private void ZoomOutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.ZoomOut);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.ZoomOut") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ZoomOutToolStripButton_Click(object sender, EventArgs e)
	{
		zoomOutToolStripMenuItem.PerformClick();
	}

	private void NavigationPanToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.Pan);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.NavigationPan") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void NavigationPanToolStripButton_Click(object sender, EventArgs e)
	{
		navigationPanToolStripMenuItem.PerformClick();
	}

	private void NavigationDefaultToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			projectManagerControl.FlowDesigner.InvokeStandardCommand(WorkflowMenuCommands.DefaultFilter);
		}
		catch (Exception exc)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MainForm.MessageBox.Error.NavigationDefault") + ErrorHelper.GetErrorDescription(exc), LocalizedResourceMgr.GetString("MainForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void NavigationDefaultToolStripButton_Click(object sender, EventArgs e)
	{
		navigationDefaultToolStripMenuItem.PerformClick();
	}

	private void OnlineServicesToolStripMenuItem_Click(object sender, EventArgs e)
	{
		OnlineServicesConfigurationForm onlineServicesConfigurationForm = new OnlineServicesConfigurationForm
		{
			OnlineServices = projectObject.OnlineServices
		};
		if (onlineServicesConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			projectObject.OnlineServices = onlineServicesConfigurationForm.OnlineServices;
			projectManagerControl.OnlineServicesUpdated();
		}
	}

	private void OnlineServicesToolStripButton_Click(object sender, EventArgs e)
	{
		onlineServicesToolStripMenuItem.PerformClick();
	}

	private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (new OptionsForm().ShowDialog() == DialogResult.OK)
		{
			ReloadRecentProjects();
		}
	}

	private void OptionsToolStripButton_Click(object sender, EventArgs e)
	{
		optionsToolStripMenuItem.PerformClick();
	}

	private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Process.Start(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "updater.exe"), "/checknow");
	}

	private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		new AboutForm().ShowDialog();
	}

	private void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/call-flow-designer-manual/");
	}

	private void DocumentationToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ShowHelp();
	}

	private void MainForm_HelpRequested(object sender, EventArgs e)
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
		System.Windows.Forms.ToolStripProfessionalRenderer toolStripProfessionalRenderer = new System.Windows.Forms.ToolStripProfessionalRenderer();
		this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
		this.statusStrip = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
		this.projectManagerControl = new TCX.CFD.Controls.ProjectManagerControl();
		this.menuStrip = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newProjectCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newProjectDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newProjectTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.importProjectFromBuildOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openDialerFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openCallflowFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openComponentFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addExistingDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addExistingCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.addExistingComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
		this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.removeComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
		this.exportProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.closeComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.saveProjectAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.recentProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
		this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewComponentsToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewProjectExplorerWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewPropertiesWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewOutputWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewErrorListWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewDebugWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
		this.viewStartPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
		this.toolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.statusbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.debugBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.releaseBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.buildAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.cancelBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.buildProjectExtensionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.debugStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.debugStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
		this.debugStepOverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.debugStepIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.debugStepOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.enableComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.disableComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
		this.expandComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.collapseComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoomLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom400ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom300ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom200ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom150ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom75ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoom50ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
		this.zoomShowAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.navigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.navigationPanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.navigationDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
		this.onlineServicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
		this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStrip = new System.Windows.Forms.ToolStrip();
		this.newToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
		this.toolbarNewProjectDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarNewProjectCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarNewProjectTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarImportProjectFromBuildOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarNewDialerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarNewCallflowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarNewComponentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarNewFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
		this.toolbarOpenProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarOpenDialerFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarOpenCallflowFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolbarOpenComponentFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
		this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.saveAllToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.undoToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.redoToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
		this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.debugBuildToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.releaseBuildToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.buildAllToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.cancelBuildToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.buildProjectExtensionToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.buildToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.debugStartToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.debugStopToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.debugStepOverToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.debugStepIntoToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.debugStepOutToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.debugToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.enableToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.disableToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.expandToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.collapseToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.zoomLevelToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
		this.zoomInToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.zoomOutToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.navigationPanToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.navigationDefaultToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolsToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.onlineServicesToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.optionsToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.saveProjectAsDialog = new System.Windows.Forms.SaveFileDialog();
		this.saveNewProjectDialog = new System.Windows.Forms.SaveFileDialog();
		this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
		this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
		this.toolStripContainer1.ContentPanel.SuspendLayout();
		this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
		this.toolStripContainer1.SuspendLayout();
		this.statusStrip.SuspendLayout();
		this.menuStrip.SuspendLayout();
		this.toolStrip.SuspendLayout();
		base.SuspendLayout();
		this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
		this.toolStripContainer1.ContentPanel.Controls.Add(this.projectManagerControl);
		this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(4);
		this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1204, 616);
		this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStripContainer1.LeftToolStripPanelVisible = false;
		this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
		this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(4);
		this.toolStripContainer1.Name = "toolStripContainer1";
		this.toolStripContainer1.RightToolStripPanelVisible = false;
		this.toolStripContainer1.Size = new System.Drawing.Size(1204, 697);
		this.toolStripContainer1.TabIndex = 0;
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
		this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripStatusLabel, this.toolStripStatusProgressBar });
		this.statusStrip.Location = new System.Drawing.Point(0, 0);
		this.statusStrip.Name = "statusStrip";
		this.statusStrip.Size = new System.Drawing.Size(1204, 25);
		this.statusStrip.TabIndex = 0;
		this.toolStripStatusLabel.Name = "toolStripStatusLabel";
		this.toolStripStatusLabel.Size = new System.Drawing.Size(1087, 20);
		this.toolStripStatusLabel.Spring = true;
		this.toolStripStatusLabel.Text = "Ready.";
		this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.toolStripStatusProgressBar.Name = "toolStripStatusProgressBar";
		this.toolStripStatusProgressBar.Size = new System.Drawing.Size(100, 19);
		this.toolStripStatusProgressBar.Step = 1;
		this.toolStripStatusProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.projectManagerControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.projectManagerControl.Location = new System.Drawing.Point(0, 0);
		this.projectManagerControl.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.projectManagerControl.Name = "projectManagerControl";
		toolStripProfessionalRenderer.RoundedEdges = true;
		this.projectManagerControl.Renderer = toolStripProfessionalRenderer;
		this.projectManagerControl.Size = new System.Drawing.Size(1204, 616);
		this.projectManagerControl.TabIndex = 0;
		this.projectManagerControl.AvailableMenuCommandsChange += new System.EventHandler(ProjectManagerControl_AvailableMenuCommandsChange);
		this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.fileToolStripMenuItem, this.editToolStripMenuItem, this.viewToolStripMenuItem, this.buildToolStripMenuItem, this.debugToolStripMenuItem, this.toolsToolStripMenuItem, this.helpToolStripMenuItem });
		this.menuStrip.Location = new System.Drawing.Point(0, 0);
		this.menuStrip.Name = "menuStrip";
		this.menuStrip.Size = new System.Drawing.Size(1204, 28);
		this.menuStrip.TabIndex = 0;
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[23]
		{
			this.newToolStripMenuItem, this.openToolStripMenuItem, this.addToolStripMenuItem, this.toolStripSeparator9, this.renameToolStripMenuItem, this.removeFolderToolStripMenuItem, this.removeCallflowToolStripMenuItem, this.removeDialerToolStripMenuItem, this.removeComponentToolStripMenuItem, this.toolStripSeparator10,
			this.exportProjectToolStripMenuItem, this.closeProjectToolStripMenuItem, this.closeCallflowToolStripMenuItem, this.closeDialerToolStripMenuItem, this.closeComponentToolStripMenuItem, this.toolStripSeparator, this.saveProjectAsToolStripMenuItem, this.saveToolStripMenuItem, this.saveAllToolStripMenuItem, this.toolStripSeparator1,
			this.recentProjectsToolStripMenuItem, this.toolStripSeparator17, this.exitToolStripMenuItem
		});
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
		this.fileToolStripMenuItem.Text = "&File";
		this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.newProjectCallflowToolStripMenuItem, this.newProjectDialerToolStripMenuItem, this.newProjectTemplateToolStripMenuItem, this.importProjectFromBuildOutputToolStripMenuItem, this.newCallflowToolStripMenuItem, this.newDialerToolStripMenuItem, this.newComponentToolStripMenuItem, this.newFolderToolStripMenuItem });
		this.newToolStripMenuItem.Name = "newToolStripMenuItem";
		this.newToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.newToolStripMenuItem.Text = "New";
		this.newProjectCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewProject;
		this.newProjectCallflowToolStripMenuItem.Name = "newProjectCallflowToolStripMenuItem";
		this.newProjectCallflowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
		this.newProjectCallflowToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newProjectCallflowToolStripMenuItem.Text = "Call&flow Project...";
		this.newProjectCallflowToolStripMenuItem.Click += new System.EventHandler(NewProjectCallflowToolStripMenuItem_Click);
		this.newProjectDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewDialer;
		this.newProjectDialerToolStripMenuItem.Name = "newProjectDialerToolStripMenuItem";
		this.newProjectDialerToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newProjectDialerToolStripMenuItem.Text = "Dia&ler Project...";
		this.newProjectDialerToolStripMenuItem.Click += new System.EventHandler(NewProjectDialerToolStripMenuItem_Click);
		this.newProjectTemplateToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewProjectFromTemplate;
		this.newProjectTemplateToolStripMenuItem.Name = "newProjectTemplateToolStripMenuItem";
		this.newProjectTemplateToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newProjectTemplateToolStripMenuItem.Text = "Project from &Template...";
		this.newProjectTemplateToolStripMenuItem.Visible = false;
		this.newProjectTemplateToolStripMenuItem.Click += new System.EventHandler(NewProjectTemplateToolStripMenuItem_Click);
		this.importProjectFromBuildOutputToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_ImportProject;
		this.importProjectFromBuildOutputToolStripMenuItem.Name = "importProjectFromBuildOutputToolStripMenuItem";
		this.importProjectFromBuildOutputToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.I | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
		this.importProjectFromBuildOutputToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.importProjectFromBuildOutputToolStripMenuItem.Text = "&Import Project from Build Output...";
		this.importProjectFromBuildOutputToolStripMenuItem.Click += new System.EventHandler(ImportProjectFromBuildOutputToolStripMenuItem_Click);
		this.newDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewDialer;
		this.newDialerToolStripMenuItem.Name = "newDialerToolStripMenuItem";
		this.newDialerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Control;
		this.newDialerToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newDialerToolStripMenuItem.Text = "&Dialer";
		this.newDialerToolStripMenuItem.Click += new System.EventHandler(NewDialerToolStripMenuItem_Click);
		this.newCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewCallflow;
		this.newCallflowToolStripMenuItem.Name = "newCallflowToolStripMenuItem";
		this.newCallflowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Control;
		this.newCallflowToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newCallflowToolStripMenuItem.Text = "&Callflow";
		this.newCallflowToolStripMenuItem.Click += new System.EventHandler(NewCallflowToolStripMenuItem_Click);
		this.newComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewUserComponent;
		this.newComponentToolStripMenuItem.Name = "newComponentToolStripMenuItem";
		this.newComponentToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.M | System.Windows.Forms.Keys.Control;
		this.newComponentToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newComponentToolStripMenuItem.Text = "C&omponent";
		this.newComponentToolStripMenuItem.Click += new System.EventHandler(NewComponentToolStripMenuItem_Click);
		this.newFolderToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewFolder;
		this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
		this.newFolderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control;
		this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
		this.newFolderToolStripMenuItem.Text = "&Folder";
		this.newFolderToolStripMenuItem.Click += new System.EventHandler(NewFolderToolStripMenuItem_Click);
		this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.openProjectToolStripMenuItem, this.openCallflowFileToolStripMenuItem, this.openDialerFileToolStripMenuItem, this.openComponentFileToolStripMenuItem });
		this.openToolStripMenuItem.Name = "openToolStripMenuItem";
		this.openToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.openToolStripMenuItem.Text = "Open";
		this.openProjectToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenProject;
		this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
		this.openProjectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control;
		this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
		this.openProjectToolStripMenuItem.Text = "&Project...";
		this.openProjectToolStripMenuItem.Click += new System.EventHandler(OpenProjectToolStripMenuItem_Click);
		this.openDialerFileToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenDialer;
		this.openDialerFileToolStripMenuItem.Name = "openDialerFileToolStripMenuItem";
		this.openDialerFileToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
		this.openDialerFileToolStripMenuItem.Text = "&Dialer";
		this.openDialerFileToolStripMenuItem.Click += new System.EventHandler(OpenFileToolStripMenuItem_Click);
		this.openCallflowFileToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenCallflow;
		this.openCallflowFileToolStripMenuItem.Name = "openCallflowFileToolStripMenuItem";
		this.openCallflowFileToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
		this.openCallflowFileToolStripMenuItem.Text = "&Callflow";
		this.openCallflowFileToolStripMenuItem.Click += new System.EventHandler(OpenFileToolStripMenuItem_Click);
		this.openComponentFileToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenUserComponent;
		this.openComponentFileToolStripMenuItem.Name = "openComponentFileToolStripMenuItem";
		this.openComponentFileToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
		this.openComponentFileToolStripMenuItem.Text = "C&omponent";
		this.openComponentFileToolStripMenuItem.Click += new System.EventHandler(OpenFileToolStripMenuItem_Click);
		this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.addExistingCallflowToolStripMenuItem, this.addExistingDialerToolStripMenuItem, this.addExistingComponentToolStripMenuItem });
		this.addToolStripMenuItem.Name = "addToolStripMenuItem";
		this.addToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.addToolStripMenuItem.Text = "Add";
		this.addExistingDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_AddExistingDialer;
		this.addExistingDialerToolStripMenuItem.Name = "addExistingDialerToolStripMenuItem";
		this.addExistingDialerToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
		this.addExistingDialerToolStripMenuItem.Text = "Existing &Dialer";
		this.addExistingDialerToolStripMenuItem.Click += new System.EventHandler(AddExistingDialerToolStripMenuItem_Click);
		this.addExistingCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_AddExistingCallflow;
		this.addExistingCallflowToolStripMenuItem.Name = "addExistingCallflowToolStripMenuItem";
		this.addExistingCallflowToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
		this.addExistingCallflowToolStripMenuItem.Text = "Existing &Callflow";
		this.addExistingCallflowToolStripMenuItem.Click += new System.EventHandler(AddExistingCallflowToolStripMenuItem_Click);
		this.addExistingComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_AddExistingUserComponent;
		this.addExistingComponentToolStripMenuItem.Name = "addExistingComponentToolStripMenuItem";
		this.addExistingComponentToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
		this.addExistingComponentToolStripMenuItem.Text = "Existing &Component";
		this.addExistingComponentToolStripMenuItem.Click += new System.EventHandler(AddExistingComponentToolStripMenuItem_Click);
		this.toolStripSeparator9.Name = "toolStripSeparator9";
		this.toolStripSeparator9.Size = new System.Drawing.Size(224, 6);
		this.renameToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_Rename;
		this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
		this.renameToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.renameToolStripMenuItem.Text = "Rename";
		this.renameToolStripMenuItem.Click += new System.EventHandler(RenameToolStripMenuItem_Click);
		this.removeFolderToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveFolder;
		this.removeFolderToolStripMenuItem.Name = "removeFolderToolStripMenuItem";
		this.removeFolderToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.removeFolderToolStripMenuItem.Text = "&Remove Folder";
		this.removeFolderToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.removeDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveDialer;
		this.removeDialerToolStripMenuItem.Name = "removeDialerToolStripMenuItem";
		this.removeDialerToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.removeDialerToolStripMenuItem.Text = "&Remove Dialer";
		this.removeDialerToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.removeCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveCallflow;
		this.removeCallflowToolStripMenuItem.Name = "removeCallflowToolStripMenuItem";
		this.removeCallflowToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.removeCallflowToolStripMenuItem.Text = "&Remove Callflow";
		this.removeCallflowToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.removeComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_RemoveComponent;
		this.removeComponentToolStripMenuItem.Name = "removeComponentToolStripMenuItem";
		this.removeComponentToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.removeComponentToolStripMenuItem.Text = "&Remove Component";
		this.removeComponentToolStripMenuItem.Click += new System.EventHandler(RemoveToolStripMenuItem_Click);
		this.toolStripSeparator10.Name = "toolStripSeparator10";
		this.toolStripSeparator10.Size = new System.Drawing.Size(224, 6);
		this.exportProjectToolStripMenuItem.Name = "exportProjectToolStripMenuItem";
		this.exportProjectToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.exportProjectToolStripMenuItem.Text = "Export Project";
		this.exportProjectToolStripMenuItem.Click += new System.EventHandler(ExportProjectToolStripMenuItem_Click);
		this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
		this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.closeProjectToolStripMenuItem.Text = "Close Project";
		this.closeProjectToolStripMenuItem.Click += new System.EventHandler(CloseProjectToolStripMenuItem_Click);
		this.closeDialerToolStripMenuItem.Name = "closeDialerToolStripMenuItem";
		this.closeDialerToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.closeDialerToolStripMenuItem.Text = "Close Dialer";
		this.closeDialerToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.closeCallflowToolStripMenuItem.Name = "closeCallflowToolStripMenuItem";
		this.closeCallflowToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.closeCallflowToolStripMenuItem.Text = "Close Callflow";
		this.closeCallflowToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.closeComponentToolStripMenuItem.Name = "closeComponentToolStripMenuItem";
		this.closeComponentToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.closeComponentToolStripMenuItem.Text = "Close Component";
		this.closeComponentToolStripMenuItem.Click += new System.EventHandler(CloseToolStripMenuItem_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(224, 6);
		this.saveProjectAsToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_Save_As;
		this.saveProjectAsToolStripMenuItem.Name = "saveProjectAsToolStripMenuItem";
		this.saveProjectAsToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.saveProjectAsToolStripMenuItem.Text = "Save Project As";
		this.saveProjectAsToolStripMenuItem.Click += new System.EventHandler(SaveProjectAsToolStripMenuItem_Click);
		this.saveToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_Save;
		this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
		this.saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control;
		this.saveToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.saveToolStripMenuItem.Text = "&Save";
		this.saveToolStripMenuItem.Click += new System.EventHandler(SaveToolStripMenuItem_Click);
		this.saveAllToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_SaveAll;
		this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
		this.saveAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
		this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.saveAllToolStripMenuItem.Text = "Save A&ll";
		this.saveAllToolStripMenuItem.Click += new System.EventHandler(SaveAllToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(224, 6);
		this.recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
		this.recentProjectsToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.recentProjectsToolStripMenuItem.Text = "Recent Projects";
		this.toolStripSeparator17.Name = "toolStripSeparator17";
		this.toolStripSeparator17.Size = new System.Drawing.Size(224, 6);
		this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
		this.exitToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
		this.exitToolStripMenuItem.Text = "E&xit";
		this.exitToolStripMenuItem.Click += new System.EventHandler(ExitToolStripMenuItem_Click);
		this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.undoToolStripMenuItem, this.redoToolStripMenuItem, this.toolStripSeparator3, this.cutToolStripMenuItem, this.copyToolStripMenuItem, this.pasteToolStripMenuItem, this.deleteToolStripMenuItem, this.toolStripSeparator4, this.selectAllToolStripMenuItem });
		this.editToolStripMenuItem.Name = "editToolStripMenuItem";
		this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
		this.editToolStripMenuItem.Text = "&Edit";
		this.undoToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Edit_Undo;
		this.undoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
		this.undoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control;
		this.undoToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.undoToolStripMenuItem.Text = "&Undo";
		this.undoToolStripMenuItem.Click += new System.EventHandler(UndoToolStripMenuItem_Click);
		this.redoToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Edit_Redo;
		this.redoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
		this.redoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Y | System.Windows.Forms.Keys.Control;
		this.redoToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.redoToolStripMenuItem.Text = "&Redo";
		this.redoToolStripMenuItem.Click += new System.EventHandler(RedoToolStripMenuItem_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(195, 6);
		this.cutToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Edit_Cut;
		this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
		this.cutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
		this.cutToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.cutToolStripMenuItem.Text = "Cu&t";
		this.cutToolStripMenuItem.Click += new System.EventHandler(CutToolStripMenuItem_Click);
		this.copyToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Edit_Copy;
		this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
		this.copyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
		this.copyToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.copyToolStripMenuItem.Text = "&Copy";
		this.copyToolStripMenuItem.Click += new System.EventHandler(CopyToolStripMenuItem_Click);
		this.pasteToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Edit_Paste;
		this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
		this.pasteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Control;
		this.pasteToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.pasteToolStripMenuItem.Text = "&Paste";
		this.pasteToolStripMenuItem.Click += new System.EventHandler(PasteToolStripMenuItem_Click);
		this.deleteToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Edit_Delete;
		this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
		this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
		this.deleteToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.deleteToolStripMenuItem.Text = "&Delete";
		this.deleteToolStripMenuItem.Click += new System.EventHandler(DeleteToolStripMenuItem_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(195, 6);
		this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
		this.selectAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control;
		this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
		this.selectAllToolStripMenuItem.Text = "Select &All";
		this.selectAllToolStripMenuItem.Click += new System.EventHandler(SelectAllToolStripMenuItem_Click);
		this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.viewComponentsToolbarToolStripMenuItem, this.viewProjectExplorerWindowToolStripMenuItem, this.viewPropertiesWindowToolStripMenuItem, this.viewOutputWindowToolStripMenuItem, this.viewErrorListWindowToolStripMenuItem, this.viewDebugWindowToolStripMenuItem, this.toolStripSeparator22, this.viewStartPageToolStripMenuItem, this.toolStripSeparator11, this.toolbarToolStripMenuItem,
			this.statusbarToolStripMenuItem
		});
		this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
		this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
		this.viewToolStripMenuItem.Text = "&View";
		this.viewComponentsToolbarToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_Toolbox;
		this.viewComponentsToolbarToolStripMenuItem.Name = "viewComponentsToolbarToolStripMenuItem";
		this.viewComponentsToolbarToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.T | System.Windows.Forms.Keys.Alt;
		this.viewComponentsToolbarToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewComponentsToolbarToolStripMenuItem.Text = "&Components Toolbox";
		this.viewComponentsToolbarToolStripMenuItem.Click += new System.EventHandler(ViewComponentsToolbarToolStripMenuItem_Click);
		this.viewProjectExplorerWindowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_ProjectExplorer;
		this.viewProjectExplorerWindowToolStripMenuItem.Name = "viewProjectExplorerWindowToolStripMenuItem";
		this.viewProjectExplorerWindowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Alt;
		this.viewProjectExplorerWindowToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewProjectExplorerWindowToolStripMenuItem.Text = "Project &Explorer";
		this.viewProjectExplorerWindowToolStripMenuItem.Click += new System.EventHandler(ViewProjectExplorerWindowToolStripMenuItem_Click);
		this.viewPropertiesWindowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_PropertiesWindow;
		this.viewPropertiesWindowToolStripMenuItem.Name = "viewPropertiesWindowToolStripMenuItem";
		this.viewPropertiesWindowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Alt;
		this.viewPropertiesWindowToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewPropertiesWindowToolStripMenuItem.Text = "&Properties";
		this.viewPropertiesWindowToolStripMenuItem.Click += new System.EventHandler(ViewPropertiesWindowToolStripMenuItem_Click);
		this.viewOutputWindowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_OutputWindow;
		this.viewOutputWindowToolStripMenuItem.Name = "viewOutputWindowToolStripMenuItem";
		this.viewOutputWindowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Alt;
		this.viewOutputWindowToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewOutputWindowToolStripMenuItem.Text = "&Output";
		this.viewOutputWindowToolStripMenuItem.Click += new System.EventHandler(ViewOutputWindowToolStripMenuItem_Click);
		this.viewErrorListWindowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_ErrorListWindow;
		this.viewErrorListWindowToolStripMenuItem.Name = "viewErrorListWindowToolStripMenuItem";
		this.viewErrorListWindowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.E | System.Windows.Forms.Keys.Alt;
		this.viewErrorListWindowToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewErrorListWindowToolStripMenuItem.Text = "E&rror List";
		this.viewErrorListWindowToolStripMenuItem.Click += new System.EventHandler(ViewErrorListWindowToolStripMenuItem_Click);
		this.viewDebugWindowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_DebugWindow;
		this.viewDebugWindowToolStripMenuItem.Name = "viewDebugWindowToolStripMenuItem";
		this.viewDebugWindowToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Alt;
		this.viewDebugWindowToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewDebugWindowToolStripMenuItem.Text = "&Debug";
		this.viewDebugWindowToolStripMenuItem.Click += new System.EventHandler(ViewDebugWindowToolStripMenuItem_Click);
		this.toolStripSeparator22.Name = "toolStripSeparator22";
		this.toolStripSeparator22.Size = new System.Drawing.Size(223, 6);
		this.viewStartPageToolStripMenuItem.Image = TCX.CFD.Properties.Resources.View_StartPage;
		this.viewStartPageToolStripMenuItem.Name = "viewStartPageToolStripMenuItem";
		this.viewStartPageToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Alt;
		this.viewStartPageToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.viewStartPageToolStripMenuItem.Text = "&Start Page";
		this.viewStartPageToolStripMenuItem.Click += new System.EventHandler(ViewStartPageToolStripMenuItem_Click);
		this.toolStripSeparator11.Name = "toolStripSeparator11";
		this.toolStripSeparator11.Size = new System.Drawing.Size(223, 6);
		this.toolbarToolStripMenuItem.Checked = true;
		this.toolbarToolStripMenuItem.CheckOnClick = true;
		this.toolbarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
		this.toolbarToolStripMenuItem.Name = "toolbarToolStripMenuItem";
		this.toolbarToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.toolbarToolStripMenuItem.Text = "&Toolbar";
		this.toolbarToolStripMenuItem.Click += new System.EventHandler(ToolbarToolStripMenuItem_Click);
		this.statusbarToolStripMenuItem.Checked = true;
		this.statusbarToolStripMenuItem.CheckOnClick = true;
		this.statusbarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
		this.statusbarToolStripMenuItem.Name = "statusbarToolStripMenuItem";
		this.statusbarToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
		this.statusbarToolStripMenuItem.Text = "Status &bar";
		this.statusbarToolStripMenuItem.Click += new System.EventHandler(StatusbarToolStripMenuItem_Click);
		this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.debugBuildToolStripMenuItem, this.releaseBuildToolStripMenuItem, this.buildAllToolStripMenuItem, this.cancelBuildToolStripMenuItem, this.buildProjectExtensionToolStripMenuItem });
		this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
		this.buildToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
		this.buildToolStripMenuItem.Text = "&Build";
		this.debugBuildToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_DebugBuild;
		this.debugBuildToolStripMenuItem.Name = "debugBuildToolStripMenuItem";
		this.debugBuildToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
		this.debugBuildToolStripMenuItem.Text = "&Debug Build";
		this.debugBuildToolStripMenuItem.Click += new System.EventHandler(DebugBuildToolStripMenuItem_Click);
		this.releaseBuildToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_ReleaseBuild;
		this.releaseBuildToolStripMenuItem.Name = "releaseBuildToolStripMenuItem";
		this.releaseBuildToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
		this.releaseBuildToolStripMenuItem.Text = "&Release Build";
		this.releaseBuildToolStripMenuItem.Click += new System.EventHandler(ReleaseBuildToolStripMenuItem_Click);
		this.buildAllToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_BuildAll;
		this.buildAllToolStripMenuItem.Name = "buildAllToolStripMenuItem";
		this.buildAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Control;
		this.buildAllToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
		this.buildAllToolStripMenuItem.Text = "Build &All";
		this.buildAllToolStripMenuItem.Click += new System.EventHandler(BuildAllToolStripMenuItem_Click);
		this.cancelBuildToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_CancelBuild;
		this.cancelBuildToolStripMenuItem.Name = "cancelBuildToolStripMenuItem";
		this.cancelBuildToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
		this.cancelBuildToolStripMenuItem.Text = "&Cancel Build";
		this.cancelBuildToolStripMenuItem.Click += new System.EventHandler(CancelBuildToolStripMenuItem_Click);
		this.buildProjectExtensionToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Build_SetExtension;
		this.buildProjectExtensionToolStripMenuItem.Name = "buildProjectExtensionToolStripMenuItem";
		this.buildProjectExtensionToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
		this.buildProjectExtensionToolStripMenuItem.Text = "&Project Extension...";
		this.buildProjectExtensionToolStripMenuItem.Click += new System.EventHandler(BuildProjectExtensionToolStripMenuItem_Click);
		this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.debugStartToolStripMenuItem, this.debugStopToolStripMenuItem, this.toolStripSeparator20, this.debugStepOverToolStripMenuItem, this.debugStepIntoToolStripMenuItem, this.debugStepOutToolStripMenuItem });
		this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
		this.debugToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
		this.debugToolStripMenuItem.Text = "&Debug";
		this.debugStartToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Debug_StartDebugging;
		this.debugStartToolStripMenuItem.Name = "debugStartToolStripMenuItem";
		this.debugStartToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
		this.debugStartToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
		this.debugStartToolStripMenuItem.Text = "&Start Debugging";
		this.debugStartToolStripMenuItem.Click += new System.EventHandler(DebugStartToolStripMenuItem_Click);
		this.debugStopToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Debug_StopDebugging;
		this.debugStopToolStripMenuItem.Name = "debugStopToolStripMenuItem";
		this.debugStopToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
		this.debugStopToolStripMenuItem.Text = "S&top Debugging";
		this.debugStopToolStripMenuItem.Click += new System.EventHandler(DebugStopToolStripMenuItem_Click);
		this.toolStripSeparator20.Name = "toolStripSeparator20";
		this.toolStripSeparator20.Size = new System.Drawing.Size(215, 6);
		this.debugStepOverToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Debug_StepOver;
		this.debugStepOverToolStripMenuItem.Name = "debugStepOverToolStripMenuItem";
		this.debugStepOverToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
		this.debugStepOverToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
		this.debugStepOverToolStripMenuItem.Text = "Step &Over";
		this.debugStepOverToolStripMenuItem.Click += new System.EventHandler(DebugStepOverToolStripMenuItem_Click);
		this.debugStepIntoToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Debug_StepInto;
		this.debugStepIntoToolStripMenuItem.Name = "debugStepIntoToolStripMenuItem";
		this.debugStepIntoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
		this.debugStepIntoToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
		this.debugStepIntoToolStripMenuItem.Text = "Step &Into";
		this.debugStepIntoToolStripMenuItem.Click += new System.EventHandler(DebugStepIntoToolStripMenuItem_Click);
		this.debugStepOutToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Debug_StepOut;
		this.debugStepOutToolStripMenuItem.Name = "debugStepOutToolStripMenuItem";
		this.debugStepOutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11 | System.Windows.Forms.Keys.Shift;
		this.debugStepOutToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
		this.debugStepOutToolStripMenuItem.Text = "Step O&ut";
		this.debugStepOutToolStripMenuItem.Click += new System.EventHandler(DebugStepOutToolStripMenuItem_Click);
		this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.enableComponentToolStripMenuItem, this.disableComponentToolStripMenuItem, this.toolStripSeparator19, this.expandComponentToolStripMenuItem, this.collapseComponentToolStripMenuItem, this.zoomLevelsToolStripMenuItem, this.navigationToolStripMenuItem, this.toolStripSeparator12, this.onlineServicesToolStripMenuItem, this.optionsToolStripMenuItem });
		this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
		this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
		this.toolsToolStripMenuItem.Text = "&Tools";
		this.enableComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_Enable;
		this.enableComponentToolStripMenuItem.Name = "enableComponentToolStripMenuItem";
		this.enableComponentToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.enableComponentToolStripMenuItem.Text = "&Enable";
		this.enableComponentToolStripMenuItem.Click += new System.EventHandler(EnableComponentToolStripMenuItem_Click);
		this.disableComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_Disable;
		this.disableComponentToolStripMenuItem.Name = "disableComponentToolStripMenuItem";
		this.disableComponentToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.disableComponentToolStripMenuItem.Text = "&Disable";
		this.disableComponentToolStripMenuItem.Click += new System.EventHandler(DisableComponentToolStripMenuItem_Click);
		this.toolStripSeparator19.Name = "toolStripSeparator19";
		this.toolStripSeparator19.Size = new System.Drawing.Size(193, 6);
		this.expandComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_Expand;
		this.expandComponentToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.expandComponentToolStripMenuItem.Name = "expandComponentToolStripMenuItem";
		this.expandComponentToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.expandComponentToolStripMenuItem.Text = "E&xpand";
		this.expandComponentToolStripMenuItem.Click += new System.EventHandler(ExpandComponentToolStripMenuItem_Click);
		this.collapseComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_Collapse;
		this.collapseComponentToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.collapseComponentToolStripMenuItem.Name = "collapseComponentToolStripMenuItem";
		this.collapseComponentToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.collapseComponentToolStripMenuItem.Text = "&Collapse";
		this.collapseComponentToolStripMenuItem.Click += new System.EventHandler(CollapseComponentToolStripMenuItem_Click);
		this.zoomLevelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.zoom400ToolStripMenuItem, this.zoom300ToolStripMenuItem, this.zoom200ToolStripMenuItem, this.zoom150ToolStripMenuItem, this.zoom100ToolStripMenuItem, this.zoom75ToolStripMenuItem, this.zoom50ToolStripMenuItem, this.toolStripSeparator13, this.zoomShowAllToolStripMenuItem });
		this.zoomLevelsToolStripMenuItem.Name = "zoomLevelsToolStripMenuItem";
		this.zoomLevelsToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.zoomLevelsToolStripMenuItem.Text = "&Zoom Levels";
		this.zoom400ToolStripMenuItem.Name = "zoom400ToolStripMenuItem";
		this.zoom400ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom400ToolStripMenuItem.Text = "400%";
		this.zoom400ToolStripMenuItem.Click += new System.EventHandler(Zoom400ToolStripMenuItem_Click);
		this.zoom300ToolStripMenuItem.Name = "zoom300ToolStripMenuItem";
		this.zoom300ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom300ToolStripMenuItem.Text = "300%";
		this.zoom300ToolStripMenuItem.Click += new System.EventHandler(Zoom300ToolStripMenuItem_Click);
		this.zoom200ToolStripMenuItem.Name = "zoom200ToolStripMenuItem";
		this.zoom200ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom200ToolStripMenuItem.Text = "200%";
		this.zoom200ToolStripMenuItem.Click += new System.EventHandler(Zoom200ToolStripMenuItem_Click);
		this.zoom150ToolStripMenuItem.Name = "zoom150ToolStripMenuItem";
		this.zoom150ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom150ToolStripMenuItem.Text = "150%";
		this.zoom150ToolStripMenuItem.Click += new System.EventHandler(Zoom150ToolStripMenuItem_Click);
		this.zoom100ToolStripMenuItem.Checked = true;
		this.zoom100ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
		this.zoom100ToolStripMenuItem.Name = "zoom100ToolStripMenuItem";
		this.zoom100ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom100ToolStripMenuItem.Text = "100%";
		this.zoom100ToolStripMenuItem.Click += new System.EventHandler(Zoom100ToolStripMenuItem_Click);
		this.zoom75ToolStripMenuItem.Name = "zoom75ToolStripMenuItem";
		this.zoom75ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom75ToolStripMenuItem.Text = "75%";
		this.zoom75ToolStripMenuItem.Click += new System.EventHandler(Zoom75ToolStripMenuItem_Click);
		this.zoom50ToolStripMenuItem.Name = "zoom50ToolStripMenuItem";
		this.zoom50ToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoom50ToolStripMenuItem.Text = "50%";
		this.zoom50ToolStripMenuItem.Click += new System.EventHandler(Zoom50ToolStripMenuItem_Click);
		this.toolStripSeparator13.Name = "toolStripSeparator13";
		this.toolStripSeparator13.Size = new System.Drawing.Size(139, 6);
		this.zoomShowAllToolStripMenuItem.Name = "zoomShowAllToolStripMenuItem";
		this.zoomShowAllToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
		this.zoomShowAllToolStripMenuItem.Text = "Show All";
		this.zoomShowAllToolStripMenuItem.Click += new System.EventHandler(ZoomShowAllToolStripMenuItem_Click);
		this.navigationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.zoomInToolStripMenuItem, this.zoomOutToolStripMenuItem, this.navigationPanToolStripMenuItem, this.navigationDefaultToolStripMenuItem });
		this.navigationToolStripMenuItem.Name = "navigationToolStripMenuItem";
		this.navigationToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.navigationToolStripMenuItem.Text = "&Navigation Tools";
		this.zoomInToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_NavigationZoomIn;
		this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
		this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
		this.zoomInToolStripMenuItem.Text = "Zoom &In";
		this.zoomInToolStripMenuItem.Click += new System.EventHandler(ZoomInToolStripMenuItem_Click);
		this.zoomOutToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_NavigationZoomOut;
		this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
		this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
		this.zoomOutToolStripMenuItem.Text = "Zoom &Out";
		this.zoomOutToolStripMenuItem.Click += new System.EventHandler(ZoomOutToolStripMenuItem_Click);
		this.navigationPanToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_NavigationPan;
		this.navigationPanToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
		this.navigationPanToolStripMenuItem.Name = "navigationPanToolStripMenuItem";
		this.navigationPanToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
		this.navigationPanToolStripMenuItem.Text = "&Pan";
		this.navigationPanToolStripMenuItem.Click += new System.EventHandler(NavigationPanToolStripMenuItem_Click);
		this.navigationDefaultToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_NavigationDefault;
		this.navigationDefaultToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
		this.navigationDefaultToolStripMenuItem.Name = "navigationDefaultToolStripMenuItem";
		this.navigationDefaultToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
		this.navigationDefaultToolStripMenuItem.Text = "&Default";
		this.navigationDefaultToolStripMenuItem.Click += new System.EventHandler(NavigationDefaultToolStripMenuItem_Click);
		this.toolStripSeparator12.Name = "toolStripSeparator12";
		this.toolStripSeparator12.Size = new System.Drawing.Size(193, 6);
		this.onlineServicesToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_OnlineServices;
		this.onlineServicesToolStripMenuItem.Name = "onlineServicesToolStripMenuItem";
		this.onlineServicesToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.onlineServicesToolStripMenuItem.Text = "O&nline Services...";
		this.onlineServicesToolStripMenuItem.Click += new System.EventHandler(OnlineServicesToolStripMenuItem_Click);
		this.optionsToolStripMenuItem.Image = TCX.CFD.Properties.Resources.Tools_Options;
		this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
		this.optionsToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.optionsToolStripMenuItem.Text = "&Options...";
		this.optionsToolStripMenuItem.Click += new System.EventHandler(OptionsToolStripMenuItem_Click);
		this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.documentationToolStripMenuItem, this.toolStripSeparator23, this.checkForUpdatesToolStripMenuItem, this.toolStripSeparator24, this.aboutToolStripMenuItem });
		this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
		this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
		this.helpToolStripMenuItem.Text = "&Help";
		this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
		this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
		this.checkForUpdatesToolStripMenuItem.Text = "&Check for Updates...";
		this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(CheckForUpdatesToolStripMenuItem_Click);
		this.toolStripSeparator23.Name = "toolStripSeparator23";
		this.toolStripSeparator23.Size = new System.Drawing.Size(288, 6);
		this.toolStripSeparator24.Name = "toolStripSeparator24";
		this.toolStripSeparator24.Size = new System.Drawing.Size(288, 6);
		this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
		this.aboutToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
		this.aboutToolStripMenuItem.Text = "&About 3CX Call Flow Designer...";
		this.aboutToolStripMenuItem.Click += new System.EventHandler(AboutToolStripMenuItem_Click);
		this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
		this.documentationToolStripMenuItem.Size = new System.Drawing.Size(291, 26);
		this.documentationToolStripMenuItem.Text = "&Documentation...";
		this.documentationToolStripMenuItem.Click += new System.EventHandler(DocumentationToolStripMenuItem_Click);
		this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[38]
		{
			this.newToolStripDropDownButton, this.openToolStripDropDownButton, this.toolStripSeparator14, this.saveToolStripButton, this.saveAllToolStripButton, this.toolStripSeparator6, this.undoToolStripButton, this.redoToolStripButton, this.toolStripSeparator16, this.cutToolStripButton,
			this.copyToolStripButton, this.pasteToolStripButton, this.deleteToolStripButton, this.toolStripSeparator7, this.debugBuildToolStripButton, this.releaseBuildToolStripButton, this.buildAllToolStripButton, this.cancelBuildToolStripButton, this.buildProjectExtensionToolStripButton, this.buildToolStripSeparator,
			this.debugStartToolStripButton, this.debugStopToolStripButton, this.debugStepOverToolStripButton, this.debugStepIntoToolStripButton, this.debugStepOutToolStripButton, this.debugToolStripSeparator, this.enableToolStripButton, this.disableToolStripButton, this.expandToolStripButton, this.collapseToolStripButton,
			this.zoomLevelToolStripComboBox, this.zoomInToolStripButton, this.zoomOutToolStripButton, this.navigationPanToolStripButton, this.navigationDefaultToolStripButton, this.toolsToolStripSeparator, this.onlineServicesToolStripButton, this.optionsToolStripButton
		});
		this.toolStrip.Location = new System.Drawing.Point(3, 28);
		this.toolStrip.Name = "toolStrip";
		this.toolStrip.Size = new System.Drawing.Size(835, 28);
		this.toolStrip.TabIndex = 1;
		this.newToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.newToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolbarNewProjectCallflowToolStripMenuItem, this.toolbarNewProjectDialerToolStripMenuItem, this.toolbarNewProjectTemplateToolStripMenuItem, this.toolbarImportProjectFromBuildOutputToolStripMenuItem, this.toolbarNewCallflowToolStripMenuItem, this.toolbarNewDialerToolStripMenuItem, this.toolbarNewComponentToolStripMenuItem, this.toolbarNewFolderToolStripMenuItem });
		this.newToolStripDropDownButton.Image = TCX.CFD.Properties.Resources.File_NewProject;
		this.newToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.newToolStripDropDownButton.Name = "newToolStripDropDownButton";
		this.newToolStripDropDownButton.Size = new System.Drawing.Size(30, 25);
		this.newToolStripDropDownButton.Text = "New";
		this.toolbarNewProjectDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewDialer;
		this.toolbarNewProjectDialerToolStripMenuItem.Name = "toolbarNewProjectDialerToolStripMenuItem";
		this.toolbarNewProjectDialerToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewProjectDialerToolStripMenuItem.Text = "New Dialer Project...";
		this.toolbarNewProjectDialerToolStripMenuItem.Click += new System.EventHandler(ToolbarNewProjectDialerToolStripMenuItem_Click);
		this.toolbarNewProjectCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewProject;
		this.toolbarNewProjectCallflowToolStripMenuItem.Name = "toolbarNewProjectCallflowToolStripMenuItem";
		this.toolbarNewProjectCallflowToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewProjectCallflowToolStripMenuItem.Text = "New Callflow Project...";
		this.toolbarNewProjectCallflowToolStripMenuItem.Click += new System.EventHandler(ToolbarNewProjectCallflowToolStripMenuItem_Click);
		this.toolbarNewProjectTemplateToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewProjectFromTemplate;
		this.toolbarNewProjectTemplateToolStripMenuItem.Name = "toolbarNewProjectTemplateToolStripMenuItem";
		this.toolbarNewProjectTemplateToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewProjectTemplateToolStripMenuItem.Text = "New Project from Template...";
		this.toolbarNewProjectTemplateToolStripMenuItem.Visible = false;
		this.toolbarNewProjectTemplateToolStripMenuItem.Click += new System.EventHandler(ToolbarNewProjectTemplateToolStripMenuItem_Click);
		this.toolbarImportProjectFromBuildOutputToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_ImportProject;
		this.toolbarImportProjectFromBuildOutputToolStripMenuItem.Name = "toolbarImportProjectFromBuildOutputToolStripMenuItem";
		this.toolbarImportProjectFromBuildOutputToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarImportProjectFromBuildOutputToolStripMenuItem.Text = "Import Project from Build Output...";
		this.toolbarImportProjectFromBuildOutputToolStripMenuItem.Click += new System.EventHandler(ToolbarImportProjectFromBuildOutputToolStripMenuItem_Click);
		this.toolbarNewDialerToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewDialer;
		this.toolbarNewDialerToolStripMenuItem.Name = "toolbarNewDialerToolStripMenuItem";
		this.toolbarNewDialerToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewDialerToolStripMenuItem.Text = "New Dialer";
		this.toolbarNewDialerToolStripMenuItem.Click += new System.EventHandler(ToolbarNewDialerToolStripMenuItem_Click);
		this.toolbarNewCallflowToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewCallflow;
		this.toolbarNewCallflowToolStripMenuItem.Name = "toolbarNewCallflowToolStripMenuItem";
		this.toolbarNewCallflowToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewCallflowToolStripMenuItem.Text = "New Callflow";
		this.toolbarNewCallflowToolStripMenuItem.Click += new System.EventHandler(ToolbarNewCallflowToolStripMenuItem_Click);
		this.toolbarNewComponentToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewUserComponent;
		this.toolbarNewComponentToolStripMenuItem.Name = "toolbarNewComponentToolStripMenuItem";
		this.toolbarNewComponentToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewComponentToolStripMenuItem.Text = "New Component";
		this.toolbarNewComponentToolStripMenuItem.Click += new System.EventHandler(ToolbarNewComponentToolStripMenuItem_Click);
		this.toolbarNewFolderToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_NewFolder;
		this.toolbarNewFolderToolStripMenuItem.Name = "toolbarNewFolderToolStripMenuItem";
		this.toolbarNewFolderToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
		this.toolbarNewFolderToolStripMenuItem.Text = "New Folder";
		this.toolbarNewFolderToolStripMenuItem.Click += new System.EventHandler(ToolbarNewFolderToolStripMenuItem_Click);
		this.openToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.openToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolbarOpenProjectToolStripMenuItem, this.toolbarOpenCallflowFileToolStripMenuItem, this.toolbarOpenDialerFileToolStripMenuItem, this.toolbarOpenComponentFileToolStripMenuItem });
		this.openToolStripDropDownButton.Image = TCX.CFD.Properties.Resources.File_OpenProject;
		this.openToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.openToolStripDropDownButton.Name = "openToolStripDropDownButton";
		this.openToolStripDropDownButton.Size = new System.Drawing.Size(30, 25);
		this.openToolStripDropDownButton.Text = "Open";
		this.toolbarOpenProjectToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenProject;
		this.toolbarOpenProjectToolStripMenuItem.Name = "toolbarOpenProjectToolStripMenuItem";
		this.toolbarOpenProjectToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
		this.toolbarOpenProjectToolStripMenuItem.Text = "Open Project...";
		this.toolbarOpenProjectToolStripMenuItem.Click += new System.EventHandler(ToolbarOpenProjectToolStripMenuItem_Click);
		this.toolbarOpenDialerFileToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenDialer;
		this.toolbarOpenDialerFileToolStripMenuItem.Name = "toolbarOpenDialerFileToolStripMenuItem";
		this.toolbarOpenDialerFileToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
		this.toolbarOpenDialerFileToolStripMenuItem.Text = "Open Dialer";
		this.toolbarOpenDialerFileToolStripMenuItem.Click += new System.EventHandler(ToolbarOpenFileToolStripMenuItem_Click);
		this.toolbarOpenCallflowFileToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenCallflow;
		this.toolbarOpenCallflowFileToolStripMenuItem.Name = "toolbarOpenCallflowFileToolStripMenuItem";
		this.toolbarOpenCallflowFileToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
		this.toolbarOpenCallflowFileToolStripMenuItem.Text = "Open Callflow";
		this.toolbarOpenCallflowFileToolStripMenuItem.Click += new System.EventHandler(ToolbarOpenFileToolStripMenuItem_Click);
		this.toolbarOpenComponentFileToolStripMenuItem.Image = TCX.CFD.Properties.Resources.File_OpenUserComponent;
		this.toolbarOpenComponentFileToolStripMenuItem.Name = "toolbarOpenComponentFileToolStripMenuItem";
		this.toolbarOpenComponentFileToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
		this.toolbarOpenComponentFileToolStripMenuItem.Text = "Open Component";
		this.toolbarOpenComponentFileToolStripMenuItem.Click += new System.EventHandler(ToolbarOpenFileToolStripMenuItem_Click);
		this.toolStripSeparator14.Name = "toolStripSeparator14";
		this.toolStripSeparator14.Size = new System.Drawing.Size(6, 28);
		this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.saveToolStripButton.Image = TCX.CFD.Properties.Resources.File_Save;
		this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.saveToolStripButton.Name = "saveToolStripButton";
		this.saveToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.saveToolStripButton.Text = "&Save";
		this.saveToolStripButton.Click += new System.EventHandler(SaveToolStripButton_Click);
		this.saveAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.saveAllToolStripButton.Image = TCX.CFD.Properties.Resources.File_SaveAll;
		this.saveAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.saveAllToolStripButton.Name = "saveAllToolStripButton";
		this.saveAllToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.saveAllToolStripButton.Text = "Save All";
		this.saveAllToolStripButton.Click += new System.EventHandler(SaveAllToolStripButton_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 28);
		this.undoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.undoToolStripButton.Image = TCX.CFD.Properties.Resources.Edit_Undo;
		this.undoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.undoToolStripButton.Name = "undoToolStripButton";
		this.undoToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.undoToolStripButton.Text = "Undo";
		this.undoToolStripButton.Click += new System.EventHandler(UndoToolStripButton_Click);
		this.redoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.redoToolStripButton.Image = TCX.CFD.Properties.Resources.Edit_Redo;
		this.redoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.redoToolStripButton.Name = "redoToolStripButton";
		this.redoToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.redoToolStripButton.Text = "Redo";
		this.redoToolStripButton.Click += new System.EventHandler(RedoToolStripButton_Click);
		this.toolStripSeparator16.Name = "toolStripSeparator16";
		this.toolStripSeparator16.Size = new System.Drawing.Size(6, 28);
		this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.cutToolStripButton.Image = TCX.CFD.Properties.Resources.Edit_Cut;
		this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.cutToolStripButton.Name = "cutToolStripButton";
		this.cutToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.cutToolStripButton.Text = "C&ut";
		this.cutToolStripButton.Click += new System.EventHandler(CutToolStripButton_Click);
		this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.copyToolStripButton.Image = TCX.CFD.Properties.Resources.Edit_Copy;
		this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.copyToolStripButton.Name = "copyToolStripButton";
		this.copyToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.copyToolStripButton.Text = "&Copy";
		this.copyToolStripButton.Click += new System.EventHandler(CopyToolStripButton_Click);
		this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.pasteToolStripButton.Image = TCX.CFD.Properties.Resources.Edit_Paste;
		this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.pasteToolStripButton.Name = "pasteToolStripButton";
		this.pasteToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.pasteToolStripButton.Text = "&Paste";
		this.pasteToolStripButton.Click += new System.EventHandler(PasteToolStripButton_Click);
		this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.deleteToolStripButton.Image = TCX.CFD.Properties.Resources.Edit_Delete;
		this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.deleteToolStripButton.Name = "deleteToolStripButton";
		this.deleteToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.deleteToolStripButton.Text = "Delete";
		this.deleteToolStripButton.Click += new System.EventHandler(DeleteToolStripButton_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 28);
		this.debugBuildToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.debugBuildToolStripButton.Image = TCX.CFD.Properties.Resources.Build_DebugBuild;
		this.debugBuildToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.debugBuildToolStripButton.Name = "debugBuildToolStripButton";
		this.debugBuildToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.debugBuildToolStripButton.Text = "Debug Build";
		this.debugBuildToolStripButton.Click += new System.EventHandler(DebugBuildToolStripButton_Click);
		this.releaseBuildToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.releaseBuildToolStripButton.Image = TCX.CFD.Properties.Resources.Build_ReleaseBuild;
		this.releaseBuildToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.releaseBuildToolStripButton.Name = "releaseBuildToolStripButton";
		this.releaseBuildToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.releaseBuildToolStripButton.Text = "Release Build";
		this.releaseBuildToolStripButton.Click += new System.EventHandler(ReleaseBuildToolStripButton_Click);
		this.buildAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.buildAllToolStripButton.Image = TCX.CFD.Properties.Resources.Build_BuildAll;
		this.buildAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.buildAllToolStripButton.Name = "buildAllToolStripButton";
		this.buildAllToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.buildAllToolStripButton.Text = "Build All";
		this.buildAllToolStripButton.Click += new System.EventHandler(BuildAllToolStripButton_Click);
		this.cancelBuildToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.cancelBuildToolStripButton.Image = TCX.CFD.Properties.Resources.Build_CancelBuild;
		this.cancelBuildToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.cancelBuildToolStripButton.Name = "cancelBuildToolStripButton";
		this.cancelBuildToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.cancelBuildToolStripButton.Text = "Cancel Build";
		this.cancelBuildToolStripButton.Click += new System.EventHandler(CancelBuildToolStripButton_Click);
		this.buildProjectExtensionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.buildProjectExtensionToolStripButton.Image = TCX.CFD.Properties.Resources.Build_SetExtension;
		this.buildProjectExtensionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.buildProjectExtensionToolStripButton.Name = "buildProjectExtensionToolStripButton";
		this.buildProjectExtensionToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.buildProjectExtensionToolStripButton.Text = "Project Extension";
		this.buildProjectExtensionToolStripButton.Click += new System.EventHandler(BuildProjectExtensionToolStripButton_Click);
		this.buildToolStripSeparator.Name = "buildToolStripSeparator";
		this.buildToolStripSeparator.Size = new System.Drawing.Size(6, 28);
		this.debugStartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.debugStartToolStripButton.Image = TCX.CFD.Properties.Resources.Debug_StartDebugging;
		this.debugStartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.debugStartToolStripButton.Name = "debugStartToolStripButton";
		this.debugStartToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.debugStartToolStripButton.Text = "Start Debugging";
		this.debugStartToolStripButton.Click += new System.EventHandler(DebugStartToolStripButton_Click);
		this.debugStopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.debugStopToolStripButton.Image = TCX.CFD.Properties.Resources.Debug_StopDebugging;
		this.debugStopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.debugStopToolStripButton.Name = "debugStopToolStripButton";
		this.debugStopToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.debugStopToolStripButton.Text = "Stop Debugging";
		this.debugStopToolStripButton.Click += new System.EventHandler(DebugStopToolStripButton_Click);
		this.debugStepOverToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.debugStepOverToolStripButton.Image = TCX.CFD.Properties.Resources.Debug_StepOver;
		this.debugStepOverToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.debugStepOverToolStripButton.Name = "debugStepOverToolStripButton";
		this.debugStepOverToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.debugStepOverToolStripButton.Text = "Step Over";
		this.debugStepOverToolStripButton.Click += new System.EventHandler(DebugStepOverToolStripButton_Click);
		this.debugStepIntoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.debugStepIntoToolStripButton.Image = TCX.CFD.Properties.Resources.Debug_StepInto;
		this.debugStepIntoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.debugStepIntoToolStripButton.Name = "debugStepIntoToolStripButton";
		this.debugStepIntoToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.debugStepIntoToolStripButton.Text = "Step Into";
		this.debugStepIntoToolStripButton.Click += new System.EventHandler(DebugStepIntoToolStripButton_Click);
		this.debugStepOutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.debugStepOutToolStripButton.Image = TCX.CFD.Properties.Resources.Debug_StepOut;
		this.debugStepOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.debugStepOutToolStripButton.Name = "debugStepOutToolStripButton";
		this.debugStepOutToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.debugStepOutToolStripButton.Text = "Step Out";
		this.debugStepOutToolStripButton.Click += new System.EventHandler(DebugStepOutToolStripButton_Click);
		this.debugToolStripSeparator.Name = "debugToolStripSeparator";
		this.debugToolStripSeparator.Size = new System.Drawing.Size(6, 28);
		this.enableToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.enableToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_Enable;
		this.enableToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.enableToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.enableToolStripButton.Name = "enableToolStripButton";
		this.enableToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.enableToolStripButton.Text = "Enable";
		this.enableToolStripButton.Click += new System.EventHandler(EnableToolStripButton_Click);
		this.disableToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.disableToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_Disable;
		this.disableToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.disableToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.disableToolStripButton.Name = "disableToolStripButton";
		this.disableToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.disableToolStripButton.Text = "Disable";
		this.disableToolStripButton.Click += new System.EventHandler(DisableToolStripButton_Click);
		this.expandToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.expandToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_Expand;
		this.expandToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.expandToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.expandToolStripButton.Name = "expandToolStripButton";
		this.expandToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.expandToolStripButton.Text = "Expand";
		this.expandToolStripButton.Click += new System.EventHandler(ExpandToolStripButton_Click);
		this.collapseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.collapseToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_Collapse;
		this.collapseToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.collapseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.collapseToolStripButton.Name = "collapseToolStripButton";
		this.collapseToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.collapseToolStripButton.Text = "Collapse";
		this.collapseToolStripButton.Click += new System.EventHandler(CollapseToolStripButton_Click);
		this.zoomLevelToolStripComboBox.MaxDropDownItems = 10;
		this.zoomLevelToolStripComboBox.Name = "zoomLevelToolStripComboBox";
		this.zoomLevelToolStripComboBox.Size = new System.Drawing.Size(121, 28);
		this.zoomLevelToolStripComboBox.Text = "100%";
		this.zoomLevelToolStripComboBox.ToolTipText = "Zoom Level";
		this.zoomLevelToolStripComboBox.SelectedIndexChanged += new System.EventHandler(ZoomLevelToolStripComboBox_SelectedIndexChanged);
		this.zoomLevelToolStripComboBox.Leave += new System.EventHandler(ZoomLevelToolStripComboBox_Leave);
		this.zoomLevelToolStripComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(ZoomLevelToolStripComboBox_KeyDown);
		this.zoomInToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.zoomInToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_NavigationZoomIn;
		this.zoomInToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.zoomInToolStripButton.Name = "zoomInToolStripButton";
		this.zoomInToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.zoomInToolStripButton.Text = "Zoom In";
		this.zoomInToolStripButton.Click += new System.EventHandler(ZoomInToolStripButton_Click);
		this.zoomOutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.zoomOutToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_NavigationZoomOut;
		this.zoomOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.zoomOutToolStripButton.Name = "zoomOutToolStripButton";
		this.zoomOutToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.zoomOutToolStripButton.Text = "Zoom Out";
		this.zoomOutToolStripButton.Click += new System.EventHandler(ZoomOutToolStripButton_Click);
		this.navigationPanToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.navigationPanToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_NavigationPan;
		this.navigationPanToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
		this.navigationPanToolStripButton.Name = "navigationPanToolStripButton";
		this.navigationPanToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.navigationPanToolStripButton.Text = "Pan Navigation Tool";
		this.navigationPanToolStripButton.Click += new System.EventHandler(NavigationPanToolStripButton_Click);
		this.navigationDefaultToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.navigationDefaultToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_NavigationDefault;
		this.navigationDefaultToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
		this.navigationDefaultToolStripButton.Name = "navigationDefaultToolStripButton";
		this.navigationDefaultToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.navigationDefaultToolStripButton.Text = "Default Navigation Tool";
		this.navigationDefaultToolStripButton.Click += new System.EventHandler(NavigationDefaultToolStripButton_Click);
		this.toolsToolStripSeparator.Name = "toolsToolStripSeparator";
		this.toolsToolStripSeparator.Size = new System.Drawing.Size(6, 28);
		this.onlineServicesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.onlineServicesToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_OnlineServices;
		this.onlineServicesToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
		this.onlineServicesToolStripButton.Name = "onlineServicesToolStripButton";
		this.onlineServicesToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.onlineServicesToolStripButton.Text = "Online Services";
		this.onlineServicesToolStripButton.Click += new System.EventHandler(OnlineServicesToolStripButton_Click);
		this.optionsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.optionsToolStripButton.Image = TCX.CFD.Properties.Resources.Tools_Options;
		this.optionsToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
		this.optionsToolStripButton.Name = "optionsToolStripButton";
		this.optionsToolStripButton.Size = new System.Drawing.Size(23, 25);
		this.optionsToolStripButton.Text = "Options";
		this.optionsToolStripButton.Click += new System.EventHandler(OptionsToolStripButton_Click);
		this.saveProjectAsDialog.DefaultExt = "cfdproj";
		this.saveProjectAsDialog.FileName = "NewProject.cfdproj";
		this.saveProjectAsDialog.Filter = "Project Files (*.cfdproj)|*.cfdproj";
		this.saveProjectAsDialog.RestoreDirectory = true;
		this.saveProjectAsDialog.SupportMultiDottedExtensions = true;
		this.saveProjectAsDialog.Title = "Save Project As";
		this.saveNewProjectDialog.DefaultExt = "cfdproj";
		this.saveNewProjectDialog.FileName = "NewProject.cfdproj";
		this.saveNewProjectDialog.Filter = "Project Files (*.cfdproj)|*.cfdproj";
		this.saveNewProjectDialog.RestoreDirectory = true;
		this.saveNewProjectDialog.SupportMultiDottedExtensions = true;
		this.saveNewProjectDialog.Title = "Create New Project";
		this.openProjectDialog.DefaultExt = "cfdproj";
		this.openProjectDialog.Filter = "Project Files (*.cfdproj)|*.cfdproj|Project Files v14 (*.vadproj)|*.vadproj";
		this.openProjectDialog.RestoreDirectory = true;
		this.openProjectDialog.SupportMultiDottedExtensions = true;
		this.openProjectDialog.Title = "Open Project";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1204, 697);
		base.Controls.Add(this.toolStripContainer1);
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.MainMenuStrip = this.menuStrip;
		base.Margin = new System.Windows.Forms.Padding(4);
		this.MinimumSize = new System.Drawing.Size(1061, 728);
		base.Name = "MainForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "3CX Call Flow Designer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(MainForm_Load);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(MainForm_HelpRequested);
		this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
		this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
		this.toolStripContainer1.ContentPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.PerformLayout();
		this.toolStripContainer1.ResumeLayout(false);
		this.toolStripContainer1.PerformLayout();
		this.statusStrip.ResumeLayout(false);
		this.statusStrip.PerformLayout();
		this.menuStrip.ResumeLayout(false);
		this.menuStrip.PerformLayout();
		this.toolStrip.ResumeLayout(false);
		this.toolStrip.PerformLayout();
		base.ResumeLayout(false);
	}
}
