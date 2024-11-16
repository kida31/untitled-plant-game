using Godot;
//using Godot.Collections;
using System.Collections.Generic;

namespace untitledplantgame.TestScenes;

public partial class FocusManager : Node
{
	public static FocusManager Instance => _instance ??= new FocusManager();
	
	private static FocusManager _instance;
	private int _currentLayer;
	private Dictionary<int, Node> _layerTargets = new ();
	[Export] private Node _nodePath0;
	[Export] private Node _nodePath1;

	public override void _Ready()
	{
		_instance = this;

		_layerTargets[0] = _nodePath0;
		_layerTargets[1] = _nodePath1;
		
		// Script Execution Layer... Why doesn't Godot say something?
		SetActiveLayer(1);
	}

	public void RegisterLayer(int layer, Node target)
	{
		_layerTargets[layer] = target;
	}

	public void SetActiveLayer(int layer)
	{
		_currentLayer = layer;
	}

	public void HandleInput(InputEvent @event)
	{
		if (_layerTargets.ContainsKey(_currentLayer))
		{
			_layerTargets[_currentLayer].Call("OnInput", @event);
		}
	}
}
