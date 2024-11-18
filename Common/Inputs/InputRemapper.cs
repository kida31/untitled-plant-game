using System.Diagnostics;
using System.Linq;
using Godot;
using untitledplantgame.Common.GameStates;

namespace untitledplantgame.Common.Inputs;

/// <summary>
/// </summary>
public partial class InputRemapper : Node
{
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

	private void BindInputEvents(GameState context)
	{
		_logger.Info($"Bind input events for {context}");
		foreach (var baseName in _actionNames)
		{
			var originalActionName = $"{BaseActionPrefix}{baseName}";
			var actionName = $"{context.Name}_{baseName}";

			// Copy input events
			_logger.Debug($"Copy input bindings {originalActionName} -> {actionName}");
			var inputs = InputMap.ActionGetEvents(originalActionName);
			Debug.Assert(inputs.Count > 0, "No inputs bound to action.");

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
			_logger.Debug($"Erase input bindings for {actionName}");
			InputMap.ActionEraseEvents(actionName);
		}
	}

	private void OnGameStateChanged(GameState prevState, GameState newState)
	{
		void TriggerReleaseOnAllActions()
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

		// Not sure if this is necessary or would feel awkward.
		// TriggerReleaseOnAllActions();
		UnbindInputEvents(prevState);
		BindInputEvents(newState);
	}

	public static string GetBaseAction(string actionName)
	{
		var parts = actionName.Split('_');
		return $"{BaseActionPrefix}{parts[1]}";
	}
}
