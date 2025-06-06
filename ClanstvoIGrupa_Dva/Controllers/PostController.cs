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
        [HttpPost]
        public ActionResult Create([FromBody] Post post)
        {
            if (post == null || post.UserId == null
            || post.UserId <= 0
            || string.IsNullOrWhiteSpace(post.Content))
            {
                return BadRequest("Morate poslati vazeci UserId i ne-prazan content.");
            }

            try
            {
                var kreirana = postDbRepository.Create(post.UserId.Value, post.Content);
                return Created($"/api/posts/{kreirana.Id}", kreirana);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska prilikom kreiranja objave: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool deleted = postDbRepository.Delete(id);

                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Dogodila se greska prilikom brisanja objave: {ex.Message}");
            }
        }
    }
}
