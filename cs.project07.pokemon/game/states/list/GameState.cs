using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.entites;
using System.Runtime.CompilerServices;

namespace cs.project07.pokemon.game.states.list
{
    public class GameState : State
    {
        public Map CurrentMap;
        private Dictionary<string, Map> Maps;

        public Player Player;
        public GameState(Game game) : base(game)
        {
            Init();
        }

        protected override void Init()
        {
            Name = "Pokemon";
            InitMap();
            InitPlayer();
        }

        private void InitMap()
        {
            Maps = new Dictionary<string, Map>
            {
                { "map1", new Map(this,"map1") },
                { "map2", new Map(this,"map2") }
            };

            Maps["map1"].ParseFileToLayers("../../../game/map/list/Map1.txt");
            Maps["map2"].ParseFileToLayers("../../../game/map/list/Map2.txt");

            CurrentMap = Maps["map1"];
        }

        private void InitPlayer()
        {
            Player = new Player(Map.PlayerSpawnPosition);
            Map.playerDraw = Player.playerPosition;
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
                    Player.mouvPlayer('O');
                    Map.playerDraw = Player.playerPosition;
                    break;
                case ConsoleKey.UpArrow:
                    // TODO Player move up
                    Player.mouvPlayer('N');
                    Map.playerDraw = Player.playerPosition;
                    break;
                case ConsoleKey.RightArrow:
                    // TODO Player move right
                    Player.mouvPlayer('E');
                    Map.playerDraw = Player.playerPosition;
                    break;
                case ConsoleKey.DownArrow:
                    // TODO Player move down
                    Player.mouvPlayer('S');
                    Map.playerDraw = Player.playerPosition;
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

        public void ChangeMap (string mapName, int posX, int posY)
        {
            
        }
    }
}
