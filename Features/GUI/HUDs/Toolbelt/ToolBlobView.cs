using Godot;
using untitledplantgame.Tools;

namespace untitledplantgame.GUI.HUDs;

/// <summary>
///		A circle-shaped GUI element to display a single tool.
/// </summary>
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

	public Tool Tool {
		get => _tool;
		set
		{
			_tool = value;
			_toolIcon.Texture = _tool?.Icon;
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
		UpdateVisuals();
	}

	private void UpdateVisuals()
	{
		if (_style == Style.Primary)
		{
			_animationPlayer?.Play(ToPrimaryAnimationName);
		}
		else
		{
			_animationPlayer?.PlayBackwards(ToPrimaryAnimationName);
		}
	}
}
