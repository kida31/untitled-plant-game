namespace untitledplantgame.Common;

public class MayBeBuggyAttribute: System.Attribute
{
    string Message;
    public MayBeBuggyAttribute(string message) {
        Message = message;
    }
}