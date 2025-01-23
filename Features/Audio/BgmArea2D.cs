using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Audio;

public partial class BgmArea2D : Area2D, IBgmArea
{
	[Export] private AudioStream _bgm;

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
}
