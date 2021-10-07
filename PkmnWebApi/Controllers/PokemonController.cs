using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PkmnOrm.Models;
using System.Collections.Generic;

namespace PkmnWebApi.Controllers
{
    public class PokemonController : Controller
    {
        private readonly List<Pokemon> _pkmnList = new();

        public PokemonController(List<Pokemon> pkmn)
        {
            _pkmnList = pkmn;
        }

        [Route("Pokemon/")]
        public IActionResult Index()
        {
            List<Pokemon> pkmnList = _pkmnList;
            return View(pkmnList);
        }

        [Route("Pokemon/{id:int}")]
        public IActionResult Index(int id)
        {
            List<Pokemon> pkmn = new();
            pkmn.Add(_pkmnList[id-1]);
            return View(pkmn);
        }

        public static string GetPokemonType(int id)
        {
            Types types = new();
            return types.IntToType[id];
        }
        public static string GetPokemonImage(int id)
        {
            if (id < 1) return "error";
            return $"~/Images/{id}.jpg";
        }
    }
}
