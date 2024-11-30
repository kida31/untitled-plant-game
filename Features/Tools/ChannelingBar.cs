using Godot;
using System;

public partial class ChannelingBar : ProgressBar
{
	public event Action Completed;
	
	private Node2D _anchorPoint;
	private float _channelingTime;
	private float _currentChannelTime;

	public ChannelingBar(Node2D anchorPoint, float channelingTime)
	{
		_anchorPoint = anchorPoint;
		_channelingTime = channelingTime;
		_currentChannelTime = 0;
		
		CustomMinimumSize = new Vector2(128, 16);
		
		MinValue = 0;
		MaxValue = 1;
		Value = 0;
		Step = 0.01;
		ShowPercentage = false;
	}

	public override void _Process(double delta)
	{
		// Update functional values
		_currentChannelTime += (float)delta;
		Value = _currentChannelTime / _channelingTime;
		if (_currentChannelTime >= _channelingTime)
		{
			GD.Print("Finished casting");
			Completed?.Invoke();
			QueueFree();
		}
		
		// Update position
		var newPos = _anchorPoint.GlobalPosition;
		newPos.X = _anchorPoint.GlobalPosition.X - Size.X / 2;
		GlobalPosition = newPos;
	}
}
