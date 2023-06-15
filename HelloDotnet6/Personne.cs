using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDotnet6
{
    internal class Personne
    {
        public int Id { get; init; }
        public string? Prenom { get; set; }
        public string Nom { get; set; } = "";

        public Personne() {    }
        public Personne(int id) { Id = id; }
    }
}
