using E_FiskalizacijaAPI.Modeli;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace E_FiskalizacijaAPI.Controllers
{
    [Route("faktura")]
    [ApiController]
    public class FakturaKontroler : ControllerBase
    {
        [HttpPost("unesifakturu")]
        public IActionResult UnosFakture([FromBody] Faktura faktura)
        {
            string pib = !Liste.Preduzeca.Any(p => p.PIB == faktura.PIBOd) ? faktura.PIBOd : !Liste.Preduzeca.Any(p => p.PIB == faktura.PIBZa) ? faktura.PIBZa : "";

            if (!string.IsNullOrEmpty(pib))
                return BadRequest("Ne postoji preduzece sa PIB-om: " + pib);

            double ukCena = 0;
            faktura.Stavke.ForEach(s => ukCena += s.CenaPoJediniciMere * s.Kolicina);
            int brFaktura = Liste.Fakture.Count;

            Liste.Fakture.Add(new Faktura {
                Id = brFaktura > 0 ? Liste.Fakture[brFaktura - 1].Id : 0,
                PIBZa = faktura.PIBZa,
                PIBOd = faktura.PIBOd,
                DatumGenerisanja = faktura.DatumGenerisanja,
                DatumValute = faktura.DatumValute,
                Stavke = faktura.Stavke,
                UkupnoZaPlacanje = ukCena,
                TipFakture = faktura.TipFakture
            });

            return Ok();
        }

        [HttpPut("izmeni/{id}")]
        public IActionResult IzmeniFakturu(int id, [FromBody] Faktura faktura)
        {
            string pib = !Liste.Preduzeca.Any(p => p.PIB == faktura.PIBOd) ? faktura.PIBOd : !Liste.Preduzeca.Any(p => p.PIB == faktura.PIBZa) ? faktura.PIBZa : "";

            if (!string.IsNullOrEmpty(pib))
                return BadRequest("Ne postoji preduzece sa PIB-om: " + pib);

            int indeks = Liste.Fakture.FindIndex(f => f.Id == id);
            if (indeks < 0)
                return NotFound("Ne postoji faktura sa id-jem: " + id);

            double ukCena = 0;
            faktura.Stavke.ForEach(s => ukCena += s.CenaPoJediniciMere * s.Kolicina);

            Liste.Fakture[indeks].PIBZa = faktura.PIBZa;
            Liste.Fakture[indeks].PIBOd = faktura.PIBOd;
            Liste.Fakture[indeks].DatumGenerisanja = faktura.DatumGenerisanja;
            Liste.Fakture[indeks].DatumValute = faktura.DatumValute;
            Liste.Fakture[indeks].Stavke = faktura.Stavke;
            Liste.Fakture[indeks].UkupnoZaPlacanje = ukCena;
            Liste.Fakture[indeks].TipFakture = faktura.TipFakture;

            return Ok();
        }

        [HttpGet("preduzece/{pib}")]
        public IActionResult FaktureZaPreduzece(string pib, [FromQuery] int? stranica, [FromQuery] int? velicinaStranice, [FromQuery] double? iznos, [FromQuery] string nazivStavke)
        {
            stranica ??= 1;
            velicinaStranice ??= 10;

            return Ok(FakZaPred(pib).Where(f => f.UkupnoZaPlacanje == (iznos ?? f.UkupnoZaPlacanje) && f.Stavke.Any(s => s.Naziv.ToLower().Contains((nazivStavke ?? "").ToLower())))
                .Skip((int)((stranica - 1) * velicinaStranice)).Take((int)velicinaStranice));
        }

        [HttpGet("preduzece/{pib}/bilans")]
        public IActionResult BilansPreduzeca(string pib, [FromQuery] DateTime datumOd, [FromQuery] DateTime datumDo)
        {
            var fakture = FakZaPred(pib).Where(f => DateTime.Compare(datumOd, f.DatumGenerisanja) < 0
                                                && DateTime.Compare(datumDo, f.DatumGenerisanja) > 0);

            return Ok(fakture.Where(f => f.PIBOd == pib).Sum(f => f.UkupnoZaPlacanje) - fakture.Where(f => f.PIBZa == pib).Sum(f => f.UkupnoZaPlacanje));
        }

        private static IEnumerable<Faktura> FakZaPred(string pib)
        {
            return Liste.Fakture.Where(f => f.PIBOd == pib || f.PIBZa == pib);
        }
    }
}
