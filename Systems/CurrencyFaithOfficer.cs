using Godot;
using System;
using untitledplantgame.Common;
using untitledplantgame.Event;
using untitledplantgame.Statistics;
using untitledplantgame.Statistics.StatTypes;

namespace untitledplantgame.Systems;

[Singleton]
public partial class CurrencyFaithOfficer : Node
{
	public static CurrencyFaithOfficer TheOneAndOnly { get; private set; }
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

		_faith = new Stat(100, new Faith(), false);
		_currency = new Stat(69420,new Currency() , false); //peanut made me do it

		TheOneAndOnly = this;
	}

	public void ChangeAny(IStatType statType, int change)
	{
		switch(statType) 
		{
			case Faith:
				_faith.AddStatModifier(change); 
				EventBus.Instance.FaithChanged(change);
				break;
			case Currency:
				_currency.AddStatModifier(change);
				EventBus.Instance.CurrencyChanged(change);
				break;
			default:
				_logger.Warn("You wanted to give the CFO a job out of his scope.");
				break;
		}
	}
	
}
