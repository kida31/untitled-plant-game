using System;
using Godot;

namespace untitledplantgame.Inventory;

public class Backpack
{
	// Fields
	private readonly IStorable[] _content;

	// Constructor
	public Backpack(int maxSize)
	{
		_content = new IStorable[maxSize];
	}

	// Events
	public event Action<IStorable[]> ContentUpdated;

	// Properties
	public int Size => _content.Length;
	public IStorable[] Content => (IStorable[])_content.Clone();

	// Methods

	/// <summary>
	///     Rearranges the content of the backpack. So that there are no gaps of null between items.
	/// </summary>
	public void RearrangeContent()
	{
		Array.Sort(
			_content,
			(a, b) =>
			{
				if (a == null)
					return 1;
				if (b == null)
					return -1;
				return string.CompareOrdinal(a.Name, b.Name);
			}
		);
		ContentUpdated?.Invoke(Content);
	}

	/// <summary>
	///     Adds an item to the backpack. If there is no space left, an error is printed to the console.
	/// </summary>
	/// <param name="item"></param>
	public void Add(IStorable item)
	{
		for (var i = 0; i < _content.Length; i++)
			if (_content[i] == null)
			{
				_content[i] = item;
				ContentUpdated?.Invoke(Content);
				return;
			}

		GD.PrintErr("No space left in backpack.");
	}

	/// <summary>
	///     Removes specfied item from the backpack. If the item is not found, an error is printed to the console.
	/// </summary>
	/// <param name="item"></param>
	public void Remove(IStorable item)
	{
		var index = Array.IndexOf(_content, item);
		if (index == -1)
		{
			GD.PrintErr("Item not found in backpack.");
		}
		else
		{
			_content[index] = null;
			ContentUpdated?.Invoke(Content);
		}
	}
}
