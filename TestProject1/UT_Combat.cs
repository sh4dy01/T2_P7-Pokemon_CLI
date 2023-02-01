using cs.project07.pokemon;
using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using NUnit.Framework;
using Type = cs.project07.pokemon.Type;

namespace UnitTest
{
    public class PokemonTests
    {
        [Test]
        [TestCase(20, Type.NORMAL, Type.NORMAL, 20)]
        [TestCase(20, Type.FIRE, Type.FIRE, 10)]
        [TestCase(100, Type.FIRE, Type.WATER, 50)]
        [TestCase(50, Type.ELECTRIC, Type.WATER, 100)]
        [TestCase(100, Type.ELECTRIC, Type.GRASS, 50)]
        [TestCase(200, Type.GRASS, Type.GROUND, 400)]
        public void CalculateTheMultipliedDamageAmount(float damage, Type attack, Type defense, int expected)
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
        public void IsAttackLandsACriticalHit()
        {
            Random rnd = new(057811);
            
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