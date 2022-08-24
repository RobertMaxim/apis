using APIsEx.Controllers;
using APIsEx.DTOs;
using APIsEx.Repositories;
using APIsEx2.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIsEx2Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _service;
        private Mock<IMapper> _mapper;
        private ProductController _productController;


        [SetUp]
        public void Init()
        {
            _service = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _service.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(new Product[] { });
            _service.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());

            _service.Setup(x => x.GetProductAsync(4)).ReturnsAsync(new Product() { AvailableUnits = 0 });
            _service.Setup(x => x.GetProductAsync(16)).ReturnsAsync(new Product() { AvailableUnits = 0 });

            _service.Setup(x => x.GetProductAsync(2)).ReturnsAsync(new Product() { AvailableUnits = 4 });
            _service.Setup(x => x.GetProductAsync(5)).ReturnsAsync(new Product() { AvailableUnits = 30 });

            _service.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(true));

            _service.Setup(x => x.GetProductAsync("rera")).ReturnsAsync(new Product());

            _service.Setup(repo => repo.Add(It.IsAny<Product>()));



            // _mapper.Setup(x => x.Map<List<Product>>(new List<ProductDto>())).Returns(new List<Product>())

            _productController = new ProductController(_service.Object, _mapper.Object);

        }


        [TestCase("2")]
        [TestCase("3")]
        [TestCase("5")]
        public async Task GETproducts_when_parameter_is_not_null(string productID)
        {

            //Arrange

            //Act
            var result = await _productController.GetProducts(productID);

            //Assert
            Assert.That(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestCase("2, 5 ,3 ")]
        public async Task GETproducts_when_parameter_is_a_list_of_strings(string productID)
        {
            //Arrange

            //Act
            var result = await _productController.GetProducts(productID);

            //Assert
            Assert.That(result.GetType().Equals(typeof(OkObjectResult)));
        }
        [TestCase(null)]
        public async Task GETproducts_when_parameter_is_null(string? productID)
        {
            //Arrange


            //Act
            var result = await _productController.GetProducts(productID);

            //Assert
            Assert.That(result.GetType() == typeof(OkObjectResult));
        }

        [TestCase("2")]
        [TestCase("5")]
        public async Task Check_Stock_Product_For_Available_Product(int productID)
        {

            //Arrange

            //Act
            var result = await _productController.CheckProductStock(productID);

            //Assert
            Assert.That(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestCase("4")]
        [TestCase("16")]
        public async Task Check_Stock_Product_For_Unavailable_Product(int productID)
        {

            //Arrange

            //Act
            var result = await _productController.CheckProductStock(productID);


            //Assert
            Assert.That(result.GetType().Equals(typeof(BadRequestObjectResult)));
        }


        [TestCase("4,5,2,3")]
        public async Task Delete_product_test(string ids)
        {
            //Arrange
            ProductDto productDto = new ProductDto()
            {
                Name = "rera",
                AvailableUnits = 123,
                Description = "put_update_product_with_correct_input",
                UnitPrice = 123
            };

            _service.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _service.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(true));

            //Act 
            var result = await _productController.DeleteProduct(ids);

            //Assert
            //Assert.That(result.Result.GetType().Equals());
            Assert.That(result.GetType() == typeof(AcceptedResult));
        }

        [Test]
        [TestCase("1,23,4")]
        public async Task Delete_ShouldReturn404_WhenProductIsInvalid(string ids)
        {
            //Arrange
            _service.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(false));
            _service.Setup(x => x.GetProductAsync(It.IsAny<int>())).Returns(Task.FromResult((Product)null)); ;

            //Act
            var actualResult = await _productController.DeleteProduct(ids);

            //Assert
            Assert.That(actualResult.GetType().Equals(typeof(NotFoundObjectResult)));
        }
        [Test]
        [TestCase("")]
        public async Task Delete_ShouldReturn400_WhenInputEmptyString(string ids)
        {
            //Arrange

            //Act
            var actualResult = await _productController.DeleteProduct(ids);

            //Assert
            Assert.That(actualResult.GetType().Equals(typeof(BadRequestObjectResult)));
        }


        [Test]
        public async Task POST_products_test_OK_Test()
        {
            //Arrange
            List<ProductDto> productsDto = new List<ProductDto>()
            {new ProductDto(){
                Name = "Crema de maini",
                AvailableUnits = 123,
                Description = "put_update_product_with_correct_input",
                UnitPrice = 123

            }  };

            List<Product> products = new List<Product>()
            {new Product(){
                Name = "Crema de maini",
                AvailableUnits = 123,
                Description = "put_update_product_with_correct_input",
                UnitPrice = 123

            }  };

            _service.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(true));
            _mapper.Setup(x => x.Map<List<Product>>(productsDto)).Returns(products) ;

            //Act
            var result = await _productController.Post(productsDto);


            //Assert
            Assert.That(result.Result.GetType() == typeof(CreatedResult));
        }
        [Test]
        public async Task POST_products_test_Bad_Result_Test()
        {
            //Arrange
            List<ProductDto> productsDto = new List<ProductDto>()
            { };

            List<Product> products = new List<Product>()
            { };

            _service.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(false));
            _mapper.Setup(x => x.Map<List<Product>>(productsDto)).Returns(products);

            //Act
            var result = await _productController.Post(productsDto);


            //Assert
            Assert.That(result.Result.GetType() == typeof(BadRequestResult));
        }

        [Test]
        public async Task Put_ShouldReturn404_WhenProductNotValid()
        {
            //Arrange
            List<ProductDto> products = new List<ProductDto>()
            {
                new ProductDto(){ Name = "rera" },
                new ProductDto(){Name="xzc"}
            };
            _service.Setup(x => x.GetProductAsync(products.Select(p => p.Name).FirstOrDefault())).Returns(Task.FromResult((Product)null));
            //Act
            var actualResult = await _productController.Put(products);

            //Assert
            Assert.That(actualResult.Result.GetType().Equals(typeof(NotFoundObjectResult)));

        }

        [Test]
        public async Task PUT_update_product_with_correct_input()
        {
            //Arrange

            List<ProductDto> products = new List<ProductDto>()
            {
                new ProductDto(){ Name = "rera", AvailableUnits=50,Description=" nice",UnitPrice=23 },
                new ProductDto(){Name= "Sos de rosii", AvailableUnits=50, Description=" nice",UnitPrice=23}
            };
            _service.Setup(x => x.GetProductAsync(It.IsAny<string>())).Returns(Task.FromResult(new Product()));
            //Act 
            var result = await _productController.Put(products);

            //Assert
            Assert.That(result.Result.GetType().Equals(typeof(OkObjectResult)));
        }

    }
}
