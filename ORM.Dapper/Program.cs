using Libraries;
using Libraries.Entities;
using Libraries.Repositories;
using System;

namespace ORM.Dapper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";
            var context = new ShopDBContext(connectionString);

            var productRepo = new ProductRepository(context);

            var products = productRepo.GetAllProducts();

            foreach (var product in products)
            {
                Console.WriteLine(product.Name);
            }

            var orderRepo = new OrderRepository(context);

            //var orderEntity = new Order()
            //{
            //    CreatedDate = DateTime.Now,
            //    ProductId = 5,
            //    Status = OrderStatus.Cancelled,
            //    UpdatedDate = DateTime.Now + TimeSpan.FromHours(2)
            //};
            //orderRepo.Create(orderEntity);

            var orders = orderRepo.GetAll(productId: 3, status: OrderStatus.Arrived);

            foreach (var order in orders)
            {
                Console.WriteLine(order.Id);
            }
        }
    }
}
