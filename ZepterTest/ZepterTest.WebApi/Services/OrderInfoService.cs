using Microsoft.Data.SqlClient;
using ZepterTest.Common.DTO;
using ZepterTest.Common.Enums;

namespace ZepterTest.WebApi.Services
{
    public class OrderInfoService : IOrderInfoService
    {
        private readonly string _connectionString;

        public OrderInfoService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<OrderInfoDTO>> GetOrderInfoAsync()
        {
            var result = new List<OrderInfoDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT 
                                o.id AS OrderID,
                                o.PaymentMethod AS PaymentMethod,
                                c.Street AS Street,
                                c.City AS City,
                                c.PostCode AS PostCode,
                                SUM(p.price * op.Quantity) AS NetTotal
                            FROM 
                                Orders o
                            INNER JOIN 
                                Shops s ON o.ShopId = s.Id
                            INNER JOIN 
                                Clients c ON o.ClientInfoId = c.Id
                            INNER JOIN 
                                OrderProducts op ON o.Id = op.OrderId
                            INNER JOIN 
                                Products p ON op.ProductCode = p.ProductCode
                            WHERE 
                                s.Id % 2 = 1 -- Sklepy o nieparzystym ID
                            AND 
                                c.City LIKE '%u%' -- Miasta zawierające małą literę 'u'
                            GROUP BY 
                                o.id,
                                o.PaymentMethod,
                                c.Street,
                                c.PostCode,
                                c.City
                            ORDER BY 
                                o.id;";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new OrderInfoDTO
                            {
                                Id = reader.GetInt64(0),
                                PaymentMethod = (PaymentMethod)reader.GetInt32(1),
                                Street = reader.GetString(2),
                                City = reader.GetString(3),
                                PostCode = reader.GetString(4),
                                NetTotal = reader.GetDecimal(5)
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
