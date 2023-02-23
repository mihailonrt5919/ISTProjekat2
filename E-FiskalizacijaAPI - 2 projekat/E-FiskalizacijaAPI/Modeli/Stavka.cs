using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_FiskalizacijaAPI.Modeli
{
    public class Stavka
    {
        public string Naziv { get; set; }
        public double CenaPoJediniciMere { get; set; }
        public string JedinicaMere { get; set; }
        public int Kolicina { get; set; }
    }
}
