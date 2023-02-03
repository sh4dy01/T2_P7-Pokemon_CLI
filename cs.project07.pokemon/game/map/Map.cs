/*****************************
 * **** MAP TRANSLATION **** *
 *****************************
 *                           *
 * |#| => Walls (collidable) *
 * |*| => Grass (spawnable)  *
 * | | => Ground             *
 * |@| => Player spawn       *
 * |T| => Teleporter         *
 * |A| => Arceus Boss        *
 *                           *
 *****************************/

using cs.project07.pokemon.game.states;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game;
using cs.project07.pokemon.game.states.list;
using cs.project07.pokemon.game.Registry;

namespace cs.project07.pokemon.game.map
{
    public class Map : IUpdatable, IRenderable<State>, ISavable
    {
        public Vector2 PlayerSpawnPosition;
        public Dictionary<string, Layer>? Layers;
        private int _itemsId;
        public int ItemsId { get { _itemsId++; return _itemsId;  } }

        public List<Tuple<string, int, int, string, int, int>>? _Teleporters { get; private set; }
        private string _Name;
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

        public string GetName() { return _Name; }
        public State Parent { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Map(State parent, string Name)
        {
            Parent = parent;
            _Name = Name;
            _itemsId = 0;
            Init();
        }

        private void Init()
        {
            InitDefaults();
            InitLayers();
        }

        public void ModifyMap (int x, int y , string layerName, char newChar)
        {
            char[,] tempData = Layers[layerName].ZoomedData;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tempData[x+i,y+j] = newChar;
                }
            }

            Layers[layerName].SetZoomData(tempData);
        }

        public void InitDefaults()
        {
            Zoom = 4;
            Left = Parent.Left;
            Top = Parent.Top;
            Width = Parent.Width;
            Height = Parent.Height;
            BackgroundColor = ConsoleColor.Gray;
            ForegroundColor = ConsoleColor.Black;
            _Teleporters = SaveManager.LoadMeta(_Name);
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
                },

                ["TELEPORTER"] = new Layer(this)
                {
                    BackgroundColor = ConsoleColor.DarkMagenta,
                    ForegroundColor = ConsoleColor.DarkMagenta
                },

                ["PLAYER"] = new Layer(this)
                {
                    Spawnable = false,
                    BackgroundColor = ConsoleColor.Black,
                    ForegroundColor = ConsoleColor.Black
                },
                ["BOSS"] = new Layer(this)
                {
                    Spawnable = true,
                    BackgroundColor = ConsoleColor.Black,
                    ForegroundColor = ConsoleColor.Black
                },
                ["ITEMS"] = new Layer(this)
                {
                    BackgroundColor = ConsoleColor.Yellow,
                    ForegroundColor = ConsoleColor.Yellow
                }
            };
        }

        int rows;
        int cols;
        public char[,] grid;
        public void ParseFileToLayers(string filePath)
        {
            char[] possibilities = { '#', '*', ' ', '@','T', 'A' };

            string[] lines = File.ReadAllLines(filePath);
            string firstLine = lines[0];
            rows = lines.Length;
            cols = firstLine.Length;


            foreach (char possibility in possibilities)
            {
                grid = new char[rows, cols];
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
                    case 'T':
                        Layers?["TELEPORTER"].InitData(grid);
                        break;
                    case 'A':
                        Layers?["BOSS"].InitData(grid);
                        break;
                }
                //Zoom = 4;
            }
            char[] ItemsPossibilities = InventoryManager.ItemsPossibilities;
            grid = new char[rows, cols];

            foreach (char possibility in ItemsPossibilities)
            {
                
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

                Layers?["ITEMS"].InitData(grid);
            }
        }

        public void Update()
        {
            Left = Parent.Left;
            Top = Parent.Top;
            //updatePlayer();

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
            if (Layers is not { Count: > 0 }) return;
            foreach (Layer layer in Layers.Values)
                layer.Render();
        }

        public void Save() {
            SaveManager.PrepareData(
                new Tuple<string, int>("NumberOfTakenItems", _itemsId)
                );
        }

        public void Load() {
            var data = SaveManager.Loaded;
            int index;
            if (data.ContainsKey("NumberOfTakenItems"))
            {
                index = data["NumberOfTakenItems"];
                for (int i = 1; i < index+1; i++)
                {
                    SaveManager.PrepareData(
                        new Tuple<string, int>("item" + i + "x", data["item" + i + "x"]),
                        new Tuple<string, int>("item" + i + "y", data["item" + i + "y"])
                        );
                    ModifyMap(data["item" + i + "x"], data["item" + i + "y"], "ITEMS", '\0');
                    ModifyMap(data["item" + i + "x"], data["item" + i + "y"], "GROUND", ' ');
                }

                _itemsId = data["NumberOfTakenItems"];
            }
        }

        /* ######################################################### */
        public class Layer : IUpdatable, IRenderable<Map>
        {
            public char[,]? Data;
            private char[,]? _zoomedData;
            public char[,]? ZoomedData { get => _zoomedData;}
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

            public void SetZoomData (char[,]? newData) { _zoomedData = newData; }

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
                _zoom = 4;
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

                Tuple<int,int> CameraOffset = ((GameState)Parent.Parent).SetCameraOffset();

                if (Zoom == 4)
                {
                    for (int y = 0; y < rows-CameraOffset.Item1; y++)
                    { 
                        for (int x = 0; x < cols-CameraOffset.Item2; x++)
                        {
                            char element = _zoomedData[y + CameraOffset.Item1, x + CameraOffset.Item2];
                            if (element == '\0') continue;
                            if (Left + x >= Left + Width || Top + y >= Top + Height) continue;
                            Console.SetCursorPosition(Left + x, Top + y);
                            Console.Write(element);
                        }
                    }
                }
                else
                {
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