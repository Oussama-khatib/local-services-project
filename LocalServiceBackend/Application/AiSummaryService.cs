using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Trial;

namespace Application
{
    public class AiSummaryService : IAiSummaryService
    {
        public async Task<string?> GenerateReviewSummaryAsync(IEnumerable<Review> reviews)
        {
            var request = reviews.ToList();
            var json = JsonConvert.SerializeObject(request);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://unobstruently-leafiest-mary.ngrok-free.dev/");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("summarize", content);
                //Console.WriteLine("Status: " + response.StatusCode);
                
                if (response.IsSuccessStatusCode) 
                {
                    string result = await response.Content.ReadAsStringAsync();
                    // Deserialize into a dynamic object
                    //var obj = JsonConvert.DeserializeObject<dynamic>(result); 
                    // Access the nested field
                    //string summary = obj.summary[0].generated_text.ToString(); 
                    //Console.WriteLine("Summary: " + summary);
                    string summary = result.Replace("{\"summary\":\"", "").Replace("\"}", "");
                    return summary;
                }
                else 
                {
                    string error = await response.Content.ReadAsStringAsync(); 
                    Console.WriteLine($"Error details: {error}");
                    return null; 
                }
            }



        }
    }
}
