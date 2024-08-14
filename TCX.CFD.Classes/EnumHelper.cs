using System;

namespace TCX.CFD.Classes;

public static class EnumHelper
{
	public static string DialerModeToString(DialerModes e)
	{
		return e switch
		{
			DialerModes.PowerDialer => "PowerDialer", 
			DialerModes.PredictiveDialer => "PredictiveDialer", 
			_ => string.Empty, 
		};
	}

	public static DialerModes StringToDialerMode(string s)
	{
		if (!(s == "PowerDialer"))
		{
			if (s == "PredictiveDialer")
			{
				return DialerModes.PredictiveDialer;
			}
			return DialerModes.PowerDialer;
		}
		return DialerModes.PowerDialer;
	}

	public static string PredictiveDialerOptimizationToString(PredictiveDialerOptimizations e)
	{
		return e switch
		{
			PredictiveDialerOptimizations.ForAgents => "ForAgents", 
			PredictiveDialerOptimizations.ForCallees => "ForCallees", 
			_ => string.Empty, 
		};
	}

	public static PredictiveDialerOptimizations StringToPredictiveDialerOptimization(string s)
	{
		if (!(s == "ForAgents"))
		{
			if (s == "ForCallees")
			{
				return PredictiveDialerOptimizations.ForCallees;
			}
			return PredictiveDialerOptimizations.ForAgents;
		}
		return PredictiveDialerOptimizations.ForAgents;
	}

	public static string HttpRequestTypeToString(HttpRequestTypes e)
	{
		return e switch
		{
			HttpRequestTypes.DELETE => "DELETE", 
			HttpRequestTypes.GET => "GET", 
			HttpRequestTypes.HEAD => "HEAD", 
			HttpRequestTypes.OPTIONS => "OPTIONS", 
			HttpRequestTypes.POST => "POST", 
			HttpRequestTypes.PUT => "PUT", 
			HttpRequestTypes.TRACE => "TRACE", 
			_ => string.Empty, 
		};
	}

	public static HttpRequestTypes StringToHttpRequestType(string s)
	{
		return s switch
		{
			"DELETE" => HttpRequestTypes.DELETE, 
			"GET" => HttpRequestTypes.GET, 
			"HEAD" => HttpRequestTypes.HEAD, 
			"OPTIONS" => HttpRequestTypes.OPTIONS, 
			"POST" => HttpRequestTypes.POST, 
			"PUT" => HttpRequestTypes.PUT, 
			"TRACE" => HttpRequestTypes.TRACE, 
			_ => HttpRequestTypes.GET, 
		};
	}

	public static string WebServiceAuthenticationTypeToString(WebServiceAuthenticationTypes e)
	{
		return e switch
		{
			WebServiceAuthenticationTypes.BasicUserPassword => "BasicUserPassword", 
			WebServiceAuthenticationTypes.BasicApiKey => "BasicApiKey", 
			WebServiceAuthenticationTypes.OAuth2 => "OAuth2", 
			_ => "None", 
		};
	}

	public static WebServiceAuthenticationTypes StringToWebServiceAuthenticationType(string s)
	{
		return s switch
		{
			"BasicUserPassword" => WebServiceAuthenticationTypes.BasicUserPassword, 
			"BasicApiKey" => WebServiceAuthenticationTypes.BasicApiKey, 
			"OAuth2" => WebServiceAuthenticationTypes.OAuth2, 
			_ => WebServiceAuthenticationTypes.None, 
		};
	}

	public static string TextToSpeechVoiceTypeToString(TextToSpeechVoiceTypes e)
	{
		return e switch
		{
			TextToSpeechVoiceTypes.Neural => "TextToSpeechAudioPrompt.TextToSpeechVoiceTypes.Neural", 
			TextToSpeechVoiceTypes.Wavenet => "TextToSpeechAudioPrompt.TextToSpeechVoiceTypes.Wavenet", 
			_ => "TextToSpeechAudioPrompt.TextToSpeechVoiceTypes.Standard", 
		};
	}

	public static TextToSpeechVoiceTypes StringToTextToSpeechVoiceType(string s)
	{
		if (!(s == "Neural"))
		{
			if (s == "Wavenet")
			{
				return TextToSpeechVoiceTypes.Wavenet;
			}
			return TextToSpeechVoiceTypes.Standard;
		}
		return TextToSpeechVoiceTypes.Neural;
	}

	public static string TextToSpeechFormatToString(TextToSpeechFormats e)
	{
		return e switch
		{
			TextToSpeechFormats.Text => "Text", 
			TextToSpeechFormats.SSML => "SSML", 
			_ => "", 
		};
	}

	public static TextToSpeechFormats StringToTextToSpeechFormat(string s)
	{
		if (!(s == "Text"))
		{
			if (s == "SSML")
			{
				return TextToSpeechFormats.SSML;
			}
			return TextToSpeechFormats.Text;
		}
		return TextToSpeechFormats.Text;
	}

	public static string TextToSpeechAmazonRegionToString(TextToSpeechAmazonRegions e)
	{
		return e switch
		{
			TextToSpeechAmazonRegions.AfricaCapeTown => "af-south-1", 
			TextToSpeechAmazonRegions.AsiaPacificHongKong => "ap-east-1", 
			TextToSpeechAmazonRegions.AsiaPacificTokyo => "ap-northeast-1", 
			TextToSpeechAmazonRegions.AsiaPacificSeoul => "ap-northeast-2", 
			TextToSpeechAmazonRegions.AsiaPacificOsaka => "ap-northeast-3", 
			TextToSpeechAmazonRegions.AsiaPacificMumbai => "ap-south-1", 
			TextToSpeechAmazonRegions.AsiaPacificSingapore => "ap-southeast-1", 
			TextToSpeechAmazonRegions.AsiaPacificSydney => "ap-southeast-2", 
			TextToSpeechAmazonRegions.CanadaCentral => "ca-central-1", 
			TextToSpeechAmazonRegions.ChinaBeijing => "cn-north-1", 
			TextToSpeechAmazonRegions.ChinaNingxia => "cn-northwest-1", 
			TextToSpeechAmazonRegions.EUFrankfurt => "eu-central-1", 
			TextToSpeechAmazonRegions.EUStockholm => "eu-north-1", 
			TextToSpeechAmazonRegions.EUIreland => "eu-west-1", 
			TextToSpeechAmazonRegions.EULondon => "eu-west-2", 
			TextToSpeechAmazonRegions.EUMilan => "eu-south-1", 
			TextToSpeechAmazonRegions.EUParis => "eu-west-3", 
			TextToSpeechAmazonRegions.MiddleEastBahrain => "me-south-1", 
			TextToSpeechAmazonRegions.SouthAmericaSaoPaulo => "sa-east-1", 
			TextToSpeechAmazonRegions.USEastVirginia => "us-east-1", 
			TextToSpeechAmazonRegions.USEastOhio => "us-east-2", 
			TextToSpeechAmazonRegions.USEastGovCloudVirginia => "us-gov-east-1", 
			TextToSpeechAmazonRegions.USWestGovCloudOregon => "us-gov-west-1", 
			TextToSpeechAmazonRegions.USWestCalifornia => "us-west-1", 
			TextToSpeechAmazonRegions.USWestOregon => "us-west-2", 
			_ => "us-east-2", 
		};
	}

	public static TextToSpeechAmazonRegions StringToTextToSpeechAmazonRegion(string s)
	{
		return s switch
		{
			"af-south-1" => TextToSpeechAmazonRegions.AfricaCapeTown, 
			"ap-east-1" => TextToSpeechAmazonRegions.AsiaPacificHongKong, 
			"ap-northeast-1" => TextToSpeechAmazonRegions.AsiaPacificTokyo, 
			"ap-northeast-2" => TextToSpeechAmazonRegions.AsiaPacificSeoul, 
			"ap-northeast-3" => TextToSpeechAmazonRegions.AsiaPacificOsaka, 
			"ap-south-1" => TextToSpeechAmazonRegions.AsiaPacificMumbai, 
			"ap-southeast-1" => TextToSpeechAmazonRegions.AsiaPacificSingapore, 
			"ap-southeast-2" => TextToSpeechAmazonRegions.AsiaPacificSydney, 
			"ca-central-1" => TextToSpeechAmazonRegions.CanadaCentral, 
			"cn-north-1" => TextToSpeechAmazonRegions.ChinaBeijing, 
			"cn-northwest-1" => TextToSpeechAmazonRegions.ChinaNingxia, 
			"eu-central-1" => TextToSpeechAmazonRegions.EUFrankfurt, 
			"eu-north-1" => TextToSpeechAmazonRegions.EUStockholm, 
			"eu-west-1" => TextToSpeechAmazonRegions.EUIreland, 
			"eu-west-2" => TextToSpeechAmazonRegions.EULondon, 
			"eu-south-1" => TextToSpeechAmazonRegions.EUMilan, 
			"eu-west-3" => TextToSpeechAmazonRegions.EUParis, 
			"me-south-1" => TextToSpeechAmazonRegions.MiddleEastBahrain, 
			"sa-east-1" => TextToSpeechAmazonRegions.SouthAmericaSaoPaulo, 
			"us-east-1" => TextToSpeechAmazonRegions.USEastVirginia, 
			"us-east-2" => TextToSpeechAmazonRegions.USEastOhio, 
			"us-gov-east-1" => TextToSpeechAmazonRegions.USEastGovCloudVirginia, 
			"us-gov-west-1" => TextToSpeechAmazonRegions.USWestGovCloudOregon, 
			"us-west-1" => TextToSpeechAmazonRegions.USWestCalifornia, 
			"us-west-2" => TextToSpeechAmazonRegions.USWestOregon, 
			_ => TextToSpeechAmazonRegions.USEastOhio, 
		};
	}

	public static string TextToSpeechEnginesToString(TextToSpeechEngines e)
	{
		return e switch
		{
			TextToSpeechEngines.AmazonPolly => "AmazonPolly", 
			TextToSpeechEngines.GoogleCloud => "GoogleCloud", 
			_ => "None", 
		};
	}

	public static TextToSpeechEngines StringToTextToSpeechEngines(string s)
	{
		if (!(s == "AmazonPolly"))
		{
			if (s == "GoogleCloud")
			{
				return TextToSpeechEngines.GoogleCloud;
			}
			return TextToSpeechEngines.None;
		}
		return TextToSpeechEngines.AmazonPolly;
	}

	public static string SpeechToTextEnginesToString(SpeechToTextEngines e)
	{
		if (e == SpeechToTextEngines.GoogleCloud)
		{
			return "GoogleCloud";
		}
		return "None";
	}

	public static SpeechToTextEngines StringToSpeechToTextEngines(string s)
	{
		if (s == "GoogleCloud")
		{
			return SpeechToTextEngines.GoogleCloud;
		}
		return SpeechToTextEngines.None;
	}

	public static string LogLevelToString(LogLevels e)
	{
		return e switch
		{
			LogLevels.Critical => "LoggerComponent.LogLevels.Critical", 
			LogLevels.Error => "LoggerComponent.LogLevels.Error", 
			LogLevels.Warn => "LoggerComponent.LogLevels.Warn", 
			LogLevels.Info => "LoggerComponent.LogLevels.Info", 
			LogLevels.Debug => "LoggerComponent.LogLevels.Debug", 
			LogLevels.Trace => "LoggerComponent.LogLevels.Trace", 
			_ => "", 
		};
	}

	public static string ScriptParameterTypeToCSharpString(ScriptParameterTypes e)
	{
		return e switch
		{
			ScriptParameterTypes.Boolean => "bool", 
			ScriptParameterTypes.Byte => "byte", 
			ScriptParameterTypes.Char => "char", 
			ScriptParameterTypes.Decimal => "decimal", 
			ScriptParameterTypes.Double => "double", 
			ScriptParameterTypes.Int16 => "short", 
			ScriptParameterTypes.Int32 => "int", 
			ScriptParameterTypes.Int64 => "long", 
			ScriptParameterTypes.Object => "object", 
			ScriptParameterTypes.SByte => "sbyte", 
			ScriptParameterTypes.Single => "float", 
			ScriptParameterTypes.String => "string", 
			ScriptParameterTypes.UInt16 => "ushort", 
			ScriptParameterTypes.UInt32 => "uint", 
			ScriptParameterTypes.UInt64 => "ulong", 
			_ => throw new ArgumentException("Invalid ScriptParameterTypes: " + e), 
		};
	}

	public static string ExtensionStatusToProfileName(ExtensionStatus e)
	{
		return e switch
		{
			ExtensionStatus.Available => "Available", 
			ExtensionStatus.Away => "Away", 
			ExtensionStatus.DoNotDisturb => "Out of office", 
			ExtensionStatus.Lunch => "Custom 1", 
			ExtensionStatus.BusinessTrip => "Custom 2", 
			_ => throw new ArgumentException("Invalid ExtensionStatus: " + e), 
		};
	}

	public static string SurveyAnswerToString(SurveyAnswers e)
	{
		return e switch
		{
			SurveyAnswers.Option0 => "0", 
			SurveyAnswers.Option1 => "1", 
			SurveyAnswers.Option2 => "2", 
			SurveyAnswers.Option3 => "3", 
			SurveyAnswers.Option4 => "4", 
			SurveyAnswers.Option5 => "5", 
			SurveyAnswers.Option6 => "6", 
			SurveyAnswers.Option7 => "7", 
			SurveyAnswers.Option8 => "8", 
			SurveyAnswers.Option9 => "9", 
			_ => throw new ArgumentException("Invalid SurveyAnswers: " + e), 
		};
	}

	public static string OfficeTimeStatusToCompilerString(OfficeTimeStatus e)
	{
		return e switch
		{
			OfficeTimeStatus.ForceInOffice => "TcxSetOfficeTimeStatusComponent.OfficeTimeStatus.ForceInOffice", 
			OfficeTimeStatus.ForceOutOfOffice => "TcxSetOfficeTimeStatusComponent.OfficeTimeStatus.ForceOutOfOffice", 
			OfficeTimeStatus.UseDefault => "TcxSetOfficeTimeStatusComponent.OfficeTimeStatus.UseDefault", 
			_ => throw new ArgumentException("Invalid OfficeTimeStatus: " + e), 
		};
	}

	public static string CallDirectionToCompilerString(CallDirections e)
	{
		return e switch
		{
			CallDirections.Inbound => "TcxGetLastCallComponent.CallDirections.Inbound", 
			CallDirections.Outbound => "TcxGetLastCallComponent.CallDirections.Outbound", 
			CallDirections.Both => "TcxGetLastCallComponent.CallDirections.Both", 
			_ => throw new ArgumentException("Invalid CallDirections: " + e), 
		};
	}
}
