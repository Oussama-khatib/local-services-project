using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class ReviewRepository
    {
        public async Task InsertReviewAsync(Review review)
        {
            using (var context = new AppDBContext())
            {
                var newReview = new Review
                {
                    JobId = review.JobId,
                    ProviderId = review.ProviderId,
                    CustomerId = review.CustomerId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreatedAt = DateTime.Now,
                };
                await context.Reviews.AddAsync(newReview);
                context.SaveChanges();
            }
        }

        public async Task DeleteReviewAsync(int ReviewId)
        {
            using (var context = new AppDBContext())
            {
                var review = context.Reviews.FirstOrDefault(r => r.ReviewId == ReviewId);
                if (review != null)
                {
                    context.Reviews.Remove(review);
                    context.SaveChanges();
                }
            }
        }

        public async Task UpdateReviewAsync(Review Review)
        {
            using (var context = new AppDBContext())
            {
                var review = context.Reviews.FirstOrDefault(r => r.ReviewId == Review.ReviewId);
                if (review != null)
                {
                    review.JobId=Review.JobId;
                    review.ProviderId=Review.ProviderId;
                    review.CustomerId=Review.CustomerId;
                    review.Rating=Review.Rating;
                    review.Comment=Review.Comment;
                    context.SaveChanges();
                }
            }
        }

        public async Task<Review?> GetByJobAsync(int jobId)
        {
            using (var context = new AppDBContext())
            {
                var review = context.Reviews.FirstOrDefault(r=>r.JobId== jobId);
                return review;
            }
        }

        public async Task<IEnumerable<Review>> GetByProviderAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Reviews
            .Where(r => r.ProviderId == providerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
            }
        }

        public async Task<IEnumerable<Review>> GetByCustomerAsync(int customerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Reviews
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
            }
        }

        public async Task<double> GetAverageRatingForProviderAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                var hasReviews = await context.Reviews
            .AnyAsync(r => r.ProviderId == providerId);
                if (!hasReviews)
                    return 0;
                return await context.Reviews
            .Where(r => r.ProviderId == providerId)
            .AverageAsync(r => r.Rating);
            }
        }

        public async Task<int> GetTotalReviewsForProviderAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                return await context.Reviews
            .CountAsync(r => r.ProviderId == providerId);
            }
        }


    }
}
