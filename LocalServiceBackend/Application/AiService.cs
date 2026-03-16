using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Trial;

namespace Application
{

    public class AiService : IAiService
    {
        public class AIResponse 
        { 
            public string Type { get; set; } 
            public string Content { get; set; } 
            public string tool_name { get; set; }
            public Dictionary<string, object>? Arguments { get; set; }
            public string Message { get; set; }
        }
        
        private HttpClient _httpClient;
        private IServiceProviderService _providerService;
        private IServiceCategoryService _categoryService;
        private IJobService _jobService;
        private IReviewsSummaryService _reviewSummaryService;

        public AiService(HttpClient httpClient, IServiceProviderService providerService, IServiceCategoryService categoryService
            , IJobService jobService, IReviewsSummaryService reviewsSummaryService)
        {
            _httpClient = httpClient;
            _providerService = providerService;
            _categoryService = categoryService;
            _jobService = jobService;
            _reviewSummaryService = reviewsSummaryService;
        }

        public async Task<string> ProcessQuestionAsync(int userId,string role, string question)
        {
            var response = await _httpClient.PostAsJsonAsync(
            "https://unobstruently-leafiest-mary.ngrok-free.dev/chat",
            new { user_id = userId, role = role, message = question });

            if (!response.IsSuccessStatusCode) 
            { 
                var error = await response.Content.ReadAsStringAsync(); 
                throw new Exception($"API call failed: {response.StatusCode}, body: {error}"); 
            }

            var aiResponse = await response.Content.ReadFromJsonAsync<AIResponse>();
            Console.WriteLine("Ai Response: " + aiResponse);

            if (aiResponse.Type == "final_answer")
                return aiResponse.Content;

            if (aiResponse.Type == "tool_call")
            {
                var result = await ExecuteTool(aiResponse,role,userId);
                Console.WriteLine("Tool: " + aiResponse.tool_name + "Result: " + result + "Question: " + question);
                if (result == new { })
                    return "this question can not be answered";

                var finalResponse = await _httpClient.PostAsJsonAsync(
                    "https://unobstruently-leafiest-mary.ngrok-free.dev/chat/tool-response",
                    new { tool_name = aiResponse.tool_name, result=result, question =question });

                var final = await finalResponse.Content.ReadFromJsonAsync<AIResponse>();

                return final.Content;
            }

            return "Error";
        }

        private async Task<object> ExecuteTool(AIResponse toolCall, string role, int userId)
        {
            if (role == "Customer")
            {
                switch (toolCall.tool_name)
                {
                    case "get_providers_by_location":
                        var location = toolCall.Arguments["location"].ToString();

                        if (location == null)
                            return new { };

                        return await _providerService.GetProvidersByLocationAsync(location);

                    case "get_top_rated_providers":
                        var limit = (int)toolCall.Arguments["limit"];

                        if (limit == null)
                        {
                            return new { };
                        }

                        return await _providerService.GetTopRatedProvidersAsync(limit);

                    case "get_all_categories":
                        return await _categoryService.GetAllCategoriesAsync();

                    default:
                        return new { };
                }
            }
            if (role =="Service Provider")
            {
                switch (toolCall.tool_name  )
                {
                    case "get_top_rated_providers":
                        var limit = (int)toolCall.Arguments["limit"];

                        if (limit == null)
                        {
                            return new { };
                        }

                        return await _providerService.GetTopRatedProvidersAsync(limit);

                    case "get_all_categories":
                        return await _categoryService.GetAllCategoriesAsync();


                    case "get_my_summary":
                        var _provider = await _providerService.GetProviderByUserIdAsync(userId);
                        var summary= await _reviewSummaryService.GetSummaryByProviderAsync(_provider.ProviderId);
                        return new List<ReviewsSummary> { summary };

                    default:
                        return new { };

                }
            }
            return new { };
        }
    }
}
