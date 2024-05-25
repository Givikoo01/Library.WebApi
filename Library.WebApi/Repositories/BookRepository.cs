using Library.WebApi.Data;
using Library.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.WebApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var books =  await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .ToListAsync();
            if (!books.Any())
            {
                throw new Exception("No books found!");
            }
            return books;
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book =  await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .SingleOrDefaultAsync(b => b.Id == id);
            if (book is null)
            {
                throw new Exception($"Book with ID {id} not found!");
            }

            return book;
        }

        public async Task AddBookAsync(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            var existingBook = await _context.Books.SingleOrDefaultAsync(b => b.Id == book.Id);

            if (existingBook is null)
            {
                throw new Exception($"Book with ID {book.Id} not found!");
            }

            existingBook.Name = book.Name;
            existingBook.Description = book.Description;
            existingBook.Image = book.Image;
            existingBook.Rating = book.Rating;
            existingBook.PublishingDate = book.PublishingDate;
            existingBook.IsCheckedOut = book.IsCheckedOut;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
            {
                throw new Exception($"Book with id {id} not found!");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task CheckOutBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
            {
                throw new ArgumentException($"Book with id {id} not found.");
            }

            if (book.IsCheckedOut)
            {
                throw new Exception($"Book with id {id} is already checked out.");
            }

            book.IsCheckedOut = true;
            await _context.SaveChangesAsync();
        }

        public async Task ReturnBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book is null)
            {
                throw new ArgumentException($"Book with id {id} not found.");
            }

            if (!book.IsCheckedOut)
            {
                throw new Exception($"Book with id {id} is not currently checked out.");
            }

            book.IsCheckedOut = false;
            await _context.SaveChangesAsync();
        }
    }
}
