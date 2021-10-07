using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkmnOrm.Models
{
    public class EvolveTo
    {
        [Key]
        public int IdOfEvolution { get; set; }
        public int IdOfBase { get; set; }
        public string Description { get; set; }
        public EvolveTo(int basePkmnId, int evolvedPkmnId, string description)
        {
            IdOfBase = basePkmnId;
            IdOfEvolution = evolvedPkmnId;
            Description = description;
        }
    }
}
