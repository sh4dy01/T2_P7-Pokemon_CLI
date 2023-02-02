using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.items.list
{
    public class Pokeball : Item
    {
        private int _pokeballLevel;
        public Pokeball(int pokeballLevel)
        {
            _pokeballLevel = pokeballLevel;
            _quantity = 0;

            switch (_pokeballLevel)
            {
                case 0:
                    _id = 'b';
                    _name = "Poke Ball";
                    break;

                case 1:
                    _id = 'B';
                    _name = "Super Ball";
                    break;

                case 2:
                    _id = 'c';
                    _name = "Hyper Ball";
                    break;

                case 3:
                    _id = 'C';
                    _name = "Master Ball";
                    break;
            }
        }
        public Pokeball(int quantity, int pokeballLevel)
        {
            _pokeballLevel = pokeballLevel;
            _quantity = quantity;

            switch (_pokeballLevel)
            {
                case 0:
                    _id = 'b';
                    _name = "Poke Ball";
                    break;

                case 1:
                    _id = 'B';
                    _name = "Super Ball";
                    break;

                case 2:
                    _id = 'c';
                    _name = "Hyper Ball";
                    break;

                case 3:
                    _id = 'C';
                    _name = "Master Ball";
                    break;
            }
        }
        public float GetMultiplcator()
        {
            switch (_pokeballLevel)
            {
                case 0:
                    return 1;
                    break;

                case 1:
                    return 1.5f;
                    break;

                case 2:
                    return 2;
                    break;

                case 3:
                    return 255;
                    break;
            }
            return 1;
        }

        override public void Use(Pokemon pokemon)
        {
            if (_quantity <= 0) return ;
            _quantity--;

            switch (_pokeballLevel)
            {
                case 0:
                    //to-do  | multuplicator :  1  |
                    break;

                case 1:
                    //to-do  | multuplicator : 1.5 |
                    break;

                case 2:
                    //to-do  | multuplicator :  2  |
                    break;

                case 3:
                    //to-do  | multuplicator : 255 |
                    break;
            }
        }
    }
}
