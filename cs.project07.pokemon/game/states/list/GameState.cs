using cs.project07.pokemon.game.map;
using System.Runtime.CompilerServices;

namespace cs.project07.pokemon.game.states.list
{
    public class GameState : State
    {
        public Map CurrentMap;
        private Dictionary<string, Map> Maps;

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
            Maps = new Dictionary<string, Map>
            {
                { "map1", new Map(this) },
                { "map2", new Map(this) }
            };

            Maps["map1"].ParseFileToLayers("game/map/list/Map1.txt");
            Maps["map2"].ParseFileToLayers("game/map/list/Map2.txt");

            CurrentMap = Maps["map1"];
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
                    CurrentMap.Zoom ++;
                    break;
                case ConsoleKey.PageDown:
                    CurrentMap.Zoom --;
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            // Update childs
            // ------ Map
            CurrentMap?.Update();
        }

        public override void Render()
        {
            base.Render();

            // Render childs
            // ------ Map
            CurrentMap?.Render();
            
        }
    }
}
