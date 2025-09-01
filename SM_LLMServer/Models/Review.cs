using System.ComponentModel.DataAnnotations;

namespace SM_LLMServer.Models
{
    public class Review
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public int Rating { get; set; }
        
        public string Author { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ReviewSummary
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string Summary { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
        
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class ReviewRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public int Rating { get; set; } = 5;
        
        public string Author { get; set; } = string.Empty;
    }

    public class SummarizeRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        public string Provider { get; set; } = "OpenAI";
    }
}
