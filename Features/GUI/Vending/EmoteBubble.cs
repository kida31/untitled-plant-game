using System;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.GUI.Hooks;
using untitledplantgame.GUI.Traits;

namespace untitledplantgame.GUI.Vending;

[Tool]
public partial class EmoteBubble : Control, IFadable
{
	public event Action<float> ValueChanged;
	public event Action<float> OverlapChanged;

	/// <summary>
	///		 The value is the position of the emote bubble.
	/// </summary>
	[Export(PropertyHint.Range, "0.0,1.0")]
	public float Value
	{
		get => _value;
		set
		{
			_value = Mathf.Clamp(value, 0, 1);
			ValueChanged?.Invoke(_value);
		}
	}

	/// <summary>
	///		 The deadzone is the area around the value where the emote bubble will not change.
	///		A value of 0 will always interpolate between two emotes.
	///		 A value of 1 will always round down the value of the emote.
	/// </summary>
	[Export(PropertyHint.Range, "0.0,1.0")]
	public float DeadZone
	{
		get => _deadZone;
		set
		{
			_deadZone = Mathf.Clamp(value, 0, 1);
			OverlapChanged?.Invoke(_deadZone);
		}
	}

	/// <summary>
	///		Emotes to display in the bubble. Each emote is a texture.
	///		Each emote acts as a tick. 
	/// </summary>
	[ExportGroup("Setup")] [Export] private Texture2D[] _emotes = Array.Empty<Texture2D>();

	/// <summary>
	///		Texture1 of the emote bubble. Used to fade into Texture2
	/// </summary>
	[Export] private TextureRect _emoteTexture1;

	/// <summary>
	///		 Texture2 of the emote bubble. Used to fade into Texture1
	/// </summary>
	[Export] private TextureRect _emoteTexture2;

	private float _value;
	private float _deadZone;
	private Logger _logger;

	public EmoteBubble()
	{
		_logger = new Logger(this);
		ValueChanged += OnValueChanged;
		OverlapChanged += OnOverlapChanged;
	}

	public override void _ExitTree()
	{
		ValueChanged -= OnValueChanged;
		OverlapChanged -= OnOverlapChanged;
	}

	private void OnOverlapChanged(float obj)
	{
		UpdateEmotes();
	}

	private void OnValueChanged(float obj)
	{
		UpdateEmotes();
	}

	private void UpdateEmotes()
	{
		if (_emotes == null || _emotes.Length == 0)
		{
			_emoteTexture1.Texture = null;
			_emoteTexture2.Texture = null;
			return;
		}

		var ticks = _emotes.Length;
		var floatIndex = _value * (ticks - 1);

		// Index lies between two emotes
		var idx1 = Mathf.FloorToInt(floatIndex);
		var idx2 = Mathf.CeilToInt(floatIndex);
		_emoteTexture1.Texture = _emotes[idx1];
		_emoteTexture2.Texture = _emotes[idx2];

		// Remaining decimal of index is the weight between the two textures.
		var weight = floatIndex - idx1;
		var remappedWeight = Mathf.Remap(weight, _deadZone, 1, 0, 1);

		// Set modulate to fade between the two textures
		var alpha2 = Mathf.Clamp(remappedWeight, 0, 1);
		var alpha1 = 1 - alpha2;
		_emoteTexture1.Modulate = new Color(1, 1, 1, alpha1);
		_emoteTexture2.Modulate = new Color(1, 1, 1, alpha2);
	}
}
