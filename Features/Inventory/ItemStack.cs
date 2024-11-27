using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using untitledplantgame.Common;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory;

/// <summary>
///     https://github.com/Bukkit/Bukkit/blob/master/src/main/java/org/bukkit/inventory/ItemStack.java
/// </summary>
[GlobalClass]
public partial class ItemStack : Resource, IItemStack
{
	[Export] public int Amount { get; set; }
	[Export] public string Id { get; set; }
	[Export] public string Name { get; set; }
	[Export] public Texture2D Icon { get; set; }
	[Export] public string Description { get; set; }
	[Export] public int MaxStackSize { get; set; }
	[Export] public int BaseValue { get; set; }

	[Export(PropertyHint.Enum, "Plant,Material,Medicine")] private string _category;
	[Export] private Array<AComponent> _component;

	public ItemCategory Category
	{
		get
		{
			return _category switch {
				"Plant" => ItemCategory.Plant,
				"Material" => ItemCategory.Material,
				"Medicine" => ItemCategory.Medicine,
				_ => null
			};

			
		}
		set
		{
			_category = value.Name;
		}
	}

	private readonly Logger _logger;

	public ItemStack()
	{
		_logger = new Logger("ItemStack");
		_component = new();
	}

	public ItemStack(
		string id,
		string name,
		Texture2D icon,
		string description,
		ItemCategory category,
		int maxStackSize,
		int baseValue,
		int amount = 1
	) : this()
	{
		Id = id;
		Name = name;
		Icon = icon;
		Description = description;
		Category = category;
		MaxStackSize = maxStackSize;
		BaseValue = baseValue;
		Amount = amount;
	}

	public T GetComponent<T>()
		where T : AComponent
	{
		var idx = _component.ToList().FindIndex(component => component is T);
		if (idx != -1)
		{
			var blah = _component[idx];
			return (T)blah;
		}
		else
		{
			return null;
		}
	}

	public void AddComponent<T>(T component)
		where T : AComponent
	{
		if (GetComponent<T>() is not null)
		{
			_logger.Warn("Component should only exist once");
			return;
		}

		_component.Add(component);
	}

	public T RemoveComponent<T>()
		where T : AComponent
	{
		var idx = _component.ToList().FindIndex(component => component is T);
		if (idx == -1)
		{
			return null;
		}

		var component = (T)_component[idx];
		_component.RemoveAt(idx);
		return component;
	}

	public bool HasSameId(IItemStack itemStack)
	{
		return Id == itemStack.Id;
	}

	public bool HasSameIdAndProps(IItemStack itemStack)
	{
		_logger.Warn("HasSameIdAndProps is not implemented correctly.");
		return Id == itemStack.Id;
	}

	public bool IsIdentical(IItemStack itemStack)
	{
		return HasSameIdAndProps(itemStack) && Amount == itemStack.Amount;
	}

	public IItemStack Clone()
	{
		var newStack = new ItemStack(Id, Name, Icon, Description, Category, MaxStackSize, BaseValue);
		newStack.Amount = Amount;
		return newStack;
	}
}
