using APIsEx.DTOs;
using APIsEx.Models;
using APIsEx.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APIsEx.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository _repository, IMapper mapper)
        {
            this._repository = _repository;
            this._mapper = mapper;
        }

        [Route("GetAllProducts")]
        [HttpGet]
        public async Task<ActionResult<Product>> GetFirstProducts()
        {
            try
            {
                var results = await _repository.GetAllProductsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("GetNumberOfProds")]
        [HttpGet]
        //functie

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product=await _repository.GetProductAsync(id);
                if (product == null) return NotFound($"Couldn't find a product with the ID: {id}");
                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Failed to get products");
            }
        }

        [Route("PostOneProduct")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Post(ProductDto productDto)
        {
            try
            {
                // Create a new Camp
                var allProducts = await _repository.GetAllProductsAsync();


                var product = _mapper.Map<Product>(productDto);

                if (!allProducts.Contains(product))
                {
                    _repository.Add(product);
                }
                else
                {
                    return BadRequest();
                }

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/product/", product);
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<ProductDto>> Put(int productID, ProductDto product)
        {
            try
            {
                var existingProductWithId = await _repository.GetProductAsync(productID);
                if (existingProductWithId == null) return NotFound($"Couldn't find a product with specified ID {productID}");

                _mapper.Map(product, existingProductWithId);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductDto>(existingProductWithId);
                }
                else return BadRequest("Failed to update database");
            }

            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}
