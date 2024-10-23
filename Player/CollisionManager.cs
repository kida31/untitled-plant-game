using Godot;
using System;
using System.Collections.Generic;

public partial class CollisionManager : Node
{
    private Dictionary<string, Action> _npcDialogueActions;

    public override void _Ready()
    {
        _npcDialogueActions = new Dictionary<string, Action>
        {
            { "Mother", () => ShowDialogue("Mother says: Hello Kid!") },
            { "Brother", () => ShowDialogue("Brother says: Sup sis!") }
            // Add more NPCs and dialogues here
        };
    }

    public void HandleNpcCollision(string npcName)
    {
        if (_npcDialogueActions.TryGetValue(npcName, out var action))
        {
            action.Invoke(); //ok
        }
        else
        {
            GD.Print("Unknown NPC collided.");
        }
    }

    private void ShowDialogue(string dialogue)
    {
        GD.Print(dialogue);
        // You can replace this with a UI update logic to show the dialogue on screen.
    }
}