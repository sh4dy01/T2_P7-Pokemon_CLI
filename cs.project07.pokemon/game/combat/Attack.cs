using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public class Attack
    {
        private readonly string _name;
        private int _damage;
        private readonly Type _type;

        public string Name => _name;
        public int Damage { get => _damage; set => _damage = value; }

        public Attack(string name, int damage, Type type)
        {
            _name = name;
            _damage = damage;
            _type = type;
        }
    }
}