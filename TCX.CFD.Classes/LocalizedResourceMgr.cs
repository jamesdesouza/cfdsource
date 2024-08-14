using System;
using System.Globalization;
using System.Reflection;

namespace TCX.CFD.Classes;

internal class LocalizedResourceMgr
{
	private static object lockObj = new object();

	private static ResourceManager resourceManager = null;

	private LocalizedResourceMgr()
	{
	}

	public static string GetString(string resourceName)
	{
		lock (lockObj)
		{
			try
			{
				if (resourceManager == null)
				{
					string location = Assembly.GetExecutingAssembly().Location;
					string startupPath = location.Substring(0, location.LastIndexOf('\\'));
					resourceManager = new ResourceManager("3CX Call Flow Designer", startupPath);
				}
				return resourceManager.GetString(resourceName, CultureInfo.CurrentCulture);
			}
			catch (Exception innerException)
			{
				throw new ApplicationException($"Resource '{resourceName}' could not be found in resource files.", innerException);
			}
		}
	}
}
