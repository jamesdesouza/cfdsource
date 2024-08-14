using System.Drawing;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class StartPageFeaturedComponent
{
	private readonly string _id;

	public string Title { get; private set; }

	public string Description { get; private set; }

	public string Link { get; private set; }

	public StartPageFeaturedComponent(string id, string title, string description, string link)
	{
		_id = id;
		Title = title;
		Description = description;
		Link = link;
	}

	public Image GetImage()
	{
		return _id switch
		{
			"voice_input" => Resources.StartPage_VoiceInput, 
			"json_xml_parser" => Resources.StartPage_JsonXmlParser, 
			"crm_lookup" => Resources.StartPage_CRMLookup, 
			"database_access" => Resources.StartPage_DatabaseAccess, 
			"web_service_rest" => Resources.StartPage_WebServiceRest, 
			"http_request" => Resources.StartPage_HttpRequest, 
			"execute_code" => Resources.StartPage_ExecuteCode, 
			"date_time_condition" => Resources.StartPage_DateTimeConditional, 
			_ => Resources.StartPage_Component, 
		};
	}
}
