using System.Collections.Generic;

namespace TCX.CFD.Classes.FileSystem;

public class AmazonPollySettings
{
	public string ClientID { get; set; } = string.Empty;


	public string ClientSecret { get; set; } = string.Empty;


	public TextToSpeechAmazonRegions Region { get; set; } = TextToSpeechAmazonRegions.USEastOhio;


	public List<string> Lexicons { get; set; } = new List<string>();

}
