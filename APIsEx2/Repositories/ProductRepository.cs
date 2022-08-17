using APIsEx2.Models;
using Microsoft.EntityFrameworkCore;

namespace APIsEx.Repositories
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(OnlineShopContext context) : base(context)
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
