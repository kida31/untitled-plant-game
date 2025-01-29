
using System;
using Godot;

namespace untitledplantgame.Fishing.New;

public partial class FishingPond: Area2D
{
    [Export] private PackedScene _fishPrefab;
    [Export] private Marker2D _spawnPoint;

    /// <summary>
    ///     Spawns a fish in a random area
    /// </summary>
    public Fish SpawnFish(float speed, float speedOpposite, Vector2 direction) {
        var fish = _fishPrefab.Instantiate<Fish>();
        fish.Initialize(speed, speedOpposite, direction);

        AddChild(fish);
        fish.GlobalPosition = _spawnPoint.GlobalPosition;
        return fish;     
    }
}