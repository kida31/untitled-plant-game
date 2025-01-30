using Godot;
using untitledplantgame.Common;
using untitledplantgame.Statistics;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Systems;

[Singleton]
public partial class CurrencyFaithOfficer : Node
{
	private const int StartingCurrency = 40;
	public static CurrencyFaithOfficer TheOneAndOnly { get; private set; }
	public static CurrencyFaithOfficer Instance => TheOneAndOnly; // Alias for TheOneAndOnly
	private Stat _faith;
	private Stat _currency;
	private readonly Logger _logger = new("CurrencyFaithOfficer");

	public override void _Ready()
	{
		if (TheOneAndOnly != null)
		{
			_logger.Warn("Multiple instances of CurrencyFaithOfficer found; deleting the new one");
			QueueFree();
			return;
		}

		_faith = new Stat(0, new Faith(), false);
		_currency = new Stat(StartingCurrency,new Currency() , false); //peanut made me do it

		TheOneAndOnly = this;
	}

	public void ChangeAny(IStatType statType, int change)
	{
		switch(statType) 
		{
			case Faith:
				_faith.AddStatModifier(change); 
				_logger.Debug($"Added Currency={change} New Balance={_faith.GetModifiedStatValue()}");
				EventBus.Instance.FaithChanged(change);
				break;
			case Currency:
				
				_currency.AddStatModifier(change);
				_logger.Debug($"Added Currency={change} New Balance={_currency.GetModifiedStatValue()}");
				EventBus.Instance.CurrencyChanged(change);
				break;
			default:
				_logger.Warn("You wanted to give the CFO a job out of his scope.");
				break;
		}
	}

	public int GetCurrentFaith()
	{
		return _faith.GetModifiedStatValue();
	}

	public int GetCurrentCurrency()
	{
		return _currency.GetModifiedStatValue();
	}
}
