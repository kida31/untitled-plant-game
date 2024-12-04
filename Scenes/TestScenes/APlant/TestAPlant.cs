using Godot;
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
		_soilTile = GetNode<SoilTile>("Soil");
		_plant = GetNode<APlant>("APlantPrefab");

		_plant.PlantOnTile(_soilTile);
		_plant2.PlantOnTile(_soilTile2);
	}
	public override void _Process(double delta)
	{
		if (_label == null)
			return;

		_label.Text = $"Current Stage {_plant.Stage} \n with Tile Hydration: {_soilTile.Hydration}";
	}

	private void OnHarvestButtonPressed()
	{
		_plant.Harvest();
	}

	private void OnWaterSoilButtonPressed()
	{
		_soilTile.AddWater(200);
	}
}
