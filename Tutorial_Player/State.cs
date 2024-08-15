using Godot;

namespace untitledplantgame.Tutorial_Player;

public partial class State : Node
{
    public TutorialPlayer Player;

    public override void _Ready()
    {
        
    }

    public virtual void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public virtual State Process(double delta)
    {
        return null;
    }
    
    public State Physics(double delta)
    {
        return null;
    }
    
    public State HandleInput(InputEvent inputEvent)
    {
        return null;
    }
}