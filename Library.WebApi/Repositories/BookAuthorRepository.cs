
using Library.WebApi.Data;
using Library.WebApi.Models;

namespace Library.WebApi.Repositories
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly AppDbContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookAuthorRepository(AppDbContext context, IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task AddAuthorToBook(int bookId, int authorId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            var author = await _authorRepository.GetAuthorByIdAsync(authorId);

            var bookAuthor = new BookAuthor
            {
                BookId = bookId,
                AuthorId = authorId
            };

            book.BookAuthors.Add(bookAuthor);
            author.BookAuthors.Add(bookAuthor);

            await _context.SaveChangesAsync();
        }
    }
}
