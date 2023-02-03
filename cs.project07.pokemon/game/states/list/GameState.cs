using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.gui;
using System.Numerics;

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
        private bool showMenu = false;

        public void switchShowMenu()
        {
            showMenu = false;
        }
        public GameState(Game gameReceive) : base(gameReceive)
        {
            game = gameReceive;
            Init();
        }

        protected override void Init()
        {
            Console.Clear();
            
            Name = "Pokemon";
            InitMap();
            InitPlayer();
            _dialogBox = new DialogBox(this);
            InitMenu();
            Load();
        }

        private void InitMenu()
        {
            _buttonManager = new ButtonManager();
            _buttons = _buttonManager.Buttons;
            _buttons["INVENTORY"] = new Button(_dialogBox, "Inventory")
            {
                Selected = true,
                Offsets = new Vector2(150, 0),
                Action = () =>
                {
                    Game.StatesList?.Push(new InventoryState(Parent));
                }
            };
            _buttons["SAVE"] = new Button(_dialogBox, "Save")
            {
                Selected = false,
                Offsets = new Vector2(150, 0),
                Action = () =>
                {
                    Parent.Parent.Save();
                }
            };
            _buttons["EXIT"] = new Button(_dialogBox, "Exit")
            {
                Selected = false,
                Offsets = new Vector2(150, 0),
                Action = () =>
                {
                    Parent.Parent.Save();
                    while(Game.StatesList?.Count > 0) 
                    {
                        Game.StatesList?.Pop();
                    }
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
            if (CurrentMap == null) return;
            
            if (pressedKey == ConsoleKey.Escape)
                HandleKeyEventButtons(pressedKey);
            bool mouv = false;
            switch (pressedKey)
            {
                case ConsoleKey.UpArrow:
                    // Player move up
                    if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'N') == true 
                        && CurrentMap.Zoom == 4 && !showMenu)
                    {
                        Player.mouvPlayer('N');
                        mouv = true;
                    }
                    if(showMenu) 
                        Button.SelectPrevious(_buttonManager.Buttons); 
                    break;
                case ConsoleKey.DownArrow:
                    // Player move down
                    if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'S') == true
                        && CurrentMap.Zoom == 4 && !showMenu)
                    {
                        Player.mouvPlayer('S');
                        mouv = true;
                    }
                    if(showMenu)
                        Button.SelectNext(_buttonManager.Buttons);
                    break;
                case ConsoleKey.LeftArrow:
                    // Player move left
                    if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'O') == true 
                        && CurrentMap.Zoom == 4 && !showMenu)
                    {
                        Player.mouvPlayer('O');
                        mouv = true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    // Player move right
                    if (Player.collisionWall(CurrentMap.Layers["WALL"].ZoomedData, 'E') == true
                        && CurrentMap.Zoom == 4 && !showMenu)
                    {
                        Player.mouvPlayer('E');
                        mouv = true;
                    }
                    break;
                case ConsoleKey.Enter:
                    // Player use action
                    if(showMenu)
                        Button.ExecuteAction(_buttonManager.Buttons);
                    Player.StopDialog();
                    break;
                case ConsoleKey.M:
                    if (CurrentMap.Zoom == 4 && !showMenu)
                    {
                        CurrentMap.Zoom = 1;
                        Player.zoomPlayer(CurrentMap.Zoom);
                    }
                    else if (CurrentMap.Zoom == 1 && !showMenu)
                    {
                        CurrentMap.Zoom = 4;
                        Player.zoomPlayer(CurrentMap.Zoom);
                    }
                    break;
            }

            if (!mouv) return;
            Player.collisionTeleporter(CurrentMap._Teleporters);
            Player.collisionItems(CurrentMap.Layers["ITEMS"].ZoomedData, CurrentMap);
            Player.collisionGrass(CurrentMap.Layers["GRASS"].ZoomedData, game);
            Player.collisionBoss(CurrentMap.Layers["BOSS"].ZoomedData, game);
        }

        private void HandleKeyEventButtons(ConsoleKey pressedKey)
        {
            showMenu = !showMenu;
            _buttonManager.HandleKeyEvent(pressedKey);
        }

        public override void Update()
        {
            //base.Update();

            // Update childs
            // ------ Map
            if (showMenu == true)
                _buttonManager.Update();
            else
                CurrentMap?.Update();
        }

        public override void Render()
        {
            //base.Render();

            // Render childs
            // ------ Map

            if (showMenu == true)
                _buttonManager?.Render();
            else
                CurrentMap?.Render();
            Player.drawPlayer(CurrentMap.Zoom, SetCameraOffset());
            Player.Render();
        }

        public override void Save()
        {
            Player.Save();

            string mapNumber = "";
            int index = 0;
            foreach (char c in CurrentMap.GetName())
            {
                index++;
                if (index > 3) mapNumber += c;
            }

            SaveManager.PrepareData(
                new Tuple<string, int>("map", Convert.ToInt32(mapNumber))
                );
            CurrentMap?.Save();


        }

        public override void Load()
        {
            SaveManager.LoadData();

            //send Load to childs
            Player.Load();

            //Load in the Map class
            var data = SaveManager.Loaded;
            int? value = null;
            if (data.ContainsKey("map")) value = SaveManager.Loaded["map"];
            if (value != null)
            {

                string map = "map" + Convert.ToString(value);
                CurrentMap = Maps[map];
                CurrentMap.Zoom = CurrentMap.Zoom;
            }

            CurrentMap?.Load();
        }


        public void ChangeMap (string mapName)
        {
            int itemIds = CurrentMap.GetItemID();
            CurrentMap = Maps[mapName];
            CurrentMap.SetItemID(itemIds);
            CurrentMap.Zoom = 4;
        }

        public Tuple<int,int> SetCameraOffset()
        {
            int ConsoleWidth = Convert.ToInt32(Game.ConsoleSize.X);
            int ConsoleHeight = Convert.ToInt32(Game.ConsoleSize.Y);

            var cameraOffsetX = Convert.ToInt32(Player.playerPosition.X) - ConsoleHeight / 2;
            var cameraOffsetY = Convert.ToInt32(Player.playerPosition.Y) - ConsoleWidth / 2;

            if (cameraOffsetX < 0) cameraOffsetX = 0;
            if (cameraOffsetY < 0) cameraOffsetY = 0;
            if (cameraOffsetY > ConsoleWidth*4) cameraOffsetY = ConsoleWidth * 4;
            if (cameraOffsetX > ConsoleHeight*4) cameraOffsetX = ConsoleHeight * 4;

            var result = new Tuple<int, int>(cameraOffsetX, cameraOffsetY);

            return result;
        }
    }
}
