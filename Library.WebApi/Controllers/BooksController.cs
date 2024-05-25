using Microsoft.AspNetCore.Mvc;
using Library.WebApi.Models;
using Library.WebApi.Repositories;

namespace Library.WebApi.Controllers
{
    /// <summary>
    /// წიგნების კონტროლერი, რომელიც გამოიყენება წიგნების სიის სამართავად
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        /// <summary>
        /// კონსტრუქტორი, რომელიც აინჯექთებს წიგნების რეპოზიტორს
        /// </summary>
        /// <param name="bookRepository">წიგნების რეპოზიტორი</param>
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// გვაძლევს საშუალებას, წამოვიღოთ წიგნების სია
        /// </summary>
        /// <returns></returns>
        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _bookRepository.GetBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        /// <summary>
        /// გვაძლევს საშუალებას, წამოვიღოთ კონკრეტული წიგნი Id-ის მიხედვით
        /// </summary>
        /// <param name="id">იმ წიგნის Id, რომელსაც ვეძებთ </param>
        /// <returns></returns>
        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                return Ok(book);
            }
            catch(Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        /// <summary>
        ///  გვაძლევს საშუალებას, დავარედაქტიროთ კონკრეტული წიგნის ველები
        /// </summary>
        /// <param name="id">წიგნის Id</param>
        /// <param name="book">წიგნი</param>
        /// <returns></returns>
        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            try
            {
                book.Id = id;
                await _bookRepository.UpdateBookAsync(book);
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
        /// გვაძლევს საშუალებას, დავამატოთ ახალი წიგნი
        /// </summary>
        /// <param name="book">წიგნი</param>
        /// <returns></returns>
        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            try
            {
                await _bookRepository.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
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
        /// გვაძლევს საშუალებას, წავშალოთ კონკრეტული წიგნი მოცემული Id-ით
        /// </summary>
        /// <param name="id">იმ წიგნის Id, რომლის წაშლაც გვსურს</param>
        /// <returns></returns>
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        /// <summary>
        /// გვაძლევს საშუალებას, გავიტანოთ წიგნი
        /// </summary>
        /// <param name="id">იმ წიგნის Id, რომლის გატანაც გვსურს</param>
        /// <returns></returns>
        [HttpPost("{id}/checkout")]
        public async Task<IActionResult> CheckOutBook(int id)
        {
            try
            { 
                await _bookRepository.CheckOutBookAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// გვაძლევს საშუალებას, დავაბრუნოთ წიგნი
        /// </summary>
        /// <param name="id">იმ წიგნის Id, რომელსაც ვაბრუნებთ</param>
        /// <returns></returns>
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            try
            {
                await _bookRepository.ReturnBookAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
