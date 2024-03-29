﻿using APIsEx2.Models;

namespace APIsEx.Repositories
{
    public interface IProductRepository:IRepository
    {
        Task<Product> GetProductAsync(int productId);
        Task<Product> GetProductAsync(string productName);
        Task<Product[]> GetAllProductsAsync();
    }
}
