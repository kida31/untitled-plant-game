using System.Threading.Tasks;

namespace untitledplantgame.NPC.NpcTask;

public interface INpcTask
{
	public void StartTask();
	public void FinishTask();
	public Task ExecuteNpcTask();
}
