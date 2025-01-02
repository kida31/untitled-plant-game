using System;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class SoilMap : Node2D
{
	private const int TileSetSourceId = 0;

	[Export] private Node2D _tester;
	private TileMapLayer _tileLayer;
	private Logger _logger;
	
	private enum SoilHydration
	{
		Dry = 50,
		Moist = 150,
		Wet = 250,
		Flooded
	}
	
	private Dictionary<SoilHydration, Array<Vector2I>> _hydrationTileMap = new()
	{
		{SoilHydration.Dry, new Array<Vector2I>{new Vector2I(0, 0)}},
		{SoilHydration.Moist, new Array<Vector2I>{new Vector2I(1, 0), new Vector2I(1, 1), new Vector2I(1, 2), new Vector2I(1, 3), new Vector2I(1, 4), new Vector2I(1, 5), new Vector2I(1, 6), new Vector2I(1, 7)}},
		{SoilHydration.Wet, new Array<Vector2I>{new Vector2I(2, 0), new Vector2I(2, 1), new Vector2I(2, 2), new Vector2I(2, 3), new Vector2I(2, 4), new Vector2I(2, 5), new Vector2I(2, 6), new Vector2I(2, 7)}},
		{SoilHydration.Flooded, new Array<Vector2I>{new Vector2I(3, 0), new Vector2I(3, 1), new Vector2I(3, 2), new Vector2I(3, 3), new Vector2I(3, 4), new Vector2I(3, 5), new Vector2I(3, 6), new Vector2I(3 , 7)}}
	};
	
	public override void _Ready()
	{
		_logger = new Logger(this);
		_logger.Debug("READY");
		_tileLayer = GetNode<TileMapLayer>("Soil");
		CallDeferred(nameof(GetSoilTiles));
	}
	
	private void GetSoilTiles()
	{
		var tiles = GetTree().GetNodesInGroup(GameGroup.Soil);
		foreach (var tile in tiles)
		{
			if (tile is not SoilTile t) return;
			t.HydrationChanged += OnHydrationChanged;
			OnHydrationChanged(t.Hydration, t);
		}
	}

	private void OnHydrationChanged(float hydration, SoilTile tile)
	{
		//testing
		var pos = ToLocal(_tester.GlobalPosition);
		_logger.Debug($"Tester position: {pos.X:F1}, {pos.Y:F1}");
		var tilePos = _tileLayer.LocalToMap(pos);
		_logger.Debug($"Test map position: {tilePos}");
		_tileLayer.SetCell(tilePos, TileSetSourceId, _hydrationTileMap[SoilHydration.Dry][0]);
		//end testing
		
		
		var tilePosition = ToLocal(tile.GlobalPosition);
		_logger.Debug($"Soil Tile position: {tilePosition.X:F1}, {tilePosition.Y:F1}");
		var tileMapPosition = _tileLayer.LocalToMap(tilePosition);
		_logger.Debug($"Tile map position: {tileMapPosition}");
		var randomSoilVariation = new Random();
		
		switch (hydration)
		{
			case < (int) SoilHydration.Dry:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Dry][0]);
				_logger.Debug($"Tile with hydration {hydration} is dry");
				break;
			case < (int) SoilHydration.Moist:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Moist][randomSoilVariation.Next(8)]);
				_logger.Debug($"Tile with hydration {hydration} is moist");
				break;
			case < (int) SoilHydration.Wet:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Wet][randomSoilVariation.Next(8)]);
				_logger.Debug($"Tile with hydration {hydration} is wet");
				break;
			default:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Flooded][randomSoilVariation.Next(8)]);
				_logger.Debug($"Tile with hydration {hydration} is flooded");
				break;
		}
	}

	public override void _Process(double delta)
	{
		
	}
}
