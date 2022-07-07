using Libraries;
using Libraries.Entities;
using Libraries.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Tests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private const string ConnectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";
        private ProductRepository _productRepository;
        private Product _product;

        [OneTimeSetUp]
        public void Setup()
        {
            var context = new ShopDBContext(ConnectionString);
            _productRepository = new ProductRepository(context);
            _product = new Product
            {
                Name = "ProductName1",
                Description = "ProductDescription",
                Height = 100,
                Length = 200,
                Weight = 101,
                Width = 200
            };
        }

        [Test]
        [Order(1)]
        public void Create_Product_InsertsProductIntoDB()
        {
            var expected = _product;

            _productRepository.Create(_product);
            var actual = _productRepository.GetAllProducts().Last();
            expected.Id = actual.Id;

            Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        [Test]
        [Order(2)]
        public void Read_ValidId_ReturnsProduct()
        {
            var expectedProduct = _product;
            expectedProduct.Id = _productRepository.GetAllProducts().Last().Id;

            var actual = _productRepository.Read(expectedProduct.Id);

            Assert.AreEqual(JsonSerializer.Serialize(expectedProduct), JsonSerializer.Serialize(actual));
        }

        [Test]
        [Order(3)]
        public void Read_NotValidId_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _productRepository.Read(2));
        }

        [Test]
        [Order(4)]
        public void Update_Product_UpdateProductInDB()
        {
            var expectedProduct = _product;
            expectedProduct.Id = _productRepository.GetAllProducts().Last().Id;
            expectedProduct.Description = "ProductDescription";

            _productRepository.Update(expectedProduct);

            var actual = _productRepository.Read(expectedProduct.Id);

            Assert.AreEqual(JsonSerializer.Serialize(expectedProduct), JsonSerializer.Serialize(actual));
        }

        [Test]
        [Order(5)]
        public void Delete_ValidId_DeleteProductInDB()
        {
            var expectedProduct = _product;
            expectedProduct.Id = _productRepository.GetAllProducts().Last().Id;
            _productRepository.Delete(expectedProduct);

            Assert.Throws<InvalidOperationException>(() => _productRepository.Read(expectedProduct.Id));
        }

    }
}
