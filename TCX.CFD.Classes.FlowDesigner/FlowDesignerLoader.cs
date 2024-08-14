using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.FlowDesigner;

public sealed class FlowDesignerLoader : WorkflowDesignerLoader
{
	private readonly FileObject fileObject;

	private readonly FlowTypes flowType;

	public override string FileName => fileObject.Path;

	public FlowDesignerLoader(FileObject fileObject, FlowTypes flowType)
	{
		this.fileObject = fileObject;
		this.flowType = flowType;
	}

	protected override void Initialize()
	{
		base.Initialize();
		IDesignerLoaderHost loaderHost = base.LoaderHost;
		if (loaderHost != null)
		{
			IServiceContainer serviceContainer = loaderHost.GetService(typeof(ServiceContainer)) as IServiceContainer;
			loaderHost.AddService(typeof(IMenuCommandService), new FlowDesignerMenuCommandService(serviceContainer));
			FlowDesignerUndoEngine flowDesignerUndoEngine = new FlowDesignerUndoEngine(serviceContainer);
			flowDesignerUndoEngine.Enabled = false;
			loaderHost.AddService(typeof(UndoEngine), flowDesignerUndoEngine);
		}
	}

	public override void Dispose()
	{
		IDesignerLoaderHost loaderHost = base.LoaderHost;
		if (loaderHost != null)
		{
			loaderHost.RemoveService(typeof(IMenuCommandService));
			loaderHost.RemoveService(typeof(UndoEngine));
		}
		base.Dispose();
	}

	public override TextReader GetFileReader(string filePath)
	{
		return new StreamReader(filePath);
	}

	public override TextWriter GetFileWriter(string filePath)
	{
		return new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate));
	}

	protected override void PerformLoad(IDesignerSerializationManager serializationManager)
	{
		try
		{
			base.PerformLoad(serializationManager);
			IDesignerHost designerHost = GetService(typeof(IDesignerHost)) as IDesignerHost;
			RootFlow rootFlow = fileObject.FlowLoader.GetRootFlow(flowType);
			if (rootFlow != null && designerHost != null)
			{
				designerHost.Container.Add(rootFlow, rootFlow.QualifiedName);
				Activity[] nestedActivities = GetNestedActivities(rootFlow);
				foreach (Activity activity in nestedActivities)
				{
					designerHost.Container.Add(activity, activity.QualifiedName);
				}
			}
			designerHost.Activate();
		}
		catch (Exception item)
		{
			List<Exception> list = new List<Exception>();
			list.Add(item);
			base.LoaderHost.EndLoad(string.Empty, successful: false, list);
		}
	}

	public void Save()
	{
		fileObject.FlowLoader.Save(flowType);
	}

	internal static Activity[] GetNestedActivities(CompositeActivity compositeActivity)
	{
		if (compositeActivity == null)
		{
			throw new ArgumentNullException("compositeActivity");
		}
		ArrayList arrayList = new ArrayList();
		Queue queue = new Queue();
		queue.Enqueue(compositeActivity);
		while (queue.Count > 0)
		{
			foreach (Activity item in (IEnumerable<Activity>)((CompositeActivity)queue.Dequeue()).Activities)
			{
				arrayList.Add(item);
				if (item is CompositeActivity)
				{
					queue.Enqueue(item);
				}
			}
		}
		return arrayList.ToArray(typeof(Activity)) as Activity[];
	}

	internal static void DestroyObjectGraphFromDesignerHost(IDesignerHost designerHost, Activity activity)
	{
		if (designerHost == null)
		{
			throw new ArgumentNullException("designerHost");
		}
		if (activity == null)
		{
			throw new ArgumentNullException("activity");
		}
		designerHost.DestroyComponent(activity);
		if (activity is CompositeActivity)
		{
			Activity[] nestedActivities = GetNestedActivities(activity as CompositeActivity);
			foreach (Activity component in nestedActivities)
			{
				designerHost.DestroyComponent(component);
			}
		}
	}
}
