using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class SoilMap : Node2D
{
	private const int TileSetSourceId = 0;
	
	private TileMapLayer _tileLayer;
	private Logger _logger;

	private enum SoilHydration
	{
		Dry = 0,
		Moist = 1,
		Wet = 2,
		Flooded = 3
	}
	
	public override void _Ready()
	{
		_tileLayer = GetNode<TileMapLayer>("Soil");
		var tiles = GetTree().GetNodesInGroup(GameGroup.Soil);
		foreach (var tile in tiles)
		{
			if (tile is not SoilTile t) return;
			t.HydrationChanged += OnHydrationChanged;
		}
		_logger = new Logger(this);
		
	}

	private void OnHydrationChanged(float hydration, SoilTile tile)
	{
		var tilePosition = tile.GlobalPosition;
		var tileMapPosition = _tileLayer.LocalToMap(tilePosition);
		switch (hydration)
		{
			case < 50:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, new Vector2I(0, (int) SoilHydration.Dry));
				_logger.Debug($"Tile with hydration {hydration} is dry");
				break;
			case < 100:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, new Vector2I(0, (int) SoilHydration.Moist));
				_logger.Debug($"Tile with hydration {hydration} is moist");
				break;
			case < 200:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, new Vector2I(0, (int) SoilHydration.Wet));
				_logger.Debug($"Tile with hydration {hydration} is wet");
				break;
			default:
				_tileLayer.SetCell(tileMapPosition, TileSetSourceId, new Vector2I(0, (int) SoilHydration.Flooded));
				_logger.Debug($"Tile with hydration {hydration} is flooded");
				break;
		}
	}

	public override void _Process(double delta)
	{
		
	}
}
