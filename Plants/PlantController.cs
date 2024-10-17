using Godot;
using untitledplantgame.Common;
using untitledplantgame.Cycle;

namespace untitledplantgame.Plants;

public partial class PlantController : Node
{
    private readonly Logger _logger = new Logger("PlantController");
    private TimeController _timeController;

    public override void _Ready()
    {
        _timeController = TimeController.Instance;
        _timeController.Visible = true;
        _timeController.DayChanged += CheckAllPlants;
    }

    private void CheckAllPlants(int day)
    {
        var plantNodes = GetTree().GetNodesInGroup("Plant");

        _logger.Debug($"Checking {plantNodes.Count} plants");
        foreach (var node in plantNodes)
        {
            var plant = node as APlant;
            plant?.CheckRequirements();
        }
    }
}
