using Godot;
using System;
using System.Diagnostics;
using InventoryV0;

namespace GUI.VendingMachine
{
    public partial class ItemStackView : Control
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
            _itemTexture.Texture = _innerItemStack.Item != null ? _placeholderIcon : null;
            _quantityLabel.Text = _innerItemStack.Quantity > 0
                ? $"({_innerItemStack.Item?.Name})\n{_innerItemStack.Quantity.ToString()}"
                : "";
        }

        public override Variant _GetDragData(Vector2 atPosition)
        {
            if (_innerItemStack.Item is null) return default;
            
            var previewClone = this.Duplicate() as ItemStackView;
            previewClone!._innerItemStack = _innerItemStack;
            SetDragPreview(previewClone);
            return this;
        }

        public override bool _CanDropData(Vector2 atPosition, Variant data)
        {
            var that = data.As<ItemStackView>();
            if (that is null) return false;
            return true;
        }

        
        public override void _DropData(Vector2 atPosition, Variant data)
        {
            var that = data.As<ItemStackView>();
            Debug.Assert(that is not null);
            
            GD.Print($"Dropping data that:{that._innerItemStack.Item?.Name} <-> this:{_innerItemStack.Item?.Name}");
            (_innerItemStack, that._innerItemStack) = (that._innerItemStack, _innerItemStack);
            
            GD.Print($"that:{that._innerItemStack.Item?.Name} this:{_innerItemStack.Item?.Name}");
        }
    }
}