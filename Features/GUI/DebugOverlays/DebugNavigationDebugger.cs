using Godot;
using untitledplantgame.Common;

public partial class DebugNavigationDebugger : Control
{
    private Viewport _vp;
    private Logger _logger;

    private Control _currentFocus;

    public override void _Ready()
    {
        _logger = new(this);
        var vp = GetViewport();
        vp.GuiFocusChanged += (ctrl) => _currentFocus = ctrl;
        _currentFocus = vp.GuiGetFocusOwner();

        var timer = new Timer();
        timer.Autostart = true;
        timer.WaitTime = 0.5f;
        timer.Timeout += UpdateDebugger;
        AddChild(timer);
    }

    public override void _Draw()
    {
        if (_currentFocus == null || !IsVisibleInTree())
        {
            return;
        }

        DrawNeighbourNavigation(_currentFocus, Side.Left);
        DrawNeighbourNavigation(_currentFocus, Side.Right);
        DrawNeighbourNavigation(_currentFocus, Side.Top);
        DrawNeighbourNavigation(_currentFocus, Side.Bottom);
    }

    private void UpdateDebugger() => UpdateDebugger(null);
    private void UpdateDebugger(Control focusedControl)
    {
        _vp ??= GetViewport();
        QueueRedraw();
    }


    private void DrawNeighbourNavigation(Control ctrl, Side side, float width = 2)
    {
        var predefinedPath = side switch
        {
            Side.Left => ctrl.FocusNeighborLeft,
            Side.Right => ctrl.FocusNeighborRight,
            Side.Top => ctrl.FocusNeighborTop,
            Side.Bottom => ctrl.FocusNeighborBottom,
            _ => null,
        };

        Control neighbour;
        Color color;

        if (predefinedPath == null || predefinedPath.IsEmpty)
        {
            neighbour = ctrl.FindValidFocusNeighbor(side);
            color = Colors.OrangeRed;
        }
        else
        {
            neighbour = ctrl.GetNode<Control>(predefinedPath);
            color = Colors.BlueViolet;
        }

        var offset = side switch
        {
            Side.Left => Vector2.Left,
            Side.Right => Vector2.Right,
            Side.Top => Vector2.Up,
            Side.Bottom => Vector2.Down,
            _ => Vector2.Zero,
        } * width * 2;

        if (neighbour != null)
        {
            DrawLineWithCaps(
              ctrl.GetGlobalRect().GetCenter() + offset,
              neighbour.GetGlobalRect().GetCenter(),
              color,
              width,
              Line2D.LineCapMode.Box,
              Line2D.LineCapMode.Round
            );
        }
    }

    private void DrawLineWithCaps(Vector2 start, Vector2 end, Color color, float width = -1,
                             Line2D.LineCapMode lineCapBegin = Line2D.LineCapMode.None,
                             Line2D.LineCapMode lineCapEnd = Line2D.LineCapMode.None,
                             bool antialiased = false)
    {
        // Calculate direction for square caps
        Vector2 adjustedStart = start;
        Vector2 adjustedEnd = end;

        // Draw the main line
        DrawLine(adjustedStart, adjustedEnd, color, width, antialiased);

        // Handle Caps: draw circles or boxes at ends
        void DrawCap(Vector2 center, float size, Line2D.LineCapMode mode)
        {
            if (mode == Line2D.LineCapMode.Box)
            {
                var rect = new Rect2(center - (Vector2.One * size), size * 2, size * 2);
                DrawRect(rect, color);
            }
            else if (mode == Line2D.LineCapMode.Round)
            {
                DrawCircle(center, size * 1.5f, color);
            }
        }

        DrawCap(start, width, lineCapBegin);
        DrawCap(end, width, lineCapEnd);
    }
}
