using Godot;
using Godot.Collections;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class SoilMap : Node2D
{
	private const int TileSetSourceId = 2;

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
	
	private Dictionary<SoilHydration, Vector2I> _hydrationTileMap = new()
	{
		{SoilHydration.Dry, new Vector2I(1, 0)},
		{SoilHydration.Moist, new Vector2I(2, 0)},
		{SoilHydration.Wet, new Vector2I(3, 0)},
		{SoilHydration.Flooded, new Vector2I(4, 0)}
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
		_tileLayer.SetCell(tilePos, TileSetSourceId, _hydrationTileMap[SoilHydration.Dry]);
		//end testing
		
		
		var tilePosition = ToLocal(tile.GlobalPosition);
		_logger.Debug($"Soil Tile position: {tilePosition.X:F1}, {tilePosition.Y:F1}");
		var tileMapPosition = _tileLayer.LocalToMap(tilePosition);
		_logger.Debug($"Tile map position: {tileMapPosition}");
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
