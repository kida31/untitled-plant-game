using Godot;
using untitledplantgame.Common;

namespace untitledplantgame;

public partial class BgmArea2D : Area2D, IBgmArea
{
	[Export] private AudioStream _bgm;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;
		GD.Print("Resource:", _bgm.ResourceName);
	}

	public AudioStream GetBgm()
	{
		return _bgm;
	}

	private void OnAreaEntered(Area2D area)
	{
		// Player is not an Area currently
		// if (area is Player.Player)
		// {
		// 	EventBus.Instance.BgmAreaChanged(this);
		// }
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player.Player)
		{
			EventBus.Instance.InvokeBgmAreaChanged(this);
		}
	}
}
