using System.Diagnostics;
using System.Linq;

public class ItemContainerController
{
    private static ItemContainerController _instance;
    public ItemContainerController Instance()
    {
        if (_instance == null)
        {
            _instance = new ItemContainerController();
        }
        return _instance;
    }

    private ItemContainerController() { }

    public bool TryTransfer<T, U>(
        ref StorableStack<T> from,
        ref StorableStack<U> to,
        int quantity)
        where T : class, IStorable
        where U : class, IStorable
    {
        var other = from.Item as U;
        Debug.Assert(other != null);
        from += to;
        return true;
    }

    public void Test<T>(        StorableStack<T> stack) where T: class, IStorable {
    }
}