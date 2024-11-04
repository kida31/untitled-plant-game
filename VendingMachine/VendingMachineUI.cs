using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using GUI.VendingMachine;
using InventoryV0;

public partial class VendingMachineUI : Control
{
    [Export] private Node _itemStackContainer;
    [Export] private Tooltip _tooltip;
    [Export] private Slider _slider;
    [Export] private Label _moneyLabel;

    private VendingMachine _vendingMachine;
    private List<ItemStackView> _itemStacks;

    public override void _Ready()
    {
        _itemStacks = _itemStackContainer.GetChildren().Cast<ItemStackView>().ToList();
        _slider.ValueChanged += OnSliderValueChanged;
    }
    
    public override void _Process(double delta)
    {
        if (_vendingMachine is null) return;
        _moneyLabel.Text = "Gold: " + _vendingMachine.Gold;
    }
    
    public void SetVendingMachine(VendingMachine vendingMachine)
    {
        if (_vendingMachine is not null) _vendingMachine.ContentChanged -= SetContent;

        _vendingMachine = vendingMachine;
        if (_vendingMachine is null) return;
        _vendingMachine.ContentChanged += SetContent;
        SetContent(_vendingMachine.Items);
    }

    private void SetContent(List<ItemStack<ISellable>> stacks) {
        
        for (var index = 0; index < _itemStacks.Count && index < stacks.Count; index++)
        {
            // TODO: un-uglify this. Thanks!
            // Tedious copying of data from vending machine to UI
            var sourceItem = _vendingMachine.Items[index];
            var destinationItem = _itemStacks[index].InnerItemStack;
            destinationItem.Item = sourceItem.Item;
            destinationItem.Quantity = sourceItem.Quantity;
            _itemStacks[index].InnerItemStack = destinationItem;
        }
    }
    
    private void OnSliderValueChanged(double value)
    {
        if (_vendingMachine is null) return;
        
        _vendingMachine.SetPriceSlider((float)value);
        
        // Update UI
        var offsetValue = value - _slider.MinValue;
        var offsetPercent = offsetValue / (_slider.MaxValue - _slider.MinValue);

        switch (offsetPercent)
        {
            case > 0.75:
                _tooltip.SetMood(Tooltip.Mood.SAD);
                break;
            case < 0.25:
                _tooltip.SetMood(Tooltip.Mood.HAPPY);
                break;
            default:
                _tooltip.SetMood(Tooltip.Mood.NEUTRAL);
                break;
        }
    }
}