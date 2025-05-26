using clanoviIGrupe.Models;
using clanoviIGrupe.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace clanoviIGrupe.Controllers
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupeKontroler : ControllerBase
    {
        // Kreirao sam takodje kontroler, samo dodaj tvoje metode koje trebaju u zadatku.

        private ClanstvoRepozitorijum clanstvoRepozitorijum = new ClanstvoRepozitorijum();
        private GrupaRepozitorijum grupaRepozitorijum = new GrupaRepozitorijum();
        private KorisnikRepozitorijum korisnikRepozitorijum = new KorisnikRepozitorijum();

        // Aleksandra ovde sam dodao metodu koja se odnosi i na tvoj B2 i na moj A2 tako da neces morati da se mucis.

        [HttpGet("{grupaId}/korisnici/{korisnikId}")]
        public ActionResult<Korisnik> GetSpecificUser(int grupaId, int korisnikId)
        {
            if (!GrupaRepozitorijum.Data.ContainsKey(grupaId))
                return NotFound();

            if (!KorisnikRepozitorijum.Data.ContainsKey(korisnikId))
                return NotFound();

            bool jeKorisnik = ClanstvoRepozitorijum.Data
                .Any(c => c.GrupaId == grupaId && c.KorisnikId == korisnikId);

            if (!jeKorisnik) 
                return NotFound();

            var korisnik = KorisnikRepozitorijum.Data[korisnikId];
            return Ok(korisnik);
        }
    }
}
