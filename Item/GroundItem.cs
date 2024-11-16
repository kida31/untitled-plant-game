using System.Collections.Generic;
using Godot;
using untitledplantgame.Entity;
using untitledplantgame.Event;
using untitledplantgame.Item.ItemTypes;

namespace untitledplantgame.Item;

/*
 * Completely different tone:
 *
 * This class is essentially the "GroundItem" from ChatGPT. It gets a list of "ItemStacks" (since it could also be a bag with multiple
 * seeds or tools).
 * This list is now the thing that needs to be transferred to the "inventory"! This also makes the whole type system here irrelevant
 */
public partial class GroundItem : Area2D, IInteractable, IEntity
{
	private string _itemName; 
	private bool _canBeInteractedWith = true;
	private List<ItemStack> _itemStacks;

	public override void _Ready()
	{
		AddToGroup("Interactables");
		
		// Temporary solution: Discuss with group how items should be created and added (visually in godot, JSON, etc.)
		/*
		 * Example:
		 * We have three distinct "ItemStacks", each has exactly one copy of their respective items.
		 */
		_itemStacks = new List<ItemStack>
		{
			new (
				new HerbCategory(
					"res://Resources/EntityStats_TEST_1.tres", 
					"StoryHerb_", 
					"MagicHerb",
					64)
				),
			new (
				new MedicineCategory(
					"res://Resources/EntityStats_TEST_1.tres", 
					"StoryHerb_", 
					"MagicHerb", 
					64)),
			new (
				new SeedCategory(
					"res://Resources/EntityStats_TEST_1.tres", 
					"StoryHerb_", 
					"MagicHerb", 
					64))
		};
	}

	public void Interact()
	{
		Hide();
		if (_canBeInteractedWith) //Very bad temporary solution. The Item should be "destroyed" after the interaction is finished
		{
			foreach (var itemStack in _itemStacks)
			{
				// TODO: Should not be called several times, but instead a list => heavy performance problems.
				EventBus.Instance.ItemPickedUp(itemStack);
			}
			_canBeInteractedWith = false;
		}
	}

	public Vector2 GetGlobalInteractablePosition()
	{
		return GlobalPosition;
	}
}
