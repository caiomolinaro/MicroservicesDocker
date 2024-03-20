using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection GetConnectionPostgreSQL()
        {
            return new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (coupon is null)
            {
                return new Coupon
                {
                    ProductName = "No discount",
                    Amount = 0,
                    Description = "No Discount Description"
                };
            }
            return coupon;
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName!, Description = coupon.Description!, Amount = coupon.Amount! });

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName!, Description = coupon.Description!, Amount = coupon.Amount!, Id = coupon.Id! });

            if (affected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
            {
                return false;
            }
            return true;
        }
    }
}