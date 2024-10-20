using Godot;

public partial class NPC : Area2D, IInteractable
{
    [Export] private string _npcName;
    [Export] private NpcLogic _npcLogicNode;
    
    public override void _Ready()
    {
        AddToGroup("Interactables");
    }

    private void OnBodyEntered(Node body)
    {
        _npcLogicNode.ManageNpcCollisionWithPlayer(body, _npcName);
    }
    
    public Vector2 GetGlobalInteractablePosition()
    {
        return GlobalPosition;
    }
    
    public void Interact()
    {
        _npcLogicNode.InteractionLogic();
    }
    
}