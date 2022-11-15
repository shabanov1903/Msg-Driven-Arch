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
//                    configurator.UseScheduledRedelivery(r =>
//                    {
//                        r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20),
//                            TimeSpan.FromSeconds(30));
//                    });
//                    configurator.UseMessageRetry(
//                        r =>
//                        {
//                            r.Incremental(3, TimeSpan.FromSeconds(1),
//                                TimeSpan.FromSeconds(2));
//                        }
//                    );
                });

            x.AddConsumer<KitchenBookingRequestFaultConsumer>();
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