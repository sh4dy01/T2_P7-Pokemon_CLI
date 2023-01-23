using cs.project07.pokemon.game.map;

namespace cs.project07.pokemon.game.states.list
{
    public class CombatState : State
    {
        // Get player pokemon
        // Get enemy pokemon
        public CombatState(Game game) : base(game)
        {
            Init();
        }

        protected override void Init()
        {
            Name = "Combat";
            InitCombat();
        }

        private void InitCombat()
        {

        }
        
        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {

            }
        }

        public override void Update()
        {
            base.Update();

            // Update childs
            // ------ Map
        }

        public override void Render()
        {
            base.Render();

            // Render childs
            // ------ Map
            
        }
    }
}
