using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Player;

public partial class Emotes : AnimatedSprite2D
{
	public override void _Ready()
	{
		TimeController.Instance.NightOccurred += OnNightOccurred;
		AnimationFinished += OnAnimationFinished;
		Visible = false;
	}

	private void OnNightOccurred()
	{
		Visible = true;
		Play("tired");
	}

	private void OnAnimationFinished()
	{
		Visible = false;
	}
}
