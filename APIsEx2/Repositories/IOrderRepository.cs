using APIsEx2.Models;

namespace APIsEx.Repositories
{
    public interface IOrderRepository:IRepository
    { 
        Task<int> GetOrderCountAsync(int clientID);
        Task<Order[]> GetAllOrdersAsync(int clientID, bool includeCustomer = false);
        Task<Order> GetOrderAsync(int orderID, bool includeCustomer = false);
    }
}