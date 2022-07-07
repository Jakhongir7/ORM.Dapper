using Dapper;
using Libraries.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraries.Repositories
{
    public class OrderRepository : IExtendedOrderRepository
    {
        private readonly ShopDBContext _context;

        public OrderRepository(ShopDBContext context)
        {
            _context = context;
        }

        public void Create(Order entity)
        {
            using var connection = _context.CreateConnection();

            var query = "INSERT INTO [dbo].[Order] (Status, CreatedDate, UpdatedDate, ProductId) " +
                        "VALUES (@Status, @CreatedDate, @UpdatedDate, @ProductId)";

            connection.Execute(query, entity);
        }

        public Order Read(int id)
        {
            using var connection = _context.CreateConnection();

            var query = "SELECT * FROM [dbo].[Order] WHERE Id = @Id";

            return connection.QueryFirst<Order>(query, new { Id = id });
        }

        public void Update(Order entity)
        {
            using var connection = _context.CreateConnection();

            var query = "UPDATE [dbo].[Order] " +
                        "SET Status = @Status, " +
                        "CreatedDate = @CreatedDate, " +
                        "UpdatedDate = @UpdatedDate " +
                        "WHERE Id = @Id";

            connection.Execute(query, entity);
        }

        public void Delete(Order entity)
        {
            using var connection = _context.CreateConnection();

            var query = "DELETE FROM [dbo].[Order] WHERE Id = @Id";

            connection.Execute(query, entity);
        }

        public void Delete(int? month = null, OrderStatus? status = null, int? year = null, int? productId = null)
        {
            using var connection = _context.CreateConnection();

            var query = "sp_DeleteOrders @Month, @Year, @Status, @ProductId";

            connection.Execute(query,
                new { Month = month, Year = year, Status = status, ProductId = productId });
        }

        public IEnumerable<Order> GetAll(int? month = null, OrderStatus? status = null, int? year = null,
            int? productId = null)
        {
            using var connection = _context.CreateConnection();

            var query = "sp_FetchOrders @Month, @Year, @Status, @ProductId";

            return connection.Query<Order>(query,
                new { Month = month, Year = year, Status = status, ProductId = productId });
        }
    }
}
