using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class PlantController : Node
{
    private readonly Logger _logger = new("PlantController");
    private TimeController _timeController;

    public override void _Ready()
    {
        _timeController = TimeController.Instance;
        _timeController.DayChanged += DayPassed;
    }
    
    private void DayPassed(int day)
    {
        _logger.Debug($"Updating plants. Day {day}");
        var plantNodes = GetPlantNodes();
        HydrateAllPlants(plantNodes);
        CheckAllPlants(plantNodes);
    }
    
    private void AbsorbSunlight(Array<Node> plantNodes)
    {
        _logger.Debug($"Checking {plantNodes.Count} plants");
        foreach (var node in plantNodes)
        {
            var plant = node as APlant;
            plant?.AbsorbSun();
        }
    }

    private void HydrateAllPlants(Array<Node> plantNodes)
    {
        _logger.Debug($"Checking {plantNodes.Count} plants");
        foreach (var node in plantNodes)
        {
            var plant = node as APlant;
            plant?.AbsorbWaterFromTile();
        }
    }
    
    private void CheckAllPlants(Array<Node> plantNodes)
    {
        _logger.Debug($"Checking {plantNodes.Count} plants");
        foreach (var node in plantNodes)
        {
            var plant = node as APlant;
            plant?.CheckRequirements();
        }
    }

    private Array<Node> GetPlantNodes()
    {
        return GetTree().GetNodesInGroup("Plant");
    }
}