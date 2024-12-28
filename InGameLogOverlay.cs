using Godot;
using System.Collections.Generic;
using untitledplantgame.Common;

public partial class InGameLogOverlay : RichTextLabel
{
	private Queue<string> _logQueue = new ();
	
	public override void _Ready()
	{
		// Subscribe to the logger's events
		Logger.MessageLogged += OnLogged;
	}

	private void OnLogged(string obj)
	{
		_logQueue.Enqueue(obj);
		if (_logQueue.Count > 8)
		{
			_logQueue.Dequeue();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = string.Join("\n", _logQueue);
	}
}
