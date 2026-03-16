using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public interface IAiSummaryService
    {
        Task<string?> GenerateReviewSummaryAsync(IEnumerable<Review> reviews);

    }
}
