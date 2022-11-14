﻿using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking;
using Restaurant.Booking.Consumers;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RestaurantBookingRequestConsumer>(configurator =>
                {
                    configurator.UseScheduledRedelivery(r =>
                    {
                        r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20),
                            TimeSpan.FromSeconds(30));
                    });
                    configurator.UseMessageRetry(
                        r =>
                        {
                            r.Incremental(3, TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(2));
                        }
                    );
                })
                .Endpoint(e =>
                {
                    e.Temporary = true;
                });

            x.AddConsumer<BookingRequestFaultConsumer>()
                .Endpoint(e =>
                {
                    e.Temporary = true;
                });

            x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                .Endpoint(e => e.Temporary = true)
                .InMemoryRepository();

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
        services.AddMassTransitHostedService(true);

        services.AddTransient<RestaurantBooking>();
        services.AddTransient<RestaurantBookingSaga>();
        services.AddTransient<Restaurant.Booking.Restaurant>();

        services.AddHostedService<Worker>();
    }).Build().Run();