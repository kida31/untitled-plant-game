using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using untitledplantgame.Common;

namespace untitledplantgame.Inventory;

/// <summary>
/// 
/// </summary>

/// <summary>
/// Represents a cursor and its inventory (one item stack)
/// </summary>
interface ICursorInventory
{
	event Action ContentChanged;
	IItemStack Content { get; }
	bool CanClick(IInventory inventory, int itemIndex);
	void HandleClick(IInventory inventory, int itemIndex);
	bool CanPickUp(IInventory inventory, int itemIndex);
	void PickUp(IInventory inventory, int itemIndex);
	bool CanPutDown(IInventory inventory, int itemIndex);
	void PutDown(IInventory inventory, int itemIndex);
	bool CanStack(IInventory inventory, int itemIndex);
	void Stack(IInventory inventory, int itemIndex);
	bool CanSwap(IInventory inventory, int itemIndex);
	void Swap(IInventory inventory, int itemIndex);
	void ReturnPickUp();
}

/// <summary>
/// 
/// </summary>
[Singleton]
public class CursorHand : ICursorInventory
{
	// Singleton (keep this at the top)
	private static readonly Lazy<CursorHand> LazySingleton = new(() => new CursorHand());
	public static CursorHand Instance => LazySingleton.Value;
	public event Action ContentChanged;
	public IItemStack Content => _content;


	private IItemStack _content;
	private readonly Logger _logger;
	private IInventory _pickupOrigin;
	private int _pickupOriginIndex;

	private CursorHand()
	{
		_logger = new("Cursor");
		_pickupOrigin = null;
		_pickupOriginIndex = -1;
		_content = null;
	}

	public void HandleClick(IInventory inventory, int itemIndex)
	{
		Assert.AssertTrue(CanClick(inventory, itemIndex));

		if (CanPickUp(inventory, itemIndex))
		{
			PickUp(inventory, itemIndex);
			return;
		}

		if (CanPutDown(inventory, itemIndex))
		{
			PutDown(inventory, itemIndex);
			return;
		}

		if (CanStack(inventory, itemIndex))
		{
			Stack(inventory, itemIndex);
			return;
		}

		if (CanSwap(inventory, itemIndex))
		{
			Swap(inventory, itemIndex);
			return;
		}

		_logger.Warn("Cursor could not handle click. Doing nothing. Consider using 'CanClick' before invoking this");
	}

	public bool CanClick(IInventory inventory, int itemIndex)
	{
		return new List<Func<IInventory, int, bool>>() { CanPickUp, CanPutDown, CanStack, CanSwap }.Any(f => f(inventory, itemIndex));
	}

	public bool CanPickUp(IInventory inventory, int itemIndex)
	{
		return _content == null && inventory.GetItem(itemIndex) != null;
	}

	public void PickUp(IInventory inventory, int itemIndex)
	{
		throw new NotImplementedException();
	}


	public bool CanPutDown(IInventory inventory, int itemIndex)
	{
		return _content != null && inventory.GetItem(itemIndex) == null;
	}

	public void PutDown(IInventory inventory, int itemIndex)
	{
		throw new NotImplementedException();
	}

	public bool CanStack(IInventory inventory, int itemIndex)
	{
		return _content != null && _content.HasSameIdAndProps(inventory.GetItem(itemIndex));
	}

	public void Stack(IInventory inventory, int itemIndex)
	{
		throw new NotImplementedException();
	}

	public bool CanSwap(IInventory inventory, int itemIndex)
	{
		return true; // I don't know why this would not work
	}

	public void Swap(IInventory inventory, int itemIndex)
	{
		throw new NotImplementedException();
	}

	public void ReturnPickUp()
	{
		if (_content == null)
		{
			_logger.Info("There is no item to return");
			return;
		}

		if (_pickupOrigin == null || _pickupOriginIndex < 0)
		{
			_logger.Warn("No origin location saved to return item to");
			TryDropContent();
			return;
		}

		var destination = _pickupOrigin.GetItem(_pickupOriginIndex);

		if (destination != null)
		{
			_logger.Error("Original space is occupied");
			TryDropContent();
			return;
		}

		var remainder = _pickupOrigin.AddItemToSlot(_pickupOriginIndex, _content);

		if (remainder != null)
		{
			// This might be the case with items that have special inventory restrictions
			_logger.Error("Could not put item back to origin although destination is free. Unexpected Error");
			TryDropContent();
			return;
		}

		_content = null;
		_pickupOrigin = null;
		_pickupOriginIndex = -1;
		_logger.Info("Returned item back to origin");
	}

	private void TryDropContent()
	{
		// TODO:
	}
}