using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs.project07.pokemon
{
    public static class AttackRegistry
    {
        private static Attack[] _attacks =
        {
            new Attack("Tackle", 10, Type.NORMAL),
            new Attack("Scratch", 10, Type.NORMAL),
            new Attack("Growl", 10, Type.NORMAL),
            new Attack("Quick attack", 10, Type.NORMAL),
            new Attack("Splash", 10, Type.NORMAL),

            new Attack("Vine whip", 10, Type.GRASS),
            new Attack("Ember", 10, Type.FIRE),
            new Attack("Water gun", 10, Type.WATER),
            new Attack("Thunder shock", 10, Type.ELECTRIC),
            new Attack("Rollout", 10, Type.GROUND),
            new Attack("Confusion", 10, Type.PSYCHIC),
            new Attack("Judgment", 10, Type.NORMAL),
            new Attack("Hyper beam", 10, Type.NORMAL),
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