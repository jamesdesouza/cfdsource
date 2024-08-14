using System;
using System.Collections.Generic;
using System.IO;

namespace TCX.CFD.Classes;

internal class CultureResources
{
	private readonly Dictionary<string, string> resourceMap;

	private CultureResources(Dictionary<string, string> resourceMap)
	{
		this.resourceMap = resourceMap;
	}

	public string GetString(string resourceName)
	{
		if (resourceMap.ContainsKey(resourceName))
		{
			return resourceMap[resourceName];
		}
		throw new ApplicationException("Resource name " + resourceName + " not found.");
	}

	public static CultureResources Create(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string[] array = File.ReadAllLines(filePath);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.StartsWith("#") || string.IsNullOrEmpty(text))
			{
				continue;
			}
			int num = text.IndexOf('=');
			if (num > 0 && num < text.Length - 1)
			{
				string text2 = text.Substring(0, num).TrimEnd();
				string value = text.Substring(num + 1).TrimStart().Replace("\\n", "\n");
				if (dictionary.ContainsKey(text2))
				{
					throw new ApplicationException("Resource duplicated: " + text2);
				}
				dictionary.Add(text2, value);
			}
		}
		return new CultureResources(dictionary);
	}
}
