using Godot;

public partial class ProcessWindow : AcceptDialog
{
    private string _log;
    private TextEdit _textEdit;

    public override void _EnterTree()
    {
        _textEdit = new TextEdit()
        {
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill,
            // WrapMode = TextEdit.LineWrappingMode.Boundary,
            // AutowrapMode = TextServer.AutowrapMode.Arbitrary,
            IndentWrappedLines = true,
            ScrollSmooth = true,
            Editable = false,
            Text = _log,
        };
        AddChild(_textEdit);

    }

    public void PrintLine(string line)
    {
        _log += line + "\n";
        if (_textEdit != null) _textEdit.Text = _log;
    }
}
