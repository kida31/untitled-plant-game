using Godot;
using untitledplantgame.EntityStatsDataContainer;
using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

public partial class InteractableItem : AInteractable
{
	[Export]
	private DataContainer _dataContainer;

	[Export]
	public new string ActionName { get; private set; } = "pickup";

	[Export(PropertyHint.Enum, "Herb,Medicine,Seed")]
	private string _selectedOption;

	public string ItemName => _dataContainer.EntityName; // Convenience property
	private ICharacteristic _characteristic;

	public ItemStack ItemStack = new ItemStack("id", "This", null, "des", ItemCategory.Material, maxStackSize: 1);

	public override void Interact()
	{
		EventBus.Instance.ItemPickedUp(ItemStack);
		EventBus.Instance.InventoryOpened();
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
		set { _selectedOption = value; }
	}
}
