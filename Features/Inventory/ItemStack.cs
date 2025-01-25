using System;
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
[Obsolete("Did you mean to use the interface IItemStack? (This is not really obsolete. Just a reminder)")]
public partial class ItemStack : Resource, IItemStack
{
	[Export] public int Amount { get; set; } = 1;
	[Export] public string Id { get; set; } = "";
	[Export] public string Name { get; set; } = "";
	[Export] public Texture2D Icon { get; set; } = null;
	[Export] public string ToolTipDescription { get; set; }
	[Export] public string WikiDescription { get; set; }
	[Export] public int MaxStackSize { get; set; } = 64;
	[Export] public int BaseValue { get; set; } = 0;

	[Export(PropertyHint.Enum, "Seed,Material,Medicine")]
	private string _category;

	[Export] public Array<AComponent> Components { get; set; } = new();

	[Export] public Array<string> RelatedItemIds { get; set; } = new();

	public ItemCategory Category
	{
		get
		{
			return _category switch
			{
				"Seed" => ItemCategory.Seed,
				"Material" => ItemCategory.Material,
				"Medicine" => ItemCategory.Medicine,
				_ => null
			};
		}
		set => _category = value.Name;
	}

	private readonly Logger _logger;

	[Obsolete("Use the constructor with the id parameter in code")]
	public ItemStack()
	{
		_logger = new Logger("ItemStack");
		Components = new();
	}

	public ItemStack(string id
	) : this()
	{
		Id = id;
	}
	
	public ItemStack(IItemStack item)
	{
		Id = item.Id;
		Name = item.Name;
		Icon = item.Icon;
		ToolTipDescription = item.ToolTipDescription;
		WikiDescription = item.WikiDescription;
		Category = item.Category;
		MaxStackSize = item.MaxStackSize;
		BaseValue = item.BaseValue;
		Amount = item.Amount;
		Components = item.Components.Duplicate(true); // I doubt this actually deep copies
	}

	public T GetComponent<T>()
		where T : AComponent
	{
		if (typeof(T) == typeof(AComponent))
		{
			_logger.Error("Cannot get component of type AComponent. Use a derived type instead.");
			return null;
		}
		
		var idx = Components.ToList().FindIndex(component => component is T);
		if (idx != -1)
		{
			var blah = Components[idx];
			return (T) blah;
		}

		return null;
	}

	public void AddComponent<T>(T component)
		where T : AComponent
	{
		_logger.Debug($"Adding component <{typeof(T).Name}: " + component);
		if (GetComponent<T>() is not null)
		{
			_logger.Warn("Component should only exist once");
			return;
		}

		Components.Add(component);
	}

	public T RemoveComponent<T>()
		where T : AComponent
	{
		var idx = Components.ToList().FindIndex(component => component is T);
		if (idx == -1)
		{
			return null;
		}

		var component = (T) Components[idx];
		Components.RemoveAt(idx);
		return component;
	}

	public bool HasSameId(IItemStack itemStack)
	{
		return Id == itemStack?.Id;
	}

	public bool HasSameIdAndProps(IItemStack itemStack)
	{
		if (itemStack == null) return false;
		return Id == itemStack.Id &&
		       /*
		       Name == itemStack.Name &&
		       Icon == itemStack.Icon &&
		       Description == itemStack.Description &&
		       Category == itemStack.Category &&
		       BaseValue == itemStack.BaseValue &&
		       MaxStackSize == itemStack.MaxStackSize &&
		       */
		       Components.All(c => c.Equals(itemStack.GetComponent(c)));
	}

	public bool IsIdentical(IItemStack itemStack)
	{
		return HasSameIdAndProps(itemStack) && Amount == itemStack.Amount;
	}

	public IItemStack Clone()
	{
		return new ItemStack(Id)
		{
			Name = Name,
			Icon = Icon,
			ToolTipDescription = ToolTipDescription,
			WikiDescription = WikiDescription,
			Category = Category,
			BaseValue = BaseValue,
			MaxStackSize = MaxStackSize,
			Amount = Amount,
			// TODO: After the JSON fiasco I absolutely do NOT trust Godot to handle deep copies well (especially looking at the Stats)
			Components = Components.Duplicate(true)
		};
	}

	public override string ToString()
	{
		return
			$"ItemStack{{Name={Name}, Id={Id}, Amount={Amount}, Category={Category}, Description={ToolTipDescription}, MaxStackSize={MaxStackSize}, BaseValue={BaseValue}, Components={Components}}}";
	}
}
