using Godot;
using System.Collections.Generic;
using untitledplantgame.Common;
using untitledplantgame.GUI.Components;

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
				_sounds[key].Stream = GD.Load<AudioStream>("res://Assets/SFX/UISFX/" + key);
				_sounds[key].Bus = busName;
				AddChild(_sounds[key]);
			}

			_logger.Debug("Initialized Audiostreams");

			InstallSounds();
			GetViewport().GuiFocusChanged += PlayHoveredSound;
		}

		public override void _ExitTree()
		{
			// Clean up events, unsubscribe from all.
			// "Uninstall Sounds"
			GetViewport().GuiFocusChanged -= PlayHoveredSound;
			var nodes = CollectAllNodes(GetTree().Root, new List<Node>());
			nodes.ForEach(node =>
			{
				// Check if the node is still valid and if it is a control
				if (!IsInstanceValid(node) || node is not Control control) return;

				// Unlike c# events, godot signals cannot be unsubscribed if they are not connected.
				if (control is Button button && button.IsConnected(BaseButton.SignalName.Pressed, Callable.From(PlayClickSound)))
				{
					button.Pressed -= PlayClickSound;
				}

				if (control is TextureButton btn && btn.IsConnected(BaseButton.SignalName.Pressed, Callable.From(PlayClickSound)))
				{
					btn.Pressed -= PlayClickSound;
				}

				if (control is Clickable clickable)
				{
					clickable.Pressed -= PlayClickSound;
				}
			});
		}

		private void PlayClickSound()
		{
			PlayUiSfx("MenuUiClickButton.wav");
		}

		private void PlayHoveredSound(object _)
		{
			PlayUiSfx("MenuUiHoverSound.wav");
		}

		private void InstallSounds()
		{
			List<Node> nodes = new();
			List<Node> allNodes = CollectAllNodes(GetTree().Root, nodes);
			_logger.Debug($"Collected Nodes {allNodes.Count}");
			foreach (var node in allNodes)
			{
				if (node is Button button)
				{
					button.Pressed += PlayClickSound;
				}

				if (node is Clickable clickable)
				{
					clickable.Pressed += PlayClickSound;
				}

				if (node is TextureButton btn)
				{
					btn.Pressed += PlayClickSound;
				}
			}
		}

		private List<Node> CollectAllNodes(Node root, List<Node> nodes)
		{
			nodes.Add(root);
			foreach (var child in root.GetChildren())
			{
				CollectAllNodes(child, nodes);
			}

			return nodes;
		}


		private void PlayUiSfx(string sfx)
		{
			_sounds[sfx].Play();
			_logger.Debug($"played {sfx}");
		}
	}
}
