using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.list;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cs.project07.pokemon.game.states.gui
{
    internal class DialogBox : IRenderable<State>, IUpdatable
    {
        private int _width;
        private int _height;
        protected string _text;
        
        protected ButtonManager _buttonManager;
        protected Dictionary<string, Button> _buttons;

        public ButtonManager ButtonManager { get => _buttonManager; }

        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get => _width; set => _width = value; }
        public int Height { get => _height; set => _height = value; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public string Text { set => _text = value; }

        public DialogBox(State state)
        {
            Parent = state;
            if (state.Name == "Main Menu")
                InitMenu();
            else
                InitDefaults();
        }
        
        public void InitDefaults()
        {
            Width = 100;
            Height = 8;
            Left = Parent.Left + Parent.Width / 2 - Width / 2;
            Top = Parent.Top + Parent.Height - Height - 5;
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            _text = "Default Text";
        }

        private void InitMenu()
        {
            Width = 100;
            Height = 8;
            Left = Parent.Left;
            Top = Parent.Top;
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            _text = null;
        }

        protected void InitDefaultButtons()
        {
            _buttonManager = new ButtonManager();
            _buttons = _buttonManager.Buttons;
        }

        protected virtual void InitButtons()
        {
        }

        protected void PaintBorder()
        {
            for (int i = 1; i < Width; i++)
            {
                Console.SetCursorPosition(Left + i, Top);
                Console.Write("-");
                Console.SetCursorPosition(Left + i, Top + Height);
                Console.Write("-");
            }

            for (int i = 0; i <= Height; i++)
            {
                Console.SetCursorPosition(Left, Top + i);
                Console.Write("|");
                Console.SetCursorPosition(Left + Width, Top + i);
                Console.Write("|");
            }
        }

        protected void PaintBackground()
        {
            for (int w = 1; w < Width; w++)
            {
                for (int h = 1; h < Height; h++)
                {
                    Console.SetCursorPosition(Left + w, Top + h);
                    Console.Write(" ");
                }
            }
        }

        private void PaintText()
        {
            Console.SetCursorPosition(Left + Width / 2 - _text.Length / 2, Top + Height / 2);
            Console.Write(_text);
        }

        public void UpdateText(string text)
        {
            _text = text;
            PaintText();
        }

        public virtual void Render()
        {
            // Render the actual state
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;

            PaintBorder();
            PaintBackground();
            PaintText();
        }

        public void Update()
        {
            // Update the actual state
            Left = Parent.Left + Parent.Width / 2 - Width / 2;
            Top = Parent.Top + Parent.Height - Height - 5;
        }
    }
}
