/*****************************
 * **** MAP TRANSLATION **** *
 *****************************
 *                           *
 * |#| => Walls (collidable) *
 * |*| => Grass (spawnable)  *
 * | | => Ground             *
 * |@| => Player spawn       *
 *                           *
 *****************************/

using cs.project07.pokemon.game.states;
using System.Collections;
using System.Numerics;
using System.Xml.Linq;

namespace cs.project07.pokemon.game.map
{
    public class Map : IUpdatable, IRenderable<State>
    {
        public Vector2 PlayerSpawnPosition;
        public Dictionary<string, Layer>? Layers;

        private int _zoom;
        public int Zoom
        {
            get => _zoom;
            set
            {
                if (value is >= 1 and <= 6)
                {
                    _zoom = value;
                    if (Layers == null || Layers.Count <= 0) return;
                    foreach (Layer layer in Layers.Values)
                        layer.Zoom = value;
                }
            }
        }

        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Map(State parent)
        {
            Parent = parent;
            Init();
        }

        private void Init()
        {
            InitDefaults();
            InitLayers();
        }

        public void InitDefaults()
        {
            Zoom = 1;
            Left = Parent.Left;
            Top = Parent.Top;
            Width = Parent.Width;
            Height = Parent.Height;
            BackgroundColor = ConsoleColor.Gray;
            ForegroundColor = ConsoleColor.Black;
        }

        private void InitLayers()
        {
            Layers = new Dictionary<string, Layer>
            {
                ["WALL"] = new Layer(this)
                {
                    Collidable = true,
                    BackgroundColor = ConsoleColor.Black,
                    ForegroundColor = ConsoleColor.Black
                },

                ["GROUND"] = new Layer(this)
                {
                    BackgroundColor = ConsoleColor.Green,
                    ForegroundColor = ConsoleColor.Green
                },

                ["GRASS"] = new Layer(this)
                {
                    Spawnable = true,
                    BackgroundColor = ConsoleColor.DarkGreen,
                    ForegroundColor = ConsoleColor.Green
                }
            };

            /*Layers["PLAYER"] = new Layer(this);*/
        }

        public void ParseFileToLayers(string filePath)
        {
            char[] possibilities = { '#', '*', ' ', '@' };

            string[] lines = File.ReadAllLines(filePath);
            string firstLine = lines[0];
            int rows = lines.Length;
            int cols = firstLine.Length;

            foreach (char possibility in possibilities)
            {
                char[,] grid = new char[rows, cols];
                for (int y = 0; y < rows; y++)
                {
                    string line = lines[y];
                    for (int x = 0; x < cols; x++)
                    {
                        char currentChar = line[x];
                        if (currentChar == possibility) grid[y, x] = currentChar;
                        else if (possibility == ' ' && currentChar == '@') grid[y, x] = ' ';
                        else if (currentChar == '@') PlayerSpawnPosition = new Vector2(y, x);
                    }
                }

                switch (possibility)
                {
                    case '#':
                        Layers?["WALL"].InitData(grid);
                        break;
                    case '*':
                        Layers?["GRASS"].InitData(grid);
                        break;
                    case ' ':
                        Layers?["GROUND"].InitData(grid);
                        break;
                }
            }
        }

        public void Update()
        {
            Left = Parent.Left;
            Top = Parent.Top;

            // Update childs
            // ------ layers
            if (Layers == null || Layers.Count <= 0) return;
            foreach (Layer layer in Layers.Values)
                layer.Update();
        }

        public void Render()
        {
            // Render childs
            // ------ layers
            if (Layers == null || Layers.Count <= 0) return;
            foreach (Layer layer in Layers.Values)
                layer.Render();
        }

        /* ######################################################### */
        public class Layer : IUpdatable, IRenderable<Map>
        {
            public char[,]? Data;
            private char[,]? _zoomedData;
            private int _zoom;
            public int Zoom
            {
                get => _zoom;
                set
                {
                    _zoom = value;

                    if (Data == null) return;

                    _zoomedData = new char[Data.GetLength(0) * _zoom, Data.GetLength(1) * _zoom];

                    int rows = Data.GetLength(0);
                    int cols = Data.GetLength(1);

                    for (int y = 0; y < rows; y++)
                    {
                        for (int x = 0; x < cols; x++)
                        {
                            char element = Data[y, x];
                            if (element == '\0') continue;
                            for (int i = 0; i < _zoom; i++)
                            {
                                for (int j = 0; j < _zoom; j++)
                                {
                                    _zoomedData[y * _zoom + i, x * _zoom + j] = element;
                                }
                            }
                        }
                    }
                }
            }
            public bool Visible { get; set; }
            public bool Collidable { get; set; }
            public bool Spawnable { get; set; }
            public Map Parent { get; set; }
            public int Left { get; set; }
            public int Top { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public ConsoleColor ForegroundColor { get; set; }
            public ConsoleColor BackgroundColor { get; set; }

            public Layer(Map parent)
            {
                Parent = parent;
                Init();
            }

            private void Init()
            {
                InitDefaults();
            }

            public void InitDefaults()
            {
                _zoom = 1;
                Visible = true;
                Collidable = false;
                Spawnable = false;
                Left = Parent.Left;
                Top = Parent.Top;
                Width = Parent.Width;
                Height = Parent.Height;
                BackgroundColor = ConsoleColor.Gray;
                ForegroundColor = ConsoleColor.Black;
            }

            public void InitData(char[,] _Data)
            {
                /*int rows = _Data.GetLength(0);
                int cols = _Data.GetLength(1);

                for (int y = 0; y < rows; y++)
                    for (int x = 0; x < cols; x++)
                    {
                        char currentChar = _Data[y, x];

                    }*/

                Data = _Data;
                _zoomedData = _Data;
            }

            public void Update()
            {
                if (!Visible) return;

                Left = Parent.Left;
                Top = Parent.Top;
            }

            public void Render()
            {
                if (_zoomedData == null) return;
                if (!Visible) return;

                Console.BackgroundColor = BackgroundColor;
                Console.ForegroundColor = ForegroundColor;

                int rows = _zoomedData.GetLength(0);
                int cols = _zoomedData.GetLength(1);

                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < cols; x++)
                    {
                        char element = _zoomedData[y, x];
                        if (element == '\0') continue;
                        if (Left + x >= Left + Width || Top + y >= Top + Height) continue;
                        Console.SetCursorPosition(Left + x, Top + y);
                        Console.Write(element);
                    }
                }
            }

            /* ######################################################### */
            /*public class Tile : IUpdatable, IRenderable<Layer>
            {
                public char Char;

                public Tile(Layer parent)
                {
                    Parent = parent;
                    InitDefaults();
                }

                public Layer Parent { get; set; }
                public int Left { get; set; }
                public int Top { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }
                public ConsoleColor ForegroundColor { get; set; }
                public ConsoleColor BackgroundColor { get; set; }

                public void InitDefaults()
                {
                    Left = Parent.Left;
                    Top = Parent.Top;
                    Width = Parent.Width;
                    Height = Parent.Height;
                    BackgroundColor = ConsoleColor.Gray;
                    ForegroundColor = ConsoleColor.Black;
                }

                public void Update()
                {
                    Left = Parent.Left;
                    Top = Parent.Top;
                }

                public void Render()
                {

                }

            }*/
        }
    }
}