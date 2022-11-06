using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications;
using Notifications.Consumers;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureServices((hostContext, services) =>
         {
             services.AddMassTransit(x =>
             {
                 x.AddConsumer<NotifierTableBookedConsumer>();
                 x.AddConsumer<KitchenReadyConsumer>();

                 x.UsingRabbitMq((context, cfg) =>
                 {
                     cfg.Host("rattlesnake.rmq.cloudamqp.com", 5672, "wqxcpmsj", h =>
                     {
                         h.Username("wqxcpmsj");
                         h.Password("5ycB-owHLnQbJmWGDI1Iq7eAMcuOZile");
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
