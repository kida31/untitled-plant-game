using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Player;

public partial class Bed : Area2D
{
	public string ActionName => "Sleep";
	private bool _playerIsInBed = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (_playerIsInBed)
		{
			TimeController.Instance.FastForwardFor(60);
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			_playerIsInBed = false;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			_playerIsInBed = true;
		}
	}
}
