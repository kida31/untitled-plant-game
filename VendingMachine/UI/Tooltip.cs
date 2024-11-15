using Godot;

namespace untitledplantgame.VendingMachine;

public partial class Tooltip : Control
{
	public enum Mood
	{
		Sad,
		Neutral,
		Happy,
	}

	[Export]
	private TextureRect _emojiTexture;

	[Export]
	private Slider _slider;

	[Export]
	private Vector2 _offset;

	[Export]
	private Timer _fadeTimer;

	[ExportGroup("Emojis")]
	[Export]
	private Texture2D[] _sadFaces;

	[Export]
	private Texture2D[] _neutralFaces;

	[Export]
	private Texture2D[] _happyFaces;

	private Mood _currentMood = Mood.Neutral;
	private bool _isFadingOut = false;

	public override void _Ready()
	{
		_slider.ValueChanged += OnSliderValueChanged;
		_fadeTimer.Timeout += () => _isFadingOut = true;
		Alpha = 0;
		Visible = false;
	}

	public override void _Process(double delta)
	{
		if (_isFadingOut)
		{
			Alpha = Mathf.Lerp(Alpha, 0.0f, 1.0f * (float)delta);
		}
	}

	public void SetMood(Mood mood)
	{
		if (_currentMood == mood)
		{
			return;
		}

		_currentMood = mood;
		_emojiTexture.Texture = GetRandomEmoji(mood);
	}

	private float Alpha
	{
		get => Modulate.A;
		set
		{
			var newModulate = Modulate;
			newModulate.A = value;
			Modulate = newModulate;
		}
	}

	private void OnSliderValueChanged(double value)
	{
		var valuePercent = _slider.Value / _slider.MaxValue;
		var updatedPosition = _slider.GlobalPosition + _offset;
		updatedPosition.X += _slider.GetRect().Size.X * (float)valuePercent;
		GlobalPosition = updatedPosition;
		_isFadingOut = false;
		_fadeTimer.Stop();
		_fadeTimer.Start();

		// Set Modulate.A
		Alpha = (float)1.0;
		Visible = true;
	}

	private Texture2D GetRandomEmoji(Mood mood)
	{
		switch (mood)
		{
			case Mood.Sad:
				return _sadFaces[GD.RandRange(0, _sadFaces.Length - 1)];
			case Mood.Happy:
				return _happyFaces[GD.RandRange(0, _happyFaces.Length - 1)];
			case Mood.Neutral:
				return _neutralFaces[GD.RandRange(0, _neutralFaces.Length - 1)];
		}

		return null;
	}
}
