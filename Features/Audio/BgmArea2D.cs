using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Audio;

/// <summary>
///		Area2D that emits an event when the player enters it. The event contains the BGM to play.
/// </summary>
public partial class BgmArea2D : Area2D, IBgmArea
{
	[Export] private AudioStream _bgm;
	[Export] private Location _location; // TODO: _bgm and _location overlap in responsibility

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;
	}

	public AudioStream GetBgm()
	{
		return _bgm;
	}

	private void OnAreaEntered(Area2D area)
	{
		// Player is not an Area
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player.Player)
		{
			EventBus.Instance.InvokeBgmAreaChanged(this);
		}
	}

	public Location GetLocation()
	{
		return _location;
	}
}
