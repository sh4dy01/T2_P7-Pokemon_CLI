using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.gui;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace cs.project07.pokemon.game.states.list
{
    public class GameState : State
    {
        public Map CurrentMap;
        private Dictionary<string, Map> Maps;

        public Player Player;
        private CombatState Combat;
        private Game game;

        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;
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
            _dialogBox = new DialogBox(this);
            InitMenu();
        }

        private void InitMenu()
        {
            _buttonManager = new ButtonManager();
            _buttons = _buttonManager.Buttons;

            _buttons["INVENTORY"] = new Button(_dialogBox, "Inventory")
            {
                Selected = true,
                Action = () =>
                {
                    Game.StatesList?.Push(new InventoryState(Parent));
                }
            };
            _buttons["SAVE"] = new Button(_dialogBox, "Save")
            {
                Selected = true,
                Action = () =>
                {
                    //TO DO SAVE THE GAME
                }
            };
            _buttons["EXIT"] = new Button(_dialogBox, "Exit")
            {
                Selected = true,
                Action = () =>
                {
                    Game.StatesList?.Pop();
                }
            };

            _buttonManager.InitHandleKeyEvent();

            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons.ElementAt(i).Value.Offsets += new Vector2(3, 1 + i);
            }
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
                Player.collisionGrass(Map.Layers["GRASS"].ZoomedData, game);
            }
        }

        public override void Update()
        {
            //base.Update();

            // Update childs
            // ------ Map
            CurrentMap?.Update();
        }

        public override void Render()
        {
            //base.Render();

            // Render childs
            // ------ Map
            CurrentMap?.Render();
            
            Player.drawPlayer();
        }

        public void ChangeMap (string mapName, int posX, int posY)
        {
            
        }

        public void ChangeMap (string mapName, int posX, int posY)
        {
            
        }
    }
}
