using System.Windows.Forms;

namespace TCX.CFD.Classes;

public static class TextToSpeechHelper
{
	public static void FillVoicesCombo(ComboBox comboVoices, string selectedVoice, TextToSpeechVoiceTypes selectedVoiceType, TextToSpeechEngines engine)
	{
		comboVoices.Items.Clear();
		object[] items;
		if (engine == TextToSpeechEngines.GoogleCloud)
		{
			ComboBox.ObjectCollection ıtems = comboVoices.Items;
			items = new string[306]
			{
				"af-ZA-Standard-A (Afrikaans, Female)", "ar-XA-Standard-A (Arabic, Female)", "ar-XA-Standard-B (Arabic, Male)", "ar-XA-Standard-C (Arabic, Male)", "ar-XA-Standard-D (Arabic, Female)", "ar-XA-Wavenet-A (Arabic, Female)", "ar-XA-Wavenet-B (Arabic, Male)", "ar-XA-Wavenet-C (Arabic, Male)", "ar-XA-Wavenet-D (Arabic, Female)", "bg-bg-Standard-A (Bulgarian - Bulgaria, Female)",
				"bn-IN-Standard-A (Bengali - India, Female)", "bn-IN-Standard-B (Bengali - India, Male)", "bn-IN-Wavenet-A (Bengali - India, Female)", "bn-IN-Wavenet-B (Bengali - India, Male)", "ca-es-Standard-A (Catalan - Spain, Female)", "cs-CZ-Standard-A (Czech - Czech Republic, Female)", "cs-CZ-Wavenet-A (Czech - Czech Republic, Female)", "da-DK-Standard-A (Danish - Denmark, Female)", "da-DK-Standard-C (Danish - Denmark, Male)", "da-DK-Standard-D (Danish - Denmark, Female)",
				"da-DK-Standard-E (Danish - Denmark, Female)", "da-DK-Wavenet-A (Danish - Denmark, Female)", "da-DK-Wavenet-C (Danish - Denmark, Male)", "da-DK-Wavenet-D (Danish - Denmark, Female)", "da-DK-Wavenet-E (Danish - Denmark, Female)", "nl-BE-Standard-A (Dutch - Belgium, Female)", "nl-BE-Standard-B (Dutch - Belgium, Male)", "nl-BE-Wavenet-A (Dutch - Belgium, Female)", "nl-BE-Wavenet-B (Dutch - Belgium, Male)", "nl-NL-Standard-A (Dutch - Netherlands, Female)",
				"nl-NL-Standard-B (Dutch - Netherlands, Male)", "nl-NL-Standard-C (Dutch - Netherlands, Male)", "nl-NL-Standard-D (Dutch - Netherlands, Female)", "nl-NL-Standard-E (Dutch - Netherlands, Female)", "nl-NL-Wavenet-A (Dutch - Netherlands, Female)", "nl-NL-Wavenet-B (Dutch - Netherlands, Male)", "nl-NL-Wavenet-C (Dutch - Netherlands, Male)", "nl-NL-Wavenet-D (Dutch - Netherlands, Female)", "nl-NL-Wavenet-E (Dutch - Netherlands, Female)", "en-AU-Standard-A (English - Australia, Female)",
				"en-AU-Standard-B (English - Australia, Male)", "en-AU-Standard-C (English - Australia, Female)", "en-AU-Standard-D (English - Australia, Male)", "en-AU-Wavenet-A (English - Australia, Female)", "en-AU-Wavenet-B (English - Australia, Male)", "en-AU-Wavenet-C (English - Australia, Female)", "en-AU-Wavenet-D (English - Australia, Male)", "en-IN-Standard-A (English - India, Female)", "en-IN-Standard-B (English - India, Male)", "en-IN-Standard-C (English - India, Male)",
				"en-IN-Standard-D (English - India, Female)", "en-IN-Wavenet-A (English - India, Female)", "en-IN-Wavenet-B (English - India, Male)", "en-IN-Wavenet-C (English - India, Male)", "en-IN-Wavenet-D (English - India, Female)", "en-GB-Standard-A (English - UK, Female)", "en-GB-Standard-B (English - UK, Male)", "en-GB-Standard-C (English - UK, Female)", "en-GB-Standard-D (English - UK, Male)", "en-GB-Standard-F (English - UK, Female)",
				"en-GB-Wavenet-A (English - UK, Female)", "en-GB-Wavenet-B (English - UK, Male)", "en-GB-Wavenet-C (English - UK, Female)", "en-GB-Wavenet-D (English - UK, Male)", "en-GB-Wavenet-F (English - UK, Female)", "en-US-Standard-A (English - US, Male)", "en-US-Standard-B (English - US, Male)", "en-US-Standard-C (English - US, Female)", "en-US-Standard-D (English - US, Male)", "en-US-Standard-E (English - US, Female)",
				"en-US-Standard-F (English - US, Female)", "en-US-Standard-G (English - US, Female)", "en-US-Standard-H (English - US, Female)", "en-US-Standard-I (English - US, Male)", "en-US-Standard-J (English - US, Male)", "en-US-Wavenet-A (English - US, Male)", "en-US-Wavenet-B (English - US, Male)", "en-US-Wavenet-C (English - US, Female)", "en-US-Wavenet-D (English - US, Male)", "en-US-Wavenet-E (English - US, Female)",
				"en-US-Wavenet-F (English - US, Female)", "en-US-Wavenet-G (English - US, Female)", "en-US-Wavenet-H (English - US, Female)", "en-US-Wavenet-I (English - US, Male)", "en-US-Wavenet-J (English - US, Male)", "fil-PH-Standard-A (Filipino - Philippines, Female)", "fil-PH-Standard-B (Filipino - Philippines, Female)", "fil-PH-Standard-C (Filipino - Philippines, Male)", "fil-PH-Standard-D (Filipino - Philippines, Male)", "fil-PH-Wavenet-A (Filipino - Philippines, Female)",
				"fil-PH-Wavenet-B (Filipino - Philippines, Female)", "fil-PH-Wavenet-C (Filipino - Philippines, Male)", "fil-PH-Wavenet-D (Filipino - Philippines, Male)", "fi-FI-Standard-A (Finnish - Finland, Female)", "fi-FI-Wavenet-A (Finnish - Finland, Female)", "fr-CA-Standard-A (French - Canada, Female)", "fr-CA-Standard-B (French - Canada, Male)", "fr-CA-Standard-C (French - Canada, Female)", "fr-CA-Standard-D (French - Canada, Male)", "fr-CA-Wavenet-A (French - Canada, Female)",
				"fr-CA-Wavenet-B (French - Canada, Male)", "fr-CA-Wavenet-C (French - Canada, Female)", "fr-CA-Wavenet-D (French - Canada, Male)", "fr-FR-Standard-A (French - France, Female)", "fr-FR-Standard-B (French - France, Male)", "fr-FR-Standard-C (French - France, Female)", "fr-FR-Standard-D (French - France, Male)", "fr-FR-Standard-E (French - France, Female)", "fr-FR-Wavenet-A (French - France, Female)", "fr-FR-Wavenet-B (French - France, Male)",
				"fr-FR-Wavenet-C (French - France, Female)", "fr-FR-Wavenet-D (French - France, Male)", "fr-FR-Wavenet-E (French - France, Female)", "de-DE-Standard-A (German - Germany, Female)", "de-DE-Standard-B (German - Germany, Male)", "de-DE-Standard-C (German - Germany, Female)", "de-DE-Standard-D (German - Germany, Male)", "de-DE-Standard-E (German - Germany, Male)", "de-DE-Standard-F (German - Germany, Female)", "de-DE-Wavenet-A (German - Germany, Female)",
				"de-DE-Wavenet-B (German - Germany, Male)", "de-DE-Wavenet-C (German - Germany, Female)", "de-DE-Wavenet-D (German - Germany, Male)", "de-DE-Wavenet-E (German - Germany, Male)", "de-DE-Wavenet-F (German - Germany, Female)", "el-GR-Standard-A (Greek - Greece, Female)", "el-GR-Wavenet-A (Greek - Greece, Female)", "gu-IN-Standard-A (Gujarati - India, Female)", "gu-IN-Standard-B (Gujarati - India, Male)", "gu-IN-Wavenet-A (Gujarati - India, Female)",
				"gu-IN-Wavenet-B (Gujarati - India, Male)", "hi-IN-Standard-A (Hindi - India, Female)", "hi-IN-Standard-B (Hindi - India, Male)", "hi-IN-Standard-C (Hindi - India, Male)", "hi-IN-Standard-D (Hindi - India, Female)", "hi-IN-Wavenet-A (Hindi - India, Female)", "hi-IN-Wavenet-B (Hindi - India, Male)", "hi-IN-Wavenet-C (Hindi - India, Male)", "hi-IN-Wavenet-D (Hindi - India, Female)", "hu-HU-Standard-A (Hungarian - Hungary, Female)",
				"hu-HU-Wavenet-A (Hungarian - Hungary, Female)", "is-is-Standard-A (Icelandic - Iceland, Female)", "id-ID-Standard-A (Indonesian - Indonesia, Female)", "id-ID-Standard-B (Indonesian - Indonesia, Male)", "id-ID-Standard-C (Indonesian - Indonesia, Male)", "id-ID-Standard-D (Indonesian - Indonesia, Female)", "id-ID-Wavenet-A (Indonesian - Indonesia, Female)", "id-ID-Wavenet-B (Indonesian - Indonesia, Male)", "id-ID-Wavenet-C (Indonesian - Indonesia, Male)", "id-ID-Wavenet-D (Indonesian - Indonesia, Female)",
				"it-IT-Standard-A (Italian - Italy, Female)", "it-IT-Standard-B (Italian - Italy, Female)", "it-IT-Standard-C (Italian - Italy, Male)", "it-IT-Standard-D (Italian - Italy, Male)", "it-IT-Wavenet-A (Italian - Italy, Female)", "it-IT-Wavenet-B (Italian - Italy, Female)", "it-IT-Wavenet-C (Italian - Italy, Male)", "it-IT-Wavenet-D (Italian - Italy, Male)", "ja-JP-Standard-A (Japanese - Japan, Female)", "ja-JP-Standard-B (Japanese - Japan, Female)",
				"ja-JP-Standard-C (Japanese - Japan, Male)", "ja-JP-Standard-D (Japanese - Japan, Male)", "ja-JP-Wavenet-A (Japanese - Japan, Female)", "ja-JP-Wavenet-B (Japanese - Japan, Female)", "ja-JP-Wavenet-C (Japanese - Japan, Male)", "ja-JP-Wavenet-D (Japanese - Japan, Male)", "kn-IN-Standard-A (Kannada - India, Female)", "kn-IN-Standard-B (Kannada - India, Male)", "kn-IN-Wavenet-A (Kannada - India, Female)", "kn-IN-Wavenet-B (Kannada - India, Male)",
				"ko-KR-Standard-A (Korean - South Korea, Female)", "ko-KR-Standard-B (Korean - South Korea, Female)", "ko-KR-Standard-C (Korean - South Korea, Male)", "ko-KR-Standard-D (Korean - South Korea, Male)", "ko-KR-Wavenet-A (Korean - South Korea, Female)", "ko-KR-Wavenet-B (Korean - South Korea, Female)", "ko-KR-Wavenet-C (Korean - South Korea, Male)", "ko-KR-Wavenet-D (Korean - South Korea, Male)", "lv-lv-Standard-A (Latvian - Latvia, Male)", "ms-MY-Standard-A (Malay - Malaysia, Female)",
				"ms-MY-Standard-B (Malay - Malaysia, Male)", "ms-MY-Standard-C (Malay - Malaysia, Female)", "ms-MY-Standard-D (Malay - Malaysia, Male)", "ms-MY-Wavenet-A (Malay - Malaysia, Female)", "ms-MY-Wavenet-B (Malay - Malaysia, Male)", "ms-MY-Wavenet-C (Malay - Malaysia, Female)", "ms-MY-Wavenet-D (Malay - Malaysia, Male)", "ml-IN-Standard-A (Malayalam - India, Female)", "ml-IN-Standard-B (Malayalam - India, Male)", "ml-IN-Wavenet-A (Malayalam - India, Female)",
				"ml-IN-Wavenet-B (Malayalam - India, Male)", "cmn-CN-Standard-A (Mandarin Chinese, Female)", "cmn-CN-Standard-B (Mandarin Chinese, Male)", "cmn-CN-Standard-C (Mandarin Chinese, Male)", "cmn-CN-Standard-D (Mandarin Chinese, Female)", "cmn-CN-Wavenet-A (Mandarin Chinese, Female)", "cmn-CN-Wavenet-B (Mandarin Chinese, Male)", "cmn-CN-Wavenet-C (Mandarin Chinese, Male)", "cmn-CN-Wavenet-D (Mandarin Chinese, Female)", "cmn-TW-Standard-A (Mandarin Chinese, Female)",
				"cmn-TW-Standard-B (Mandarin Chinese, Male)", "cmn-TW-Standard-C (Mandarin Chinese, Male)", "cmn-TW-Wavenet-A (Mandarin Chinese, Female)", "cmn-TW-Wavenet-B (Mandarin Chinese, Male)", "cmn-TW-Wavenet-C (Mandarin Chinese, Male)", "nb-NO-Standard-A (Norwegian - Norway, Female)", "nb-NO-Standard-B (Norwegian - Norway, Male)", "nb-NO-Standard-C (Norwegian - Norway, Female)", "nb-NO-Standard-D (Norwegian - Norway, Male)", "nb-NO-Wavenet-A (Norwegian - Norway, Female)",
				"nb-NO-Wavenet-B (Norwegian - Norway, Male)", "nb-NO-Wavenet-C (Norwegian - Norway, Female)", "nb-NO-Wavenet-D (Norwegian - Norway, Male)", "nb-no-Standard-E (Norwegian - Norway, Female)", "nb-no-Wavenet-E (Norwegian - Norway, Female)", "pl-PL-Standard-A (Polish - Poland, Female)", "pl-PL-Standard-B (Polish - Poland, Male)", "pl-PL-Standard-C (Polish - Poland, Male)", "pl-PL-Standard-D (Polish - Poland, Female)", "pl-PL-Standard-E (Polish - Poland, Female)",
				"pl-PL-Wavenet-A (Polish - Poland, Female)", "pl-PL-Wavenet-B (Polish - Poland, Male)", "pl-PL-Wavenet-C (Polish - Poland, Male)", "pl-PL-Wavenet-D (Polish - Poland, Female)", "pl-PL-Wavenet-E (Polish - Poland, Female)", "pt-BR-Standard-A (Portuguese - Brazil, Female)", "pt-BR-Standard-B (Portuguese - Brazil, Male)", "pt-BR-Wavenet-A (Portuguese - Brazil, Female)", "pt-BR-Wavenet-B (Portuguese - Brazil, Male)", "pt-PT-Standard-A (Portuguese - Portugal, Female)",
				"pt-PT-Standard-B (Portuguese - Portugal, Male)", "pt-PT-Standard-C (Portuguese - Portugal, Male)", "pt-PT-Standard-D (Portuguese - Portugal, Female)", "pt-PT-Wavenet-A (Portuguese - Portugal, Female)", "pt-PT-Wavenet-B (Portuguese - Portugal, Male)", "pt-PT-Wavenet-C (Portuguese - Portugal, Male)", "pt-PT-Wavenet-D (Portuguese - Portugal, Female)", "pa-IN-Standard-A (Punjabi - India, Female)", "pa-IN-Standard-B (Punjabi - India, Male)", "pa-IN-Standard-C (Punjabi - India, Female)",
				"pa-IN-Standard-D (Punjabi - India, Male)", "pa-IN-Wavenet-A (Punjabi - India, Female)", "pa-IN-Wavenet-B (Punjabi - India, Male)", "pa-IN-Wavenet-C (Punjabi - India, Female)", "pa-IN-Wavenet-D (Punjabi - India, Male)", "ro-RO-Standard-A (Romanian - Romania, Female)", "ro-RO-Wavenet-A (Romanian - Romania, Female)", "ru-RU-Standard-A (Russian - Russia, Female)", "ru-RU-Standard-B (Russian - Russia, Male)", "ru-RU-Standard-C (Russian - Russia, Female)",
				"ru-RU-Standard-D (Russian - Russia, Male)", "ru-RU-Standard-E (Russian - Russia, Female)", "ru-RU-Wavenet-A (Russian - Russia, Female)", "ru-RU-Wavenet-B (Russian - Russia, Male)", "ru-RU-Wavenet-C (Russian - Russia, Female)", "ru-RU-Wavenet-D (Russian - Russia, Male)", "ru-RU-Wavenet-E (Russian - Russia, Female)", "sr-rs-Standard-A (Serbian - Cyrillic, Female)", "sk-SK-Standard-A (Slovak - Slovakia, Female)", "sk-SK-Wavenet-A (Slovak - Slovakia, Female)",
				"es-ES-Standard-A (Spanish - Spain, Female)", "es-ES-Standard-B (Spanish - Spain, Male)", "es-ES-Standard-C (Spanish - Spain, Female)", "es-ES-Standard-D (Spanish - Spain, Female)", "es-ES-Wavenet-B (Spanish - Spain, Male)", "es-ES-Wavenet-C (Spanish - Spain, Female)", "es-ES-Wavenet-D (Spanish - Spain, Female)", "es-US-Standard-A (Spanish - US, Female)", "es-US-Standard-B (Spanish - US, Male)", "es-US-Standard-C (Spanish - US, Male)",
				"es-US-Wavenet-A (Spanish - US, Female)", "es-US-Wavenet-B (Spanish - US, Male)", "es-US-Wavenet-C (Spanish - US, Male)", "sv-SE-Standard-A (Swedish - Sweden, Female)", "sv-SE-Wavenet-A (Swedish - Sweden, Female)", "ta-IN-Standard-A (Tamil - India, Female)", "ta-IN-Standard-B (Tamil - India, Male)", "ta-IN-Wavenet-A (Tamil - India, Female)", "ta-IN-Wavenet-B (Tamil - India, Male)", "te-IN-Standard-A (Telugu - India, Female)",
				"te-IN-Standard-B (Telugu - India, Male)", "th-TH-Standard-A (Thai - Thailand, Female)", "tr-TR-Standard-A (Turkish - Turkey, Female)", "tr-TR-Standard-B (Turkish - Turkey, Male)", "tr-TR-Standard-C (Turkish - Turkey, Female)", "tr-TR-Standard-D (Turkish - Turkey, Female)", "tr-TR-Standard-E (Turkish - Turkey, Male)", "tr-TR-Wavenet-A (Turkish - Turkey, Female)", "tr-TR-Wavenet-B (Turkish - Turkey, Male)", "tr-TR-Wavenet-C (Turkish - Turkey, Female)",
				"tr-TR-Wavenet-D (Turkish - Turkey, Female)", "tr-TR-Wavenet-E (Turkish - Turkey, Male)", "uk-UA-Standard-A (Ukrainian - Ukraine, Female)", "uk-UA-Wavenet-A (Ukrainian - Ukraine, Female)", "vi-VN-Standard-A (Vietnamese - Vietnam, Female)", "vi-VN-Standard-B (Vietnamese - Vietnam, Male)", "vi-VN-Standard-C (Vietnamese - Vietnam, Female)", "vi-VN-Standard-D (Vietnamese - Vietnam, Male)", "vi-VN-Wavenet-A (Vietnamese - Vietnam, Female)", "vi-VN-Wavenet-B (Vietnamese - Vietnam, Male)",
				"vi-VN-Wavenet-C (Vietnamese - Vietnam, Female)", "vi-VN-Wavenet-D (Vietnamese - Vietnam, Male)", "yue-HK-Standard-A (Chinese - Hong Kong, Female)", "yue-HK-Standard-B (Chinese - Hong Kong, Male)", "yue-HK-Standard-C (Chinese - Hong Kong, Female)", "yue-HK-Standard-D (Chinese - Hong Kong, Male)"
			};
			ıtems.AddRange(items);
			for (int i = 0; i < comboVoices.Items.Count; i++)
			{
				if ((comboVoices.Items[i] as string).StartsWith(selectedVoice))
				{
					comboVoices.SelectedIndex = i;
					break;
				}
			}
			if (comboVoices.SelectedIndex == -1)
			{
				comboVoices.Text = selectedVoice;
			}
			return;
		}
		ComboBox.ObjectCollection ıtems2 = comboVoices.Items;
		items = new string[84]
		{
			"Zeina (Arabic, Female - Standard)", "Zhiyu (Chinese, Female - Standard)", "Naja (Danish, Female - Standard)", "Mads (Danish, Male - Standard)", "Lotte (Dutch, Female - Standard)", "Ruben (Dutch, Male - Standard)", "Nicole (English - Australian, Female - Standard)", "Olivia (English - Australian, Female - Neural)", "Russell (English - Australian, Male - Standard)", "Amy (English - British, Female - Standard)",
			"Amy (English - British, Female - Neural)", "Emma (English - British, Female - Standard)", "Emma (English - British, Female - Neural)", "Brian (English - British, Male - Standard)", "Brian (English - British, Male - Neural)", "Raveena (English - Indian, Female - Standard)", "Aditi (English - Indian and Hindi, Female - Standard)", "Aria (English - NZ, Female - Neural)", "Ayanda (English - ZA, Female - Neural)", "Ivy (English - US, Female Child - Standard)",
			"Ivy (English - US, Female Child - Neural)", "Joanna (English - US, Female - Standard)", "Joanna (English - US, Female - Neural)", "Kendra (English - US, Female - Standard)", "Kendra (English - US, Female - Neural)", "Kimberly (English - US, Female - Standard)", "Kimberly (English - US, Female - Neural)", "Salli (English - US, Female - Standard)", "Salli (English - US, Female - Neural)", "Joey (English - US, Male - Standard)",
			"Joey (English - US, Male - Neural)", "Justin (English - US, Male Child - Standard)", "Justin (English - US, Male Child - Neural)", "Kevin (English - US, Male Child - Neural)", "Matthew (English - US, Male - Standard)", "Matthew (English - US, Male - Neural)", "Geraint (English - Welsh, Male - Standard)", "Celine (French - France, Female - Standard)", "Lea (French - France, Female - Standard)", "Lea (French - France, Female - Neural)",
			"Mathieu (French - France, Male - Standard)", "Chantal (French - Canada, Female - Standard)", "Gabrielle (French - Canada, Female - Neural)", "Marlene (German, Female - Standard)", "Vicki (German, Female - Standard)", "Vicki (German, Female - Neural)", "Hans (German, Male - Standard)", "Dora (Icelandic, Female - Standard)", "Karl (Icelandic, Male - Standard)", "Carla (Italian, Female - Standard)",
			"Bianca (Italian, Female - Standard)", "Bianca (Italian, Female - Neural)", "Giorgio (Italian, Male - Standard)", "Mizuki (Japanese, Female - Standard)", "Takumi (Japanese, Male - Standard)", "Takumi (Japanese, Male - Neural)", "Seoyeon (Korean, Female - Standard)", "Seoyeon (Korean, Female - Neural)", "Liv (Norwegian, Female - Standard)", "Ewa (Polish, Female - Standard)",
			"Maja (Polish, Female - Standard)", "Jacek (Polish, Male - Standard)", "Jan (Polish, Male - Standard)", "Camila (Portuguese - Brazilian, Female - Standard)", "Camila (Portuguese - Brazilian, Female - Neural)", "Vitoria (Portuguese - Brazilian, Female - Standard)", "Ricardo (Portuguese - Brazilian, Male - Standard)", "Ines (Portuguese - European, Female - Standard)", "Cristiano (Portuguese - European, Male - Standard)", "Carmen (Romanian, Female - Standard)",
			"Tatyana (Russian, Female - Standard)", "Maxim (Russian, Male - Standard)", "Conchita (Spanish - Castilian, Female - Standard)", "Lucia (Spanish - Castilian, Female - Standard)", "Lucia (Spanish - Castilian, Female - Neural)", "Enrique (Spanish - Castilian, Male - Standard)", "Mia (Spanish - Mexico, Female - Standard)", "Lupe (Spanish - Latin American, Female - Standard)", "Lupe (Spanish - Latin American, Female - Neural)", "Penelope (Spanish - Latin American, Female - Standard)",
			"Miguel (Spanish - Latin American, Male - Standard)", "Astrid (Swedish, Female - Standard)", "Filiz (Turkish, Female - Standard)", "Gwyneth (Welsh, Female - Standard)"
		};
		ıtems2.AddRange(items);
		for (int j = 0; j < comboVoices.Items.Count; j++)
		{
			string text = comboVoices.Items[j] as string;
			if (text.StartsWith(selectedVoice) && ((selectedVoiceType == TextToSpeechVoiceTypes.Standard && text.Contains("Standard")) || (selectedVoiceType == TextToSpeechVoiceTypes.Neural && text.Contains("Neural"))))
			{
				comboVoices.SelectedIndex = j;
				break;
			}
		}
		if (comboVoices.SelectedIndex == -1)
		{
			comboVoices.Text = selectedVoice;
		}
	}
}
