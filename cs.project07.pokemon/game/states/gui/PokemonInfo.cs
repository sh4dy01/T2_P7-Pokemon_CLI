using cs.project07.pokemon.game.entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.states.gui
{
    internal class PokemonInfo : IRenderable<State>
    {
        private Pokemon _pokemon;
        private bool _isEnemy;
        private float _percentage;

        public PokemonInfo(State state, Pokemon pokemon, bool isEnemy) 
        {
            Parent = state;
            _pokemon = pokemon;
            _isEnemy = isEnemy;
            _percentage = (_pokemon.Currenthealth / _pokemon.MaxHealth) * 100;
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
            _percentage = (_pokemon.Currenthealth / _pokemon.MaxHealth) * 100;
        }

        public void Render()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;

            Console.SetCursorPosition(Left, Top);
            Console.WriteLine(_pokemon.Name);
            Console.SetCursorPosition(Left+Width-6, Top+1);
            Console.WriteLine("Level : " + _pokemon.Level);
            Console.SetCursorPosition(Left, Top + 3);
            Console.Write("HP: ");

            for (int i = 1; i <= 20; i++)
            {
                Console.SetCursorPosition(Left+3+i, Top+3);
                if (i*5 <= _percentage) Console.BackgroundColor = ConsoleColor.DarkGreen;
                else Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Write(' ');
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
