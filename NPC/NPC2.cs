using Godot;

public partial class NPC2 : Area2D, IInteractable
{
    [Export] private string _npcName;
    [Export] private NpcLogic _npcLogicNode;
    
    public override void _Ready()
    {
        AddToGroup("Interactables");
        var eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.Connect("NPCInteracted", new Callable(this, nameof(OnNPCInteracted)));
    }

    private void OnBodyEntered(Node body)
    {
        _npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
    }
    
    public Vector2 GetGlobalInteractablePosition()
    {
        return GlobalPosition;
    }

    private void OnNPCInteracted(Node npc)
    {
        // Only react if the interaction is from NPC1
        if (npc is NPC1)
        {
            GD.Print("NPC2 COOOOOOOOOOOOOOOOOOOODEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            MoveLocalX(10);
        }
    }

    public void Interact()
    {
        
    }
}