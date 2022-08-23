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
        private IProductRepository _service;
        private IMapper _mapper;
        private ProductController _productController;


        [SetUp]
        public void Init()
        {
            var serviceMock = new Mock<IProductRepository>();
            var mapperMock = new Mock<IMapper>();

            serviceMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(new Product[] { });
            serviceMock.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());

            serviceMock.Setup(x => x.GetProductAsync(4)).ReturnsAsync(new Product() { AvailableUnits = 0 });
            serviceMock.Setup(x => x.GetProductAsync(16)).ReturnsAsync(new Product() { AvailableUnits = 0 });

            serviceMock.Setup(x => x.GetProductAsync(2)).ReturnsAsync(new Product() { AvailableUnits = 4 });
            serviceMock.Setup(x => x.GetProductAsync(5)).ReturnsAsync(new Product() { AvailableUnits = 30 });

            serviceMock.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(true));
           
            serviceMock.Setup(x => x.GetProductAsync("rera")).ReturnsAsync(new Product());

            _productController = new ProductController(serviceMock.Object, mapperMock.Object);

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
      /*  [Test]
        public async Task PUT_update_product_with_correct_input()
        {
            //Arrange
            ProductDto productDto = new ProductDto() 
            { Name="rera", 
             AvailableUnits=123,
             Description="put_update_product_with_correct_input",
             UnitPrice=123
            };

            //Act 
            var result = await _productController.UpdateProduct(productDto);

            //Assert
             Assert.That(result.Result.GetType().Equals());
        }*/

    }
}
