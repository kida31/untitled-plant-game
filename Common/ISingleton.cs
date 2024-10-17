using Godot;
using System;

public interface ISingleton<T>
{
    static T Instance { get; private set; }
    static T GetSomething => throw new NotImplementedException();
}
