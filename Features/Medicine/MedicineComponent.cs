using Godot;
using untitledplantgame.Item;

namespace untitledplantgame.Medicine;

[GlobalClass]
public partial class MedicineComponent : AComponent
{
	[Export]
	private string name;
	[Export]
	private int value;
	
	public MedicineComponent(string name, int value)
	{
		this.name = name;
		this.value = value;
	}
	public MedicineComponent() {}
}
