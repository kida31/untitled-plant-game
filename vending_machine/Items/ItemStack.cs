using System;

public struct ItemStack<T> : IEquatable<ItemStack<T>> where T : class, IStorable
{
    public T Item;
    public int Quantity;

    public ItemStack(T item, int quantity) {
        this.Item = item;
        this.Quantity = quantity;
    }

    public bool Equals(ItemStack<T> other)
    {
        return this.Item == other.Item && this.Quantity == other.Quantity;
    }

    public static bool operator ==(ItemStack<T> a, ItemStack<T> b) => a.Equals(b);
    public static bool operator !=(ItemStack<T> a, ItemStack<T> b) => !(a == b);
    public static ItemStack<T> operator +(ItemStack<T> a, ItemStack<T> b) {
        if (a.Item != b.Item) throw new Exception("Unsupported Operation");
        return new ItemStack<T>(a.Item, a.Quantity + b.Quantity);
    }
}