using untitledplantgame.Common;
using untitledplantgame.Tools;

public class Toolbelt
{
    private Tool[] _tools;

    public Tool CurrentTool => _toolIndex < 0 ? null : _tools[_toolIndex];
    private int _toolIndex;
    private readonly Logger _logger;

    public Toolbelt() : this(new Tool[] { })
    {

    }

    public Toolbelt(Tool[] tools)
    {
        _tools = tools;
        _toolIndex = tools.Length - 1; // -1 if it's empty
        _logger = new("Toolbelt");
    }

    public void GoToNext()
    {
        // I feel like theres some smarter math to do here
        if ((_tools?.Length ?? 0) <= 0)
        {
            _toolIndex = -1;
        }
        else
        {
            _toolIndex = (_toolIndex + 1) % _tools.Length;
            _logger.Info("Switch to tool: " + CurrentTool);
        }
    }

    public void GoToPrevious()
    {
        // I feel like theres some smarter math to do here
        if ((_tools?.Length ?? 0) <= 0)
        {
            _toolIndex = -1;
        }
        else
        {
            _toolIndex = (_toolIndex + _tools.Length + 1) % _tools.Length;
            _logger.Info("Switch to tool: " + CurrentTool);
        }
    }
}