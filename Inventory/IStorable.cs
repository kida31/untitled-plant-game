using Godot;

namespace untitledplantgame.Inventory;

public interface IStorable
{
    string Name
    {
        get;
    }
    
    Texture2D Icon
    {
        get;
    }
}