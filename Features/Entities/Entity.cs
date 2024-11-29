using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using untitledplantgame.Common;
using untitledplantgame.Entities.GodotComponents;

namespace untitledplantgame.Entities;

public partial class Entity : Node2D
{
	private Logger _logger = new("EntityBeforeEnterTree");
	private readonly Dictionary<Type, IComponent> _components = new();

	[Export]
	public string MyName
	{
		get => GetComponent<NameComponent>()?.Name ?? "Unnamed";
		set { }
	}

	public override void _EnterTree()
	{
		_logger = new Logger(this);
	}

	public override void _Ready()
	{
		// Prefabs already have components set up. Hook these up
		var children = GetChildren();
		foreach (var child in children)
		{
			if (child is IComponent component)
			{
				_RegisterComponent(component);
			}
		}

		// Test
		var comp = new NameComponent();
		comp.Name = "Thomas";
		AddComponent(comp);

		var result = RemoveComponent(new NameComponent());
		_logger.Debug($"Removed component: {result}");
		_logger.Info("Ready!");
	}

	public void AddComponent<T>(T component, bool replace = false) where T : class, IComponent
	{
		_RegisterComponent(component);
		if (component is Node nodeComponent)
		{
			AddChild(nodeComponent);
		}
	}

	protected void _RegisterComponent<T>(T component, bool replace = false) where T : class, IComponent
	{
		if (component == null)
		{
			_logger.Error("Could not add component. Component is null.");
			return;
		}

		if (component.Entity != null)
		{
			_logger.Error("Could not add component. Component already has an owner.");
			return;
		}

		if (HasComponent<T>())
		{
			if (!replace)
			{
				_logger.Error("Could not add component. Entity already has a component of type " + typeof(T).Name);
				return;
			}

			RemoveComponent<T>();
		}

		component.Entity = this;
		_components.Add(component.GetType(), component);
	}

	public bool HasComponent<T>() where T : IComponent => _components.ContainsKey(typeof(T));

	public T GetComponent<T>() where T : class, IComponent
	{
		if (_components.TryGetValue(typeof(T), out var component))
		{
			return (T) component;
		}

		_logger.Warn($"There is no component of type {typeof(T).Name} in this entity. Consider using HasComponent() instead.");
		return null;
	}

	public T RemoveComponent<T>(T component) where T : class, IComponent
	{
		if (component == null)
		{
			_logger.Error("Cannot remove null component");
			return null;
		}

		if (!_components.TryGetValue(component.GetType(), out var existingComponent))
		{
			_logger.Error("Cannot remove. Component is not part of this entity.");
			return null;
		}

		if (existingComponent != component)
		{
			_logger.Error("Cannot remove. Component is not the same instance as the one on the entity.");
			return null;
		}

		// Unregister
		Assert.AssertTrue(_components.Remove(component.GetType()));
		component.Entity = null;

		// Remove from tree
		if (component is Node nodeComponent && nodeComponent.GetParent() == this)
		{
			RemoveChild(nodeComponent);
		}

		return component;
	}

	public T RemoveComponent<T>() where T : class, IComponent
	{
		if (_components.TryGetValue(typeof(T), out var component))
		{
			return RemoveComponent(component as T);
		}

		_logger.Warn("Cannot remove. There is no component of type " + typeof(T).Name);
		return null;
	}
}
