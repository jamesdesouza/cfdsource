using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public static class VoiceInputHintsHelper
{
	private static readonly Dictionary<string, List<VoiceInputHintToken>> hintTokenDictionary = new Dictionary<string, List<VoiceInputHintToken>>
	{
		{
			"ar-EG",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_FULLPHONENUM", "A phone number, as used in the target locale.", "صفر اثنين واحد اثنين ثلاثة اربعة خمسة ستة سبعة ثمانية", "0212345678"),
				new VoiceInputHintToken("$OOV_CLASS_OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "سالب اثنين", "-2")
			}
		},
		{
			"zh",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "八 六 四 零 四 二 三 四 五 六 七 八 九", "86-404-2345-6789"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "二 元", "2元"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "三", "三"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "第 一", "第 1"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "百 分 之 一 百", "100%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "五 点", "5点")
			}
		},
		{
			"zh-TW",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "二 元", "2元"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "三", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "百 分 之 一 百", "100%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "加 八 八 六 二 三 四 五 六 七 八 九 零", "+886-2-3456-7890"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "五 點", "5點"),
				new VoiceInputHintToken("$YEAR", "A year.", "二 零 零 零", "2000")
			}
		},
		{
			"cs-CZ",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "tři sta dvacet pět", "325"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "prvního", "1"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "tři sta korun", "300 kč"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "července", "července"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "dvacátý sedmý", "27."),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "plus čtyři sta dvacet devět osm sedm šest pět čtyři tři dva jedna", "+420 987 654 321"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "tři jedna dva jedna jedna", "312 11")
			}
		},
		{
			"da-DK",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ADDRESSNUM", "A street number for an address, as used in the target locale.", "femogtyve", "25"),
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "en to to nul", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "en nul fire komma tre", "104,3"),
				new VoiceInputHintToken("$OOV_CLASS_OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "to", "2"),
				new VoiceInputHintToken("$OOV_CLASS_POSTALCODE", "A postal code, as used in the target locale.", "syvogtredive hundrede", "3700"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "en", "1"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "treogtyvende", "23."),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juli", "juli"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "nulte", "0."),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "en procent", "1%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "tretten femogfyrre syvogtres niogfirs", "13 45 67 89"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "et treogtyve", "1:23"),
				new VoiceInputHintToken("$YEAR", "A year.", "nitten hundrede og firs", "1980")
			}
		},
		{
			"de-DE",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "sieben hundert sieben und vierzig", "747"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "ein hundert eins komma neun", "101,9"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "eins", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "neun tausend neun hundert neun und neunzig", "9999"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "drei und zwanzigste", "23."),
				new VoiceInputHintToken("$FULLDATE", "A full date using numbers.", "neunter neunter zwei tausend vierzehn", "9.9.2014"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "null zwölf vier und dreißig sechs und fünfzig acht und siebzig neun", "01234 56789"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "drei hundert euro", "300 €"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juli", "Juli"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "sechzehn uhr", "16 Uhr"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "drei", "3"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "dreizehntem", "13."),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "zwei prozent", "2%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "acht fünf null null", "8500"),
				new VoiceInputHintToken("$YEAR", "A year.", "tausend neun hundert neun und neunzig", "1999")
			}
		},
		{
			"en-AU",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ALPHANUMERIC_SEQUENCE", "A sequence of letters [a-z] and digits.", "a b c one two three", "ABC123"),
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "twelve twenty", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero one two three four five six seven eight nine", "0123456789"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "one oh four point three", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_FULLPHONENUM", "A phone number, as used in the target locale.", "oh nine one two three four five six seven eight", "09 1234 5678"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "one", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "one hundred ninety one", "191"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "twenty third", "23rd"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "zero nine eight seven six five four three two one", "09 8765 4321"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "one dollar", "$1"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "february", "February"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "four", "4"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "a hundred percent", "100%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "a quarter to nine", "8:45"),
				new VoiceInputHintToken("$YEAR", "A year.", "two thousand ten", "2010")
			}
		},
		{
			"en-GB",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "twelve twenty", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero one two three four five six seven eight nine", "0123456789"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "one oh four point three", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_POSTALCODE", "A postal code, as used in the target locale.", "n one c four a g", "N1C 4AG"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "one", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "three hundred twenty five", "325"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "the twenty third", "23rd"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "oh one six three two one two three four five", "01632 12345"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "a dollar", "$1"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "february", "February"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "twenty five", "25"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "one percent", "1%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "eighteen thirty five", "18:35"),
				new VoiceInputHintToken("$YEAR", "A year.", "two thousand ten", "2010")
			}
		},
		{
			"en-IN",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ALPHANUMERIC_SEQUENCE", "A sequence of letters [a-z] and digits.", "a b c one two three", "ABC123"),
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "one thousand two hundred twenty", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero one two three four five six seven eight nine", "0123456789"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "one hundred four point three", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_FULLPHONENUM", "A phone number, as used in the target locale.", "shoonya do do teen char panch chah saat aath nou shoonya", "022 3456 7890"),
				new VoiceInputHintToken("$OOV_CLASS_PERCENT", "A percentage value, including any corresponding percent sign.", "minus one percent", "-1%"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "one", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "three hundred twenty five", "325"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "the twenty third", "23rd"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "one hundred and twenty three rupee", "123 rupee"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "february", "February"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "four", "4"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "zero seven zero one five", "07015"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "quarter till nine", "8:45"),
				new VoiceInputHintToken("$YEAR", "A year.", "two thousand ten", "2010")
			}
		},
		{
			"en-US",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ALPHANUMERIC_SEQUENCE", "A sequence of letters [a-z] and digits.", "a b c one two three", "ABC123"),
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "twelve twenty", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero zero zero zero", "0000"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "one oh four point three", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "one", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "one hundred ninety one", "191"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "the fifth", "5th"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "one eight hundred five five five four oh oh one", "+1-800-555-4001"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "one dollar", "$1"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "july", "July"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "two", "2"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "a hundred percent", "100%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "one zero zero one zero", "10010"),
				new VoiceInputHintToken("$STREET", "A numbered street name.", "fifty first", "51st"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "ten o'clock", "10:00"),
				new VoiceInputHintToken("$YEAR", "A year.", "twenty ten", "2010")
			}
		},
		{
			"en-KE",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ALPHANUMERIC_SEQUENCE", "A sequence of letters [a-z] and digits.", "a b c one two three", "ABC123"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero one two three four five six seven eight nine", "0123456789"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "twenty three", "23"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "february", "February"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "four", "4"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "minus one point nine zero five percent", "-1.905%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "a quarter to nine", "8:45"),
				new VoiceInputHintToken("$YEAR", "A year.", "sixteen hundred thirty four", "1634")
			}
		},
		{
			"en-NG",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ALPHANUMERIC_SEQUENCE", "A sequence of letters [a-z] and digits.", "a b c one two three", "ABC123"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero one two three four five six seven eight nine", "0123456789"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "twenty three", "23"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "february", "February"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "four", "4"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "minus one point nine zero five percent", "-1.905%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "a quarter to nine", "8:45"),
				new VoiceInputHintToken("$YEAR", "A year.", "sixteen hundred thirty four", "1634")
			}
		},
		{
			"en-ZA",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ALPHANUMERIC_SEQUENCE", "A sequence of letters [a-z] and digits.", "a b c one two three", "ABC123"),
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "zero one two three four five six seven eight nine", "0123456789"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "twenty three", "23"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "february", "February"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "four", "4"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "minus one point nine zero five percent", "-1.905%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "a quarter to nine", "8:45"),
				new VoiceInputHintToken("$YEAR", "A year.", "sixteen hundred thirty four", "1634")
			}
		},
		{
			"es-ES",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "doce veinte", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "ciento cuatro punto tres", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "uno", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "trescientos veinticinco", "325"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "veintitrés", "23"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "más treinta y cuatro tres ocho siete sesenta y cinco cuatro tres dos uno", "+34 387 654 321"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trescientos euros", "300 €"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "tres", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "uno por ciento", "1%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "ocho veinte diez", "82010"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "cuatro de la tarde en punto", "16:00"),
				new VoiceInputHintToken("$YEAR", "A year.", "mil novecientos sesenta y cuatro", "1964")
			}
		},
		{
			"es-US",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "doce veinte", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "ciento cuatro punto tres", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "uno", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "uno cinco dos cinco", "1525"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "veintitrés", "23"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trescientos euros", "300€"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "julio", "julio"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "tres", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "uno por ciento", "1%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "uno tres uno cuatro cinco cinco cinco uno uno nueve uno", "1 314 555 1191"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "ochenta treinta y dos cuatro", "80324"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "cuatro", "4:00"),
				new VoiceInputHintToken("$YEAR", "A year.", "dos mil diez", "2010")
			}
		},
		{
			"fil-PH",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "three twenty five", "325"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "plus animnapu't tatlo dalawa tatlo apat lima anim pito walo siyam", "+63 2 345 6789"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "isandaan at dalawampu't tatlong milyon apat na raan at limampu't anim na libo pitondaan at walumpu't isang piso at dalawampu't tatlo", "Php123456781.23"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "ikaisandaan", "ika-100"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "seven zero one five", "7015"),
				new VoiceInputHintToken("$YEAR", "A year.", "dos mil dyis", "2010")
			}
		},
		{
			"fr-CA",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "mille cinq cent huitante-quatre", "1584"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "cent quatre point trois", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "un", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "vingt-cinq", "25"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "vingt-trois", "23"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "plus un huit zéro zéro cinq cent cinquante-cinq quatre mille un", "+1 800 555-4001"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trois cents euros", "300 €"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juillet", "juillet"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "trente six", "30,6"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "un pour cent", "1%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "a trois r quatre t cinq", "A3R 4T5"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "quinze heures zéro zéro", "15 h 00"),
				new VoiceInputHintToken("$YEAR", "A year.", "deux mille dix", "2010")
			}
		},
		{
			"fr-FR",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "mille cinq cent huitante-quatre", "1584"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "cent quatre point trois", "104.3"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "un", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "vingt-cinq", "25"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "vingt-trois", "23"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "trente-trois neuf quatre-vingt-dix-neuf treize vingt-quatre trente-cinq", "33 9 99 13 24 35"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trois cents euros", "300 €"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juillet", "juillet"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "trente six", "30,6"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "un pour cent", "1%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "zéro sept mille quinze", "07015"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "quinze heures zéro zéro", "15h00"),
				new VoiceInputHintToken("$YEAR", "A year.", "deux mille dix", "2010")
			}
		},
		{
			"hi-IN",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "twenty three", "23"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "एक एक द\u094b त\u0940न च\u093eर प\u093e\u0902च छह स\u093eत आठ न\u094c", "11 2345 6789"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "त\u0940न स\u094c र\u0941पय\u0947", "₹300"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "ज\u0941ल\u093eई", "ज\u0941ल\u093eई"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "त\u0940न स\u094c", "300"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "श\u0942न\u094dय प\u094dरत\u093fशत", "0%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "", ""),
				new VoiceInputHintToken("$YEAR", "A year.", "एक हज\u093c\u093eर न\u094c स\u094c", "1900")
			}
		},
		{
			"id-ID",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "delapan ratus empat puluh enam", "846"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "sembilan puluh titik lima", "90.5"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "satu ratus dua puluh delapan", "128"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "dua puluh tiga", "23"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "seratus dua puluh tiga rupiah", "Rp123"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juli", "juli"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "tiga", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "tiga puluh persen", "30%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "nol dua satu satu dua tiga satu dua tiga empat", "021 123 1234"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "tiga puluh dua tiga empat lima", "32345"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "tujuh lewat tiga puluh tiga menit", "7.33"),
				new VoiceInputHintToken("$YEAR", "A year.", "sembilan belas lima lima", "1955")
			}
		},
		{
			"it-IT",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "otto quaranta sei", "846"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "novanta punto cinque", "90.5"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "un", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "venti cinque", "25"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "venti tre", "23"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "tre due uno uno due tre quattro cinque sei sette", "321 12 34 567"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trecento euro", "€300"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "luglio", "luglio"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "tre", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "uno per cento", "1%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "ottanta due zero dieci", "82010"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "zero", "0"),
				new VoiceInputHintToken("$YEAR", "A year.", "mille e novecento novanta nove", "1999")
			}
		},
		{
			"ja-JP",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_DIGIT_SEQUENCE", "A digit sequence of any length.", "ニ", "2"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "いち の じゅー いち の に じゅー きゅー", "1-11-29"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "ぜろ きゅー ぜろ いち にー さん よん ごー ろく なな はち", "090-1234-5678"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "きゅーせん えん", "9000 円"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "れー てん いち", "0.1"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "れー てん ぜろ ぱーせんと", "0.0%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "にー さん ぜろ にー さん ぜろ ぜろ", "230-2300"),
				new VoiceInputHintToken("$TIME", "A specific time of day, or duration.", "に じゅー じ さん じっ ぷん", "20 時 30 分")
			}
		},
		{
			"ko-KR",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "공 이 삼 사 오 육 다시 칠 팔 구 공", "02-3456-7890"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "일시 사십이분", "1시 42분")
			}
		},
		{
			"nb-NO",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ADDRESSNUM", "A street number for an address, as used in the target locale.", "femogtyve", "25"),
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "en fire åtti fem", "1485"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "sytti syv komma en", "77,1"),
				new VoiceInputHintToken("$OOV_CLASS_OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "null komma en", "0,1"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "en", "1"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "fireogtredve komma fem seks prosent", "34,56%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "tretti femogførti sjuogseksti niogåtti", "30 45 67 89"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "ni tusen og ni hundre og tretti", "9930"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "ett tjue tre", "1:23"),
				new VoiceInputHintToken("$YEAR", "A year.", "tjue syv åtti", "2780")
			}
		},
		{
			"nl-NL",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "zeven honderd zeven en veertig", "747"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "honderd één punt negen", "101.9"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "één", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "negen en negentig negen en negentig", "9999"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "drie en twintig", "23"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "plus één en dertig zes vijf zes één twee drie vier vijf zes", "+31-656-123456"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "drie honderd euro", "€300"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juli", "juli"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "drie", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "één procent", "1%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "twee en negentig drie en zeventig h z", "9273 hz"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "vier 's-middags", "16:00"),
				new VoiceInputHintToken("$YEAR", "A year.", "achttien dertig", "1830")
			}
		},
		{
			"pl-PL",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ADDRESSNUM", "A street number for an address, as used in the target locale.", "dwadzieścia pięć", "25"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "dwudziesty drugi", "22"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "dwa tysiące trzysta czterdzieści pięć funtów i pięćdziesiąt sześć pensów", "2345,56 £"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "trzy", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "jeden procent", "1%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "dwanaście trzy czterdzieści pięć sześćdziesiąt siedem osiemdziesiąt dziewięć", "12 345 67 89"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "druga", "2:00")
			}
		},
		{
			"pt-BR",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "dois cinco", "25"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "vinte e três", "23"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trezentos reais", "r$ 300"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "julho", "julho"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "um ponto seis", "1.6"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "duzentos por cento", "200%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "oitenta e sete sessenta e cinco quarenta e três vinte e um", "8765-4321"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "oito cinquenta oito zero sessenta e três quatro", "85080-634"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "dezesseis horas", "16 horas"),
				new VoiceInputHintToken("$YEAR", "A year.", "dois mil e dez", "2010")
			}
		},
		{
			"pt-PT",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "nove mil novecentos e noventa e nove", "9999"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "um", "1"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "um dois três quatro cinco seis sete oito nove", "12 345 6789"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "trezentos euros", "300 €"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "julho", "julho"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "três", "3"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "centésima", "100ª"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "dois por cento", "2%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "dois mil duzentos e quinze seiscentos e trinta e quatro", "2215-634"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "dezesseis horas", "16 horas")
			}
		},
		{
			"ru-RU",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "триста двадцать пять", "325"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "второго", "2"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "триста тридцать три сто двадцать три четыре пять шестьдесят семь", "333 123 45 67"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "триста рублей", "300 руб."),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "декабря", "декабря"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "три", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "нуля процентов", "0%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "шесть два один нуль пять шесть", "621056"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "шесть часов", "6:00"),
				new VoiceInputHintToken("$YEAR", "A year.", "две тысячи пятого", "2005")
			}
		},
		{
			"sv-SE",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_AM_RADIO_FREQUENCY", "An AM radio frequency.", "ett tusen två hundra tjugo", "1220"),
				new VoiceInputHintToken("$OOV_CLASS_FM_RADIO_FREQUENCY", "An FM radio frequency.", "ett hundra fyra komma tre", "104,3"),
				new VoiceInputHintToken("$OOV_CLASS_TEMPERATURE", "A temperature, in degrees.", "en", "1"),
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "nio tusen nio hundra nittio nio", "9999"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "tjugo tredje", "23"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "tre hundra kronor", "300 kr"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "juli", "juli"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "tre", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "en procent", "1%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "noll åtta ett hundra tjugo tre fyra fem sex sjuttio åtta", "08-123 456 78"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "åtta hundra femtio ett fyrtio fyra", "851 44"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "ett tjugo tre", "1.23"),
				new VoiceInputHintToken("$YEAR", "A year.", "nitton hundra åttio", "1980")
			}
		},
		{
			"th-TH",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_FULLPHONENUM", "A phone number, as used in the target locale.", "ศ\u0e39นย\u0e4c สอง ข\u0e35ด หน\u0e36\u0e48ง สอง สาม ส\u0e35\u0e48 ห\u0e49า หก เจ\u0e47ด", "02-1234567"),
				new VoiceInputHintToken("$OOV_CLASS_OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "ลบ สอง", "-2"),
				new VoiceInputHintToken("$OOV_CLASS_PERCENT", "A percentage value, including any corresponding percent sign.", "หน\u0e36\u0e48ง percent", "1%"),
				new VoiceInputHintToken("$OOV_CLASS_POSTALCODE", "A postal code, as used in the target locale.", "หน\u0e36\u0e48ง ศ\u0e39นย\u0e4c สอง สอง ส\u0e35\u0e48", "10224")
			}
		},
		{
			"tr-TR",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$OOV_CLASS_ADDRESSNUM", "A street number for an address, as used in the target locale.", "yirmi beş", "25"),
				new VoiceInputHintToken("$OOV_CLASS_FULLPHONENUM", "A phone number, as used in the target locale.", "iki yüz altmış sekiz iki yüz otuz dört elli altı yetmiş sekiz", "268 234 56 78"),
				new VoiceInputHintToken("$DAY", "A numbered day within a month.", "yirmi üç", "23"),
				new VoiceInputHintToken("$MONTH", "A named month in a year. Contextual phrases like \"2 months from now\" are not supported.", "kasım", "kasım"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "üç", "3"),
				new VoiceInputHintToken("$ORDINAL", "An ordinal number.", "birinci", "1."),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "yüzde bir", "1%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "sekiz iki bir sıfır sıfır", "82100"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "öğleden sonra dört", "16"),
				new VoiceInputHintToken("$YEAR", "A year.", "bin dokuz yüz doksan dokuz", "1999")
			}
		},
		{
			"uk-UA",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "трьомстам гривням", "300 грн"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "одинадцять відсоток", "11%"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "один два три чотири п'ять шість сім вісім дев'ять нуль", "123 456 78 90"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "нуль сім нуль п'ятнадцять", "07015"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "за двадцять шість тринадцята", "12:34")
			}
		},
		{
			"vi-VN",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$ADDRESSNUM", "A street number for an address, as used in the target locale.", "sáu mươi hai", "62"),
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "không chín chín mười hai ba bốn mươi lăm sáu mươi bảy", "099 123 4567"),
				new VoiceInputHintToken("$MONEY", "An amount of money with a currency unit name.", "ba trăm đồng", "300₫"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "ba", "3"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "hai phần trăm", "2%"),
				new VoiceInputHintToken("$POSTALCODE", "A postal code, as used in the target locale.", "một bảy không một năm không", "170150"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "mười sáu giờ", "16")
			}
		},
		{
			"yue-Hant-HK",
			new List<VoiceInputHintToken>
			{
				new VoiceInputHintToken("$FULLPHONENUM", "A phone number, as used in the target locale.", "一 二 三 四 五 六 七 八", "1234 5678"),
				new VoiceInputHintToken("$OPERAND", "A numerical value including whole numbers, fractions, and decimals.", "三", "三"),
				new VoiceInputHintToken("$PERCENT", "A percentage value, including any corresponding percent sign.", "一 百 percent", "100%"),
				new VoiceInputHintToken("$TIME", "A specific time of day.", "五 點", "5點")
			}
		}
	};

	public static List<VoiceInputHintToken> GetTokensForLanguage(string languageCode)
	{
		if (hintTokenDictionary.TryGetValue(languageCode, out var value))
		{
			return value;
		}
		return new List<VoiceInputHintToken>();
	}

	public static VoiceInputHintToken GetToken(string languageCode, string token)
	{
		foreach (VoiceInputHintToken item in GetTokensForLanguage(languageCode))
		{
			if (token == item.GetText())
			{
				return item;
			}
		}
		return null;
	}
}
