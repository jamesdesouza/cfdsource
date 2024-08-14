using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace TCX.CFD.Classes.FileSystem;

public class OnlineServices
{
	public TextToSpeechEngines TextToSpeechEngine { get; set; }

	public SpeechToTextEngines SpeechToTextEngine { get; set; }

	public AmazonPollySettings AmazonPollySettings { get; set; } = new AmazonPollySettings();


	public GoogleCloudSettings GoogleCloudSettings { get; set; } = new GoogleCloudSettings();


	public bool IsReadyForTTS()
	{
		switch (TextToSpeechEngine)
		{
		case TextToSpeechEngines.AmazonPolly:
			if (!string.IsNullOrEmpty(AmazonPollySettings.ClientID))
			{
				return !string.IsNullOrEmpty(AmazonPollySettings.ClientSecret);
			}
			return false;
		case TextToSpeechEngines.GoogleCloud:
			return !string.IsNullOrEmpty(GoogleCloudSettings.ServiceAccountKeyJSON);
		default:
			return false;
		}
	}

	public bool IsReadyForSTT()
	{
		if (SpeechToTextEngine == SpeechToTextEngines.GoogleCloud)
		{
			return !string.IsNullOrEmpty(GoogleCloudSettings.ServiceAccountKeyJSON);
		}
		return false;
	}

	private string ToLiteral(string input)
	{
		using StringWriter stringWriter = new StringWriter();
		using CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
		codeDomProvider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), stringWriter, null);
		return stringWriter.ToString();
	}

	private string GetInitializationFromList(List<string> values)
	{
		List<string> list = new List<string>();
		foreach (string value in values)
		{
			list.Add("\"" + value + "\"");
		}
		return string.Join(", ", list);
	}

	public bool IsTTSProperlyConfigured()
	{
		switch (TextToSpeechEngine)
		{
		case TextToSpeechEngines.AmazonPolly:
			if (!string.IsNullOrEmpty(AmazonPollySettings.ClientID))
			{
				return !string.IsNullOrEmpty(AmazonPollySettings.ClientSecret);
			}
			return false;
		case TextToSpeechEngines.GoogleCloud:
			if (!string.IsNullOrEmpty(GoogleCloudSettings.ServiceAccountKeyFileName))
			{
				return !string.IsNullOrEmpty(GoogleCloudSettings.ServiceAccountKeyJSON);
			}
			return false;
		default:
			return true;
		}
	}

	public bool IsSTTProperlyConfigured()
	{
		if (SpeechToTextEngine == SpeechToTextEngines.GoogleCloud)
		{
			if (!string.IsNullOrEmpty(GoogleCloudSettings.ServiceAccountKeyFileName))
			{
				return !string.IsNullOrEmpty(GoogleCloudSettings.ServiceAccountKeyJSON);
			}
			return false;
		}
		return true;
	}

	public string GetTTSInitializationCode()
	{
		return TextToSpeechEngine switch
		{
			TextToSpeechEngines.AmazonPolly => $"new AmazonPollyTextToSpeechEngine(new AmazonPollySettings(\"{AmazonPollySettings.ClientID}\", \"{AmazonPollySettings.ClientSecret}\", \"{EnumHelper.TextToSpeechAmazonRegionToString(AmazonPollySettings.Region)}\", new List<string>() {{ {GetInitializationFromList(AmazonPollySettings.Lexicons)} }}))", 
			TextToSpeechEngines.GoogleCloud => $"new GoogleCloudTextToSpeechEngine(new GoogleCloudSettings({ToLiteral(GoogleCloudSettings.ServiceAccountKeyJSON)}))", 
			_ => "null", 
		};
	}

	public string GetSTTInitializationCode()
	{
		if (SpeechToTextEngine == SpeechToTextEngines.GoogleCloud)
		{
			return $"new GoogleCloudSpeechToTextEngine(new GoogleCloudSettings({ToLiteral(GoogleCloudSettings.ServiceAccountKeyJSON)}))";
		}
		return "null";
	}
}
