using System.Linq;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.Plants;
using untitledplantgame.Player;
using untitledplantgame.Tools;

public class Shears : Tool
{
    private Logger _logger;
	public Shears(float radius, float range) : base(radius, range)
	{
        _logger = new("Shears");
	}

	protected override bool OnHit(Player user, Node2D[] hits)
	{
        var plant = hits.OfType<APlant>().FirstOrDefault();
        if (plant == null) {
            return false;
        }

        plant.Harvest();
        GD.Print("...pipiab");
        // TODO: move result to player inventory
        return true;
	}

	protected override void OnMiss(Player user)
	{
		// pass
	}

	protected override void OnUse(Player user)
	{
		GD.Print("Schnippschnapp...");
	}
}