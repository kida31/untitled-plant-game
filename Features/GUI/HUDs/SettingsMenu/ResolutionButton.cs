using System.Text.RegularExpressions;
using System.Collections.Generic;
using Godot;

namespace untitledplantgame.GUI.HUDs.SettingsMenu;

public partial class ResolutionButton : OptionButton
{
	private List<Vector2> _resolution;

	public override void _Ready()
	{
		//GetPopup().MaxSize = new Vector2I((int)Size.X * 5, 160); //Size.X = 160, BUT (int) Size.X = 32 ...? WHAT???
		GetPopup().MaxSize = new Vector2I(160, 160);
		FillResolutionList();
		CreateResolutions();
	}

	private void FillResolutionList()
	{
		_resolution = new List<Vector2>
		{
			new(640, 480), // 4:3
			new(720, 576), // 5:4
			new(800, 450), // 16:9
			new(800, 480), // 5:3
			new(800, 600), // 4:3
			new(960, 540), // 16:9
			new(1024, 768), // 4:3
			new(1152, 864), // 4:3
			new(1280, 720), // 16:9
			new(1280, 768), // 5:3
			new(1280, 800), // 16:10
			new(1280, 960), // 4:3
			new(1280, 1024), // 5:4
			new(1366, 768), // 16:9
			new(1400, 1050), // 4:3
			new(1440, 900), // 16:10
			new(1600, 900), // 16:9
			new(1600, 1200), // 4:3
			new(1680, 1050), // 16:10
			new(1920, 1080), // 16:9
			new(1920, 1152), // 5:3
			new(1920, 1200), // 16:10
			new(2048, 1080), // 17:9
			new(2560, 1080), // 21:9
			new(2560, 1440), // 16:9
			new(2560, 1600), // 16:10
			new(2560, 2048), // 5:4
			new(3440, 1440), // 21:9
			new(3840, 1600), // 21:9
			new(3840, 2160), // 16:9
			new(5120, 2160), // 21:9
			new(7680, 4320), // 16:9
		};
	}
	
	private string ConvertVectorToResolutionString(Vector2 vector2)
	{
		return Regex.Replace(vector2.ToString(), @"[()\s]", "").Replace(",", "x");
	}

	private void CreateResolutions()
	{
		for (int i = 0; i < _resolution.Count; i++)
		{
			AddItem(ConvertVectorToResolutionString(_resolution[i]), i);
		}
	}
}
