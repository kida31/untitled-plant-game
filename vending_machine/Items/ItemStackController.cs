using System.Diagnostics;
using System.Linq;

namespace InventoryV0
{
    public class ItemStackController
    {
        private static ItemStackController _instance;

        public static ItemStackController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemStackController();
                }

                return _instance;
            }
        }

        public void MoveItem<T>(ref ItemStack<T> from, ref ItemStack<T> to) where T : class, IStorable
        {
            if (to.Item != null && from.Item != to.Item)
            {
                throw new System.Exception("Unsupported Operation");
            }

            to.Item ??= from.Item;
            to.Quantity += from.Quantity;

            from.Quantity = 0;
            from.Item = null;
        }
    }
}