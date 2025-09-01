using Microsoft.EntityFrameworkCore;
using SM_LLMServer.Data;
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
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewSummary?> GetReviewSummary(int productId)
        {
            var summary = await _context.Summaries
                .Where(s => s.ProductId == productId && s.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
                
            return summary;
        }

        public async Task<List<Review>> GetReviews(int productId, int limit = 10)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Review> CreateReview(ReviewRequest request)
        {
            var review = new Review
            {
                ProductId = request.ProductId,
                Content = request.Content,
                Rating = request.Rating,
                Author = string.IsNullOrEmpty(request.Author) ? "Anonymous" : request.Author,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            
            return review;
        }

        public async Task StoreReviewSummary(int productId, string summary)
        {
            var now = DateTime.UtcNow;
            var expiresAt = now.AddDays(7);
            
            var existingSummary = await _context.Summaries
                .FirstOrDefaultAsync(s => s.ProductId == productId);
            
            if (existingSummary != null)
            {
                existingSummary.Summary = summary;
                existingSummary.UpdatedAt = now;
                existingSummary.ExpiresAt = expiresAt;
                existingSummary.GeneratedAt = now;
            }
            else
            {
                _context.Summaries.Add(new ReviewSummary
                {
                    ProductId = productId,
                    Summary = summary,
                    CreatedAt = now,
                    UpdatedAt = now,
                    ExpiresAt = expiresAt,
                    GeneratedAt = now
                });
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
