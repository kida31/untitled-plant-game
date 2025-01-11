using System;
using Godot;

namespace untitledplantgame.Player;

public partial class EmotionPopup : AnimatedSprite2D
{
	private AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public enum PlayerEmotion
	{
		Sweat,
		Angry,
		Excited,
	}
	
	public void ShowEmotion(PlayerEmotion emotion)
	{
		// Set the animation based on the emotion
		switch (emotion)
		{
			case PlayerEmotion.Sweat:
				Play("sweat");
				break;
			case PlayerEmotion.Angry:
				Play("angry");
				break;
			case PlayerEmotion.Excited:
				Play("excited");
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(emotion), emotion, null);
		}
		_animationPlayer.Play();
	}
}
