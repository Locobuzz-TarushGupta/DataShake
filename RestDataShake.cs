using DataShakeApiLocobuzz.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataShakeApiLocobuzz
{
    class RestDataShake
    {
        public async Task<LocobuzzResponse> RestAddProfile(IConfiguration config, string url)
        {
            LocobuzzResponse response;
            try
            {
                string AddProfileUrl = config.GetSection("Url").GetValue<string>("AddProfileUrl");
                string spidermanToken = config.GetSection("Token").GetValue<string>("SpidermanToken");
                var client = new RestSharp.RestClient(AddProfileUrl);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("spiderman-token", spidermanToken);
                request.AddParameter("url", url, ParameterType.RequestBody);

                var result = client.Post(request);
                Console.WriteLine(result.Content+"\n\n\n");

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Status code ok." + result.Content);
                    response = new(true, "Profile Added", result.Content);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Status code Unauthorised." + result.Content);
                    response = new(false, "Unauthorised", result.Content);
                }
                else
                {
                    Console.WriteLine("Status code Error." + result.Content);
                    response = new(false, "Profile not added", result.Content);
                }
            }
            catch(Exception ex)
            {
          //      logger.LogError(ex, "Adding Profile", null, new {});
                Console.WriteLine("Error - " + ex + "\n");
           //     logger.LogInformation("Status code error. In catch.");
                response = new(false, "Error message", ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> RestAddProfileBulk(IConfiguration config, List<BulkUrl> url)
        {
            LocobuzzResponse response;
            try
            {
                string AddProfileUrlBulk = config.GetSection("Url").GetValue<string>("AddProfileUrlBulk");
                string spidermanToken = config.GetSection("Token").GetValue<string>("SpidermanToken");
                string stringUrl = JsonConvert.SerializeObject(url);
                var client = new RestSharp.RestClient(AddProfileUrlBulk);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("spiderman-token", spidermanToken);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", stringUrl, ParameterType.RequestBody);

                var result = client.Post(request);
                Console.WriteLine(result.Content + "\n\n\n");

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Status code ok." + result.Content);
                    response = new(true, "Profile Added", result.Content);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Status code Unauthorised." + result.Content);
                    response = new(false, "Unauthorised", result.Content);
                }
                else
                {
                    Console.WriteLine("Status code Error." + result.Content);
                    response = new(false, "Profile not added", result.Content);
                }
            }
            catch (Exception ex)
            {
                //      logger.LogError(ex, "Adding Profile", null, new {});
                Console.WriteLine("Error - " + ex + "\n");
                //     logger.LogInformation("Status code error. In catch.");
                response = new(false, "Error message", ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> RestGetProfile(IConfiguration config, int jobid)
        {
            LocobuzzResponse response;
            try
            {
                string GetProfileUrl = config.GetSection("Url").GetValue<string>("GetProfileUrl");
                string spidermanToken = config.GetSection("Token").GetValue<string>("SpidermanToken");
                var client = new RestSharp.RestClient(GetProfileUrl);
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("spiderman-token", spidermanToken);
                request.AddParameter("job_id", jobid);

                var result = client.Execute(request);
                if(result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    response = new(true, null, result.Content);
                }
                else if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    response = new(false, "Unauthorised", result.Content);
                }
                else
                {
                    response = new(false, null, result.Content);
                }
            }
            catch(Exception ex) 
            {
          //      logger.LogError(ex, "Getting Profile", null, new { jobid});
                Console.WriteLine("Error - " + ex + "\n");
                response = new(false, null, ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> RestGetReviews(IConfiguration config, int jobid)
        {
            LocobuzzResponse response;
            try
            {
                string GetReviewUrl = config.GetSection("Url").GetValue<string>("GetReviewsUrl");
                string SpidermanToken = config.GetSection("Token").GetValue<string>("SpidermanToken");
                var client = new RestSharp.RestClient(GetReviewUrl);
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("spiderman-token", SpidermanToken);
                request.AddParameter("job_id", jobid);

                var result = client.Execute(request);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    response = new(true, null, result.Content);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    response = new(false, "Unauthorised", result.Content);
                }
                else
                {
                    response = new(false, null, result.Content);
                }
            }
            catch (Exception ex)
            {
      //          logger.LogError(ex, "Getting Reviews", null, new { jobid });
                Console.WriteLine("Error - " + ex + "\n");
                response = new(false, null, ex.Message);
            }
            return response;
        }
    }
}
