using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.Registry;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.states.gui.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.states.list
{
    internal class StarterSelectionState : State
    {
        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;
        private PokemonSprite _sprite;

        bool init = false;
        
        public StarterSelectionState(Game game) : base(game)
        {
            Init();
            BackgroundColor = ConsoleColor.Black;
        }

        protected override void Init()
        {
            Console.Clear();
            
            Name = "Starter Selection Menu";
            _dialogBox = new DialogBox(this);
            _dialogBox.Top = Console.WindowHeight / 2;
            _dialogBox.Left = Console.WindowWidth / 2 - 45;

            InitButtons();
        }

        protected void InitButtons()
        {
            _buttonManager = new ButtonManager();
            _buttons = _buttonManager.Buttons;
            bool selected = true;
            int offsetX = -20;
            int offsetY = 0;
            
            foreach (var starter in PokemonRegistry._starterPokemons)
            {
                _sprite = new PokemonSprite(true, new Vector2(_dialogBox.Left + offsetX, _dialogBox.Top - 20), ForegroundColor, BackgroundColor);
                _sprite.LoadSprite(starter.Name);
                
                _buttons[starter.Name] = new Button(_dialogBox, starter.Name)
                {
                    Selected = selected,
                    Offsets = new Vector2(offsetX + 10, offsetY),
                    Action = () =>
                    {
                        PokemonListManager.SetStarter(new Pokemon(starter, 5));
                        Game.StatesList?.Push(new GameState(Parent));
                    },
                    ForegroundColor = TypeChart.TypeColor[starter.Element],
                    ActiveForegroundColor = TypeChart.TypeColor[starter.Element]

                };

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 3);
                Console.WriteLine("HP: " + starter.Stat.MaxHP);
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 4);
                Console.WriteLine("Type: " + starter.Element);
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 6);
                Console.WriteLine("Attack: " + starter.Stat.Attack);
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 7);
                Console.WriteLine("Defense: " + starter.Stat.Defense);
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 8);
                Console.WriteLine("SP Attack: " + starter.Stat.SPAttack);
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 9);
                Console.WriteLine("SP Defense: " + starter.Stat.SPDefense);
                Console.SetCursorPosition(_dialogBox.Left + offsetX + 10, _dialogBox.Top + 10);
                Console.WriteLine("Speed: " + starter.Stat.Speed);

                selected = false;
                offsetX += 50;
                offsetY -= 1;
            }

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
            init = true;
        }

        public override void Load() { }
        public override void Save() { }
    }
}
