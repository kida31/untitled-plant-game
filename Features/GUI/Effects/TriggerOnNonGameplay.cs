using Godot;
using untitledplantgame.Common.ExtensionMethods;
using untitledplantgame.Common.GameStates;

public partial class TriggerOnNonGameplay : Node
{
	[Export(PropertyHint.Range, "0.0,1.0")] private float _fadeInDuration = 0.2f;
	[Export(PropertyHint.Range, "0.0,1.0")] private float _fadeOutDuration = 0.2f;

	private Control _parent;
	private Tween _tween;

	public override void _Ready()
	{
		_parent = GetParent<Control>();

		GameStateMachine.Instance.StateChanged += OnStateChanged;
	}

	private void OnStateChanged(GameState prev, GameState next)
	{
		_tween?.Stop();

		if (next != GameState.FreeRoam)
		{
			_parent.Show();
			_tween = _parent.FadeIn(_fadeInDuration);
		}
		else
		{
			_tween = _parent.FadeOut(_fadeOutDuration);
			ToSignal(_tween, Tween.SignalName.Finished).OnCompleted(_parent.Hide);
		}
	}
}
