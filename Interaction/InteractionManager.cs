using Godot;
using System.Collections.Generic;

public partial class InteractionManager : Node2D
{
    [Export] private NodePath playerPath;
    [Export] private NodePath labelPath;
    
    private Node2D player;
    private Label label;
    
    private const string BaseText = "[E] to ";
    private List<InteractionArea> activeAreas = new List<InteractionArea>();
    private bool canInteract = true;

    public override void _Ready()
    {
    player = (Node2D)GetTree().GetFirstNodeInGroup("Player");
    label = GetNode<Label>("Label");

    if (player == null)
    {
        GD.PrintErr("Player node not found.");
    }
}

    public void RegisterArea(InteractionArea area)
    {
        activeAreas.Add(area);
    }

    public void UnregisterArea(InteractionArea area)
    {
        int index = activeAreas.IndexOf(area);
        if (index != -1){
            activeAreas.Remove(area);
        }
    }

    public override void _Process(double delta)
    {
        if (activeAreas.Count > 0 && canInteract)
        {
            activeAreas.Sort(SortByDistanceToPlayer);
            label.Text = BaseText + activeAreas[0].ActionName;
            label.GlobalPosition = activeAreas[0].GlobalPosition;
            label.GlobalPosition -= new Vector2(label.Size.X / 2, 36);
            label.Show();
        }
        else
        {
            label.Hide();
        }
    }

    private int SortByDistanceToPlayer(InteractionArea area1, InteractionArea area2)
    {
        float distance1 = player.GlobalPosition.DistanceTo(area1.GlobalPosition);
        float distance2 = player.GlobalPosition.DistanceTo(area2.GlobalPosition);
        return distance1.CompareTo(distance2);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") && canInteract)
        {
            if (activeAreas.Count > 0)
            {
                canInteract = false;
                label.Hide();
                
                activeAreas[0].Interact();

                canInteract = true;
            }
        }
    }
}
