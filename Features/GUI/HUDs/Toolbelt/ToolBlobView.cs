using Godot;
using System;

public partial class ToolBlobView : MarginContainer
{
	private const string ToPrimaryAnimationName = "TransitionToPrimary";

	public enum Style
	{
		Primary,
		Secondary
	}

	[Export] private AnimationPlayer _animationPlayer;

	[Export]
	public Style BlobStyle
	{
		get => _style;
		set => SetBlobStyle(value);
	}

	[Export]
	public Style InstantBlobStyle
	{
		get => _style;
		set => SetBlobStyle(value, customSpeed: Single.MaxValue);
	}

	private Style _style = Style.Primary;

	private void SetBlobStyle(Style value, float customSpeed = 1.0f)
	{
		if (value == _style)
		{
			return;
		}

		_style = value;

		if (_style == Style.Primary)
		{
			_animationPlayer.Play(ToPrimaryAnimationName, customSpeed: customSpeed);
		}
		else
		{
			_animationPlayer.Play(ToPrimaryAnimationName, customSpeed: -customSpeed, fromEnd: true);
		}
	}
}
