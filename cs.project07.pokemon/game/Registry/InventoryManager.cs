using cs.project07.pokemon.game.items;

namespace cs.project07.pokemon.game.Registry
{
    internal static class InventoryManager
    {
        private static List<Item> _inventory = new();

        public static void AddItem(Item item)
        {
            if (_inventory.Contains(item))
            {
                _inventory.Find(x => x.getName() == item.getName()).Add(1);
            }
            else
            {
                _inventory.Add(item);
            }
        }

        public static void RemoveItem(Item item)
        {
            if (_inventory.Contains(item))
            {
                _inventory.Find(x => x.getName() == item.getName()).Remove(1);
            }
        }
    }
}
