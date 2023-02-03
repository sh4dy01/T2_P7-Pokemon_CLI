using System.Numerics;

namespace cs.project07.pokemon.game.states.gui
{
    internal class Sprite : IRenderable<State>
    {
        protected string[]? _sprite;
        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Sprite(Vector2 position, ConsoleColor fgColor, ConsoleColor bgColor)
        {
            Left = (int)position.X;
            Top = (int)position.Y;
            ForegroundColor = fgColor;
            BackgroundColor = bgColor;
            _sprite = null;
        }

        public bool IsEmpty()
        {
            return _sprite is null ? true : false;
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

        public virtual void LoadSprite(string name)
        {
            if (!IsEmpty()) Clear();

            _sprite = AsciiArtLoader.LoadSprite(name);

            Render();
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
            throw new NotImplementedException();
        }
    }
}
