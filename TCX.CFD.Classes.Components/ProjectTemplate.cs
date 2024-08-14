using System.Drawing;
using System.IO;
using System.Reflection;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class ProjectTemplate
{
	public string ID { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public string Folder { get; set; }

	public string GetFullPathToTemplate()
	{
		return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Templates", Folder);
	}

	public Image GetImage()
	{
		return ID switch
		{
			"authentication" => Resources.CreateProjectFromTemplate_Authentication, 
			"crm_lookup" => Resources.CreateProjectFromTemplate_CRMLookup, 
			"database_access" => Resources.CreateProjectFromTemplate_Database, 
			"date_time_routing" => Resources.CreateProjectFromTemplate_DateTimeCondition, 
			"email_sender" => Resources.CreateProjectFromTemplate_EmailSender, 
			"power_dialer" => Resources.CreateProjectFromTemplate_PowerDialer, 
			"predictive_dialer" => Resources.CreateProjectFromTemplate_PredictiveDialer, 
			"voice_input" => Resources.CreateProjectFromTemplate_VoiceInput, 
			"web_service_rest" => Resources.CreateProjectFromTemplate_WebService, 
			_ => Resources.CreateProjectFromTemplate_Component, 
		};
	}
}
