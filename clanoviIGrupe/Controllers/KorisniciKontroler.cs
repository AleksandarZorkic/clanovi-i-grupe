using clanoviIGrupe.Models;
using clanoviIGrupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clanoviIGrupe.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisniciKontroler : ControllerBase
    {
        private KorisnikRepozitorijum korisnikRepozitorijum = new KorisnikRepozitorijum();
        private GrupaRepozitorijum grupaRepozitorijum = new GrupaRepozitorijum();

        [HttpGet]
        public ActionResult<List<Korisnik>> getAll()
        {
            List<Korisnik> korisnici = KorisnikRepozitorijum.Data.Values.ToList();
            return Ok(korisnici);
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> getById(int id)
        {
            if (!KorisnikRepozitorijum.Data.ContainsKey(id))
            {
                return NotFound();
            }
            var korisnik = KorisnikRepozitorijum.Data[id];
            return Ok(korisnik);
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme))
            {
                return BadRequest();
            }

            noviKorisnik.Id = SracunajNoviID(KorisnikRepozitorijum.Data.Keys.ToList());
            KorisnikRepozitorijum.Data[noviKorisnik.Id] = noviKorisnik;
            korisnikRepozitorijum.Save();
            
            return Ok(noviKorisnik);
        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik uKorisnik)
        {
            if (string.IsNullOrWhiteSpace(uKorisnik.KorisnickoIme))
            {
                return BadRequest();
            }
            if (!KorisnikRepozitorijum.Data.ContainsKey(id))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepozitorijum.Data[id];
            korisnik.KorisnickoIme = uKorisnik.KorisnickoIme;
            korisnik.Ime = uKorisnik.Ime;
            korisnik.Prezime = uKorisnik.Prezime;
            korisnik.DatumRodjenja = uKorisnik.DatumRodjenja;
            korisnikRepozitorijum.Save();

            return Ok(korisnik);
        }

        private int SracunajNoviID(List<int> identifikatori)
        {
            int maxId = 0;
            foreach (int id in identifikatori)
            {
                if (maxId < id)
                {
                    maxId = id;
                }
            }
            return maxId + 1;
        }

    }
}
