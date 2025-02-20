﻿namespace untitledplantgame.Common.Inputs.GameActions;

/// <summary>
///		List of book game actions. Only active when the book is open.
/// </summary>
public static class Book
{
	public const string Left = "book_left";
	public const string Right = "book_right";
	public const string Up = "book_up";
	public const string Down = "book_down";

	public const string North = "book_north";
	public const string South = "book_south";
	public const string East = "book_east";
	public const string West = "book_west";

	public const string BumperLeft = "book_lb";
	public const string BumperRight = "book_rb";
	
	public const string TriggerLeft = "book_lt";
	public const string TriggerRight = "book_rt";

	public const string Confirm = South;
	public const string CloseBook = North;
	public const string Back = East;
	public const string Special = West;
}
