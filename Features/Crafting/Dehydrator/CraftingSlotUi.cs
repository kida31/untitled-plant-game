using System;
using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Crafting;

public partial class CraftingSlotUi : Node
{
	public TextureRect ItemIcon { get; set; }
	public ItemStack ItemStack { get; set; }

	public void SetItemTexture(ItemStack item)
	{
		ItemIcon.Texture = item.Icon;
	}

	public void RemoveItemTexture()
	{
		ItemIcon.Texture = null;
	}
}
