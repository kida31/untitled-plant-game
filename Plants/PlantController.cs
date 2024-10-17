using Godot;
using untitledplantgame.Cycle;

namespace untitledplantgame.Plants;

public partial class PlantController : Node
{
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
        foreach (var node in plantNodes)
        {
            var plant = node as APlant;
            plant?.CheckRequirements();
        }
    }
}