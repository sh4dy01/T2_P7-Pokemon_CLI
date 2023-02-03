using cs.project07.pokemon.game.entites;

namespace cs.project07.pokemon.game.items.list
{
    public class Spray : Item
    {
        public Spray()
        {
            _quantity = 0;
            Init();
        }

        public Spray(int quantity)
        {
            _quantity = quantity;
            Init();
        }

        protected override void Init()
        {
            _name = "Spray";
            _id = 'S';
        }

        public override void Use()
        {
            if (_quantity <= 0) return ;
            _quantity--;

            Player.SetSprayMovementLeft(50);
        }
    }
    
}
