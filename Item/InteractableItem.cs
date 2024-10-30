using Godot;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Inventory.GeneralInventory.UI_ItemCategory;

namespace untitledplantgame.Item;

public partial class InteractableItem : Area2D, IInteractable
{
	[Export] private DataContainer _dataContainer;
	[Export(PropertyHint.Enum, "Herb,Medicine,Seed")]
	private string SelectedOption
	{
		get => _selectedOption;
		set
		{
			_selectedOption = value;
			_characteristic = CreateInstance();
			// Godot seems to not know what changed is. This is not causing any major issues for now... But idk.
			EmitSignal("changed"); // Notify Godot that the property has changed to update the Inspector
		}
	}
	
	private string _selectedOption;
	private ICharacteristic _characteristic;
	private EventBus _globalEventBus;
	private bool _canBeInteractedWith = true;
	
	public override void _Ready()
	{
		AddToGroup("Interactables");
		_globalEventBus = GetNode<EventBus>("/root/EventBus");
	}
	
	private ICharacteristic CreateInstance() => _selectedOption switch
	{
		"Herb" => new HerbCategory(),
		"Medicine" => new MedicineCategory(),
		"Seed" => new SeedCategory(),
		_ => null
	};
	
	public void Interact()
	{
		if (_canBeInteractedWith) //Very bad temporary solution. The Item should be "destroyed" anyway
		{
			_globalEventBus.ItemPickedUp(this);
			_canBeInteractedWith = false;
		}
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}

	public DataContainer GetItemDataContainer()
	{
		return _dataContainer;
	}

	public ICharacteristic GetICharacteristic()
	{
		return _characteristic;
	}
}
