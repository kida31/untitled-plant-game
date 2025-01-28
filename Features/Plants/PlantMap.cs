using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Plants;

public partial class PlantMap : TileMapLayer
{
	private Logger _logger;
	private TileMapLayer _tileLayer;

	public override void _Ready()
	{
		_logger = new Logger(this);
		_logger.Debug("READY");
		_tileLayer = this;
		EventBus.Instance.OnSeedPlanted += OnSeedPlanted;
		EventBus.Instance.PlantHarvested += PlantHarvested;
		CallDeferred(nameof(GetPlantTiles));
	}

	private void PlantHarvested(Plant obj)
	{
		OnPlantSpriteChange(obj);
	}

	private void OnPlantSpriteChange(Plant plant)
	{
		var name = plant.PlantName;
		var pos = ToLocal(plant.GlobalPosition);
		_logger.Debug($"plant {name} at {pos}");

		var mapPos = _tileLayer.LocalToMap(pos);
		var tileSetId = GetTileSetId(name);

		if (plant.Stage == GrowthStage.Dead)
		{
			_logger.Debug($"Setting {name} at {mapPos} with tileSetId {tileSetId} and stage {plant.Stage}");
			_tileLayer.SetCell(mapPos, GetTileSetId("dead"), new Vector2I(0, 0));
		}
		else
		{
			_logger.Debug($"Setting {name} at {mapPos} with tileSetId {tileSetId} and stage {plant.Stage}");
			_tileLayer.SetCell(mapPos, tileSetId, new Vector2I((int)plant.Stage, 0));
		}
	}

	private void GetPlantTiles()
	{
		var plantNodes = GetTree().GetNodesInGroup(GameGroup.Plants);
		foreach (var plant in plantNodes)
		{
			if (plant is not Plant p) return;
			p.BeforePlantRemoved += OnBeforePlantRemoved;
			p.PlantGrown += OnPlantSpriteChange;
			p.PlantDied += OnPlantSpriteChange;
			OnPlantSpriteChange(p);
		}
	}

	private void OnSeedPlanted(Plant obj)
	{
		obj.BeforePlantRemoved += OnBeforePlantRemoved;
		obj.PlantGrown += OnPlantSpriteChange;
		OnPlantSpriteChange(obj);
	}

	//TODO Make it work with shovel tool
	private void OnBeforePlantRemoved(Plant plant)
	{
		var pos = ToLocal(plant.GlobalPosition);
		var mapPos = _tileLayer.LocalToMap(pos);
		_tileLayer.EraseCell(mapPos);
		plant.BeforePlantRemoved -= OnBeforePlantRemoved;
		plant.PlantGrown -= OnPlantSpriteChange;
	}

	private int GetTileSetId(string name)
	{
		switch (name.ToLower())
		{
			case "dead": return 0;
			case "chuberry": return 1;
			case "drupoleaum": return 2;
			case "licary": return 3;

			default: return -1;
		}
	}
}
