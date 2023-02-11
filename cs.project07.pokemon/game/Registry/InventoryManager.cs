using cs.project07.pokemon.game.items;
using cs.project07.pokemon.game.items.list;

namespace cs.project07.pokemon.game.Registry
{
    public static class InventoryManager
    {
        private static readonly List<Item> _inventory = new()
        {
            new Pokeball(0, 10),
            new Pokeball(1, 5),
            new Pokeball(2, 3),
            new Pokeball(3),
            new Potion(0, 10),
            new Potion(1, 5),
            new Potion(2, 3),
            new Potion(3, 1),
            new Spray(5),
        };

        public static List<Item> Inventory => _inventory;
        private static readonly char[] _itemsPossibilities = { 'p', 'P', 'h', 'H', 'b', 'B', 'c', 'C', 'S' };
        public static char[] ItemsPossibilities => _itemsPossibilities;

    }
}
