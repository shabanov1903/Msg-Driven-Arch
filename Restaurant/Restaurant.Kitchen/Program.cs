using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Consumers;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<KitchenTableBookedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rattlesnake.rmq.cloudamqp.com", 5672, "wqxcpmsj", h =>
                {
                    h.Username("wqxcpmsj");
                    h.Password("5ycB-owHLnQbJmWGDI1Iq7eAMcuOZile");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddSingleton<Manager>();

        services.AddMassTransitHostedService(true);
    }).Build().Run();