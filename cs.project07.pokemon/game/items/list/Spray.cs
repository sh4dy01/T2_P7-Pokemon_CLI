using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.items.list
{
    public class Spray : Item
    {
        public Spray()
        {
            _quantity = 0;
            _name = "Spray";
        }

        public Spray(int quantity)
        {
            _quantity = quantity;
            _name = "Spray";

        }

        override public void Use(Player player)
        {
            if (_quantity <= 0) throw new ArgumentException("not enougth " + _name);
            _quantity--;

            player.SetSprayMovementLeft(50);
        }
    }
    
}
