using System;
using Godot;
using untitledplantgame.Plants;

// TODO Test ALL APlant public methods and mechanics

public partial class TestAPlant : Node2D
{
    [Export] private Button _sunButton;
    [Export] private Button _waterButton;
    [Export] private Label _label;
    
    [Export] private APlant _plant;

    [Export] private SoilTile _soilTile;
    
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sunButton.Pressed += OnSunButtonPressed;
        _waterButton.Pressed += OnWaterButtonPressed;
        var waterSoilButton = GetNode<Button>("VBoxContainer/Button");
        waterSoilButton.Pressed += OnWaterSoilButtonPressed;
        
        _plant.PlantOnTile(_soilTile);
        
    }

    private void OnWaterSoilButtonPressed()
    {
        _soilTile.WaterTile(100);
    }

    private void OnGrowButtonPressed()
    {
        GD.Print("Grow!");
        _plant.CheckRequirements();
    }

    private void OnWaterButtonPressed()
    {
        GD.Print("Water!");
        _plant.AbsorbWaterFromTile();
    }

    private void OnSunButtonPressed()
    {
        GD.Print("Sun!");
        _plant.AbsorbSun();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_label != null)
        {
            _label.Text = _plant.Stage + "\n";
            _label.Text += "Tile Hydration: " + _soilTile.Hydration;
        }
    }
}