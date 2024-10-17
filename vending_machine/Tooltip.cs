using Godot;
using System;
namespace GUI.VendingMachine;
public partial class Tooltip : Control
{
	[Export] private TextureRect _emojiTexture;
	[ExportGroup("Emojis")]
	[Export] private Texture2D[] _sadFaces;
	[Export] private Texture2D[] _neutralFaces;
	[Export] private Texture2D[] _happyFaces;

	public enum Mood
	{
		SAD,
		NEUTRAL,
		HAPPY,
	}
	
	private Mood currentMood = Mood.NEUTRAL;

	public override void _Ready()
	{
		
	}

	public void SetMood(Mood mood)
	{
		if (currentMood == mood) return;
		currentMood = mood;
		_emojiTexture.Texture = GetRandomEmoji(mood);
	}

	private Texture2D GetRandomEmoji(Mood mood)
	{
		switch (mood)
		{
			case Mood.SAD:
				return _sadFaces[GD.RandRange(0, _sadFaces.Length)];
			case Mood.HAPPY:
				return _happyFaces[GD.RandRange(0, _happyFaces.Length)];
			case Mood.NEUTRAL:
				return _neutralFaces[GD.RandRange(0, _neutralFaces.Length)];
		}

		return null;
	}
}
