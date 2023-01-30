using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public static class AttackRegistry
    {
        private const int TYPE_PP = 10;
        private const int SPECIAL_PP = 5;

        private static Attack[] _attacks =
        {
            new Attack("Tackle", 10, Type.NORMAL),
            new Attack("Scratch", 10, Type.NORMAL),
            new Attack("Growl", 10, Type.NORMAL),
            new Attack("Quick attack", 10, Type.NORMAL),
            new Attack("Splash", 0, Type.NORMAL),

            new Attack("Vine whip", 20, Type.GRASS, TYPE_PP),
            new Attack("Ember", 20, Type.FIRE, TYPE_PP),
            new Attack("Water gun", 20, Type.WATER, TYPE_PP),
            new Attack("Thunder shock", 20, Type.ELECTRIC, TYPE_PP),
            new Attack("Rollout", 20, Type.GROUND, TYPE_PP),
            new Attack("Confusion", 30, Type.PSYCHIC, TYPE_PP),
            new Attack("Judgment", 100, Type.NORMAL, SPECIAL_PP),
            new Attack("Hyper beam", 100, Type.NORMAL, SPECIAL_PP),
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