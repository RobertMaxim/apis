using APIsEx.Models;
using Microsoft.EntityFrameworkCore;

namespace APIsEx.Repositories
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(OnlineShopContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            return await _context.Products.ToArrayAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products.Where(product => product.ProductId == productId).FirstOrDefaultAsync();
        }
    }
}
