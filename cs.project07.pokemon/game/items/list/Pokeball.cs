
namespace cs.project07.pokemon.game.items.list
{
    public class Pokeball : Item
    {
        private readonly int _pokeballLevel;
        public Pokeball(int pokeballLevel)
        {
            _pokeballLevel = pokeballLevel;
            _quantity = 0;

            Init();
        }

        public Pokeball(int pokeballLevel, int quantity)
        {
            _pokeballLevel = pokeballLevel;
            _quantity = quantity;

            Init();
        }

        protected override void Init()
        {
            switch (_pokeballLevel)
            {
                case 0:
                    _id = 'b';
                    _name = "Poke Ball";
                    break;

                case 1:
                    _id = 'B';
                    _name = "Super Ball";
                    break;

                case 2:
                    _id = 'c';
                    _name = "Hyper Ball";
                    break;

                case 3:
                    _id = 'C';
                    _name = "Master Ball";
                    break;
            }
        }
        public float GetMultiplicator()
        {
            switch (_pokeballLevel)
            {
                case 0:
                    return 1;

                case 1:
                    return 1.5f;

                case 2:
                    return 2;

                case 3:
                    return 255;
            }
            return 1;
        }

        public override void Use()
        {
            if (_quantity <= 0) return ;
            _quantity--;
        }
    }
}
