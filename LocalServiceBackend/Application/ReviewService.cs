using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class ReviewService : IReviewService
    {
        public async Task<Review> AddReviewAsync(int jobId, int customerId, int providerId, int rating, string comment)
        {
            JobRepository jobRepo = new JobRepository();
            var job = await jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new Exception("Job not found");

            if (job.Status != "Completed")
                throw new Exception("Review allowed only after job completion");

            ReviewRepository reviewRepo = new ReviewRepository();
            var existingReview = await reviewRepo.GetByJobAsync(jobId);
            if (existingReview != null)
                throw new Exception("Review already exists for this job");

            CustomerRepository customerRepo = new CustomerRepository();
            var existingCustomer= await customerRepo.GetByIdAsync(customerId);
            if (existingCustomer == null)
                throw new Exception("Customer not found");

            ServiceProviderRepository servicesRepo = new ServiceProviderRepository();
            var existingProvider= await servicesRepo.GetByIdAsync(providerId);
            if (existingProvider == null)
                throw new Exception("Provider not found");

            if (rating < 1 || rating > 5)
                throw new Exception("Rating must be between 1 and 5");
            var review = new Review
            {
                JobId = jobId,
                CustomerId = customerId,
                ProviderId = providerId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };
            await reviewRepo.InsertReviewAsync(review);

            // Update provider trust score after review
            await UpdateProviderTrustScore(providerId);

            return review;

        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            ReviewRepository reviewRepo = new ReviewRepository();
            await reviewRepo.DeleteReviewAsync(reviewId);
            return true;
        }

        public async Task<Review?> UpdateReviewAsync(Review review)
        {
            ReviewRepository reviewRepo = new ReviewRepository();
            var existing = await reviewRepo.GetByJobAsync(review.JobId);
            if (existing == null) return null;

            if (review.Rating < 1 || review.Rating > 5)
                throw new Exception("Invalid rating");

            existing.Rating = review.Rating;
            existing.Comment = review.Comment;

            await reviewRepo.UpdateReviewAsync(existing);

            await UpdateProviderTrustScore(existing.ProviderId);

            return existing;
        }

        public async Task<Review?> GetReviewByJobAsync(int jobId)
        {
            ReviewRepository reviewRepo = new ReviewRepository();
            return await reviewRepo.GetByJobAsync(jobId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByProviderAsync(int providerId)
        {
            ReviewRepository reviewRepo = new ReviewRepository();
            return await reviewRepo.GetByProviderAsync(providerId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByCustomerAsync(int customerId)
        {
            ReviewRepository reviewRepo = new ReviewRepository();
            return await reviewRepo.GetByCustomerAsync(customerId);
        }

        private async Task UpdateProviderTrustScore(int providerId)
        {
            ReviewRepository reviewRepo = new ReviewRepository();
            double avgRating = await reviewRepo.GetAverageRatingForProviderAsync(providerId);
            int totalReviews = await reviewRepo.GetTotalReviewsForProviderAsync(providerId);

            int trustScore = (int)((avgRating / 5.0) * 100);

            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            await providerRepo.UpdateTrustScoreAsync(providerId, trustScore);
        }
    }
}
