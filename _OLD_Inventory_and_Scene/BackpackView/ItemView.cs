using System;
using Godot;
using untitledplantgame.Inventory;

public partial class ItemView : Control
{
	private IStorable _item;

	[Export]
	private Label _nameLabel;

	[Export]
	private BaseButton _deleteButton;

	[Export]
	private TextureRect _iconTexture;

	[Export]
	private Texture2D _defaultImage;
	public event Action DeletePressed;

	public override void _Ready()
	{
		// Do whatever "DeletePressed" was assigned to
		_deleteButton.Pressed += () => DeletePressed?.Invoke();
	}

	public IStorable Item
	{
		get => _item;
		set
		{
			_item = value;
			if (IsInsideTree())
				UpdateItemView();
		}
	}

	private void UpdateItemView()
	{
		// Set Text
		_nameLabel.Text = Item?.Name ?? "";

		// Set Icon
		if (Item != null)
			GD.Print(Item.Name, " ", Item.Icon ?? _defaultImage);
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

	public override Variant _GetDragData(Vector2 atPosition)
	{
		// This is your custom method generating the preview of the drag data. Can be any Control node.
		var rect = new TextureRect();
		rect.Texture = _iconTexture.Texture;
		SetDragPreview(rect);

		// Return data. Can be other dedicated type
		return this;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return true; // Should not be always true. Please don't drop item into the void.
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		// Swap items
		var thatItemView = (ItemView)data;
		var tmp = _item;
		_item = thatItemView._item;
		thatItemView._item = tmp;

		// Force text and sprite update
		UpdateItemView();
		thatItemView.UpdateItemView();
	}
}
