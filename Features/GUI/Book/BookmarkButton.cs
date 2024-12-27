using Godot;

[Tool]
public partial class BookmarkButton : Clickable
{
	[Export]
	private Texture2D _textureActive
	{
		get => _rectActive?.Texture;
		set
		{
			if (_rectActive != null)
			{
				_rectActive.Texture = value;
			}
		}
	}
	[Export]
	private Texture2D _textureInactive
	{
		get => _rectInactive?.Texture;
		set
		{
			if (_rectInactive != null)
			{
				_rectInactive.Texture = value;
			}
		}
	}

	[Export]
	public bool Active
	{
		get => _active;
		set
		{
			_active = value;
			if (_active)
			{
				TransitionIn();
			}
			else
			{
				TransitionOut();
			}
		}
	}

	[ExportGroup("Hidden")]
	[Export] private NinePatchRect _rectActive;
	[Export] private NinePatchRect _rectInactive;

	private bool _active;

	private void TransitionIn()
	{
		_rectActive?.Show();
		_rectInactive?.Hide();
	}

	private void TransitionOut()
	{
		_rectActive?.Hide();
		_rectInactive?.Show();
	}
}
