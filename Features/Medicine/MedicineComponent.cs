using System;
using Godot;
using untitledplantgame.Item;

namespace untitledplantgame.Medicine;

[GlobalClass] [Obsolete("This class is a placeholder.")]
public partial class MedicineComponent : AComponent //TODO: PLACEHOLDER
{
	[Export]
	private string _name;
	[Export]
	private int _value;
	
	public MedicineComponent(string name, int value)
	{
		_name = name;
		_value = value;
	}
	public MedicineComponent() {} //needed to instantiate the class

	public override AComponent Clone()
	{
		return new MedicineComponent(_name, _value);
	}
}
