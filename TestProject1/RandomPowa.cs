using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class RandomPowa : Random
    {
        private readonly int[] valueToGive;
        int _idx;

        public RandomPowa(int[] valueToGive) : base()
        {
            this.valueToGive = valueToGive;
            _idx = 0;
        }

        public new int Next(int min, int max) => valueToGive[_idx++];


    }
}
