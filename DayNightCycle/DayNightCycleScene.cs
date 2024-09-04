using Godot;
using System;
using System.Diagnostics;
using untitledplantgame.DayNightCycle.UI;

namespace untitledplantgame.DayNightCycle;

public partial class DayNightCycleScene : Node
{
	private CanvasLayer _canvasLayer;
	private CanvasModulate _canvasModulate;
	private DayNightCycleUi _ui;
	//private SoundMachine _soundmachine;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_canvasLayer = GetNode<CanvasLayer>("Sprite2D/CanvasLayer");
		_canvasModulate = GetNode<CanvasModulate>("Sprite2D/CanvasModulate");
		_ui = GetNode<DayNightCycleUi>("Sprite2D/CanvasLayer/DayNightCycleUI");

		_canvasLayer.Visible = true;
		//_canvasModulate.TimeTick()
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
