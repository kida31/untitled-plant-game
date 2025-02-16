using System;

namespace untitledplantgame.Common;

/// <summary>
///		Images repackaged for use in BBCode.
///		Interpolate or append to strings to include in BBCode.
///		Make sure the object is a RichTextLabel and the BBCode is enabled.
/// </summary>
[Obsolete("(Probably) Godot RichtTextLabel has own methods for handling images. This class might not be needed")]
public class BbImage
{
	public static BbImage Coin => new("res://Assets/UI/Book/Icons/mini_coin.png");

	public string Path { get; }
	public int X { get; set; }
	public int Y { get; set; }

	private BbImage(string path)
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
}
