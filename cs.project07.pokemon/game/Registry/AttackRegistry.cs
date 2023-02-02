

namespace cs.project07.pokemon
{
    public static class AttackRegistry
    {
        private const int TYPE_PP = 10;
        private const int SPECIAL_PP = 5;

        private static Attack[] _attacks =
        {
            new Attack("Tackle", 35, ElementType.NORMAL, 35),
            new Attack("Scratch", 40, ElementType.NORMAL, 35),
            new Attack("Growl", 10, ElementType.NORMAL, 40),
            new Attack("Quick attack", 40, ElementType.NORMAL, 30),
            new Attack("Splash", 0, ElementType.NORMAL, 40),

            new Attack("Vine whip", 35, ElementType.GRASS, 10),
            new Attack("Ember", 40, ElementType.FIRE, 25),
            new Attack("Water gun", 40, ElementType.WATER, 25),
            new Attack("Thunder shock", 40, ElementType.ELECTRIC, 30),
            new Attack("Rollout", 30, ElementType.GROUND, 20),
            new Attack("Confusion", 50, ElementType.PSYCHIC, 25),
            new Attack("Judgment", 100, ElementType.NORMAL, SPECIAL_PP),
            new Attack("Hyper beam", 150, ElementType.NORMAL, SPECIAL_PP),
        };

        static public Attack TACKLE = _attacks[0];
        static public Attack SCRATCH = _attacks[1];
        static public Attack GROWL = _attacks[2];
        static public Attack QUICK_ATTACK = _attacks[3];
        static public Attack SPLASH = _attacks[4];

        static public Attack VINE_WHIP = _attacks[5];
        static public Attack EMBER = _attacks[6];
        static public Attack WATER_GUN = _attacks[7];
        static public Attack THUNDER_SHOCK = _attacks[8];
        static public Attack ROLLOUT = _attacks[9];
        static public Attack CONFUSION = _attacks[10];
        static public Attack JUDGMENT = _attacks[11];
        static public Attack HYPER_BEAM = _attacks[12];
    }
}