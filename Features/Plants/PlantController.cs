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
		_logger = new Logger(this);
	}

	private void DayPassed(int day)
	{
		_logger.Debug($"Updating plants. Day {day}");
		var plantNodes = GetPlantNodes();

		LetPlantsGrow(plantNodes);
	}

	private void LetPlantsGrow(Array<Node> plantNodes)
	{
		_logger.Debug($"Checking {plantNodes.Count} plant(s)");
		foreach (var node in plantNodes)
		{
			var plant = node as APlant;
			plant?.DoGrowthCycle();
		}
	}

	private Array<Node> GetPlantNodes()
	{
		return GetTree().GetNodesInGroup(GameGroup.Plants);
	}
}
