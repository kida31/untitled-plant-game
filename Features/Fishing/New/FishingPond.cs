
using System;
using Godot;

namespace untitledplantgame.Fishing.New;

public partial class FishingPond: Area2D
{
    [Export] private PackedScene _fishPrefab;
    [Export] private Marker2D _spawnAreaA;
    [Export] private Marker2D _spawnAreaB;

    /// <summary>
    ///     Spawns a fish in a random area
    /// </summary>
    public Fish SpawnFish(float speed, float speedOpposite, Vector2 direction) {
        var fish = _fishPrefab.Instantiate<Fish>();
        fish.Initialize(speed, speedOpposite, direction);

        var pos = Vector2.Zero;
        pos.X = (float) GD.RandRange(_spawnAreaA.GlobalPosition.X, _spawnAreaB.GlobalPosition.X);
        pos.Y = (float) GD.RandRange(_spawnAreaA.GlobalPosition.Y, _spawnAreaB.GlobalPosition.Y);

        AddChild(fish);
        fish.GlobalPosition = pos;   
        return fish;     
    }
}