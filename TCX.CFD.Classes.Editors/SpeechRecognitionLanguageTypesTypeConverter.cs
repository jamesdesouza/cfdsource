using System.Collections.Generic;
using System.ComponentModel;

namespace TCX.CFD.Classes.Editors;

public class SpeechRecognitionLanguageTypesTypeConverter : StringConverter
{
	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return false;
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		return new StandardValuesCollection(new List<string>
		{
			"af-ZA", "am-ET", "ar-AE", "ar-BH", "ar-DZ", "ar-EG", "ar-IL", "ar-IQ", "ar-JO", "ar-KW",
			"ar-LB", "ar-MA", "ar-OM", "ar-PS", "ar-QA", "ar-SA", "ar-TN", "ar-YE", "az-AZ", "bg-BG",
			"bn-BD", "bn-IN", "bs-BA", "ca-ES", "cs-CZ", "da-DK", "de-AT", "de-CH", "de-DE", "el-GR",
			"en-AU", "en-CA", "en-GB", "en-GH", "en-HK", "en-IE", "en-IN", "en-KE", "en-NG", "en-NZ",
			"en-PH", "en-PK", "en-SG", "en-TZ", "en-US", "en-ZA", "es-AR", "es-BO", "es-CL", "es-CO",
			"es-CR", "es-DO", "es-EC", "es-ES", "es-GT", "es-HN", "es-MX", "es-NI", "es-PA", "es-PE",
			"es-PR", "es-PY", "es-SV", "es-US", "es-UY", "es-VE", "et-EE", "eu-ES", "fa-IR", "fi-FI",
			"fil-PH", "fr-BE", "fr-CA", "fr-CH", "fr-FR", "gl-ES", "gu-IN", "hi-IN", "hr-HR", "hu-HU",
			"hy-AM", "id-ID", "is-IS", "it-CH", "it-IT", "iw-IL", "ja-JP", "jv-ID", "ka-GE", "kk-KZ",
			"km-KH", "kn-IN", "ko-KR", "lo-LA", "lt-LT", "lv-LV", "mk-MK", "ml-IN", "mn-MN", "mr-IN",
			"ms-MY", "my-MM", "ne-NP", "nl-BE", "nl-NL", "no-NO", "pa-Guru-IN", "pl-PL", "pt-BR", "pt-PT",
			"ro-RO", "ru-RU", "si-LK", "sk-SK", "sl-SI", "sq-AL", "sr-RS", "su-ID", "sv-SE", "sw-KE",
			"sw-TZ", "ta-IN", "ta-LK", "ta-MY", "ta-SG", "te-IN", "th-TH", "tr-TR", "uk-UA", "ur-IN",
			"ur-PK", "uz-UZ", "vi-VN", "yue-Hant-HK", "zh", "zh-TW", "zu-ZA"
		});
	}
}
