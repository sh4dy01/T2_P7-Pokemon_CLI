using cs.project07.pokemon;

namespace UnitTest
{
    internal class FakeAttack : Attack
    {
        public FakeAttack(string name, int damage, ElementType type, int maxUsage = 20) : base(name, damage, type, maxUsage)
        {
        }
    }
}
