using Godot;
using untitledplantgame.Inventory;

namespace untitledplantgame.Item;

[GlobalClass]
public abstract partial class AComponent : Resource, IComponent
{
	public AComponent()
	{
	}
}
