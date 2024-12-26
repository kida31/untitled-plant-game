using Godot;

public partial class Settings : Node
{
	[Export] private Button _backButton;
	[Export] private CheckBox _checkBox;
	[Export] private ResolutionButton _resolutionButton; // This should be 100% custom logic to handle the elements! Something basic for resolution

	private long _lastValidOptionButton;
	private Vector2I _gameResolution;
	
	public override void _Ready()
	{
		_backButton.Pressed += () => GetTree().ChangeSceneToFile("res://Features/GUI/HUDs/MainMenu/MainMenu.tscn");
		_resolutionButton.ItemSelected += GetSelectedItem;
		_checkBox.Toggled += FullScreenToggled;
	}

	private void FullScreenToggled(bool fullScreen)
	{
		if (fullScreen)
		{
			_resolutionButton.Select(-1);
		}
		else
		{
			_resolutionButton.Select((int) _lastValidOptionButton);
		}
		
		DisplayServer.WindowSetMode(fullScreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
	}

	private void GetSelectedItem(long buttonIndex)
	{
		_lastValidOptionButton = buttonIndex;
		SetGameResolutionFromOptionButtonText(buttonIndex);
		SetGameResolution();
	}

	private void SetGameResolutionFromOptionButtonText(long buttonIndex)
	{
		var resolution = _resolutionButton.GetItemText((int)buttonIndex).Replace("#", "");
		var parts = resolution.Split('x');
		
		_gameResolution = new Vector2I(int.Parse(parts[0]), int.Parse(parts[1]));
	}

	private void SetGameResolution()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		DisplayServer.WindowSetSize(_gameResolution);
	}
}
