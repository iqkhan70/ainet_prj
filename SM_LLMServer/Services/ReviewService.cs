using SM_LLMServer.Models;

namespace SM_LLMServer.Services
{
    public interface IReviewService
    {
        Task<string> SummarizeReviews(int productId, string provider = "OpenAI");
        Task<Review> CreateReview(ReviewRequest request);
        Task<List<Review>> GetReviews(int productId, int limit = 10);
        Task<List<Review>> GetAllReviews();
    }

    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly LlmClient _llmClient;

        public ReviewService(IReviewRepository reviewRepository, LlmClient llmClient)
        {
            _reviewRepository = reviewRepository;
            _llmClient = llmClient;
        }

        public async Task<string> SummarizeReviews(int productId, string provider = "OpenAI")
        {
            // Check if we already have a summary
            var existingSummary = await _reviewRepository.GetReviewSummary(productId);
            if (existingSummary != null)
            {
                return existingSummary.Summary;
            }

            // Get reviews for the product
            var reviews = await _reviewRepository.GetReviews(productId, 10);
            if (!reviews.Any())
            {
                return "No reviews available for this product.";
            }

            // Join reviews content
            var joinedReviews = string.Join("\n\n", reviews.Select(r => r.Content));

            // Create prompt for summarization
            var prompt = $"Please provide a concise summary of the following product reviews. Focus on common themes, overall sentiment, and key points:\n\n{joinedReviews}";

            // Generate summary using LLM
            var llmRequest = new LlmRequest
            {
                Prompt = prompt,
                Provider = ParseProvider(provider)
            };

            var response = await _llmClient.GenerateTextAsync(llmRequest);
            
            if (response?.Text != null)
            {
                var summary = response.Text;
                
                // Store the summary for future use
                await _reviewRepository.StoreReviewSummary(productId, summary);
                
                return summary;
            }

            return "Unable to generate summary at this time.";
        }

        public async Task<Review> CreateReview(ReviewRequest request)
        {
            return await _reviewRepository.CreateReview(request);
        }

        public async Task<List<Review>> GetReviews(int productId, int limit = 10)
        {
            return await _reviewRepository.GetReviews(productId, limit);
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await _reviewRepository.GetAllReviews();
        }

        private AiProvider ParseProvider(string provider)
        {
            return provider.ToLower() switch
            {
                "openai" => AiProvider.OpenAI,
                "ollama" => AiProvider.Ollama,
                "customknowledge" => AiProvider.CustomKnowledge,
                "huggingface" => AiProvider.HuggingFace,
                _ => AiProvider.OpenAI
            };
        }
    }
}
