namespace untitledplantgame.Common;

public class BbImage
{
	public static string Coin => new("res://res://Assets/UI/Book/Icons/CoinIcon.png");

	public string Path { get; }
	public int X { get; set; }
	public int Y { get; set; }

	public BbImage(string path)
	{
		Path = path;
	}

	public override string ToString()
	{
		if (X > 0 && Y > 0)
		{
			return $"[img={X},{Y}]{Path}[/img]";
		}

		return $"[img]{Path}[/img]";
	}
}
