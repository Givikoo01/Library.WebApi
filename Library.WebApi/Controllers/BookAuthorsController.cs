using Microsoft.AspNetCore.Mvc;
using Library.WebApi.Repositories;

namespace Library.WebApi.Controllers
{
    /// <summary>
    /// კონტროლერი, რომელიც გამოიყენება წიგნსა და ავტორს შორის ურთიერთობის სამართავად
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;
        /// <summary>
        /// კონსტრუქტორი, რომელიც დააინჯექთებს წიგნების და ავტორის რეპოზიტორის
        /// </summary>
        /// <param name="bookAuthorRepository"></param>
        public BookAuthorsController(IBookAuthorRepository bookAuthorRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        /// <summary>
        /// გვაძლევს საშუალებას, დავუკავშიროთ ერთმანეთს წიგნი და ავტორი
        /// </summary>
        /// <param name="bookId">წიგნის Id</param>
        /// <param name="authorId">ავტორის Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAuthorToBook(int bookId, int authorId)
        {
            try
            {
                await _bookAuthorRepository.AddAuthorToBook(bookId, authorId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

    }
}
