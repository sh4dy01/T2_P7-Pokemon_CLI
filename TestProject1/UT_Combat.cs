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
        [TestCase(200, Type.GRASS, Type.GROUND, 400)]
        public void CalculateTheMultipliedDamageAmount(float damage, Type attack, Type defense, int expected)
        {
            TypeChart.Init();

            damage *= TypeChart.GetDamageMultiplier(attack, defense);
            Assert.That(damage, Is.EqualTo(expected));
        }

        [Test]
        public void CheckIfPokemonHasCorrectLife()
        {
            Pokemon pokemon = new(PokemonRegistry.GetRandomPokemon());
            Assert.That(pokemon.Currenthealth, Is.EqualTo(pokemon.MaxHealth));

            pokemon.TakeDamage(10);
            Assert.That(pokemon.Currenthealth, Is.EqualTo(pokemon.MaxHealth - 10));

            pokemon.TakeDamage(1000);
            Assert.That(pokemon.Currenthealth, Is.EqualTo(0));

            pokemon.Heal(1000);
            Assert.That(pokemon.Currenthealth, Is.EqualTo(pokemon.MaxHealth));
        }
    }
}