using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkmnOrm.Models
{
    public class PokemonImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public PokemonImage(int id)
        {
            Id = id;
            Path = createPath(id);
        }

        private string createPath(int id)
        {
            return $@"C:\plushogskolan\Webbutveckling Backend\PkmnWebApi\PkmnOrm\Images\{id}.jpg";
        }
    }
}
