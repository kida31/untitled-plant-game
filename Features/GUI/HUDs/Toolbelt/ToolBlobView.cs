using System;
using Godot;
using untitledplantgame.Tools;

namespace untitledplantgame.GUI.HUDs;

public partial class ToolBlobView : Control
{
	private const string ToPrimaryAnimationName = "TransitionToPrimary";

	public enum Style
	{
		Primary,
		Secondary
	}

	[Export]
	public Style BlobStyle
	{
		get => _style;
		set => SetBlobStyle(value);
	}

	[Export] private AnimationPlayer _animationPlayer;
	[Export] private TextureRect _toolIcon;
	
	[ExportGroup("Placeholder hardcoded icons")]
	[Export] private Texture2D _wateringCanIcon;
	[Export] private Texture2D _shovelIcon;
	[Export] private Texture2D _shearsIcon;
	[Export] private Texture2D _seedBagIcon;

	public Tool Tool {
		get => _tool;
		set
		{
			_tool = value;
			_toolIcon.Texture = _tool switch
			{
				// Placeholder
				WateringCan => _wateringCanIcon,
				Shears => _shearsIcon,
				SeedBag => _seedBagIcon,
				null => null,
				_ => _shovelIcon,
			};
		}
	}

	private Tool _tool;
	private Style _style = Style.Primary;

	public override void _Ready()
	{
		UpdateVisuals();
	}

	private void SetBlobStyle(Style value)
	{
		if (value == _style)
		{
			return;
		}

		_style = value;
		UpdateVisuals(1.0f);
	}

	private void UpdateVisuals(float customSpeed = Single.MaxValue)
	{
		if (_style == Style.Primary)
		{
			_animationPlayer?.Play(ToPrimaryAnimationName, customSpeed: customSpeed);
		}
		else
		{
			_animationPlayer?.Play(ToPrimaryAnimationName, customSpeed: -customSpeed, fromEnd: true);
		}
	}
}
