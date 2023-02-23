using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_FiskalizacijaAPI.Modeli
{
    public class Faktura
    {
        public int Id { get; set; }
        public string PIBZa { get; set; }
        public string PIBOd { get; set; }
        public DateTime DatumGenerisanja { get; set; }
        public DateTime DatumValute { get; set; }
        public List<Stavka> Stavke { get; set; }
        public double UkupnoZaPlacanje { get; set; }
        public TipFakture TipFakture { get; set; }
    }
}
