namespace cs.project07.pokemon
{
    public static class AttackRegistry
    {
        private const int SPECIAL_PP = 5;

        private static readonly Attack[] _attacks =
        {
            new("Tackle", 35, ElementType.NORMAL, 35),
            new("Scratch", 40, ElementType.NORMAL, 35),
            new("Growl", 10, ElementType.NORMAL, 40),
            new("Quick attack", 40, ElementType.NORMAL, 30),
            new("Splash", 0, ElementType.NORMAL, 40),

            new("Vine whip", 35, ElementType.GRASS, 10),
            new("Ember", 40, ElementType.FIRE, 25),
            new("Water gun", 40, ElementType.WATER, 25),
            new("Thunder shock", 40, ElementType.ELECTRIC, 30),
            new("Rollout", 30, ElementType.GROUND, 20),
            new("Confusion", 50, ElementType.PSYCHIC, 25),
            new("Judgment", 100, ElementType.NORMAL, SPECIAL_PP),
            new("Hyper beam", 150, ElementType.NORMAL, SPECIAL_PP),
        };

        public static Attack TACKLE = _attacks[0];
        public static Attack SCRATCH = _attacks[1];
        public static Attack GROWL = _attacks[2];
        public static Attack QUICK_ATTACK = _attacks[3];
        public static Attack SPLASH = _attacks[4];

        public static Attack VINE_WHIP = _attacks[5];
        public static Attack EMBER = _attacks[6];
        public static Attack WATER_GUN = _attacks[7];
        public static Attack THUNDER_SHOCK = _attacks[8];
        public static Attack ROLLOUT = _attacks[9];
        public static Attack CONFUSION = _attacks[10];
        public static Attack JUDGMENT = _attacks[11];
        public static Attack HYPER_BEAM = _attacks[12];
    }
}