using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.items.list
{
    internal class Pokeball : Item
    {
        public Pokeball()
        {
            _quantity = 0;
            _id = 'B';
        }
        public Pokeball(int quantity)
        {
            _quantity = quantity;
            _id = 'B';
        }

        override public void Use(Pokemon pokemon)
        {
            
        }
    }
}
