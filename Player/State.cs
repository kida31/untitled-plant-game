using Godot;

namespace untitledplantgame.Player;

public partial class State : Node
{
    public Player Player;

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