using Godot;
using untitledplantgame.Common;
using untitledplantgame.Dialogue.Models;

namespace untitledplantgame.Dialogue.Events;

[GlobalClass]
public partial class CameraPan : DialogueEvent
{
	[Export] private Vector2 _cameraPosition;
    
    public override void Execute()
    {
	    EventBus.Instance.MoveCameraAndBack(_cameraPosition);
    }
}
