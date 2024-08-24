using Godot;

namespace untitledplantgame.Inventory;

public class Item : IStorable
{
    public string Name { get; }
    public Image Icon { get; }

    public Item(string name, Image icon)
    {
        Name = name;
        Icon = icon;
    }
}