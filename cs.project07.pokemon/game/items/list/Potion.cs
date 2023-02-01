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
        private int _potionLevel;
        public Potion(int potionLevel) 
        {
            if (potionLevel < 0 || potionLevel > 3) throw new ArgumentException ("potion level is only settable between 0 and 3");
            _quantity = 0;
            _potionLevel = potionLevel;

            switch (_potionLevel)
            {
                case 0:
                    _id = 'p';
                    _name = "Potion";
                    break;

                case 1:
                    _id = 'P';
                    _name = "Super Potion";
                    break;

                case 2:
                    _id = 'h';
                    _name = "Hyper Potion";
                    break;

                case 3:
                    _id = 'H';
                    _name = "Potion Max";
                    break;
            }
        }
        public Potion(int quantity, int potionLevel) 
        {
            if (potionLevel < 0 || potionLevel > 3) throw new ArgumentException("potion level is only settable between 0 and 3");
            _quantity = quantity;
            _potionLevel = potionLevel;

            switch (_potionLevel)
            {
                case 0:
                    _id = 'p';
                    _name = "Potion";
                    break;

                case 1:
                    _id = 'P';
                    _name = "Super Potion";
                    break;

                case 2:
                    _id = 'h';
                    _name = "Hyper Potion"; 
                    break;

                case 3:
                    _id = 'H';
                    _name = "Potion Max";
                    break;
            }
        }

        override public void Use(Pokemon pokemon) 
        {
            if (_quantity <= 0) throw new ArgumentException("not enougth" + _name );
            _quantity--;

            switch (_potionLevel)
            {
                case 0:
                    pokemon.Heal(20);
                    break;

                case 1:
                    pokemon.Heal(50);
                    break;

                case 2:
                    pokemon.Heal(120);
                    break;

                case 3:
                    pokemon.HealMax();
                    break;
            }
        }
    }
}
