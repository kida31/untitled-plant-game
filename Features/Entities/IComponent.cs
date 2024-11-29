using System;

namespace untitledplantgame.Entities;

public interface IComponent
{
	Type GetImplementationType()
	{
		return GetType();
	}

	Entity Entity { get; set; }
}
