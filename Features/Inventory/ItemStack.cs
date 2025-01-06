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
	[Export] public string ToolTipDescription { get; set; }
	[Export] public string WikiDescription { get; set; }
	[Export] public int MaxStackSize { get; set; }
	[Export] public int BaseValue { get; set; }

	[Export(PropertyHint.Enum, "Seed,Material,Medicine")]
	private string _category;

	[Export] public Array<AComponent> Components { get; set; }

	[Export] public Array<string> RelatedItemIds { get; set; }

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

	public ItemStack()
	{
		_logger = new Logger("ItemStack");
		Components = new();
	}

	public ItemStack(string id,
		string name,
		Texture2D icon,
		string toolTipDescription,
		string wikiDescription,
		ItemCategory category,
		int baseValue = 1,
		int maxStackSize = 64,
		int amount = 1,
		Array<AComponent> components = null
	) : this()
	{
		Id = id;
		Name = name;
		Icon = icon;
		ToolTipDescription = toolTipDescription;
		Category = category;
		MaxStackSize = maxStackSize;
		BaseValue = baseValue;
		Amount = amount;
		Components = components;
	}

	public T GetComponent<T>()
		where T : AComponent
	{
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
		return Id == itemStack.Id;
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
		// TODO: After the JSON fiasco I absolutely do NOT trust Godot to handle deep copies well (especially looking at the Stats)
		var newStack = new ItemStack(Id, Name, Icon, ToolTipDescription, WikiDescription, Category, baseValue: BaseValue, maxStackSize: MaxStackSize,
			amount: Amount, components: Components.Duplicate(true));
		return newStack;
	}

	public override string ToString()
	{
		return
			$"ItemStack{{Name={Name}, Id={Id}, Amount={Amount}, Category={Category}, Description={ToolTipDescription}, MaxStackSize={MaxStackSize}, BaseValue={BaseValue}, Components={Components}}}";
	}
}
