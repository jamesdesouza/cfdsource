using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Debug;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;
using TCX.Windows.Forms.Controls.Docking;
using TCX.Windows.Forms.Controls.TabbedDocuments;

namespace TCX.CFD.Controls;

public class ProjectManagerControl : DockingManagerControl
{
	public delegate void BuildRequestEventHandler(BuildTypes buildType);

	private StatusStrip statusStrip;

	private readonly DesignSurfaceManager designSurfaceManager;

	private ProjectObject projectObject;

	private StartPageControl startPageControl;

	private BuildTypes lastBuildType = BuildTypes.All;

	private readonly List<ComponentExecutionDebugInfo> componentExecutionDebugInfoList;

	private readonly int componentExecutionDebugInfoIndex;

	private readonly IVadActivity debuggingActivity;

	private readonly FlowTypes debuggingFlowType;

	private readonly ToolBox.ToolboxNode callRelatedComponentsNode;

	private readonly ToolBox.ToolboxNode controlStructuresComponentsNode;

	private readonly ToolBox.ToolboxNode tcxComponentsNode;

	private readonly ToolBox.ToolboxNode advancedComponentsNode;

	private readonly ToolBox.ToolboxNode userDefinedComponentsNode;

	private IContainer components;

	private TabbedDocumentControl tabbedDocumentControl;

	private ProjectExplorerControl projectExplorerControl;

	private PropertyGrid propertyGrid;

	private RichTextBoxEx outputTextBox;

	private ErrorListControl errorListControl;

	private ToolBox componentsToolBox;

	private BackgroundWorker buildBackgroundWorker;

	public ProjectExplorerControl ProjectExplorer => projectExplorerControl;

	public FlowDesignerControl FlowDesigner
	{
		get
		{
			if (tabbedDocumentControl.SelectedControl != startPageControl)
			{
				return tabbedDocumentControl.SelectedControl as FlowDesignerControl;
			}
			return null;
		}
	}

	public bool IsFlowDesignerControlActive => base.ActiveControl == tabbedDocumentControl;

	public bool IsEditing
	{
		get
		{
			if (tabbedDocumentControl.SelectedControl == startPageControl)
			{
				return false;
			}
			return tabbedDocumentControl.SelectedControl is FlowDesignerControl;
		}
	}

	public bool IsCurrentFileModified => GetEditingFileObject()?.HasChanges ?? false;

	public bool IsBuilding { get; private set; }

	public bool IsDebugging => componentExecutionDebugInfoList != null;

	public bool CanStepOver
	{
		get
		{
			Activity activity = debuggingActivity as Activity;
			if (activity.Parent == null)
			{
				return false;
			}
			if (activity.Parent.EnabledActivities.IndexOf(activity) < activity.Parent.EnabledActivities.Count - 1)
			{
				return !(activity.Parent is AbsVadCompositeActivity);
			}
			return false;
		}
	}

	public bool CanStepInto
	{
		get
		{
			if (!(debuggingActivity is AbsVadCompositeActivity) && !(debuggingActivity is AbsVadSequenceActivity))
			{
				return debuggingActivity is UserComponent;
			}
			return true;
		}
	}

	public bool CanStepOut => componentExecutionDebugInfoIndex < componentExecutionDebugInfoList.Count - 1;

	public event EventHandler AvailableMenuCommandsChange;

	private void EnableDisableControls()
	{
		bool flag = false;
		if (tabbedDocumentControl.Items.Count == 0 || tabbedDocumentControl.SelectedControl == null || tabbedDocumentControl.SelectedControl == startPageControl)
		{
			foreach (ToolBox.ToolboxNode node in callRelatedComponentsNode.Nodes)
			{
				if (node.SetEnabled(enabled: false))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node2 in controlStructuresComponentsNode.Nodes)
			{
				if (node2.SetEnabled(enabled: false))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node3 in tcxComponentsNode.Nodes)
			{
				if (node3.SetEnabled(enabled: false))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node4 in advancedComponentsNode.Nodes)
			{
				if (node4.SetEnabled(enabled: false))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node5 in userDefinedComponentsNode.Nodes)
			{
				if (node5.SetEnabled(enabled: false))
				{
					flag = true;
				}
			}
		}
		else
		{
			FlowDesignerControl flowDesignerControl = tabbedDocumentControl.SelectedControl as FlowDesignerControl;
			foreach (ToolBox.ToolboxNode node6 in callRelatedComponentsNode.Nodes)
			{
				if (node6.SetEnabled((flowDesignerControl.NeedsDisconnectHandlerFlow && !flowDesignerControl.IsDisconnectHandlerActive) || node6.ComponentType == typeof(MakeCallComponent)))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node7 in controlStructuresComponentsNode.Nodes)
			{
				if (node7.SetEnabled(enabled: true))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node8 in tcxComponentsNode.Nodes)
			{
				if (node8.SetEnabled(enabled: true))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node9 in advancedComponentsNode.Nodes)
			{
				if (node9.SetEnabled(enabled: true))
				{
					flag = true;
				}
			}
			foreach (ToolBox.ToolboxNode node10 in userDefinedComponentsNode.Nodes)
			{
				bool flag2;
				if (flowDesignerControl.FileObject is ComponentFileObject componentFileObject)
				{
					ComponentFileObject componentFileObject2 = node10.Tag as ComponentFileObject;
					List<ComponentFileObject> componentFileObjects = componentFileObject2.GetComponentFileObjects();
					flag2 = componentFileObject2 != componentFileObject && !componentFileObjects.Contains(componentFileObject);
					if (flag2 && (!flowDesignerControl.NeedsDisconnectHandlerFlow || flowDesignerControl.IsDisconnectHandlerActive))
					{
						RootFlow rootFlow = componentFileObject2.FlowLoader.GetRootFlow(FlowTypes.MainFlow);
						if (!ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(projectObject, rootFlow.EnabledActivities))
						{
							flag2 = false;
						}
						else
						{
							RootFlow rootFlow2 = componentFileObject2.FlowLoader.GetRootFlow(FlowTypes.ErrorHandler);
							if (!ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(projectObject, rootFlow2.EnabledActivities))
							{
								flag2 = false;
							}
						}
					}
				}
				else
				{
					flag2 = true;
					if (!flowDesignerControl.NeedsDisconnectHandlerFlow || flowDesignerControl.IsDisconnectHandlerActive)
					{
						ComponentFileObject componentFileObject3 = node10.Tag as ComponentFileObject;
						RootFlow rootFlow3 = componentFileObject3.FlowLoader.GetRootFlow(FlowTypes.MainFlow);
						if (!ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(projectObject, rootFlow3.EnabledActivities))
						{
							flag2 = false;
						}
						else
						{
							RootFlow rootFlow4 = componentFileObject3.FlowLoader.GetRootFlow(FlowTypes.ErrorHandler);
							if (!ComponentDesignerHelper.CanInsertActivitiesWithoutActiveCall(projectObject, rootFlow4.EnabledActivities))
							{
								flag2 = false;
							}
						}
					}
				}
				if (node10.SetEnabled(flag2))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			componentsToolBox.Refresh();
		}
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	private void ChildControl_Enter(object sender, EventArgs e)
	{
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	public ProjectManagerControl()
	{
		InitializeComponent();
		SuspendLayout();
		designSurfaceManager = new DesignSurfaceManager();
		designSurfaceManager.ActiveDesignSurfaceChanged += DesignSurfaceManager_ActiveDesignSurfaceChanged;
		IServiceContainer serviceContainer = designSurfaceManager.GetService(typeof(ServiceContainer)) as IServiceContainer;
		serviceContainer.AddService(typeof(INameCreationService), new FlowDesignerNameCreationService());
		serviceContainer.AddService(typeof(IDesignerSerializationService), new FlowDesignerSerializationService(serviceContainer));
		serviceContainer.AddService(typeof(IToolboxService), componentsToolBox);
		serviceContainer.AddService(typeof(ITypeProvider), new TypeProvider(serviceContainer), promote: true);
		DateTime ınstallDateV18Update = Settings.Default.InstallDateV18Update2;
		bool isNew = ınstallDateV18Update == DateTime.MinValue || ınstallDateV18Update.AddDays(15.0) > DateTime.Now;
		if (ınstallDateV18Update == DateTime.MinValue)
		{
			Settings.Default.InstallDateV18Update2 = DateTime.Now;
			Settings.Default.Save();
		}
		IDockingPanel dockingPanel = base.Panels[DockingType.Right].InsertPanel(0);
		dockingPanel.DockedControls.Add(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.ProjectExplorer.Title"), projectExplorerControl);
		dockingPanel.DockedControls.Add(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Properties.Title"), propertyGrid);
		callRelatedComponentsNode = new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.CallRelatedComponents.Title"));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.MenuItem.Title"), Resources.Menu, typeof(MenuComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.UserInputItem.Title"), Resources.UserInput, typeof(UserInputComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.VoiceInputItem.Title"), Resources.VoiceInput, typeof(VoiceInputComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.AuthenticationItem.Title"), Resources.Authentication, typeof(AuthenticationComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.CreditCardItem.Title"), Resources.CreditCard, typeof(CreditCardComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.SurveyItem.Title"), Resources.Survey, typeof(SurveyComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.PromptPlaybackItem.Title"), Resources.PromptPlayback, typeof(PromptPlaybackComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.RecordItem.Title"), Resources.Record, typeof(RecordComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.RecordAndEmailItem.Title"), Resources.RecordEmail, typeof(RecordAndEmailComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TransferItem.Title"), Resources.Transfer, typeof(TransferComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.AttachCallDataItem.Title"), Resources.AttachCallData, typeof(AttachCallDataComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.GetAttachedCallDataItem.Title"), Resources.GetAttachedCallData, typeof(GetAttachedCallDataComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.DisconnectCallItem.Title"), Resources.DisconnectCall, typeof(DisconnectCallComponent), "", "", isNew: false));
		callRelatedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.MakeCallItem.Title"), Resources.MakeCall, typeof(MakeCallComponent), "", "", isNew: false));
		controlStructuresComponentsNode = new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.ControlStructuresComponents.Title"));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.VariableAssignmentItem.Title"), Resources.VariableAssignment, typeof(VariableAssignmentComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.IncrementVariableItem.Title"), Resources.IncrementVariable, typeof(IncrementVariableComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.DecrementVariableItem.Title"), Resources.DecrementVariable, typeof(DecrementVariableComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.ConditionalItem.Title"), Resources.Conditional, typeof(ConditionalComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.DateTimeConditionalItem.Title"), Resources.DateTimeConditional, typeof(DateTimeConditionalComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.ParallelExecutionItem.Title"), Resources.ParallelExecution, typeof(ParallelExecutionComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.LoopItem.Title"), Resources.Loop, typeof(LoopComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.LoggerItem.Title"), Resources.Logger, typeof(LoggerComponent), "", "", isNew: false));
		controlStructuresComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.ExitCallflowItem.Title"), Resources.ExitCallflow, typeof(ExitCallflowComponent), "", "", isNew: false));
		tcxComponentsNode = new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxComponents.Title"));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxGetDnPropertyItem.Title"), Resources.Read3CX, typeof(TcxGetDnPropertyComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxSetDnPropertyItem.Title"), Resources.Write3CX, typeof(TcxSetDnPropertyComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxGetGlobalPropertyItem.Title"), Resources.Read3CX, typeof(TcxGetGlobalPropertyComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxSetGlobalPropertyItem.Title"), Resources.Write3CX, typeof(TcxSetGlobalPropertyComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxGetExtensionStatusItem.Title"), Resources.Read3CX, typeof(TcxGetExtensionStatusComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxSetExtensionStatusItem.Title"), Resources.Write3CX, typeof(TcxSetExtensionStatusComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxGetQueueExtensionsItem.Title"), Resources.Read3CX, typeof(TcxGetQueueExtensionsComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxGetQueueStatusItem.Title"), Resources.Read3CX, typeof(TcxGetQueueStatusComponent), "", "", isNew: false));
		tcxComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TcxSetQueueExtensionStatusItem.Title"), Resources.Write3CX, typeof(TcxSetQueueExtensionStatusComponent), "", "", isNew: false));
		advancedComponentsNode = new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.AdvancedComponents.Title"));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.CryptographyItem.Title"), Resources.Cryptography, typeof(CryptographyComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.CRMLookupItem.Title"), Resources.CRMLookup, typeof(CRMLookupComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.DatabaseAccessItem.Title"), Resources.DatabaseAccess, typeof(DatabaseAccessComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.EMailSenderItem.Title"), Resources.EmailSender, typeof(EMailSenderComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.ExternalCodeExecutionItem.Title"), Resources.ExternalCodeExecution, typeof(ExternalCodeExecutionComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.ExecuteCSharpCodeItem.Title"), Resources.ExternalCodeExecution, typeof(ExecuteCSharpCodeComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.FileManagementItem.Title"), Resources.FileManagement, typeof(FileManagementComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.SocketClientItem.Title"), Resources.SocketClient, typeof(SocketClientComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.WebInteractionItem.Title"), Resources.WebInteraction, typeof(WebInteractionComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.WebServiceRestItem.Title"), Resources.WebServicesInteraction, typeof(WebServiceRestComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.JsonXmlParserItem.Title"), Resources.JsonXmlParser, typeof(JsonXmlParserComponent), "", "", isNew: false));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.CsvParserItem.Title"), Resources.CsvParser, typeof(CsvParserComponent), "", "", isNew));
		advancedComponentsNode.Nodes.Add(new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.TranscribeAudioItem.Title"), Resources.TranscribeAudio, typeof(TranscribeAudioComponent), "", "", isNew));
		userDefinedComponentsNode = new ToolBox.ToolboxNode(LocalizedResourceMgr.GetString("ProjectManagerControl.Toolbar.UserDefinedComponents.Title"));
		componentsToolBox.Nodes.Add(callRelatedComponentsNode);
		componentsToolBox.Nodes.Add(controlStructuresComponentsNode);
		componentsToolBox.Nodes.Add(advancedComponentsNode);
		componentsToolBox.Nodes.Add(tcxComponentsNode);
		componentsToolBox.Nodes.Add(userDefinedComponentsNode);
		componentsToolBox.ExpandAll();
		tcxComponentsNode.Collapse();
		base.Panels[DockingType.Left].InsertPanel(0).DockedControls.Add(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Components.Title"), componentsToolBox);
		IDockingPanel dockingPanel2 = base.Panels[DockingType.Bottom].InsertPanel(0);
		IDockingControl dockingControl = dockingPanel2.DockedControls.Add(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.ErrorList.Title"), errorListControl);
		IDockingControl dockingControl2 = dockingPanel2.DockedControls.Add(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Output.Title"), outputTextBox);
		dockingControl.AutoHide = true;
		dockingControl2.AutoHide = true;
		dockingPanel2.Tabbed = true;
		propertyGrid.HelpRequested += PropertyGrid_HelpRequested;
		componentsToolBox.HelpRequested += ComponentsToolBox_HelpRequested;
		ResumeLayout(performLayout: true);
		EnableDisableControls();
	}

	private void PropertyGrid_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/#h.mfle8nipvacm");
	}

	private void ComponentsToolBox_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/#h.n7zfsvs038nf");
	}

	public void SetStatusStrip(StatusStrip statusStrip)
	{
		this.statusStrip = statusStrip;
		projectExplorerControl.SetParentControls(statusStrip, propertyGrid);
	}

	public void RegisterProjectObject(ProjectObject projectObject)
	{
		this.projectObject = projectObject;
		projectObject.Changed += ProjectObject_Changed;
		projectObject.Loaded += ProjectObject_Loaded;
		projectObject.Closing += ProjectObject_Closing;
		projectObject.Closed += ProjectObject_Closed;
	}

	private void DesignSurfaceManager_ActiveDesignSurfaceChanged(object sender, ActiveDesignSurfaceChangedEventArgs e)
	{
		if (!(e.NewSurface.GetService(typeof(ISelectionService)) is ISelectionService selectionService))
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		foreach (object selectedComponent in selectionService.GetSelectedComponents())
		{
			if (selectedComponent is RootFlow)
			{
				arrayList.Add((selectedComponent as RootFlow).FileObject);
			}
			else if (selectedComponent is ComponentBranch)
			{
				arrayList.Add((selectedComponent as ComponentBranch).Parent);
			}
			else
			{
				arrayList.Add(selectedComponent);
			}
		}
		propertyGrid.SelectedObjects = arrayList.ToArray();
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	public void OnlineServicesUpdated()
	{
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem != startPageControl && ıtem is FlowDesignerControl flowDesignerControl)
			{
				RefreshActivitiesForWorkflowView(flowDesignerControl.GetWorkflowViewFor(FlowTypes.MainFlow), flowDesignerControl.GetDesignerHostFor(FlowTypes.MainFlow));
				RefreshActivitiesForWorkflowView(flowDesignerControl.GetWorkflowViewFor(FlowTypes.ErrorHandler), flowDesignerControl.GetDesignerHostFor(FlowTypes.ErrorHandler));
				if (flowDesignerControl.NeedsDisconnectHandlerFlow)
				{
					RefreshActivitiesForWorkflowView(flowDesignerControl.GetWorkflowViewFor(FlowTypes.DisconnectHandler), flowDesignerControl.GetDesignerHostFor(FlowTypes.DisconnectHandler));
				}
			}
		}
	}

	private void RefreshActivitiesForWorkflowView(WorkflowView view, IDesignerHost designerHost)
	{
		if (view == null || designerHost == null || !(view.RootDesigner?.Activity is RootFlow rootFlow))
		{
			return;
		}
		using DesignerTransaction designerTransaction = designerHost.CreateTransaction("Refresh components when updating online services");
		foreach (Activity activity in rootFlow.Activities)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(activity);
			if (activity.Enabled)
			{
				properties["Enabled"].SetValue(activity, false);
				properties["Enabled"].SetValue(activity, true);
			}
			else
			{
				properties["Enabled"].SetValue(activity, true);
				properties["Enabled"].SetValue(activity, false);
			}
		}
		designerTransaction.Commit();
	}

	private void ProjectObject_Changed(object sender, EventArgs e)
	{
		if (!IsBuilding && this.AvailableMenuCommandsChange != null)
		{
			this.AvailableMenuCommandsChange(this, EventArgs.Empty);
		}
		if (IsDebugging)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("ProjectManagerControl.MessageBox.Error.ChangeWhileDebugging"), LocalizedResourceMgr.GetString("ProjectManagerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			StopDebugging();
		}
	}

	private void ProjectObject_Loaded(object sender, EventArgs e)
	{
	}

	private void ProjectObject_Closing(object sender, CancelEventArgs e)
	{
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem == startPageControl)
			{
				continue;
			}
			FlowDesignerControl flowDesignerControl = ıtem as FlowDesignerControl;
			if (flowDesignerControl.FileObject.HasChanges)
			{
				switch (MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectManagerControl.MessageBox.Question.SaveChanges"), flowDesignerControl.FileObject.Path), LocalizedResourceMgr.GetString("ProjectManagerControl.MessageBox.Title"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
				{
				case DialogResult.Yes:
					flowDesignerControl.FileObject.Save();
					break;
				case DialogResult.Cancel:
					e.Cancel = true;
					return;
				}
			}
		}
	}

	private void ProjectObject_Closed(object sender, EventArgs e)
	{
		SetProjectClosed();
	}

	public void SetProjectClosed()
	{
		if (IsDebugging)
		{
			StopDebugging();
		}
		userDefinedComponentsNode.Nodes.Clear();
		outputTextBox.Clear();
		errorListControl.ClearErrorList();
		for (int num = tabbedDocumentControl.Items.Count - 1; num >= 0; num--)
		{
			if (tabbedDocumentControl.Items[num] != startPageControl)
			{
				tabbedDocumentControl.Items.RemoveAt(num);
			}
		}
		EnableDisableControls();
	}

	public void RegisterCallflowFileObject(CallflowFileObject callflowFileObject)
	{
		RegisterFileObject(callflowFileObject);
	}

	public void RegisterDialerFileObject(DialerFileObject dialerFileObject)
	{
		RegisterFileObject(dialerFileObject);
	}

	public void RegisterComponentFileObject(ComponentFileObject componentFileObject)
	{
		ToolBox.ToolboxNode toolboxNode = new ToolBox.ToolboxNode(componentFileObject.GetNameWithoutExtension(), Resources.UserComponent, typeof(UserComponent), "", "", isNew: false)
		{
			Tag = componentFileObject
		};
		bool flag = false;
		for (int i = 0; i < userDefinedComponentsNode.Nodes.Count; i++)
		{
			if (toolboxNode.Text.CompareTo(userDefinedComponentsNode.Nodes[i].Text) < 0)
			{
				userDefinedComponentsNode.Nodes.Insert(i, toolboxNode);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			userDefinedComponentsNode.Nodes.Add(toolboxNode);
		}
		if (userDefinedComponentsNode.Nodes.Count == 1)
		{
			userDefinedComponentsNode.Expand();
		}
		RegisterFileObject(componentFileObject);
		EnableDisableControls();
	}

	private void RegisterFileObject(FileObject fileObject)
	{
		fileObject.Changed += FileObject_Changed;
		fileObject.NameChanged += FileObject_NameChanged;
		fileObject.Loading += FileObject_Loading;
		fileObject.Selected += FileObject_Selected;
		fileObject.Closing += FileObject_Closing;
		fileObject.Closed += FileObject_Closed;
		fileObject.Saved += FileObject_Saved;
		fileObject.Deleted += FileObject_Deleted;
	}

	private FlowDesignerControl GetFlowDesignerControl(FileObject fileObject)
	{
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem != startPageControl)
			{
				FlowDesignerControl flowDesignerControl = ıtem as FlowDesignerControl;
				if (flowDesignerControl.FileObject == fileObject)
				{
					return flowDesignerControl;
				}
			}
		}
		return null;
	}

	private void FileObject_Changed(object sender, EventArgs e)
	{
		FlowDesignerControl flowDesignerControl = GetFlowDesignerControl(sender as FileObject);
		if (flowDesignerControl != null)
		{
			tabbedDocumentControl.ItemTitles[flowDesignerControl] = flowDesignerControl.FileObject.GetNameWithoutExtension() + "*";
			EnableDisableControls();
		}
	}

	private ToolBox.ToolboxNode GetToolboxNode(FileObject fileObject)
	{
		foreach (ToolBox.ToolboxNode node in userDefinedComponentsNode.Nodes)
		{
			if (node.Tag == fileObject)
			{
				return node;
			}
		}
		return null;
	}

	private void FileObject_NameChanged(object sender, EventArgs e)
	{
		FileObject fileObject = sender as FileObject;
		ToolBox.ToolboxNode toolboxNode = GetToolboxNode(fileObject);
		if (toolboxNode == null)
		{
			return;
		}
		toolboxNode.Text = fileObject.GetNameWithoutExtension();
		toolboxNode.ToolTipCaption = fileObject.GetNameWithoutExtension();
		toolboxNode.ToolTipText = fileObject.Path;
		userDefinedComponentsNode.Nodes.Remove(toolboxNode);
		bool flag = false;
		for (int i = 0; i < userDefinedComponentsNode.Nodes.Count; i++)
		{
			if (toolboxNode.Text.CompareTo(userDefinedComponentsNode.Nodes[i].Text) < 0)
			{
				userDefinedComponentsNode.Nodes.Insert(i, toolboxNode);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			userDefinedComponentsNode.Nodes.Add(toolboxNode);
		}
		componentsToolBox.Refresh();
	}

	private void FileObject_Loading(object sender, EventArgs e)
	{
		FlowDesignerControl flowDesignerControl = new FlowDesignerControl(sender as FileObject, designSurfaceManager, propertyGrid);
		flowDesignerControl.AvailableMenuCommandsChange += ChildControl_AvailableMenuCommandsChange;
		flowDesignerControl.ShowPropertiesWindow += ChildControl_ShowPropertiesWindow;
		tabbedDocumentControl.Items.Add(flowDesignerControl);
		tabbedDocumentControl.SelectedControl = flowDesignerControl;
	}

	private void FileObject_Selected(object sender, EventArgs e)
	{
		tabbedDocumentControl.SelectedControl = GetFlowDesignerControl(sender as FileObject);
	}

	private void FileObject_Closing(object sender, CancelEventArgs e)
	{
		FileObject fileObject = sender as FileObject;
		if (fileObject.HasChanges)
		{
			switch (MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ProjectManagerControl.MessageBox.Question.SaveChanges"), fileObject.Path), LocalizedResourceMgr.GetString("ProjectManagerControl.MessageBox.Title"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
			{
			case DialogResult.Yes:
				fileObject.Save();
				break;
			case DialogResult.Cancel:
				e.Cancel = true;
				break;
			}
		}
	}

	private void FileObject_Closed(object sender, EventArgs e)
	{
		FlowDesignerControl flowDesignerControl = GetFlowDesignerControl(sender as FileObject);
		if (flowDesignerControl != null)
		{
			tabbedDocumentControl.Items.Remove(flowDesignerControl);
			EnableDisableControls();
		}
	}

	private void FileObject_Saved(object sender, EventArgs e)
	{
		FlowDesignerControl flowDesignerControl = GetFlowDesignerControl(sender as FileObject);
		if (flowDesignerControl != null)
		{
			tabbedDocumentControl.ItemTitles[flowDesignerControl] = flowDesignerControl.FileObject.GetNameWithoutExtension();
			EnableDisableControls();
		}
	}

	private void FileObject_Deleted(object sender, EventArgs e)
	{
		ToolBox.ToolboxNode toolboxNode = GetToolboxNode(sender as FileObject);
		if (toolboxNode != null)
		{
			toolboxNode.Remove();
			componentsToolBox.Refresh();
			if (tabbedDocumentControl.SelectedControl != startPageControl)
			{
				(tabbedDocumentControl.SelectedControl as FlowDesignerControl).Refresh();
			}
		}
	}

	private void TabbedDocumentControl_DocumentRemoving(TabbedDocumentControl documentControl, Control control, CancelEventArgs e)
	{
		if (control != startPageControl)
		{
			FlowDesignerControl flowDesignerControl = control as FlowDesignerControl;
			e.Cancel = !flowDesignerControl.FileObject.Close();
		}
		EnableDisableControls();
	}

	private void TabbedDocumentControl_DocumentAdded(TabbedDocumentControl documentControl, Control control)
	{
		EnableDisableControls();
	}

	private void TabbedDocumentControl_SelectedControlChanged(object sender, EventArgs e)
	{
		EnableDisableControls();
	}

	public void CloseFile()
	{
		GetEditingFileObject()?.Close();
	}

	public void SaveFile()
	{
		GetEditingFileObject()?.Save();
	}

	public void SaveAll()
	{
		projectObject.Save();
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem != startPageControl)
			{
				(ıtem as FlowDesignerControl).FileObject.Save();
			}
		}
	}

	public FileObject GetEditingFileObject()
	{
		if (tabbedDocumentControl.SelectedControl == startPageControl)
		{
			return null;
		}
		if (!(tabbedDocumentControl.SelectedControl is FlowDesignerControl flowDesignerControl))
		{
			return null;
		}
		return flowDesignerControl.FileObject;
	}

	public string GetEditingFileName()
	{
		FileObject editingFileObject = GetEditingFileObject();
		if (editingFileObject == null)
		{
			return string.Empty;
		}
		return editingFileObject.Name;
	}

	private void ShowWindow(string title, bool pin)
	{
		foreach (IDockingControl dockingControl in base.DockingControls)
		{
			if (!(dockingControl.Title == title))
			{
				continue;
			}
			dockingControl.Cancelled = false;
			if (dockingControl.AutoHide)
			{
				dockingControl.ShowControl();
				if (pin)
				{
					dockingControl.AutoHide = false;
				}
			}
			((UserControl)dockingControl).Focus();
			break;
		}
	}

	public void ShowComponentsToolbar()
	{
		ShowWindow(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Components.Title"), pin: false);
	}

	public void ShowProjectExplorerWindow()
	{
		ShowWindow(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.ProjectExplorer.Title"), pin: false);
	}

	public void ShowPropertiesWindow()
	{
		ShowWindow(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Properties.Title"), pin: false);
	}

	public void ShowOutputWindow()
	{
		ShowWindow(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Output.Title"), pin: false);
	}

	public void ShowErrorListWindow(bool pin)
	{
		ShowWindow(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.ErrorList.Title"), pin);
	}

	public void ShowDebugWindow()
	{
		ShowWindow(LocalizedResourceMgr.GetString("ProjectManagerControl.Windows.Debug.Title"), pin: false);
	}

	public void ShowStartPage(Func<StartPageControl> createStartPageControlHandler)
	{
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem == startPageControl)
			{
				tabbedDocumentControl.SelectedControl = ıtem;
				return;
			}
		}
		startPageControl = createStartPageControlHandler();
		tabbedDocumentControl.Items.Add(startPageControl);
		tabbedDocumentControl.SelectedControl = startPageControl;
	}

	private void ChildControl_AvailableMenuCommandsChange(object sender, EventArgs e)
	{
		EnableDisableControls();
	}

	private void ChildControl_ShowPropertiesWindow(object sender, EventArgs e)
	{
		ShowPropertiesWindow();
	}

	private void BuildRequest(BuildTypes buildType)
	{
		BuildProject(buildType);
	}

	private void SetReadOnly()
	{
		projectExplorerControl.Enabled = false;
		propertyGrid.Enabled = false;
		componentsToolBox.Enabled = false;
		startPageControl.Enabled = false;
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem != startPageControl)
			{
				(ıtem as FlowDesignerControl).SetReadOnlyDesigners();
			}
		}
	}

	private void SetReadWrite()
	{
		projectExplorerControl.Enabled = true;
		propertyGrid.Enabled = true;
		componentsToolBox.Enabled = true;
		startPageControl.Enabled = true;
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem != startPageControl)
			{
				(ıtem as FlowDesignerControl).SetReadWriteDesigners();
			}
		}
	}

	public void BuildProject(BuildTypes buildType)
	{
		SaveAll();
		SetReadOnly();
		errorListControl.ClearErrorList();
		outputTextBox.Clear();
		ShowOutputWindow();
		lastBuildType = buildType;
		IsBuilding = true;
		buildBackgroundWorker.RunWorkerAsync(buildType);
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	private void BuildBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		ProjectCompiler projectCompiler = new ProjectCompiler(new UICompilerResultCollector(buildBackgroundWorker, this), projectObject);
		BuildTypes buildTypes = (BuildTypes)e.Argument;
		if (buildTypes == BuildTypes.All)
		{
			e.Cancel = projectCompiler.Compile(isDebugBuild: false) == CompilationResult.Cancelled;
			if (!e.Cancel)
			{
				e.Cancel = projectCompiler.Compile(isDebugBuild: true) == CompilationResult.Cancelled;
			}
		}
		else
		{
			e.Cancel = projectCompiler.Compile(buildTypes == BuildTypes.Debug) == CompilationResult.Cancelled;
		}
	}

	private void BuildBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		ShowOutputWindow();
		CompilerEvent compilerEvent = e.UserState as CompilerEvent;
		switch (compilerEvent.Type)
		{
		case CompilerEventTypes.OutputWindowText:
			outputTextBox.SelectedText = compilerEvent.Message;
			if (compilerEvent.AddNewLineEnding)
			{
				outputTextBox.SelectedText = Environment.NewLine;
			}
			break;
		case CompilerEventTypes.OutputWindowLink:
			outputTextBox.InsertLink(compilerEvent.Message + Environment.NewLine, compilerEvent.Link);
			if (compilerEvent.AddNewLineEnding)
			{
				outputTextBox.SelectedText = Environment.NewLine;
			}
			break;
		case CompilerEventTypes.ErrorDescriptor:
			outputTextBox.SelectedText = compilerEvent.ErrorDescriptor.Description;
			if (compilerEvent.AddNewLineEnding)
			{
				outputTextBox.SelectedText = Environment.NewLine;
			}
			errorListControl.AddErrorDescriptor(compilerEvent.ErrorDescriptor);
			break;
		}
		(statusStrip.Items[1] as ToolStripProgressBar).Value = e.ProgressPercentage;
	}

	private void BuildBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		bool flag = false;
		if (e.Cancelled)
		{
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectManagerControl.Statusbar.BuildCancelled");
		}
		else if (e.Error == null && errorListControl.ErrorCount == 0)
		{
			flag = true;
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectManagerControl.Statusbar.BuildSucceeded");
		}
		else
		{
			if (e.Error != null)
			{
				errorListControl.AddErrorDescriptor(new ErrorDescriptor(CompilerMessageTypes.Error, e.Error.Message));
			}
			statusStrip.Items[0].Text = LocalizedResourceMgr.GetString("ProjectManagerControl.Statusbar.BuildFailed");
		}
		if (lastBuildType == BuildTypes.Debug || lastBuildType == BuildTypes.All)
		{
			projectObject.DebugBuildSuccessful = flag;
		}
		if (lastBuildType == BuildTypes.Release || lastBuildType == BuildTypes.All)
		{
			projectObject.ReleaseBuildSuccessful = flag;
		}
		SetReadWrite();
		if (errorListControl.TotalErrorCount != 0)
		{
			ShowErrorListWindow(pin: true);
		}
		IsBuilding = false;
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	public void CancelBuild()
	{
		buildBackgroundWorker.CancelAsync();
	}

	private void OutputTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
	{
		string[] array = e.LinkText.Split('#');
		if (array.Length == 2)
		{
			Process.Start("explorer.exe", "/select,\"" + array[1] + "\"");
		}
	}

	private void EnsureDebuggingActivityVisible(FileObject fileObject)
	{
		fileObject.Open();
		foreach (Control ıtem in tabbedDocumentControl.Items)
		{
			if (ıtem != startPageControl)
			{
				FlowDesignerControl flowDesignerControl = ıtem as FlowDesignerControl;
				if (flowDesignerControl.FileObject == fileObject)
				{
					flowDesignerControl.SetDebuggingActivity(debuggingActivity as Activity, debuggingFlowType);
					break;
				}
			}
		}
	}

	public void StartDebugging(string debugInfoFilePath)
	{
	}

	public void StopDebugging()
	{
	}

	public void StepOver()
	{
	}

	public void StepInto()
	{
	}

	public void StepOut()
	{
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
		this.tabbedDocumentControl = new TCX.Windows.Forms.Controls.TabbedDocuments.TabbedDocumentControl();
		this.propertyGrid = new System.Windows.Forms.PropertyGrid();
		this.buildBackgroundWorker = new System.ComponentModel.BackgroundWorker();
		this.componentsToolBox = new TCX.CFD.Controls.ToolBox();
		this.errorListControl = new TCX.CFD.Controls.ErrorListControl();
		this.outputTextBox = new TCX.CFD.Classes.Components.RichTextBoxEx();
		this.projectExplorerControl = new TCX.CFD.Controls.ProjectExplorerControl();
		base.SuspendLayout();
		this.tabbedDocumentControl.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.tabbedDocumentControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabbedDocumentControl.Location = new System.Drawing.Point(32, 24);
		this.tabbedDocumentControl.Margin = new System.Windows.Forms.Padding(5);
		this.tabbedDocumentControl.Name = "tabbedDocumentControl";
		toolStripProfessionalRenderer.RoundedEdges = true;
		this.tabbedDocumentControl.Renderer = toolStripProfessionalRenderer;
		this.tabbedDocumentControl.SelectedControl = null;
		this.tabbedDocumentControl.Size = new System.Drawing.Size(1017, 612);
		this.tabbedDocumentControl.TabIndex = 14;
		this.tabbedDocumentControl.SelectedControlChanged += new System.EventHandler(TabbedDocumentControl_SelectedControlChanged);
		this.tabbedDocumentControl.DocumentAdded += new TCX.Windows.Forms.Controls.TabbedDocuments.TabbedDocumentControl.DocumentHandler(TabbedDocumentControl_DocumentAdded);
		this.tabbedDocumentControl.DocumentRemoving += new TCX.Windows.Forms.Controls.TabbedDocuments.TabbedDocumentControl.CancelDocumentHandler(TabbedDocumentControl_DocumentRemoving);
		this.propertyGrid.CategoryForeColor = System.Drawing.Color.FromArgb(61, 61, 61);
		this.propertyGrid.CommandsLinkColor = System.Drawing.Color.FromArgb(5, 150, 212);
		this.propertyGrid.LineColor = System.Drawing.Color.FromArgb(219, 219, 219);
		this.propertyGrid.Location = new System.Drawing.Point(1049, 447);
		this.propertyGrid.Margin = new System.Windows.Forms.Padding(4);
		this.propertyGrid.Name = "propertyGrid";
		this.propertyGrid.Size = new System.Drawing.Size(173, 160);
		this.propertyGrid.TabIndex = 16;
		this.propertyGrid.Enter += new System.EventHandler(ChildControl_Enter);
		this.buildBackgroundWorker.WorkerReportsProgress = true;
		this.buildBackgroundWorker.WorkerSupportsCancellation = true;
		this.buildBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(BuildBackgroundWorker_DoWork);
		this.buildBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(BuildBackgroundWorker_ProgressChanged);
		this.buildBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BuildBackgroundWorker_RunWorkerCompleted);
		this.componentsToolBox.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
		this.componentsToolBox.FullRowSelect = true;
		this.componentsToolBox.HideSelection = false;
		this.componentsToolBox.HotTracking = true;
		this.componentsToolBox.ItemHeight = 20;
		this.componentsToolBox.Location = new System.Drawing.Point(4, 361);
		this.componentsToolBox.Margin = new System.Windows.Forms.Padding(4);
		this.componentsToolBox.Name = "componentsToolBox";
		this.componentsToolBox.SelectedCategory = "";
		this.componentsToolBox.ShowLines = false;
		this.componentsToolBox.Size = new System.Drawing.Size(187, 118);
		this.componentsToolBox.TabIndex = 20;
		this.componentsToolBox.Enter += new System.EventHandler(ChildControl_Enter);
		this.errorListControl.Location = new System.Drawing.Point(425, 635);
		this.errorListControl.Margin = new System.Windows.Forms.Padding(5);
		this.errorListControl.Name = "errorListControl";
		this.errorListControl.Size = new System.Drawing.Size(689, 156);
		this.errorListControl.TabIndex = 19;
		this.errorListControl.Enter += new System.EventHandler(ChildControl_Enter);
		this.outputTextBox.HideSelection = false;
		this.outputTextBox.Location = new System.Drawing.Point(76, 635);
		this.outputTextBox.Margin = new System.Windows.Forms.Padding(4);
		this.outputTextBox.Name = "outputTextBox";
		this.outputTextBox.ReadOnly = true;
		this.outputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
		this.outputTextBox.Size = new System.Drawing.Size(192, 24);
		this.outputTextBox.TabIndex = 18;
		this.outputTextBox.Text = "";
		this.outputTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(OutputTextBox_LinkClicked);
		this.outputTextBox.Enter += new System.EventHandler(ChildControl_Enter);
		this.projectExplorerControl.Location = new System.Drawing.Point(1049, 68);
		this.projectExplorerControl.Margin = new System.Windows.Forms.Padding(4);
		this.projectExplorerControl.Name = "projectExplorerControl";
		this.projectExplorerControl.Size = new System.Drawing.Size(161, 336);
		this.projectExplorerControl.TabIndex = 15;
		this.projectExplorerControl.AvailableMenuCommandsChange += new System.EventHandler(ChildControl_AvailableMenuCommandsChange);
		this.projectExplorerControl.BuildRequest += new TCX.CFD.Controls.ProjectManagerControl.BuildRequestEventHandler(BuildRequest);
		this.projectExplorerControl.Enter += new System.EventHandler(ChildControl_Enter);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.Controls.Add(this.componentsToolBox);
		base.Controls.Add(this.errorListControl);
		base.Controls.Add(this.outputTextBox);
		base.Controls.Add(this.propertyGrid);
		base.Controls.Add(this.projectExplorerControl);
		base.Controls.Add(this.tabbedDocumentControl);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "ProjectManagerControl";
		base.Controls.SetChildIndex(this.tabbedDocumentControl, 0);
		base.Controls.SetChildIndex(this.projectExplorerControl, 0);
		base.Controls.SetChildIndex(this.propertyGrid, 0);
		base.Controls.SetChildIndex(this.outputTextBox, 0);
		base.Controls.SetChildIndex(this.errorListControl, 0);
		base.Controls.SetChildIndex(this.componentsToolBox, 0);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
