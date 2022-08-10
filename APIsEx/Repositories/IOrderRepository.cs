using APIsEx.Models;

namespace APIsEx.Repositories
{
    public interface IOrderRepository
    { 
        Task<int> GetOrderCountAsync(int clientID,bool status = false);
        Task<Order[]> GetAllOrdersAsync(int clientID);
        Task<Order> GetOrderAsync(int orderID);
    }
}