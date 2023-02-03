using cs.project07.pokemon;
using cs.project07.pokemon.game.combat;
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

        public override ElementType Element => _et;

        public Fakemon(ElementType et, (float, float, float, float, float, float) stat) : base()
        {
            _dex = new PokedexEntry(0, "Fakemon", et, stat, Array.Empty<Attack>(), 0);
            _stat = new(stat);
            _et = et;
            _level = 1;
            _requiredExperience = LEVEL_UP_STEP;
        }

        public void SetLevel(int level)
        {
            _level = level;
            InitStat();
        }
        
        public void InitHealth(int amount)
        {
            if (amount < 0) amount = 0;

            _currentHealth = amount;
        }
    }
}
