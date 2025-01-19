namespace untitledplantgame.Common;

/// <summary>
/// This attribute marks any classes or methods that have not been extensively tested.
/// This is often referred to as "experimental" as well
/// </summary>
public class Unstable : System.Attribute
{
	string Message;

	public Unstable(string message)
	{
		Message = message;
	}
}
