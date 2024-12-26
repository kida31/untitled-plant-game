using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This box tracks changes in its children and adjusts its own spacing to negative 
/// </summary>
[Tool]
public partial class HBoxTabContainer : HBoxContainer
{
	[Export] public float MaxWidth = 0;
	[Export] private int _desiredSeparation = 0;

	private List<Control> _children;

	public override void _Ready()
	{
		_children = GetChildren().OfType<Control>().ToList();
		SetSeparation(_desiredSeparation); // Updating value to editor export;

		ChildEnteredTree += OnChildEnteredTree;
		ChildOrderChanged += OnChildOrderChanged;
		ChildExitingTree += OnChildExitingTree;
		SizeFlagsChanged += ForceSeparationUpdate;
		VisibilityChanged += ForceSeparationUpdate;
	}
	private void SetSeparation(int value) {
		// GD.Print("Overriding separation " + value);
		AddThemeConstantOverride("separation", value);
	}
	private void ForceSeparationUpdate() {
		if (MaxWidth <= 0 || _children.Count <= 1) {
			// GD.Print("No children. Resetting separation");
			SetSeparation(_desiredSeparation); 
			return;
		}

		var spaceBetween = MaxWidth - _children.Select(c => c.GetRect().Size.X).Sum();
		// GD.Print($"Spacebetween={spaceBetween}");
		var maxSeparation = (int) spaceBetween / (_children.Count - 1);
		// GD.Print($"maxSpacing={maxSeparation}");
		SetSeparation(Math.Min(maxSeparation, _desiredSeparation));
	}

	private void OnChildEnteredTree(Node node)
	{
		_children = GetChildren().OfType<Control>().ToList();
		ForceSeparationUpdate();
	}

	private void OnChildOrderChanged()
	{
		_children = GetChildren().OfType<Control>().ToList();
		ForceSeparationUpdate();
	}

	private void OnChildExitingTree(Node node)
	{
		_children = GetChildren().OfType<Control>().Where(c => c != node).ToList();
		ForceSeparationUpdate();
	}
}
