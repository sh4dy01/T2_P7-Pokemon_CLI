using cs.project07.pokemon;
using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using NUnit.Framework;

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
        public void CalculateTheMultipliedDamageAmount(float damage, ElementType attack, ElementType defense, int expected)
        {
            TypeChart.Init();

            damage *= TypeChart.GetDamageMultiplier(attack, defense);
            Assert.That(damage, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(100, 35, 65)]
        [TestCase(100, -20, 100)]
        [TestCase(0, 35, 0)]
        [TestCase(-10, 35, 0)]
        public void CheckIfPokemonHasCorrectLifeAfterAttack(int hp, int damage, int expected)
        {
            Pokemon pokemon = new(PokemonRegistry.GetRandomPokemon()); 
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
            Pokemon pokemon = new(PokemonRegistry.GetRandomPokemon());
            pokemon.InitHealth(hp);

            pokemon.Heal(heal);
            Assert.That(pokemon.Currenthealth, Is.EqualTo(expected));
        }

        [Test]
        public void CheckIfPokemonHasCorrectLevel()
        {
            Pokemon pokemon = new(PokemonRegistry.GetRandomPokemon());
            Assert.That(pokemon.Level, Is.EqualTo(1));

            pokemon.GainExperience(20);
            Assert.That(pokemon.Level, Is.EqualTo(2));

            pokemon = new(PokemonRegistry.GetRandomPokemon());
            pokemon.GainExperience(1000);
            Assert.That(pokemon.Level, Is.EqualTo(9));
            Assert.That(pokemon.Experience, Is.EqualTo(120));
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
        public void CheckIfPhysicalOrSpecial()
        {
            Attack attack = AttackRegistry.ROLLOUT;
            Assert.That(attack.IsPhysicalMove(), Is.EqualTo(true));

            attack = AttackRegistry.EMBER;
            Assert.That(attack.IsPhysicalMove(), Is.EqualTo(false));
        }

        [Test]
        public void CheckIfCorrectAD()
        {
            Pokemon attacker = new(PokemonRegistry.GetRandomPokemon());
            Pokemon defender = new(PokemonRegistry.GetRandomPokemon());

            Attack attack = AttackRegistry.VINE_WHIP;

            DamageCalculator.GetAttAndDefStat(attack, attacker, defender, out float a, out float b);
            Assert.Multiple(() =>
            {
                Assert.That(a, Is.EqualTo(attacker.SPAttack));
                Assert.That(b, Is.EqualTo(defender.SPDefense));
            });

            attack = AttackRegistry.ROLLOUT;

            DamageCalculator.GetAttAndDefStat(attack, attacker, defender, out a, out b);

            Assert.Multiple(() =>
            {
                Assert.That(a, Is.EqualTo(attacker.Attack));
                Assert.That(b, Is.EqualTo(defender.Defense));
            });
        }

        /*void Mock()
        {
            var a = new Attack("", 10, ElementType.NORMAL);
            var pkmn = new Fakemon( ElementType.ELECTRIC, (10,10,23,23));
            var pkmn2 = new Fakemon( ElementType.ELECTRIC, (10,10,23,23));

            DamageCalculator.GetAttAndDefStat(attack, attacker, defender, out a, out b);

        }*/

        [Test]
        public void IsAttackLandsACriticalHit()
        {
            int count = 10;
            Random rnd = new(057811);

            for (int i = 0; i < count; i++) rnd.Next();

            float[] attackerSpeed = { -10, 0, 15, 30, 45, 75, 95, 120, 140, 245, 255, 260, 210, 650, 714, 10000 };
            bool[] expected = { false, false, false, false, true, false, false, false, true, true, true, true, false, true, true, true };

            for (int i = 0; i < attackerSpeed.Length; i++)
            {
                bool isCritical = DamageCalculator.IsCritical(attackerSpeed[i], rnd, rnd.Next(0, 256)) > 1;
                Assert.That(isCritical, Is.EqualTo(expected[i]));
            }
        }
    }
}