
using DataShakeApiLocobuzz.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataShakeApiLocobuzz
{
    class LogicDataShake
    {
        private IConfiguration config;
        public readonly ILogger logger;
        public LogicDataShake(IConfiguration _config, ILogger _logger)
        {
            config = _config;
            logger = _logger;
        }

        Logging log = new Logging();
        DataShakeWrapper wrapper = new DataShakeWrapper();
        Dictionary<int, string> URLS= new Dictionary<int, string>();

        public async Task<LocobuzzResponse> BulkUrl()
        {
            LocobuzzResponse response;
            try
            {
                Console.WriteLine("In Logic");
                LocobuzzResponse resultWrapper = wrapper.GetAllUrls(this.config).Result;
                Console.WriteLine(resultWrapper.Data);
                List<Review> reviews = new List<Review>(); 
                string message = "";
                if (resultWrapper != null && resultWrapper.Success)
                {
                    List<wrapperUrl> urls = (List<wrapperUrl>)resultWrapper.Data;
                    
                    var urlsGroupedById = urls.GroupBy(url => url.brandId);
                    var group1 = urlsGroupedById.First();
                    int count = 0;
                    bool check = true;
                    List<string> urlList = new List<string>();
                    List<string> urlFailed = new List<string>();

                    

                    foreach (var urlItem in urlsGroupedById)
                    {
                        foreach(var url in urlItem)
                        {
                            count++;
                            urlList.Add(url.url);
                            if (count == 10)
                            {
                                Console.WriteLine(url);
                                LocobuzzResponse result = new(false, null, null);
                                Thread thread = new Thread(() => { result = BulkUrl1(urlList).Result; });
                                thread.Start();
                                List<object> data = (List<object>)result.Data;
                                List<string> urlAdded = (List<string>)data[0];
                                List<string> urlNotAdded = (List<string>)data[1];
                                if (result != null && result.Success == false)
                                {
                                    response = new(true, "Reviews fetched.", (List<Review>)result.Data);
                                }
                                else
                                {
                                    message = result.Message;
                                    urlFailed = (List<string>)result.Data;
                                    check = false;
                                }
                                Console.Write("Reviews fetched for URLS:\n");
                                foreach(var item in urlAdded)
                                {
                                    Console.WriteLine(item);
                                }
                                Console.Write("Reviews not fetched for URLS:\n");
                                foreach (var item in urlNotAdded)
                                {
                                    Console.WriteLine(item);
                                }
                                urlList.Clear();
                                count = 0;
                            }
                        }
                        if (count > 0)
                        {
                            Console.WriteLine(urlList);
                            LocobuzzResponse result = new(true, "", "");
                            Thread thread = new Thread(() => { result = BulkUrl1(urlList).Result; });
                            thread.Start();
                            
                            List<object> data = (List<object>)result.Data;
                            List<string> urlAdded = (List<string>)data[0];
                            List<string> urlNotAdded = (List<string>)data[1];

                            if (result != null && result.Success == false)
                            {
                                reviews = (List<Review>)result.Data;
                          //      Console.WriteLine("Reviews fetched ")
                                response = new(true, "Reviews fetched.", (List<Review>)result.Data);
                            }
                            else
                            {
                                message = result.Message;
                                urlFailed = (List<string>)result.Data;
                                check = false;
                            }
                            Console.Write("Reviews fetched for URLS:\n");
                            foreach (var item in urlAdded)
                            {
                                Console.WriteLine(item);
                            }
                            Console.Write("Reviews not fetched for URLS:\n");
                            foreach (var item in urlNotAdded)
                            {
                                Console.WriteLine(item);
                            }
                        }
                    }

                    if (!check) response = new(false, null, message);
                    else response = new(true, "", reviews);
                } 
                else
                {
                    response = new(false, null, message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;
        }


        public async Task<LocobuzzResponse> BulkUrlWrapper(List<string> urls)
        {
            LocobuzzResponse response;
            try
            {
                LocobuzzResponse result = BulkUrl1(urls).Result;
                /*
                List<object> data = (List<object>)result.Data;
                List<string> urlAdded = (List<string>)data[0];
                List<string> urlNotAdded = (List<string>)data[1];
                bool check = true;
                string message = "";
                List<Review> reviews = new List<Review>();

                if (result != null && result.Success == false)
                {
                    reviews = (List<Review>)result.Data;
                    //      Console.WriteLine("Reviews fetched ")
                    
                }
                else
                {
                    message = result.Message;
                    urlNotAdded = (List<string>)result.Data;
                    check = false;
                }
                Console.Write("Reviews fetched for URLS:\n");
                foreach (var item in urlAdded)
                {
                    Console.WriteLine(item);
                }
                Console.Write("Reviews not fetched for URLS:\n");
                foreach (var item in urlNotAdded)
                {
                    Console.WriteLine(item);
                }
                */
            //    response = new(true, "Reviews fetched.", (List<Review>)result.Data);
                response = new(true, "Reviews fetched.", null);
            }
            catch(Exception ex)
            {
                response = new(false, ex.Message, null);
            }
            return response;
            
        }

        public async Task<LocobuzzResponse> BulkUrl1 (List<string> urls)
        {
            LocobuzzResponse response;

            List<string> urlAdded = new List<string>();
            List<string> urlFailed = new List<string>();
            List<object> dataResponse = new List<object>();

            try
            {
          //      BulkUrl url = new BulkUrl();
                List<BulkUrl> bulkUrl = new List<BulkUrl>();
                string message = "";
                foreach(string item in urls)
                {
                    BulkUrl url1 = new BulkUrl();
                    url1.Url = item;
                    bulkUrl.Add(url1);
                }


                List<BulkUrlBlock> bulkUrlBlock = new List<BulkUrlBlock>();
                string messageBlock = "";
                foreach (string item in urls)
                {
                    BulkUrlBlock url1 = new BulkUrlBlock();
                    url1.Url = item;
                    url1.blocks = 2;
                    bulkUrlBlock.Add(url1);
                }


                List<Review> reviewsArray = new List<Review>();
                bool check = true;
                
                LocobuzzResponse result = AddProfileBulk(bulkUrlBlock).Result;
                Console.WriteLine("Profiles added in bulk");
                List<object> data = (List<object>)result.Data;
                if (result != null && result.Success == true)
                {
                    
                    List<int> job_Ids = (List<int>)data[0];
                    List<string> UrlSuccess = (List<string>)data[1];
                    SemaphoreSlim semaphoreSlim = new SemaphoreSlim(8);
                    //         int jobId = (int)result.Data;
                    int count = 0;
                    Console.WriteLine("UrlSuccess count: " + UrlSuccess.Count);
                    foreach(var jobid in job_Ids)
                    {
                        semaphoreSlim.Wait();
                        Console.WriteLine("Count: " + count);
                        Console.WriteLine("Job id fetched: " + job_Ids[count]);
                        Console.WriteLine("Url: " + UrlSuccess[count]);
                        int jobId = job_Ids[count];
                        string url = UrlSuccess[count];
                        Console.WriteLine("Calling Reviews method for jobid: " + job_Ids[count]);
                        LocobuzzResponse result1 = new(true, "", "");
                        Thread t = new Thread(() => { ReviewsWrapper(jobId, semaphoreSlim, url); });
                        t.Start();
                        count++;
                    }

                    /*
                    for (int i=0;i< job_Ids.Count;i++) 
                    {
                     //   URLS.Add(job_Ids[i], UrlSuccess[i]);
                        semaphoreSlim.Wait();

                        Console.WriteLine("Job id fetched: " + job_Ids[i]);
                        Console.WriteLine("Calling Reviews method for jobid: " + job_Ids[i]);
                        LocobuzzResponse result1 = new(true, "" ,"");
                        Thread t = new Thread(() => { ReviewsWrapper(job_Ids[i], semaphoreSlim, UrlSuccess[i]); } );
                        t.Start();
                    }
                    */
      //              urlFailed.AddRange((List<string>)data[1]);
      //              dataResponse.Add(urlAdded);
      //              dataResponse.Add(urlFailed);
                    if (!check) response = new(false, message, dataResponse);
                    else response = new(true, "All reviews fetched for the current thread.", null);
                }
                else
                {
                    foreach(var url in urls)
                    {
                        Console.WriteLine("Review could not be fetched for: "+ url);
                    }
     //               urlFailed = urls;
     //               urlFailed.AddRange((List<string>)data[1]);
     //              dataResponse.Add(urlAdded);
     //               dataResponse.Add(urlFailed);
                    Console.WriteLine("Error Occured " + (string)result.Message);
                    response = new(false, result.Message, dataResponse);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
     //           dataResponse.Add(urlAdded);
     //           dataResponse.Add(urlFailed);
                response = new(false, ex.Message, dataResponse);
            }
            return response;
        }

        public async Task<LocobuzzResponse> ReviewsWrapper(int jobId, SemaphoreSlim semaphoreSlim, string url)
        {
            LocobuzzResponse response;
            try
            {
                Console.WriteLine("In Reviews Wrapper.");
                LocobuzzResponse result = Reviews(jobId, semaphoreSlim).Result;

                List<Review> reviewsArray = (List<Review>)result.Data;
                //      LocobuzzResponse result1 = Reviews(jobId).Result;
                List<string> urlAdded = new List<string>();
                List<string> urlFailed = new List<string>();
                bool check = true;
                string message = "";

                if (result != null && result.Success == true)
                {
                    Console.WriteLine(" Reviews fetched for url: " + url);
                    urlAdded.Add(url);
                    List<Review> reviews = (List<Review>)result.Data;
                    string OutputPath = "D:\\locobuzz\\DataShakeApiLocobuzz\\Reviews.txt";
                    using (StreamWriter tw = new StreamWriter(OutputPath))
                    {
                        foreach (var item in reviews)
                        {
                            string item1 = JsonConvert.SerializeObject(item);
                            tw.WriteLine(item1);
                        }
                    }
                    response = new(true, "reviews added to file", reviews);
                }
                else
                {
                    Console.WriteLine("Reviews could not be fetched for url: " + url);  
                    urlFailed.Add(url);
                    check = false;
                    message = result.Message;
                    Console.WriteLine("Error Occured " + (string)result.Message);
                    response = new(true, "Not added", null);
                }
            }
            catch(Exception ex)
            {
                response = new(false, ex.Message, null);
            }
            semaphoreSlim.Release();
            return response;
        }

        public async Task<LocobuzzResponse> AddProfile(string url)
        {
            LocobuzzResponse response;
            try 
            {
                RestDataShake obj = new RestDataShake();
         //       this.logger.LogInformation("Adding profile for url - " + url);
                LocobuzzResponse result = obj.RestAddProfile(this.config, url).Result;
                Console.WriteLine(result.Success + result.Message + result.Data);
         //       this.logger.LogInformation(result.Success + result.Message + result.Data);
                if(result != null && result.Success == true)
                {
                    var data = (string)result.Data;
                    var deserializedResult = (JObject)JsonConvert.DeserializeObject(data);
                    string success = deserializedResult["success"].ToString().Trim();
                    if (success == "True")
                    {
                        int jobid = deserializedResult["job_id"].Value<int>();
                        response = new(true, "Profile Added", jobid);
                    }
                    else
                    {
                        response = new(false, "Profile not added", null);
                    }
                } 
                else
                {
                    response = new(false, "Profile not added", null);
                }
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> AddProfileBulk(List<BulkUrlBlock> url)
        {
            LocobuzzResponse response;
            List<string> urls = url.Select(url => url.Url).ToList();
            List<string> urlFailed = new List<string>();
            List<string> urlSuccess = new List<string>();

            try
            {
                RestDataShake obj = new RestDataShake();
                
                //       this.logger.LogInformation("Adding profile for url - " + url);
                LocobuzzResponse result = obj.RestAddProfileBulk(this.config, url).Result;
                List<BulkUrlResponse> bulkresponse = JsonConvert.DeserializeObject<List<BulkUrlResponse>>((string)result.Data);
                Console.WriteLine(result.Success + result.Message + result.Data);
                
                if (result != null && result.Success == true)
                {
                    var data = (string)result.Data;
                    List<BulkUrlResponse> deserializedResult = JsonConvert.DeserializeObject<List<BulkUrlResponse>>(data);
                    List<int> job_Ids = new List<int>();
                    bool check = true;
                    foreach(var Response in deserializedResult)
                    {
                        if (Response.success.ToString().Trim() == "True")
                        {
                            job_Ids.Add(Response.job_id);
                            urlSuccess.Add(Response.url);
                        }
                        else
                        {
                            urlFailed.Add(Response.url);
                            check = false;
                        }
                    }
                    List<object> dataResponse = new List<object>();
                    dataResponse.Add(job_Ids);
                    dataResponse.Add(urlSuccess);
                    dataResponse.Add(urlFailed);
                    
                    response = new(true, "Profiles Added.", dataResponse);
                }
                else
                {
                    List<object> dataResponse = new List<object>();
                    urlFailed = urls;
                    dataResponse.Add(urlSuccess);
                    dataResponse.Add(urlFailed);
                    response = new(false, "Profile not added", dataResponse);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                List<object> dataResponse = new List<object>();
                urlFailed = urls;
                dataResponse.Add(urlSuccess);
                dataResponse.Add(urlFailed);
                response = new(false, ex.Message, dataResponse);
            }
            return response;
        }

        public async Task<LocobuzzResponse> GetStatus(int jobId)
        {
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                LocobuzzResponse result = obj.RestGetProfile(this.config, jobId).Result;
                if (result != null && result.Success == true)
                {
                    Root profile = JsonConvert.DeserializeObject<Root>((string)result.Data);
                    response = new(true, "profile data", profile);
                }
                else
                {
                    response = new(false, null, null);
                }          
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> GetProfile(int jobId)
        {
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                LocobuzzResponse result = obj.RestGetProfile(this.config, jobId).Result;
                string data = (string)result.Data;
                ProfileData profileData = JsonConvert.DeserializeObject<ProfileData>(data); 
                if (result != null && result.Success == true)
                {
                    Root profile = JsonConvert.DeserializeObject<Root>((string)result.Data);
                    response = new(true, "Profile data", profileData);
                }
                else
                {
                    response = new(false, null, null);
                }
            }
            catch(Exception ex)
            {
              //  this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> GetReviews(int jobId, int max_no_of_pages)
        {
            Console.WriteLine("In Get reviews");
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                LocobuzzResponse response1;
                string message = "";
                List<Review> reviews = new List<Review>();
                bool check = false;
                for (int i=1;i<=max_no_of_pages; i++)
                {
                    LocobuzzResponse result = obj.RestGetReviews(this.config, jobId, i).Result;
                    string data = (string)result.Data;
                    Console.WriteLine("Retrieved data:\n" + data);
                    if (result != null && result.Success == true)
                    {
                        Root profileInfo = JsonConvert.DeserializeObject<Root>(data);
                        reviews.AddRange(profileInfo.reviews);                        
                    }
                    else
                    {
                        message = result.Message;
                        check = true;
                        break;
                    }
                }
                if (!check) response = new(true, "Reviews fetched.", reviews);
                else response = new(false, message, null);
            }
            catch(Exception ex)
            {
           //     this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, ex.Message, null);
            }
            return response;
        }

        public async Task<LocobuzzResponse> CheckStatus(int jobId)
        {
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                LocobuzzResponse result = await GetProfile(jobId);
                if (result != null && result.Success == true)
                {
                    ProfileData profile = (ProfileData)result.Data;
                    string status = profile.crawl_status;
                    Console.WriteLine(status);
                    if (status == "pending")
                    {
                        Console.WriteLine("intervalPending");
                        int intervalPending = config.GetSection("Interval").GetValue<int>("IntervalPending");
                        Thread.Sleep(intervalPending);
                        LocobuzzResponse result1 = CheckStatus(jobId).Result;
                        if (result1 != null && result1.Success == true)
                        {
                            int val = (int)result1.Data;
                            if (val != -1)
                            {
                                response = new(true, "Crawling complete", (int)result1.Data);
                            }
                            else
                            {
                                response = new(false, "Error occured.", -1);
                            }
                        }
                        else
                        {
                            response = new(false, "Error occured.", -1);
                        }
                    }
                    else
                    {
                        float percentage_complete = (float)profile.percentage_complete;
                        if (percentage_complete == 100.0f)
                        {
                            Console.WriteLine("Crawling completed.");
                            Console.WriteLine(profile.review_count);
                            response = new(true, "Crawling completed", (int)profile.review_count); // "Crawling has completed.Now you can fetch reviews.";
                        }
                        else
                        {
                            Console.WriteLine("Checking percentage_completed now.");
                            int IntervalComplete = config.GetSection("Interval").GetValue<int>("IntervalComplete");
                            Console.WriteLine("intervalComplete");
                            Thread.Sleep(IntervalComplete);
                            LocobuzzResponse result1 = CheckStatus(jobId).Result;
                            if (result1 != null && result1.Success == true)
                            {
                                int val = (int)result1.Data;
                                if (val != -1)
                                {
                                    response = new(true, "Crawling complete", (int)result1.Data);
                                }
                                else
                                {
                                    response = new(false, null, -1);
                                }
                            }
                            else
                            {
                                response = new(false, null, -1);
                            }
                        }
                    }
                }
                else
                {
                    response = new(false, null, -1);
                }
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;
        }

        public async Task<LocobuzzResponse> Reviews(int jobId, SemaphoreSlim semaphoreSlim)
        {
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                Console.WriteLine("In Reviews.");
                LocobuzzResponse result = CheckStatus(jobId).Result;
                if (result != null && result.Success == true)
                {
                    Console.WriteLine(result.Data);
                    if ((int)result.Data != -1)
                    {
                        int reviewsCount = (int)result.Data;
                        int max_no_of_pages = (reviewsCount / 500);
                        if (reviewsCount % 500 > 0) max_no_of_pages += 1;
                        LocobuzzResponse review = GetReviews(jobId, max_no_of_pages).Result;
                        List<Review> reviews = (List<Review>)review.Data;
                        response = new(true, "List of Reviews", reviews);
                    }
                    else
                    {
                        response = new(false, null, null);
                    }
                }
                else
                {
                    response = new(false, null, null);
                }
              //  semaphoreSlim.Release();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                semaphoreSlim.Release();
                response = new(false, null, ex.Message);
            }
            return response;
        }

    }

    class Logging
    {
        public void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\n Log Entry: ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine(" :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("----------------------");
        }
        public void log(string logMessage)
        {
            using(StreamWriter w = File.AppendText("D:\\locobuzz\\DataShakeApiLocobuzz\\log.txt"))
            {
                Log(logMessage, w);
            }
        }
    }
}
