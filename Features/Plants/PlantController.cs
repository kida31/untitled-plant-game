using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class PlantController : Node
{
	private Logger _logger;
	private TimeController _timeController;

	public override void _Ready()
	{
		_timeController = TimeController.Instance;
		_timeController.DayChanged += DayPassed;
		_timeController.NoonOccured += NoonOccured;
		_logger = new Logger(this);
	}

	private void NoonOccured()
	{
		_logger.Debug("Noon occured. Updating plants.");
		LetPlantsGrow();
	}

	private void DayPassed(int day)
	{
		_logger.Debug($"Updating plants. Day {day}");
		LetPlantsGrow();
	}

	private void LetPlantsGrow()
	{
		var plantNodes = GetPlantNodes();
		_logger.Debug($"Checking {plantNodes.Count} plant(s)");
		foreach (var node in plantNodes)
		{
			var plant = node as Plant;
			plant?.DoGrowthCycle();
		}
	}

	private Array<Node> GetPlantNodes()
	{
		return GetTree().GetNodesInGroup(GameGroup.Plants);
	}
}
