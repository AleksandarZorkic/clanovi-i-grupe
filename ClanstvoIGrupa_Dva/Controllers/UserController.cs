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
        private readonly UserDbRepository UserDbRepository;

        public UserController(UserDbRepository userDbRepository)
        {
            this.UserDbRepository = userDbRepository;
        }

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            try
            {
                var korisnici = UserDbRepository.GetAll();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            try
            {
                var korisnik = UserDbRepository.GetById(id);

                if (korisnik == null)
                {
                    return NotFound();
                }
                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska: {ex.Message}");
            }
        }
        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            try
            {
                var korisnik = UserDbRepository.Create(noviKorisnik.KorisnickoIme, noviKorisnik.Ime, noviKorisnik.Prezime, noviKorisnik.DatumRodjenja.ToString("yyyy-MM-dd"));

                if (korisnik == null)
                {
                    return BadRequest();
                }
                return CreatedAtAction(nameof(GetById), new { id = korisnik.Id }, korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik korisnik)
        {
            try
            {
                if (korisnik.Id != 0 && korisnik.Id != id)
                    return BadRequest();

                korisnik.Id = id;

                bool uspesnaPromena = UserDbRepository.Update(korisnik);

                if (!uspesnaPromena)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<Korisnik> Delete(int id)
        {
            try
            {
                var kotisnik = UserDbRepository.GetById(id);
                if (kotisnik == null)
                    return NotFound();

                bool deleted = UserDbRepository.Delete(kotisnik);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska: {ex.Message}");
            }
        }
    }
}
