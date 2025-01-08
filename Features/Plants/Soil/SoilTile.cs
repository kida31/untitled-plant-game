using System;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Plants.Soil;

public partial class SoilTile : Area2D, IWaterable
{
	[Export]
	public float Hydration { get; private set; }
	private float _maxHydration = 200;
	private float Fertilization { get; set; }
	private Logger _logger = new Logger("SoilTile");
	private Vector2 _originalPosition;

	public float WithdrawHydration(float reductionValue)
	{
		var prevHydration = Hydration;
		Hydration = Math.Clamp(Hydration - reductionValue, 0, Hydration);

		return prevHydration - Hydration;
	}

	public void AddWater(float addedWater)
	{
		Hydration = Math.Min(Hydration + addedWater, _maxHydration);
	}

	//Do we want this?
	public void EvaporateWater()
	{
		Hydration--;
	}

	public override void _Ready()
	{
		_originalPosition = GlobalPosition;
		EventBus.Instance.OnSceneChange += OnSceneChange;
		_logger.Debug("Original position: " + _originalPosition);	
	}

	public override void _ExitTree()
	{
		EventBus.Instance.OnSceneChange -= OnSceneChange;
	}

    private void OnSceneChange(Node from, Node to)
    {
		if (from.Name == "GardenMapScene")
		{
			GlobalPosition += new Vector2(10, -10);
			_logger.Debug("first, Move to " + GlobalPosition);

		}
		if (to.Name == "GardenMapScene")
		{
			GlobalPosition += _originalPosition;
			_logger.Debug("second, Move to " + _originalPosition);
		}
        _logger.Debug("Scene changed from " + from.Name + " to " + to.Name);

    }

}
