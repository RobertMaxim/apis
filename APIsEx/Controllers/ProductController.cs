using APIsEx.Models;
using APIsEx.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIsEx.Controllers
{
    [ApiController]
    public class ProductController:ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ProductController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            this._mapper = mapper;
        }
    }
}
