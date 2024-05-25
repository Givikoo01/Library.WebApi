using Library.WebApi.Data;
using Library.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.WebApi.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var authors = await _context.Authors
                .Include(ba => ba.BookAuthors)
                .ThenInclude(b => b.Book)
                .ToListAsync();
            if (!authors.Any())
            {
                throw new Exception("No authors found!");
            }
            return authors;
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors
                .Include(ba => ba.BookAuthors)
                .ThenInclude(b => b.Book)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (author is null)
            {
                throw new Exception($"No author with id {id} was found!");
            }
            return author;
        }

        public async Task AddAuthorAsync(Author author)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            var existingAuthor = await _context.Authors.Include(ba => ba.BookAuthors).SingleOrDefaultAsync(a => a.Id == author.Id);
            
            if (existingAuthor is null)
            {
                throw new Exception($"No author with id {author.Id} was found!");
            }

            existingAuthor.FirstName = author.FirstName;
            existingAuthor.LastName = author.LastName;
            existingAuthor.YearOfBirth = author.YearOfBirth;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author is null)
            {
                throw new Exception($"No author with id {id} was found!");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}
