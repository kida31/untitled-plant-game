
using Godot;
using System.Collections.Generic;
using System.Linq;
using GUI.VendingMachine;
using untitledplantgame.Inventory.Alt;

public partial class VendingMachineUI : Control
{
    [Export] private Node _itemStackContainer;
    [Export] private Tooltip _tooltip;
    [Export] private Slider _slider;
    [Export] private Label _moneyLabel;

    private VendingMachine _vendingMachine;
    private List<ItemSlotUI> _itemSlots;

    public override void _Ready()
    {
        _itemSlots = _itemStackContainer.GetChildren().Cast<ItemSlotUI>().ToList();
        _slider.ValueChanged += OnSliderValueChanged;
    }
    
    public override void _Process(double delta)
    {
        if (_vendingMachine is null) return;
        _moneyLabel.Text = "Gold: " + _vendingMachine.Gold;
    }
    
    public void SetVendingMachine(VendingMachine vendingMachine)
    {
        if (_vendingMachine is not null) _vendingMachine.ContentChanged -= UpdateContent;

        _vendingMachine = vendingMachine;
        if (_vendingMachine is null) return;
        _vendingMachine.ContentChanged += UpdateContent;
        UpdateContent(_vendingMachine.Inventory);
    }

    private void UpdateContent(IInventory inventory)
    {
	    var items = inventory.GetContents();
        for (var index = 0; index < _itemSlots.Count && index < items.Count; index++)
        {
	        _itemSlots[index].ItemStack = items[index];
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
