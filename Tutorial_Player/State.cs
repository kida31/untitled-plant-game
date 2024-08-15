using Godot;

namespace untitledplantgame.Tutorial_Player;

public abstract class State : Node
{
    public TutorialPlayer Player;

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual State Process(double delta)
    {
        return null;
    }
    
    public virtual State Physics(double delta)
    {
        return null;
    }
    
    public virtual State HandleInput(InputEvent inputEvent)
    {
        return null;
    }
}