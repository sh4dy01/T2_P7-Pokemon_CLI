using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.Registry;
using cs.project07.pokemon.game.states;
using cs.project07.pokemon.game.states.list;
using System.Numerics;

namespace cs.project07.pokemon.game
{
    public class Game : IUpdatable, IRenderable<Game>, ISavable
    {
        public static Vector2 ConsoleSize = new(237,60);

        public bool Running = true;
        public Game Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        public static Stack<State>? StatesList;

        public Game()
        {
            Console.Title = "Pokemon";
            Init();
        }

        public void InitDefaults()
        {
            Console.SetBufferSize(Convert.ToInt32(ConsoleSize.X), Convert.ToInt32(ConsoleSize.Y));
            Console.SetWindowSize(Convert.ToInt32(ConsoleSize.X), Convert.ToInt32(ConsoleSize.Y));
            Console.SetWindowPosition(0, 0);
            Parent = this;
            Left = Console.WindowLeft;
            Top = Console.WindowTop;
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            BackgroundColor = ConsoleColor.DarkGray;
            ForegroundColor = ConsoleColor.Black;
            StatesList = new Stack<State>();
            TypeChart.Init();
            PokemonListManager.Init();
        }

        private void InitStates()
        {
            StatesList?.Push(new MenuState(this));
        }

        private void Init()
        {
            // Init variables
            InitDefaults();

            // Init states
            InitStates();

            // Hide cursor
            Console.CursorVisible = false;
        }

        public void Run()
        {
            Update();
            Render();
            do
            {
                HandleEvent();
                Update();
                Render();
            } while (Running);
        }

        private static void HandleEvent()
        {
            ConsoleKey pressedKey;

            do
            {
                pressedKey = Console.ReadKey(true).Key;
            } while (Console.KeyAvailable);

            switch (pressedKey)
            {
                /*case ConsoleKey.Escape:
                    End();
                    break;*/
                default:
                    if (StatesList?.Count > 0)
                        StatesList.First().HandleKeyEvent(pressedKey);
                    break;
            }
        }

        public void Update()
        {
            if (StatesList?.Count > 0)
                StatesList.First().Update();
            else
                End();
        }

        public void Render()
        {
            Console.ResetColor();

            // Check if program still alive else return
            if (!Running) return;

            if (StatesList?.Count > 0)
                StatesList.First().Render();
        }

        public void Save()
        {
            // Save the state instance
            foreach (State s in StatesList)
            {
                s.Save();
            }
            SaveManager.SaveData();
        }

        public void Load()
        {
            // Load the state instance

            SaveManager.LoadData();
            foreach (State s in StatesList)
            {
                s.Load();
            }
        }

        private void End()
        {
            Running = false;
            Console.ResetColor();
            
            //Console.Clear();
        }
    }
}
