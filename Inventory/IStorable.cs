using Godot;

namespace untitledplantgame.Inventory;

public interface IStorable
{
    string Name
    {
        get;
    }
    
    Image Icon
    {
        get;
    }
}