namespace untitledplantgame.Common;

// Clipboard
// Find: ([A-Za-z]+) \| .*
// Replace: public static BBColor $1 => new("\L$1");

/// <summary>
/// Precompiled list of BBColor Hexcodes and Apply method
/// </summary>
public class BBColor
{
	public static BBColor Pink => new("#FFC0CB");
	public static BBColor Lightpink => new("#FFB6C1");
	public static BBColor Hotpink => new("#FF69B4");
	public static BBColor Deeppink => new("#FF1493");
	public static BBColor Palevioletred => new("#D87093");
	public static BBColor Mediumvioletred => new("#C71585");
	public static BBColor Lavender => new("#E6E6FA");
	public static BBColor Thistle => new("#D8BFD8");
	public static BBColor Plum => new("#DDA0DD");
	public static BBColor Orchid => new("#DA70D6");
	public static BBColor Violet => new("#EE82EE");
	public static BBColor Fuchsia => new("#FF00FF");
	public static BBColor Magenta => new("#FF00FF");
	public static BBColor Mediumorchid => new("#BA55D3");
	public static BBColor Darkorchid => new("#9932CC");
	public static BBColor Darkviolet => new("#9400D3");
	public static BBColor Blueviolet => new("#8A2BE2");
	public static BBColor Darkmagenta => new("#8B008B");
	public static BBColor Purple => new("#800080");
	public static BBColor Mediumpurple => new("#9370D8");
	public static BBColor Mediumslateblue => new("#7B68EE");
	public static BBColor Slateblue => new("#6A5ACD");
	public static BBColor Darkslateblue => new("#483D8B");
	public static BBColor Rebeccapurple => new("#663399");
	public static BBColor Indigo => new("#4B0082");
	public static BBColor Lightsalmon => new("#FFA07A");
	public static BBColor Salmon => new("#FA8072");
	public static BBColor Darksalmon => new("#E9967A");
	public static BBColor Lightcoral => new("#F08080");
	public static BBColor Indianred => new("#CD5C5C");
	public static BBColor Crimson => new("#DC143C");
	public static BBColor Red => new("#FF0000");
	public static BBColor Firebrick => new("#B22222");
	public static BBColor Darkred => new("#8B0000");
	public static BBColor Maroon => new("#800000");
	public static BBColor Orange => new("#FFA500");
	public static BBColor Darkorange => new("#FF8C00");
	public static BBColor Coral => new("#FF7F50");
	public static BBColor Tomato => new("#FF6347");
	public static BBColor Orangered => new("#FF4500");
	public static BBColor Gold => new("#FFD700");
	public static BBColor Yellow => new("#FFFF00");
	public static BBColor Lightyellow => new("#FFFFE0");
	public static BBColor Lemonchiffon => new("#FFFACD");
	public static BBColor Lightgoldenrodyellow => new("#FAFAD2");
	public static BBColor Papayawhip => new("#FFEFD5");
	public static BBColor Moccasin => new("#FFE4B5");
	public static BBColor Peachpuff => new("#FFDAB9");
	public static BBColor Palegoldenrod => new("#EEE8AA");
	public static BBColor Khaki => new("#F0E68C");
	public static BBColor Darkkhaki => new("#BDB76B");
	public static BBColor Goldenrod => new("#DAA520");
	public static BBColor Darkgoldenrod => new("#B8860B	          	Greenyellow | ADFF2F");
	public static BBColor Chartreuse => new("#7FFF00");
	public static BBColor Lawngreen => new("#7CFC00");
	public static BBColor Lime => new("#00FF00");
	public static BBColor Limegreen => new("#32CD32");
	public static BBColor Palegreen => new("#98FB98");
	public static BBColor Lightgreen => new("#90EE90");
	public static BBColor Mediumspringgreen => new("#00FA9A");
	public static BBColor Springgreen => new("#00FF7F");
	public static BBColor Mediumseagreen => new("#3CB371");
	public static BBColor Seagreen => new("#2E8B57");
	public static BBColor Forestgreen => new("#228B22");
	public static BBColor Green => new("#008000");
	public static BBColor Darkgreen => new("#006400");
	public static BBColor Yellowgreen => new("#9ACD32");
	public static BBColor Olivedrab => new("#688E23");
	public static BBColor Olive => new("#808000");
	public static BBColor Darkolivegreen => new("#556B2F");
	public static BBColor Mediumaquamarine => new("#66CDAA");
	public static BBColor Darkseagreen => new("#8FBC8F");
	public static BBColor Lightseagreen => new("#20B2AA");
	public static BBColor Darkcyan => new("#008B8B");
	public static BBColor Teal => new("#008080");
	public static BBColor Aqua => new("#00FFFF");
	public static BBColor Cyan => new("#00FFFF");
	public static BBColor Lightcyan => new("#E0FFFF");
	public static BBColor Paleturquoise => new("#AFEEEE");
	public static BBColor Aquamarine => new("#7FFFD4");
	public static BBColor Turquoise => new("#40E0D0");
	public static BBColor Mediumturquoise => new("#48D1CC");
	public static BBColor Darkturquoise => new("#00CED1");
	public static BBColor Cadetblue => new("#5F9EA0");
	public static BBColor Steelblue => new("#4682B4");
	public static BBColor Lightsteelblue => new("#B0C4DE");
	public static BBColor Lightblue => new("#ADD8E6");
	public static BBColor Powderblue => new("#B0E0E6");
	public static BBColor Lightskyblue => new("#87CEFA");
	public static BBColor Skyblue => new("#87CEEB");
	public static BBColor Cornflowerblue => new("#6495ED");
	public static BBColor Deepskyblue => new("#00BFFF");
	public static BBColor Dodgerblue => new("#1E90FF");
	public static BBColor Royalblue => new("#4169E1");
	public static BBColor Blue => new("#0000FF");
	public static BBColor Mediumblue => new("#0000CD");
	public static BBColor Darkblue => new("#00008B");
	public static BBColor Navy => new("#000080");
	public static BBColor Midnightblue => new("#191970	          	Cornsilk | FFF8DC");
	public static BBColor Blanchedalmond => new("#FFEBCD");
	public static BBColor Bisque => new("#FFE4C4");
	public static BBColor Navajowhite => new("#FFDEAD");
	public static BBColor Wheat => new("#F5DEB3");
	public static BBColor Burlywood => new("#DEB887");
	public static BBColor Tan => new("#D2B48C");
	public static BBColor Rosybrown => new("#BC8F8F");
	public static BBColor Sandybrown => new("#F4A460");
	public static BBColor Peru => new("#CD853F");
	public static BBColor Chocolate => new("#D2691E");
	public static BBColor Saddlebrown => new("#8B4513");
	public static BBColor Sienna => new("#A0522D");
	public static BBColor Brown => new("#A52A2A");
	public static BBColor White => new("#FFFFFF");
	public static BBColor Snow => new("#FFFAFA");
	public static BBColor Honeydew => new("#F0FFF0");
	public static BBColor Mintcream => new("#F5FFFA");
	public static BBColor Azure => new("#F0FFFF");
	public static BBColor Aliceblue => new("#F0F8FF");
	public static BBColor Ghostwhite => new("#F8F8FF");
	public static BBColor Whitesmoke => new("#F5F5F5");
	public static BBColor Seashell => new("#FFF5EE");
	public static BBColor Beige => new("#F5F5DC");
	public static BBColor Oldlace => new("#FDF5E6");
	public static BBColor Floralwhite => new("#FFFAF0");
	public static BBColor Ivory => new("#FFFFF0");
	public static BBColor Antiquewhite => new("#FAEBD7");
	public static BBColor Linen => new("#FAF0E6");
	public static BBColor Lavenderblush => new("#FFF0F5");
	public static BBColor Mistyrose => new("#FFE4E1");
	public static BBColor Gainsboro => new("#DCDCDC");
	public static BBColor Lightgray => new("#D3D3D3");
	public static BBColor Silver => new("#C0C0C0");
	public static BBColor Darkgray => new("#A9A9A9");
	public static BBColor Dimgray => new("#696969");
	public static BBColor Gray => new("#808080");
	public static BBColor Lightslategray => new("#778899");
	public static BBColor Slategray => new("#708090");
	public static BBColor Darkslategray => new("#2F4F4F");
	public static BBColor Black => new("#000000");

	private readonly string Value;

	public BBColor(string value)
	{
		Value = value;
	}

	public string Apply(string text)
	{
		return $"[color={Value}]{text}[/color]";
	}
}
