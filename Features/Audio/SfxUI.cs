using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.GUI.Components;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Audio
{
	public partial class SfxUI : Node
	{
		private Dictionary<string, AudioStreamPlayer> _sounds = new Dictionary<string, AudioStreamPlayer>()
		{
			{"MenuUiPlayGame.wav", new AudioStreamPlayer()},
			{"MenuUiHoverSound.wav", new AudioStreamPlayer()},
			{"MenuUiExitGame.wav", new AudioStreamPlayer()},
			{"MenuUiClickButton.wav", new AudioStreamPlayer()}
		};
		private const string busName = "UI";
		private Logger _logger = new Logger("SfxUI");

		public override void _Ready()
		{	
			// Create and add audio players to the scene
			foreach (var key in _sounds.Keys) 
			{
				_sounds[key].Stream = GD.Load<AudioStream>("res://Assets/SFX/" + key);
				_sounds[key].Bus = busName;
				AddChild(_sounds[key]);
			}
			_logger.Debug("Initialized Audiostreams");

			InstallSounds();
			GetViewport().GuiFocusChanged += (_) => PlayHoveredSound();
			
		}

		private void PlayClickSound() {
			PlayUiSfx("MenuUiClickButton.wav");
		}

		private void PlayHoveredSound() {
			PlayUiSfx("MenuUiHoverSound.wav");
		}

		private void InstallSounds() {
			List <Node> nodes = new();
			List<Node> allNodes = CollectAllNodes(GetTree().Root, nodes);
			_logger.Debug($"Collected Nodes {allNodes.Count}");
			foreach (var node in allNodes)
			{
				if (node is Button button) {
					button.Pressed += PlayClickSound;
				}
				if (node is Clickable clickable)
				{
					clickable.Pressed += PlayHoveredSound;
				}

			}
		}

		private List<Node> CollectAllNodes(Node root, List<Node> nodes) { 
            nodes.Add(root);
			foreach (var child in root.GetChildren())
			{
				CollectAllNodes(child, nodes);
			}
			return nodes;
		}

		
		

		private void PlayUiSfx(string sfx) {
			_sounds[sfx].Play();
			_logger.Debug($"played {sfx}");
		}
	}
}
