using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace cs.project07.pokemon.game.save
{
    
    static class Save
    {
        private const string PATH = "./Save.txt";
        static void SaveData(params Tuple<string, int>[] data)
        {
            StreamWriter writer = new StreamWriter(File.OpenRead(PATH));

            foreach (var element in data)
            {
                writer.WriteLine(element.Item1.ToString() + " " + element.Item2.ToString());
            }
            writer.WriteLine("endFile");

            writer.Close();
        }

        static Dictionary<string,int>? LoadData() 
        {
            string? line = "";
            string? key;
            string? value;
            bool onKey = true;

            Dictionary<string, int> data = new Dictionary<string, int>();

            StreamReader reader = new StreamReader(File.OpenRead(PATH));

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

    }
}
