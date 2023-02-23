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