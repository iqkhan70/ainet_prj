using Microsoft.AspNetCore.Mvc;
using SM_LLMServer.Models;
using SM_LLMServer.Services;

namespace SM_LLMServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Review>>> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviews();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<List<Review>>> GetReviews(int productId, [FromQuery] int limit = 10)
        {
            try
            {
                var reviews = await _reviewService.GetReviews(productId, limit);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview([FromBody] ReviewRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var review = await _reviewService.CreateReview(request);
                return CreatedAtAction(nameof(GetReviews), new { productId = review.ProductId }, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{productId}/summarize")]
        public async Task<ActionResult<string>> SummarizeReviews(int productId, [FromQuery] string provider = "OpenAI")
        {
            try
            {
                var summary = await _reviewService.SummarizeReviews(productId, provider);
                return Ok(new { summary, provider });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("providers")]
        public ActionResult<string[]> GetProviders()
        {
            return Ok(new[] { "OpenAI", "Ollama", "CustomKnowledge", "HuggingFace" });
        }
    }
}
