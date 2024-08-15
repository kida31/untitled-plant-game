using Godot;
using System;
using System.Diagnostics;
using System.Transactions;

public partial class state : Node
{
	//stores a reference to the player that this state belongs to
	private static Player _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	//what happens when the player enters this state
	public void Enter()
	{
		
	}
	
	//what happens when the player exits this state
	public void Exit()
	{
		
	}

	//what happens during the _process update in this state
	private state Process(float _delta)
	{
		return null;
	}

	//what happens during the _physics_process update in this state
	private state Physics(float _delta)
	{
		return null;
	}

	//what happens with input events in this state
	private state HandleInput(InputEvent _event)
	{
		return null;

	}
	
	
}
