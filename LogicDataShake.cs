using com.sun.tools.javac.jvm;
using DataShakeApiLocobuzz.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public async Task<LocobuzzResponse> Logic()
        {
            /*
            LocobuzzResponse response;
            try
            {
                LocobuzzResponse result1 = Reviews(475274137).Result;
                Console.WriteLine("logic1");
                if (result1 != null && result1.Success == true)
                {
                    Review[] reviews = (Review[])result1.Data;
                    string OutputPath = "D:\\locobuzz\\DataShakeApiLocobuzz\\Reviews.txt";
                    using (StreamWriter tw = new StreamWriter(OutputPath))
                    {
                        foreach (var item in reviews)
                        {
                            string item1 = JsonConvert.SerializeObject(item);
                            tw.WriteLine(item1);
                        }
                    }
                    response = new(true, "Reviews", reviews);
                }
                else
                {
                    Console.WriteLine("Error Occured " + (string)result1.Message);
                    response = new(false, null, null);
                }
                return new(true, "", result1.Data);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;
            */



            
            LocobuzzResponse response;
            try
            {
                //LocobuzzResponse result;
                Console.WriteLine("In Logic");
                string filePath = @"D:\\locobuzz\\DataShakeApiLocobuzz\\Input.txt";
                string text = string.Empty;
                Console.WriteLine("Reading from file.");
                if (File.Exists(filePath))
                {
                    text = File.ReadAllText(filePath);
                    Console.WriteLine(text);
                }
                Console.WriteLine(text);
                List<string> urls = text.Split(',').ToList();
                foreach(string url in urls)
                {
                    Console.WriteLine(url);
                    LocobuzzResponse result = new(false, null, null);
                    Thread thread = new Thread(() => { result = Logic1(url).Result; });
                    thread.Start();
                    if(result != null && result.Success == false)
                    {
                        response = new(true, "Reviews fetched.", (List<Review>)result.Data);
                    }
                    else
                    {
                        response = new(false, null, null);
                    }
                }
                response = new(true, null, null);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Adding Profile", null, new { });
                Console.WriteLine("Error - " + ex + "\n");
                log.log(ex.Message);
                response = new(false, null, ex.Message);
            }
            return response;



        }

        public async Task<LocobuzzResponse> LogicBulk()
        {
            LocobuzzResponse response;
            try
            {
                //LocobuzzResponse result;
                Console.WriteLine("In Logic");
                string filePath = @"D:\\locobuzz\\DataShakeApiLocobuzz\\Input.txt";
                string text = string.Empty;
                Console.WriteLine("Reading from file.");
                if (File.Exists(filePath))
                {
                    text = File.ReadAllText(filePath);
                    Console.WriteLine(text);
                }
                Console.WriteLine(text);
                List<string> urls = text.Split(',').ToList();
                List<string> urlList = new List<string>();
                List<BulkUrl> bulkurl = new List<BulkUrl>();
                int count = 0;
                foreach(string url in urls)
                {
                    count++;
                    urlList.Add(url);
                    if(count == 10)
                    {
                        Console.WriteLine(url);
                        LocobuzzResponse result = new(false, null, null);
                        Thread thread = new Thread(() => { result = Logic1Bulk(urls).Result; });
                        thread.Start();
                        if (result != null && result.Success == false)
                        {
                            response = new(true, "Reviews fetched.", (List<Review>)result.Data);
                        }
                        else
                        {
                            response = new(false, null, null);
                        }
                        urlList.Clear();
                        count = 0;
                    }
                }
                foreach (string url in urls)
                {
                    Console.WriteLine(url);
                    LocobuzzResponse result = new(false, null, null);
                    Thread thread = new Thread(() => { result = Logic1Bulk(urls).Result; });
                    thread.Start();
                    if (result != null && result.Success == false)
                    {
                        response = new(true, "Reviews fetched.", (List<Review>)result.Data);
                    }
                    else
                    {
                        response = new(false, null, null);
                    }
                }
                response = new(true, null, null);
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

            public async Task<LocobuzzResponse> Logic1(string url)
        {
            LocobuzzResponse response;
            try
            {
                Console.WriteLine("Calling AddProfile method for url - " + url);
                LocobuzzResponse result = AddProfile(url).Result;
                //    int jobId = LogicDataShake.AddProfile(config, logger);
                if (result != null && result.Success == true)
                {
                    int jobId = (int)result.Data;
                    Console.WriteLine("Job id fetched: " + jobId);
                    Console.WriteLine("Calling Reviews method for jobid: " + jobId);
                    LocobuzzResponse result1 = Reviews(jobId).Result;
                    Console.WriteLine("logic1");
                    if (result1 != null && result1.Success == true)
                    {
                        Review[] reviews = (Review[])result1.Data;
                        string OutputPath = "D:\\locobuzz\\DataShakeApiLocobuzz\\Reviews.txt";
                        using (StreamWriter tw = new StreamWriter(OutputPath))
                        {
                            foreach (var item in reviews)
                            {
                                string item1 = JsonConvert.SerializeObject(item);
                                tw.WriteLine(item1);
                            }
                        }
                        response = new(true, "Reviews", reviews);
                    }
                    else
                    {
                        Console.WriteLine("Error Occured " + (string)result.Message);
                        response = new(false, null, null);
                    }
                }
                else
                {
                    Console.WriteLine("Error Occured " + (string)result.Message);
                    response = new(false, null, null);
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

        public async Task<LocobuzzResponse> Logic1Bulk(List<string> urls)
        {
            LocobuzzResponse response;
            try
            {
                BulkUrl url = new BulkUrl();
                List<BulkUrl> bulkUrl = new List<BulkUrl>();
                bulkUrl.Add(url);
                foreach(string item in urls)
                {
                    BulkUrl url1 = new BulkUrl();
                    url1.Url = item;
                    bulkUrl.Add(url1);
                }
                //     Console.WriteLine("Calling AddProfile method for url - " + url);
                List<Review[]> reviewsArray = new List<Review[]>();
                bool check = true;
                LocobuzzResponse result = AddProfileBulk(bulkUrl).Result;
                //    int jobId = LogicDataShake.AddProfile(config, logger);
                if (result != null && result.Success == true)
                {
                    List<int> job_Ids = (List<int>)result.Data;
           //         int jobId = (int)result.Data;
                    foreach(int jobId in job_Ids)
                    {
                        Console.WriteLine("Job id fetched: " + jobId);
                        Console.WriteLine("Calling Reviews method for jobid: " + jobId);
                        LocobuzzResponse result1 = Reviews(jobId).Result;
                        Console.WriteLine("logic1");
                        
                        
                        if (result1 != null && result1.Success == true)
                        {
                            Review[] reviews = (Review[])result1.Data;
                            string OutputPath = "D:\\locobuzz\\DataShakeApiLocobuzz\\Reviews.txt";
                            using (StreamWriter tw = new StreamWriter(OutputPath))
                            {
                                foreach (var item in reviews)
                                {
                                    string item1 = JsonConvert.SerializeObject(item);
                                    tw.WriteLine(item1);
                                }
                            }
                            reviewsArray.Add(reviews);
                        }
                        else
                        {
                            check = false;
                            Console.WriteLine("Error Occured " + (string)result.Message);
                            
                        }
                    }
                    if (!check) response = new(false, null, null);
                    else response = new(true, "All reviews fetched for the current thread.", reviewsArray);
                }
                else
                {
                    Console.WriteLine("Error Occured " + (string)result.Message);
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
                    //    string result = restDataShake.RestAddProfile(config, logger, url).ToString();
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

        public async Task<LocobuzzResponse> AddProfileBulk(List<BulkUrl> url)
        {
            LocobuzzResponse response;
            try
            {
                RestDataShake obj = new RestDataShake();
                //       this.logger.LogInformation("Adding profile for url - " + url);
                LocobuzzResponse result = obj.RestAddProfileBulk(this.config, url).Result;
                List<BulkUrlResponse> bulkresponse = JsonConvert.DeserializeObject<List<BulkUrlResponse>>((string)result.Data);
                Console.WriteLine(result.Success + result.Message + result.Data);
                //       this.logger.LogInformation(result.Success + result.Message + result.Data);
                if (result != null && result.Success == true)
                {
                    var data = (string)result.Data;
                    //    string result = restDataShake.RestAddProfile(config, logger, url).ToString();
                    List<BulkUrlResponse> deserializedResult = JsonConvert.DeserializeObject<List<BulkUrlResponse>>(data);
                    List<int> job_Ids = new List<int>();
                    bool check = true;
                    foreach(var Response in deserializedResult)
                    {
                        if(Response.success.ToString().Trim() == "True")  job_Ids.Add(Response.job_id);
                        else
                        {
                            check = false;
                        }
                    }
                    if(check) response = new(true, "Profiles Added.", job_Ids);
                    else response = new(false, "Profile not added", null);
                }
                else
                {
                    response = new(false, "Profile not added", null);
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

        public async Task<LocobuzzResponse> GetReviews(int jobId)
        {
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                LocobuzzResponse result = obj.RestGetReviews(this.config, jobId).Result;
                string data = (string)result.Data;
                if (result != null && result.Success == true)
                {
                    Root profileInfo = JsonConvert.DeserializeObject<Root>(data);
                    response = new(true, "Reviews fetched.", profileInfo.reviews);
                }
                else
                {
                    response = new(false, null, null);
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
                    //       Root profile = await GetProfile(config, logger, jobId);
                    string status = profile.crawl_status;
                    if (status == "pending")
                    {
                        Console.WriteLine("intervalPending");
                        int intervalPending = config.GetSection("Interval").GetValue<int>("IntervalPending");
                        Thread.Sleep(intervalPending);
                        LocobuzzResponse result1 = CheckStatus(jobId).Result;
                        if (result1 != null && result1.Success == true)
                        {
                            int val = (int)result1.Data;
                            if (val == 0)
                            {
                                response = new(true, "Crawling complete", 0);
                            }
                            else
                            {
                                response = new(false, "Error occured.", null);
                            }
                        }
                        else
                        {
                            response = new(false, "Error occured.", null);
                        }
                    }
                    else
                    {
                        float percentage_complete = (float)profile.percentage_complete;
                        if (percentage_complete == 100.0f)
                        {
                            response = new(true, "Crawling completed", 0); // "Crawling has completed.Now you can fetch reviews.";
                        }
                        else
                        {
                            int IntervalComplete = config.GetSection("Interval").GetValue<int>("IntervalComplete");
                            Console.WriteLine("intervalComplete");
                            Thread.Sleep(IntervalComplete);
                            LocobuzzResponse result1 = CheckStatus(jobId).Result;
                            if (result1 != null && result1.Success == true)
                            {
                                int val = (int)result1.Data;
                                if (val == 0)
                                {
                                    response = new(true, "Crawling complete", 0);
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
                        }
                    }
                }
                else
                {
                    response = new(false, null, null);
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

        public async Task<LocobuzzResponse> Reviews(int jobId)
        {
            LocobuzzResponse response;
            RestDataShake obj = new RestDataShake();
            try
            {
                Console.WriteLine("In Reviews.");
                LocobuzzResponse result = CheckStatus(jobId).Result;
                if (result != null && result.Success == true)
                {
                    if ((int)result.Data == 0)
                    {
                        LocobuzzResponse review = GetReviews(jobId).Result;
                        Review[] reviews = (Review[])review.Data;
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
