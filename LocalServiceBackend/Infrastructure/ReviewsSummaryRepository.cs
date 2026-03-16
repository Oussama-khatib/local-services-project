using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Infrastructure
{
    public class ReviewsSummaryRepository
    {
        public async Task<ReviewsSummary?> GetByProviderAsync(int providerId)
        {
            using (var context= new AppDBContext())
            {
                return await context.ReviewsSummaries
            .FirstOrDefaultAsync(s => s.ProviderId == providerId);
            }
        }

        public async Task CreateAsync(ReviewsSummary summary)
        {
            using (var context = new AppDBContext())
            {
                await context.ReviewsSummaries.AddAsync(summary);
                await context.SaveChangesAsync();
            }

        }

        public async Task UpdateAsync(ReviewsSummary summary)
        {
            using (var context = new AppDBContext())
            {
                var existing = await context.ReviewsSummaries
            .FirstOrDefaultAsync(s => s.ProviderId == summary.ProviderId);
                if (existing == null)
                    return;
                existing.SummaryText = summary.SummaryText;

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteByProviderAsync(int providerId)
        {
            using (var context = new AppDBContext())
            {
                var summary = await context.ReviewsSummaries
            .FirstOrDefaultAsync(s => s.ProviderId == providerId);

                if (summary == null)
                    return;
                context.ReviewsSummaries.Remove(summary);
                await context.SaveChangesAsync();
            }
        }
    }
}
