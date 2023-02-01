using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon
{
    internal interface IInitializable
    {
        void InitDefaults();
    }
    internal interface IUpdatable
    {
        void Update();
    }

    internal interface IRenderable<T> : IInitializable where T : class
    {
        T Parent { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
        void Render();
    }

    internal interface ISavable
    {
        void Save();
        void Load();
    }
}
