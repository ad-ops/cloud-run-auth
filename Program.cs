using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace cloud_run_auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    string host = System.Environment.GetEnvironmentVariable("HOST") ?? "0.0.0.0";
                    string port = System.Environment.GetEnvironmentVariable("PORT") ?? "5000";
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls($"http://{host}:{port}/");
                });
    }
}
