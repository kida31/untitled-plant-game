using System;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class SoilMap : TileMapLayer
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
		{SoilHydration.Dry, new Array<Vector2I>{new (0, 0)}},
		{SoilHydration.Moist, new Array<Vector2I>{new (1, 0), new (1, 1), new (1, 2), new (1, 3), new (1, 4), new (1, 5), new (1, 6), new (1, 7)}},
		{SoilHydration.Wet, new Array<Vector2I>{new (2, 0), new (2, 1), new (2, 2), new (2, 3), new (2, 4), new (2, 5), new (2, 6), new (2, 7)}},
		{SoilHydration.Flooded, new Array<Vector2I>{new (3, 0), new (3, 1), new (3, 2), new (3, 3), new (3, 4), new (3, 5), new (3, 6), new (3 , 7)}}
	};
	
	public override void _Ready()
	{
		_logger = new Logger(this);
		_logger.Debug("READY");
		_tileLayer = this;
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
		/*//testing
		//use the _tester node to test the positioning of the tiles
		//you can use it to see where global positions are and how they translate to local positions on the tile map
		//drag the _tester node around the scene and see if the tile map updates on the same position as the _tester node
		 
		var pos = ToLocal(_tester.GlobalPosition);
		_logger.Debug($"Tester position: {pos.X:F1}, {pos.Y:F1}");
		var tilePos = _tileLayer.LocalToMap(pos);
		_logger.Debug($"Test map position: {tilePos}");
		_tileLayer.SetCell(tilePos, TileSetSourceId, _hydrationTileMap[SoilHydration.Dry][0]);
		//end testing*/
		
		var tilePosition = ToLocal(tile.GlobalPosition);
		var tileMapPosition = _tileLayer.LocalToMap(tilePosition);
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
}
