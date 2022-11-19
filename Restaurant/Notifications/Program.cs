using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications;
using Restaurant.Notification.Consumers;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             services.AddMassTransit(x =>
             {
                 x.AddConsumer<NotifyConsumer>();

                 x.UsingRabbitMq((context, cfg) =>
                 {
                     cfg.Host("localhost", h =>
                     {
                         h.Username("guest");
                         h.Password("guest");
                     });

                     cfg.UseMessageRetry(r =>
                     {
                         r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(100), TimeSpan.FromSeconds(5));
                         r.Ignore<StackOverflowException>();
                         r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
                     });

                     cfg.ConfigureEndpoints(context);
                 });

             });
             services.AddSingleton<Notifier>();
             services.AddMassTransitHostedService(true);
         });

CreateHostBuilder(args).Build().Run();
