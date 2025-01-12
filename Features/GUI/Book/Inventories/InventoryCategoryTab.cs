using Godot;

namespace untitledplantgame.GUI.Book.Inventories;

/// <summary>
///     InventoryCategoryTab is a clickable button that represents a category in the inventory.
/// </summary>
[Tool]
public partial class InventoryCategoryTab : Components.Clickable
{
	private const string ActivateAnimation = "Activate";

	[ExportGroup("Hidden")] [Export] private TextureRect _activeIcon;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private TextureRect _inactiveIcon;

	private bool _isActive;
	[Export] private Label _textLabel;

	[Export]
	public string Text
	{
		get => _textLabel.Text ?? "";
		set
		{
			if (_textLabel != null)
			{
				_textLabel.Text = value;
			}
		}
	}

	[Export]
	public bool IsActive
	{
		get => _isActive;
		private set
		{
			// if is editor, skip animation
			if (Engine.IsEditorHint())
			{
				SetIsActive(value);
			}
			else
			{
				SetIsActive(value);
			}
		}
	}

	public void SetIsActive(bool value)
	{
		if (_isActive == value)
		{
			return;
		}

		_isActive = value;

		if (_isActive)
		{
			_animationPlayer.Play(ActivateAnimation);
			// Play animation
		}
		else
		{
			_animationPlayer.PlayBackwards(ActivateAnimation);
			// Play animation
		}
	}
}
