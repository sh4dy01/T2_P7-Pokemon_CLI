#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.

using System.Data;
using System.Numerics;

internal enum Status { IDLE, ACTIVE }

namespace cs.project07.pokemon.game.states.gui
{
    public class Button : IUpdatable, IRenderable<DialogBox>
    {
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                UpdateStatus();
            }
        }
        private Status _currentStatus;

        public Action Action { get; set; }

        public string Text { get; set; }

        public DialogBox Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColor ActiveForegroundColor { get; set; }
        public ConsoleColor ActiveBackgroundColor { get; set; }
        public ConsoleColor _currentBackgroundColor { get; set; }
        public ConsoleColor _currentForegroundColor { get; set; }

        public Vector2 Offsets;

        public Button(DialogBox _Parent, string _Text)
        {
            InitDefaults();
            Parent = _Parent;
            Text = _Text;
            Top = Parent.Top + Parent.Height/ 2;
            Left = Parent.Left + Parent.Width / 2;
        }

        public void InitDefaults()
        {
            Selected = false;
            _currentStatus = Status.IDLE;
            Text = "NOT LOADED";
            Left = 0;
            Top = 0;
            Width = 0;
            Height = 0;
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            ActiveBackgroundColor = ConsoleColor.Black;
            ActiveForegroundColor = ConsoleColor.White;
            _currentBackgroundColor = BackgroundColor;
            _currentForegroundColor = ForegroundColor;
            Offsets.X = 0;
            Offsets.Y = 0;
        }

        private void UpdateStatus()
        {
            if (Selected) _currentStatus = Status.ACTIVE;
            else _currentStatus = Status.IDLE;

            UpdateFromStatus();
        }

        private void UpdateFromStatus()
        {
            switch (_currentStatus)
            {
                case Status.IDLE:
                    if (_currentBackgroundColor == BackgroundColor) break;
                    _currentBackgroundColor = BackgroundColor;
                    _currentForegroundColor = ForegroundColor;
                    break;
                case Status.ACTIVE:
                    if (_currentBackgroundColor == ActiveBackgroundColor) break;
                    _currentBackgroundColor = ActiveBackgroundColor;
                    _currentForegroundColor = ActiveForegroundColor;
                    break;
            }
        }

        public void Update()
        {
            Left = Parent?.Left ?? 0;
            Top = Parent?.Top ?? 0;
        }

        public void Render()
        {
            Console.ForegroundColor = ActiveForegroundColor;
            Console.BackgroundColor = ActiveBackgroundColor;
            
            if (Selected && Left >= 0)
            {
                Console.BackgroundColor = ActiveBackgroundColor;
                Console.ForegroundColor = ActiveForegroundColor;
                Console.SetCursorPosition(Left + (int)Offsets.X - 3, Top + (int)Offsets.Y);
                Console.WriteLine(">> ");
            }

            Console.SetCursorPosition(Left + (int)Offsets.X, Top + (int)Offsets.Y);
            Console.WriteLine(Text);
        }

        public static void SelectPrevious(Dictionary<string, Button> _Buttons)
        {
            if (_Buttons.Count <= 0) return;
            for (int i = _Buttons.Count - 1; i >= 0; i--)
            {
                if (!_Buttons.ElementAt(i).Value.Selected) continue;

                _Buttons.ElementAt(i).Value.Selected = false;
                if (i - 1 >= 0)
                    _Buttons.ElementAt(i - 1).Value.Selected = true;
                else
                    _Buttons.ElementAt(_Buttons.Count - 1).Value.Selected = true;
                break;
            }
        }

        public static void SelectNext(Dictionary<string, Button> buttons)
        {
            if (buttons.Count <= 0) return;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (!buttons.ElementAt(i).Value.Selected) continue;

                buttons.ElementAt(i).Value.Selected = false;
                if (i < buttons.Count - 1)
                    buttons.ElementAt(i + 1).Value.Selected = true;
                else
                    buttons.ElementAt(0).Value.Selected = true;
                break;
            }
        }

        public static void ExecuteAction(Dictionary<string, Button> buttons)
        {
            if (buttons.Count <= 0) return;
            foreach (Button button in buttons.Values)
                if (button.Selected)
                {
                    button.Action();
                    break;
                }
        }
    }
}
