using System.Threading.Tasks;
using untitledplantgame.NPC.Routine;

namespace untitledplantgame.NPC.NpcTask;

public interface INpcTask
{
	public void InitializeTask(Npc owningNpc);
	public void StartTask();
	public void FinishTask();
	public bool IsTaskActive();
	public void InterruptCurrentTask();
	public void ResumeCurrentTask();
	public Task ExecuteNpcTask();
}
