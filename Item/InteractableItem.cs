using Godot;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Inventory.GeneralInventory.UI_ItemCategory;

namespace untitledplantgame.Item;

public partial class InteractableItem : Area2D, IInteractable
{
	public string ItemName => _dataContainer.EntityName; // Convenience property
	
	[Export]
	private DataContainer _dataContainer;

	[Export(PropertyHint.Enum, "Herb,Medicine,Seed")]
	private string SelectedOption
	{
		get => _selectedOption;
		set
		{
			_selectedOption = value;
			_characteristic = CreateInstance();
		}
	}

	private string _selectedOption;
	private ICharacteristic _characteristic;
	private bool _canBeInteractedWith = true;

	public override void _Ready()
	{
		AddToGroup("Interactables");
	}

	private ICharacteristic CreateInstance() =>
		_selectedOption switch
		{
			"Herb" => new HerbCategory(),
			"Medicine" => new MedicineCategory(),
			"Seed" => new SeedCategory(),
			_ => null,
		};

	public void Interact()
	{
		if (_canBeInteractedWith) //Very bad temporary solution. The Item should be "destroyed" anyway
		{
			EventBus.Instance.ItemPickedUp(this);
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
