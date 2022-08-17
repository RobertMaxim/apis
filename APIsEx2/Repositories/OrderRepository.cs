using APIsEx2.DTOs;
using APIsEx2.Models;
using Microsoft.EntityFrameworkCore;

namespace APIsEx.Repositories
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(OnlineShopContext context) : base(context)
        {
        }

        public void Add(Order o)
        {

        }

        public async Task<int> GetOrderCountAsync(int clientID)
        {

            return await _context.Orders.Where(order => order.CustomerId == clientID).CountAsync();
        }

        public async Task<Order[]> GetAllOrdersAsync(int clientID, bool includeCustomer = false)
        {
            IQueryable<Order>query= _context.Orders;
            if (includeCustomer)
            {
                query = query.Include(t => t.Customer);
            }

            query = query.Where(o => o.CustomerId == clientID);

            return await query.ToArrayAsync();
        }

        public async Task<Order> GetOrderAsync(int orderID, bool includeCustomer = false, bool includeProducts=false)
        {
            IQueryable<Order> query = _context.Orders.Where(order=>order.OrderId==orderID);
            if (includeCustomer)
            {
                query = query.Include(t => t.Customer);
            }

            if(includeProducts)
            {
                query = query.Include(t => t.OrderProducts).ThenInclude(p=>p.Product);
            }

            return await query.FirstOrDefaultAsync();
        }

    }
}
