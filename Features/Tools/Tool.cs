using untitledplantgame.Inventory;

namespace untitledplantgame.Tools;

public interface Tool : ItemStackConvertible
{

	/*
	private Dictionary<Type, IComponent> _components = new();

	bool HasComponent<T>() where T : IComponent
	{
		return _components.ContainsKey(typeof(T));
	}

	IComponent GetComponent<T>() where T : IComponent
	{
		return _components.GetValueOrDefault(typeof(T), null);
	}

	IComponent AddComponent<T>(T component) where T : IComponent
	{
		_components[typeof(T)] = component;
		return component;
	}

	void RemoveComponent<T>() where T : IComponent
	{
		_components.Remove(typeof(T));
	}
	*/
}
