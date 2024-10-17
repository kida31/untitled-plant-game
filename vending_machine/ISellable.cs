namespace InventoryV0
{
    public interface ISellable : IStorable
    {
        int Price { get; }
    }
}