using Microsoft.EntityFrameworkCore;
using SM_LLMServer.Models;

namespace SM_LLMServer.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(AppDbContext context)
        {
            // Check if data already exists
            if (await context.Reviews.AnyAsync())
            {
                return; // Data already seeded
            }

            // Seed sample reviews
            var reviews = new List<Review>
            {
                new Review
                {
                    ProductId = 1,
                    Content = "This product exceeded my expectations! The quality is outstanding and it works perfectly. Highly recommend!",
                    Rating = 5,
                    Author = "John Doe",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Review
                {
                    ProductId = 1,
                    Content = "Great value for money. The features are exactly what I needed and the customer service was excellent.",
                    Rating = 4,
                    Author = "Jane Smith",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Review
                {
                    ProductId = 1,
                    Content = "Good product overall, but shipping took longer than expected. The product itself is solid.",
                    Rating = 4,
                    Author = "Mike Johnson",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Review
                {
                    ProductId = 2,
                    Content = "Amazing quality! This is my second purchase and I'm still impressed. Fast delivery too.",
                    Rating = 5,
                    Author = "Sarah Wilson",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Review
                {
                    ProductId = 2,
                    Content = "The product is okay, but I expected more features for the price. It does the basic job well.",
                    Rating = 3,
                    Author = "Tom Brown",
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Review
                {
                    ProductId = 1,
                    Content = "Absolutely love this product! It's exactly what I was looking for and the quality is top-notch.",
                    Rating = 5,
                    Author = "Emily Davis",
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Review
                {
                    ProductId = 2,
                    Content = "Fast shipping and great packaging. The product works as advertised. Very satisfied!",
                    Rating = 4,
                    Author = "David Miller",
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                }
            };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }
    }
}
