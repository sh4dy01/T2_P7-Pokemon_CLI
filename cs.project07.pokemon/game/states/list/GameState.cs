using cs.project07.pokemon.game.map;

namespace cs.project07.pokemon.game.states.list
{
    public class GameState : State
    {
        public Map Map;

        public GameState(Game game) : base(game)
        {
            Init();
        }

        protected override void Init()
        {
            Name = "Pokemon";
            InitMap();
        }

        private void InitMap()
        {
            Map = new Map(this);
            Map.ParseFileToLayers("game/map/list/Map2.txt");
        }
        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {
                case ConsoleKey.Insert:
                    // TODO Remove when Pause menu is complete
                    // Back to previous menu
                    Game.StatesList?.Pop();
                    break;
                case ConsoleKey.Escape:
                    // TODO Pause menu
                    break;
                case ConsoleKey.LeftArrow:
                    // TODO Player move left
                    break;
                case ConsoleKey.UpArrow:
                    // TODO Player move up
                    break;
                case ConsoleKey.RightArrow:
                    // TODO Player move right
                    break;
                case ConsoleKey.DownArrow:
                    // TODO Player move down
                    break;
                case ConsoleKey.Enter:
                    // TODO Player use action
                    break;
                case ConsoleKey.PageUp:
                    Map.Zoom ++;
                    break;
                case ConsoleKey.PageDown:
                    Map.Zoom --;
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            // Update childs
            // ------ Map
            Map?.Update();
        }

        public override void Render()
        {
            base.Render();

            // Render childs
            // ------ Map
            Map?.Render();
            
        }
    }
}
