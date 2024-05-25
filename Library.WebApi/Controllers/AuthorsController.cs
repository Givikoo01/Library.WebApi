using Microsoft.AspNetCore.Mvc;
using Library.WebApi.Models;
using Library.WebApi.Repositories;

namespace Library.WebApi.Controllers
{
    /// <summary>
    /// ავტორის კონტროლერი, ავტორების სიის სამართავად
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        /// <summary>
        /// ავტორების კონსტრუქტორი, რომელიც აინჯექთებს ავტორის რეპოზიტორის
        /// </summary>
        /// <param name="authorRepository">ავტორ რეპოზიტორი</param>
        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/Authors
        /// <summary>
        /// გვაძლევს საშუალებას, წამოვიღოთ ავტორების სია
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {
                var authors = await _authorRepository.GetAuthorsAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        /// <summary>
        /// გვაძლევს საშუალებას, წამოვიღოთ კონკრეტული ავტორი Id-ის მიხედვით
        /// </summary>
        /// <param name="id">იმ ავტორის Id, რომელსაც ვეძებთ</param>
        /// <returns></returns>
        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            try
            {
                var author = await _authorRepository.GetAuthorByIdAsync(id);
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        /// <summary>
        /// გვაძლევს საშუალებას, დავარედაქტიროთ კონკრეტული ავტორის ველები
        /// </summary>
        /// <param name="id">ავტორის Id</param>
        /// <param name="author">ავტორი</param>
        /// <returns></returns>
        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            try
            {
                author.Id = id;
                await _authorRepository.UpdateAuthorAsync(author);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        /// <summary>
        /// გვაძლევს საშუალებას, დავამატოთ ახალი ავტორი
        /// </summary>
        /// <param name="author">ავტორი, რომელიც უნდა დავამატოთ</param>
        /// <returns></returns>
        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            try
            {
                await _authorRepository.AddAuthorAsync(author);
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// გვაძლევს საშუალებას, წავშალოთ კონკრეტული ავტორი მოცემული Id-ით
        /// </summary>
        /// <param name="id">იმ ავტორის Id, რომელსაც ვშლით</param>
        /// <returns></returns>
        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                await _authorRepository.DeleteAuthorAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
    }
}
