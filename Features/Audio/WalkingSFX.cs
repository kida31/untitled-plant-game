using Godot;
using untitledplantgame.Common;



namespace untitledplantgame.Audio;
public partial class WalkingSFX : Node
{
	public static WalkingSFX Instance { get; private set; }

	private Node2D _playerPos;
	private TileMapLayer _layer;
	private string[,] sfx; 
	
	public override void _Ready()
	{
		
	}

	private void loadSFXPathsIntoArray() { 
		
	}

	private int GetTileType() {
		var playerPositionLocal = _playerPos.ToLocal(Game.Instance.GetPlayer().GlobalPosition);
		var playerPositionOnMap = _layer.LocalToMap(playerPositionLocal);
		var currentTile = _layer.GetCellTileData(playerPositionOnMap);
		var tileType = currentTile.GetCustomData("Type").AsInt32();
		return tileType;
	}
}
