using APIsEx.Models;

namespace APIsEx.Repositories
{
    public interface IProductRepository:IRepository
    {
        Task<Product> GetProductAsync(int productId);
        Task<Product[]> GetAllProductsAsync();
    }
}
