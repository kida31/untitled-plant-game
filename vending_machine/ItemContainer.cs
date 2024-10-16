using System.Collections.Generic;

public class ItemContainer<T> where T : class, IStorable
{
    private Dictionary<T, int> _items;

    public void AddItemStack(T item) {
        // Implementation here
    }

    public T PopItemStack(T item) {
        return null;
    }

    public T RemoveItem(T item, int quantity) {
        return null;
    }

    public Dictionary<T, int> Items => _items;
}