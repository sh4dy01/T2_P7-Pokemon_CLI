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

        public MenuState(Game game) : base(game)
        {
            Init();
        }

        protected override void Init()
        {
            Name = "Main Menu";
            _dialogBox = new DialogBox(this);
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
                    Game.StatesList?.Push(new GameState(Parent));
                }
            };
            _buttons["CREDITS"] = new Button(_dialogBox, "Credits")
            {
                Action = () =>
                {
                    
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
            _buttonManager?.Render();
        }
    }
}
