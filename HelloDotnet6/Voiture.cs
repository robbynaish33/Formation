using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDotnet6
{
    internal record Voiture(string Immatriculation, string Marque, string Modele, int Prix)
    {
        public int Prix { get; set; } = Prix;
    }
}
