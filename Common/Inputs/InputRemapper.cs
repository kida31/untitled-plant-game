﻿using System;
using System.Diagnostics;
using System.Linq;
using Godot;
using untitledplantgame.Common.GameStates;

namespace untitledplantgame.Common.Inputs;

/// <summary>
/// This Singleton class is responsible for remapping inputs based on the current game state.
/// Input will only trigger actions corresponding to the current game state.
/// This way objects do not have to check the game state themselves.
/// </summary>
public partial class InputRemapper : Node
{
	/// <summary>
	/// NOTE: This is a workaround to map the south button to the accept action.
	/// Alternatively directly map ui_accept in Godot Input maps, but do not forget to match it to base_south
	/// </summary>
	private const bool MapSouthToAccept = true;
	
	/// <summary>
	/// Returns they Key that is mapped to this action
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	[Unstable("Debugging only")]
	public static Key GetKey(string actionName) => ControlScheme.GetKey(actionName);

	/// <summary>
	/// Returns they gamepad button that is mapped to this action
	/// </summary>
	/// <param name="actionName"></param>
	/// <returns></returns>
	[Unstable("Debugging only")]
	public static JoyButton GetButton(string actionName) => ControlScheme.GetButton(actionName);

	private const string BaseActionPrefix = "base_";

	private string[] _actionNames;
	private Logger _logger;

	public override void _Ready()
	{
		_logger = new Logger(this);

		// Save base actions
		_actionNames = InputMap
			.GetActions()
			.Select(s => s.ToString())
			.Where(s => s.StartsWith(BaseActionPrefix))
			.Select(s => s.Substring(BaseActionPrefix.Length))
			.ToArray();

		// Make placeholder actions for all game states
		foreach (var gs in GameState.GetValues())
		{
			foreach (var actionName in _actionNames)
			{
				var action = $"{gs.Name}_{actionName}";
				_logger.Debug($"Add action: {action}");
				InputMap.AddAction(action);
			}
		}

		if (MapSouthToAccept)
		{
			BindSouthAsAccept();
		}
		// Bind inputs to actions for current context
		BindInputEvents(GameStateMachine.Instance.CurrentState);

		GameStateMachine.Instance.StateChanged += OnGameStateChanged;
	}
	
	private void BindSouthAsAccept()
	{
		const string southAction = "base_south";
		const string acceptAction = "ui_accept";
		
		var inputs = InputMap.ActionGetEvents(southAction);
		foreach (var e in inputs)
		{
			InputMap.ActionAddEvent(acceptAction, e);
		}
	}

	private void BindInputEvents(GameState context)
	{
		_logger.Info($"Bind input events for {context}");
		foreach (var baseName in _actionNames)
		{
			var originalActionName = $"{BaseActionPrefix}{baseName}";
			var actionName = $"{context.Name}_{baseName}";

			// Copy input events
			// _logger.Debug($"Copy input bindings {originalActionName} -> {actionName}");
			var inputs = InputMap.ActionGetEvents(originalActionName);
			Assert.AssertTrue(inputs.Count > 0, "No inputs bound to action.");

			foreach (var e in inputs)
			{
				InputMap.ActionAddEvent(actionName, e);
			}
		}
	}

	private void UnbindInputEvents(GameState context)
	{
		_logger.Info($"Unbind input events for {context}");
		foreach (var baseName in _actionNames)
		{
			// Unregister inputs
			var actionName = $"{context.Name}_{baseName}";
			// _logger.Debug($"Erase input bindings for {actionName}");
			InputMap.ActionEraseEvents(actionName);
		}
	}

	private void OnGameStateChanged(GameState prevState, GameState newState)
	{
		void ReleaseAllActions()
		{
			foreach (var actionName in InputMap.GetActions())
			{
				Input.ActionRelease(actionName);
			}
		}

		// Not sure if this is necessary or would feel awkward.
		ReleaseAllActions();
		UnbindInputEvents(prevState);
		BindInputEvents(newState);
	}

	public static string GetBaseAction(string actionName)
	{
		var parts = actionName.Split('_');
		return $"{BaseActionPrefix}{parts[^1]}"; // prefix + last part
	}
}
