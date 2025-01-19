using Godot;
using System;
using System.Threading.Tasks;
using untitledplantgame.Common;
using untitledplantgame.Common.GameStates;
using untitledplantgame.Scenes;

public partial class BlurController : Node
{
	public static BlurController Instance { get; private set; }
	
	public event Action BlurEnabled;
	public event Action BlurDisabled;

	[Export(PropertyHint.Range, "0.0,5.0")] private float _strength;
	[Export(PropertyHint.Range,"0.0,2.0")] private float _transitionDuration;
	
	[ExportGroup("Setup")]
	[Export] private BlurEffect _blurEffect;

	private float _currentLod;
	private Tween _tween; // Tween for strength transition
	private Logger _logger;

	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}

		_logger = new Logger(this);
		Instance = this;
		
		GameStateMachine.Instance.StateChanged += OnGameStateChanged;
	}

	public override void _ExitTree()
	{
		GameStateMachine.Instance.StateChanged -= OnGameStateChanged;
		Instance = null;
	}

	private void OnGameStateChanged(GameState prev, GameState next)
	{
		if (next != GameState.FreeRoam)
		{
			_logger.Debug("Enable blur");
			EnableBlur();
		}
		else
		{
			_logger.Info("Disable blur");
			DisableBlur();
		}
	}

	// Currently no public use.
	public void EnableBlur()
	{
		_tween?.Stop();
		
		_tween = CreateTween();
		_tween.TweenMethod(Callable.From<float>(_blurEffect.SetStrength), _blurEffect.Strength, _strength, _transitionDuration);
		_tween.Finished += () => BlurEnabled?.Invoke();
	}

	// Currently no public use.
	public void DisableBlur()
	{
		_tween?.Stop();

		_tween = CreateTween();
		_tween.TweenMethod(Callable.From<float>(_blurEffect.SetStrength), _blurEffect.Strength, 0, _transitionDuration);
		_tween.Finished += () => BlurDisabled?.Invoke();
	}
	
	public async Task EnableBlurAsync()
	{
		_tween?.Stop();
		
		_tween = CreateTween();
		_tween.TweenMethod(Callable.From<float>(_blurEffect.SetStrength), _blurEffect.Strength, _strength, _transitionDuration);
		await ToSignal(_tween, "finished");
		BlurEnabled?.Invoke();
	}
	
	public async Task DisableBlurAsync()
	{
		_tween?.Stop();
		_tween?.Free();

		_tween = CreateTween();
		_tween.TweenMethod(Callable.From<float>(_blurEffect.SetStrength), _blurEffect.Strength, 0, _transitionDuration);
		await ToSignal(_tween, "finished");
		BlurDisabled?.Invoke();
	}
}
