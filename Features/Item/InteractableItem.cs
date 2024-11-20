// using System.Text.RegularExpressions;
using Godot;
using untitledplantgame.Common;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Inventory.GeneralInventory.UI_ItemCategory;

namespace untitledplantgame.Item;

public partial class InteractableItem : Area2D, IInteractable
{
	[Export]
	private DataContainer _dataContainer;

	[Export(PropertyHint.Enum, "Herb,Medicine,Seed")]
	private string _selectedOption;
	public string ItemName => _dataContainer.EntityName; // Convenience property

	public string ActionName { get; private set; } = "pickup";

	private ICharacteristic _characteristic;
	private bool _canBeInteractedWith = true;

	public override void _Ready()
	{
		AddToGroup(Group.Interactables);
	}

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

	private string SelectedOption
	{
		get => _selectedOption;
		set
		{
			_selectedOption = value;
			_characteristic = CreateInstance();
		}
	}

	private ICharacteristic CreateInstance() =>
		_selectedOption switch
		{
			"Herb" => new HerbCategory(),
			"Medicine" => new MedicineCategory(),
			"Seed" => new SeedCategory(),
			_ => null,
		};
}
