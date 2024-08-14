using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TCX.CFD.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DefaultProjectsFolder
	{
		get
		{
			return (string)this["DefaultProjectsFolder"];
		}
		set
		{
			this["DefaultProjectsFolder"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool MenuTemplateAcceptDtmfInput
	{
		get
		{
			return (bool)this["MenuTemplateAcceptDtmfInput"];
		}
		set
		{
			this["MenuTemplateAcceptDtmfInput"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint MenuTemplateMaxRetryCount
	{
		get
		{
			return (uint)this["MenuTemplateMaxRetryCount"];
		}
		set
		{
			this["MenuTemplateMaxRetryCount"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("5")]
	public uint MenuTemplateTimeout
	{
		get
		{
			return (uint)this["MenuTemplateTimeout"];
		}
		set
		{
			this["MenuTemplateTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("60")]
	public uint RecordTemplateMaxTime
	{
		get
		{
			return (uint)this["RecordTemplateMaxTime"];
		}
		set
		{
			this["RecordTemplateMaxTime"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool RecordTemplateSaveToFile
	{
		get
		{
			return (bool)this["RecordTemplateSaveToFile"];
		}
		set
		{
			this["RecordTemplateSaveToFile"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool RecordTemplateTerminateByDtmf
	{
		get
		{
			return (bool)this["RecordTemplateTerminateByDtmf"];
		}
		set
		{
			this["RecordTemplateTerminateByDtmf"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateAcceptDtmfInput
	{
		get
		{
			return (bool)this["UserInputTemplateAcceptDtmfInput"];
		}
		set
		{
			this["UserInputTemplateAcceptDtmfInput"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint UserInputTemplateMaxRetryCount
	{
		get
		{
			return (uint)this["UserInputTemplateMaxRetryCount"];
		}
		set
		{
			this["UserInputTemplateMaxRetryCount"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint UserInputTemplateMinDigits
	{
		get
		{
			return (uint)this["UserInputTemplateMinDigits"];
		}
		set
		{
			this["UserInputTemplateMinDigits"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("6")]
	public uint UserInputTemplateMaxDigits
	{
		get
		{
			return (uint)this["UserInputTemplateMaxDigits"];
		}
		set
		{
			this["UserInputTemplateMaxDigits"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("5")]
	public uint UserInputTemplateFirstDigitTimeout
	{
		get
		{
			return (uint)this["UserInputTemplateFirstDigitTimeout"];
		}
		set
		{
			this["UserInputTemplateFirstDigitTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint UserInputTemplateInterDigitTimeout
	{
		get
		{
			return (uint)this["UserInputTemplateInterDigitTimeout"];
		}
		set
		{
			this["UserInputTemplateInterDigitTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("2")]
	public uint UserInputTemplateFinalDigitTimeout
	{
		get
		{
			return (uint)this["UserInputTemplateFinalDigitTimeout"];
		}
		set
		{
			this["UserInputTemplateFinalDigitTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("#")]
	public string UserInputTemplateStopDigit
	{
		get
		{
			return (string)this["UserInputTemplateStopDigit"];
		}
		set
		{
			this["UserInputTemplateStopDigit"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("GET")]
	public string WebInteractionTemplateHttpRequestType
	{
		get
		{
			return (string)this["WebInteractionTemplateHttpRequestType"];
		}
		set
		{
			this["WebInteractionTemplateHttpRequestType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("30")]
	public uint WebInteractionTemplateTimeout
	{
		get
		{
			return (uint)this["WebInteractionTemplateTimeout"];
		}
		set
		{
			this["WebInteractionTemplateTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("100")]
	public uint MaxErrorsBuildingProject
	{
		get
		{
			return (uint)this["MaxErrorsBuildingProject"];
		}
		set
		{
			this["MaxErrorsBuildingProject"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool RecordTemplateBeep
	{
		get
		{
			return (bool)this["RecordTemplateBeep"];
		}
		set
		{
			this["RecordTemplateBeep"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption0
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption0"];
		}
		set
		{
			this["MenuTemplateIsValidOption0"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool MenuTemplateIsValidOption1
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption1"];
		}
		set
		{
			this["MenuTemplateIsValidOption1"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool MenuTemplateIsValidOption2
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption2"];
		}
		set
		{
			this["MenuTemplateIsValidOption2"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption3
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption3"];
		}
		set
		{
			this["MenuTemplateIsValidOption3"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOptionStar
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOptionStar"];
		}
		set
		{
			this["MenuTemplateIsValidOptionStar"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOptionPound
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOptionPound"];
		}
		set
		{
			this["MenuTemplateIsValidOptionPound"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption8
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption8"];
		}
		set
		{
			this["MenuTemplateIsValidOption8"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption7
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption7"];
		}
		set
		{
			this["MenuTemplateIsValidOption7"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption6
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption6"];
		}
		set
		{
			this["MenuTemplateIsValidOption6"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption9
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption9"];
		}
		set
		{
			this["MenuTemplateIsValidOption9"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption5
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption5"];
		}
		set
		{
			this["MenuTemplateIsValidOption5"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool MenuTemplateIsValidOption4
	{
		get
		{
			return (bool)this["MenuTemplateIsValidOption4"];
		}
		set
		{
			this["MenuTemplateIsValidOption4"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption0
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption0"];
		}
		set
		{
			this["UserInputTemplateIsValidOption0"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption1
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption1"];
		}
		set
		{
			this["UserInputTemplateIsValidOption1"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption2
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption2"];
		}
		set
		{
			this["UserInputTemplateIsValidOption2"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption3
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption3"];
		}
		set
		{
			this["UserInputTemplateIsValidOption3"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption4
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption4"];
		}
		set
		{
			this["UserInputTemplateIsValidOption4"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption5
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption5"];
		}
		set
		{
			this["UserInputTemplateIsValidOption5"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption6
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption6"];
		}
		set
		{
			this["UserInputTemplateIsValidOption6"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption7
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption7"];
		}
		set
		{
			this["UserInputTemplateIsValidOption7"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption8
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption8"];
		}
		set
		{
			this["UserInputTemplateIsValidOption8"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool UserInputTemplateIsValidOption9
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOption9"];
		}
		set
		{
			this["UserInputTemplateIsValidOption9"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool UserInputTemplateIsValidOptionPound
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOptionPound"];
		}
		set
		{
			this["UserInputTemplateIsValidOptionPound"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool UserInputTemplateIsValidOptionStar
	{
		get
		{
			return (bool)this["UserInputTemplateIsValidOptionStar"];
		}
		set
		{
			this["UserInputTemplateIsValidOptionStar"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3DES")]
	public string CryptographyTemplateAlgorithm
	{
		get
		{
			return (string)this["CryptographyTemplateAlgorithm"];
		}
		set
		{
			this["CryptographyTemplateAlgorithm"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Hexadecimal")]
	public string CryptographyTemplateFormat
	{
		get
		{
			return (string)this["CryptographyTemplateFormat"];
		}
		set
		{
			this["CryptographyTemplateFormat"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string CryptographyTemplateKey
	{
		get
		{
			return (string)this["CryptographyTemplateKey"];
		}
		set
		{
			this["CryptographyTemplateKey"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Append")]
	public string FileManagementTemplateOpenMode
	{
		get
		{
			return (string)this["FileManagementTemplateOpenMode"];
		}
		set
		{
			this["FileManagementTemplateOpenMode"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Write")]
	public string FileManagementTemplateAction
	{
		get
		{
			return (string)this["FileManagementTemplateAction"];
		}
		set
		{
			this["FileManagementTemplateAction"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("TCP")]
	public string SocketClientTemplateConnectionType
	{
		get
		{
			return (string)this["SocketClientTemplateConnectionType"];
		}
		set
		{
			this["SocketClientTemplateConnectionType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("localhost")]
	public string SocketClientTemplateHost
	{
		get
		{
			return (string)this["SocketClientTemplateHost"];
		}
		set
		{
			this["SocketClientTemplateHost"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool SocketClientTemplateWaitForResponse
	{
		get
		{
			return (bool)this["SocketClientTemplateWaitForResponse"];
		}
		set
		{
			this["SocketClientTemplateWaitForResponse"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateServer
	{
		get
		{
			return (string)this["EMailSenderTemplateServer"];
		}
		set
		{
			this["EMailSenderTemplateServer"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateUserName
	{
		get
		{
			return (string)this["EMailSenderTemplateUserName"];
		}
		set
		{
			this["EMailSenderTemplateUserName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplatePassword
	{
		get
		{
			return (string)this["EMailSenderTemplatePassword"];
		}
		set
		{
			this["EMailSenderTemplatePassword"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateFrom
	{
		get
		{
			return (string)this["EMailSenderTemplateFrom"];
		}
		set
		{
			this["EMailSenderTemplateFrom"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateTo
	{
		get
		{
			return (string)this["EMailSenderTemplateTo"];
		}
		set
		{
			this["EMailSenderTemplateTo"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateCC
	{
		get
		{
			return (string)this["EMailSenderTemplateCC"];
		}
		set
		{
			this["EMailSenderTemplateCC"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateBCC
	{
		get
		{
			return (string)this["EMailSenderTemplateBCC"];
		}
		set
		{
			this["EMailSenderTemplateBCC"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateSubject
	{
		get
		{
			return (string)this["EMailSenderTemplateSubject"];
		}
		set
		{
			this["EMailSenderTemplateSubject"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string EMailSenderTemplateBody
	{
		get
		{
			return (string)this["EMailSenderTemplateBody"];
		}
		set
		{
			this["EMailSenderTemplateBody"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Normal")]
	public string EMailSenderTemplatePriority
	{
		get
		{
			return (string)this["EMailSenderTemplatePriority"];
		}
		set
		{
			this["EMailSenderTemplatePriority"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("30")]
	public uint WebServicesInteractionTemplateTimeout
	{
		get
		{
			return (uint)this["WebServicesInteractionTemplateTimeout"];
		}
		set
		{
			this["WebServicesInteractionTemplateTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DatabaseAccessTemplateServer
	{
		get
		{
			return (string)this["DatabaseAccessTemplateServer"];
		}
		set
		{
			this["DatabaseAccessTemplateServer"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DatabaseAccessTemplateDatabase
	{
		get
		{
			return (string)this["DatabaseAccessTemplateDatabase"];
		}
		set
		{
			this["DatabaseAccessTemplateDatabase"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DatabaseAccessTemplateUserName
	{
		get
		{
			return (string)this["DatabaseAccessTemplateUserName"];
		}
		set
		{
			this["DatabaseAccessTemplateUserName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DatabaseAccessTemplatePassword
	{
		get
		{
			return (string)this["DatabaseAccessTemplatePassword"];
		}
		set
		{
			this["DatabaseAccessTemplatePassword"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("SqlServer")]
	public string DatabaseAccessTemplateDatabaseType
	{
		get
		{
			return (string)this["DatabaseAccessTemplateDatabaseType"];
		}
		set
		{
			this["DatabaseAccessTemplateDatabaseType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Query")]
	public string DatabaseAccessTemplateStatementType
	{
		get
		{
			return (string)this["DatabaseAccessTemplateStatementType"];
		}
		set
		{
			this["DatabaseAccessTemplateStatementType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("30")]
	public uint DatabaseAccessTemplateTimeout
	{
		get
		{
			return (uint)this["DatabaseAccessTemplateTimeout"];
		}
		set
		{
			this["DatabaseAccessTemplateTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool PromptPlaybackTemplateAcceptDtmfInput
	{
		get
		{
			return (bool)this["PromptPlaybackTemplateAcceptDtmfInput"];
		}
		set
		{
			this["PromptPlaybackTemplateAcceptDtmfInput"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool HasToUpgradeSettings
	{
		get
		{
			return (bool)this["HasToUpgradeSettings"];
		}
		set
		{
			this["HasToUpgradeSettings"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("1234")]
	public uint SocketClientTemplatePort
	{
		get
		{
			return (uint)this["SocketClientTemplatePort"];
		}
		set
		{
			this["SocketClientTemplatePort"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool EMailSenderTemplateIgnoreMissingAttachments
	{
		get
		{
			return (bool)this["EMailSenderTemplateIgnoreMissingAttachments"];
		}
		set
		{
			this["EMailSenderTemplateIgnoreMissingAttachments"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DefaultTtsVoiceName
	{
		get
		{
			return (string)this["DefaultTtsVoiceName"];
		}
		set
		{
			this["DefaultTtsVoiceName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Text")]
	public string DefaultTtsFormat
	{
		get
		{
			return (string)this["DefaultTtsFormat"];
		}
		set
		{
			this["DefaultTtsFormat"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("25")]
	public int EMailSenderTemplateServerPort
	{
		get
		{
			return (int)this["EMailSenderTemplateServerPort"];
		}
		set
		{
			this["EMailSenderTemplateServerPort"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool EMailSenderTemplateServerEnableSSL
	{
		get
		{
			return (bool)this["EMailSenderTemplateServerEnableSSL"];
		}
		set
		{
			this["EMailSenderTemplateServerEnableSSL"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool EMailSenderTemplateIsBodyHtml
	{
		get
		{
			return (bool)this["EMailSenderTemplateIsBodyHtml"];
		}
		set
		{
			this["EMailSenderTemplateIsBodyHtml"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("-1")]
	public int DatabaseAccessTemplatePort
	{
		get
		{
			return (int)this["DatabaseAccessTemplatePort"];
		}
		set
		{
			this["DatabaseAccessTemplatePort"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string LastOpenExistingFileFolder
	{
		get
		{
			return (string)this["LastOpenExistingFileFolder"];
		}
		set
		{
			this["LastOpenExistingFileFolder"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string LastOpenExistingProjectFolder
	{
		get
		{
			return (string)this["LastOpenExistingProjectFolder"];
		}
		set
		{
			this["LastOpenExistingProjectFolder"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool DatabaseAccessTemplateUseConnectionString
	{
		get
		{
			return (bool)this["DatabaseAccessTemplateUseConnectionString"];
		}
		set
		{
			this["DatabaseAccessTemplateUseConnectionString"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string DatabaseAccessTemplateConnectionString
	{
		get
		{
			return (string)this["DatabaseAccessTemplateConnectionString"];
		}
		set
		{
			this["DatabaseAccessTemplateConnectionString"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool EMailSenderTemplateUseServerSettings
	{
		get
		{
			return (bool)this["EMailSenderTemplateUseServerSettings"];
		}
		set
		{
			this["EMailSenderTemplateUseServerSettings"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool AuthenticationTemplateIsPinRequired
	{
		get
		{
			return (bool)this["AuthenticationTemplateIsPinRequired"];
		}
		set
		{
			this["AuthenticationTemplateIsPinRequired"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint AuthenticationTemplateMaxRetryCount
	{
		get
		{
			return (uint)this["AuthenticationTemplateMaxRetryCount"];
		}
		set
		{
			this["AuthenticationTemplateMaxRetryCount"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("POST")]
	public string WebServiceRestTemplateHttpRequestType
	{
		get
		{
			return (string)this["WebServiceRestTemplateHttpRequestType"];
		}
		set
		{
			this["WebServiceRestTemplateHttpRequestType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("30")]
	public uint WebServiceRestTemplateTimeout
	{
		get
		{
			return (uint)this["WebServiceRestTemplateTimeout"];
		}
		set
		{
			this["WebServiceRestTemplateTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("None")]
	public string WebServiceRestTemplateAuthenticationType
	{
		get
		{
			return (string)this["WebServiceRestTemplateAuthenticationType"];
		}
		set
		{
			this["WebServiceRestTemplateAuthenticationType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool CreditCardTemplateIsExpirationRequired
	{
		get
		{
			return (bool)this["CreditCardTemplateIsExpirationRequired"];
		}
		set
		{
			this["CreditCardTemplateIsExpirationRequired"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool CreditCardTemplateIsSecurityCodeRequired
	{
		get
		{
			return (bool)this["CreditCardTemplateIsSecurityCodeRequired"];
		}
		set
		{
			this["CreditCardTemplateIsSecurityCodeRequired"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint CreditCardTemplateMaxRetryCount
	{
		get
		{
			return (uint)this["CreditCardTemplateMaxRetryCount"];
		}
		set
		{
			this["CreditCardTemplateMaxRetryCount"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool AutoUpdatesEnabled
	{
		get
		{
			return (bool)this["AutoUpdatesEnabled"];
		}
		set
		{
			this["AutoUpdatesEnabled"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("8")]
	public uint MaxRecentProjects
	{
		get
		{
			return (uint)this["MaxRecentProjects"];
		}
		set
		{
			this["MaxRecentProjects"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string RecentProjects
	{
		get
		{
			return (string)this["RecentProjects"];
		}
		set
		{
			this["RecentProjects"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string AmazonPollyClientID
	{
		get
		{
			return (string)this["AmazonPollyClientID"];
		}
		set
		{
			this["AmazonPollyClientID"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string AmazonPollyClientSecret
	{
		get
		{
			return (string)this["AmazonPollyClientSecret"];
		}
		set
		{
			this["AmazonPollyClientSecret"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("us-east-2")]
	public string AmazonRegion
	{
		get
		{
			return (string)this["AmazonRegion"];
		}
		set
		{
			this["AmazonRegion"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool SurveyTemplateAcceptDtmfInput
	{
		get
		{
			return (bool)this["SurveyTemplateAcceptDtmfInput"];
		}
		set
		{
			this["SurveyTemplateAcceptDtmfInput"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("5")]
	public uint SurveyTemplateTimeout
	{
		get
		{
			return (uint)this["SurveyTemplateTimeout"];
		}
		set
		{
			this["SurveyTemplateTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint SurveyTemplateMaxRetryCount
	{
		get
		{
			return (uint)this["SurveyTemplateMaxRetryCount"];
		}
		set
		{
			this["SurveyTemplateMaxRetryCount"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint VoiceInputTemplateInputTimeout
	{
		get
		{
			return (uint)this["VoiceInputTemplateInputTimeout"];
		}
		set
		{
			this["VoiceInputTemplateInputTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint VoiceInputTemplateMaxRetryCount
	{
		get
		{
			return (uint)this["VoiceInputTemplateMaxRetryCount"];
		}
		set
		{
			this["VoiceInputTemplateMaxRetryCount"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("en-US")]
	public string VoiceInputTemplateLanguageCode
	{
		get
		{
			return (string)this["VoiceInputTemplateLanguageCode"];
		}
		set
		{
			this["VoiceInputTemplateLanguageCode"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	public DateTime InstallDateV18Update2
	{
		get
		{
			return (DateTime)this["InstallDateV18Update2"];
		}
		set
		{
			this["InstallDateV18Update2"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("None")]
	public string OnlineServicesTextToSpeechEngine
	{
		get
		{
			return (string)this["OnlineServicesTextToSpeechEngine"];
		}
		set
		{
			this["OnlineServicesTextToSpeechEngine"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string GoogleCloudServiceAccountKeyJSON
	{
		get
		{
			return (string)this["GoogleCloudServiceAccountKeyJSON"];
		}
		set
		{
			this["GoogleCloudServiceAccountKeyJSON"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("None")]
	public string OnlineServicesSpeechToTextEngine
	{
		get
		{
			return (string)this["OnlineServicesSpeechToTextEngine"];
		}
		set
		{
			this["OnlineServicesSpeechToTextEngine"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string GoogleCloudServiceAccountKeyFileName
	{
		get
		{
			return (string)this["GoogleCloudServiceAccountKeyFileName"];
		}
		set
		{
			this["GoogleCloudServiceAccountKeyFileName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Standard")]
	public string DefaultTtsVoiceType
	{
		get
		{
			return (string)this["DefaultTtsVoiceType"];
		}
		set
		{
			this["DefaultTtsVoiceType"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string AmazonPollyLexicons
	{
		get
		{
			return (string)this["AmazonPollyLexicons"];
		}
		set
		{
			this["AmazonPollyLexicons"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string StartPageFeed
	{
		get
		{
			return (string)this["StartPageFeed"];
		}
		set
		{
			this["StartPageFeed"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool VoiceInputTemplateAcceptDtmfInput
	{
		get
		{
			return (bool)this["VoiceInputTemplateAcceptDtmfInput"];
		}
		set
		{
			this["VoiceInputTemplateAcceptDtmfInput"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("3")]
	public uint VoiceInputTemplateDtmfTimeout
	{
		get
		{
			return (uint)this["VoiceInputTemplateDtmfTimeout"];
		}
		set
		{
			this["VoiceInputTemplateDtmfTimeout"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("1")]
	public uint VoiceInputTemplateMinDigits
	{
		get
		{
			return (uint)this["VoiceInputTemplateMinDigits"];
		}
		set
		{
			this["VoiceInputTemplateMinDigits"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("1")]
	public uint VoiceInputTemplateMaxDigits
	{
		get
		{
			return (uint)this["VoiceInputTemplateMaxDigits"];
		}
		set
		{
			this["VoiceInputTemplateMaxDigits"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("#")]
	public string VoiceInputTemplateStopDigit
	{
		get
		{
			return (string)this["VoiceInputTemplateStopDigit"];
		}
		set
		{
			this["VoiceInputTemplateStopDigit"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption0
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption0"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption0"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption1
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption1"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption1"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption2
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption2"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption2"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption3
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption3"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption3"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption4
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption4"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption4"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption5
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption5"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption5"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption6
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption6"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption6"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption7
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption7"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption7"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption8
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption8"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption8"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool VoiceInputTemplateIsValidOption9
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOption9"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOption9"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool VoiceInputTemplateIsValidOptionStar
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOptionStar"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOptionStar"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool VoiceInputTemplateIsValidOptionPound
	{
		get
		{
			return (bool)this["VoiceInputTemplateIsValidOptionPound"];
		}
		set
		{
			this["VoiceInputTemplateIsValidOptionPound"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool VoiceInputTemplateSaveToFile
	{
		get
		{
			return (bool)this["VoiceInputTemplateSaveToFile"];
		}
		set
		{
			this["VoiceInputTemplateSaveToFile"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("en-US")]
	public string TranscribeAudioTemplateLanguageCode
	{
		get
		{
			return (string)this["TranscribeAudioTemplateLanguageCode"];
		}
		set
		{
			this["TranscribeAudioTemplateLanguageCode"] = value;
		}
	}

	private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
	{
	}

	private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
	{
	}
}
