using Godot;
using System;

public partial class FishingGame : Node2D
{
	private static int _progressPerPull = 5;
	private static int _progressDecayPerSecond = 5;
	[Export] private ProgressBar _progressBar;
	[Export] private Hookbar _hookbar;
	[Export] private Fish _fish;
	[Export] private Label _textLabel;
	[Export] private Button _restartButton;

	[ExportCategory("Config")] [Export] private LineEdit _fishSpeed;
	[Export] private LineEdit _hookedFishSpeedModifier;
	[Export] private LineEdit _pullDistance;
	[Export] private LineEdit _hookDistanceMod;
	[Export] private LineEdit _hookDistance;
	[Export] private LineEdit _progressDecayPerSecondText;
	[Export] private LineEdit _progressPerPullText;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hookbar.PulledFish += OnFishPulled;
		_progressBar.ValueChanged += OnProgressBarValueChanged;
		_restartButton.Pressed += RestartGame;

		_fishSpeed.Text = $"{_fish.BaseSpeed}";
		_fishSpeed.TextChanged += (text) => _fish.BaseSpeed = float.Parse(text);
		_hookedFishSpeedModifier.Text = $"{Fish.HookSpeedMod}";
		_hookedFishSpeedModifier.TextChanged += (text) => Fish.HookSpeedMod = float.Parse(text);
		_pullDistance.Text = $"{_hookbar.PullDistance}";
		_pullDistance.TextChanged += (text) => _hookbar.PullDistance = float.Parse(text);
		_hookDistanceMod.Text = $"{_hookbar.HookIsHookedDistanceMod}";
		_hookDistanceMod.TextChanged += (text) => _hookbar.HookIsHookedDistanceMod = float.Parse(text);
		_hookDistance.Text = $"{_hookbar.StepDistance}";
		_hookDistance.TextChanged += (text) => _hookbar.StepDistance = float.Parse(text);
		_progressDecayPerSecondText.Text = $"{_progressDecayPerSecond}";
		_progressDecayPerSecondText.TextChanged += (text) => _progressDecayPerSecond = int.Parse(text);
		_progressPerPullText.Text = $"{_progressPerPull}";
		_progressPerPullText.TextChanged += (text) => _progressPerPull = int.Parse(text);
		
	}

	private void OnFishPulled()
	{
		_progressBar.Value += _progressPerPull;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_progressBar.Value -= _progressDecayPerSecond * (float)delta;
	}

	private void OnProgressBarValueChanged(double value)
	{
		if (value >= _progressBar.MaxValue)
		{
			GD.Print("You caught a fish!");
			_progressBar.Value = _progressBar.MaxValue;
			_fish.QueueFree();
			_hookbar.QueueFree();
			_textLabel.Show();
SetProcess(false);		
		}
	}

	private void RestartGame()
	{
		GetTree().ReloadCurrentScene();
		SetProcess(true);
	}
}
