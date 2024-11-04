using Godot;
using System;

namespace GUI.VendingMachine;

public partial class Tooltip : Control
{
    public enum Mood
    {
        SAD,
        NEUTRAL,
        HAPPY,
    }

    [Export] private TextureRect _emojiTexture;
    [Export] private Slider _slider;
    [Export] private Vector2 _offset;

    [ExportGroup("Emojis")] [Export] private Texture2D[] _sadFaces;
    [Export] private Texture2D[] _neutralFaces;
    [Export] private Texture2D[] _happyFaces;

    private Mood _currentMood = Mood.NEUTRAL;
    private bool _isDragging = false;

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

    public override void _Ready()
    {
        _slider.ValueChanged += OnSliderValueChanged;
        _slider.DragEnded += OnDragEnded;
        Visible = true;
    }

    public override void _Process(double delta)
    {
        if (!_isDragging) Alpha = Mathf.Lerp(Alpha, 0.0f,  3.0f * (float) delta);
    }

    private void OnDragEnded(bool valuechanged)
    {
        Alpha = (float) 1.0;
        _isDragging = false;
    }

    private void OnSliderValueChanged(double value)
    {
        var valuePercent = _slider.Value / _slider.MaxValue;
        var updatedPosition = _slider.GlobalPosition + _offset; 
        updatedPosition.X += _slider.GetRect().Size.X * (float) valuePercent;
        GlobalPosition = updatedPosition;
        _isDragging = true;

        // Set Modulate.A
        Alpha = (float) 1.0;
    }


    public void SetMood(Mood mood)
    {
        if (_currentMood == mood) return;
        _currentMood = mood;
        _emojiTexture.Texture = GetRandomEmoji(mood);
    }

    private Texture2D GetRandomEmoji(Mood mood)
    {
        switch (mood)
        {
            case Mood.SAD:
                return _sadFaces[GD.RandRange(0, _sadFaces.Length - 1)];
            case Mood.HAPPY:
                return _happyFaces[GD.RandRange(0, _happyFaces.Length - 1)];
            case Mood.NEUTRAL:
                return _neutralFaces[GD.RandRange(0, _neutralFaces.Length - 1)];
        }

        return null;
    }
}