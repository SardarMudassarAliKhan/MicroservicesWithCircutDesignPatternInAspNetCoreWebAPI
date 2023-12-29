using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_BAL.IService;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.IRepository;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_BAL.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30), OnBreak, OnReset, OnHalfOpen);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("Executing GetAllBooksAsync method.");
                    return await _bookRepository.GetAllBooksAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in GetAllBooksAsync method.");
                    throw; // Re-throw the exception to propagate it up the call stack
                }
            });
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    _logger.LogInformation($"Executing GetBookByIdAsync method for book with ID: {id}.");
                    return await _bookRepository.GetBookByIdAsync(id);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred in GetBookByIdAsync method for book with ID: {id}.");
                    throw; // Re-throw the exception to propagate it up the call stack
                }
            });
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    _logger.LogInformation("Executing AddBookAsync method.");
                    return await _bookRepository.AddBookAsync(book);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in AddBookAsync method.");
                    throw; // Re-throw the exception to propagate it up the call stack
                }
            });
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    _logger.LogInformation($"Executing UpdateBookAsync method for book with ID: {id}.");
                    return await _bookRepository.UpdateBookAsync(id, book);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred in UpdateBookAsync method for book with ID: {id}.");
                    throw; // Re-throw the exception to propagate it up the call stack
                }
            });
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    _logger.LogInformation($"Executing DeleteBookAsync method for book with ID: {id}.");
                    return await _bookRepository.DeleteBookAsync(id);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred in DeleteBookAsync method for book with ID: {id}.");
                    throw; // Re-throw the exception to propagate it up the call stack
                }
            });
        }

        private void OnBreak(Exception exception, TimeSpan duration)
        {
            _logger.LogError(exception, $"Circuit is open for {duration.TotalSeconds} seconds due to {exception.Message}");
        }

        private void OnReset()
        {
            _logger.LogInformation("Circuit is reset");
        }

        private void OnHalfOpen()
        {
            _logger.LogInformation("Circuit is half-open");
        }
    }
}
