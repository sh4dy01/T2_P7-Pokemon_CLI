namespace cs.project07.pokemon.game.states.list
{
    public class InventoryState : State
    {
        public Game game;
        public InventoryState(Game gameReceive) : base(gameReceive) 
        { 
            game = gameReceive;
            Init();
        }

        protected override void Init()
        {
            Name = "Inventory";
            InitInventory();
        }

        private void InitInventory()
        {

        }
    }
}
