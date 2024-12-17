using Godot;
using untitledplantgame.Item;
using APlant = untitledplantgame.Plants.APlant;
using SoilTile = untitledplantgame.Plants.Soil.SoilTile;

// TODO Test ALL APlant public methods and mechanics

public partial class TestAPlant : Node2D
{
	[Export]
	private Label _label;

	[Export]
	private APlant _plant;
	[Export]
	private APlant _plant2;

	[Export]
	private SoilTile _soilTile;
	[Export]
	private SoilTile _soilTile2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var waterSoilButton = GetNode<Button>("VBoxContainer/Button");
		waterSoilButton.Pressed += OnWaterSoilButtonPressed;
		var harvestButton = GetNode<Button>("VBoxContainer/Button2");
		harvestButton.Pressed += OnHarvestButtonPressed;
		_label = GetNode<Label>("VBoxContainer/Label");

		_plant.PlantOnTile(_soilTile);
		_plant2.PlantOnTile(_soilTile2);
	}
	public override void _Process(double delta)
	{
		if (_label == null)
			return;

		_label.Text = $"First Plant: Stage {_plant.Stage} with Tile Hydration: {_soilTile.Hydration} \n" +
		              $"Second Plant: Stage {_plant2.Stage} with Tile Hydration: {_soilTile2.Hydration}";
	}

	private void OnHarvestButtonPressed()
	{
		var item = _plant.Harvest();
		InteractableItem interact = new InteractableItem(item);
		//TODO might need to add it somewhere else, not root
		//How can this be removed from root when loading new scenes?
		//Do we save if somewhere so it appears again?
		GetTree().Root.AddChild(interact);
		interact.GlobalPosition = _plant.GlobalPosition;
	}

	private void OnWaterSoilButtonPressed()
	{
		_soilTile.AddWater(200);
		_soilTile2.AddWater(200);
	}
}
