using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states.list;

namespace cs.project07.pokemon.game.states.gui
{
    internal class AttackInfoBox : IRenderable<State>
    {
        private Attack? _attackInfo;
        private bool _isVisible = false;
        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public AttackInfoBox(CombatState state)
        {
            Parent = state;
            InitDefaults();
        }

        public void InitDefaults()
        {
            Width = 20;
            Height = 8;
            Left = Parent.Width / 3;
            Top = Parent.Height - 10;

            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
        }

        public void Show(Attack attack)
        {
            _isVisible = true;
            _attackInfo = attack;
        }

        public void Hide()
        {
            _isVisible = false;
            _attackInfo = null;
        }

        public void Render()
        {
            if (_isVisible && _attackInfo is not null)
            {
                Console.SetCursorPosition(Left, Top);
                Console.WriteLine(_attackInfo.Name);
                Console.SetCursorPosition(Left, Top+1);
                Console.WriteLine("Damage: " + _attackInfo.Power);
                Console.SetCursorPosition(Left, Top + 2);
                Console.WriteLine("Type: " + _attackInfo.Element);
                Console.SetCursorPosition(Left, Top + 3);
                Console.WriteLine("PP: " + _attackInfo.Usage);
            }
        }
    }
}
