using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using untitledplantgame.Inventory.GUI;

[Tool]
public partial class Clickable : Control, IPressable, IFocusable
{
    public event Action Pressed;

    public Clickable()
    {
        GuiInput += OnGuiInput;
    }

    public override string[] _GetConfigurationWarnings()
    {
        var messages = new List<string>();
        if (FocusMode == FocusModeEnum.None)
        {
            messages.Add("FocusMode is set to None. Are you sure you want to do this?");
        }

        if (MouseFilter == MouseFilterEnum.Ignore)
        {
            messages.Add("MouseFilter is set to Ignore. This will ignore clicks.");
        }

        static List<Control> CollectChildren(Node node)
        {
            var children = node.GetChildren();
            var grandChildren = children.SelectMany(c => CollectChildren(c));
            return children.Concat(grandChildren).OfType<Control>().ToList();
        }
        foreach (var c in CollectChildren(this).Where(c => c.MouseFilter == MouseFilterEnum.Stop))
        {
            messages.Add($"Child \"{c.Name}\" is set to MouseFilter.Stop. This may block clicks on this node");
        }

        return messages.ToArray();
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton button)
        {
            if (button.ButtonIndex == MouseButton.Left && button.Pressed)
            {
                Pressed?.Invoke();
            }
        }

        if (@event.IsAction("ui_accept"))
        {
            Pressed?.Invoke();
        }
    }
}
