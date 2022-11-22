using System.Text;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;

namespace Restaurant.Booking;

public class Worker : BackgroundService
{
    private readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.OutputEncoding = Encoding.UTF8;
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            Console.WriteLine("Привет! Желаете забронировать столик?");
            var b = Guid.NewGuid();

            var dateTime = DateTime.Now;

            await _bus.Publish(new BookingRequest(b, Guid.NewGuid(), null, dateTime),
                stoppingToken);
        }
    }
}
