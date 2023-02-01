using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.save
{

    static class SaveManager
    {
        private const string SAVEPATH = "../../../game/save/Save.txt";
        private const string METAPATH = "../../../game/save/Meta.txt";
        static public void SaveData(params Tuple<string, int>[] data)
        {
            StreamWriter writer = new StreamWriter(File.OpenRead(SAVEPATH));

            foreach (var element in data)
            {
                writer.WriteLine(element.Item1.ToString() + " " + element.Item2.ToString());
            }
            writer.WriteLine("endFile");

            writer.Close();
        }

        static public Dictionary<string,int>? LoadData() 
        {
            string? line = "";
            string? key;
            string? value;
            bool onKey = true;

            Dictionary<string, int> data = new Dictionary<string, int>();

            StreamReader reader = new StreamReader(File.OpenRead(SAVEPATH));

            while (line != "endFile")
            {
                key = null;
                value = null;
                onKey= true;

                foreach(var c in line)
                {
                    if (c is not ' ' && onKey)
                        key += c;

                    else if (c is ' ')
                        onKey = false; 

                    else
                        value += c;
                }

                if (key != null && value != null)
                {
                    data.Add(key, Convert.ToInt32(value));
                }

                line = reader.ReadLine();
            }

            reader.Close();


            return data;
        }

        public static List<Tuple<string,int,int,string,int,int>>? LoadMeta(string mapName)
        {
            List<Tuple<string, int, int, string, int, int>>? data = new List<Tuple<string, int, int, string, int, int>>();
            string line = "";
            string map1;
            string map2;
            int posX1;
            int posY1;
            int posX2;
            int posY2;


            StreamReader reader = new StreamReader(File.OpenRead(METAPATH));

            while (line != "endFile")
            {
                map1 = "";
                map2 = "";
                posX1 = 0;
                posY1 = 0;
                posX2 = 0;
                posY2 = 0;

                if (line == ">") 
                {
                    line = reader.ReadLine();
                    map1 = line;
                    if (map1 != mapName) continue;

                    line = reader.ReadLine();
                    posX1 = Convert.ToInt32(line);

                    line = reader.ReadLine();
                    posY1 = Convert.ToInt32(line);

                    line = reader.ReadLine();
                    map2 = line;

                    line = reader.ReadLine();
                    posX2 = Convert.ToInt32(line);

                    line = reader.ReadLine();
                    posY2 = Convert.ToInt32(line);

                    data.Add(Tuple.Create(map1, posX1, posY1, map2, posX2, posY2));
                }

                line = reader.ReadLine();
            }

            reader.Close();


            return data;
        }

    }
}
