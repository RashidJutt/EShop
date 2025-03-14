using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure
{
    public static class DatabaseSeeding
    {
        public static async Task SeedAsyc(OrderDbContext orderDbContext, ILogger<OrderDbContext> logger)
        {
            if (!orderDbContext.Orders.Any())
            {
                await orderDbContext.Orders.AddAsync(GetOrder());
                await orderDbContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderDbContext).Name} seeded!!!");
            }
        }

        private static Order GetOrder()
        {
            return new()
            {
                UserName = "rahul",
                FirstName = "Rahul",
                LastName = "Sahay",
                EmailAddress = "rahulsahay@eCommerce.net",
                Address = new Address
                {
                    Country = "India",
                    ZipCode = "560001",
                    Line = "Banglore",
                    State = "KA"
                },
                PaymentDetails = new PaymentDetails
                {
                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    Cvv = "123",
                    Expiration = "12/25",
                    PaymentMethod = 1,
                },
                CreatedBy = "Rahul",
                LastModifiedBy = "Rahul",
                TotalPrice = 100,
                LastModifiedDate = new DateTime(),
            };
        }
    }
}
