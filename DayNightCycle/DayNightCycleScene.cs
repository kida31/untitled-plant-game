using Godot;
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

		var dayNightCycle = (TimeController) _canvasModulate;
		dayNightCycle.TimeTick += _ui.SetDaytime; //canvas_modulate.time_tick.connect(ui.set_daytime)

		/*
		 TODO: figure out how to connect the signal
		 gd code:
		   canvas_modulate.time_tick.connect(sound_machine.set_daytime)

		maybe c# code:
		_ui.SetDaytime(DayNightCycle.TimeTickEventHandler);
		_canvasModulate.Connect("TimeTick", this, nameof(DayNightCycle.TimeTickEventHandler));
		**/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
