using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using System.Numerics;
using System.Text;

namespace cs.project07.pokemon.game.states.gui
{
    public class PokemonInfoBox : IRenderable<State>
    {        
        private Pokemon _pokemon;
        private readonly bool _isEnemy;
        
        private float _healthPercentage;
        private float _oldLifePercentage;
        private float _oldExpPercentage;
        private float _expPercentage;

        private Vector2 currentLifePos;
        private Vector2 currentExpPos;

        public PokemonInfoBox(State state, Pokemon pokemon, bool isEnemy) 
        {
            Parent = state;
            _pokemon = pokemon;
            _isEnemy = isEnemy;
            currentLifePos = new Vector2();
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
            Width = 25;
            Height = 6;

            if (_isEnemy) {
                Left = 65;
                Top = 10;
                Height = 4;
            } else
            {
                Left = Parent.Width /2 + 15;
                Top = Parent.Height /2;
            }
            
            _healthPercentage = (_pokemon.Currenthealth / _pokemon.Stat.MaxHP) * 100;
            _oldLifePercentage = (_pokemon.Currenthealth / _pokemon.Stat.MaxHP) * 100;
            _oldExpPercentage = (_pokemon.Experience / _pokemon.RequiredExp) * 100;
            _expPercentage = (_pokemon.Experience / _pokemon.RequiredExp) * 100;

            currentLifePos.X = Left + 3 + _healthPercentage / 5;
            currentLifePos.Y = Top + 3;
            currentExpPos.X = Left + Width + 3 - _expPercentage / 5;
            currentExpPos.Y = Top + 5;

            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
        }

        public void UpdateHealth(Pokemon pokemon) //TODO: Clean this
        {
            
            if (!_isEnemy)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append((int)_pokemon.Currenthealth).Append("/ ").Append((int)_pokemon.Stat.MaxHP);
                
                Console.SetCursorPosition(Left + Width - 8, Top + 4);
                Console.WriteLine(sb.ToString());
            }

            _pokemon = pokemon;
            _healthPercentage = (_pokemon.Currenthealth / _pokemon.Stat.MaxHP) * 100;

            int startIndex = (int)(_oldLifePercentage / 5);
            int endIndex = (int)((_oldLifePercentage - _healthPercentage) / 5);

            if (_healthPercentage <= 0)
                endIndex = 0;

            if (_healthPercentage > _oldLifePercentage)
            {
                endIndex = (int)(_healthPercentage - _oldLifePercentage) / 5;

                currentLifePos.X = Left + 3 + (int)_oldLifePercentage / 5;
                for (int i = startIndex; i < endIndex; i++)
                {
                    Console.SetCursorPosition((int)currentLifePos.X, (int)currentLifePos.Y);
                    RenderBar(i);
                    currentLifePos.X++;
                    Thread.Sleep(100);
                }
            }
            else
            {
                currentLifePos.X = Left + 3 + (int)_oldLifePercentage / 5;
                for (int i = startIndex; i > endIndex; i--)
                {
                    Console.SetCursorPosition((int)currentLifePos.X, (int)currentLifePos.Y);
                    RenderBar(i);
                    currentLifePos.X--;
                    Thread.Sleep(100);
                }
            }
            
            _oldLifePercentage = _healthPercentage;
        }

        public void UpdateExperience(int experience)
        {
            int lvUp = 0;
            while (experience > 0)
            {
                float _requiredExp = (int)_pokemon.RequiredExp + lvUp * Pokemon.LEVEL_UP_STEP;
                _expPercentage = (experience / _requiredExp) * 100;
                if (_expPercentage >= 100) _expPercentage = 100;
                
                int startIndex = 20 - (int)(_oldExpPercentage / 5);
                int endIndex = 20 - (int)((_expPercentage - _oldExpPercentage) / 5);

                currentExpPos.X = Left + Width - 1 - (int)_oldExpPercentage / 5;
                for (int i = startIndex; i > endIndex; i--) // TODO: Fix this
                {
                    Console.SetCursorPosition((int)currentExpPos.X, (int)currentExpPos.Y);
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write(' ');
                    currentExpPos.X--;
                    Thread.Sleep(100);
                }

                Console.BackgroundColor = ConsoleColor.Black;

                experience -= (int)_requiredExp;
                if (experience > 0)
                {
                    lvUp++;
                    _oldExpPercentage = 0;
                    
                    Console.SetCursorPosition(Left + 4, (int)currentExpPos.Y);
                    Console.WriteLine(new string(' ', 20));
                }
            }
        }

        private void RenderBasicInfo()
        {
            Console.SetCursorPosition(Left + Width/2 - _pokemon.Name.Length/2, Top);
            Console.ForegroundColor = TypeChart.TypeColor[_pokemon.Element];
            Console.WriteLine(_pokemon.Name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Left + Width - 6, Top + 1);
            Console.WriteLine("Lv: " + _pokemon.Level);
            Console.SetCursorPosition(Left, Top + 3);
            Console.Write(" HP: ");
        }

        private void RenderPlayerPokemonInfo()
        {
            Console.SetCursorPosition(Left + Width - 8, Top + 4);
            Console.WriteLine((int)_pokemon.Currenthealth + "/ " + (int)_pokemon.Stat.MaxHP);
            Console.SetCursorPosition(Left, Top + 5);
            Console.WriteLine("Exp:");
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(Left + Width - 1 - i, Top + 5);
                Console.BackgroundColor = _expPercentage > i * 5 ? ConsoleColor.Cyan : BackgroundColor;
                Console.WriteLine(' ');
            }
        }

        private void RenderLifeBar()
        {
            for (int i = 1; i <= 20; i++)
            {
                Console.SetCursorPosition(Left + 3 + i, Top + 3);
                RenderBar(i);
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private void RenderBar(int i)
        {
            if (i * 5 <= _healthPercentage && _healthPercentage <= 40) Console.BackgroundColor = ConsoleColor.DarkYellow;
            else if (i * 5 <= _healthPercentage) Console.BackgroundColor = ConsoleColor.DarkGreen;
            else Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(' ');
        }

        public void Clear()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(Left, Top + i);
                Console.WriteLine(new string(' ', Width));
            }
        }

        public void Render()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;

            PaintBackground();
            RenderBasicInfo();
            RenderLifeBar();

            if (!_isEnemy)
            {
                RenderPlayerPokemonInfo();
            }
        }

        private void PaintBackground()
        {
            Console.BackgroundColor = BackgroundColor;
            
            for (int y = 0; y < Height+1; y++)
            {
                Console.SetCursorPosition(Left, Top + y);
                Console.WriteLine(new string(' ', Width));
            }
        }
    }
}
