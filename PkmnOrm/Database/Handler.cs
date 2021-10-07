using PkmnOrm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace PkmnOrm.Database
{
    public static class Handler
    {
        static string path = @"C:\plushogskolan\Webbutveckling Backend\PkmnWebApi\PkmnOrm\PokemonList.json";    // Change the path to wherever you want to save it.
        public static void Write(List<Pokemon> pkmnList)
        {
            string pkmnAsString = JsonConvert.SerializeObject(pkmnList);
            using Stream stream = File.Create(path); 
            using StreamWriter writer = new(stream);
            writer.Write(pkmnAsString);
        }

        public static List<Pokemon> Read()
        {
            StreamReader reader = new(File.Open(path, FileMode.Open));
            return JsonConvert.DeserializeObject<List<Pokemon>>(reader.ReadToEnd());
        }
    }
}
