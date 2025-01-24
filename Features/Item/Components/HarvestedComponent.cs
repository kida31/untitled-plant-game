using untitledplantgame.Plants;

namespace untitledplantgame.Item.Components;

public partial class HarvestedComponent : AComponent
{
	private string _plantName;
	private GrowthStage _stage;

	public HarvestedComponent(string plantName, GrowthStage stage)
	{
		_plantName = plantName;
		_stage = stage;
	}

	public HarvestedComponent()
	{
	}

	public override AComponent Clone()
	{
		return new HarvestedComponent(_plantName, _stage);
	}

	public override bool Equals(AComponent other)
	{
		return other is HarvestedComponent component && component._plantName == _plantName && component._stage == _stage;
	}

	public override AComponent Combine(AComponent otherComponent)
	{
		return new HarvestedComponent();
	}
}
