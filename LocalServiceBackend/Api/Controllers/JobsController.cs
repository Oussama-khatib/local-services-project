using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trial;

namespace Api.Controllers
{

    public class JobResponseDto { public int jobId  { get; set; } public int customerId { get; set; } public int categoryId { get; set; }
    public string description { get; set; } public string location { get; set; } public bool? isEmergency { get; set;}
    public string status { get; set; } public DateTime? createdAt { get; set; }  public DateTime? acceptedAt { get; set; }
    public DateTime? completedAt { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _service;

        public JobsController(IJobService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(
                int customerId,
                int categoryId,
                string description,
                string location,
                bool isEmergency,
                int providerId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var job = await _service.CreateJobAsync(customerId, categoryId, description, location, isEmergency);
                var service = new JobAssignmentService();
                var jobAssignment = await service.AssignProviderToJobAsync(job.JobId, providerId);
                return Ok("job and job assignment created ");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("provider/{jobId}")]
        public async Task<IActionResult> GetProviderByJobId(int jobId)
        {
            try
            {
                var assignmentService = new JobAssignmentService();
                var jobAssignment = await assignmentService.GetAssignmentByJobAsync(jobId);

                var providerService = new ServiceProviderService();
                var provider = await providerService.GetProviderByIdAsync(jobAssignment.ProviderId);

                var userService = new UserService();
                var user = await userService.GetUserByIdAsync(provider.UserId);

                return Ok(new
                {
                    providerId = provider.ProviderId,
                    userId = provider.UserId,
                    Bio = provider.Biography,
                    yearOfExperience = provider.YearOfExperience,
                    location = provider.Location,
                    name = user.FullName,
                    phoneNumber = user.PhoneNumber,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _service.GetJobByIdAsync(id);

            if (job == null)
                return NotFound();

            var response = new JobResponseDto
            {
                jobId = job.JobId,
                customerId= job.CustomerId,
                categoryId= job.ServiceCategoryId,
                description= job.Description,
                location= job.Location,
                isEmergency= job.IsEmergency,
                status=job.Status,
                createdAt=job.CreatedAt,acceptedAt=job.AcceptedAt,completedAt=job.CompletedAt
            };
            return Ok(response);
        }

        [HttpGet("my-jobs")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyJobs()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            CustomerService service= new CustomerService();
            var customer= await service.GetCustomerByUserIdAsync(userId);

            var jobs = await _service.GetJobsByCustomerAsync(customer.CustomerId);
            var result = jobs.Select(j => new JobResponseDto
            {
                jobId = j.JobId,
                customerId = j.CustomerId,
                categoryId = j.ServiceCategoryId,
                description = j.Description,
                location = j.Location,
                isEmergency = j.IsEmergency,
                status = j.Status,
                createdAt = j.CreatedAt,
                acceptedAt = j.AcceptedAt,
                completedAt = j.CompletedAt
            });
            return Ok(result);
        }

        [HttpGet("my-active-jobs")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyActiveJobs()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            CustomerService service = new CustomerService();
            var customer = await service.GetCustomerByUserIdAsync(userId);

            var jobs = await _service.GetActiveJobsByCustomerAsync(customer.CustomerId);

            var result = jobs.Select(j => new JobResponseDto
            {
                jobId = j.JobId,
                customerId = j.CustomerId,
                categoryId = j.ServiceCategoryId,
                description = j.Description,
                location = j.Location,
                isEmergency = j.IsEmergency,
                status = j.Status,
                createdAt = j.CreatedAt,
                acceptedAt = j.AcceptedAt,
                completedAt = j.CompletedAt
            });
            return Ok(result);
        }

        [HttpGet("open/{categoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOpenJobs(int categoryId)
        {
            var jobs = await _service.GetOpenJobsByCategoryAsync(categoryId);
            var result = jobs.Select(j => new JobResponseDto
            {
                jobId = j.JobId,
                customerId = j.CustomerId,
                categoryId = j.ServiceCategoryId,
                description = j.Description,
                location = j.Location,
                isEmergency = j.IsEmergency,
                status = j.Status,
                createdAt = j.CreatedAt,
                acceptedAt = j.AcceptedAt,
                completedAt = j.CompletedAt
            });

            return Ok(result);
        }

        [HttpGet("provider")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> GetProviderJobs()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            ServiceProviderService service = new ServiceProviderService();
            var provider = await service.GetProviderByUserIdAsync(userId);

            var jobs = await _service.GetProviderJobsAsync(provider.ProviderId);

            var result = jobs.Select(j => new JobResponseDto
            {
                jobId = j.JobId,
                customerId = j.CustomerId,
                categoryId = j.ServiceCategoryId,
                description = j.Description,
                location = j.Location,
                isEmergency = j.IsEmergency,
                status = j.Status,
                createdAt = j.CreatedAt,
                acceptedAt = j.AcceptedAt,
                completedAt = j.CompletedAt
            });

            return Ok(result);
        }

        [HttpPost("{jobId}/accept")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> AcceptJob(int jobId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            ServiceProviderService service = new ServiceProviderService();
            var provider = await service.GetProviderByUserIdAsync(userId);
            try
            {
                await _service.AcceptJobAsync(jobId, provider.ProviderId);

                return Ok("Job accepted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{jobId}/complete")]
        [Authorize(Roles = "Service Provider")]
        public async Task<IActionResult> CompleteJob(int jobId, decimal price)
        {
            try
            {
                await _service.CompleteJobAsync(jobId, price);

                return Ok("Job completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{jobId}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelJob(int jobId)
        {
            try
            {
                await _service.CancelJobAsync(jobId);

                return Ok("Job cancelled successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{jobId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete(int jobId)
        {
            try
            {
                var result = await _service.DeleteJobAsync(jobId);

                if (!result)
                    return NotFound();
                

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
