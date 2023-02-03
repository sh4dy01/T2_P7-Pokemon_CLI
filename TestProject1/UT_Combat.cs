using cs.project07.pokemon;
using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using NUnit.Framework;
using System;
using cs.project07.pokemon.game.items;
using cs.project07.pokemon.game.Registry;

namespace UnitTest
{
    public class PokemonTests
    {
        [Test]
        [TestCase(20, ElementType.NORMAL, ElementType.NORMAL, 20)]
        [TestCase(20, ElementType.FIRE, ElementType.FIRE, 10)]
        [TestCase(100, ElementType.FIRE, ElementType.WATER, 50)]
        [TestCase(50, ElementType.ELECTRIC, ElementType.WATER, 100)]
        [TestCase(100, ElementType.ELECTRIC, ElementType.GRASS, 50)]
        [TestCase(200, ElementType.GRASS, ElementType.GROUND, 400)]
        public void CheckIfCorrectDamageMultiplier(float damage, ElementType attack, ElementType defense, float expected)
        {
            TypeChart.Init();

            damage *= TypeChart.GetDamageMultiplier(attack, defense);
            Assert.That(damage, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(ElementType.DARK, ElementType.BUG, false)]
        [TestCase(ElementType.FIRE, ElementType.FIRE, true)]
        [TestCase(ElementType.NORMAL, ElementType.STEEL, false)]
        public void CheckIfCorrectSTAB(ElementType attackType, ElementType pokemonType, bool expected)
        {
            float stab = DamageCalculator.GetSTAB(attackType, pokemonType);
            Assert.That(stab > 1, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(ElementType.NORMAL, true)]
        [TestCase(ElementType.FIRE, false)]
        [TestCase(ElementType.BUG, true)]
        [TestCase(ElementType.STEEL, true)]
        public void CheckIfPhysicalOrSpecial(ElementType el, bool expected)
        {
            FakeAttack attack = new("test", 0, el);
            Assert.That(attack.IsPhysicalMove(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(ElementType.NORMAL)]
        [TestCase(ElementType.FIRE)]
        [TestCase(ElementType.FIGHTING)]
        [TestCase(ElementType.WATER)]
        public void CheckIfCorrectAD(ElementType elem)
        {
            Fakemon attacker = new(ElementType.NORMAL, (0, 100, 20, 350, 400, 0));
            Fakemon defender = new(ElementType.NORMAL, (0, 10, 22, 75, 852, 0));

            FakeAttack atq = new("test", 0, elem);

            DamageCalculator.GetAttAndDefStat(atq, attacker, defender, out float a, out float b);

            if (atq.IsPhysicalMove())
            {
                Assert.Multiple(() =>
                {
                    Assert.That(a, Is.EqualTo(attacker.Stat.Attack));
                    Assert.That(b, Is.EqualTo(defender.Stat.Defense));
                });
            } else
            {
                Assert.Multiple(() =>
                {
                    Assert.That(a, Is.EqualTo(attacker.Stat.SPAttack));
                    Assert.That(b, Is.EqualTo(defender.Stat.SPDefense));
                });
            }
        }

        [Test]
        public void IsAttackLandsACriticalHit()
        {
            int count = 10;
            Random rnd = new(057811);

            for (int i = 0; i < count; i++) rnd.Next();

            float[] attackerSpeed = { -10, 0, 15, 30, 45, 75, 95, 120, 140, 245, 255, 260, 210, 650, 714, 10000 };
            bool[] expected = { false, false, false, false, true, false, false, false, false, true, true, true, true, true, true, true };

            for (int i = 0; i < attackerSpeed.Length; i++)
            {
                bool isCritical = DamageCalculator.IsCritical(attackerSpeed[i], rnd, rnd.Next(0, 256)) > 1;
                Assert.That(isCritical, Is.EqualTo(expected[i]));
            }
        }

        //You need to calculate the damage with a calculator
        [Test]
        [TestCase(100, 2, 5, ElementType.DARK, ElementType.FIRE, ElementType.GRASS, 27)]
        [TestCase(45, 2, 2, ElementType.FIRE, ElementType.FIRE, ElementType.WATER, 7)]
        public void CalculateTheFinalDamage(
            int attPower, int critical, int pkmLevel, 
            ElementType atckEl, ElementType attElm, ElementType defElm,
            int expected)

        {
            TypeChart.Init();

            FakeAttack attack = new("", attPower, atckEl);
            Fakemon attacker = new(attElm, (100, 52, 15, 35, 40, 10));
            Fakemon defender = new(defElm, (100, 11, 16, 44, 17, 10));

            float damageMultiplier = TypeChart.GetDamageMultiplier(attack.Element, defender.Element);
            CheckIfCorrectDamageMultiplier(attPower, attack.Element, defender.Element, attPower * damageMultiplier);

            float STAB = DamageCalculator.GetSTAB(attack.Element, attacker.Element);
            CheckIfCorrectSTAB(attack.Element, attacker.Element, attack.Element == attacker.Element);

            DamageCalculator.GetAttAndDefStat(attack, attacker, defender, out float a, out float d);
            CheckIfCorrectAD(attack.Element);

            double damage = ((((2 * pkmLevel * critical) / 5.0f + 2) * attack.Power * Math.Round(a / d, 2)) / 50.0f + 2) * STAB * damageMultiplier;

            Assert.That((int)Math.Round(damage), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(100, 35, 65)]
        [TestCase(100, -20, 100)]
        [TestCase(0, 35, 0)]
        [TestCase(-10, 35, 0)]
        public void CheckIfPokemonHasCorrectLifeAfterAttack(int hp, int damage, int expected)
        {
            Fakemon pokemon = new(ElementType.NORMAL, (100, 0, 0, 0, 0, 0));
            pokemon.InitHealth(hp);

            pokemon.TakeDamage(damage);
            Assert.That(pokemon.Currenthealth, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(200, 200, 20, 200)]
        [TestCase(50, 99, 20, 70)]
        [TestCase(75, 80, 20, 80)]
        [TestCase(75, 80, -10, 75)]
        [TestCase(-10, 80, 20, 0)]
        public void CheckIfPokemonHealCorrectly(int hp, int maxHP, int heal, int expected)
        {
            Fakemon pokemon = new(ElementType.NORMAL, (maxHP, 0, 0, 0, 0, 0));
            pokemon.InitHealth(hp);

            pokemon.Heal(heal);
            Assert.That(pokemon.Currenthealth, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(-10, 1, 0, 25)]
        [TestCase(25, 2, 0, 50)]
        [TestCase(50, 2, 25, 50)]
        [TestCase(50, 2, 25, 50)]
        [TestCase(1000, 9, 100, 225)]
        public void CheckIfPokemonHasCorrectLevel(int experience, int levelExpected, int expLeft, int expNeeded)
        {
            Fakemon pokemon = new(ElementType.NORMAL, (10, 0, 0, 0, 0, 0));

            pokemon.GainExperience(experience);

            Assert.That(pokemon.Level, Is.EqualTo(levelExpected));
            Assert.That(pokemon.Experience, Is.EqualTo(expLeft));
            Assert.That(pokemon.RequiredExp, Is.EqualTo(expNeeded));
        }
        
        //Level up stat multiplier is 0.07f
        [Test]
        [TestCase(1, 100, 100)]
        [TestCase(5, 20, 26)]
        [TestCase(100, 100, 793)]
        public void CheckIfPokemonStatLevelUpCorrectly(int level, int baseStat, int statLvlExpected)
        {
            Fakemon pokemon = new(ElementType.NORMAL, (baseStat, baseStat, baseStat, baseStat, baseStat, baseStat));

            pokemon.SetLevel(level);

            Assert.That(pokemon.Stat.Attack, Is.EqualTo(statLvlExpected));
            Assert.That(pokemon.Stat.Defense, Is.EqualTo(statLvlExpected));
            Assert.That(pokemon.Stat.SPAttack, Is.EqualTo(statLvlExpected));
            Assert.That(pokemon.Stat.SPDefense, Is.EqualTo(statLvlExpected));
            Assert.That(pokemon.Stat.Speed, Is.EqualTo(statLvlExpected));
        }
    }
}