using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    internal class DamageCalculator
    {
        public float DamageWithMultiplier(Attack attack, Pokemon attacker, Pokemon defender, out float damageMultiplier, out int critical)
        {
            Random rnd = new();

            damageMultiplier = TypeChart.GetDamageMultiplier(attack.Type, defender.Type);

            GetAttAndDefStat(attack, attacker, defender, out float a, out float d);

            float STAB = GetSTAB(attack, attacker);
            critical = IsCritical(attacker);
            float random = rnd.Next(217, 256) / 255.0f;

            float damage = ((2 * attacker.Level * critical / 5 + 2) * attack.Power * a / d / 50 + 2) * STAB * damageMultiplier * random;

            return damage;
        }

        private float GetSTAB(Attack attack, Pokemon attacker)
        {
            float STAB = 1;
            if (attack.Type == attacker.Type)
                STAB = 1.5f;

            return STAB;
        }

        private void GetAttAndDefStat(Attack attack, Pokemon attacker, Pokemon defender, out float a, out float d)
        {
            if (attack.IsPhysicalMove())
            {
                a = attacker.Attack;
                d = defender.Defense;
            }
            else
            {
                a = attacker.SPAttack;
                d = defender.SPDefense;
            }
        }

        private int IsCritical(Pokemon attacker)
        {
            Random rnd = new();

            int critical = 1;

            int chance = rnd.Next(256);
            float threshold = attacker.Speed / 2;

            if (chance <= threshold)
                critical = 2;

            return critical;
        }
    }
}
