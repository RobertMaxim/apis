using APIsEx.DTOs;
using APIsEx2.Models;
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

        [Route("Get")]
        [HttpGet]
        public async Task<ActionResult> GetProducts(string? productsID)
        {
            try
            {
                
                if(string.IsNullOrEmpty(productsID))
                {
                    var results = await _repository.GetAllProductsAsync();

                    return Ok(_mapper.Map<ProductDto[]>(results));
                }

                var idsList = productsID.Split(',', StringSplitOptions.RemoveEmptyEntries);

                List<Product> products = new List<Product>();
                foreach (var id in idsList)
                {
                    var product = await _repository.GetProductAsync(int.Parse(id));
                    if (product == null)
                        continue;
                    products.Add(product);
                }

                if (products.Count > 0)
                    return Ok(_mapper.Map<List<ProductDto>>(products));

                return BadRequest("No products");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("Insert")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Post(List<ProductDto> productDto)
        {
            try
            {
                var allProducts = await _repository.GetAllProductsAsync();


                var products = _mapper.Map<List<Product>>(productDto);

                products = products.Where(p => allProducts.Contains(p) == false).ToList();

                foreach (var product in products)
                    _repository.Add(product);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/product/", products);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [Route("Update")]
        [HttpPut]
        public async Task<ActionResult<List<ProductDto>>> Put(List<ProductDto> products)
        {
            try
            {
                List<Product> toBeUpdated = new List<Product>();
                foreach (var item in products)
                {
                    var oldCamp = await _repository.GetProductAsync(item.Name);
                    if (oldCamp == null) return NotFound("One or more products name isn't valid. (Not found in db)");

                    toBeUpdated.Add(oldCamp);
                }

                foreach (var item in toBeUpdated)
                {
                    _mapper.Map(products.Where(p => p.Name.Equals(item.Name)).FirstOrDefault(), item);
                }

                if (await _repository.SaveChangesAsync())
                    return _mapper.Map<List<ProductDto>>(toBeUpdated);

                return BadRequest("No changes made.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [Route("Put")]
        [HttpPut]
        public async Task<ActionResult<ProductDto>> UpdateProduct(ProductDto product)
        {
            var existingProductWithId = await _repository.GetProductAsync(product.Name);
            if (existingProductWithId == null) return NotFound($"Couldn't find a product with specified ID {product.Name}");

            _mapper.Map(product, existingProductWithId);

            if (await _repository.SaveChangesAsync())
            {
                return _mapper.Map<ProductDto>(existingProductWithId);
            }
            else return BadRequest("Failed to update database");
        }

        [Route("Delete")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(string productsId)
        {
            try
            {
                var idsList = productsId.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (idsList.Length == 0)
                    return NotFound("0 products found");

                foreach (var productId in idsList)
                {
                    var product = await _repository.GetProductAsync(int.Parse(productId));

                    if (product != null) _repository.Delete(product);
                }

                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                else return BadRequest("Failed to update database");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Null found");
            }
        }

        [Route("Availability")]
        [HttpGet]
        public async Task<ActionResult> CheckAvailabilityOfProduct(int productID)
        {
            try
            {
                var product = await _repository.GetProductAsync(productID);
                if (product == null) return NotFound("No product having this ID");

                if (product.AvailableUnits > 0)
                    return Ok("Product is on stock");
                return BadRequest("DB failure");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Route("Stock")]
        [HttpGet]
        public async Task<ActionResult> CheckProductStock(int productID)
        {
            try
            {
                var product = await _repository.GetProductAsync(productID);
                if (product == null) return NotFound("This product doesn't exist in our store");

                if (product.AvailableUnits > 0)
                    return Ok(product.AvailableUnits + " units available in our stocks");
                return BadRequest("Not available in our stocks");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }
    }
}
