using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TCX.CFD.Classes;

internal class ResourceManager
{
	private readonly string fileBaseName;

	private readonly string startupPath;

	private readonly CultureResources defaultCultureResources;

	private readonly Dictionary<CultureInfo, CultureResources> cultureResourcesMap;

	public ResourceManager(string fileBaseName, string startupPath)
	{
		this.fileBaseName = fileBaseName;
		this.startupPath = startupPath;
		defaultCultureResources = CultureResources.Create(Path.Combine(this.startupPath, fileBaseName + ".en.txt"));
		cultureResourcesMap = new Dictionary<CultureInfo, CultureResources>();
	}

	public string GetString(string resourceName, CultureInfo cultureInfo)
	{
		CultureResources cultureResources;
		if (cultureResourcesMap.ContainsKey(cultureInfo))
		{
			cultureResources = cultureResourcesMap[cultureInfo];
		}
		else
		{
			cultureResources = CultureResources.Create(Path.Combine(startupPath, fileBaseName + "." + cultureInfo.TwoLetterISOLanguageName + ".txt"));
			if (cultureResources == null)
			{
				cultureResources = defaultCultureResources;
			}
			if (cultureResources == null)
			{
				throw new ApplicationException("Default culture resources not found at '" + Path.Combine(startupPath, fileBaseName + ".en.txt") + "'.");
			}
			cultureResourcesMap.Add(cultureInfo, cultureResources);
		}
		return cultureResources.GetString(resourceName);
	}
}
