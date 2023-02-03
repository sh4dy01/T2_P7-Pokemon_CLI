
namespace cs.project07.pokemon.game.states
{
    public abstract class State : IUpdatable, IRenderable<Game>, ISavable
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        public Game Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        protected State(Game game)
        {
            Parent = game;
            InitDefaults();
        }

        public void InitDefaults()
        {
            Name = "NOT LOADED";
            Left = 0;
            Top = 0;
            Width = Parent.Width;
            Height = Parent.Height;
            BackgroundColor = ConsoleColor.Gray;
            ForegroundColor = ConsoleColor.Black;
        }

        protected abstract void Init();

        public virtual void HandleKeyEvent(ConsoleKey pressedKey)
        {
        }

        public virtual void Update()
        {
            // Update the actual state
            Left = Parent.Left;
            Top = Parent.Top;
        }

        public virtual void Render()
        {
            // Render the actual state
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            //PaintBackground();
        }

        public virtual void Save() { }
        public virtual void Load() { }

        protected void PaintBackground()
        {
            Console.BackgroundColor = BackgroundColor;
            
            for (var y = 0; y < Height; y++)
            {
                Console.SetCursorPosition(Left, Top + y);
                Console.WriteLine(new string(' ', Width));
            }
        }
    }
}
