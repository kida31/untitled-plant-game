using Godot;
using untitledplantgame.Common;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Inventory.GeneralInventory.UI_ItemCategory;

namespace untitledplantgame.Item;

public partial class InteractableItem : AbstractNPC
{
	[Export]
	private DataContainer _dataContainer;

	[Export]
	public new string ActionName { get; private set; } = "pickup";

	[Export(PropertyHint.Enum, "Herb,Medicine,Seed")]
	private string _selectedOption;

	public string ItemName => _dataContainer.EntityName; // Convenience property
	private ICharacteristic _characteristic;

	public override void Interact()
	{
		EventBus.Instance.ItemPickedUp(this);
		GD.Print("Item picked up");
		QueueFree();
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
