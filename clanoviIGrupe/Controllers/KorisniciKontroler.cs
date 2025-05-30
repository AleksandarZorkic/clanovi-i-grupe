using clanoviIGrupe.Models;
using clanoviIGrupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace clanoviIGrupe.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisniciKontroler : ControllerBase
    {
        private readonly KorisnikRepozitorijum korisnikRepozitorijum = new();
        private readonly GrupaRepozitorijum grupaRepozitorijum = new();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            var korisnici = korisnikRepozitorijum.GetFromDataBase();
            return Ok(korisnici);
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            var korisnik = korisnikRepozitorijum.GetFromDataBase().FirstOrDefault(k =>  k.Id == id);

            if (korisnik == null)
            {
                return NotFound();
            }
            return Ok(korisnik);
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme))
            {
                return BadRequest();
            }

            var dodat = korisnikRepozitorijum.AddToDatabase(noviKorisnik);
            return CreatedAtAction(nameof(GetById), new {Id = dodat.Id}, dodat);

        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik uKorisnik)
        {
            if (string.IsNullOrWhiteSpace(uKorisnik.KorisnickoIme))
            {
                return BadRequest();
            }

            var updated = korisnikRepozitorijum.UpdateInDatabase(id, uKorisnik);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
    }
}
