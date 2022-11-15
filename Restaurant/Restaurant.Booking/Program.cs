using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking;
using Restaurant.Booking.Consumers;
using Restaurant.Messages.InMemoryDb;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<RestaurantBookingRequestConsumer>();

            x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                .InMemoryRepository();

            x.AddDelayedMessageScheduler();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Durable = false;
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
        services.AddSingleton<IInMemoryRepository<BookingRequestModel>, InMemoryRepository<BookingRequestModel>>();

        services.AddHostedService<Worker>();
    }).Build().Run();