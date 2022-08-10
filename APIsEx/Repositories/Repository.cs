using APIsEx.Models;
using Microsoft.EntityFrameworkCore;

namespace APIsEx.Repositories
{
    public class Repository : IRepository
    {
        protected readonly OnlineShopContext _context;
        protected readonly ILogger _logger;
        public Repository(OnlineShopContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
