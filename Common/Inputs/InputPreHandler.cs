using System;
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
	private Logger _logger = new("InputPreHandler");
	private string[] _actions;

	public override void _Ready()
	{
		_actions = InputMap.GetActions().Select(s => s.ToString()).ToArray();

		foreach (var baseActionName in _actions.Where(a => a.StartsWith(BaseActionPrefix)))
		{
			var actionName = baseActionName.Substring(BaseActionPrefix.Length);
			InputMap.AddAction($"freeroam_{actionName}");
			var events = InputMap.ActionGetEvents(baseActionName);
			foreach (var inputEvent in events)
			{
				InputMap.ActionAddEvent($"freeroam_{actionName}", inputEvent);
			}
			
			InputMap.EraseAction(baseActionName);
		}
		
		// GameStateMachine.Instance.ChangeState(GameState.Book);

		void ReleaseAll()
		{
			foreach (var actionName in _actions)
			{
				var actionEvent = new InputEventAction();
				actionEvent.SetAction(actionName);
				actionEvent.SetPressed(false);
				actionEvent.SetStrength(0);
				Input.ParseInputEvent(actionEvent);
			}
		}

		GameStateMachine.Instance.StateChanged += (_, _) => ReleaseAll();
	}

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.F12))
		{
			GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.CurrentState == GameState.FreeRoam ? GameState.Book : GameState.FreeRoam);
			_logger.Debug($"Toggle gamestate to {GameStateMachine.Instance.CurrentState}");
		}
	}

	public override void _Input(InputEvent @event)
	{
		var actionName = _actions.FirstOrDefault(a => @event.IsAction(a));
		if (actionName == null)
		{
			// Irrelevant or ungoverned input
			return;
		}

		// Only let action pass if it prefixes with state name
		if (!actionName.StartsWith(GameStateMachine.Instance.CurrentState.Name.ToLower()))
		{
			GetViewport().SetInputAsHandled();
			_logger.Debug($"Blocked input {actionName}");
			return;
		}
		
		_logger.Info($"Input: {actionName}");
	}
}
