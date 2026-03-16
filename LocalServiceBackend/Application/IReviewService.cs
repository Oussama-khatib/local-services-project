using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IReviewService
    {
        Task<Review> AddReviewAsync(int jobId, int customerId, int providerId, int rating, string comment);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<Review?> UpdateReviewAsync(Review review);
        Task<Review?> GetReviewByJobAsync(int jobId);
        Task<IEnumerable<Review>> GetReviewsByProviderAsync(int providerId);
        Task<IEnumerable<Review>> GetReviewsByCustomerAsync(int customerId);
    }
}
