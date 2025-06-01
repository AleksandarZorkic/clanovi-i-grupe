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
    }
}
