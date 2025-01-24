using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.GUI.HUDs;

public partial class ToolbeltView : Control
{
	private const string RotateRightAnimationName = "RotateRight";
	private const string RotateLeftAnimationName = "RotateLeft";

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
		placeholderBecauseImTooStupidToDoProperDependencies.WaitTime = .1f;
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
			UpdateToolBlobs();
		};
		AddChild(placeholderBecauseImTooStupidToDoProperDependencies);

		_animationPlayer.AnimationFinished += OnAnimationEnded;
	}

	private void AnimateToolChange(bool rotateLeft)
	{
		_animationPlayer.Play(rotateLeft ? RotateLeftAnimationName : RotateRightAnimationName);
	}

	private void OnAnimationEnded(object ignored)
	{
		// FIXME This is a hack to reset the animation 
		_animationPlayer.Play("RESET");
		/* await */
		ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);
		UpdateToolBlobs();
	}

	private void UpdateToolBlobs()
	{
		_leftBlob.Tool = _player.Toolbelt.LeftTool;
		_centerBlob.Tool = _player.Toolbelt.CurrentTool;
		_rightBlob.Tool = _player.Toolbelt.RightTool;
	}
}
