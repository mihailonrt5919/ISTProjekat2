using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_FiskalizacijaAPI.Modeli
{
    public static class Liste
    {
        public static List<Preduzece> Preduzeca { get; set; } = new List<Preduzece>();
        public static List<Faktura> Fakture { get; set; } = new List<Faktura>();
    }
}
