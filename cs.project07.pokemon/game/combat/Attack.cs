using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public class Attack
    {
        private readonly string _name;
        private int _power;
        private readonly Type _type;
        private int _maxUsage;
        private int _currentUsage;


        public string Name => _name;
        public int Power { get => _power; set => _power = value; }
        public Type Type { get => _type; }
        public int Usage { get => _currentUsage; }

        public void Use()
        {
            _currentUsage--;
        }

        public Attack(string name, int damage, Type type, int maxUsage = 20)
        {
            _name = name;
            _power = damage;
            _type = type;
            _maxUsage = maxUsage;
            _currentUsage = _maxUsage;
        }
    }
}