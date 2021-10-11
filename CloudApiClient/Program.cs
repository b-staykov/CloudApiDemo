using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int sendRequests = 50;

            try
            {
                string configFile = "appsettings.json";

                if (args.Length == 1)
                {
                    configFile = args[0];
                }

                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                    .Build();

                var uri = config.GetSection("EndPointUri").Value;

                if(string.IsNullOrEmpty(uri))
                {
                    throw new Exception("Configurartion section for Api URL not found.");
                }                               
                
                var client = new RestClient<TranslateRequest>(uri);

                var rnd = new Random();

                Console.WriteLine($"Sending {sendRequests} requests with random numbers...");

                for(var i = 0; i < sendRequests; i++)
                {
                    await Task.Run(() => client.Publish(new TranslateRequest() { NumberOfTasks = rnd.Next(1, 10000) }));
                }

                await Task.Delay(20000);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }

            Console.WriteLine($"\nAll tasks completed.");

            Console.WriteLine($"\nPress any key to continue...");

            Console.ReadKey();
        }       
    }
}
