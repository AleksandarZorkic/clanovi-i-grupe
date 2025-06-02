using System.Xml.Linq;
using ClanstvoIGrupa_Dva.Models;
using ClanstvoIGrupa_Dva.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ClanstvoIGrupa_Dva.Controllers
{
    [Route("api/users")]

    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbRepository UserDbRepository = new();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            var korisnici = UserDbRepository.GetAll();
            return Ok(korisnici);
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            var korisnik = UserDbRepository.GetById(id);

            if (korisnik == null)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpPost]
        public ActionResult<Korisnik> Create(int id, [FromBody] Korisnik noviKorisnik)
        {
            var korisnik = UserDbRepository.Create(noviKorisnik.KorisnickoIme, noviKorisnik.Ime, noviKorisnik.Prezime, noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

            if (korisnik == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = korisnik.Id }, korisnik);
        }
        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik korisnik)
        {
            if (korisnik.Id != 0 && korisnik.Id != id) 
                return BadRequest();

            korisnik.Id = id;

            bool uspesnaPromena = UserDbRepository.Update(korisnik);

            if (!uspesnaPromena)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult<Korisnik> Delete(int id, [FromBody] Korisnik korisnik)
        {
            if(korisnik.Id != 0 && korisnik.Id != id)
            {
                return NotFound();
            }

            korisnik.Id = id;
            bool deleted = UserDbRepository.Delete(korisnik);

            return NoContent();
        }
    }
}
