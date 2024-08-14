using System.Collections.Generic;
using System.ComponentModel;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Editors;

public class JsonXmlInputTypeConverter : StringConverter
{
	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return false;
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		List<string> list = new List<string>();
		if (context.Instance is IVadActivity vadActivity)
		{
			RootFlow rootFlow = vadActivity.GetRootFlow();
			foreach (string validVariable in ExpressionHelper.GetValidVariables(vadActivity))
			{
				string[] array = validVariable.Split('.');
				string text = array[0];
				if (rootFlow.GetActivityByName(text) is IVadActivity vadActivity2)
				{
					string text2 = array[1];
					if ((vadActivity2 is CRMLookupComponent && text2 == "Result") || (vadActivity2 is DatabaseAccessComponent && text2 == "ScalarResult") || (vadActivity2 is ExecuteCSharpCodeComponent && text2 == "ReturnValue") || (vadActivity2 is ExternalCodeExecutionComponent && text2 == "ReturnValue") || (vadActivity2 is FileManagementComponent && text2 == "Result") || (vadActivity2 is SocketClientComponent && text2 == "Response") || (vadActivity2 is WebInteractionComponent && text2 == "ResponseContent") || (vadActivity2 is WebServiceRestComponent && text2 == "ResponseContent") || (vadActivity2 is WebServicesInteractionComponent && text2 == "ResponseContent") || (vadActivity2 is TcxGetDnPropertyComponent && text2 == "PropertyValue") || (vadActivity2 is TcxGetGlobalPropertyComponent && text2 == "PropertyValue"))
					{
						list.Add(validVariable);
					}
				}
				else if (text == "callflow$" || text == "project$")
				{
					list.Add(validVariable);
				}
			}
		}
		return new StandardValuesCollection(list);
	}
}
