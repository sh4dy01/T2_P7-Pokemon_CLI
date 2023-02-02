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
        protected string? _name;
        public string Name => _name;

        protected char _id;
        public char ID => _id;
        protected int _quantity { get; set; }

        public Item() { }
        public virtual void Use() {}
        public virtual void Use(Pokemon pokemon) {}
        public virtual void Use(Player pokemon) {}
        protected virtual void Init() {}

        public void Add ( ) { _quantity += 1; }
        public void Remove(int quantity) { _quantity -= quantity; }
        public int GetQuantity() { return _quantity; }
        public virtual void SetQuantity( int quantity ) { _quantity = quantity; }
    }
}
