using System;
using Godot;
using untitledplantgame.Common;

public partial class TestCycleWithPlants : Node2D
{
	[Export]
	private Button _skipButton;

	private TimeController _timeController;

	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		_timeController = TimeController.Instance;
		_skipButton.Pressed += OnSkipButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta) { }

	void OnSkipButtonPressed()
	{
		_timeController.GoToNextDay();
	}
}
