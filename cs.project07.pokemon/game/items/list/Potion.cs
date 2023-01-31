using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.items.list
{
    internal class Potion : Item
    {
        public Potion() 
        {
            _quantity = 0;
            _id = 'P';
        }
        public Potion(int quantity) 
        {
            _quantity = quantity;
            _id = 'P';
        }

        override public void Use(Pokemon pokemon) 
        {
            pokemon.Heal(40);
        }
    }
}
