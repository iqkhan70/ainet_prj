using SM_LLMServer.Models;

namespace SM_LLMServer.Services
{
    public interface IReviewRepository
    {
        Task<ReviewSummary?> GetReviewSummary(int productId);
        Task<List<Review>> GetReviews(int productId, int limit = 10);
        Task<Review> CreateReview(ReviewRequest request);
        Task StoreReviewSummary(int productId, string summary);
        Task<List<Review>> GetAllReviews();
    }

    public class ReviewRepository : IReviewRepository
    {
        private readonly List<Review> _reviews;
        private readonly List<ReviewSummary> _summaries;
        private int _nextReviewId = 1;
        private int _nextSummaryId = 1;

        public ReviewRepository()
        {
            // Initialize with some sample data
            _reviews = new List<Review>
            {
                new Review
                {
                    Id = _nextReviewId++,
                    ProductId = 1,
                    Content = "This product exceeded my expectations! The quality is outstanding and it works perfectly. Highly recommend!",
                    Rating = 5,
                    Author = "John Doe",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Review
                {
                    Id = _nextReviewId++,
                    ProductId = 1,
                    Content = "Great value for money. The features are exactly what I needed and the customer service was excellent.",
                    Rating = 4,
                    Author = "Jane Smith",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Review
                {
                    Id = _nextReviewId++,
                    ProductId = 1,
                    Content = "Good product overall, but shipping took longer than expected. The product itself is solid.",
                    Rating = 4,
                    Author = "Mike Johnson",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Review
                {
                    Id = _nextReviewId++,
                    ProductId = 2,
                    Content = "Amazing quality! This is my second purchase and I'm still impressed. Fast delivery too.",
                    Rating = 5,
                    Author = "Sarah Wilson",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Review
                {
                    Id = _nextReviewId++,
                    ProductId = 2,
                    Content = "The product is okay, but I expected more features for the price. It does the basic job well.",
                    Rating = 3,
                    Author = "Tom Brown",
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                }
            };

            _summaries = new List<ReviewSummary>();
        }

        public async Task<ReviewSummary?> GetReviewSummary(int productId)
        {
            await Task.Delay(10); // Simulate async operation
            return _summaries.FirstOrDefault(s => s.ProductId == productId);
        }

        public async Task<List<Review>> GetReviews(int productId, int limit = 10)
        {
            await Task.Delay(10); // Simulate async operation
            return _reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(limit)
                .ToList();
        }

        public async Task<Review> CreateReview(ReviewRequest request)
        {
            await Task.Delay(10); // Simulate async operation
            
            var review = new Review
            {
                Id = _nextReviewId++,
                ProductId = request.ProductId,
                Content = request.Content,
                Rating = request.Rating,
                Author = string.IsNullOrEmpty(request.Author) ? "Anonymous" : request.Author,
                CreatedAt = DateTime.UtcNow
            };

            _reviews.Add(review);
            return review;
        }

        public async Task StoreReviewSummary(int productId, string summary)
        {
            await Task.Delay(10); // Simulate async operation
            
            var existingSummary = _summaries.FirstOrDefault(s => s.ProductId == productId);
            
            if (existingSummary != null)
            {
                existingSummary.Summary = summary;
                existingSummary.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                _summaries.Add(new ReviewSummary
                {
                    Id = _nextSummaryId++,
                    ProductId = productId,
                    Summary = summary,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        public async Task<List<Review>> GetAllReviews()
        {
            await Task.Delay(10); // Simulate async operation
            return _reviews.OrderByDescending(r => r.CreatedAt).ToList();
        }
    }
}
