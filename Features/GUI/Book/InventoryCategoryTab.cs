using Godot;
using System;
using untitledplantgame.Inventory.GUI;

[Tool]
public partial class InventoryCategoryTab : Clickable
{
	private const string ActivateAnimation = "Activate";
	// public event Action Pressed;
	
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
				SetIsActive(value, true);
			}
			else
			{
				SetIsActive(value);	
			}
		}
	}
	
	[ExportGroup("Hidden")]
	[Export] private TextureRect _activeIcon;
	[Export] private TextureRect _inactiveIcon;
	[Export] private Label _textLabel;
	[Export] private AnimationPlayer _animationPlayer;

	private bool _isActive;

	public void SetIsActive(bool value, bool instantAnimation = false)
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
