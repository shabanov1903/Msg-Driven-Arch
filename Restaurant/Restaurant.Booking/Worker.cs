using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;

namespace Restaurant.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            _restaurant = restaurant;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Привет! Желаете забронировать столик?");
                var b = Guid.NewGuid();
                var dateTime = DateTime.Now;
                await _bus.Publish(
                    (IBookingRequest)new BookingRequest(b, Guid.NewGuid(), null, dateTime),
                    stoppingToken);

                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}