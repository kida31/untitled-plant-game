using untitledplantgame.Inventory;

namespace untitledplantgame.Medicine;

public class MedicineComponent : IComponent
{
	public Illness[] Illness { get; private set; }
	public MedicinalEffect[] Effect { get; private set; }
	public bool Equals(IComponent other)
	{
		throw new System.NotImplementedException();
	}
}
