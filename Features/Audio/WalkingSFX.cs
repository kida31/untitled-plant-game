using Godot;
using untitledplantgame.Common;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace untitledplantgame.Audio;
public partial class WalkingSFX : Node
{
	public static WalkingSFX Instance { get; private set; }

	[Export]
	private AudioStreamPlayer _audioStreamPlayer;
	private Node2D _playerPos;
	private TileMapLayer _layer;
	private Random _random = new();
	private List<List<string>> _filePaths = new();
	
	
	
	public override void _Ready()
	{
		LoadWalkingSFXPathsIntoArray("res://Assets/SFX/WalkingSFX");
	}



	public void PlayTileSoundAtPlayerPosition() 
	{
		int tileType = GetTileTypeFromPlayerPosition();
		PlayRandomSoundFromTileType(tileType);

	}

	private void PlayRandomSoundFromTileType(int tileType) {
		if (tileType < _filePaths.Count)
		{
			int randomIndex = _random.Next(0, _filePaths[tileType].Count);
			string filePath = _filePaths[tileType][randomIndex];
			_audioStreamPlayer.Stream = GD.Load<AudioStream>(filePath);
			_audioStreamPlayer.Play();
		}
	}


	private void LoadWalkingSFXPathsIntoArray(string rootPath) {
		if (Directory.Exists(rootPath))
		{
			foreach (string directory in Directory.GetDirectories(rootPath))
			{
				List<string> tempfilePaths = new();
				foreach (string fileName in Directory.EnumerateFiles(directory))
				{
					tempfilePaths.Add(fileName);
				}
				_filePaths.Add(tempfilePaths);
				tempfilePaths.Clear();
			}
		}
	}

	private int GetTileTypeFromPlayerPosition() {
		var playerPositionLocal = _playerPos.ToLocal(Game.Instance.GetPlayer().GlobalPosition);
		var playerPositionOnMap = _layer.LocalToMap(playerPositionLocal);
		var currentTile = _layer.GetCellTileData(playerPositionOnMap);
		var tileType = currentTile.GetCustomData("Type").AsInt32();
		return tileType;
	}
}
