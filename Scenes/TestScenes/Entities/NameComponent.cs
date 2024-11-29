namespace untitledplantgame.Entities;

/// <summary>
/// This is an example component
/// </summary>
public class NameComponent : IComponent
{
	public Entity Entity { get; set; }

	// Own properties
	public string Name { get; set; }
}
