using Godot;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;
using untitledplantgame.GUI.Components;

namespace untitledplantgame.Audio
{
	public partial class SfxUI : Node
	{
		[Export] private float _volume = -10; // Default volume
		private Dictionary<string, AudioStreamPlayer> _sounds = new Dictionary<string, AudioStreamPlayer>()
		{
			{"MenuUiPlayGame.wav", new AudioStreamPlayer()},
			{"MenuUiHoverSound.wav", new AudioStreamPlayer()},
			{"MenuUiExitGame.wav", new AudioStreamPlayer()},
			{"MenuUiClickButton.wav", new AudioStreamPlayer()}
		};

		private Logger _logger = new Logger("SfxUI");

		public override void _Ready()
		{	
			// Create and add audio players to the scene
			foreach (var key in _sounds.Keys) 
			{
				_sounds[key].Stream = (AudioStream)GD.Load("res://Assets/SFX/" + key);
				_sounds[key].Bus = "UI";
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
			List<Node> allNodes = CollectAllNodes(GetTree().Root);

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

		private List<Node> CollectAllNodes(Node root) { 
			List<Node> nodes = new List<Node>();
			CollectNodesRecursively(root, nodes);
			_logger.Debug($"Collected all Nodes {nodes.Count}");
			return nodes;
		}

		private void CollectNodesRecursively(Node current, List<Node> nodes) { 
			nodes.Add(current);
			foreach (var child in current.GetChildren()) {
				CollectNodesRecursively(child, nodes);
			}
		}
		

		private void PlayUiSfx(string sfx) {
			_sounds[sfx].Play();
			_logger.Debug($"played {sfx}");
		}
	}
}
