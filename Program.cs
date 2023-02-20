using DataShakeApiLocobuzz.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataShakeApiLocobuzz
{
    class Program
    {
        public static IConfiguration config;
        public static readonly ILogger logger;
        static void Main()
        {
      //      Program.setInitialLogging();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
            Program.setInitialConfig();
            LogicDataShake obj = new LogicDataShake(config, logger);
            LocobuzzResponse result = obj.BulkUrl().Result;
            /*
            LocobuzzResponse result = obj.AddProfile(config, logger).Result;
            //    int jobId = LogicDataShake.AddProfile(config, logger);
            if(result != null && result.Success == true)
            {
                int jobId = (int)result.Data;
                LocobuzzResponse result1 = obj.Reviews(config, logger, jobId).Result;
                if(result1 != null && result1.Success == true)
                {
                    List<Review> reviews = (List<Review>)result1.Data;
                    string OutputPath = "Reviews.txt";
                    using (TextWriter tw = new StreamWriter(OutputPath))
                    {
                        foreach (var item in reviews)
                        {
                            tw.WriteLine(item);
                        }
                    }
                } 
                else
                {
                    Console.WriteLine("Error Occured " + (string)result.Message);
                }                
            }
            else
            {
                Console.WriteLine("Error Occured " + (string)result.Message);
            }
            */
            
        }

        public static void setInitialConfig()
        {
            try
            {
                Console.WriteLine("in setting config");
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile("appsettings.json");
                config = configurationBuilder.Build();
                Console.WriteLine("Configuration Build Successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex + "\n");
                logger.LogError(ex.Message);
            }
        }

        public static async void setInitialLogging()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Information);
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Logging");
            Console.WriteLine("Logging Build successful.");
        }
    }
}