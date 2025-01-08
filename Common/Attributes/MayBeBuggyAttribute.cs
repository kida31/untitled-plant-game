namespace untitledplantgame.Common;

/// <summary>
/// This attribute marks any classes or methods that have not been extensively tested.
/// This is often referred to as "experimental" as well
/// </summary>
public class MayBeBuggyAttribute : System.Attribute
{
	string Message;

	public MayBeBuggyAttribute(string message)
	{
		Message = message;
	}
}
