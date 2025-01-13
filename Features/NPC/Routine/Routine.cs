using Godot;
using Godot.Collections;
using untitledplantgame.NPC.Task;

namespace untitledplantgame.NPC.Routine;

public abstract partial class Routine : Node
{
	public abstract Array<NpcTask> GetAllNpcTasks();
}
