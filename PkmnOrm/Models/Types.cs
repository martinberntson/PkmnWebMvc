using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkmnOrm.Models
{
    public class Types
    {

        public Dictionary<int, String> IntToType = new();
        public Dictionary<String, int> TypeToInt = new();


        internal static string GetType(int i)
        {
            throw new NotImplementedException();
        }


        public Types()
        {
            String[] types = { "Normal", "Fire", "Fighting", "Water", "Flying", "Grass", "Poison", "Electric", "Ground",
                                "Psychic", "Rock", "Ice", "Bug", "Dragon", "Ghost", "Dark", "Steel", "Fairy", "???" };
            for (int x = 0; x < types.Length; x++)
            {
                IntToType.Add(x, types[x]);
                TypeToInt.Add(types[x], x);
            }
        }
    }
}
