using Godot;
using System;
using System.Collections.Generic;

namespace untitledplantgame.Audio
{
	public partial class SfxUI : Node
	{
		[Export] private float _volume = 100; // Default volume
		private List<AudioStreamPlayer> _audioPlayers = new List<AudioStreamPlayer>();
		private Dictionary<string, AudioStreamPlayer> _sounds = new Dictionary<string, AudioStreamPlayer>()
		{
			{"menu-ui_Play_Game", new AudioStreamPlayer()},
			{"menu-ui_Hover_Sound", new AudioStreamPlayer()},
			{"menu-ui_Exit_Game", new AudioStreamPlayer()},
			{"menu-ui_Click_Button", new AudioStreamPlayer()}
		};


		public override void _Ready()
		{
			// Create and add audio players to the scene
			for (int i = 0; i < 10; i++)
			{
				AudioStreamPlayer player = new AudioStreamPlayer();
				player.SetVolumeDb(_volume); // Set the volume from the exported variable
				AddChild(player); // Add the player to the scene tree
				_audioPlayers.Add(player); // Add the player to the list
			}
			GD.Print($"Initialized {_audioPlayers.Count} audio players.");
		}

		public void PlaySound(string resourcePath)
		{
			AudioStream audioStream = GD.Load<AudioStream>(resourcePath);
			if (audioStream == null)
			{
				GD.PrintErr($"Failed to load audio stream from path: {resourcePath}");
				return;
			}
			GD.Print($"AudioStream initialized: {audioStream != null}");

			foreach (var player in _audioPlayers)
			{
				GD.Print($"Player {player.Name} Playing: {player.Playing}"); // Debugging output
				if (!player.Playing)
				{
					player.Stream = audioStream;
					player.Play();
					GD.Print("Sound Played Successfully");
					return;
				}
			}
			GD.Print("No available audio players to play sound.");
		}
	}
}
