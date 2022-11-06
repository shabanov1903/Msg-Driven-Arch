using Messaging;
using Microsoft.Extensions.DependencyInjection;
using rest = Restaurant.Models;

var serviceCollection = new ServiceCollection()
    .AddSingleton<Producer>()
    .AddSingleton<rest.Messenger>(provider =>
     {
         var producer = provider.GetRequiredService<Producer>();
         return new rest.Messenger(producer);
     })
    .AddSingleton<rest.Restaurant>(provider =>
    {
        var messenger = provider.GetRequiredService<rest.Messenger>();
        ushort max = 10;
        return new rest.Restaurant(max, messenger);
    });

var serviceProvider = serviceCollection.BuildServiceProvider();

var restaurant = serviceProvider.GetService<rest.Restaurant>();

restaurant.StartClearJob();

while(true)
{
    Console.WriteLine("Выберите действите:");
    Console.WriteLine(" 0 - забронировать \n 1 - освободить ");

    if (!int.TryParse(Console.ReadLine(), out var choice) && choice is not (0 or 1))
    {
        Console.WriteLine("Повторите ввод");
        continue;
    }

    switch (choice)
    {
        case 0:
            restaurant.BookFreeTableAsync(1); break;
        case 1:
            {
                Console.WriteLine("Какой стол освободить?");
                if (!int.TryParse(Console.ReadLine(), out var number))
                {
                    Console.WriteLine("Повторите ввод");
                    continue;
                }
                restaurant.MakeFreeTableAsync(number);
            }; break;
    }
}
