using System.Collections.Generic;
using System.IO;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes;

public static class RecentProjectsHelper
{
	private static List<string> FilterValidProjects(List<string> recentProjects)
	{
		for (int i = 0; i < recentProjects.Count; i++)
		{
			if (!File.Exists(recentProjects[i]))
			{
				recentProjects.RemoveAt(i--);
			}
		}
		return recentProjects;
	}

	private static List<string> FilterMaxProjects(List<string> recentProjects)
	{
		while (recentProjects.Count > Settings.Default.MaxRecentProjects)
		{
			recentProjects.RemoveAt(recentProjects.Count - 1);
		}
		return recentProjects;
	}

	private static void SaveRecentProjects(List<string> recentProjects)
	{
		Settings.Default.RecentProjects = string.Join(",", recentProjects);
		Settings.Default.Save();
	}

	public static List<string> GetRecentProjects()
	{
		List<string> list = FilterMaxProjects(FilterValidProjects(new List<string>(Settings.Default.RecentProjects.Split(','))));
		SaveRecentProjects(list);
		return list;
	}

	public static void AddRecentProject(string projectPath)
	{
		List<string> list = FilterValidProjects(new List<string>(Settings.Default.RecentProjects.Split(',')));
		list.Remove(projectPath);
		list.Insert(0, projectPath);
		SaveRecentProjects(FilterMaxProjects(list));
	}
}
