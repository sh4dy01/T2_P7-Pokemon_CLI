using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    public class DamageCalculator
    {
        public static float DamageWithMultiplier(Attack attack, Pokemon attacker, Pokemon defender, out float damageMultiplier, out int critical)
        {
            Random rnd = new();

            damageMultiplier = TypeChart.GetDamageMultiplier(attack.Type, defender.Type);

            GetAttAndDefStat(attack, attacker, defender, out float a, out float d);

            float STAB = GetSTAB(attack, attacker);
            critical = IsCritical(attacker.Speed, null, -1);
            float random = rnd.Next(217, 256) / 255.0f;

            float damage = ((2 * attacker.Level * critical / 5 + 2) * attack.Power * a / d / 50 + 2) * STAB * damageMultiplier * random;

            return damage;
        }

        public static float GetSTAB(Attack attack, Pokemon attacker)
        {
            float STAB = 1;
            if (attack.Type == attacker.Type)
                STAB = 1.5f;

            return STAB;
        }

        public static void GetAttAndDefStat(Attack attack, Pokemon attacker, Pokemon defender, out float a, out float d)
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

        public static int IsCritical(float attackerSpeed, Random? isUT, int chanceUT)
        {
            Random rnd = isUT ?? new Random();

            int chance = isUT != null ? chanceUT : rnd.Next(0, 256);
            int critical = 1;

            int threshold = (int)attackerSpeed / 2;
            if (threshold > 255) threshold = 255;
            else if (threshold < 0) threshold = 0;

            if (chance <= threshold)
                critical = 2;

            return critical;
        }
    }
}
