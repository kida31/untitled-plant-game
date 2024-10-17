using InventoryV0;

namespace untitledplantgame.vending_machine;

public class ItemImpl: ISellable
{
    public int Price { get; }
    public string Name { get;}
    
    public ItemImpl(string name, int price)
    {
        Name = name;
        Price = price;
    }
}