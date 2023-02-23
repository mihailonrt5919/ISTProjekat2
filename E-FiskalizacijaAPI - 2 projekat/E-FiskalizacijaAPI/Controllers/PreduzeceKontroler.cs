using E_FiskalizacijaAPI.Modeli;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E_FiskalizacijaAPI.Controllers
{
    [Route("preduzece")]
    [ApiController]
    public class PreduzeceKontroler : ControllerBase
    {
        [HttpGet("svapreduzeca")]
        public IActionResult SvaPreduzeca([FromQuery] string PIB, [FromQuery] string naziv)
        {
            return Ok(Liste.Preduzeca.Where(p => p.PIB.Contains(PIB ?? "") && p.Naziv.ToLower().Contains((naziv ?? "").ToLower())).OrderBy(p => p.PIB).ThenBy(p => p.Naziv));
        }

        [HttpPost("unesipreduzece")]
        public IActionResult UnesiPreduzece([FromBody] Preduzece preduzece)
        {
            if (preduzece.PIB.Length != 9 || !new Regex(@"^\d{9}").IsMatch(preduzece.PIB))
                return BadRequest("PIB mora biti devetocifreni broj");

            if (Liste.Preduzeca.Any(p => p.PIB == preduzece.PIB))
                return BadRequest("Postoji preduzeće sa PIB-om: " + preduzece.PIB + '!');

            Liste.Preduzeca.Add(new Preduzece {
                OdgovornoLice = preduzece.OdgovornoLice,
                KontaktTelefon = preduzece.KontaktTelefon,
                Email = preduzece.Email,
                Naziv = preduzece.Naziv,
                Adresa = preduzece.Adresa,
                PIB = preduzece.PIB
            });

            return Ok();
        }

        [HttpPut("izmenipreduzece/{pib}")]
        public IActionResult IzmeniPreduzece(string pib, [FromBody] Preduzece preduzece)
        {
            int indeks = Liste.Preduzeca.FindIndex(p => p.PIB == pib);

            if (indeks < 0)
                return NotFound();

            Liste.Preduzeca[indeks].OdgovornoLice = preduzece.OdgovornoLice;
            Liste.Preduzeca[indeks].KontaktTelefon = preduzece.KontaktTelefon;
            Liste.Preduzeca[indeks].Email = preduzece.Email;
            Liste.Preduzeca[indeks].Naziv = preduzece.Naziv;
            Liste.Preduzeca[indeks].Adresa = preduzece.Adresa;

            return Ok();
        }

        [HttpGet("{pib}")]
        public IActionResult DohvatiPreduzece(string pib)
        {
            int indeks = Liste.Preduzeca.FindIndex(p => p.PIB == pib);

            if (indeks < 0)
                return NotFound();

            return Ok(Liste.Preduzeca[indeks]);
        }
    }
}
