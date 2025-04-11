using untitledplantgame.NPC.RoutinePlanner;

namespace untitledplantgame.NPC.NpcTask;

/// <summary>
///		Interface for all Task Interruptions.
/// </summary>
public interface ITaskInterruption
{
	/// <summary>
	///		In order for a TaskInterruption to work, it must have access to the Npc's RoutinePlanner.
	///		In most cases, the NpcRoutinePlanner should be a child node of the npc, so implementing this method using "GetParent()" should
	///		work in most cases.
	///
	///		Of course, a cast may or may not be necessary.
	/// </summary>
	/// <returns></returns>
	public NpcRoutinePlanner GetRoutinePlanner();
}
