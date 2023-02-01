#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states;
using cs.project07.pokemon.game.states.list;
using System.Numerics;

namespace cs.project07.pokemon.game
{
    public class Game : IUpdatable, IRenderable<Game>
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
        }

        private void InitStates()
        {
            StatesList?.Push(new CombatState(this));
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
            // Recenter on x axis in case console had been resized
            //Left = Console.BufferWidth / 2 - Width / 2;

            // Update the state instance if there's at least one, else it cut the program
            if (StatesList?.Count > 0)
                StatesList.First().Update();
            else
                End();
        }

        public void Render()
        {
            // Clear the console
            Console.ResetColor();
            //Console.Clear();

            // Check if program still alive else return
            if (!Running) return;

            // Draw the window borders
            // DrawWindowBorder();

            // Render the state instance
            if (StatesList?.Count > 0)
                StatesList.First().Render();
        }

        private void End()
        {
            Running = false;
            Console.ResetColor();
            //Console.Clear();
        }

        //public void SwitchState(State state)
        //{
        //    StatesList?.Push(state);
        //}

        //private void DrawWindowBorder()
        //{

        //    Console.BackgroundColor = BackgroundColor;
        //    Console.ForegroundColor = ForegroundColor;

        //    // Draw state title
        //    Console.SetCursorPosition(Left - 1, Top - 3);
        //    Console.WriteLine(new string(' ', Width + 2));
        //    Console.SetCursorPosition(Left - 1, Top - 2);
        //    Console.WriteLine(new string(' ', Width + 2));
        //    if (StatesList?.Count > 0)
        //    {
        //        Console.SetCursorPosition(Left - 1 + Width / 2 - StatesList.First().Name.Length / 2, Top - 2);
        //        Console.WriteLine(StatesList?.First().Name);
        //    }

        //    // Draw Top and bottom borders
        //    Console.SetCursorPosition(Left - 1, Top - 1);
        //    Console.WriteLine(new string(' ', Width + 2));
        //    Console.SetCursorPosition(Left - 1, Top + Height);
        //    Console.WriteLine(new string(' ', Width + 2));

        //    Console.WriteLine();

        //    // Draw Left and right borders
        //    for (int i = 0; i <= Height; i++)
        //    {
        //        Console.SetCursorPosition(Left - 1, Top + i);
        //        Console.Write(" ");
        //        Console.SetCursorPosition(Left + Width, Top + i);
        //        Console.Write(" ");
        //    }
        //}
    }
}
