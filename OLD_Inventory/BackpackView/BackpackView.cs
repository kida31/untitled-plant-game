using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Inventory;

public partial class BackpackView : Control
{
    [Export] private PackedScene _itemViewScene;
    [Export] private GridContainer _gridContainer;
    [Export] private Button _arrangeButton;

    private readonly List<ItemView> _itemViews = new();
    private Backpack _backpack;


    public override void _Ready()
    {
        _arrangeButton.Pressed += () => _backpack.RearrangeContent();
    }

    public void SetBackpack(Backpack backpack)
    {
        // Clear old backpack.
        // Unsubscribe from old backpack.
        // Remove and delete all children related to old backpack.
        if (_backpack != null) _backpack.ContentUpdated -= OnContentUpdated;
        _itemViews.Clear();
        foreach (var child in _gridContainer.GetChildren())
        {
            _gridContainer.RemoveChild(child);
            child.QueueFree();
        }

        
        // Set new backpack and subscribe.
        _backpack = backpack;
        _backpack.ContentUpdated += OnContentUpdated;

        
        // Create visual representation of new backpack.
        for (var i = 0; i < _backpack.Size; i++)
        {
            // Create visual representation
            var itemView = _itemViewScene.Instantiate<ItemView>();
            _gridContainer.AddChild(itemView);
            _itemViews.Add(itemView);

            // Connect "Delete" button
            var staticIndex = i;
            itemView.DeletePressed += () => _backpack.Remove(_itemViews[staticIndex].Item);
            
            // Assign Item
            _itemViews[i].Item = _backpack.Content[i];
        }
    }

    private void OnContentUpdated(IStorable[] items)
    {
        for (var i = 0; i < items.Length; i++)
        {
            // Re-assign all items. (Also forces visual update)
            _itemViews[i].Item = items[i];
        }
    }
}