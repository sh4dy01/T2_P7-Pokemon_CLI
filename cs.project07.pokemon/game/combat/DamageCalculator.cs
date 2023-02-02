using cs.project07.pokemon.game.entites;

namespace cs.project07.pokemon.game.combat
{
    public static class DamageCalculator
    {
        public static int DamageWithMultiplier(Attack attack, Pokemon attacker, Pokemon defender, out float damageMultiplier, out int critical)
        {
            damageMultiplier = 1.0f;
            critical = 1;
            if (attack.Power == 0) { return 0; }
            
            Random rnd = new();
            
            damageMultiplier = TypeChart.GetDamageMultiplier(attack.Element, defender.Element);

            GetAttAndDefStat(attack, attacker, defender, out float a, out float d);

            float STAB = GetSTAB(attack.Element, attacker.Element);
            critical = IsCritical(attacker.Stat.Speed, null, -1);
            float random = rnd.Next(217, 256) / 255.0f;
            
            double damage = ((((2 * attacker.Level * critical) / 5.0f + 2) * attack.Power * (a / d)) / 50.0f + 2) * STAB * damageMultiplier * random;

            return (int)Math.Ceiling(damage);
        }

        public static float GetSTAB(ElementType attackType, ElementType attackerType)
        {
            float STAB = 1;
            if (attackType == attackerType)
                STAB = 1.5f;

            return STAB;
        }

        public static void GetAttAndDefStat(Attack attack, Pokemon attacker, Pokemon defender, out float a, out float d)
        {
            if (attack.IsPhysicalMove())
            {
                a = attacker.Stat.Attack;
                d = defender.Stat.Defense;
            }
            else
            {
                a = attacker.Stat.SPAttack;
                d = defender.Stat.SPDefense;
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
