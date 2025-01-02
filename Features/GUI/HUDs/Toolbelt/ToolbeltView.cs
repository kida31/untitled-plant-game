using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.HUDs;

public partial class ToolbeltView : Control
{
	public const string RotateRightAnimationName = "RotateRight";

	[Export] private AnimationPlayer _animationPlayer;
	[Export] private ToolBlobView _leftBlob;
	[Export] private ToolBlobView _centerBlob;
	[Export] private ToolBlobView _rightBlob;
	private Player.Player _player;

	public override void _Ready()
	{
		// Search for player until found
		var placeholderBecauseImTooStupidToDoProperDependencies = new Timer();
		placeholderBecauseImTooStupidToDoProperDependencies.Autostart = true;
		placeholderBecauseImTooStupidToDoProperDependencies.WaitTime = .5f;
		placeholderBecauseImTooStupidToDoProperDependencies.OneShot = false;
		placeholderBecauseImTooStupidToDoProperDependencies.Timeout += () =>
		{
			if (_player == null)
			{
				_player = GetTree().GetFirstNodeInGroup(GameGroup.Player) as Player.Player;
				if (_player == null)
				{
					return; // Try again
				}
				new Logger(this).Info("Player found");
			}
			
			_player.Toolbelt.WentToNextTool += () => AnimateToolChange(true);
			_player.Toolbelt.WentToPreviousTool += () => AnimateToolChange(false);
			placeholderBecauseImTooStupidToDoProperDependencies.Stop();
			placeholderBecauseImTooStupidToDoProperDependencies.QueueFree();
		};
		AddChild(placeholderBecauseImTooStupidToDoProperDependencies);
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.F1))
		{
			_animationPlayer.Play(RotateRightAnimationName, customSpeed: -1.0f, fromEnd: true);
		}

		if (Input.IsKeyPressed(Key.F2))
		{
			_animationPlayer.Play(RotateRightAnimationName);
		}
	}

	private void AnimateToolChange(bool right)
	{
		if (right)
		{
			_animationPlayer.Play(RotateRightAnimationName);
			_animationPlayer.AnimationFinished += OnAnimationEnded;
		}
		else
		{
			_animationPlayer.Play(RotateRightAnimationName, customSpeed: -1.0f, fromEnd: true);
			
			// When playing backwards this needs to called initially, but still looks janky sometimes
			// TODO: Fix workaround, or actually animate the second part instead of being a lazy bum
			UpdateToolBlobs();
		}
	}

	private void OnAnimationEnded(object ignored)
	{
		_animationPlayer.AnimationFinished -= OnAnimationEnded;
		UpdateToolBlobs();
	}

	private void UpdateToolBlobs()
	{
		_leftBlob.Tool = _player.Toolbelt.LeftTool;
		_centerBlob.Tool = _player.Toolbelt.CurrentTool;
		_rightBlob.Tool = _player.Toolbelt.RightTool;
	}
}
