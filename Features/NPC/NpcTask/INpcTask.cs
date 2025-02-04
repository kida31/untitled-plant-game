using System.Threading.Tasks;
namespace untitledplantgame.NPC.NpcTask;

/// <summary>
///		Common interface for every Task a Npc can execute.
/// </summary>
public interface INpcTask
{
	/// <summary>
	///		This method is called when a Routine executes all specified Tasks.
	///		While not every type of Task needs to know which Npc is executing it, the InitializeTask Method can be used to dynamically
	///		assign fields or link various Actions or events.
	/// </summary>
	/// <param name="owningNpc"></param>
	public void InitializeTask(Npc owningNpc);
	/// <summary>
	///		This method is supposed to give every Task a method which can be used as a starting point, i.e., for logging the start of a Task.
	/// </summary>
	public void StartTask();
	/// <summary>
	///		This method is supposed to give every Task a method which can be used as an end point, i.e., for logging the end of a Task.
	/// </summary>
	public void FinishTask();
	/// <summary>
	///		Since it's not guaranteed every task can be finished before the Npc is supposed to do something else, this method can force
	///		the npc to interrupt the current task and, i.e., spontaneously do something else, like talk to the player.
	/// </summary>
	public void InterruptCurrentTask();
	/// <summary>
	///		After a task has been interrupted, it can be resumed using "ResumeCurrentTask".
	///		If this method isn't called, the Npc won't attempt to start another Task OR Routine. 
	/// </summary>
	public void ResumeCurrentTask();
	/// <summary>
	///		Executes the Task and returns a C# "Task" object.
	///		A Task is always awaited before the next Task in a Routine can be started.
	/// </summary>
	/// <returns></returns>
	public Task ExecuteNpcTask();
}
