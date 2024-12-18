using untitledplantgame.Medicine;

namespace untitledplantgame.Item.Components;

public partial class IllnessComponent : AComponent
{
	public Illness[] Illness;
	public IllnessComponent(Illness[] illness)
	{
		Illness = illness;
	}
	public override AComponent Clone() => new IllnessComponent(Illness);
}
