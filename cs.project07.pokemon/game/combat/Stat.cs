using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    public class Stat
    {
        const float LEVEL_UP_STEP = 0.02f;
        
        private float _maxHP;
        private float _attack;
        private float _defense;
        private float _spAttack;
        private float _spDefense;
        private float _speed;

        public float MaxHP { get => _maxHP; }
        public float Attack { get => _attack; }
        public float Defense { get => _defense; }
        public float SPAttack { get => _spAttack; }
        public float SPDefense { get => _spDefense; }
        public float Speed { get => _speed; }

        public Stat((float, float, float, float, float, float) stat)
        {
            (_maxHP, _attack, _defense, _spAttack, _spDefense, _speed) = stat;
        }

        public void LevelUpStat(Stat stat)
        {
            _maxHP += (int)(stat.MaxHP * LEVEL_UP_STEP); 
            _attack += (int)(stat.Attack * LEVEL_UP_STEP);
            _defense += (int)(stat.Defense * LEVEL_UP_STEP);
            _spAttack += (int)(stat.SPAttack * LEVEL_UP_STEP);
            _spDefense += (int)(stat.SPDefense * LEVEL_UP_STEP);
            _speed += (int)(stat.Speed * LEVEL_UP_STEP);
        }
    }
}
