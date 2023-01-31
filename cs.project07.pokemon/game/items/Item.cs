using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.items
{
    public class Item
    {
        static protected char _id { get; set; }
        protected int _quantity { get; set; }

        public Item() { }   
        virtual public void Use(Pokemon pokemon) {}
        public void Add ( int quantity ) { _quantity += quantity; }
        public void Remove(int quantity) { _quantity -= quantity; }
        public int GetQuantity() { return _quantity; }
        public virtual void SetQuantity( int quantity ) { _quantity = quantity; }

    }
}
