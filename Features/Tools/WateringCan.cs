using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants.Soil;
using untitledplantgame.Player;
using untitledplantgame.Tools;

public class WateringCan : Tool
{
    // Placeholders
    private const int PlaceholderWateringAmount = 20;
    private const float WateringRadius = 16;
    private const float WateringRange = 16; // TODO: Fix; Direction of player is only active while moving instead of most recent facing
    // EndPlaceholders

    private readonly Logger _logger;
    public WateringCan() : this(WateringRadius, WateringRange)
    {
    }

    public WateringCan(float radius, float range) : base(radius, range)
    {
        _logger = new Logger("WateringCan");
    }

    protected override bool OnHit(Player user, Node2D[] hits)
    {
        var soil = hits.OfType<SoilTile>().FirstOrDefault();
        if (soil == null)
        {
            return false;
        }

        _logger.Debug("Watering soil");
        soil.WaterSoilTile(PlaceholderWateringAmount);
        return true;
    }

    protected override void OnMiss(Player user)
    {
        _logger.Debug("Did not water anything. Waste of water");
    }

    protected override void OnUse(Player user)
    {
        // Something. Example reduce water in tool
    }
}
