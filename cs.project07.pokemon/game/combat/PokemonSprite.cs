using cs.project07.pokemon.game.states;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.combat
{
    internal class PokemonSprite : IRenderable<State>
    {
        private string[] _sprite;
        private bool _isEnemy;

        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public PokemonSprite(bool isEnemy, Vector2 position, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            _isEnemy = isEnemy;
            Left = (int)position.X;
            Top = (int)position.Y;
            ForegroundColor = fgColor;
            BackgroundColor = bgColor;
        }

        public bool IsEmpty()
        {
            return _sprite is null ? true : false;
        }

        public void LoadSprite(string name)
        {
            if (!IsEmpty()) Clear();

            switch (_isEnemy)
            {
                case false:
                    _sprite = AsciiArtLoader.GetPlayerSpriteByName(name);
                    Width = 40;
                    break;
                case true:
                    _sprite = AsciiArtLoader.GetEnemySpriteByName(name);
                    Width = 35;
                    break;

            }

            Render();
        }
        
        public void Render()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;

            for (int i = 0; i < _sprite.Length; i++)
            {
                Console.SetCursorPosition(Left, Top + i);
                Console.WriteLine(_sprite[i]);
            }
        }
        
        public void Clear()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;

            for (int i = 0; i < _sprite.Length; i++)
            {
                Console.SetCursorPosition(Left, Top + i);
                Console.WriteLine(new string(' ', Width));
            }
        }

        public void InitDefaults()
        {
            
        }
    }
}
