using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace untitledplantgame.Item.Components;

public partial class TagsComponent : AComponent, ICollection<TagsComponent.Tags>
{
	private HashSet<Tags> _tags;

	public enum Tags
	{
		IsDrieable,
		IsDried,
		IsLeaf,
		IsFruit,
		IsFlower,
		IsUnknown,
		IsPriceless,
		IsLegendary,
		IsMagical,
		IsInedible,
		IsEdible,
		IsWorthless,
		IsFish,
	}

	public TagsComponent(params Tags[] items)
	{
		_tags = new HashSet<Tags>(items);
	}

	public TagsComponent()
	{
	}

	public override TagsComponent Clone()
	{
		return new TagsComponent(_tags.ToArray());
	}

	public override bool Equals(AComponent other)
	{
		return other is TagsComponent component && _tags.SetEquals(component._tags);
	}

	public IEnumerator<Tags> GetEnumerator()
	{
		return _tags.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(Tags item)
	{
		_tags.Add(item);
	}

	public void Clear()
	{
		_tags.Clear();
	}

	public bool Contains(Tags item)
	{
		return _tags.Contains(item);
	}

	public void CopyTo(Tags[] array, int arrayIndex)
	{
		_tags.CopyTo(array, arrayIndex);
	}

	public bool Remove(Tags item)
	{
		return _tags.Remove(item);
	}

	public override string ToString()
	{
		return "Tags: " + string.Join(", ", _tags);
	}

	public int Count => _tags.Count;
	public bool IsReadOnly => false;
}
