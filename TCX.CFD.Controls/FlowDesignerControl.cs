using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;
using TCX.Windows.Forms.Controls.TabbedDocuments;

namespace TCX.CFD.Controls;

public class FlowDesignerControl : UserControl, IServiceProvider, ISite, IDisposable
{
	private readonly DesignSurfaceManager designSurfaceManager;

	private readonly PropertyGrid propertyGrid;

	private WorkflowView mainWorkflowView;

	private DesignSurface mainDesignSurface;

	private FlowDesignerLoader mainLoader;

	private WorkflowView errorHandlerWorkflowView;

	private DesignSurface errorHandlerDesignSurface;

	private FlowDesignerLoader errorHandlerLoader;

	private WorkflowView disconnectHandlerWorkflowView;

	private DesignSurface disconnectHandlerDesignSurface;

	private FlowDesignerLoader disconnectHandlerLoader;

	private IContainer components;

	private TabControl viewTabControl;

	private TabPage mainFlowTabPage;

	private TabPage errorHandlerTabPage;

	private TabPage disconnectHandlerTabPage;

	public IComponent Component => this;

	public new bool DesignMode => true;

	public WorkflowView CurrentView
	{
		get
		{
			if (viewTabControl.SelectedTab == mainFlowTabPage)
			{
				return mainWorkflowView;
			}
			if (viewTabControl.SelectedTab == errorHandlerTabPage)
			{
				return errorHandlerWorkflowView;
			}
			return disconnectHandlerWorkflowView;
		}
	}

	public bool NeedsDisconnectHandlerFlow => FileObject.NeedsDisconnectHandlerFlow;

	public bool IsDisconnectHandlerActive => viewTabControl.SelectedTab == disconnectHandlerTabPage;

	public FileObject FileObject { get; }

	public event EventHandler AvailableMenuCommandsChange;

	public event EventHandler ShowPropertiesWindow;

	private string GetDesignerTitle(FlowTypes flowType)
	{
		return flowType switch
		{
			FlowTypes.MainFlow => FileObject.GetNameWithoutExtension(), 
			FlowTypes.ErrorHandler => LocalizedResourceMgr.GetString("FlowDesignerControl.DesignerTitle.ErrorHandler") + " " + FileObject.GetNameWithoutExtension(), 
			FlowTypes.DisconnectHandler => LocalizedResourceMgr.GetString("FlowDesignerControl.DesignerTitle.DisconnectHandler") + " " + FileObject.GetNameWithoutExtension(), 
			_ => string.Empty, 
		};
	}

	private void SetDesignerTitle(DesignSurface designSurface, FlowTypes flowType)
	{
		if (designSurface.GetService(typeof(IDesignerHost)) is IDesignerHost { RootComponent: not null } designerHost && designerHost.GetDesigner(designerHost.RootComponent) is RootFlowDesigner rootFlowDesigner)
		{
			rootFlowDesigner.SetDesignerTitle(GetDesignerTitle(flowType));
		}
	}

	private void DestroyDesigner(ref WorkflowView workflowView, ref DesignSurface designSurface)
	{
		if (designSurface.GetService(typeof(UndoEngine)) is FlowDesignerUndoEngine flowDesignerUndoEngine)
		{
			flowDesignerUndoEngine.Enabled = false;
		}
		if (designSurface.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost && designerHost.Container.Components.Count > 0)
		{
			FlowDesignerLoader.DestroyObjectGraphFromDesignerHost(designerHost, designerHost.RootComponent as Activity);
		}
		if (designSurface.GetService(typeof(ISelectionService)) is ISelectionService selectionService)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		if (designSurface.GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.ComponentChanged -= OnComponentChanged;
		}
		if (designSurface.GetService(typeof(IMenuCommandService)) is FlowDesignerMenuCommandService flowDesignerMenuCommandService)
		{
			flowDesignerMenuCommandService.CommandExecuted -= OnCommandExecuted;
			flowDesignerMenuCommandService.ShowPropertiesRequested -= OnShowPropertiesWindow;
		}
		if (designSurface != null)
		{
			designSurface.Dispose();
			designSurface = null;
		}
		if (workflowView != null)
		{
			workflowView.Dispose();
			workflowView = null;
		}
	}

	private void CreateDesigner(ref WorkflowView workflowView, ref DesignSurface designSurface, ref FlowDesignerLoader loader, FlowTypes flowType)
	{
		designSurface = designSurfaceManager.CreateDesignSurface(designSurfaceManager);
		loader = new FlowDesignerLoader(FileObject, flowType);
		designSurface.Loaded += DesignSurface_Loaded;
		designSurface.BeginLoad(loader);
		if (designSurface.LoadErrors.Count > 0)
		{
			string text = string.Empty;
			foreach (Exception loadError in designSurface.LoadErrors)
			{
				text = text + ErrorHelper.GetErrorDescription(loadError) + "\n";
			}
			throw new ApplicationException(LocalizedResourceMgr.GetString("FlowDesignerControl.MessageBox.Error.Loading") + text);
		}
		if (!(designSurface.GetService(typeof(IDesignerHost)) is IDesignerHost { RootComponent: not null } designerHost) || !(designerHost.GetDesigner(designerHost.RootComponent) is RootFlowDesigner rootFlowDesigner))
		{
			return;
		}
		rootFlowDesigner.SetDesignerTitle(GetDesignerTitle(flowType));
		rootFlowDesigner.LayoutChanged += RootDesigner_LayoutChanged;
		workflowView = ((IRootDesigner)rootFlowDesigner).GetView(ViewTechnology.Default) as WorkflowView;
		workflowView.Dock = DockStyle.Fill;
		workflowView.PrintDocument.DocumentName = GetDesignerTitle(flowType);
		workflowView.PrintDocument.OriginAtMargins = true;
		workflowView.Site = designerHost.RootComponent.Site;
		workflowView.TabStop = true;
		workflowView.HScrollBar.TabStop = false;
		workflowView.VScrollBar.TabStop = false;
		workflowView.Enter += WorkflowView_Enter;
		workflowView.ZoomChanged += WorkflowView_ZoomChanged;
		if (workflowView.Controls.Count == 2)
		{
			Control control = workflowView.Controls[1];
			if (control.Controls.Count == 3)
			{
				control.Controls[1].Visible = false;
				control.Controls[2].Visible = false;
			}
		}
		if (((IServiceProvider)workflowView).GetService(typeof(ISelectionService)) is ISelectionService selectionService)
		{
			selectionService.SelectionChanged += OnSelectionChanged;
		}
		if (((IServiceProvider)workflowView).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.ComponentChanged += OnComponentChanged;
		}
		if (((IServiceProvider)workflowView).GetService(typeof(IMenuCommandService)) is FlowDesignerMenuCommandService flowDesignerMenuCommandService)
		{
			flowDesignerMenuCommandService.CommandExecuted += OnCommandExecuted;
			flowDesignerMenuCommandService.ShowPropertiesRequested += OnShowPropertiesWindow;
		}
	}

	private void DesignSurface_Loaded(object sender, LoadedEventArgs e)
	{
		if (e.HasSucceeded)
		{
			OnEnter(EventArgs.Empty);
			if ((sender as DesignSurface).GetService(typeof(UndoEngine)) is FlowDesignerUndoEngine flowDesignerUndoEngine)
			{
				flowDesignerUndoEngine.Enabled = true;
			}
		}
	}

	private void RootDesigner_LayoutChanged(object sender, EventArgs e)
	{
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	private void InitializeUserComponent(UserComponent userComponent)
	{
		if (userComponent.FileObject == null)
		{
			string relativeFilePath = userComponent.GetRelativeFilePath();
			if (!(FileObject.GetProjectObject().GetFileSystemObject(relativeFilePath) is ComponentFileObject fileObject))
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("FlowDesignerControl.MessageBox.Error.ReferenceNotFound"), relativeFilePath), LocalizedResourceMgr.GetString("FlowDesignerControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				userComponent.Enabled = false;
			}
			else
			{
				userComponent.FileObject = fileObject;
			}
		}
	}

	private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
	{
		if (e.NewValue != null || e.OldValue != null)
		{
			FileObject.HasChanges = true;
		}
		if (e.Member != null && e.Member.Name == "Name")
		{
			(e.Component as IVadActivity).GetRootFlow()?.NotifyComponentRenamed(e.OldValue as string, e.NewValue as string);
		}
		if (e.OldValue != null && e.OldValue is ActivityCollectionChangeEventArgs)
		{
			foreach (Activity addedItem in (e.OldValue as ActivityCollectionChangeEventArgs).AddedItems)
			{
				if (addedItem is CompositeActivity)
				{
					Activity[] nestedActivities = FlowDesignerLoader.GetNestedActivities(addedItem as CompositeActivity);
					foreach (Activity activity in nestedActivities)
					{
						if (activity is UserComponent)
						{
							InitializeUserComponent(activity as UserComponent);
						}
					}
				}
				else if (addedItem is UserComponent)
				{
					InitializeUserComponent(addedItem as UserComponent);
				}
			}
		}
		propertyGrid.Refresh();
	}

	private void OnCommandExecuted(object sender, EventArgs e)
	{
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	private void OnShowPropertiesWindow(object sender, EventArgs e)
	{
		this.ShowPropertiesWindow?.Invoke(this, EventArgs.Empty);
	}

	private void WorkflowView_Enter(object sender, EventArgs e)
	{
		ISelectionService sender2 = (sender as IServiceProvider).GetService(typeof(ISelectionService)) as ISelectionService;
		OnSelectionChanged(sender2, EventArgs.Empty);
	}

	private void WorkflowView_ZoomChanged(object sender, EventArgs e)
	{
		this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		if (!(sender is ISelectionService selectionService))
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		foreach (object selectedComponent in selectionService.GetSelectedComponents())
		{
			if (selectedComponent is RootFlow)
			{
				arrayList.Add(FileObject);
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

	public FlowDesignerControl(FileObject fileObject, DesignSurfaceManager designSurfaceManager, PropertyGrid propertyGrid)
	{
		InitializeComponent();
		SuspendLayout();
		FileObject = fileObject;
		this.designSurfaceManager = designSurfaceManager;
		this.propertyGrid = propertyGrid;
		base.Name = fileObject.GetNameWithoutExtension();
		base.AccessibleDescription = fileObject.Path;
		base.Tag = fileObject.GetImage();
		fileObject.NameChanged += FileObject_NameChanged;
		fileObject.Saving += FileObject_Saving;
		fileObject.FlowTypeChanged += FileObject_FlowTypeChanged;
		fileObject.ActivitySelected += FileObject_ActivitySelected;
		CreateDesigner(ref mainWorkflowView, ref mainDesignSurface, ref mainLoader, FlowTypes.MainFlow);
		mainFlowTabPage.Controls.Add(mainWorkflowView);
		mainFlowTabPage.Text = LocalizedResourceMgr.GetString("FlowDesignerControl.MainFlowTabPage.Text");
		CreateDesigner(ref errorHandlerWorkflowView, ref errorHandlerDesignSurface, ref errorHandlerLoader, FlowTypes.ErrorHandler);
		errorHandlerTabPage.Controls.Add(errorHandlerWorkflowView);
		errorHandlerTabPage.Text = LocalizedResourceMgr.GetString("FlowDesignerControl.ErrorHandlerTabPage.Text");
		if (fileObject.NeedsDisconnectHandlerFlow)
		{
			CreateDesigner(ref disconnectHandlerWorkflowView, ref disconnectHandlerDesignSurface, ref disconnectHandlerLoader, FlowTypes.DisconnectHandler);
			disconnectHandlerTabPage.Controls.Add(disconnectHandlerWorkflowView);
			disconnectHandlerTabPage.Text = LocalizedResourceMgr.GetString("FlowDesignerControl.DisconnectHandlerTabPage.Text");
			viewTabControl.Controls.Add(disconnectHandlerTabPage);
		}
		ResumeLayout(performLayout: true);
	}

	private void FileObject_NameChanged(object sender, EventArgs e)
	{
		base.Name = FileObject.GetNameWithoutExtension();
		base.AccessibleDescription = FileObject.Path;
		(base.Parent as TabbedDocumentControl).ItemTitles[this] = FileObject.GetNameWithoutExtension() + (FileObject.HasChanges ? "*" : string.Empty);
		SetDesignerTitle(mainDesignSurface, FlowTypes.MainFlow);
		SetDesignerTitle(errorHandlerDesignSurface, FlowTypes.ErrorHandler);
		if (FileObject.NeedsDisconnectHandlerFlow)
		{
			SetDesignerTitle(disconnectHandlerDesignSurface, FlowTypes.DisconnectHandler);
		}
	}

	private void FileObject_Saving(object sender, CancelEventArgs e)
	{
		mainLoader.Save();
		errorHandlerLoader.Save();
		if (FileObject.NeedsDisconnectHandlerFlow)
		{
			disconnectHandlerLoader.Save();
		}
	}

	private void FileObject_FlowTypeChanged(FileObject sender, FlowTypes flowType)
	{
		switch (flowType)
		{
		case FlowTypes.MainFlow:
			viewTabControl.SelectedTab = mainFlowTabPage;
			break;
		case FlowTypes.ErrorHandler:
			viewTabControl.SelectedTab = errorHandlerTabPage;
			break;
		case FlowTypes.DisconnectHandler:
			viewTabControl.SelectedTab = disconnectHandlerTabPage;
			break;
		}
	}

	private void FileObject_ActivitySelected(FileObject sender, string activityName, FlowTypes flowType)
	{
		Activity activityByName = FileObject.FlowLoader.GetRootFlow(flowType).GetActivityByName(activityName);
		if (activityByName != null)
		{
			ArrayList selectedComponents = new ArrayList { activityByName };
			(((IServiceProvider)designSurfaceManager.ActiveDesignSurface).GetService(typeof(ISelectionService)) as ISelectionService).SetSelectedComponents(selectedComponents);
			WorkflowView currentView = CurrentView;
			if (currentView != null && !currentView.IsDisposed)
			{
				currentView.EnsureVisible(activityByName);
			}
		}
	}

	private void SetCurrentDesignSurface()
	{
		switch (viewTabControl.SelectedIndex)
		{
		case 0:
			designSurfaceManager.ActiveDesignSurface = mainDesignSurface;
			break;
		case 1:
			designSurfaceManager.ActiveDesignSurface = errorHandlerDesignSurface;
			break;
		case 2:
			designSurfaceManager.ActiveDesignSurface = disconnectHandlerDesignSurface;
			break;
		}
		ISelectionService sender = ((IServiceProvider)designSurfaceManager.ActiveDesignSurface).GetService(typeof(ISelectionService)) as ISelectionService;
		OnSelectionChanged(sender, EventArgs.Empty);
	}

	private void ViewTabControl_DrawItem(object sender, DrawItemEventArgs e)
	{
		Rectangle tabRect = viewTabControl.GetTabRect(viewTabControl.TabPages.Count - 1);
		RectangleF rect = new RectangleF(tabRect.X + tabRect.Width, tabRect.Y - 5, viewTabControl.Width - (tabRect.X + tabRect.Width), tabRect.Height + 5);
		using (Brush brush = new SolidBrush(Color.FromArgb(219, 219, 219)))
		{
			e.Graphics.FillRectangle(brush, rect);
		}
		using Brush brush2 = new SolidBrush((e.State == DrawItemState.Selected) ? Color.White : SystemColors.Control);
		Rectangle bounds = e.Bounds;
		if (e.State != DrawItemState.Selected)
		{
			bounds.Inflate(0, 5);
		}
		e.Graphics.FillRectangle(brush2, bounds);
		SizeF sizeF = e.Graphics.MeasureString(viewTabControl.TabPages[e.Index].Text, e.Font);
		bounds.Offset((int)((float)e.Bounds.Width - sizeF.Width - 21f) / 2, 4);
		e.Graphics.DrawImage((e.Index == 0) ? Resources.MainFlow : ((e.Index == 1) ? Resources.ErrorHandlerFlow : Resources.DisconnectHandlerFlow), bounds.Location);
		bounds.Offset(21, 3);
		e.Graphics.DrawString(viewTabControl.TabPages[e.Index].Text, e.Font, Brushes.Black, bounds.Location);
	}

	private void ViewTabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetCurrentDesignSurface();
		this.AvailableMenuCommandsChange(this, EventArgs.Empty);
	}

	protected override void OnEnter(EventArgs e)
	{
		base.OnEnter(e);
		SetCurrentDesignSurface();
	}

	public new object GetService(Type serviceType)
	{
		return base.GetService(serviceType);
	}

	private MenuCommand GetMenuCommand(CommandID cmd)
	{
		WorkflowView currentView = CurrentView;
		if (currentView == null)
		{
			return null;
		}
		if (!(((IServiceProvider)currentView).GetService(typeof(IMenuCommandService)) is IMenuCommandService menuCommandService))
		{
			return null;
		}
		return menuCommandService.FindCommand(cmd);
	}

	public void InvokeStandardCommand(CommandID cmd)
	{
		if (((IServiceProvider)CurrentView).GetService(typeof(IMenuCommandService)) is IMenuCommandService menuCommandService)
		{
			menuCommandService.GlobalInvoke(cmd);
			this.AvailableMenuCommandsChange?.Invoke(this, EventArgs.Empty);
		}
	}

	public bool IsStandardCommandChecked(CommandID cmd)
	{
		return GetMenuCommand(cmd)?.Checked ?? false;
	}

	public bool IsStandardCommandEnabled(CommandID cmd)
	{
		return GetMenuCommand(cmd)?.Enabled ?? false;
	}

	public string GetUndoDescription()
	{
		WorkflowView currentView = CurrentView;
		if (currentView == null)
		{
			return string.Empty;
		}
		if (!(((IServiceProvider)currentView).GetService(typeof(UndoEngine)) is FlowDesignerUndoEngine flowDesignerUndoEngine))
		{
			return string.Empty;
		}
		return flowDesignerUndoEngine.GetLastUndoDescription();
	}

	public string GetRedoDescription()
	{
		WorkflowView currentView = CurrentView;
		if (currentView == null)
		{
			return string.Empty;
		}
		if (!(((IServiceProvider)currentView).GetService(typeof(UndoEngine)) is FlowDesignerUndoEngine flowDesignerUndoEngine))
		{
			return string.Empty;
		}
		return flowDesignerUndoEngine.GetLastRedoDescription();
	}

	public void SetReadOnlyDesigners()
	{
		mainWorkflowView.Enabled = false;
		errorHandlerWorkflowView.Enabled = false;
		if (FileObject.NeedsDisconnectHandlerFlow)
		{
			disconnectHandlerWorkflowView.Enabled = false;
		}
	}

	public void SetReadWriteDesigners()
	{
		mainWorkflowView.Enabled = true;
		errorHandlerWorkflowView.Enabled = true;
		if (FileObject.NeedsDisconnectHandlerFlow)
		{
			disconnectHandlerWorkflowView.Enabled = true;
		}
	}

	public void SetDebuggingActivity(Activity currentlyDebuggingActivity, FlowTypes debuggingFlowType)
	{
		switch (debuggingFlowType)
		{
		case FlowTypes.MainFlow:
			viewTabControl.SelectedTab = mainFlowTabPage;
			break;
		case FlowTypes.ErrorHandler:
			viewTabControl.SelectedTab = errorHandlerTabPage;
			break;
		case FlowTypes.DisconnectHandler:
			viewTabControl.SelectedTab = disconnectHandlerTabPage;
			break;
		}
		ArrayList selectedComponents = new ArrayList { currentlyDebuggingActivity };
		(((IServiceProvider)designSurfaceManager.ActiveDesignSurface).GetService(typeof(ISelectionService)) as ISelectionService).SetSelectedComponents(selectedComponents);
		CurrentView?.EnsureVisible(currentlyDebuggingActivity);
	}

	public WorkflowView GetWorkflowViewFor(FlowTypes flowType)
	{
		return flowType switch
		{
			FlowTypes.MainFlow => mainWorkflowView, 
			FlowTypes.ErrorHandler => errorHandlerWorkflowView, 
			FlowTypes.DisconnectHandler => disconnectHandlerWorkflowView, 
			_ => null, 
		};
	}

	public IDesignerHost GetDesignerHostFor(FlowTypes flowType)
	{
		return flowType switch
		{
			FlowTypes.MainFlow => mainDesignSurface.GetService(typeof(IDesignerHost)) as IDesignerHost, 
			FlowTypes.ErrorHandler => errorHandlerDesignSurface.GetService(typeof(IDesignerHost)) as IDesignerHost, 
			FlowTypes.DisconnectHandler => disconnectHandlerDesignSurface.GetService(typeof(IDesignerHost)) as IDesignerHost, 
			_ => null, 
		};
	}

	private void FlowDesignerControl_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		if (((IServiceProvider)designSurfaceManager.ActiveDesignSurface)?.GetService(typeof(ISelectionService)) is ISelectionService selectionService)
		{
			ICollection selectedComponents = selectionService.GetSelectedComponents();
			if (selectedComponents.Count == 1)
			{
				foreach (object item in selectedComponents)
				{
					if (item is ConnectorHitTestInfo)
					{
						Activity activity = (item as ConnectorHitTestInfo).AssociatedDesigner?.Activity;
						if (activity != null && activity is IVadActivity vadActivity)
						{
							vadActivity.ShowHelp();
							return;
						}
					}
					else if (item is IVadActivity vadActivity2)
					{
						vadActivity2.ShowHelp();
						return;
					}
				}
			}
		}
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/#h.9emcy6sp073c");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (components != null)
			{
				components.Dispose();
			}
			FileObject.NameChanged -= FileObject_NameChanged;
			FileObject.Saving -= FileObject_Saving;
			FileObject.FlowTypeChanged -= FileObject_FlowTypeChanged;
			FileObject.ActivitySelected -= FileObject_ActivitySelected;
			DestroyDesigner(ref mainWorkflowView, ref mainDesignSurface);
			DestroyDesigner(ref errorHandlerWorkflowView, ref errorHandlerDesignSurface);
			if (NeedsDisconnectHandlerFlow)
			{
				DestroyDesigner(ref disconnectHandlerWorkflowView, ref disconnectHandlerDesignSurface);
			}
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.viewTabControl = new System.Windows.Forms.TabControl();
		this.mainFlowTabPage = new System.Windows.Forms.TabPage();
		this.errorHandlerTabPage = new System.Windows.Forms.TabPage();
		this.disconnectHandlerTabPage = new System.Windows.Forms.TabPage();
		this.viewTabControl.SuspendLayout();
		base.SuspendLayout();
		this.viewTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.viewTabControl.Controls.Add(this.mainFlowTabPage);
		this.viewTabControl.Controls.Add(this.errorHandlerTabPage);
		this.viewTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.viewTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
		this.viewTabControl.ItemSize = new System.Drawing.Size(160, 21);
		this.viewTabControl.Location = new System.Drawing.Point(0, 0);
		this.viewTabControl.Margin = new System.Windows.Forms.Padding(4);
		this.viewTabControl.Name = "viewTabControl";
		this.viewTabControl.SelectedIndex = 0;
		this.viewTabControl.Size = new System.Drawing.Size(449, 185);
		this.viewTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.viewTabControl.TabIndex = 0;
		this.viewTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(ViewTabControl_DrawItem);
		this.viewTabControl.SelectedIndexChanged += new System.EventHandler(ViewTabControl_SelectedIndexChanged);
		this.mainFlowTabPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mainFlowTabPage.ImageIndex = 0;
		this.mainFlowTabPage.Location = new System.Drawing.Point(4, 4);
		this.mainFlowTabPage.Margin = new System.Windows.Forms.Padding(4);
		this.mainFlowTabPage.Name = "mainFlowTabPage";
		this.mainFlowTabPage.Padding = new System.Windows.Forms.Padding(4);
		this.mainFlowTabPage.Size = new System.Drawing.Size(441, 156);
		this.mainFlowTabPage.TabIndex = 0;
		this.mainFlowTabPage.Text = "Main Flow";
		this.mainFlowTabPage.UseVisualStyleBackColor = true;
		this.errorHandlerTabPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.errorHandlerTabPage.ImageIndex = 1;
		this.errorHandlerTabPage.Location = new System.Drawing.Point(4, 4);
		this.errorHandlerTabPage.Margin = new System.Windows.Forms.Padding(4);
		this.errorHandlerTabPage.Name = "errorHandlerTabPage";
		this.errorHandlerTabPage.Padding = new System.Windows.Forms.Padding(4);
		this.errorHandlerTabPage.Size = new System.Drawing.Size(441, 156);
		this.errorHandlerTabPage.TabIndex = 1;
		this.errorHandlerTabPage.Text = "Error Handler Flow";
		this.errorHandlerTabPage.UseVisualStyleBackColor = true;
		this.disconnectHandlerTabPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.disconnectHandlerTabPage.ImageIndex = 2;
		this.disconnectHandlerTabPage.Location = new System.Drawing.Point(4, 4);
		this.disconnectHandlerTabPage.Name = "disconnectHandlerTabPage";
		this.disconnectHandlerTabPage.Padding = new System.Windows.Forms.Padding(3);
		this.disconnectHandlerTabPage.Size = new System.Drawing.Size(329, 123);
		this.disconnectHandlerTabPage.TabIndex = 2;
		this.disconnectHandlerTabPage.Text = "Disconnect Handler Flow";
		this.disconnectHandlerTabPage.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Info;
		base.Controls.Add(this.viewTabControl);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "FlowDesignerControl";
		base.Size = new System.Drawing.Size(449, 185);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(FlowDesignerControl_HelpRequested);
		this.viewTabControl.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	[SpecialName]
	IContainer ISite.get_Container()
	{
		return base.Container;
	}

	[SpecialName]
	string ISite.get_Name()
	{
		return base.Name;
	}

	[SpecialName]
	void ISite.set_Name(string value)
	{
		base.Name = value;
	}
}
