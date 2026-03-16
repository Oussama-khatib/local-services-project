using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IReviewsSummaryService
    {
        Task<ReviewsSummary?> GetSummaryByProviderAsync(int providerId);
        Task DeleteSummaryAsync(int providerId);
        Task<ReviewsSummary> GenerateOrUpdateSummaryAsync(int providerId);
    }
}
