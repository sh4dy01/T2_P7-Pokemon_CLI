using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.states.gui
{
    internal class PokemonInfoBox : IRenderable<State>
    {
        private Pokemon _pokemon;
        private bool _isEnemy;
        private float _healthPercentage;
        private float _expPercentage;


        public PokemonInfoBox(State state, Pokemon pokemon, bool isEnemy) 
        {
            Parent = state;
            _pokemon = pokemon;
            _isEnemy = isEnemy;
            _healthPercentage = (_pokemon.Currenthealth / _pokemon.MaxHealth) * 100;
            _expPercentage = (_pokemon.Experience / _pokemon.RequiredExp) * 100;
            InitDefaults();
        }

        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get ; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public void InitDefaults()
        {
            Width = 20;
            Height = 8;

            if (_isEnemy) {
                Left = 65;
                Top = 15;
            } else
            {
                Left = Parent.Width /2 + 25;
                Top = Parent.Height /2;
            }
           
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
        }

        public void UpdateUI(Pokemon pokemon)
        {
            _pokemon = pokemon;
            _healthPercentage = (_pokemon.Currenthealth / _pokemon.MaxHealth) * 100;
            _expPercentage = (_pokemon.Experience / _pokemon.RequiredExp) * 100;
        }

        public void Render()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;

            Console.SetCursorPosition(Left, Top);
            Console.WriteLine(_pokemon.Name);
            Console.SetCursorPosition(Left+Width-6, Top+1);
            Console.WriteLine("Lv : " + _pokemon.Level);
            Console.SetCursorPosition(Left, Top + 3);
            Console.Write("HP: ");

            for (int i = 1; i <= 20; i++)
            {
                Console.SetCursorPosition(Left+3+i, Top+3);
                if (i * 5 <= _healthPercentage && _healthPercentage <= 40) Console.BackgroundColor = ConsoleColor.DarkYellow;
                else if (i * 5 <= _healthPercentage) Console.BackgroundColor = ConsoleColor.DarkGreen;
                else Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(' ');
            }
            Console.BackgroundColor = ConsoleColor.Black;

            if (!_isEnemy)
            {
                Console.SetCursorPosition(Left + Width - 2, Top + 4);
                Console.WriteLine(_pokemon.Currenthealth + "/ " + _pokemon.MaxHealth);
                
                for (int i = 0; i < 20; i++)
                {
                    Console.SetCursorPosition(Left + Width + 3 - i, Top + 5);
                    if (_expPercentage > i * 5) Console.BackgroundColor = ConsoleColor.Cyan;
                    else Console.BackgroundColor = BackgroundColor;
                    Console.WriteLine(' ');
                }
            }
        }
    }
}
