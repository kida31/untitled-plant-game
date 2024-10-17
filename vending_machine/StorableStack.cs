using System;

public struct StorableStack<T> : IEquatable<StorableStack<T>> where T : class, IStorable
{
    public T Item;
    public int Quantity;

    public StorableStack(T item, int quantity) {
        this.Item = item;
        this.Quantity = quantity;
    }

    public bool Equals(StorableStack<T> other)
    {
        return this.Item == other.Item && this.Quantity == other.Quantity;
    }

    public static bool operator ==(StorableStack<T> a, StorableStack<T> b) => a.Equals(b);
    public static bool operator !=(StorableStack<T> a, StorableStack<T> b) => !(a == b);
    public static StorableStack<T> operator +(StorableStack<T> a, StorableStack<T> b) {
        if (a.Item != b.Item) throw new Exception("Unsupported Operation");
        return new StorableStack<T>(a.Item, a.Quantity + b.Quantity);
    }
}