using Godot;

namespace untitledplantgame.Common;

/// <summary>
/// Images repackaged for use in BBCode
/// </summary>
public class BbImage
{
	public static BbImage Coin => new("res://Assets/UI/Book/Icons/CoinIcon.png");

	public string Path { get; }
	public int X { get; set; }
	public int Y { get; set; }

	public BbImage(string path)
	{
		Path = path;
		// Roughly the size of our default font
		X = 8;
		Y = 8;
	}

	public override string ToString()
	{
		if (X > 0 && Y > 0)
		{
			return $"[img={X}x{Y}]{Path}[/img]";
		}

		return $"[img]{Path}[/img]";
	}

	public string AsBB()
	{
		return $"[img]{Path}[/img]";
	}
}
