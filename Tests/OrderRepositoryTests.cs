using Libraries;
using Libraries.Entities;
using Libraries.Repositories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.Json;

namespace Tests
{
    [TestFixture]
    // TODO: make tests isolated!
    public class OrderRepositoryTests
    {
        private const string ConnectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";

        private IExtendedOrderRepository _orderRepository;
        private Order _order;

        [OneTimeSetUp]
        public void Setup()
        {
            var context = new ShopDBContext(ConnectionString);
            _orderRepository = new OrderRepository(context);
            _order = new Order
            {
                CreatedDate = new DateTime(2022, 7, 7),
                UpdatedDate = new DateTime(2022, 7, 7) + TimeSpan.FromHours(2),
                ProductId = 1,
                Status = OrderStatus.Arrived,
            };
        }

        [Test]
        [Order(1)]
        public void Create_Order_InsertsOrderIntoDb()
        {
            var expected = _order;
            _orderRepository.Create(_order);
            var actual = _orderRepository.GetAll().Last();
            expected.Id = actual.Id;
            Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        [Test]
        [Order(2)]
        public void Read_ValidId_ReturnsOrder()
        {
            var expectedOrder = _order;
            expectedOrder.Id = _orderRepository.GetAll().Last().Id;
            var actual = _orderRepository.Read(expectedOrder.Id);

            Assert.AreEqual(JsonSerializer.Serialize(expectedOrder), JsonSerializer.Serialize(actual));
        }

        [Test]
        [Order(3)]
        public void Read_NotValidId_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _orderRepository.Read(10));
        }

        [Test]
        [Order(4)]
        public void Update_Order_UpdatesOrderIntoDb()
        {
            var expectedOrder = _order;
            expectedOrder.Id = _orderRepository.GetAll().Last().Id;
            expectedOrder.UpdatedDate = _order.UpdatedDate + TimeSpan.FromHours(4);

            _orderRepository.Update(expectedOrder);
            var actual = _orderRepository.Read(expectedOrder.Id);

            Assert.AreEqual(JsonSerializer.Serialize(expectedOrder), JsonSerializer.Serialize(actual));
        }

        [Test]
        [Order(5)]
        public void Delete_ValidId_DeletesOrderFromDb()
        {
            var order = _orderRepository.GetAll().Last();

            _orderRepository.Delete(order);

            Assert.Throws<InvalidOperationException>(() => _orderRepository.Read(order.Id));
        }
    }
}