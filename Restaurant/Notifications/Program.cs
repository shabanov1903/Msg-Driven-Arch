using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             services.AddHostedService<Worker>();
         });

CreateHostBuilder(args).Build().Run();
