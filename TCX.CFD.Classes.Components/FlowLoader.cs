using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Classes.FlowDesigner;

namespace TCX.CFD.Classes.Components;

public class FlowLoader
{
	private readonly Dictionary<FlowTypes, RootFlow> rootFlowDictionary = new Dictionary<FlowTypes, RootFlow>();

	private RootFlow CreateNewFlow(FlowTypes flowType)
	{
		return flowType switch
		{
			FlowTypes.MainFlow => new MainFlow(), 
			FlowTypes.ErrorHandler => new ErrorHandlerFlow(), 
			FlowTypes.DisconnectHandler => new DisconnectHandlerFlow(), 
			_ => throw new InvalidEnumArgumentException(LocalizedResourceMgr.GetString("FlowLoader.MessageBox.Error.FlowTypeIsInvalid")), 
		};
	}

	private string AdjustNamespace(string flowContent)
	{
		int num = flowContent.IndexOf("Assembly=3CX Call Flow Designer");
		if (num == -1)
		{
			return flowContent;
		}
		int num2 = flowContent.IndexOf("\"", num);
		if (num2 == -1)
		{
			return flowContent;
		}
		return flowContent.Substring(0, num) + "Assembly=" + Assembly.GetExecutingAssembly().FullName + flowContent.Substring(num2);
	}

	private string AdjustExecuteCSharpCodeProperties(string flowContent)
	{
		return flowContent.Replace("ObjectType=", "ClassName=");
	}

	private string AdjustAllowDtmfInput(string flowContent)
	{
		return flowContent.Replace("AllowDtmfInput=", "AcceptDtmfInput=");
	}

	private string AdjustMaxRetries(string flowContent)
	{
		return flowContent.Replace("MaxRetryCount=\"0\"", "MaxRetryCount=\"1\"");
	}

	private string AdjustFlowContent(string flowContent)
	{
		flowContent = AdjustNamespace(flowContent);
		flowContent = AdjustExecuteCSharpCodeProperties(flowContent);
		flowContent = AdjustAllowDtmfInput(flowContent);
		flowContent = AdjustMaxRetries(flowContent);
		return flowContent;
	}

	private void Load(FileObject fileObject, FlowTypes flowType)
	{
		RootFlow rootFlow = null;
		string fileContent = fileObject.GetFileContent(flowType);
		if (string.IsNullOrEmpty(fileContent))
		{
			rootFlow = CreateNewFlow(flowType);
			rootFlow.Name = fileObject.GetNameWithoutExtension();
			rootFlow.FileObject = fileObject;
			rootFlowDictionary[flowType] = rootFlow;
		}
		else
		{
			fileContent = AdjustFlowContent(fileContent);
			using XmlReader reader = new XmlTextReader(new StringReader(fileContent));
			rootFlow = new CompositeActivityMarkupSerializer().Deserialize(reader) as RootFlow;
			if (rootFlow != null)
			{
				rootFlow.FileObject = fileObject;
				rootFlowDictionary[flowType] = rootFlow;
				Activity[] nestedActivities = FlowDesignerLoader.GetNestedActivities(rootFlow);
				foreach (Activity activity in nestedActivities)
				{
					if (activity is UserComponent)
					{
						UserComponent userComponent = activity as UserComponent;
						string relativeFilePath = userComponent.GetRelativeFilePath();
						if (!(fileObject.GetProjectObject().GetFileSystemObject(relativeFilePath) is ComponentFileObject fileObject2))
						{
							MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("FlowLoader.MessageBox.Error.ReferenceNotFound"), fileObject.RelativePath, relativeFilePath), LocalizedResourceMgr.GetString("FlowLoader.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
							activity.Enabled = false;
						}
						else
						{
							userComponent.FileObject = fileObject2;
						}
					}
					if (fileObject.FsoVersion == FileSystemObjectVersions.V1_0)
					{
						if (activity is WebInteractionComponent)
						{
							WebInteractionComponent webInteractionComponent = activity as WebInteractionComponent;
							webInteractionComponent.URI = "\"" + webInteractionComponent.URI + "\"";
						}
						else if (activity is WebServicesInteractionComponent)
						{
							WebServicesInteractionComponent webServicesInteractionComponent = activity as WebServicesInteractionComponent;
							webServicesInteractionComponent.URI = "\"" + webServicesInteractionComponent.URI + "\"";
							webServicesInteractionComponent.WebServiceName = "\"" + webServicesInteractionComponent.WebServiceName + "\"";
						}
					}
					if (fileObject.FsoVersion == FileSystemObjectVersions.V1_0 || fileObject.FsoVersion == FileSystemObjectVersions.V1_1)
					{
						if (activity is FileManagementComponent)
						{
							FileManagementComponent fileManagementComponent = activity as FileManagementComponent;
							fileManagementComponent.FileName = "\"" + fileManagementComponent.FileName + "\"";
						}
						if (activity is IVadActivity)
						{
							(activity as IVadActivity).MigrateConstantStringExpressions();
						}
					}
					if ((fileObject.FsoVersion != 0 && fileObject.FsoVersion != FileSystemObjectVersions.V1_1 && fileObject.FsoVersion != FileSystemObjectVersions.V2_0) || !(activity is DatabaseAccessComponent))
					{
						continue;
					}
					DatabaseAccessComponent databaseAccessComponent = activity as DatabaseAccessComponent;
					if (databaseAccessComponent.Parameters.Count == 0)
					{
						databaseAccessComponent.SqlStatement = "\"" + ExpressionHelper.EscapeConstantString(databaseAccessComponent.SqlStatement) + "\"";
						continue;
					}
					string text = "CONCATENATE(\"" + ExpressionHelper.EscapeConstantString(databaseAccessComponent.SqlStatement) + "\")";
					for (int j = 0; j < databaseAccessComponent.Parameters.Count; j++)
					{
						Parameter parameter = databaseAccessComponent.Parameters[j];
						text = text.Replace("{" + j + "}", "\"," + parameter.Value + ",\"");
					}
					text = text.Replace(",\"\")", ")");
					databaseAccessComponent.SqlStatement = text;
				}
			}
		}
		if (rootFlow == null)
		{
			throw new FormatException(LocalizedResourceMgr.GetString("FlowLoader.MessageBox.Error.InvalidFileFormat"));
		}
	}

	public FlowLoader(FileObject fileObject)
	{
		Load(fileObject, FlowTypes.MainFlow);
		Load(fileObject, FlowTypes.ErrorHandler);
		if (fileObject.NeedsDisconnectHandlerFlow)
		{
			Load(fileObject, FlowTypes.DisconnectHandler);
		}
	}

	public RootFlow GetRootFlow(FlowTypes flowType)
	{
		return rootFlowDictionary[flowType];
	}

	public bool DisableUserComponent(ComponentFileObject componentFileObject)
	{
		bool result = false;
		if (rootFlowDictionary[FlowTypes.MainFlow].DisableUserComponent(componentFileObject))
		{
			result = true;
		}
		if (rootFlowDictionary[FlowTypes.ErrorHandler].DisableUserComponent(componentFileObject))
		{
			result = true;
		}
		if (rootFlowDictionary.ContainsKey(FlowTypes.DisconnectHandler) && rootFlowDictionary[FlowTypes.DisconnectHandler].DisableUserComponent(componentFileObject))
		{
			result = true;
		}
		return result;
	}

	public bool IsUsingUserComponent(ComponentFileObject componentFileObject)
	{
		if (rootFlowDictionary[FlowTypes.MainFlow].IsUsingUserComponent(componentFileObject))
		{
			return true;
		}
		if (rootFlowDictionary[FlowTypes.ErrorHandler].IsUsingUserComponent(componentFileObject))
		{
			return true;
		}
		if (rootFlowDictionary.ContainsKey(FlowTypes.DisconnectHandler) && rootFlowDictionary[FlowTypes.DisconnectHandler].IsUsingUserComponent(componentFileObject))
		{
			return true;
		}
		return false;
	}

	public void Save()
	{
		Save(FlowTypes.MainFlow);
		Save(FlowTypes.ErrorHandler);
		if (rootFlowDictionary.ContainsKey(FlowTypes.DisconnectHandler))
		{
			Save(FlowTypes.DisconnectHandler);
		}
	}

	public void Save(FlowTypes flowType)
	{
		RootFlow rootFlow = rootFlowDictionary[flowType];
		StringBuilder stringBuilder = new StringBuilder();
		using XmlWriter writer = XmlWriter.Create(stringBuilder);
		new WorkflowMarkupSerializer().Serialize(writer, rootFlow);
		rootFlow.FileObject.SetFileContent(flowType, stringBuilder.ToString());
	}
}
