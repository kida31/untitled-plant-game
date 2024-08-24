using Godot;
using System;
using untitledplantgame.Inventory;

public partial class ItemView : Control
{
    private IStorable _item;

    [Export] private Label _nameLabel;
    [Export] private BaseButton _deleteButton;
    [Export] private TextureRect _iconTexture;
    [Export] private Texture2D _defaultImage;
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

        // Set Icon
        if (Item != null) GD.Print(Item.Name, " ", Item.Icon ?? _defaultImage);
        _iconTexture.Texture = Item == null ? null : (Item.Icon ?? _defaultImage);
        GD.Print(_iconTexture.Texture);

        // Set hover tooltip
        TooltipText = Item?.Name ?? "";

        // Set Background color
        var oldColor = Modulate;
        oldColor.A = Item == null ? 0.5f : 1f;
        Modulate = oldColor;

        _deleteButton.Visible = Item != null;
    }
}