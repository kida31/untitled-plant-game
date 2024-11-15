﻿using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory.Alt;

/// <summary>
///     https://github.com/Bukkit/Bukkit/blob/master/src/main/java/org/bukkit/inventory/ItemStack.java
/// </summary>
public class ItemStack : IItemStack
{
	private readonly List<IComponent> _components = new();
	private readonly Logger _logger = new("ItemStack");

	public ItemStack(
		string id,
		string name,
		Texture2D icon,
		string description,
		ItemCategory category,
		int maxStackSize,
		int baseValue,
		int amount = 1
	)
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

	public string Id { get; }
	public string Name { get; }
	public Texture2D Icon { get; }
	public string Description { get; }
	public ItemCategory Category { get; }
	public int Amount { get; set; }
	public int MaxStackSize { get; }
	public int BaseValue { get; }

	public T GetComponent<T>()
		where T : class, IComponent
	{
		var idx = _components.FindIndex(component => component is T);
		return idx != -1 ? (T)_components[idx] : null;
	}

	public void AddComponent<T>(T component)
		where T : class, IComponent
	{
		Debug.Assert(GetComponent<T>() == null, "Component already exists");
		_components.Add(component);
	}

	public T RemoveComponent<T>()
		where T : class, IComponent
	{
		var idx = _components.FindIndex(component => component is T);
		if (idx == -1)
		{
			return null;
		}

		var component = (T)_components[idx];
		_components.RemoveAt(idx);
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
