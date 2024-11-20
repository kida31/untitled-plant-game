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
	private SoilTile _soilTile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var waterSoilButton = GetNode<Button>("VBoxContainer/Button");
		waterSoilButton.Pressed += OnWaterSoilButtonPressed;
		var harvestButton = GetNode<Button>("VBoxContainer/Button2");
		harvestButton.Pressed += OnHarvestButtonPressed;
		_label = GetNode<Label>("VBoxContainer/Label");
		GD.Print(_label.Name);
		_soilTile = GetNode<SoilTile>("Soil");
		GD.Print(_soilTile.Name);
		_plant = GetNode<APlant>("APlantPrefab");
		GD.Print(_plant.Name);

		_plant.PlantOnTile(_soilTile);
	}

	private void OnHarvestButtonPressed()
	{
		_plant.Harvest();
	}

	private void OnWaterSoilButtonPressed()
	{
		_soilTile.WaterSoilTile(200);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_label == null)
			return;

		_label.Text = $"Current Stage {_plant.Stage} \n with Tile Hydration: {_soilTile.Hydration}";
	}
}
