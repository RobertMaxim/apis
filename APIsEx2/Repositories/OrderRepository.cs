using APIsEx.Models;
using Microsoft.EntityFrameworkCore;

namespace APIsEx.Repositories
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(OnlineShopContext context) : base(context)
        {
        }

        public async Task<int> GetOrderCountAsync(int clientID, bool status = false)
        {
            return await _context.Orders.Where(order => order.CustomerId == clientID).CountAsync();
        }

        public async Task<Order[]> GetAllOrdersAsync(int clientID)
        {
            return await _context.Orders.Where(order => order.CustomerId == clientID).ToArrayAsync();
        }

        public async Task<Order> GetOrderAsync(int orderID)
        {
            return await _context.Orders.Where(order => order.OrderId == orderID).FirstOrDefaultAsync();
        }
    }
}
