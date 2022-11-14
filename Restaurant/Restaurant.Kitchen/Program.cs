using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Consumers;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<KitchenBookingRequestedConsumer>(
                configurator =>
                {
                    // Empty Congiguration
                })
                .Endpoint(e =>
                {
                    e.Temporary = true;
                }); ;

            x.AddConsumer<KitchenBookingRequestFaultConsumer>()
                .Endpoint(e =>
                {
                    e.Temporary = true;
                });
            x.AddDelayedMessageScheduler();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseDelayedMessageScheduler();
                cfg.UseInMemoryOutbox();

                cfg.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddSingleton<Manager>();

        services.AddMassTransitHostedService(true);
    }).Build().Run();