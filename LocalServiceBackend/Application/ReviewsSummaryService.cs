using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class ReviewsSummaryService : IReviewsSummaryService
    {
        public async Task<ReviewsSummary?> GetSummaryByProviderAsync(int providerId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var existingProvider = await providerRepo.GetByIdAsync(providerId);
            if (existingProvider == null)
                throw new Exception("Provider not found");

            ReviewsSummaryRepository summaryRepo   = new ReviewsSummaryRepository();
            return await summaryRepo.GetByProviderAsync(providerId);
        }

        public async Task<ReviewsSummary> GenerateOrUpdateSummaryAsync(int providerId)
        {
            ServiceProviderRepository providerRepo = new ServiceProviderRepository();
            var existingProvider = await providerRepo.GetByIdAsync(providerId);
            if (existingProvider == null)
                throw new Exception("Provider not found");

            ReviewRepository reviewRepo = new ReviewRepository();
            var reviews = await reviewRepo.GetByProviderAsync(providerId);

            if (reviews == null)
                throw new Exception("No reviews available to generate summary");

            // AI generates the summary
            AiSummaryService aiService = new AiSummaryService();
            var summaryText = await aiService.GenerateReviewSummaryAsync(reviews);

            ReviewsSummaryRepository summaryRepo = new ReviewsSummaryRepository();
            var existing = await summaryRepo.GetByProviderAsync(providerId);

            if (existing == null)
            {
                var newSummary = new ReviewsSummary
                {
                    ProviderId = providerId,
                    SummaryText = summaryText
                };
                await summaryRepo.CreateAsync(newSummary);
                return newSummary;
            }
            else
            {
                existing.SummaryText = summaryText;
                await summaryRepo.UpdateAsync(existing);
                return existing;
            }

        }

        public async Task DeleteSummaryAsync(int providerId)
        {
            ReviewsSummaryRepository summaryRepo = new ReviewsSummaryRepository();
            await summaryRepo.DeleteByProviderAsync(providerId);
        }
    }
}
