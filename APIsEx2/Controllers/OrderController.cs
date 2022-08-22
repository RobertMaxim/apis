using APIsEx.Repositories;
using APIsEx2.DTOs;
using APIsEx2.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIsEx2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route("Get")]
        [HttpGet]
        public async Task<ActionResult<OrderDto>> Get(int orderID, bool includeCustomer, bool includeProducts)
        {
            try
            {
                var order = await _repository.GetOrderAsync(orderID, includeCustomer, includeProducts);
                if (order == null) return NotFound($"Order with id {orderID} doesn't exist.");
                
                return Ok(_mapper.Map<OrderDto>(order));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
         }

        [Route("Status")]
        [HttpGet]
        public async Task<ActionResult> CheckIfOrderIsCanceled(int orderID)
        {
            try
            {
                var result = await _repository.GetOrderAsync(orderID);
                if (result == null) return NotFound($"OrderID ({orderID}) is invalid");

                if (result.Canceled) return Ok("Not canceled");

                return Ok("This order is canceled.");
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("TotalOrdersForClient")]
        [HttpGet]
        public async Task<ActionResult> GetNoOfOrders(int clientId)
        {
            try
            {
                var customerOrders = await _repository.GetAllOrdersAsync(clientId);
                if (customerOrders == null) return NotFound($"No orders for this client {clientId}");

                return Ok($"Customer with id {clientId} has {customerOrders.Count()} orders");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _repository.GetOrderAsync(orderId);
                if (order == null) return NotFound($"No order with this id({orderId})");

                _repository.Delete(order);
                if (await _repository.SaveChangesAsync())
                    return Accepted("Deleted succesfully");
                else 
                    return BadRequest("Db failure");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
