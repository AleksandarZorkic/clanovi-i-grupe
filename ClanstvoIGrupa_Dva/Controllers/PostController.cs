using ClanstvoIGrupa_Dva.Models;
using ClanstvoIGrupa_Dva.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClanstvoIGrupa_Dva.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostDbRepository postDbRepository;

        public PostController(PostDbRepository postsDbRepository)
        {
            this.postDbRepository = postsDbRepository;
        }

        [HttpGet]
        public ActionResult<List<Post>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("PageSize i offset moraju biti veci od nula.");
            }

            try
            {
                var lista = postDbRepository.GetAll(page, pageSize);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska: {ex.Message}");
            }
        } 
    }
}
