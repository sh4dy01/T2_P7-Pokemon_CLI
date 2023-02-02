using System.Diagnostics.CodeAnalysis;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.states.gui.managers;
using System.Numerics;

namespace cs.project07.pokemon.game.states.list
{
    internal class MenuState : State
    {
        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;
        private Sprite _mainMenu;

        public MenuState(Game game) : base(game)
        {
            Init();
        }

        protected override void Init()
        {
            Name = "Main Menu";
            _dialogBox = new DialogBox(this);
            _dialogBox.Top = Console.WindowHeight / 2 + 5;
            _dialogBox.Left = Console.WindowWidth / 2 - 5;
            _mainMenu = new Sprite(new Vector2(Console.WindowWidth /2 - 40, Console.WindowHeight / 2 - 17), ConsoleColor.White, ConsoleColor.Black);
            _mainMenu.LoadSprite("menu_title");

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
                    //If no save
                        //Push starter state
                    //Else
                    Game.StatesList?.Push(new GameState(Parent));
                }
            };
            _buttons["CREDITS"] = new Button(_dialogBox, "Credits")
            {
                Action = () =>
                {
                    //TODO: Add credits
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
        }
    }
}
