using Godot;

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
	string Name { get; }

	/// <summary>
	///     The icon of the item
	/// </summary>
	Texture2D Icon { get; }

	/// <summary>
	///     The description of the item
	/// </summary>
	string Description { get; }

	/// <summary>
	///     The category of the item
	/// </summary>
	ItemCategory Category { get; }

	/// <summary>
	///     The base monetary value of the item
	/// </summary>
	public int BaseValue { get; }

	/// <summary>
	///     The amount of items in this stack
	/// </summary>
	int Amount { get; set; }

	/// <summary>
	///     The maximum amount of items in this stack
	/// </summary>
	int MaxStackSize { get; }

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
		where T : class, IComponent;

	/// <summary>
	///     Adds a component to the item
	/// </summary>
	/// <param name="component">A component</param>
	/// <typeparam name="T">Component type</typeparam>
	void AddComponent<T>(T component)
		where T : class, IComponent;

	/// <summary>
	///     Removes a component from the item
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
	/// <returns>Removed component</returns>
	T RemoveComponent<T>()
		where T : class, IComponent;

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
	
	// Additionals for convenience
	IItemStack Subtract(IItemStack that)
	{
		if (!HasSameIdAndProps(that))
		{
			throw new System.ArgumentException("Cannot subtract different items");
		}
		if (Amount < that.Amount)
		{
			throw new System.ArgumentException("Cannot subtract more than available");
		}
		var selfClone = Clone();
		selfClone.Amount -= that.Amount;
		return selfClone;
	}
}
