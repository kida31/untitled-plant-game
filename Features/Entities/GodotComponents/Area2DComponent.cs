using Godot;
using System;

namespace untitledplantgame.Entities.GodotComponents;

public partial class Area2DComponent : Area2D, IComponent
{
	public Entity Entity { get; set; }
}
