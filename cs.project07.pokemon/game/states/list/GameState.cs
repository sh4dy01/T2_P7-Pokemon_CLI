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

        public Player Player { get; set; }
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
                    //Game.StatesList?.Push(new InventoryState(Parent));
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

            Maps["map1"].ParseFileToLayers("game/map/list/Map1.txt");
            Maps["map2"].ParseFileToLayers("game/map/list/Map2.txt");
            CurrentMap = Maps["map1"];
        }

        private void InitPlayer()
        {
            Player = new Player(CurrentMap.PlayerSpawnPosition, this);
            
            CurrentMap.Zoom = 4;
            Player.zoomPlayer(CurrentMap.Zoom);
        }

        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            if(CurrentMap != null)
            {
                bool mouv = false;
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
                        if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'N') == true && CurrentMap.Zoom == 4)
                        {
                            Player.mouvPlayer('N');
                            mouv = true;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        // TODO Player move down
                        if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'S') == true && CurrentMap.Zoom == 4)
                        {
                            Player.mouvPlayer('S');
                            mouv = true;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        // TODO Player move left
                        if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'O') == true && CurrentMap.Zoom == 4)
                        {
                            Player.mouvPlayer('O');
                            mouv = true;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        // TODO Player move right
                        if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'E') == true && CurrentMap.Zoom == 4)
                        {
                            Player.mouvPlayer('E');
                            mouv = true;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // TODO Player use action
                        break;
                    case ConsoleKey.M:
                        if (CurrentMap.Zoom == 4)
                        {
                            CurrentMap.Zoom = 1;
                            Player.zoomPlayer(CurrentMap.Zoom);
                        }
                        else if (CurrentMap.Zoom == 1)
                        {
                            CurrentMap.Zoom = 4;
                            Player.zoomPlayer(CurrentMap.Zoom);
                        }
                        break;
                    case ConsoleKey.PageDown:
                        CurrentMap.Zoom--;
                        break;
                }
                if(mouv == true)
                {
                    Player.collisionGrass(CurrentMap.Layers["GRASS"].ZoomedData, game);
                    Player.collisionTeleporter(CurrentMap._Teleporters);
                }
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
            
            Player.drawPlayer(CurrentMap.Zoom, SetCameraOffset());
        }

        public void ChangeMap (string mapName)
        {
            CurrentMap = Maps[mapName];
            CurrentMap.Zoom = 4;
        }

        public Tuple<int,int> SetCameraOffset()
        {
            int ConsoleWidth = Convert.ToInt32(Game.ConsoleSize.X);
            int ConsoleHeight = Convert.ToInt32(Game.ConsoleSize.Y);

            int cameraOffsetX;
            int cameraOffsetY;

            Tuple<int, int> result;

            cameraOffsetX = Convert.ToInt32(Player.playerPosition.X) - ConsoleHeight / 2;
            cameraOffsetY = Convert.ToInt32(Player.playerPosition.Y) - ConsoleWidth / 2;

            if (cameraOffsetX < 0) cameraOffsetX = 0;
            if (cameraOffsetY < 0) cameraOffsetY = 0;
            if (cameraOffsetY > ConsoleWidth*4) cameraOffsetY = ConsoleWidth * 4;
            if (cameraOffsetX > ConsoleHeight*4) cameraOffsetX = ConsoleHeight * 4;

            result = new Tuple<int, int>(cameraOffsetX, cameraOffsetY);

            return result;
        }
    }
}
