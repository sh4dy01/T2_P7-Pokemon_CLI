using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.entites;

namespace cs.project07.pokemon.game.states.list
{
    public class GameState : State
    {
        public Map Map;
        public Player Player;
        private CombatState Combat;
        private Game game;
        public GameState(Game gameReceive) : base(gameReceive)
        {
            game = gameReceive;
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
            Map = new Map(this);
            Map.ParseFileToLayers("game/Map/list/Outdoor.txt");
        }

        private void InitPlayer()
        {
            Player = new Player(Map.PlayerSpawnPosition);
            Map.playerDraw = Player.playerPosition;
            Map.Zoom = 4;
            Player.zoomPlayer(Map.Zoom);
        }

        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            if(Map.Layers["WALL"].Data != null)
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
                    case ConsoleKey.UpArrow:
                        // TODO Player move up
                        if (Player.collisionWall(Map.Layers["WALL"].ZoomedData, 'N') == true && Map.Zoom == 4)
                        {
                            Player.mouvPlayer('N', Map.Zoom);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        // TODO Player move down
                        if (Player.collisionWall(Map.Layers["WALL"].ZoomedData, 'S') == true && Map.Zoom == 4)
                        {
                            Player.mouvPlayer('S', Map.Zoom);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        // TODO Player move left
                        if (Player.collisionWall(Map.Layers["WALL"].ZoomedData, 'O') == true && Map.Zoom == 4)
                        {
                            Player.mouvPlayer('O', Map.Zoom);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        // TODO Player move right
                        if (Player.collisionWall(Map.Layers["WALL"].ZoomedData, 'E') == true && Map.Zoom == 4)
                        {
                            Player.mouvPlayer('E', Map.Zoom);

                        }
                        break;
                    case ConsoleKey.Enter:
                        // TODO Player use action
                        break;
                    case ConsoleKey.M:
                        if (Map.Zoom == 4)
                        {
                            Map.Zoom = 1;
                            Player.zoomPlayer(Map.Zoom);
                        }
                        else if (Map.Zoom == 1)
                        {
                            Map.Zoom = 4;
                            Player.zoomPlayer(Map.Zoom);
                        }
                        break;
                    case ConsoleKey.PageDown:
                        Map.Zoom--;
                        break;
                }
                GrassColision();
            }
        }

        private void GrassColision()
        {
            if (Player.collisionGrass(Map.Layers["GRASS"].ZoomedData))
            {
                const int maxPercentage = 15;
                int percentage = new Random().Next(1, 100);
                if (percentage > 0 && percentage <= maxPercentage)
                {
                    Game.StatesList?.Push(new CombatState(game));
                }
            }
        }

        public override void Update()
        {
            //base.Update();

            // Update childs
            // ------ Map
            Map?.Update();
        }

        public override void Render()
        {
            //base.Render();

            // Render childs
            // ------ Map
            Map?.Render();
            
            Player.drawPlayer();
        }
    }
}
