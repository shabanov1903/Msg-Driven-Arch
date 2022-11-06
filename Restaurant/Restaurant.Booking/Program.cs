using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
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
        services.AddMassTransitHostedService(true);

        services.AddTransient<Restaurant.Booking.Restaurant>();

        services.AddHostedService<Worker>();
    }).Build().Run();