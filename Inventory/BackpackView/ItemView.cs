using Godot;
using System;
using untitledplantgame.Inventory;

public partial class ItemView : Control
{
    private IStorable _item;

    [Export] private Label _nameLabel;
    [Export] private Button _deleteButton;

    
    public event Action DeletePressed;


    public override void _Ready()
    {
        _deleteButton.Pressed += () => DeletePressed?.Invoke();
    }


    public IStorable Item
    {
        get => _item;
        set
        {
            _item = value;
            if (IsInsideTree()) UpdateItemView();
        }
    }


    private void UpdateItemView()
    {
        // Set Text
        _nameLabel.Text = Item?.Name ?? "";

        // Set hover tooltip
        TooltipText = Item?.Name ?? "";

        // Set Background color
        var oldColor = Modulate;
        oldColor.A = Item == null ? 0.5f : 1f;
        Modulate = oldColor;

        _deleteButton.Visible = Item != null;
    }
}