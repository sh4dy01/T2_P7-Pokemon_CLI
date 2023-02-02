using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    public class Stat
    {
        public const float LEVEL_UP_STEP = 0.07f;
        
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

        //UT Test
        public void LevelUpStat(Stat stat)
        {
            _maxHP += (float)Math.Round(stat.MaxHP * LEVEL_UP_STEP, 1); 
            _attack += (float)Math.Round(stat.Attack * LEVEL_UP_STEP, 1);
            _defense += (float)Math.Round(stat.Defense * LEVEL_UP_STEP, 1);
            _spAttack += (float)Math.Round(stat.SPAttack * LEVEL_UP_STEP, 1);
            _spDefense += (float)Math.Round(stat.SPDefense * LEVEL_UP_STEP, 1);
            _speed += (float)Math.Round(stat.Speed * LEVEL_UP_STEP, 1);
        }

        public void RoundToInt()
        {
            _maxHP = (float)Math.Round(_maxHP, 0);
            _attack = (float)Math.Round(_attack, 0);
            _defense = (float)Math.Round(_defense, 0);
            _spAttack = (float)Math.Round(_spAttack, 0);
            _spDefense = (float)Math.Round(_spDefense, 0);
            _speed = (float)Math.Round(_speed, 0);
        }
    }
}
