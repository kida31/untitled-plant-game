﻿using System;
using Godot;
using Godot.Collections;
using untitledplantgame.Item;

namespace untitledplantgame.Inventory;

public interface IItemStack
{
	/// <summary>
	///     The unique identifier of the item
	/// </summary>
	string Id { get; }

	/// <summary>
	///     The name of the item
	/// </summary>
	string Name { get; set; }
	
	Array<AComponent> Components { get; set; }

	/// <summary>
	///     The icon of the item
	/// </summary>
	Texture2D Icon { get; set; }

	/// <summary>
	///     The description of the item
	/// </summary>
	string ToolTipDescription { get; set; }

	/// <summary>
	///     The category of the item
	/// </summary>
	ItemCategory Category { get; set; }

	/// <summary>
	///     The base monetary value of the item
	/// </summary>
	public int BaseValue { get; set; }

	/// <summary>
	///     The amount of items in this stack
	/// </summary>
	int Amount { get; set; }

	/// <summary>
	///     The maximum amount of items in this stack
	/// </summary>
	int MaxStackSize { get; set; }

	// /// <summary>
	// /// The components of the item
	// /// </summary>
	// ReadOnlyCollection<IComponent> Components { get; }

	/// <summary>
	///     Gets the component of the specified type
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
	/// <returns>Component of the specified type</returns>
	T GetComponent<T>()
		where T : AComponent;
	
	T GetComponent<T>(T component)
		where T : AComponent => GetComponent<T>();

	/// <summary>
	///     Adds a component to the item
	/// </summary>
	/// <param name="component">A component</param>
	/// <typeparam name="T">Component type</typeparam>
	void AddComponent<T>(T component)
		where T : AComponent;

	/// <summary>
	///     Removes a component from the item
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
	/// <returns>Removed component</returns>
	T RemoveComponent<T>()
		where T : AComponent;

	/// <summary>
	///     Returns whether two item stacks are similar
	/// </summary>
	/// <param name="itemStack">Compared item stack</param>
	/// <returns>Whether two item stacks are similar</returns>
	bool HasSameId(IItemStack itemStack); // TODO: Name is subject to change

	/// <summary>
	///     Returns whether the contained item is the same. (More strict match than HasSameId)
	/// </summary>
	/// <param name="itemStack"></param>
	/// <returns></returns>
	public bool HasSameIdAndProps(IItemStack itemStack);

	public bool IsIdentical(IItemStack itemStack);

	IItemStack Clone();
}
