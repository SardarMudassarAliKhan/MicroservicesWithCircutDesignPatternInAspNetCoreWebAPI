using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Models;

namespace MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.IRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(int id, Book book);
        Task<bool> DeleteBookAsync(int id);
    }
}
