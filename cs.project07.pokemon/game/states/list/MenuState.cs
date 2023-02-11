using System.Diagnostics.CodeAnalysis;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.states.gui.managers;
using System.Numerics;
using cs.project07.pokemon.game.save;

namespace cs.project07.pokemon.game.states.list
{
    internal class MenuState : State
    {
        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;
        private Sprite _mainMenu;
        private bool _firstGame;

        public MenuState(Game game) : base(game)
        {
            Console.Clear();
            Console.ResetColor();
            Init();
        }

        protected override void Init()
        {
            Name = "Main Menu";
            _dialogBox = new DialogBox(this);
            _dialogBox.Top = Console.WindowHeight / 2;
            _dialogBox.Left = Console.WindowWidth / 2;
            _mainMenu = new Sprite(new Vector2(Console.WindowWidth /2 - 55, Console.WindowHeight / 2 - 23), ConsoleColor.White, ConsoleColor.Black);
            _mainMenu.LoadSprite("menu_title");
            _firstGame = true;
            Load();

            InitButtons();
        }

        protected void InitButtons()
        {
            _buttonManager = new ButtonManager();

            _buttons = _buttonManager.Buttons;

            _buttons["PLAY"] = new Button(_dialogBox, "Play")
            {
                Selected = true,
                Action = () =>
                {
                    if (_firstGame)
                    {
                        Game.StatesList?.Push(new StarterSelectionState(Parent));
                    }
                    else { Game.StatesList?.Push(new GameState(Parent)); }
                    //Game.StatesList?.Push(new GameState(Parent));
                }
            };
            _buttons["QUIT"] = new Button(_dialogBox, "Quit")
            {
                Offsets = new Vector2(0, 10),
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

        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            HandleKeyEventButtons(pressedKey);
        }   

        private void HandleKeyEventButtons(ConsoleKey pressedKey)
        {
            _buttonManager.HandleKeyEvent(pressedKey);
        }

        override public void Load()
        {
            SaveManager.LoadData();
            var data = SaveManager.Loaded;
            if (data.ContainsKey("firstGame")) if (data["firstGame"] == 1) _firstGame = false; else _firstGame = true;
        }

        override public void Save() 
        {
            SaveManager.PrepareData(
                new Tuple<string, int>("firstGame", 1)
                );
        }

        public override void Update()
        {
            base.Update();

            // Update state childs
            // ------ Buttons
            _buttonManager?.Update();
        }

        public override void Render()
        {
            base.Render();

            // Render state childs
            // ------ Buttons
            _mainMenu.Render();
            _buttonManager?.Render();
            
            Console.SetCursorPosition(10, Parent.Height-1);
            Console.Write("Made by these 3 beautiful people :");
            Console.SetCursorPosition(70, Parent.Height-1);
            Console.Write("Guilian PIPART");
            Console.SetCursorPosition(120, Parent.Height-1);
            Console.Write("Hugo MAESTRACCI");
            Console.SetCursorPosition(160, Parent.Height-1);
            Console.Write("Quentin RIPOT");
        }
    }
}
