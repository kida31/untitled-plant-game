using System;
using Godot;

namespace untitledplantgame.Item.Components;

//
public partial class Basil : AComponent
{
	public Basil() { } //needed to instantiate the class

	public override AComponent Clone()
	{
		return new Basil();
	}
}
