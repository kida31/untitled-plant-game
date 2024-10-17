using Godot;
using System;
using InventoryV0;

namespace GUI.VendingMachine
{
    public partial class ItemStack : Control
    {
        private ItemStack<IStorable> _innerItemStack;

        [Export] private Label _quantityLabel;
        [Export] private TextureRect _itemTexture;
        [Export] private Texture2D _placeholderIcon;

        public ItemStack<IStorable> InnerItemStack
        {
            get => _innerItemStack;
            set => _innerItemStack = value;
        }

        public override void _Process(double delta)
        {
            // i do not know whether this affects performance
            GD.Print(_innerItemStack == null);
            _itemTexture.Texture = _innerItemStack.Item != null ? _placeholderIcon : null;
            _quantityLabel.Text = _innerItemStack.Quantity > 0 ? _innerItemStack.Quantity.ToString() : "";
        }

        public override Variant _GetDragData(Vector2 atPosition)
        {
            return this;
        }

        public override bool _CanDropData(Vector2 atPosition, Variant data)
        {
            var that = data.As<ItemStack>();
            return false;
        }
        
        public override void _DropData(Vector2 atPosition, Variant data)
        {
            var that = data.As<ItemStack>();
        }
    }

    // TODO: this wrapper feels horrible
    partial class ItemStackWrapper : GodotObject
    {
        public ItemStack<IStorable> Item;
    }
}