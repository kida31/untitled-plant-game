using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Player;

/// <summary>
///		This class is responsible for updating the player's animation based on the player's direction.
/// </summary>
public partial class PlayerAnimatedSprite : AnimatedSprite2D
{
	[Export] private Player _player;

	private Vector2 FaceDirection => _player.FaceDirection;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new(this);
		if (!IsInstanceValid(_player))
		{
			_logger.Error("Player is not set.");
		}
	}

	public void UpdateAnimation(string animationName)
	{
		var animationState = animationName + "_" + GetDirectionSuffix();
		if (FaceDirection == Vector2.Left)
		{
			FlipH = true;
		}

		if (FaceDirection == Vector2.Right)
		{
			FlipH = false;
		}

		Play(animationState);
	}

	private string GetDirectionSuffix()
	{
		if (FaceDirection == Vector2.Down)
			return "down";
		if (FaceDirection == Vector2.Up)
			return "up";
		if (FaceDirection == Vector2.Left)
			return "left";
		if (FaceDirection == Vector2.Right)
			return "right";

		throw new InvalidOperationException("Invalid direction; Unreachable Code");
	}
}
