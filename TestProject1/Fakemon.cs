using cs.project07.pokemon;
using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class Fakemon : Pokemon
    {
        ElementType _et;
        int atk;
        int def;
        int atkS;
        int defS;
        public override ElementType Element => _et;

        public override float Attack => atk;


        public Fakemon(ElementType et, (int, int, int, int) stat) : base()
        {
            (atk, def, atkS, defS) = stat;
            _et = et;
        }

    }
}
