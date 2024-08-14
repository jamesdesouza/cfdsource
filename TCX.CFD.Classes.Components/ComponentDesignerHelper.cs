using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public static class ComponentDesignerHelper
{
	public static bool CanInsertActivitiesWithoutActiveCall(ProjectObject projectObject, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		foreach (Activity item in activitiesToInsert)
		{
			if ((item as IVadActivity).IsCallRelated())
			{
				return false;
			}
			if (item is CompositeActivity compositeActivity && !CanInsertActivitiesWithoutActiveCall(projectObject, compositeActivity.EnabledActivities))
			{
				return false;
			}
			if (item is UserComponent { FileObject: var componentFileObject } userComponent)
			{
				if (componentFileObject == null)
				{
					componentFileObject = (ComponentFileObject)projectObject.GetFileSystemObject(userComponent.GetRelativeFilePath());
					userComponent.FileObject = componentFileObject;
				}
				RootFlow rootFlow = componentFileObject.FlowLoader.GetRootFlow(FlowTypes.MainFlow);
				if (!CanInsertActivitiesWithoutActiveCall(projectObject, rootFlow.EnabledActivities))
				{
					return false;
				}
				RootFlow rootFlow2 = componentFileObject.FlowLoader.GetRootFlow(FlowTypes.ErrorHandler);
				if (!CanInsertActivitiesWithoutActiveCall(projectObject, rootFlow2.EnabledActivities))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool CanInsertActivitiesCheckingUserComponent(ComponentFileObject forbiddenFileObject, ReadOnlyCollection<Activity> activitiesToInsert)
	{
		foreach (Activity item in activitiesToInsert)
		{
			if (item is UserComponent { FileObject: var componentFileObject } userComponent)
			{
				if (componentFileObject == null)
				{
					componentFileObject = (ComponentFileObject)forbiddenFileObject.GetProjectObject().GetFileSystemObject(userComponent.GetRelativeFilePath());
					userComponent.FileObject = componentFileObject;
				}
				FlowLoader flowLoader = componentFileObject.FlowLoader;
				RootFlow rootFlow = flowLoader.GetRootFlow(FlowTypes.MainFlow);
				RootFlow rootFlow2 = flowLoader.GetRootFlow(FlowTypes.ErrorHandler);
				RootFlow rootFlow3 = flowLoader.GetRootFlow(FlowTypes.DisconnectHandler);
				List<ComponentFileObject> list = new List<ComponentFileObject>();
				list.AddRange(rootFlow.GetComponentFileObjects());
				list.AddRange(rootFlow2.GetComponentFileObjects());
				list.AddRange(rootFlow3.GetComponentFileObjects());
				if (componentFileObject == forbiddenFileObject || list.Contains(forbiddenFileObject))
				{
					return false;
				}
			}
			else if (item is CompositeActivity compositeActivity && !CanInsertActivitiesCheckingUserComponent(forbiddenFileObject, compositeActivity.EnabledActivities))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsMakeCallComponentUsed(ProjectObject projectObject, ReadOnlyCollection<Activity> activityCollection)
	{
		foreach (Activity item in activityCollection)
		{
			if (item is MakeCallComponent)
			{
				return true;
			}
			if (item is CompositeActivity compositeActivity && IsMakeCallComponentUsed(projectObject, compositeActivity.EnabledActivities))
			{
				return true;
			}
			if (item is UserComponent { FileObject: var componentFileObject } userComponent)
			{
				if (componentFileObject == null)
				{
					componentFileObject = (ComponentFileObject)projectObject.GetFileSystemObject(userComponent.GetRelativeFilePath());
					userComponent.FileObject = componentFileObject;
				}
				RootFlow rootFlow = componentFileObject.FlowLoader.GetRootFlow(FlowTypes.MainFlow);
				if (IsMakeCallComponentUsed(projectObject, rootFlow.EnabledActivities))
				{
					return true;
				}
				RootFlow rootFlow2 = componentFileObject.FlowLoader.GetRootFlow(FlowTypes.ErrorHandler);
				if (IsMakeCallComponentUsed(projectObject, rootFlow2.EnabledActivities))
				{
					return true;
				}
				RootFlow rootFlow3 = componentFileObject.FlowLoader.GetRootFlow(FlowTypes.DisconnectHandler);
				if (IsMakeCallComponentUsed(projectObject, rootFlow3.EnabledActivities))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void HideProperty(IDictionary properties, string name)
	{
		if (!(properties[name] is PropertyDescriptor propertyDescriptor))
		{
			return;
		}
		List<Attribute> list = new List<Attribute>(propertyDescriptor.Attributes.Count)
		{
			new BrowsableAttribute(browsable: false)
		};
		foreach (Attribute attribute in propertyDescriptor.Attributes)
		{
			if (!(attribute is BrowsableAttribute))
			{
				list.Add(attribute);
			}
		}
		PropertyDescriptor propertyDescriptor2 = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, list.ToArray());
		properties[propertyDescriptor2.Name] = propertyDescriptor2;
	}

	public static void UpdateExistingProperty(IDictionary properties, string name, string category)
	{
		UpdateExistingProperty(properties, name, category, null, isReadOnly: false);
	}

	public static void UpdateExistingProperty(IDictionary properties, string name, string category, string description)
	{
		UpdateExistingProperty(properties, name, category, description, isReadOnly: false);
	}

	public static void UpdateExistingProperty(IDictionary properties, string name, string category, bool isReadOnly)
	{
		UpdateExistingProperty(properties, name, category, null, isReadOnly);
	}

	public static void UpdateExistingProperty(IDictionary properties, string name, string category, string description, bool isReadOnly)
	{
		if (!(properties[name] is PropertyDescriptor propertyDescriptor))
		{
			return;
		}
		bool flag = false;
		List<Attribute> list = new List<Attribute>(propertyDescriptor.Attributes.Count);
		foreach (Attribute attribute in propertyDescriptor.Attributes)
		{
			if (attribute is CategoryAttribute)
			{
				list.Add(new CategoryAttribute(category));
			}
			else if (attribute is DescriptionAttribute && description != null)
			{
				list.Add(new DescriptionAttribute(description));
			}
			else if (attribute is ReadOnlyAttribute)
			{
				list.Add(new ReadOnlyAttribute(isReadOnly));
				flag = true;
			}
			else
			{
				list.Add(attribute);
			}
		}
		if (isReadOnly && !flag)
		{
			list.Add(new ReadOnlyAttribute(isReadOnly));
		}
		PropertyDescriptor propertyDescriptor2 = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, list.ToArray());
		properties[propertyDescriptor2.Name] = propertyDescriptor2;
	}
}
