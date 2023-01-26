using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public class Attack
    {
        private string _name;
        private int _damage;
        private Element _element;

        public string Name { get => _name; }

        public Attack(string name, int damage, Element element)
        {
            _name = name;
            _damage = damage;
            _element = element;
        }
    }
}