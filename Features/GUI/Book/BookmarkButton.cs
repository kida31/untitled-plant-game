using Godot;

/// <summary>
///     This GUI Component represents a bookmark button.
///     It can be clicked and toggles between active and inactive state.
///     The state is represented by two textures.
/// </summary>
[Tool]
public partial class BookmarkButton : Clickable
{
	private bool _active;

	[ExportGroup("Hidden")] [Export] private NinePatchRect _rectActive;

	[Export] private NinePatchRect _rectInactive;

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
