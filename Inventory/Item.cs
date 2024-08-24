using Godot;

namespace untitledplantgame.Inventory;

public class Item : IStorable
{
    public string Name { get; }
    public Texture2D Icon { get; }

    public Item(string name, Texture2D icon)
    {
        Name = name;
        Icon = icon;
    }
}