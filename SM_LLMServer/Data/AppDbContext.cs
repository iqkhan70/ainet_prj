using Microsoft.EntityFrameworkCore;
using SM_LLMServer.Models;

namespace SM_LLMServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewSummary> Summaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Author).HasMaxLength(100);
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                
                // Index for faster queries
                entity.HasIndex(e => e.ProductId);
                entity.HasIndex(e => e.CreatedAt);
            });

            // Configure ReviewSummary entity
            modelBuilder.Entity<ReviewSummary>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Summary).IsRequired().HasMaxLength(5000);
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.ExpiresAt).IsRequired();
                
                // Index for faster queries
                entity.HasIndex(e => e.ProductId);
                entity.HasIndex(e => e.ExpiresAt);
            });
        }
    }
}
