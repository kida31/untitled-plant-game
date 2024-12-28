using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class SoilMap : Node2D
{
	private const int TileSetSourceId = 2;
	
	private TileMapLayer _tileLayer;
	private Logger _logger;
	
	private enum SoilHydration
	{
		Dry = 50,
		Moist = 150,
		Wet = 250,
		Flooded
	}
	
	private Dictionary<SoilHydration, Vector2I> _hydrationTileMap = new()
	{
		{SoilHydration.Dry, new Vector2I(0, 1)},
		{SoilHydration.Moist, new Vector2I(0, 2)},
		{SoilHydration.Wet, new Vector2I(0, 3)},
		{SoilHydration.Flooded, new Vector2I(0, 4)}
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
			_logger.Debug(nameof(t));
			t.HydrationChanged += OnHydrationChanged;
		}
	}

	private void OnHydrationChanged(float hydration, SoilTile tile)
	{
		var tilePosition = tile.GlobalPosition;
		var tileMapPosition = _tileLayer.LocalToMap(tilePosition);
		switch (hydration)
		{
			case < (int) SoilHydration.Dry:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Dry]);
				_logger.Debug($"Tile with hydration {hydration} is dry");
				break;
			case < (int) SoilHydration.Moist:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Moist]);
				_logger.Debug($"Tile with hydration {hydration} is moist");
				break;
			case < (int) SoilHydration.Wet:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Wet]);
				_logger.Debug($"Tile with hydration {hydration} is wet");
				break;
			default:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, _hydrationTileMap[SoilHydration.Flooded]);
				_logger.Debug($"Tile with hydration {hydration} is flooded");
				break;
		}
	}

	public override void _Process(double delta)
	{
		
	}
}
