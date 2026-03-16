using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trial;

namespace Api.Controllers
{
    public class ReviewResponse { public int ReviewId  { get; set; }  public int JobId { get; set; } public int ProviderId { get; set; }
    public int CustomerId { get; set; } public int Rating { get; set; } public string? Comment { get; set; }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddReview(
        int jobId,
        int providerId,
        int rating,
        string comment)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            CustomerService customerService= new CustomerService();
            var customer= await customerService.GetCustomerByUserIdAsync(userId);
            try
            {
                var review = await _service.AddReviewAsync(
                    jobId,
                    customer.CustomerId,
                    providerId,
                    rating,
                    comment);

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(int jobId)
        {
            var review = await _service.GetReviewByJobAsync(jobId);

            if (review == null)
                return NotFound();

            var result = new ReviewResponse
            {
                ReviewId = review.ReviewId,
                JobId = review.JobId,
                ProviderId = review.ProviderId,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment
            };

            return Ok(result);
        }

        [HttpGet("my-provider-reviews")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> GetMyProviderReviews()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            ServiceProviderService service = new ServiceProviderService();
            var provider= await service.GetProviderByUserIdAsync(userId);

            var reviews = await _service.GetReviewsByProviderAsync(provider.ProviderId);

            var result = reviews.Select(r => new ReviewResponse
            {
                ReviewId = r.ReviewId,
                JobId = r.JobId,
                ProviderId = r.ProviderId,
                CustomerId = r.CustomerId,
                Rating = r.Rating,
                Comment = r.Comment
            });

            return Ok(result);
        }

        [HttpGet("my-reviews")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyReviews()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            CustomerService customerService = new CustomerService();
            var customer = await customerService.GetCustomerByUserIdAsync(userId);

            var reviews = await _service.GetReviewsByCustomerAsync(customer.CustomerId);

            var result = reviews.Select(r => new ReviewResponse
            {
                ReviewId = r.ReviewId,
                JobId = r.JobId,
                ProviderId = r.ProviderId,
                CustomerId = r.CustomerId,
                Rating = r.Rating,
                Comment = r.Comment
            });

            return Ok(result);
        }

        [HttpPut("{reviewId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(int reviewId, string comment , int rating)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            CustomerService customerService = new CustomerService();
            var customer = await customerService.GetCustomerByUserIdAsync(userId);

            var review = new Review
            {
                Comment = comment,
                ReviewId = reviewId,
                Rating = rating,
                CustomerId=customer.CustomerId
            };

            try
            {
                var updated = await _service.UpdateReviewAsync(review);

                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{reviewId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var result = await _service.DeleteReviewAsync(reviewId);

            if (!result)
                return NotFound();

            return NoContent();
        }


    }
}
