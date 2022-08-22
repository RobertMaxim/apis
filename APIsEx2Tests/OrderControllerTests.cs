using APIsEx.Repositories;
using APIsEx2.Controllers;
using APIsEx2.DTOs;
using APIsEx2.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace APIsEx2Tests
{
    public class OrderControllerTests
    {

        private Mock<IOrderRepository> _mockRepo;
        private Mock<IMapper> _mapper;
        private OrderController _controller;


        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();

            _mockRepo.Setup(x => x.GetOrderAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(new Order());
            _mockRepo.Setup(x => x.GetAllOrdersAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new Order[] { });
            //_mockRepo.Setup(x => x.Delete(It.IsAny<Order>()));

            _controller = new OrderController(_mockRepo.Object, _mapper.Object);
        }


        [Test]
        [TestCase(4, false, false)]
        [TestCase(1, true, false)]
        [TestCase(5, true, true)]
        [TestCase(215, false, true)]
        public async Task Get_ShouldReturnStatusCode200_WhenOrderIsValid(int id, bool includeCustomers, bool includeProducts)
        {
            //Act
            var actualResult = await _controller.Get(id, includeCustomers, includeProducts);

            //Assert
            Assert.That(actualResult.Result.GetType() == typeof(OkObjectResult));
        }

        [Test]
        [TestCase(4, false, false)]
        [TestCase(4, true, false)]
        [TestCase(4, true, true)]
        [TestCase(4, false, true)]
        public async Task CheckIfOrderIsCanceled_ShouldReturn200_WhenOrderIdExists(int id, bool includeCustomers, bool includeProducts)
        {
            //Act
            var actualResult = await _controller.CheckIfOrderIsCanceled(id);

            //Assert
            Assert.That(actualResult.GetType() == typeof(OkObjectResult));

        }

        [Test]
        [TestCase(4, false, false)]
        [TestCase(4, true, false)]
        [TestCase(4, true, true)]
        [TestCase(4, false, true)]
        public async Task Get_ShouldReturn404NotFound_WhenOrderIsInvalid(int id, bool includeCustomers, bool includeProducts)
        {
            //Arrange
            _mockRepo.Setup(x => x.GetOrderAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()));

            //Act
            var actualResult = await _controller.Get(id, includeCustomers, includeProducts);

            //Assert
            Assert.That(actualResult.Result.GetType().Equals(typeof(NotFoundObjectResult)));
        }

        [Test]
        [TestCase(4, false, false)]
        [TestCase(4, true, false)]
        [TestCase(4, true, true)]
        [TestCase(4, false, true)]
        public async Task CheckIfOrderIsCanceled_ShouldReturn404_WhenOrderIdIsInvalid(int id, bool includeCustomers, bool includeProducts)
        {
            //Arrange
            _mockRepo.Setup(x => x.GetOrderAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()));

            //Act
            var actualResult = await _controller.CheckIfOrderIsCanceled(id);

            //Assert
            Assert.That(actualResult.GetType() == typeof(NotFoundObjectResult));
        }

        [Test]
        [TestCase(35)]
        public async Task GetNoOfOrders_ShouldReturn200Code_WhenClientIdIsValid(int id)
        {
            //Default Arrange

            //Act
            var actualResult = await _controller.GetNoOfOrders(id);

            //Assert
            Assert.That(actualResult.GetType() == typeof(OkObjectResult));
        }

        [Test]
        [TestCase(35)]
        public async Task GetNoOfOrders_ShouldReturn404Code_WhenClientIdIsInvalid(int id)
        {
            //Arrange
            _mockRepo.Setup(x => x.GetAllOrdersAsync(It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult((Order[])null));

            //Act
            var actualResult = await _controller.GetNoOfOrders(id);

            //Assert
            Assert.That(actualResult.GetType() == typeof(NotFoundObjectResult));
        }

        [Test]
        [TestCase(30)]
        public async Task DeleteOrder_ShouldReturn404Code_WhenClientIdIsInvalid(int id)
        {
            //Arrange
            _mockRepo.Setup(x => x.GetOrderAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult((Order)null));

            //Act
            var actualResult = await _controller.DeleteOrder(id);

            //Assert
            Assert.That(actualResult.GetType() == typeof(NotFoundObjectResult));
        }

        [Test]
        [TestCase(30)]
        public async Task DeleteOrder_ShouldReturn201Code_WhenClientIdIsValid(int id)
        {
            //Default arrange +
            _mockRepo.Setup(x=>x.SaveChangesAsync()).Returns(Task.FromResult(true));

            //Act
            var actualResult = await _controller.DeleteOrder(id);

            //Assert
            Assert.That(actualResult.GetType() == typeof(AcceptedResult));
        }

        [Test]
        [TestCase(35)]
        public async Task DeleteOrder_ShouldReturn400Code_WhenChangesNotSaved(int id)
        {
            //Default arrange

            //Act
            var actualResult = await _controller.DeleteOrder(id);

            //Assert
            Assert.That(actualResult.GetType().Equals(typeof(BadRequestObjectResult)));
        }
    }
}