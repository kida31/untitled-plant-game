using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using untitledplantgame.Common.GameStates;
using static untitledplantgame.Common.Inputs.UPGActions;

namespace untitledplantgame.Common;

/// <summary>
/// 
/// </summary>
public partial class InputPreHandler : Node
{
	private const string BaseActionPrefix = "base_";
	
	private Logger _logger;
	private string[] _actionNames;

	public override void _Ready()
	{
		_logger = new(this);
		
		// Save base actions
		_actionNames = InputMap.GetActions()
			.Select(s => s.ToString())
			.Where(s => s.StartsWith(BaseActionPrefix))
			.Select(s => s.Substring(BaseActionPrefix.Length))
			.ToArray();

		// Make placeholder actions for all game states
		// and remove original
		foreach (var gs in GameState.GetValues())
		{
			foreach (var actionName in _actionNames)
			{
				var action = $"{gs.Name}_{actionName}";
				_logger.Debug($"Add action: {action}");
				InputMap.AddAction(action);
			}
		}
		
		// Bind inputs to actions for current context
		BindInputEvents(GameStateMachine.Instance.CurrentState);
		
		GameStateMachine.Instance.StateChanged += OnGameStateChanged;
	}

	public override void _Input(InputEvent e_)
	{
		foreach (var action in InputMap.GetActions())
		{
			if (e_.IsAction(action) && !action.ToString().StartsWith(BaseActionPrefix))
			{
				GD.Print($"{e_.AsText()}::{action}");
				return;
			}
		}
		
		if (e_ is InputEventKey e && e.Keycode == Key.F12 && e.IsPressed())
		{
			GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.CurrentState == GameState.FreeRoam
				? GameState.Book
				: GameState.FreeRoam);
			_logger.Debug($"Toggle gamestate to {GameStateMachine.Instance.CurrentState}");
		}
	}

	private void BindInputEvents(GameState context)
	{
		_logger.Info($"Add InputEvents for {context}");
		foreach (var baseName in _actionNames)
		{
			var originalActionName = $"{BaseActionPrefix}{baseName}";
			var actionName = $"{context.Name}_{baseName}";

			// Copy input events
			// _logger.Debug("Copy inputs for " + actionName + " from " + originalActionName);
			var inputs = InputMap.ActionGetEvents(originalActionName);
			Debug.Assert(inputs.Count > 0, "No inputs bound to action.");
			foreach (var e in inputs)
			{
				_logger.Debug($"Add {baseName}.{e.AsText()} to {actionName}");
				InputMap.ActionAddEvent(actionName, e);
			}
		}
	}

	private void UnbindInputEvents(GameState context)
	{
		_logger.Info($"Remove InputEvents for {context}");
		foreach (var baseName in _actionNames)
		{
			// Unregister inputs
			var actionName = $"{context.Name}_{baseName}";
			InputMap.ActionEraseEvents(actionName);
		}
	}

	private void OnGameStateChanged(GameState prevState, GameState newState)
	{
		void ReleaseAll()
		{
			foreach (var actionName in InputMap.GetActions())
			{
				var actionEvent = new InputEventAction();
				actionEvent.SetAction(actionName);
				actionEvent.SetPressed(false);
				actionEvent.SetStrength(0);
				Input.ParseInputEvent(actionEvent);
			}
		}

		// ReleaseAll();
		UnbindInputEvents(prevState);
		BindInputEvents(newState);
	}
}
