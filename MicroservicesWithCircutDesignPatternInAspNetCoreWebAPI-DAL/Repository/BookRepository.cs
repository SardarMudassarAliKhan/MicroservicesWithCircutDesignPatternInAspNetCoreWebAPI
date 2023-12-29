using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.AppDbContext;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.IRepository;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _dbContext.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _dbContext.Books.FindAsync(id);
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await _dbContext.Books.FindAsync(id);
            if(existingBook == null)
                return null;

            // Update existingBook properties with book
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.ISBN = book.ISBN;

            await _dbContext.SaveChangesAsync();
            return existingBook;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var bookToDelete = await _dbContext.Books.FindAsync(id);
            if(bookToDelete == null)
                return false;

            _dbContext.Books.Remove(bookToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
